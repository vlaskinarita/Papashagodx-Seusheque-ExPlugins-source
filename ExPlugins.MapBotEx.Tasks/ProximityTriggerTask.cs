using System;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.Tasks;

public class ProximityTriggerTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static string string_0;

	private static CachedObject cachedObject_0;

	private static Func<Task> func_0;

	public string Name => "ProximityTriggerTask";

	public string Description => "Task that comes to certain objects to trigger an event.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (string_0 == null || MapExplorationTask.MapCompleted)
		{
			return false;
		}
		if (!(cachedObject_0 == null) && !cachedObject_0.Ignored && !cachedObject_0.Unwalkable)
		{
			if (World.CurrentArea.IsMap)
			{
				if (!(World.CurrentArea.Name == "Laboratory") || SpecialObjectTask.LabCounter >= 4)
				{
					WalkablePosition pos = cachedObject_0.Position;
					if (pos.Distance > 10 || pos.PathDistance > 12f)
					{
						if (!pos.TryCome())
						{
							GlobalLog.Error($"[ProximityTriggerTask] Fail to move to {pos}. Marking this trigger object as unwalkable.");
							cachedObject_0.Unwalkable = true;
						}
						return true;
					}
					await Coroutines.FinishCurrentAction(true);
					if (func_0 != null)
					{
						await func_0();
					}
					string_0 = null;
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (string_0 == null || cachedObject_0 != null || MapExplorationTask.MapCompleted || !interval_0.Elapsed || !LokiPoe.IsInGame || !World.CurrentArea.IsMap)
		{
			return;
		}
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			if (@object.Metadata == string_0)
			{
				WalkablePosition walkablePosition = @object.WalkablePosition();
				GlobalLog.Warn($"[ProximityTriggerTask] Registering {walkablePosition}");
				cachedObject_0 = new CachedObject(@object.Id, walkablePosition);
				break;
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (!(id == "MB_new_map_entered_event"))
		{
			if (!(id == "explorer_local_transition_entered_event"))
			{
				return (MessageResult)1;
			}
			if (cachedObject_0 != null)
			{
				GlobalLog.Info("[ProximityTriggerTask] Resetting unwalkable flag.");
				cachedObject_0.Unwalkable = false;
			}
			return (MessageResult)0;
		}
		GlobalLog.Info("[ProximityTriggerTask] Reset.");
		Reset(message.GetInput<string>(0));
		if (string_0 != null)
		{
			GlobalLog.Info("[ProximityTriggerTask] Enabled.");
		}
		return (MessageResult)0;
	}

	private static void Reset(string areaName)
	{
		string_0 = null;
		cachedObject_0 = null;
		func_0 = null;
		if (areaName == MapNames.Laboratory)
		{
			string_0 = "Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_Counter";
			func_0 = LaboratoryWait;
		}
		else if (!(areaName == MapNames.Mausoleum))
		{
			if (areaName == MapNames.MaoKun)
			{
				string_0 = "Metadata/Terrain/EndGame/MapTreasureIsland/Objects/FairgravesTreasureIsland";
				func_0 = MaoKunWait;
			}
		}
		else
		{
			string_0 = "Metadata/Terrain/EndGame/MapMausoleum/Objects/AnkhOfEternityMap";
			func_0 = MausoleumWait;
		}
	}

	private static async Task LaboratoryWait()
	{
		await Wait.For(() => ObjectManager.Objects.Any((Monster m) => ((NetworkObject)m).Distance < 70f && m.IsActive), "any active monster", 100, 2000);
	}

	private static async Task MausoleumWait()
	{
		await Wait.For(() => ObjectManager.Objects.Any((Monster m) => ((NetworkObject)m).Distance < 70f && m.IsActive), "any active monster", 500, 10000);
	}

	private static async Task MaoKunWait()
	{
		await Wait.For(delegate
		{
			NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Terrain/EndGame/MapTreasureIsland/Objects/FairgravesTreasureIsland");
			return val != (NetworkObject)null && val.IsTargetable;
		}, "Fairgraves activation", 500, 7000);
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static ProximityTriggerTask()
	{
		interval_0 = new Interval(200);
	}
}
