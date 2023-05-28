using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.MapBotEx;
using ExPlugins.SqRoutine;

namespace ExPlugins.EXtensions.CommonTasks;

public class CastAuraTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly HashSet<string> hashSet_0;

	private bool bool_0;

	private bool bool_1;

	private bool bool_2;

	private Stopwatch stopwatch_0;

	private int int_0;

	private static IEnumerable<Skill> AllAuras => Enumerable.Where(SkillBarHud.Skills, (Skill s) => (hashSet_0.Contains(s.Name) || s.IsAurifiedCurse) && !s.Stats.ContainsKey((StatTypeGGG)9137)).OrderByDescending(GetReservationPct);

	public string Name => "CastAuraTask";

	public string Description => "Task for casting auras before entering a map.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.IsTown)
		{
			if (((Actor)LokiPoe.Me).IsDead)
			{
				return false;
			}
			if (!SkillsUi.IsOpened && !AtlasSkillsUi.IsOpened)
			{
				if (area.IsMyHideoutArea || area.IsMap || !(((IAuthored)BotManager.Current).Name == "MapBotEx"))
				{
					if (!area.IsMyHideoutArea && !area.IsOverworldArea && ((IAuthored)BotManager.Current).Name == "QuestBotEx")
					{
						return false;
					}
					if (area.IsMyHideoutArea || (area.IsOverworldArea && !(area.Name == "Aspirants' Plaza")) || !(((IAuthored)BotManager.Current).Name == "LabRunner"))
					{
						if (!bool_1)
						{
							foreach (Skill aura in AllAuras)
							{
								if (!SkillBlacklist.IsBlacklisted(aura))
								{
									GlobalLog.Debug($"[CastAuraTask] {aura.Name} reserv: {GetReservationPct(aura)}% blacklisted: {SkillBlacklist.IsBlacklisted(aura)}");
								}
								else
								{
									GlobalLog.Warn($"[CastAuraTask] {aura.Name} reserv: {GetReservationPct(aura)}% blacklisted: {SkillBlacklist.IsBlacklisted(aura)}");
								}
							}
							bool_1 = true;
						}
						Skill skill_0 = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.SkillType.Contains("golem") && s.IsCastable);
						ObservableCollection<GolemEntry> specificGolems = GeneralSettings.Instance.GolemsToSummon;
						int totalGolems = 0;
						if (specificGolems.Any())
						{
							List<Skill> skills = Enumerable.Where(SkillBarHud.Skills, (Skill s) => s.SkillType.Contains("golem") && s.IsCastable).ToList();
							totalGolems = skills.Sum((Skill s) => s.NumberDeployed);
							foreach (GolemEntry golemEntry_0 in specificGolems)
							{
								Skill skill = Enumerable.FirstOrDefault(skills, (Skill s) => s.Name.ContainsIgnorecase(golemEntry_0.Golem));
								if (!((RemoteMemoryObject)(object)skill == (RemoteMemoryObject)null) && skill.NumberDeployed < golemEntry_0.Amount)
								{
									skill_0 = skill;
									break;
								}
							}
						}
						if ((RemoteMemoryObject)(object)skill_0 != (RemoteMemoryObject)null)
						{
							int golemsDeployed = skill_0.NumberDeployed;
							int maxGolems = skill_0.GetStat((StatTypeGGG)2890);
							if (golemsDeployed < maxGolems && totalGolems < maxGolems)
							{
								if (!skill_0.CanUse(false, true, true) && area.IsHideoutArea)
								{
									Skill highestReservAura = Enumerable.FirstOrDefault(AllAuras, (Skill a) => PlayerHasAura(a.Name));
									if (!skill_0.IsOnCooldown)
									{
										if (!((RemoteMemoryObject)(object)highestReservAura == (RemoteMemoryObject)null) || AllAuras.Any())
										{
											if ((RemoteMemoryObject)(object)highestReservAura == (RemoteMemoryObject)null)
											{
												GlobalLog.Debug("[" + Name + "] All auras are cancelled. Waiting for mana regen");
												await Wait.For(() => skill_0.CanUse(false, true, true), "mana regen", 300, 5000, log: true);
												return true;
											}
											GlobalLog.Debug($"[{Name}] Highest reservation aura: {highestReservAura.Name} reserv: {GetReservationPct(highestReservAura)}%");
											if (!highestReservAura.IsOnSkillBar)
											{
												await SetAuraToSlot(highestReservAura, GeneralSettings.Instance.AuraSwapSlot);
											}
											await CastAura(highestReservAura, disable: true);
											await Wait.SleepSafe(2000);
											return true;
										}
										GlobalLog.Error("[" + Name + "] We can't cast golems and don't have aura to disable! Stopping the bot because it cannot continue");
										BotManager.Stop(new StopReasonData("summon_golem_fail", "We can't cast golems and don't have aura to disable! Stopping the bot because it cannot continue", (object)null), false);
										return true;
									}
									await Wait.For(() => !Enumerable.Any(SkillBarHud.Skills, (Skill s) => !s.IsOnCooldown && s.Name.Equals(skill_0.Name)), "golem cooldown", 200, 6000, log: true);
									return true;
								}
								if (skill_0.CanUse(false, true, true))
								{
									if (skill_0.IsOnSkillBar)
									{
										ProcessHookManager.ClearAllKeyStates();
										await Coroutines.CloseBlockingWindows();
										UseResult err = await MethodExtensions.SqUseAt(pos: new Vector2i(LokiPoe.MyPosition.X + LokiPoe.Random.Next(-10, 10), LokiPoe.MyPosition.Y + LokiPoe.Random.Next(-10, 10)), skill: skill_0);
										if ((int)err > 0)
										{
											GlobalLog.Error($"[{Name}] {skill_0.Name} error: {err}");
										}
										return true;
									}
									await SetAuraToSlot(skill_0, GeneralSettings.Instance.AuraSwapSlot);
									return true;
								}
							}
						}
						Skill animateGuard = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.InternalId == "animate_armour" && s.IsCastable);
						if ((RemoteMemoryObject)(object)animateGuard != (RemoteMemoryObject)null && LokiPoe.Me.IsInHideout && !bool_0)
						{
							Monster monster_0 = default(Monster);
							ref Monster reference = ref monster_0;
							NetworkObject obj = animateGuard.DeployedObjects.FirstOrDefault();
							reference = (Monster)(object)((obj is Monster) ? obj : null);
							if ((NetworkObject)(object)monster_0 == (NetworkObject)null)
							{
								if (!animateGuard.IsOnSkillBar)
								{
									await SetAuraToSlot(animateGuard, GeneralSettings.Instance.AuraSwapSlot);
								}
								await Coroutines.FinishCurrentAction(true);
								ProcessHookManager.ClearAllKeyStates();
								GlobalLog.Debug("[CastAuraTask] Now summoning \"" + animateGuard.Name + "\".");
								await Coroutines.CloseBlockingWindows();
								await MethodExtensions.SqUse(animateGuard);
								await Wait.SleepSafe(100);
								await Coroutines.FinishCurrentAction(true);
								ProcessHookManager.ClearAllKeyStates();
								await Wait.SleepSafe(100);
								bool_0 = true;
							}
							if (!bool_2 && ((Actor)monster_0).HealthPercent < 60f)
							{
								await Wait.For(() => ((Actor)monster_0).HealthPercent > 55f, $"Animated Guard to regen HP ({((Actor)monster_0).HealthPercent:0.00}%)", 500, 35000);
								bool_2 = true;
								return true;
							}
						}
						List<Skill> auras = GetAurasForCast();
						if (GetAurasForCast().Count > 0)
						{
							GlobalLog.Info($"[CastAuraTask] Found {auras.Count} aura(s) for casting.");
							await Coroutines.CloseBlockingWindows();
							ProcessHookManager.ClearAllKeyStates();
							await CastAuras(auras);
							return true;
						}
						while (true)
						{
							if (!((Actor)LokiPoe.Me).IsDead)
							{
								if (int_0 < 10)
								{
									Stopwatch stopwatch = stopwatch_0;
									if (stopwatch == null || stopwatch.ElapsedMilliseconds >= 60000L || area.IsHideoutArea)
									{
										Skill zombie2 = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == "Raise Zombie" && s.IsCastable);
										Skill desecrate = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == "Desecrate" && s.IsCastable);
										if (!((RemoteMemoryObject)(object)zombie2 == (RemoteMemoryObject)null) && zombie2.CanUse(false, true, true) && zombie2.IsCastable)
										{
											int summoned = zombie2.NumberDeployed;
											if (zombie2.NumberDeployed != zombie2.GetStat((StatTypeGGG)725))
											{
												Monster bestDeadTarget2 = (from m in Enumerable.Where(ObjectManager.GetObjectsByType<Monster>(), (Monster m) => ((NetworkObject)m).Distance < 45f && m.IsActiveDead && (int)m.Rarity != 3 && m.CorpseUsable)
													orderby ExilePather.CanObjectSee((NetworkObject)(object)m, (NetworkObject)(object)LokiPoe.Me, false, false), ((NetworkObject)m).Distance
													select m).FirstOrDefault();
												if ((NetworkObject)(object)bestDeadTarget2 != (NetworkObject)null && zombie2.NumberDeployed < zombie2.GetStat((StatTypeGGG)725))
												{
													zombie2 = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == "Raise Zombie" && s.IsCastable);
													if ((RemoteMemoryObject)(object)zombie2 == (RemoteMemoryObject)null)
													{
														GlobalLog.Error("Zombie is null! very weird");
														ErrorManager.ReportError();
														return true;
													}
													if (!zombie2.IsOnSkillBar)
													{
														await SetAuraToSlot(zombie2, GeneralSettings.Instance.AuraSwapSlot);
													}
													await Coroutines.FinishCurrentAction(true);
													await Coroutines.CloseBlockingWindows();
													await MethodExtensions.SqUseAt(zombie2, ((NetworkObject)bestDeadTarget2).Position);
													if (summoned == zombie2.NumberDeployed)
													{
														int_0++;
													}
													if (zombie2.NumberDeployed < zombie2.GetStat((StatTypeGGG)725))
													{
														continue;
													}
												}
												bestDeadTarget2 = (from m in Enumerable.Where(ObjectManager.GetObjectsByType<Monster>(), (Monster m) => ((NetworkObject)m).Distance < 45f && m.IsActiveDead && (int)m.Rarity != 3 && m.CorpseUsable)
													orderby ExilePather.CanObjectSee((NetworkObject)(object)m, (NetworkObject)(object)LokiPoe.Me, false, false), ((NetworkObject)m).Distance
													select m).FirstOrDefault();
												if ((NetworkObject)(object)bestDeadTarget2 == (NetworkObject)null && (RemoteMemoryObject)(object)desecrate != (RemoteMemoryObject)null && desecrate.IsOnSkillBar)
												{
													Vector2i randomLocation = ((NetworkObject)LokiPoe.Me).Position + new Vector2i(LokiPoe.Random.Next(-20, 20), LokiPoe.Random.Next(-20, 20));
													await Coroutines.FinishCurrentAction(true);
													ProcessHookManager.ClearAllKeyStates();
													await MethodExtensions.SqUseAt(desecrate, randomLocation);
													await Wait.SleepSafe(100);
													await Coroutines.FinishCurrentAction(true);
													ProcessHookManager.ClearAllKeyStates();
													await Wait.SleepSafe(100);
													continue;
												}
												if ((RemoteMemoryObject)(object)desecrate != (RemoteMemoryObject)null && !desecrate.IsOnSkillBar)
												{
													await SetAuraToSlot(desecrate, GeneralSettings.Instance.AuraSwapSlot);
												}
												break;
											}
											int_0 = 0;
											string RufmymhqnO = GeneralSettings.Instance.ReplaceAuraSkillName;
											Skill hiddenSkill2 = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == RufmymhqnO);
											if (string.IsNullOrEmpty(RufmymhqnO))
											{
												return false;
											}
											if ((RemoteMemoryObject)(object)hiddenSkill2 == (RemoteMemoryObject)null || hiddenSkill2.IsOnSkillBar)
											{
												return false;
											}
											await SetAuraToSlot(hiddenSkill2, GeneralSettings.Instance.AuraSwapSlot);
											return false;
										}
										if (string.IsNullOrEmpty(GeneralSettings.Instance.ReplaceAuraSkillName))
										{
											return false;
										}
										Skill hiddenSkill = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == GeneralSettings.Instance.ReplaceAuraSkillName && s.IsCastable);
										if ((RemoteMemoryObject)(object)hiddenSkill == (RemoteMemoryObject)null || hiddenSkill.IsOnSkillBar)
										{
											return false;
										}
										await SetAuraToSlot(hiddenSkill, GeneralSettings.Instance.AuraSwapSlot);
										return false;
									}
									return false;
								}
								int_0 = 0;
								if (stopwatch_0 != null)
								{
									stopwatch_0.Restart();
								}
								else
								{
									stopwatch_0 = Stopwatch.StartNew();
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "area_changed_event")
		{
			int_0 = 0;
			bool_0 = false;
			bool_2 = false;
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static async Task CastAuras(IEnumerable<Skill> auras)
	{
		foreach (Skill aura in auras)
		{
			if (!SkillBlacklist.IsBlacklisted(aura))
			{
				if (aura.Slot == -1)
				{
					await SetAuraToSlot(aura, GeneralSettings.Instance.AuraSwapSlot);
				}
				await CastAura(aura);
			}
		}
	}

	private static async Task CastAura(Skill aura, bool disable = false)
	{
		string name = aura.Name;
		if (!disable)
		{
			if (PlayerHasAura(name))
			{
				return;
			}
		}
		else if (!PlayerHasAura(name))
		{
			return;
		}
		GlobalLog.Debug("[CastAuraTask] Now casting \"" + name + "\".");
		ProcessHookManager.ClearAllKeyStates();
		UseResult used = await MethodExtensions.SqUse(aura);
		if ((int)used > 0)
		{
			GlobalLog.Error($"[CastAuraTask] Fail to cast \"{name}\". Error: \"{used}\".");
		}
		else
		{
			await Wait.LatencySleep();
		}
	}

	private static async Task SetAuraToSlot(Skill aura, int slot)
	{
		string string_0 = aura.Name;
		GlobalLog.Debug($"[CastAuraTask] Now setting \"{string_0}\" to slot {slot}.");
		SetSlotResult isSet = SkillBarHud.SetSlot(slot, aura);
		if ((int)isSet > 0)
		{
			GlobalLog.Error($"[CastAuraTask] Fail to set \"{string_0}\" to slot {slot}. Error: \"{isSet}\".");
			return;
		}
		await Wait.For(() => IsInSlot(slot, string_0), "aura slot changing", 25);
	}

	private static bool IsInSlot(int slot, string name)
	{
		Skill val = SkillBarHud.Slot(slot);
		return (RemoteMemoryObject)(object)val != (RemoteMemoryObject)null && val.Name == name;
	}

	private static List<Skill> GetAurasForCast()
	{
		return Enumerable.Where(AllAuras, (Skill aura) => !SkillBlacklist.IsBlacklisted(aura.Name) && !PlayerHasAura(aura.Name)).ToList();
	}

	private static double GetReservationPct(Skill aura)
	{
		aura.Stats.TryGetValue((StatTypeGGG)13979, out var value);
		aura.Stats.TryGetValue((StatTypeGGG)11781, out var value2);
		if (value != 0)
		{
			return (double)value / 100.0;
		}
		return Math.Round((double)value2 / (double)((Actor)LokiPoe.Me).MaxMana * 100.0, 2);
	}

	private static bool PlayerHasAura(string auraName)
	{
		switch (auraName)
		{
		case "Summon Skitterbots":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.InternalName.Equals("skitterbots_buff")))
			{
				break;
			}
			goto IL_02ec;
		case "Aspect of the Avian":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("avian aspect")))
			{
				break;
			}
			goto IL_02ec;
		case "Pride":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.InternalName.Equals("player_physical_damage_aura_self")))
			{
				break;
			}
			goto IL_02ec;
		case "Flesh and Stone":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("flesh") || a.Name.ContainsIgnorecase("stone")))
			{
				break;
			}
			goto IL_02ec;
		case "Blood and Sand":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("stance")))
			{
				break;
			}
			goto IL_02ec;
		case "Aspect of the Spider":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("spider aspect")))
			{
				break;
			}
			goto IL_02ec;
		case "Aspect of the Cat":
			if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("cat aspect")))
			{
				break;
			}
			goto IL_02ec;
		case "Aspect of the Crab":
			{
				if (!Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.ContainsIgnorecase("crab aspect")))
				{
					break;
				}
				goto IL_02ec;
			}
			IL_02ec:
			return true;
		}
		return Enumerable.Any(((Actor)LokiPoe.Me).Auras, (Aura a) => a.Name.EqualsIgnorecase(auraName) || a.Name.EqualsIgnorecase(auraName + " aura"));
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static CastAuraTask()
	{
		hashSet_0 = new HashSet<string>
		{
			"Anger", "Zealotry", "Clarity", "Determination", "Discipline", "Grace", "Haste", "Hatred", "Purity of Elements", "Purity of Fire",
			"Purity of Ice", "Purity of Lightning", "Vitality", "Wrath", "Precision", "Spellslinger", "Malevolence", "Envy", "Herald of Ash", "Herald of Ice",
			"Herald of Thunder", "Herald of Purity", "Herald of Agony", "Aspect of the Avian", "Aspect of the Cat", "Aspect of the Crab", "Aspect of the Spider", "Arctic Armour", "Tempest Shield", "Blood and Sand",
			"Summon Skitterbots", "Flesh and Stone", "Pride", "Dread Banner", "Defiance Banner", "War Banner", "Petrified Blood"
		};
	}
}
