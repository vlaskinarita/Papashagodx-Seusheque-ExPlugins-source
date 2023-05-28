using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.TangledAltarsEx;
using ExPlugins.TangledAltarsEx.Classes;

internal class Class13 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static int int_0;

	private static int int_1;

	private static object object_0;

	private static readonly List<TangleAltar> list_0;

	private readonly TangledAltarsExSettings tangledAltarsExSettings_0 = TangledAltarsExSettings.Instance;

	private readonly Interval interval_0 = new Interval(1000);

	public JsonSettings Settings => (JsonSettings)(object)TangledAltarsExSettings.Instance;

	public string Name => "CheckAndClickShrine";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame)
		{
			return false;
		}
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.IsMap)
		{
			return false;
		}
		if ((NetworkObject)object_0 == (NetworkObject)null)
		{
			object_0 = ((!list_0.Contains(TangledAltarsEx.TangleAltars.FirstOrDefault())) ? TangledAltarsEx.TangleAltars.FirstOrDefault() : null);
		}
		if ((NetworkObject)object_0 == (NetworkObject)null)
		{
			return false;
		}
		if (!Blight.IsEncounterRunning && !BlightUi.IsOpened)
		{
			WalkablePosition altarPos = new WalkablePosition("AltarPosition", ((NetworkObject)object_0).Position);
			if (altarPos.Distance > 30)
			{
				if (ExilePather.PathExistsBetween(LokiPoe.MyPosition, (Vector2i)altarPos, false))
				{
					if (int_0 > 10)
					{
						GlobalLog.Error($"[CheckAndClickAltar] Fail to move to {altarPos}. Altar is unwalkable.");
						DoOnFinishInteraction();
						return false;
					}
					if (interval_0.Elapsed)
					{
						int_0++;
						if (tangledAltarsExSettings_0.DebugMode)
						{
							GlobalLog.Debug($"[CheckAndClickAltar] Moving closer to the Altar for the {int_0} time.");
						}
					}
					if (!(await altarPos.TryComeAtOnce()))
					{
						GlobalLog.Error($"[CheckAndClickAltar] Fail to move to {altarPos}. Altar is unwalkable.");
						DoOnFinishInteraction();
						await OpenDoor();
						return true;
					}
					return true;
				}
				return false;
			}
			if (interval_0.Elapsed)
			{
				int_0++;
				if (tangledAltarsExSettings_0.DebugMode)
				{
					GlobalLog.Debug($"[CheckAndClickAltar] Moving closer to the Altar for the {int_0} time.");
				}
			}
			if (int_0 > 10)
			{
				GlobalLog.Error($"[CheckAndClickAltar] Fail to move to {altarPos}. Altar is unwalkable.");
				DoOnFinishInteraction();
				return false;
			}
			await Coroutines.FinishCurrentAction(true);
			ProcessHookManager.ClearAllKeyStates();
			List<Option> options = null;
			if ((NetworkObject)object_0 != (NetworkObject)null && ((TangleAltar)object_0).Options != null)
			{
				options = ((TangleAltar)object_0).Options;
			}
			int fattestWeight = -1;
			Option chosenMod = null;
			if (options != null)
			{
				foreach (Option opt in options)
				{
					int weight = ChoseAltarBonus(opt);
					if (weight != -1 && weight > fattestWeight)
					{
						fattestWeight = weight;
						chosenMod = opt;
					}
				}
			}
			if (fattestWeight != -1)
			{
				if (chosenMod != null)
				{
					await Wait.LatencySleep();
					CachedObject cached = new CachedObject((NetworkObject)(object)TangledAltarsEx.TangleAltars.FirstOrDefault());
					await PlayerAction.EnableAlwaysHighlight();
					MouseManager.SetMousePos((string)null, (Vector2i)cached.Position, true);
					chosenMod.Activate();
					int_1++;
					GlobalLog.Debug($"[CheckAndClickAltar] Trying to interact with altar for the {int_1} time.");
				}
				if (int_1 <= 10)
				{
					if (!((NetworkObject)(object)TangledAltarsEx.TangleAltars.FirstOrDefault() != (NetworkObject)null))
					{
						DoOnFinishInteraction();
						return false;
					}
					return true;
				}
				GlobalLog.Error("[CheckAndClickAltar] Failed to interact with altar.");
				DoOnFinishInteraction();
				return false;
			}
			GlobalLog.Error("[CheckAndClickAltar] All altar mods had ignored downsides. Now ignoring the altar.");
			DoOnFinishInteraction();
			return false;
		}
		return false;
	}

	private static void DoOnFinishInteraction()
	{
		list_0.Add((TangleAltar)object_0);
		int_1 = 0;
		object_0 = null;
		int_0 = 0;
	}

	private int ChoseAltarBonus(Option option)
	{
		foreach (string item in option.Text)
		{
			string string_0 = Regex.Replace(item, "[0-9]{1,}", "#");
			if (tangledAltarsExSettings_0.ModsToIgnoreList != null && tangledAltarsExSettings_0.ModsToIgnoreList.Any((TangledAltarsExSettings.NameEntry m) => m.Name.EqualsIgnorecase(string_0)))
			{
				GlobalLog.Debug("[CheckAndClickAltar] Found ignored downside mod " + string_0 + ". Will now ignore this option.");
				return -1;
			}
		}
		string text = option.Text.Last().Replace("–", "");
		text = text.Replace(".", "");
		text = text.Replace("(", "");
		text = text.Replace(")", "");
		text = Regex.Replace(text, "[0-9]{1,}", "#");
		if (!text.Contains("increased Rarity of Items found in this Area"))
		{
			text = text.Split('{')[1];
		}
		text = text.Replace("}", "").Replace(".", "");
		foreach (ShrineMod item2 in tangledAltarsExSettings_0.ModList.Where((ShrineMod x) => x.WhoGains.ToLower() == option.Text[0].ToLower().Split('{')[1].Replace("}", "").Replace(".", "")))
		{
			string text2 = item2.ModBonus.Replace("–", "");
			text2 = text2.Replace(".", "");
			text2 = text2.Replace("(", "");
			text2 = text2.Replace(")", "");
			text2 = Regex.Replace(text2, "[0-9]{1,}", "#");
			if (text2 == text)
			{
				GlobalLog.Debug($"[CheckAndClickAltar] Found needed mod {item2.ModBonus}, weight {item2.Weight}.");
				return item2.Weight;
			}
		}
		GlobalLog.Error("[CheckAndClickAltar] No mod found. Source mod: " + text);
		return 0;
	}

	private static async Task OpenDoor()
	{
		TriggerableBlockage triggerableBlockage_0 = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)IsClosedDoor);
		if ((NetworkObject)(object)triggerableBlockage_0 == (NetworkObject)null)
		{
			return;
		}
		if (await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0))
		{
			await Wait.LatencySleep();
			await Wait.For(() => !((NetworkObject)triggerableBlockage_0).IsTargetable || triggerableBlockage_0.IsOpened, "door opening", 50, 300);
		}
		else
		{
			await Wait.SleepSafe(300);
		}
	}

	public static bool IsClosedDoor(TriggerableBlockage d)
	{
		return ((NetworkObject)d).IsTargetable && !d.IsOpened && ((NetworkObject)d).Distance <= 25f && (((NetworkObject)d).Name == "Door" || ((NetworkObject)d).Metadata == "Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch_Once" || ((NetworkObject)d).Metadata == "Metadata/MiscellaneousObjects/Smashable" || ((NetworkObject)d).Metadata.Contains("LabyrinthSmashableDoor"));
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

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		if (message.Id == "MB_new_map_entered_event")
		{
			list_0.Clear();
			int_1 = 0;
			object_0 = null;
			int_0 = 0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	static Class13()
	{
		list_0 = new List<TangleAltar>();
	}
}
