using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.Global;

public static class TrackMobLogic
{
	private static readonly Interval interval_0;

	public static CachedMonster CurrentTarget;

	private static readonly Dictionary<int, CachedMonster> dictionary_0;

	private static readonly Dictionary<int, CachedMonster> dictionary_1;

	static TrackMobLogic()
	{
		interval_0 = new Interval(3000);
		dictionary_0 = new Dictionary<int, CachedMonster>();
		dictionary_1 = new Dictionary<int, CachedMonster>();
		Events.AreaChanged += delegate
		{
			CurrentTarget = null;
			dictionary_0.Clear();
			dictionary_1.Clear();
		};
	}

	public static void AddBlighted(CachedMonster m)
	{
		dictionary_1[m.Id] = m;
	}

	public static void AddValuable(CachedMonster m)
	{
		dictionary_0[m.Id] = m;
	}

	private static void SelectTarget(int range, bool trackAll, bool byRarity, bool onlyBlighted)
	{
		Dictionary<int, CachedMonster>.ValueCollection values = dictionary_0.Values;
		Dictionary<int, CachedMonster>.ValueCollection values2 = dictionary_1.Values;
		if (onlyBlighted)
		{
			CurrentTarget = ((range == -1) ? values2.ClosestValid() : values2.ClosestValid((CachedMonster m) => m.Position.Distance <= range));
			return;
		}
		CurrentTarget = ((range == -1) ? values.ClosestValid() : values.ClosestValid((CachedMonster m) => m.Position.Distance <= range));
		if (!(CurrentTarget == null && trackAll))
		{
			return;
		}
		IEnumerable<CachedMonster> enumerable = Enumerable.Where(CombatAreaCache.Current.Monsters, (CachedMonster e) => !e.Ignored && !e.Unwalkable);
		if (range == -1)
		{
			CurrentTarget = (byRarity ? enumerable.OrderByDescending((CachedMonster m) => m.Rarity).FirstOrDefault() : (from m in enumerable
				orderby (int)m.Rarity != 3 descending, m.Position.Distance
				select m).FirstOrDefault());
		}
		else
		{
			CurrentTarget = enumerable.ClosestValid((CachedMonster m) => m.Position.Distance <= range);
		}
	}

	public static async Task<bool> Execute(int range = -1, bool trackAll = true, bool byRarity = true, bool onlyBlighted = false)
	{
		List<CachedMonster> cachedMonsters = CombatAreaCache.Current.Monsters;
		if (CurrentTarget == null)
		{
			SelectTarget(range, trackAll, byRarity, onlyBlighted);
			if (CurrentTarget == null)
			{
				return false;
			}
		}
		if (Blacklist.Contains(CurrentTarget.Id))
		{
			GlobalLog.Debug("[TrackMobLogic] Current target is in global blacklist. Now abandoning it.");
			CurrentTarget.Ignored = true;
			RemoveCached(CurrentTarget);
			return true;
		}
		WalkablePosition pos = CurrentTarget.Position;
		if (pos.IsFar || pos.IsFarByPath)
		{
			if (interval_0.Elapsed)
			{
				GlobalLog.Debug($"[TrackMobTask] Cached monster locations: {cachedMonsters.Valid().Count()}");
				GlobalLog.Debug($"[TrackMobTask] Moving to {pos}:{CurrentTarget.Rarity}");
			}
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[TrackMobTask] Fail to move to {pos}. Marking this monster as unwalkable.");
				CurrentTarget.Unwalkable = true;
				RemoveCached(CurrentTarget);
			}
			return true;
		}
		Monster monsterObj = CurrentTarget.Object;
		if ((NetworkObject)(object)monsterObj == (NetworkObject)null || ((Actor)monsterObj).IsDead)
		{
			cachedMonsters.Remove(CurrentTarget);
			RemoveCached(CurrentTarget);
		}
		else
		{
			int attempts = ++CurrentTarget.InteractionAttempts;
			if (attempts > 20)
			{
				GlobalLog.Error("[TrackMobTask] All attempts to kill current monster have been spent. Now ignoring it.");
				CurrentTarget.Ignored = true;
				RemoveCached(CurrentTarget);
				return true;
			}
			GlobalLog.Debug($"[TrackMobTask] Alive monster is nearby, this is our {attempts}/{20} attempt to kill it.");
			Utility.BroadcastMessage((object)null, "SetBestTarget", new object[1] { CurrentTarget.Id });
			await Coroutine.Sleep(15);
		}
		return true;
	}

	public static void RemoveCached(CachedObject monster)
	{
		if (monster == CurrentTarget)
		{
			CurrentTarget = null;
		}
		dictionary_0.Remove(monster.Id);
		dictionary_1.Remove(monster.Id);
	}
}
