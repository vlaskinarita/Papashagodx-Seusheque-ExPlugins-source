using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;
using ExPlugins.SqRoutine;

namespace ExPlugins.SimulacrumPluginEx.Tasks;

public class PrepareForWaveTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool Active;

	private static Stopwatch stopwatch_0;

	private static readonly SimulSett Config;

	public string Name => "PrepareForWaveTask";

	public string Description => "Task that prepares for simulacrum wave";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (GeneralSettings.SimulacrumsEnabled)
			{
				DatWorldAreaWrapper area = World.CurrentArea;
				if (area.Id.Contains("Affliction"))
				{
					if ((NetworkObject)(object)StartSimulacrumTask.Afflictionator != (NetworkObject)null)
					{
						StartSimulacrumTask.Afflictionator.HideUi();
					}
					if (Active)
					{
						if (!stopwatch_0.IsRunning)
						{
							stopwatch_0 = Stopwatch.StartNew();
						}
						if (Config.EnableAnchorPoints)
						{
							WalkablePosition point = SimulacrumPluginEx.GetCoords(area.Name);
							if (point != null && point.Distance > 10)
							{
								point.TryCome();
								return true;
							}
						}
						List<Skill> skills = SkillBarHud.Skills.ToList();
						Vector2i vector2i_0 = LokiPoe.MyPosition;
						Skill sigilOfPowerSkill = skills.FirstOrDefault((Skill s) => s.InternalId == "circle_of_power" && s.IsCastable);
						Skill cremationSkill = skills.FirstOrDefault((Skill s) => s.InternalId == "cremation" && s.IsCastable);
						Skill unearthSkill = skills.FirstOrDefault((Skill s) => s.InternalId == "unearth" && s.IsCastable);
						Skill desecrateSkill = skills.FirstOrDefault((Skill s) => s.Name == "Desecrate" && s.IsCastable);
						Skill guardSkill = skills.FirstOrDefault((Skill s) => (!s.SkillType.Contains("vaal") && s.SkillTags.Any((string t) => t.EqualsIgnorecase("guard"))) || (s.InternalId.Equals("bone_armour") && s.IsCastable));
						List<Effect> cremateGeysers = (from e in ObjectManager.GetObjectsByType<Effect>()
							where ((NetworkObject)e).AnimatedPropertiesMetadata.Contains("CorpseEruptionGroundFX")
							select e).ToList();
						List<Monster> usableCorpses = ObjectManager.GetObjectsByType<Monster>().Where(CorpseUsability).OrderBy(delegate(Monster m)
						{
							//IL_0001: Unknown result type (might be due to invalid IL or missing references)
							//IL_0006: Unknown result type (might be due to invalid IL or missing references)
							//IL_000b: Unknown result type (might be due to invalid IL or missing references)
							Vector2i position = ((NetworkObject)m).Position;
							return ((Vector2i)(ref position)).Distance(vector2i_0);
						})
							.ToList();
						int offset = LokiPoe.Random.Next(-35, 35);
						WalkablePosition randPos = new WalkablePosition("rand pos", new Vector2i(vector2i_0.X + offset, vector2i_0.Y + offset), 5)
						{
							Initialized = true
						};
						if (!(stopwatch_0.Elapsed.TotalSeconds > 20.0) && InstanceInfo.MonstersRemaining <= 45 && cremateGeysers.Count < 3)
						{
							if (!((RemoteMemoryObject)(object)sigilOfPowerSkill != (RemoteMemoryObject)null) || !sigilOfPowerSkill.CanUse(false, false, true) || ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("circle_of_power_buff")) || (int)(await MethodExtensions.SqUse(sigilOfPowerSkill)) != 0)
							{
								if ((RemoteMemoryObject)(object)guardSkill != (RemoteMemoryObject)null && guardSkill.CanUse(false, false, true))
								{
									UseResult err2 = await MethodExtensions.SqUse(guardSkill);
									GlobalLog.Debug($"[{Name}] Used: {guardSkill.Name} Errors: {err2}");
									if ((int)err2 == 0)
									{
										return true;
									}
								}
								if ((RemoteMemoryObject)(object)desecrateSkill != (RemoteMemoryObject)null && usableCorpses.Count < 8 && desecrateSkill.CanUse(false, false, true))
								{
									GlobalLog.Debug("[" + Name + "] No corpses nearby. Using " + desecrateSkill.Name);
									if ((int)(await MethodExtensions.SqUseAt(desecrateSkill, randPos)) == 0)
									{
										await Wait.LatencySleep();
									}
									return true;
								}
								if (!((RemoteMemoryObject)(object)cremationSkill == (RemoteMemoryObject)null) && !((RemoteMemoryObject)(object)unearthSkill == (RemoteMemoryObject)null))
								{
									if (cremationSkill.CanUse(false, false, true) && cremateGeysers.Count < 3)
									{
										Monster usableCorpse = usableCorpses.FirstOrDefault();
										if (!((NetworkObject)(object)usableCorpse != (NetworkObject)null))
										{
											if (unearthSkill.CanUse(false, false, true))
											{
												GlobalLog.Debug("[" + Name + "] No corpses nearby for cremation. Using " + unearthSkill.Name);
												if ((int)(await MethodExtensions.SqUseAt(unearthSkill, randPos)) == 0)
												{
													await Wait.LatencySleep();
													await MethodExtensions.SqUseAt(cremationSkill, randPos);
													return true;
												}
											}
										}
										else
										{
											Vector2i corpsePos = ((NetworkObject)usableCorpse).Position;
											UseResult err1 = await MethodExtensions.SqUseAt(cremationSkill, corpsePos);
											GlobalLog.Debug($"[{Name}] Using {cremationSkill.Name} at {corpsePos}");
											if ((int)err1 == 0)
											{
												return true;
											}
										}
									}
									return true;
								}
								Active = false;
								return false;
							}
							return true;
						}
						Active = false;
						stopwatch_0.Reset();
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static bool CorpseUsability(Monster m)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (!((NetworkObject)m).Name.Contains("Totem"))
		{
			if (!m.IsActiveDead)
			{
				return false;
			}
			if (m.CorpseUsable)
			{
				Vector2i position = ((NetworkObject)m).Position;
				return ((Vector2i)(ref position)).Distance(LokiPoe.MyPosition) < 60;
			}
			return false;
		}
		return false;
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

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static PrepareForWaveTask()
	{
		stopwatch_0 = new Stopwatch();
		Config = SimulSett.Instance;
	}
}
