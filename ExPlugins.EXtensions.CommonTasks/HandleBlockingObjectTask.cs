using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions.CommonTasks;

public class HandleBlockingObjectTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static int int_0;

	private static int int_1;

	private static readonly Dictionary<string, Func<NetworkObject>> dictionary_0;

	public string Name => "HandleBlockingObjectTask";

	public string Description => "Task that handles various blocking objects.";

	public string Author => "alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!World.CurrentArea.IsCombatArea)
		{
			return false;
		}
		if (dictionary_0.TryGetValue(World.CurrentArea.Id, out var func_0))
		{
			NetworkObject obj = func_0();
			if (obj != (NetworkObject)null)
			{
				if (AttemptLimitReached(name: obj.Name, id: obj.Id))
				{
					await LeaveArea();
					return true;
				}
				if (!(await PlayerAction.Interact(obj)))
				{
					await Wait.SleepSafe(500);
				}
				else
				{
					await Wait.LatencySleep();
					await Wait.For(() => func_0() == (NetworkObject)null, "object interaction", 200, 2000);
				}
				return true;
			}
		}
		TriggerableBlockage triggerableBlockage_0 = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)IsClosedDoor);
		if ((NetworkObject)(object)triggerableBlockage_0 == (NetworkObject)null)
		{
			return false;
		}
		if (!CombatAreaCache.IsInIncursion)
		{
			if (AttemptLimitReached(((NetworkObject)triggerableBlockage_0).Id, ((NetworkObject)triggerableBlockage_0).Name))
			{
				await LeaveArea();
				return true;
			}
			if (await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0))
			{
				await Wait.LatencySleep();
				await Wait.For(() => !((NetworkObject)triggerableBlockage_0).IsTargetable || triggerableBlockage_0.IsOpened, "door opening", 50, 300);
				return true;
			}
			await Wait.SleepSafe(300);
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "area_changed_event")
		{
			int_0 = 0;
			int_1 = 0;
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static async Task LeaveArea()
	{
		GlobalLog.Error("[HandleBlockingObjectTask] Fail to remove a blocking object. Now requesting a new instance.");
		EXtensions.AbandonCurrentArea();
		if (!(await PlayerAction.TpToTown()))
		{
			ErrorManager.ReportError();
		}
	}

	private static bool AttemptLimitReached(int id, string name)
	{
		if (int_0 != id)
		{
			int_0 = id;
			int_1 = 0;
		}
		else
		{
			int_1++;
			if (int_1 > 25)
			{
				return true;
			}
			if (int_1 >= 2)
			{
				GlobalLog.Error($"[HandleBlockingObjectTask] {int_1}/{25} attempt to interact with \"{name}\" (id: {id})");
			}
		}
		return false;
	}

	private static bool IsClosedDoor(TriggerableBlockage d)
	{
		return ((NetworkObject)d).IsTargetable && !d.IsOpened && ((NetworkObject)d).Distance <= 25f && (((NetworkObject)d).Name == "Door" || ((NetworkObject)d).Metadata == "Metadata/MiscellaneousObjects/Smashable" || ((NetworkObject)d).Metadata.Contains("LabyrinthSmashableDoor"));
	}

	private static NetworkObject PitGate()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 15f && o.Metadata.Contains("PitGateTransition"));
	}

	private static NetworkObject BellyGate()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 3f && o.Metadata.Contains("BellyArenaTransition"));
	}

	private static NetworkObject TreeRoots()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/Inca/PoisonTree");
		if (val != (NetworkObject)null && PlayerHasQuestItem("Metadata/Items/QuestItems/PoisonSpear") && PlayerHasQuestItem("Metadata/Items/QuestItems/PoisonSkillGem"))
		{
			return val;
		}
		return null;
	}

	private static NetworkObject AncientSeal()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 20f && o.Metadata == "Metadata/QuestObjects/Inca/IncaDarknessRelease");
	}

	private static NetworkObject SewerGrating()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/Sewers/SewersLockedDoor");
		if (!(val != (NetworkObject)null) || !PlayerHasQuestItem("Metadata/Items/QuestItems/SewerKeys"))
		{
			return null;
		}
		return val;
	}

	private static NetworkObject UndyingBlockage()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 20f && o.Metadata == "Metadata/QuestObjects/Sewers/BioWall");
		if (!(val != (NetworkObject)null) || !PlayerHasQuestItem("Metadata/Items/QuestItems/InfernalTalc"))
		{
			return null;
		}
		return val;
	}

	private static NetworkObject TowerDoor()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/EpicDoor/EpicDoorLock");
		if (val != (NetworkObject)null && PlayerHasQuestItem("Metadata/Items/QuestItems/TowerKey"))
		{
			return val;
		}
		return null;
	}

	private static NetworkObject OriathSquareDoor()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 50f && o.Metadata == "Metadata/Terrain/Act5/Area2/Objects/SlavePenSecurityDoor");
	}

	private static NetworkObject TemplarCourtsDoor()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 50f && o.Metadata == "Metadata/QuestObjects/Act5/TemplarCourtsDoor");
		if (!(val != (NetworkObject)null) || !PlayerHasQuestItem("Metadata/Items/QuestItems/Act5/TemplarCourtKey"))
		{
			return null;
		}
		return val;
	}

	private static NetworkObject KaruiFortressGate()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/Act6/MudFlatsKaruiDoor");
		if (val != (NetworkObject)null && PlayerHasQuestItem("Metadata/Items/QuestItems/Act6/KaruiEye"))
		{
			return val;
		}
		return null;
	}

	private static NetworkObject LooseGrate()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/Sewers/SewersGrate");
	}

	private static NetworkObject SecretPassage()
	{
		NetworkObject val = ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/QuestObjects/Act7/MaligaroPassageCover");
		if (val != (NetworkObject)null && PlayerHasQuestItem("Metadata/Items/QuestItems/Act7/ObsidianKey"))
		{
			return val;
		}
		return null;
	}

	private static NetworkObject VoltaicWorkshop()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o is AreaTransition && o.IsTargetable && o.Distance <= 15f && o.Name == "Voltaic Workshop");
	}

	private static NetworkObject PlazaLever()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.IsTargetable && o.Distance <= 30f && o.Metadata == "Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch_Once");
	}

	private static bool PlayerHasQuestItem(string metadata)
	{
		return Inventories.InventoryItems.Exists((Item i) => i.Class == "QuestItem" && i.Metadata == metadata);
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Tick()
	{
	}

	public void Stop()
	{
	}

	static HandleBlockingObjectTask()
	{
		dictionary_0 = new Dictionary<string, Func<NetworkObject>>
		{
			[World.Act2.Wetlands.Id] = TreeRoots,
			[World.Act2.VaalRuins.Id] = AncientSeal,
			[World.Act3.Slums.Id] = SewerGrating,
			[World.Act3.Sewers.Id] = UndyingBlockage,
			[World.Act3.ImperialGardens.Id] = TowerDoor,
			[World.Act4.DaressoDream.Id] = PitGate,
			[World.Act4.BellyOfBeast2.Id] = BellyGate,
			[World.Act4.Harvest.Id] = BellyGate,
			[World.Act5.ControlBlocks.Id] = OriathSquareDoor,
			[World.Act5.OriathSquare.Id] = TemplarCourtsDoor,
			[World.Act6.MudFlats.Id] = KaruiFortressGate,
			[World.Act7.ChamberOfSins2.Id] = SecretPassage,
			[World.Act8.DoedreCesspool.Id] = LooseGrate,
			[World.Act9.Refinery.Id] = VoltaicWorkshop,
			[World.Act9.RottingCore.Id] = BellyGate,
			["MapWorldsPit"] = PitGate,
			["MapWorldsMalformation"] = BellyGate,
			["MapWorldsCore"] = BellyGate,
			["MapWorldsTribunal"] = BellyGate,
			["MapWorldsSepulchre"] = BellyGate,
			["MapWorldsOvergrownShrine"] = BellyGate,
			["MapWorldsFactory"] = VoltaicWorkshop,
			["MapWorldsPlaza"] = PlazaLever,
			["MapAtlasPit"] = PitGate,
			["MapAtlasMalformation"] = BellyGate,
			["MapAtlasCore"] = BellyGate,
			["MapAtlasOvergrownShrine"] = BellyGate,
			["MapAtlasFactory"] = VoltaicWorkshop,
			["MapAtlasPlaza"] = PlazaLever
		};
	}
}
