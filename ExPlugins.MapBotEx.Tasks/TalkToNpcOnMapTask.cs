using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.Tasks;

public class TalkToNpcOnMapTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static Npc Envoy => ObjectManager.Objects.FirstOrDefault((Npc a) => ((NetworkObject)a).Name.Equals("The Envoy"));

	private static CachedObject CachedEnvoy
	{
		get
		{
			return CombatAreaCache.Current.Storage["CachedEnvoy"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CachedEnvoy"] = value;
		}
	}

	private static Npc Oshabi => ObjectManager.Objects.FirstOrDefault((Npc a) => ((NetworkObject)a).Name.Equals("Oshabi"));

	private static CachedObject CachedOshabi
	{
		get
		{
			return CombatAreaCache.Current.Storage["CachedOshabi"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CachedOshabi"] = value;
		}
	}

	private static Npc Tane => ObjectManager.Objects.FirstOrDefault((Npc a) => ((NetworkObject)a).Name.Equals("Tane Octavius"));

	private static CachedObject CachedTane
	{
		get
		{
			return CombatAreaCache.Current.Storage["CachedTane"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CachedTane"] = value;
		}
	}

	public string Name => "TalkToNpcOnMapTask";

	public string Description => "Talk to the hand";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsMap)
			{
				return false;
			}
			if (CachedEnvoy != null && (NetworkObject)(object)Envoy != (NetworkObject)null)
			{
				await TalkToNpc(CachedEnvoy);
			}
			if (CachedTane != null && (NetworkObject)(object)Tane != (NetworkObject)null)
			{
				await TalkToNpc(CachedTane);
			}
			if (CachedOshabi != null && (NetworkObject)(object)Oshabi != (NetworkObject)null)
			{
				await TalkToNpc(CachedOshabi);
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (!interval_0.Elapsed || !LokiPoe.IsInGame)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (currentArea.IsMap)
		{
			if (CombatAreaCache.IsInHarvest && CachedOshabi == null && (NetworkObject)(object)Oshabi != (NetworkObject)null)
			{
				CachedOshabi = new CachedObject((NetworkObject)(object)Oshabi);
			}
			if (CachedEnvoy == null && (NetworkObject)(object)Envoy != (NetworkObject)null)
			{
				CachedEnvoy = new CachedObject((NetworkObject)(object)Envoy);
			}
			if (CachedTane == null && (NetworkObject)(object)Tane != (NetworkObject)null)
			{
				CachedTane = new CachedObject((NetworkObject)(object)Tane);
			}
		}
	}

	private async Task TalkToNpc(CachedObject cachedNpc)
	{
		if (!(cachedNpc.Object != (NetworkObject)null) || !cachedNpc.Object.HasNpcFloatingIcon)
		{
			return;
		}
		WalkablePosition npcPos = cachedNpc.Position;
		if (npcPos.IsFar || npcPos.IsFarByPath)
		{
			if (!ExilePather.PathExistsBetween(LokiPoe.MyPosition, (Vector2i)npcPos, false))
			{
				return;
			}
			GlobalLog.Warn($"[{Name}] Moving closer to {npcPos}.");
			if (!npcPos.TryCome())
			{
				GlobalLog.Error($"[{Name}] Fail to move to NPC. {npcPos} is unwalkable.");
				await OpenDoor();
				return;
			}
		}
		await Coroutines.FinishCurrentAction(true);
		GlobalLog.Warn($"[{Name}] {npcPos} has NPC Floationg Icon, going to interact it.");
		if (npcPos.IsNearByPath && await PlayerAction.Interact(cachedNpc.Object))
		{
			await Wait.Sleep(200);
			await Coroutines.CloseBlockingWindows();
		}
		await Wait.Sleep(100);
	}

	private static async Task OpenDoor()
	{
		TriggerableBlockage triggerableBlockage_0 = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)IsClosedDoor);
		if ((NetworkObject)(object)triggerableBlockage_0 == (NetworkObject)null)
		{
			return;
		}
		if (!(await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0)))
		{
			await Wait.SleepSafe(300);
			return;
		}
		await Wait.LatencySleep();
		await Wait.For(() => !((NetworkObject)triggerableBlockage_0).IsTargetable || triggerableBlockage_0.IsOpened, "door opening", 50, 300);
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
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static TalkToNpcOnMapTask()
	{
		interval_0 = new Interval(500);
	}
}
