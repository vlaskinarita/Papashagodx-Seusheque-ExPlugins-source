using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.AutoFlaskEx;

public class AutoFlaskEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, ITickEvents, IUrlProvider, IStartStopEvents
{
	private static readonly Interval interval_0;

	private static bool bool_0;

	private static readonly Func<Rarity, int, bool> func_0;

	private static readonly Interval interval_1;

	private static readonly Interval interval_2;

	private static readonly Interval interval_3;

	private static readonly Interval interval_4;

	private static readonly Interval interval_5;

	private static readonly Stopwatch stopwatch_0;

	private static readonly DataCache dataCache_0;

	private static FlaskInfo flaskInfo_0;

	private Gui gui_0;

	public string Name => "AutoFlaskEx";

	public string Description => "Plugin that provides flask usage functionality.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public JsonSettings Settings => (JsonSettings)(object)global::ExPlugins.AutoFlaskEx.Settings.Instance;

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public string Url => "https://discord.gg/HeqYtkujWW";

	public MessageResult Message(Message message)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "ingame_bot_start_event"))
		{
			if (message.Id == "area_changed_event")
			{
				SetFlaskInfo();
				SetFlaskHook();
				return (MessageResult)0;
			}
			return (MessageResult)1;
		}
		SetFlaskInfo();
		SetFlaskHook();
		bool_0 = PluginManager.EnabledPlugins.Any((IPlugin x) => ((IAuthored)x).Name == "EquipPluginPro");
		return (MessageResult)0;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[AutoFlaskEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
	}

	public void Stop()
	{
	}

	public void Tick()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Invalid comparison between Unknown and I4
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[AutoFlaskEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!LokiPoe.IsInGame || !World.CurrentArea.IsCombatArea || ((Actor)LokiPoe.Me).IsDead)
		{
			return;
		}
		if (bool_0 && interval_0.Elapsed)
		{
			SetFlaskInfo();
		}
		dataCache_0.Clear();
		Settings instance = global::ExPlugins.AutoFlaskEx.Settings.Instance;
		if (interval_1.Elapsed)
		{
			if (flaskInfo_0.HasAntiFreeze && instance.RemoveFreeze && dataCache_0.HasAura("frozen"))
			{
				Item kiaraDetermination = Flasks.KiaraDetermination;
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are frozen");
					return;
				}
				kiaraDetermination = Flasks.ByStat((StatTypeGGG)2635);
				if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
				{
					kiaraDetermination = Flasks.ByStat((StatTypeGGG)14808);
				}
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are frozen");
					return;
				}
			}
			if (flaskInfo_0.HasAntiCurse && instance.RemoveSilence && dataCache_0.HasAura("curse_silence"))
			{
				Item kiaraDetermination = Flasks.KiaraDetermination;
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are cursed with Silence");
					return;
				}
				kiaraDetermination = Flasks.ByStat((StatTypeGGG)1157);
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are cursed with Silence");
					return;
				}
			}
			if (flaskInfo_0.HasAntiBleed)
			{
				if (instance.RemoveBleed && dataCache_0.HasAura("bleeding_moving"))
				{
					Item kiaraDetermination = Flasks.ByStat((StatTypeGGG)2637);
					if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
					{
						kiaraDetermination = Flasks.ByStat((StatTypeGGG)14810);
					}
					if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
					{
						Use(kiaraDetermination, "we are bleeding during movement");
						return;
					}
				}
				if (instance.RemoveCblood)
				{
					int myCbloodStacks = Helpers.MyCbloodStacks;
					if (myCbloodStacks >= instance.MinCbloodStacks)
					{
						Item kiaraDetermination = Flasks.ByStat((StatTypeGGG)2637);
						if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
						{
							kiaraDetermination = Flasks.ByStat((StatTypeGGG)14810);
						}
						if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
						{
							Use(kiaraDetermination, $"we have {myCbloodStacks} Corrupted Blood stacks");
							return;
						}
					}
				}
			}
			if (flaskInfo_0.HasAntiPoison && instance.RemovePoison)
			{
				int poisonStacks = dataCache_0.PoisonStacks;
				if (poisonStacks >= instance.MinPoisonStacks)
				{
					Item kiaraDetermination = Flasks.ByStat((StatTypeGGG)3946);
					if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
					{
						kiaraDetermination = Flasks.ByStat((StatTypeGGG)14811);
					}
					if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
					{
						Use(kiaraDetermination, $"we have {poisonStacks} Poison stacks");
						return;
					}
				}
			}
			if (flaskInfo_0.HasAntiShock && instance.RemoveShock && dataCache_0.HasAura("shocked"))
			{
				Item kiaraDetermination = Flasks.ByStat((StatTypeGGG)2636);
				if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
				{
					kiaraDetermination = Flasks.ByStat((StatTypeGGG)14812);
				}
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are shocked");
					return;
				}
			}
			if (flaskInfo_0.HasAntiIgnite && instance.RemoveIgnite && dataCache_0.HasAura("ignited"))
			{
				Item kiaraDetermination = Flasks.ByStat((StatTypeGGG)2634);
				if ((RemoteMemoryObject)(object)kiaraDetermination == (RemoteMemoryObject)null)
				{
					kiaraDetermination = Flasks.ByStat((StatTypeGGG)14809);
				}
				if ((RemoteMemoryObject)(object)kiaraDetermination != (RemoteMemoryObject)null)
				{
					Use(kiaraDetermination, "we are ignited");
					return;
				}
			}
		}
		if (flaskInfo_0.HasLifeFlask && interval_2.Elapsed)
		{
			int num = (int)((Actor)LokiPoe.Me).HealthPercent;
			if (num < instance.HpPercent && !dataCache_0.HasAura("flask_effect_life") && stopwatch_0.ElapsedMilliseconds > 500L)
			{
				Item lifeFlask;
				if (!((RemoteMemoryObject)(object)(lifeFlask = Flasks.LifeFlask) != (RemoteMemoryObject)null))
				{
					if ((RemoteMemoryObject)(object)(lifeFlask = Flasks.HybridFlask) != (RemoteMemoryObject)null)
					{
						Use(lifeFlask, $"we are at {num}% HP");
						stopwatch_0.Restart();
					}
				}
				else
				{
					Use(lifeFlask, $"we are at {num}% HP");
					stopwatch_0.Restart();
				}
			}
			if (num < instance.HpPercentInstant)
			{
				Item lifeFlask;
				if ((RemoteMemoryObject)(object)(lifeFlask = Flasks.InstantLifeFlask) != (RemoteMemoryObject)null)
				{
					Use(lifeFlask, $"instant we are at {num}% HP");
				}
				else if ((RemoteMemoryObject)(object)(lifeFlask = Flasks.InstantHybridFlask) != (RemoteMemoryObject)null)
				{
					Use(lifeFlask, $"instant we are at {num}% HP");
				}
			}
		}
		if (flaskInfo_0.HasManaFlask && interval_3.Elapsed)
		{
			int num2 = (int)((Actor)LokiPoe.Me).ManaPercent;
			if (num2 < instance.MpPercent)
			{
				bool flag = false;
				if (!dataCache_0.HasAura("flask_effect_mana"))
				{
					Item manaFlask;
					if ((RemoteMemoryObject)(object)(manaFlask = Flasks.ManaFlask) != (RemoteMemoryObject)null)
					{
						Use(manaFlask, $"we are at {num2}% MP");
						flag = true;
					}
					else if ((RemoteMemoryObject)(object)(manaFlask = Flasks.HybridFlask) != (RemoteMemoryObject)null)
					{
						Use(manaFlask, $"we are at {num2}% MP");
						flag = true;
					}
				}
				if (!flag)
				{
					Item manaFlask;
					if ((RemoteMemoryObject)(object)(manaFlask = Flasks.InstantManaFlask) != (RemoteMemoryObject)null)
					{
						Use(manaFlask, $"we are at {num2}% MP");
					}
					else if ((RemoteMemoryObject)(object)(manaFlask = Flasks.InstantHybridFlask) != (RemoteMemoryObject)null && (int)Flasks.InstantHybridFlask.Rarity < 3)
					{
						Use(manaFlask, $"we are at {num2}% MP");
					}
				}
			}
		}
		if (flaskInfo_0.HasQsilverFlask && interval_4.Elapsed && !dataCache_0.HasAura("flask_utility_sprint"))
		{
			int closestDistance;
			if (instance.QsilverRange == 0)
			{
				Item quicksilverFlask = Flasks.QuicksilverFlask;
				if ((RemoteMemoryObject)(object)quicksilverFlask != (RemoteMemoryObject)null)
				{
					Use(quicksilverFlask, "quicksilver flask is in \"fire at will\" mode");
				}
			}
			else if (InstanceInfo.LastActionId == 10505 && Helpers.NoMobsInRange(instance.QsilverRange, out closestDistance))
			{
				Item quicksilverFlask2 = Flasks.QuicksilverFlask;
				if ((RemoteMemoryObject)(object)quicksilverFlask2 != (RemoteMemoryObject)null)
				{
					string reason = ((closestDistance == -1) ? "there are no monsters nearby" : $"closest monster is {closestDistance} away");
					Use(quicksilverFlask2, reason);
				}
			}
		}
		if (!flaskInfo_0.HasTriggerFlask || !interval_5.Elapsed)
		{
			return;
		}
		foreach (FlaskInfo.TriggerFlask triggerFlask in flaskInfo_0.TriggerFlasks)
		{
			if (!dataCache_0.HasAura(triggerFlask.Slot) && Helpers.ShouldTrigger(triggerFlask.Triggers, dataCache_0, out var reason2))
			{
				Item val = Flasks.ByProperName(triggerFlask.Name);
				if ((RemoteMemoryObject)(object)val != (RemoteMemoryObject)null)
				{
					Use(val, reason2);
				}
			}
		}
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		bool flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		bool flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	private static bool FlaskHook(Rarity rarity, int hpPercent)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		foreach (FlaskInfo.TriggerFlask triggerFlask in flaskInfo_0.TriggerFlasks)
		{
			foreach (FlaskTrigger trigger in triggerFlask.Triggers)
			{
				if (trigger.Type == TriggerType.Attack && rarity == trigger.MobRarity && hpPercent >= trigger.MobHpPercent && triggerFlask.PostUseDelay.ElapsedMilliseconds >= 1000L && !dataCache_0.HasAura(triggerFlask.Slot))
				{
					Item val = Flasks.ByProperName(triggerFlask.Name);
					if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null))
					{
						triggerFlask.PostUseDelay.Restart();
						Use(val, $"we are attacking {rarity} monster with {hpPercent}% HP");
						result = true;
					}
				}
			}
		}
		return result;
	}

	private static void Use(Item flask, string reason)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		int num = flask.LocationTopLeft.X + 1;
		GlobalLog.Debug($"[Autoflask] Using {flask.ProperName()} (slot {num}) because {reason}.");
		if (!QuickFlaskHud.UseFlaskInSlot(num))
		{
			GlobalLog.Error($"[Autoflask] UseFlaskInSlot returned false for slot {num}.");
		}
	}

	private static void SetFlaskInfo()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Invalid comparison between Unknown and I4
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Invalid comparison between Unknown and I4
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Invalid comparison between Unknown and I4
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		flaskInfo_0 = new FlaskInfo();
		List<Item> items = QuickFlaskHud.InventoryControl.Inventory.Items;
		foreach (Item item in items)
		{
			foreach (StatTypeGGG key in item.LocalStats.Keys)
			{
				if ((int)key == 2635 || (int)key == 14808)
				{
					flaskInfo_0.HasAntiFreeze = true;
				}
				else if ((int)key != 2634 && (int)key != 14809)
				{
					if ((int)key != 2636 && (int)key != 14812)
					{
						if ((int)key == 3946 || (int)key == 14811)
						{
							flaskInfo_0.HasAntiPoison = true;
						}
						else if ((int)key != 1157)
						{
							if ((int)key == 2637 || (int)key == 14810)
							{
								flaskInfo_0.HasAntiBleed = true;
							}
						}
						else
						{
							flaskInfo_0.HasAntiCurse = true;
						}
					}
					else
					{
						flaskInfo_0.HasAntiShock = true;
					}
				}
				else
				{
					flaskInfo_0.HasAntiIgnite = true;
				}
			}
			if (!(item.Name == "Quicksilver Flask"))
			{
				string @class = item.Class;
				if (!(@class == "LifeFlask"))
				{
					if (@class == "ManaFlask")
					{
						flaskInfo_0.HasManaFlask = true;
						continue;
					}
					string fullName = item.FullName;
					if (!(@class == "HybridFlask") || (int)item.Rarity == 3)
					{
						if (fullName == "Kiara's Determination")
						{
							flaskInfo_0.HasAntiFreeze = true;
							flaskInfo_0.HasAntiCurse = true;
						}
						if (fullName.Contains("Silver Flask"))
						{
							flaskInfo_0.HasSilverFlask = true;
						}
						if (fullName == "Soul Catcher")
						{
							flaskInfo_0.HasSoulCatcherFlask = true;
						}
						if (fullName == "Taste of Hate")
						{
							flaskInfo_0.bool_0 = true;
						}
						if (fullName == "Atziri's Promise")
						{
							flaskInfo_0.HasAtzirisFlask = true;
						}
						if (fullName == "Divination Distillate")
						{
							flaskInfo_0.HasDivinationDistillateFlask = true;
						}
						if (fullName == "Rumi's Concoction")
						{
							flaskInfo_0.HasRumiConcoctionFlask = true;
						}
						string text = item.ProperName();
						List<FlaskTrigger> flaskTriggers = global::ExPlugins.AutoFlaskEx.Settings.Instance.GetFlaskTriggers(text);
						if (flaskTriggers == null)
						{
							GlobalLog.Warn("[Autoflask] \"" + text + "\" is unknown and will not be used.");
						}
						else if (flaskTriggers.Count != 0)
						{
							flaskInfo_0.AddTriggerFlask(item.LocationTopLeft.X, text, flaskTriggers);
							flaskInfo_0.HasTriggerFlask = true;
						}
					}
					else
					{
						flaskInfo_0.HasLifeFlask = true;
						flaskInfo_0.HasManaFlask = true;
					}
				}
				else
				{
					flaskInfo_0.HasLifeFlask = true;
				}
			}
			else
			{
				flaskInfo_0.HasQsilverFlask = true;
			}
		}
		flaskInfo_0.Log();
	}

	private void SetFlaskHook()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		if (flaskInfo_0.TriggerFlasks.Exists((FlaskInfo.TriggerFlask f) => f.Triggers.Exists((FlaskTrigger t) => t.Type == TriggerType.Attack)))
		{
			Message val = new Message("SetFlaskHook", (object)this, new object[1] { func_0 });
			IRoutine current = RoutineManager.Current;
			if ((int)((IMessageHandler)current).Message(val) > 0)
			{
				GlobalLog.Error("[Autoflask] Cannot use \"Before attacking\" triggers because " + ((IAuthored)current).Name + " does not support flask hook.");
				GlobalLog.Error("[Autoflask] Please use routine from latest bot version or remove all \"Before attacking\" triggers.");
				BotManager.Stop(new StopReasonData("flask_hook_error", "Cannot use \"Before attacking\" triggers because " + ((IAuthored)current).Name + " does not support flask hook.", (object)null), false);
			}
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	static AutoFlaskEx()
	{
		interval_0 = new Interval(3000);
		func_0 = FlaskHook;
		interval_1 = new Interval(100);
		interval_2 = new Interval(150);
		interval_3 = new Interval(400);
		interval_4 = new Interval(350);
		interval_5 = new Interval(50);
		stopwatch_0 = Stopwatch.StartNew();
		dataCache_0 = new DataCache();
		flaskInfo_0 = new FlaskInfo();
	}
}
