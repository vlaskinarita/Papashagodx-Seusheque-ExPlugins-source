using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class OpenChestTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly ExtensionsSettings Settings;

	private static readonly Interval interval_0;

	private static readonly Dictionary<int, Stopwatch> dictionary_0;

	private static readonly TimeSpan whKfegrYyW;

	private static CachedObject cachedObject_0;

	private static CachedObject cachedObject_1;

	private static CachedObject cachedObject_2;

	private static CachedStrongbox cachedStrongbox_0;

	private static readonly List<CachedObject> list_0;

	private static readonly Interval interval_1;

	public string Name => "OpenChestTask";

	public string Description => "Task for opening chests, strongboxes and shrines.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!World.CurrentArea.IsCombatArea)
		{
			return false;
		}
		CombatAreaCache cache = CombatAreaCache.Current;
		List<CachedObject> cystCluster = (from c in Enumerable.Where(cache.SpecialChests, (CachedObject o) => o.Object != (NetworkObject)null && !list_0.Contains(o) && o.Object.Distance < 20f && o.Object.Metadata.Contains("Chests/Blight") && !o.Ignored)
			orderby c.Position.Distance
			select c).ToList();
		if (cystCluster.Any())
		{
			if ((NetworkObject)(object)PlayerAction.AnyPortalInRangeOf(20) != (NetworkObject)null)
			{
				WorldPosition walkablePos = WorldPosition.FindPathablePositionAtDistance(60, 150, 30);
				await Move.AtOnce(walkablePos, "portal position");
				await PlayerAction.CreateTownPortal();
			}
			await cystCluster.OrderBy((CachedObject c) => c.Position.Distance).First().Position.TryComeAtOnce();
			await ProcessBlightCystCluster(cystCluster);
			return true;
		}
		if (!(cachedObject_1 != null))
		{
			CachedObject closestSpecialChest = Enumerable.Where(cache.SpecialChests, (CachedObject c) => !IsTemporaryIgnored(c.Id)).ClosestValid();
			if (!(closestSpecialChest != null))
			{
				if (Settings.ChestOpenRange != 0)
				{
					if (cachedObject_0 != null)
					{
						await ProcessChest();
						return true;
					}
					CachedObject closestChest = cache.Chests.ClosestValid();
					if (closestChest != null && ShouldOpen(closestChest, Settings.ChestOpenRange, Settings.Chests))
					{
						cachedObject_0 = closestChest;
						return true;
					}
				}
				if (Settings.ShrineOpenRange != 0)
				{
					if (cachedObject_2 != null)
					{
						await ProcessShrine();
						return true;
					}
					CachedObject closestShrine = cache.Shrines.ClosestValid();
					if (closestShrine != null && ShouldOpen(closestShrine, Settings.ShrineOpenRange, Settings.Shrines))
					{
						cachedObject_2 = closestShrine;
						return true;
					}
				}
				if (Settings.StrongboxOpenRange != 0)
				{
					if (cachedStrongbox_0 != null)
					{
						await ProcessStrongbox();
						return true;
					}
					CachedStrongbox closestStrongbox = cache.Strongboxes.ClosestValid();
					if (closestStrongbox != null && closestStrongbox.Rarity <= Settings.MaxStrongboxRarity && ShouldOpen(closestStrongbox, Settings.StrongboxOpenRange, Settings.Strongboxes))
					{
						cachedStrongbox_0 = closestStrongbox;
						return true;
					}
				}
				return false;
			}
			cachedObject_1 = closestSpecialChest;
			return true;
		}
		await ProcessSpecialChest();
		return true;
	}

	public void Tick()
	{
		if (!LokiPoe.IsInGame || !interval_0.Elapsed)
		{
			return;
		}
		if (interval_1.Elapsed)
		{
			list_0.Clear();
		}
		if (cachedObject_1 != null)
		{
			NetworkObject @object = cachedObject_1.Object;
			Chest val = (Chest)(object)((@object is Chest) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && val.IsOpened)
			{
				CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
				cachedObject_1 = null;
			}
		}
		if (cachedObject_0 != null)
		{
			NetworkObject object2 = cachedObject_0.Object;
			Chest val2 = (Chest)(object)((object2 is Chest) ? object2 : null);
			if ((NetworkObject)(object)val2 != (NetworkObject)null && val2.IsOpened)
			{
				CombatAreaCache.Current.Chests.Remove(cachedObject_0);
				cachedObject_0 = null;
			}
		}
		if (cachedStrongbox_0 != null)
		{
			Chest object3 = cachedStrongbox_0.Object;
			if ((NetworkObject)(object)object3 != (NetworkObject)null && (object3.IsOpened || object3.IsLocked))
			{
				CombatAreaCache.Current.Strongboxes.Remove(cachedStrongbox_0);
				cachedStrongbox_0 = null;
			}
		}
		if (cachedObject_2 != null)
		{
			NetworkObject object4 = cachedObject_2.Object;
			Shrine val3 = (Shrine)(object)((object4 is Shrine) ? object4 : null);
			if ((NetworkObject)(object)val3 != (NetworkObject)null && val3.IsDeactivated)
			{
				CombatAreaCache.Current.Shrines.Remove(cachedObject_2);
				cachedObject_2 = null;
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "area_changed_event"))
		{
			return (MessageResult)1;
		}
		cachedObject_0 = null;
		cachedObject_1 = null;
		cachedStrongbox_0 = null;
		cachedObject_2 = null;
		dictionary_0.Clear();
		list_0.Clear();
		foreach (CachedObject item in Enumerable.Where(CombatAreaCache.Current.Chests, (CachedObject i) => i.Ignored))
		{
			GlobalLog.Debug($"[{Name}] Removing ignored flag from {item.Position} because we changed area.");
			item.Ignored = false;
		}
		foreach (CachedObject item2 in Enumerable.Where(CombatAreaCache.Current.SpecialChests, (CachedObject i) => i.Ignored))
		{
			GlobalLog.Debug($"[{Name}] Removing ignored flag from {item2.Position} because we changed area.");
			item2.Ignored = false;
		}
		return (MessageResult)0;
	}

	private static async Task ProcessBlightCystCluster(List<CachedObject> chestList)
	{
		Chest chest_0 = default(Chest);
		foreach (CachedObject chest in chestList)
		{
			list_0.Add(chest);
			if (chest.Position.IsFar)
			{
				continue;
			}
			cachedObject_1 = chest;
			ref Chest reference = ref chest_0;
			NetworkObject @object = chest.Object;
			reference = (Chest)(object)((@object is Chest) ? @object : null);
			if ((NetworkObject)(object)chest_0 == (NetworkObject)null || chest_0.IsOpened)
			{
				CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
				cachedObject_1 = null;
				continue;
			}
			WalkablePosition pos = chest.Position;
			cachedObject_1.InteractionAttempts++;
			if (cachedObject_1.InteractionAttempts <= 10)
			{
				ProcessHookManager.ClearAllKeyStates();
				bool flag = await PlayerAction.Interact((NetworkObject)(object)chest_0);
				if (flag)
				{
					flag = await Wait.For(() => chest_0.IsOpened, $"{pos} opening", 20, 200);
				}
				if (flag)
				{
					GlobalLog.Debug($"[OpenChestTask-FastOpen] {pos} succesfully opened!");
					CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
					StuckDetection.Reset();
					cachedObject_1 = null;
				}
				continue;
			}
			GlobalLog.Error($"[OpenChestTask] All attempts to open {pos} have been spent. Now ignoring it.");
			TemporaryIgnore(cachedObject_1.Id);
			cachedObject_1 = null;
			return;
		}
	}

	private static async Task ProcessChest()
	{
		WalkablePosition pos = cachedObject_0.Position;
		if (Settings.ChestOpenRange != -1 && (float)pos.Distance > (float)Settings.ChestOpenRange * 1.25f)
		{
			GlobalLog.Debug("[OpenChestTask] Abandoning current chest because its too far away.");
			TemporaryIgnore(cachedObject_0.Id);
			cachedObject_0 = null;
			return;
		}
		if (pos.IsFar)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[OpenChestTask] Fail to move to {pos}. Marking this chest as unwalkable.");
				cachedObject_0.Unwalkable = true;
				cachedObject_0 = null;
			}
			return;
		}
		Chest chest_0 = default(Chest);
		ref Chest reference = ref chest_0;
		NetworkObject @object = cachedObject_0.Object;
		reference = (Chest)(object)((@object is Chest) ? @object : null);
		if ((NetworkObject)(object)chest_0 == (NetworkObject)null || chest_0.IsOpened)
		{
			CombatAreaCache.Current.Chests.Remove(cachedObject_0);
			cachedObject_0 = null;
		}
		else if (++cachedObject_0.InteractionAttempts > 3)
		{
			GlobalLog.Error("[OpenChestTask] All attempts to open a chest have been spent. Now ignoring it.");
			cachedObject_0.Ignored = true;
			cachedObject_0 = null;
		}
		else if (await PlayerAction.Interact((NetworkObject)(object)chest_0))
		{
			if (await Wait.For(() => chest_0.IsOpened, "chest opening", 50, 300))
			{
				CombatAreaCache.Current.Chests.Remove(cachedObject_0);
				cachedObject_0 = null;
			}
		}
		else
		{
			await Wait.SleepSafe(300);
		}
	}

	private static async Task ProcessStrongbox()
	{
		WalkablePosition pos = cachedStrongbox_0.Position;
		int id = cachedStrongbox_0.Id;
		if (Settings.StrongboxOpenRange != -1 && (float)pos.Distance > (float)Settings.StrongboxOpenRange * 1.25f)
		{
			GlobalLog.Debug("[OpenChestTask] Abandoning current strongbox because its too far away.");
			TemporaryIgnore(cachedStrongbox_0.Id);
			cachedStrongbox_0 = null;
		}
		else if (!pos.IsFar)
		{
			Chest NcIzhoEgeI = cachedStrongbox_0.Object;
			if (!((NetworkObject)(object)NcIzhoEgeI == (NetworkObject)null) && !NcIzhoEgeI.IsOpened)
			{
				if (!NcIzhoEgeI.IsLocked)
				{
					if (++cachedStrongbox_0.InteractionAttempts > 5)
					{
						GlobalLog.Error("[OpenChestTask] All attempts to open a strongbox have been spent. Now ignoring it.");
						cachedStrongbox_0.Ignored = true;
						cachedStrongbox_0 = null;
					}
					else if (await PlayerAction.Interact((NetworkObject)(object)NcIzhoEgeI))
					{
						if (await Wait.For(() => NcIzhoEgeI.IsLocked, "strongbox opening", 100, 400))
						{
							await PlayerAction.MoveAway(25, 55);
							Utility.BroadcastMessage((object)null, "strongbox_opened_event", Array.Empty<object>());
							CombatAreaCache.Current.RemoveStrongboxFromCache(id);
							cachedStrongbox_0 = null;
						}
					}
					else
					{
						await Wait.SleepSafe(400);
					}
				}
				else
				{
					cachedStrongbox_0 = null;
				}
			}
			else
			{
				CombatAreaCache.Current.RemoveStrongboxFromCache(id);
				cachedStrongbox_0 = null;
			}
		}
		else if (!pos.TryCome())
		{
			GlobalLog.Error($"[OpenChestTask] Fail to move to {pos}. Marking this strongbox as unwalkable.");
			cachedStrongbox_0.Unwalkable = true;
			cachedStrongbox_0 = null;
		}
	}

	private static async Task ProcessShrine()
	{
		if (Blacklist.Contains(cachedObject_2.Id))
		{
			GlobalLog.Error("[OpenChestTask] Current shrine was blacklisted from outside.");
			cachedObject_2.Ignored = true;
			cachedObject_2 = null;
			return;
		}
		WalkablePosition pos = cachedObject_2.Position;
		if (Settings.ShrineOpenRange != -1 && (float)pos.Distance > (float)Settings.ShrineOpenRange * 1.25f)
		{
			GlobalLog.Debug("[OpenChestTask] Abandoning current shrine because its too far away.");
			TemporaryIgnore(cachedObject_2.Id);
			cachedObject_2 = null;
		}
		else if (!pos.IsFar)
		{
			Shrine shrine_0 = default(Shrine);
			ref Shrine reference = ref shrine_0;
			NetworkObject @object = cachedObject_2.Object;
			reference = (Shrine)(object)((@object is Shrine) ? @object : null);
			if (!((NetworkObject)(object)shrine_0 == (NetworkObject)null) && !shrine_0.IsDeactivated)
			{
				if (++cachedObject_2.InteractionAttempts > 5)
				{
					GlobalLog.Error("[OpenChestTask] All attempts to take a shrine have been spent. Now ignoring it.");
					cachedObject_2.Ignored = true;
					cachedObject_2 = null;
				}
				else if (await PlayerAction.Interact((NetworkObject)(object)shrine_0))
				{
					await Wait.LatencySleep();
					if (await Wait.For(() => shrine_0.IsDeactivated, "shrine deactivation", 100, 400))
					{
						CombatAreaCache.Current.Shrines.Remove(cachedObject_2);
						cachedObject_2 = null;
					}
				}
				else
				{
					await Wait.SleepSafe(400);
				}
			}
			else
			{
				CombatAreaCache.Current.Shrines.Remove(cachedObject_2);
				cachedObject_2 = null;
			}
		}
		else if (!pos.TryCome())
		{
			GlobalLog.Error($"[OpenChestTask] Fail to move to {pos}. Marking this shrine as unwalkable.");
			cachedObject_2.Unwalkable = true;
			cachedObject_2 = null;
		}
	}

	private static async Task ProcessSpecialChest()
	{
		WalkablePosition pos = cachedObject_1.Position;
		if (pos == null)
		{
			GlobalLog.Error("[OpenChestTask] Something is weird. Chestposition returned null. It's most likely is already opened. Removing from cache");
			CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
			cachedObject_1 = null;
			return;
		}
		Chest chest_0 = default(Chest);
		ref Chest reference = ref chest_0;
		NetworkObject @object = cachedObject_1.Object;
		reference = (Chest)(object)((@object is Chest) ? @object : null);
		if (!((NetworkObject)(object)chest_0 != (NetworkObject)null) || !chest_0.IsOpened)
		{
			if (!pos.IsFar && !(pos.PathDistance > 25f))
			{
				if ((NetworkObject)(object)chest_0 == (NetworkObject)null || chest_0.IsOpened)
				{
					CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
					cachedObject_1 = null;
					StuckDetection.Reset();
					return;
				}
				if (++cachedObject_1.InteractionAttempts > 10)
				{
					GlobalLog.Error("[OpenChestTask] All attempts to open " + pos.Name + " have been spent. Now ignoring it.");
					TemporaryIgnore(cachedObject_1.Id);
					cachedObject_1 = null;
					return;
				}
				await Coroutines.FinishCurrentAction(true);
				bool flag = await PlayerAction.Interact((NetworkObject)(object)chest_0);
				if (flag)
				{
					flag = await Wait.For(() => chest_0.IsOpened, $"{pos} opening", 20, 800);
				}
				if (flag)
				{
					CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
					cachedObject_1 = null;
					StuckDetection.Reset();
				}
			}
			else if (!pos.TryCome())
			{
				GlobalLog.Error($"[OpenChestTask] Fail to move to {pos}. Marking this special chest as unwalkable.");
				cachedObject_1.Unwalkable = true;
				cachedObject_1 = null;
			}
		}
		else
		{
			CombatAreaCache.Current.SpecialChests.Remove(cachedObject_1);
			cachedObject_1 = null;
		}
	}

	private static bool ShouldOpen(CachedObject obj, int openRange, IEnumerable<ExtensionsSettings.ChestEntry> settingsList)
	{
		if (openRange != -1 && obj.Position.Distance > openRange)
		{
			return false;
		}
		return !IsTemporaryIgnored(obj.Id) && Enumerable.Where(settingsList, (ExtensionsSettings.ChestEntry c) => !c.Enabled).All((ExtensionsSettings.ChestEntry c) => c.Name != obj.Position.Name);
	}

	private static void TemporaryIgnore(int id)
	{
		if (!dictionary_0.ContainsKey(id))
		{
			dictionary_0.Add(id, Stopwatch.StartNew());
		}
	}

	private static bool IsTemporaryIgnored(int id)
	{
		if (!dictionary_0.TryGetValue(id, out var value))
		{
			return false;
		}
		if (value.Elapsed < whKfegrYyW)
		{
			return true;
		}
		dictionary_0.Remove(id);
		return false;
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

	static OpenChestTask()
	{
		Settings = ExtensionsSettings.Instance;
		interval_0 = new Interval(100);
		dictionary_0 = new Dictionary<int, Stopwatch>();
		whKfegrYyW = TimeSpan.FromSeconds(30.0);
		list_0 = new List<CachedObject>();
		interval_1 = new Interval(6000);
	}
}
