using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.IncursionEx;

public class HandleIncursionTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class24
	{
		public readonly List<Class49> list_0 = new List<Class49>();

		public readonly List<CachedObject> list_1 = new List<CachedObject>();
	}

	private class Class49 : CachedObject
	{
		[CompilerGenerated]
		private readonly string PtwbbijpJX;

		public string Type
		{
			[CompilerGenerated]
			get
			{
				return PtwbbijpJX;
			}
		}

		public Class49(int id, WalkablePosition pos, string type)
			: base(id, pos)
		{
			PtwbbijpJX = type;
		}
	}

	private static readonly Interval interval_0;

	private static List<NetworkObject> IncursionDoors => ObjectManager.Objects.Where((NetworkObject o) => o.Metadata.Equals("Metadata/Terrain/Leagues/Incursion/Objects/ClosedDoorPast")).ToList();

	private static Class24 CachedIncursionData
	{
		get
		{
			Class24 @class = CombatAreaCache.Current.Storage["IncursionData"] as Class24;
			if (@class == null)
			{
				@class = new Class24();
				CombatAreaCache.Current.Storage["IncursionData"] = @class;
			}
			return @class;
		}
	}

	private static bool HasKey => Inventories.InventoryItems.Any((Item i) => i.Metadata == "Metadata/Items/Incursion/IncursionKey");

	public string Name => "HandleIncursionTask";

	public string Description => "Task that handles incursion areas.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsMap || area.IsOverworldArea)
		{
			CombatAreaCache cache = CombatAreaCache.Current;
			if (!CombatAreaCache.IsInIncursion)
			{
				return false;
			}
			RoomEntry settings = IncursionEx.CurrentRoomSettings;
			if (settings != null)
			{
				if (settings.PriorityAction != PriorityAction.Upgrading)
				{
					if (settings.PriorityAction == PriorityAction.Changing && await KillArchitect("IncursionArchitectReplace"))
					{
						return true;
					}
				}
				else if (await KillArchitect("IncursionArchitectUpgrade"))
				{
					return true;
				}
			}
			if (await TrackMobLogic.Execute())
			{
				return true;
			}
			if (await cache.Explorer.Execute())
			{
				return true;
			}
			if (await OpenIncursionDoor())
			{
				return true;
			}
			if (!(await ExitIncursion()))
			{
				GlobalLog.Debug("[HandleIncursionTask] Incursion is fully explored. Waiting for timer to run out...");
				await Wait.StuckDetectionSleep(200);
				return true;
			}
			return true;
		}
		return false;
	}

	public void Tick()
	{
		if (CombatAreaCache.IsInIncursion && interval_0.Elapsed && LokiPoe.IsInGame)
		{
			Class24 cachedIncursionData = CachedIncursionData;
			DoorScan(cachedIncursionData);
			ArchitectScan(cachedIncursionData);
		}
	}

	private static void DoorScan(Class24 data)
	{
		List<NetworkObject> incursionDoors = IncursionDoors;
		foreach (NetworkObject item in incursionDoors)
		{
			bool isTargetable = item.IsTargetable;
			int int_0 = item.Id;
			int num = data.list_1.FindIndex((CachedObject d) => d.Id == int_0);
			if (num >= 0)
			{
				if (!isTargetable)
				{
					GlobalLog.Info($"[HandleIncursionTask] Removing opened {item.WalkablePosition()}");
					data.list_1.RemoveAt(num);
				}
			}
			else if (isTargetable)
			{
				WalkablePosition walkablePosition = item.WalkablePosition();
				GlobalLog.Warn($"[HandleIncursionTask] Registering {walkablePosition}");
				data.list_1.Add(new CachedObject(int_0, walkablePosition));
			}
		}
	}

	private static void ArchitectScan(Class24 data)
	{
		List<MinimapIconWrapper> minimapIcons = InstanceInfo.MinimapIcons;
		foreach (MinimapIconWrapper item in minimapIcons)
		{
			string name = item.MinimapIcon.Name;
			if (name != "IncursionArchitectUpgrade" && name != "IncursionArchitectReplace")
			{
				continue;
			}
			int int_0 = item.ObjectId;
			if (Blacklist.Contains(int_0))
			{
				continue;
			}
			RoomEntry currentRoomSettings = IncursionEx.CurrentRoomSettings;
			if (currentRoomSettings != null)
			{
				if (currentRoomSettings.NoUpgrade && name == "IncursionArchitectUpgrade")
				{
					GlobalLog.Warn("[HandleIncursionTask] Blacklisting upgrading architect according to settings.");
					Blacklist.Add(int_0, TimeSpan.FromMinutes(10.0), "");
					continue;
				}
				if (currentRoomSettings.NoChange && name == "IncursionArchitectReplace")
				{
					GlobalLog.Warn("[HandleIncursionTask] Blacklisting changing architect according to settings.");
					Blacklist.Add(int_0, TimeSpan.FromMinutes(10.0), "");
					continue;
				}
			}
			NetworkObject networkObject = item.NetworkObject;
			Monster val = (Monster)(object)((networkObject is Monster) ? networkObject : null);
			if ((NetworkObject)(object)val == (NetworkObject)null)
			{
				continue;
			}
			bool isDead = ((Actor)val).IsDead;
			int num = data.list_0.FindIndex((Class49 a) => a.Id == int_0);
			if (num >= 0)
			{
				if (isDead)
				{
					GlobalLog.Info($"[HandleIncursionTask] Removing dead {((NetworkObject)(object)val).WalkablePosition()}");
					data.list_0.RemoveAt(num);
				}
				else
				{
					data.list_0[num].Position = ((NetworkObject)(object)val).WalkablePosition();
				}
			}
			else if (!isDead)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)val).WalkablePosition();
				GlobalLog.Warn($"[HandleIncursionTask] Registering {walkablePosition}");
				data.list_0.Add(new Class49(int_0, walkablePosition, name));
			}
		}
	}

	private static async Task<bool> OpenIncursionDoor()
	{
		if (HasKey)
		{
			CachedObject door = CachedIncursionData.list_1.Find((CachedObject d) => !d.Unwalkable && !d.Ignored);
			if (door == null)
			{
				return false;
			}
			WalkablePosition pos = door.Position;
			if (!pos.IsFar && !pos.IsFarByPath)
			{
				NetworkObject networkObject_0 = door.Object;
				if (networkObject_0 == (NetworkObject)null)
				{
					GlobalLog.Error("[HandleIncursionTask] Unexpected error. We are near cached incursion door, but actual object is null.");
					door.Ignored = true;
					return true;
				}
				string name = networkObject_0.Name;
				if (++door.InteractionAttempts <= 3)
				{
					if (!networkObject_0.IsTargetable)
					{
						CachedIncursionData.list_1.Remove(door);
						return true;
					}
					if (await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, name + " interaction"))
					{
						CachedIncursionData.list_1.Remove(door);
					}
					return true;
				}
				GlobalLog.Error("[HandleIncursionTask] All attempts to open " + name + " have been spent.");
				door.Ignored = true;
				return true;
			}
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[HandleIncursionTask] Fail to move to {pos}.");
				door.Unwalkable = true;
			}
			return true;
		}
		return false;
	}

	private static async Task<bool> KillArchitect(string type)
	{
		Class49 arch = CachedIncursionData.list_0.Find((Class49 a) => !a.Unwalkable && !a.Ignored && a.Type == type);
		if (arch == null)
		{
			return false;
		}
		WalkablePosition pos = arch.Position;
		if (pos.IsFar)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[HandleIncursionTask] Fail to move to {pos}. Now marking it as unwalkable.");
				arch.Unwalkable = true;
			}
			return true;
		}
		int attempts = ++arch.InteractionAttempts;
		if (attempts > 15)
		{
			GlobalLog.Error("[HandleIncursionTask] " + pos.Name + " was not killed. Now ignoring it.");
			arch.Ignored = true;
			return true;
		}
		await Coroutines.FinishCurrentAction(true);
		GlobalLog.Debug($"[HandleIncursionTask] Waiting for combat routine to kill the architect ({attempts}/{15})");
		await Wait.StuckDetectionSleep(200);
		return true;
	}

	private static async Task<bool> ExitIncursion()
	{
		CachedTransition cachedPortal = CombatAreaCache.Current.AreaTransitions.Find((CachedTransition a) => a.Type == TransitionType.Incursion && !a.Ignored && !a.Unwalkable);
		if (cachedPortal == null)
		{
			return false;
		}
		WalkablePosition pos = cachedPortal.Position;
		if (pos.IsFar || pos.IsFarByPath)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[HandleIncursionTask] Fail to move to {pos}.");
				cachedPortal.Unwalkable = true;
			}
			return true;
		}
		AreaTransition portalObj = cachedPortal.Object;
		if (!((NetworkObject)(object)portalObj == (NetworkObject)null))
		{
			if (((NetworkObject)portalObj).IsTargetable)
			{
				if (++cachedPortal.InteractionAttempts > 5)
				{
					GlobalLog.Error("[HandleIncursionTask] All attempts to interact with Time Portal have been spent.");
					cachedPortal.Ignored = true;
					return true;
				}
				await PlayerAction.TakeTransition(portalObj);
				GlobalLog.Info("Incursion is finished, waiting 2 secs for loot to drop");
				await Wait.Sleep(2000);
				return true;
			}
			GlobalLog.Error("[HandleIncursionTask] Cannot exit incursion. Time Portal is not active.");
			cachedPortal.Ignored = true;
			return true;
		}
		GlobalLog.Error("[HandleIncursionTask] Unexpected error. We are near cached Time Portal, but actual object is null.");
		cachedPortal.Ignored = true;
		return true;
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

	static HandleIncursionTask()
	{
		interval_0 = new Interval(600);
	}
}
