using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.FilesInMemory;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;

namespace ExPlugins.EXtensions.Global;

public class CombatAreaCache
{
	public class ObjectDictionary
	{
		private readonly Dictionary<string, object> dictionary_0 = new Dictionary<string, object>();

		public object this[string key]
		{
			get
			{
				dictionary_0.TryGetValue(key, out var value);
				return value;
			}
			set
			{
				if (!dictionary_0.ContainsKey(key))
				{
					GlobalLog.Debug(string.Format("[Storage] Registering [{0}] = [{1}]", key, value ?? "null"));
					dictionary_0.Add(key, value);
				}
				else
				{
					dictionary_0[key] = value;
				}
			}
		}

		public bool Contains(string key)
		{
			return dictionary_0.ContainsKey(key);
		}
	}

	private class Class42
	{
		public readonly Func<Item, bool> func_0;

		public readonly string Id;

		public Class42(string id, Func<Item, bool> eval)
		{
			Id = id;
			func_0 = eval;
		}
	}

	private static readonly TimeSpan timeSpan_0;

	private static readonly Interval interval_0;

	private static readonly Interval interval_1;

	private static readonly List<Class42> list_0;

	private static readonly Dictionary<uint, CombatAreaCache> dictionary_0;

	private static readonly HashSet<string> hashSet_0;

	private readonly Stopwatch stopwatch_0;

	private readonly HashSet<int> hashSet_1 = new HashSet<int>();

	private readonly HashSet<int> hashSet_2 = new HashSet<int>();

	private readonly HashSet<Vector2i> hashSet_3 = new HashSet<Vector2i>();

	public readonly List<CachedTransition> AreaTransitions = new List<CachedTransition>();

	public readonly List<CachedBlightTower> BlightTowers = new List<CachedBlightTower>();

	public readonly List<CachedObject> Chests = new List<CachedObject>();

	public readonly List<CachedObject> CraftingRecipe = new List<CachedObject>();

	public readonly List<CachedHarvestIrrigator> HarvestIrrigators = new List<CachedHarvestIrrigator>();

	public readonly List<CachedWorldItem> Items = new List<CachedWorldItem>();

	public readonly List<CachedMonster> Monsters = new List<CachedMonster>();

	public readonly List<CachedObject> Shrines = new List<CachedObject>();

	public readonly List<CachedObject> SpecialChests = new List<CachedObject>();

	public readonly ObjectDictionary Storage = new ObjectDictionary();

	public readonly List<CachedStrongbox> Strongboxes = new List<CachedStrongbox>();

	private CombatAreaCache combatAreaCache_0;

	private CombatAreaCache combatAreaCache_1;

	private CombatAreaCache combatAreaCache_2;

	[CompilerGenerated]
	private static bool bool_0;

	[CompilerGenerated]
	private static bool bool_1;

	[CompilerGenerated]
	private static bool bool_2;

	[CompilerGenerated]
	private readonly uint uint_0;

	[CompilerGenerated]
	private readonly DatWorldAreaWrapper datWorldAreaWrapper_0;

	[CompilerGenerated]
	private readonly ComplexExplorer complexExplorer_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private int int_1;

	public static bool IsInIncursion
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		set
		{
			bool_0 = value;
		}
	}

	public static bool IsInHarvest
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
		[CompilerGenerated]
		set
		{
			bool_1 = value;
		}
	}

	public static bool IsInSyndicateLab
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
		[CompilerGenerated]
		set
		{
			bool_2 = value;
		}
	}

	public static CombatAreaCache Current
	{
		get
		{
			uint areaHash = LocalData.AreaHash;
			if (!dictionary_0.TryGetValue(areaHash, out var value))
			{
				RemoveOldCaches();
				CombatAreaCache combatAreaCache = new CombatAreaCache(areaHash);
				dictionary_0.Add(areaHash, combatAreaCache);
				return combatAreaCache;
			}
			value.stopwatch_0.Restart();
			if (!bool_0)
			{
				if (!bool_0 && value.combatAreaCache_1 != null)
				{
					value.combatAreaCache_1 = null;
				}
				if (!bool_1)
				{
					if (!bool_1 && value.combatAreaCache_0 != null)
					{
						value.combatAreaCache_0 = null;
					}
					if (!bool_2)
					{
						return value;
					}
					return value.combatAreaCache_2 ?? (value.combatAreaCache_2 = new CombatAreaCache(areaHash));
				}
				return value.combatAreaCache_0 ?? (value.combatAreaCache_0 = new CombatAreaCache(areaHash));
			}
			return value.combatAreaCache_1 ?? (value.combatAreaCache_1 = new CombatAreaCache(areaHash));
		}
	}

	public uint Hash
	{
		[CompilerGenerated]
		get
		{
			return uint_0;
		}
	}

	public DatWorldAreaWrapper WorldArea
	{
		[CompilerGenerated]
		get
		{
			return datWorldAreaWrapper_0;
		}
	}

	public ComplexExplorer Explorer
	{
		[CompilerGenerated]
		get
		{
			return complexExplorer_0;
		}
	}

	public int DeathCount
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
		[CompilerGenerated]
		internal set
		{
			int_0 = value;
		}
	}

	public int StuckCount
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
		[CompilerGenerated]
		internal set
		{
			int_1 = value;
		}
	}

	static CombatAreaCache()
	{
		timeSpan_0 = TimeSpan.FromMinutes(15.0);
		interval_0 = new Interval(100);
		interval_1 = new Interval(50);
		list_0 = new List<Class42>();
		dictionary_0 = new Dictionary<uint, CombatAreaCache>();
		hashSet_0 = new HashSet<string> { "Metadata/Chests/BootyChest", "Metadata/Chests/NotSoBootyChest", "Metadata/Chests/VaultTreasurePile", "Metadata/Chests/GhostPirateBootyChest", "Metadata/Chests/StatueMakersTools", "Metadata/Chests/StrongBoxes/VaultsOfAtziriUniqueChest", "Metadata/Chests/CopperChestEpic3", "Metadata/Chests/TutorialSupportGemChest" };
		ItemEvaluator.OnRefreshed += OnItemEvaluatorRefresh;
		ComplexExplorer.LocalTransitionEntered += OnLocalTransitionEntered;
		BotManager.OnBotChanged += delegate
		{
			dictionary_0.Clear();
		};
	}

	private CombatAreaCache(uint hash)
	{
		GlobalLog.Info($"[CombatAreaCache] Creating cache for \"{World.CurrentArea.Name}\" (hash: {hash})");
		uint_0 = hash;
		datWorldAreaWrapper_0 = World.CurrentArea;
		complexExplorer_0 = new ComplexExplorer();
		stopwatch_0 = Stopwatch.StartNew();
	}

	public static bool IsAreaCached()
	{
		uint areaHash = LocalData.AreaHash;
		return dictionary_0.ContainsKey(areaHash);
	}

	private static void RemoveOldCaches()
	{
		List<CombatAreaCache> list = (from c in Enumerable.Where(dictionary_0, (KeyValuePair<uint, CombatAreaCache> c) => c.Value.stopwatch_0.Elapsed > timeSpan_0)
			select c.Value).ToList();
		foreach (CombatAreaCache item in list)
		{
			GlobalLog.Info($"[CombatAreaCache] Removing cache for \"{item.WorldArea.Name}\" (hash: {item.Hash}). Last accessed {(int)item.stopwatch_0.Elapsed.TotalMinutes} minutes ago.");
			dictionary_0.Remove(item.Hash);
		}
	}

	public static bool AddPickupItemEvaluator(string id, Func<Item, bool> evaluator)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (evaluator == null)
		{
			throw new ArgumentNullException("evaluator");
		}
		if (list_0.Exists((Class42 e) => e.Id == id))
		{
			return false;
		}
		list_0.Add(new Class42(id, evaluator));
		return true;
	}

	public static bool RemovePickupItemEvaluator(string id)
	{
		if (id != null)
		{
			int num = list_0.FindIndex((Class42 e) => e.Id == id);
			if (num >= 0)
			{
				list_0.RemoveAt(num);
				return true;
			}
			return false;
		}
		throw new ArgumentNullException("id");
	}

	public static void Tick()
	{
		if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead && World.CurrentArea.IsCombatArea)
		{
			Current.OnTick();
		}
	}

	private void OnTick()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		Current.Explorer.Tick();
		if (interval_1.Elapsed)
		{
			WorldItemScan();
		}
		if (!interval_0.Elapsed)
		{
			return;
		}
		foreach (NetworkObject item in ObjectManager.Objects.ToHashSet())
		{
			if (item.AnimatedPropertiesMetadata == "Metadata/Terrain/Doodads/EndGame/FrozenCabins/CorsairGate.ao")
			{
				TriggerableBlockage val = (TriggerableBlockage)(object)((item is TriggerableBlockage) ? item : null);
				if ((NetworkObject)(object)val == (NetworkObject)null)
				{
					continue;
				}
				int id = ((NetworkObject)val).Id;
				if (!hashSet_2.Contains(id))
				{
					if (val.IsOpened)
					{
						ExilePather.PolyPathfinder.ClearObstacles();
						hashSet_2.Add(id);
					}
					else
					{
						ExilePather.PolyPathfinder.AddObstacle(((NetworkObject)val).Position, 10f);
						ExilePather.PolyPathfinder.UpdateObstacles();
					}
				}
				continue;
			}
			if (item.Metadata == "Metadata/Terrain/Leagues/Crucible/Objects/CrucibleDevice")
			{
				int id2 = item.Id;
				if (!hashSet_2.Contains(id2))
				{
					Utility.BroadcastMessage((object)this, "crucible_found", new object[1] { $"Found Crucible at distance: {(int)item.Distance} [{WorldArea.Name}]" });
					ExilePather.PolyPathfinder.AddObstacle(item.Position, 7f);
					ExilePather.PolyPathfinder.UpdateObstacles();
					hashSet_2.Add(id2);
				}
				continue;
			}
			AreaTransition val2 = (AreaTransition)(object)((item is AreaTransition) ? item : null);
			if (!((NetworkObject)(object)val2 != (NetworkObject)null))
			{
				HarvestIrrigator val3 = (HarvestIrrigator)(object)((item is HarvestIrrigator) ? item : null);
				if (!((NetworkObject)(object)val3 != (NetworkObject)null))
				{
					BlightDefensiveTower val4 = (BlightDefensiveTower)(object)((item is BlightDefensiveTower) ? item : null);
					if (!((NetworkObject)(object)val4 != (NetworkObject)null))
					{
						Monster val5 = (Monster)(object)((item is Monster) ? item : null);
						if (!((NetworkObject)(object)val5 != (NetworkObject)null))
						{
							if (item.Metadata.Contains("Metadata/Terrain/Missions/CraftingUnlocks/"))
							{
								CraftingRecipe val6 = (CraftingRecipe)(object)((item is CraftingRecipe) ? item : null);
								if ((NetworkObject)(object)val6 != (NetworkObject)null)
								{
									ProcessRecipe(val6);
									continue;
								}
								GlobalLog.Debug("[CombatAreaCache] The recipe is null.");
							}
							Chest val7 = (Chest)(object)((item is Chest) ? item : null);
							if ((NetworkObject)(object)val7 != (NetworkObject)null)
							{
								if (!IsSpecialChest((NetworkObject)(object)val7))
								{
									if (val7.IsStrongBox)
									{
										ProcessStrongbox(val7);
									}
									else
									{
										ProcessChest(val7);
									}
								}
								else
								{
									ProcessSpeacialChest(val7);
								}
							}
							else
							{
								Shrine val8 = (Shrine)(object)((item is Shrine) ? item : null);
								if ((NetworkObject)(object)val8 != (NetworkObject)null)
								{
									ProcessShrine(val8);
								}
							}
						}
						else
						{
							ProcessMonster(val5);
						}
					}
					else
					{
						ProcessBlightTower(val4);
					}
				}
				else if (bool_1)
				{
					ProcessHarvestIrrigator(val3);
				}
			}
			else
			{
				ProcessTransition(val2);
			}
		}
		if (bool_1)
		{
			UpdateHarvest();
		}
		UpdateMonsters();
	}

	private void WorldItemScan()
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			WorldItem val = (WorldItem)(object)((@object is WorldItem) ? @object : null);
			if ((NetworkObject)(object)val == (NetworkObject)null || (val.IsAllocatedToOther && DateTime.Now < val.PublicTime))
			{
				continue;
			}
			int id = ((NetworkObject)val).Id;
			if (!hashSet_1.Contains(id))
			{
				Item item_0 = val.Item;
				if (ItemEvaluator.Match(item_0, (EvaluationType)1) || list_0.Exists((Class42 e) => e.func_0(item_0)))
				{
					WalkablePosition walkablePosition = ((NetworkObject)(object)val).WalkablePosition();
					walkablePosition.Initialized = true;
					Items.Add(new CachedWorldItem(id, walkablePosition, item_0.Size, item_0.Rarity, item_0.Class, item_0.Name));
				}
				hashSet_1.Add(id);
			}
		}
	}

	public void RemoveItemFromCache(CachedWorldItem item)
	{
		hashSet_1.Remove(item.Id);
		Items.Remove(item);
	}

	public void RemoveStrongboxFromCache(int id)
	{
		hashSet_2.Remove(id);
		CachedStrongbox cachedStrongbox = Enumerable.FirstOrDefault(Strongboxes, (CachedStrongbox s) => s.Id == id);
		if (cachedStrongbox != null)
		{
			Strongboxes.Remove(cachedStrongbox);
		}
	}

	private void ProcessMonster(Monster m)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (((Actor)m).IsDead || m.CannotDie || (int)((NetworkObject)m).Reaction != 1 || ((!((NetworkObject)m).IsTargetable || ((Actor)m).GetStat((StatTypeGGG)141) == 1) && !IsEmerging(m)) || (!bool_0 && Enumerable.Any(m.ExplicitAffixes, (ModRecord a) => a.InternalName.StartsWith("MonsterIncursion"))))
		{
			return;
		}
		int id = ((NetworkObject)m).Id;
		if (!hashSet_2.Contains(id))
		{
			if (!HasImmunityAura((Actor)(object)m) && !SkipThisMob((NetworkObject)(object)m))
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)m).WalkablePosition();
				walkablePosition.Initialized = true;
				Monsters.Add(new CachedMonster(id, walkablePosition, ((Actor)m).Auras, m.Rarity, ((NetworkObject)m).Name, ((NetworkObject)m).Metadata));
				hashSet_2.Add(id);
			}
			else
			{
				hashSet_2.Add(id);
			}
		}
	}

	private void ProcessHarvestIrrigator(HarvestIrrigator irrigator)
	{
		int id = ((NetworkObject)irrigator).Id;
		if (!hashSet_2.Contains(id))
		{
			hashSet_2.Add(id);
			List<Tuple<int, string>> mobList = new List<Tuple<int, string>>();
			WalkablePosition position = ((NetworkObject)(object)irrigator).WalkablePosition();
			CachedHarvestIrrigator cachedHarvestIrrigator = new CachedHarvestIrrigator(id, position, mobList);
			HarvestIrrigators.Add(cachedHarvestIrrigator);
			GlobalLog.Debug($"[CombatAreaCache] Registering {cachedHarvestIrrigator.Position}.");
		}
	}

	private void ProcessBlightTower(BlightDefensiveTower tower)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		int id = ((NetworkObject)tower).Id;
		if (hashSet_2.Contains(id))
		{
			return;
		}
		WalkablePosition walkablePosition = ((NetworkObject)(object)tower).WalkablePosition();
		Vector2i vector2i_0 = walkablePosition.AsVector;
		if (hashSet_3.Contains(vector2i_0))
		{
			CachedBlightTower cachedBlightTower = Enumerable.FirstOrDefault(BlightTowers, (CachedBlightTower t) => t.Position.AsVector == vector2i_0 && t.Tier < tower.Tier);
			if (cachedBlightTower != null)
			{
				BlightTowers.RemoveAll((CachedBlightTower t) => t.Position.AsVector == vector2i_0);
				BlightTowers.Add(new CachedBlightTower(id, walkablePosition, tower.Tier, ((NetworkObject)tower).Name, ((NetworkObject)tower).Metadata));
				hashSet_2.Add(id);
				GlobalLog.Debug("[CombatAreaCache] Updating: " + cachedBlightTower.Name + " -> " + ((NetworkObject)tower).Name + ".");
				return;
			}
		}
		BlightTowers.Add(new CachedBlightTower(id, walkablePosition, tower.Tier, ((NetworkObject)tower).Name, ((NetworkObject)tower).Metadata));
		hashSet_2.Add(id);
		hashSet_3.Add(vector2i_0);
	}

	private void ProcessChest(Chest c)
	{
		if (c.IsOpened || c.IsLocked || c.OpensOnDamage || !((NetworkObject)c).IsTargetable)
		{
			return;
		}
		int id = ((NetworkObject)c).Id;
		string name = ((NetworkObject)c).Name;
		if (!hashSet_2.Contains(id))
		{
			hashSet_2.Add(id);
			if (!name.Contains("Sealed Remains") && !((NetworkObject)c).Metadata.Contains("Leagues/Harvest"))
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)c).WalkablePosition(5, 20);
				Chests.Add(new CachedObject(id, walkablePosition));
				GlobalLog.Debug($"[CombatAreaCache] Registering {walkablePosition} as chest. Stomp: {c.OpensOnDamage}");
			}
		}
	}

	private void ProcessRecipe(CraftingRecipe c)
	{
		if (!c.IsOpened && ((NetworkObject)c).IsTargetable)
		{
			int id = ((NetworkObject)c).Id;
			if (!hashSet_2.Contains(id))
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)c).WalkablePosition();
				CraftingRecipe.Add(new CachedObject(id, walkablePosition));
				hashSet_2.Add(id);
				GlobalLog.Warn($"[CombatAreaCache] Registering Crafting Recipe {walkablePosition}");
			}
		}
	}

	private void ProcessSpeacialChest(Chest c)
	{
		if (!c.IsOpened && ((NetworkObject)c).IsTargetable && (!c.IsLocked || ((NetworkObject)c).Metadata.Contains("/PerandusChests/")))
		{
			int id = ((NetworkObject)c).Id;
			if (!hashSet_2.Contains(id))
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)c).WalkablePosition();
				SpecialChests.Add(new CachedObject(id, walkablePosition));
				hashSet_2.Add(id);
				GlobalLog.Warn($"[CombatAreaCache] Marking {walkablePosition} as special chest.");
			}
		}
	}

	private void ProcessStrongbox(Chest box)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if (box.IsLocked)
		{
			return;
		}
		int id = ((NetworkObject)box).Id;
		if (!hashSet_2.Contains(id))
		{
			if (!box.IsOpened && ((NetworkObject)box).IsTargetable)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)box).WalkablePosition();
				hashSet_2.Add(id);
				Strongboxes.Add(new CachedStrongbox(id, walkablePosition, box.Rarity));
				GlobalLog.Warn($"[CombatAreaCache] Registering {walkablePosition}");
			}
			else
			{
				hashSet_2.Add(id);
			}
		}
	}

	private void ProcessShrine(Shrine s)
	{
		if (!s.IsDeactivated && ((NetworkObject)s).IsTargetable)
		{
			int id = ((NetworkObject)s).Id;
			if (!hashSet_2.Contains(id))
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)s).WalkablePosition();
				Shrines.Add(new CachedObject(id, walkablePosition));
				hashSet_2.Add(id);
				GlobalLog.Warn($"[CombatAreaCache] Registering {walkablePosition}");
			}
		}
	}

	private void ProcessTransition(AreaTransition t)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		int id = ((NetworkObject)t).Id;
		if (hashSet_2.Contains(id))
		{
			return;
		}
		if (SkipThisTransition(t))
		{
			hashSet_2.Add(id);
			return;
		}
		TransitionType transitionType = (((NetworkObject)t).Metadata.Contains("LabyrinthTrial") ? TransitionType.Trial : (((NetworkObject)t).Metadata.Contains("IncursionPortal") ? TransitionType.Incursion : (Enumerable.Any(t.ExplicitAffixes, (ModRecord a) => a.Category == "MapMissionMods") ? TransitionType.Master : (Enumerable.Any(t.ExplicitAffixes, (ModRecord a) => a.InternalName.Contains("CorruptedSideArea")) ? TransitionType.Vaal : (((NetworkObject)t).Metadata.Contains("SynthesisPortal") ? TransitionType.Synthesis : ((((NetworkObject)t).Name == "Syndicate Laboratory") ? TransitionType.Syndicate : ((((NetworkObject)t).Name == "The Sacred Grove") ? TransitionType.Harvest : (((int)t.TransitionType == 1) ? TransitionType.Local : TransitionType.Regular))))))));
		WalkablePosition walkablePosition = ((NetworkObject)(object)t).WalkablePosition(10, 20);
		DatWorldAreaWrapper destination = t.Destination ?? Dat.LookupWorldArea(1);
		CachedTransition cachedTransition = new CachedTransition(id, walkablePosition, transitionType, destination);
		AreaTransitions.Add(cachedTransition);
		hashSet_2.Add(id);
		GlobalLog.Debug($"[CombatAreaCache] Registering {walkablePosition} (Type: {transitionType})");
		TweakTransition(cachedTransition);
	}

	private void UpdateMonsters()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		List<CachedMonster> list = new List<CachedMonster>();
		foreach (CachedMonster monster in Monsters)
		{
			Monster @object = monster.Object;
			if ((NetworkObject)(object)@object != (NetworkObject)null)
			{
				if (!((Actor)@object).IsDead && !@object.CannotDie && (int)((NetworkObject)@object).Reaction == 1 && !HasImmunityAura((Actor)(object)@object))
				{
					WalkablePosition walkablePosition = ((NetworkObject)(object)@object).WalkablePosition();
					walkablePosition.Initialized = true;
					monster.Position = walkablePosition;
				}
				else
				{
					list.Add(monster);
					TrackMobLogic.RemoveCached(monster);
				}
			}
			else if (monster.Position.Distance <= 80)
			{
				list.Add(monster);
				TrackMobLogic.RemoveCached(monster);
			}
		}
		foreach (CachedMonster item in list)
		{
			Monsters.Remove(item);
		}
	}

	private void UpdateHarvest()
	{
		if (!ConfigManager.IsAlwaysHighlightEnabled)
		{
			return;
		}
		foreach (CachedHarvestIrrigator item in Enumerable.Where(HarvestIrrigators, (CachedHarvestIrrigator i) => i.Inactive))
		{
			HarvestIrrigator harvestIrrigator_0 = item.Object;
			if (!((NetworkObject)(object)harvestIrrigator_0 != (NetworkObject)null))
			{
				continue;
			}
			HarvestExtraxtor val = ObjectManager.GetObjectsByType<HarvestExtraxtor>().OrderBy(delegate(HarvestExtraxtor e)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Vector2i position2 = ((NetworkObject)e).Position;
				return ((Vector2i)(ref position2)).Distance(((NetworkObject)harvestIrrigator_0).Position);
			}).FirstOrDefault();
			List<Tuple<int, string>> list = new List<Tuple<int, string>>();
			List<Tuple<int, string>> monsterList = harvestIrrigator_0.MonsterList;
			if (monsterList != null && monsterList.Any())
			{
				list = monsterList;
			}
			if ((NetworkObject)(object)val != (NetworkObject)null)
			{
				List<Tuple<int, string>> monsterList2 = val.MonsterList;
				if (monsterList2 != null && monsterList2.Any())
				{
					list = monsterList2;
				}
			}
			if (list.Any())
			{
				item.MobList = list;
				item.Visited = true;
				item.Inactive = false;
				string arg = list.Select((Tuple<int, string> m) => $"[{m.Item1}] {m.Item2} ").Aggregate("", string.Concat);
				GlobalLog.Warn($"[CombatAreaCache] Updating {item.Position}: {arg}");
			}
		}
		foreach (CachedHarvestIrrigator cachedHarvestIrrigator_0 in Enumerable.Where(HarvestIrrigators, (CachedHarvestIrrigator i) => !i.Inactive))
		{
			HarvestIrrigator @object = cachedHarvestIrrigator_0.Object;
			HarvestExtraxtor val2 = ObjectManager.GetObjectsByType<HarvestExtraxtor>().OrderBy(delegate(HarvestExtraxtor e)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				Vector2i position = ((NetworkObject)e).Position;
				return ((Vector2i)(ref position)).Distance((Vector2i)cachedHarvestIrrigator_0.Position);
			}).FirstOrDefault();
			if (!((NetworkObject)(object)@object != (NetworkObject)null) || (!(((NetworkObject)@object).Distance < 85f) && !@object.MonsterList.Any() && (!((NetworkObject)(object)val2 != (NetworkObject)null) || !val2.MonsterList.Any())))
			{
				continue;
			}
			if (@object.MonsterList.Any() || (!((NetworkObject)(object)val2 == (NetworkObject)null) && val2.MonsterList.Any()))
			{
				List<Tuple<int, string>> list2 = new List<Tuple<int, string>>();
				List<Tuple<int, string>> monsterList3 = @object.MonsterList;
				if (monsterList3 != null && monsterList3.Any())
				{
					list2 = monsterList3;
				}
				if ((NetworkObject)(object)val2 != (NetworkObject)null)
				{
					List<Tuple<int, string>> monsterList4 = val2.MonsterList;
					if (monsterList4 != null && monsterList4.Any())
					{
						list2 = monsterList4;
					}
				}
				if (list2.Any() && !cachedHarvestIrrigator_0.MobList.Any())
				{
					cachedHarvestIrrigator_0.MobList = list2;
					cachedHarvestIrrigator_0.Visited = true;
					string arg2 = list2.Select((Tuple<int, string> m) => $"[{m.Item1}] {m.Item2} ").Aggregate("", string.Concat);
					GlobalLog.Warn($"[CombatAreaCache] Updating {cachedHarvestIrrigator_0.Position}: {arg2}");
				}
			}
			else
			{
				cachedHarvestIrrigator_0.Inactive = true;
				cachedHarvestIrrigator_0.Visited = true;
			}
		}
	}

	private static bool HasImmunityAura(Actor mob)
	{
		foreach (Aura aura in mob.Auras)
		{
			string internalName = aura.InternalName;
			switch (internalName)
			{
			default:
				goto IL_004f;
			case "cannot_be_damaged":
			case "bloodlines_necrovigil":
			case "god_mode":
				break;
			}
			goto IL_006c;
			IL_006c:
			return true;
			IL_004f:
			if (!(internalName == "shrine_godmode"))
			{
				continue;
			}
			goto IL_006c;
		}
		return false;
	}

	private static bool SkipThisMob(NetworkObject mob)
	{
		string metadata = mob.Metadata;
		return metadata == "Metadata/Monsters/LeagueIncursion/VaalSaucerBoss" || metadata.Contains("DoedreStonePillar");
	}

	private static bool IsEmerging(Monster mob)
	{
		if (((Actor)mob).GetStat((StatTypeGGG)861) == 1)
		{
			string metadata = ((NetworkObject)mob).Metadata;
			return metadata.Contains("/SandSpitterEmerge/") || metadata.Contains("/WaterElemental/") || metadata.Contains("/RootSpiders/") || metadata.Contains("ZombieMiredGraspEmerge") || metadata.Contains("ReliquaryMonsterEmerge");
		}
		return false;
	}

	private bool SkipThisTransition(AreaTransition t)
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		string name = ((NetworkObject)t).Name;
		string metadata = ((NetworkObject)t).Metadata;
		WalkablePosition walkablePosition = ((NetworkObject)(object)t).WalkablePosition();
		if (!metadata.Contains("Metadata/Terrain/Leagues/Sanctum/Objects/SanctumWorldTransition"))
		{
			if (!name.Contains(" Hideout"))
			{
				switch (name)
				{
				case "Abyssal Depths":
					GlobalLog.Warn($"[CombatAreaCache] Skipping Abyss transition {walkablePosition}");
					return true;
				case "Area Transition":
					if (t.Destination == null)
					{
						GlobalLog.Debug($"[CombatAreaCache] Skipping dummy area transition ({walkablePosition})");
						return true;
					}
					goto default;
				default:
					if (!(name == MapNames.Caldera))
					{
						if (name == "Tomb" && Current.WorldArea.Name == "Barrows")
						{
							GlobalLog.Debug($"[CombatAreaCache] Skipping Tomb area transition ({walkablePosition})");
							return true;
						}
						if ((int)t.TransitionType == 1 && !((NetworkObject)t).Metadata.Contains("IncursionPortal"))
						{
							if (WorldArea.Name == MapNames.Laboratory && metadata.Contains("PortalToggleableSmall_Pair"))
							{
								GlobalLog.Debug($"[CombatAreaCache] Skipping {walkablePosition} because it leads to the same level.");
								return true;
							}
							if (WorldArea.Id == World.Act9.RottingCore.Id)
							{
								if (metadata == "Metadata/QuestObjects/Act9/HarvestFinalBossTransition")
								{
									GlobalLog.Debug($"[CombatAreaCache] Skipping {walkablePosition} because because it is unlocked by a quest.");
									return true;
								}
								if (metadata.Contains("BellyArenaTransition"))
								{
									GlobalLog.Debug($"[CombatAreaCache] Skipping {walkablePosition} because because it is not a pathfinding obstacle.");
									return true;
								}
							}
						}
						return false;
					}
					GlobalLog.Debug($"[CombatAreaCache] Skip this transition (Caldera tweak) ({walkablePosition})");
					return true;
				case "The Sacred Grove":
				{
					bool flag = !GeneralSettings.Instance.EnableHarvest;
					if (AreaTransitions.All((CachedTransition cached) => ((NetworkObject)t).Position != (Vector2i)cached.Position))
					{
						Utility.BroadcastMessage((object)this, "harvest_found", new object[1] { $"Harvest found on {WorldArea.Name}. {walkablePosition}. Skip: {flag}" });
					}
					return flag;
				}
				}
			}
			GlobalLog.Warn($"[CombatAreaCache] Skipping Hideout transition {walkablePosition}");
			return true;
		}
		GlobalLog.Warn($"[CombatAreaCache] Skipping Sanctum league transition {walkablePosition}");
		return true;
	}

	private void TweakTransition(CachedTransition t)
	{
		string name = t.Position.Name;
		string name2 = WorldArea.Name;
		if (name2 == MapNames.Villa && (name == MapNames.Villa || name == "Arena"))
		{
			GlobalLog.Debug("[CombatAreaCache] Marking this area transition as unwalkable (Villa tweak)");
			t.Unwalkable = true;
			return;
		}
		if (name2 == MapNames.Caldera && name == "Portal" && ObjectManager.Objects.Count((NetworkObject o) => o.AnimatedPropertiesMetadata.Contains("Town_Portals/KaomPortal/KaomPortal")) < 2)
		{
			GlobalLog.Debug("[CombatAreaCache] Marking this area transition as back transition (Caldera tweak)");
			t.LeadsBack = true;
		}
		if (name2 == MapNames.Summit && name == MapNames.Summit)
		{
			GlobalLog.Debug("[CombatAreaCache] Marking this area transition as back transition (Summit tweak)");
			t.LeadsBack = true;
		}
	}

	private static bool IsSpecialChest(NetworkObject chest)
	{
		string metadata = chest.Metadata;
		if (hashSet_0.Contains(metadata))
		{
			return true;
		}
		if (!metadata.Contains("AbyssFinalChest"))
		{
			if (!metadata.Contains("Chests/League"))
			{
				if (!metadata.Contains("Chests/Blight"))
				{
					if (!metadata.Contains("/PerandusChests/"))
					{
						if (!metadata.Contains("IncursionChest"))
						{
							return false;
						}
						return true;
					}
					return true;
				}
				return true;
			}
			return true;
		}
		return true;
	}

	private static void OnLocalTransitionEntered()
	{
		GlobalLog.Info("[CombatAreaCache] Resetting unwalkable flags on all cached objects.");
		CombatAreaCache current = Current;
		foreach (CachedWorldItem item in current.Items)
		{
			item.Unwalkable = false;
		}
		foreach (CachedMonster monster in current.Monsters)
		{
			monster.Unwalkable = false;
		}
		foreach (CachedObject chest in current.Chests)
		{
			chest.Unwalkable = false;
		}
		foreach (CachedObject item2 in current.CraftingRecipe)
		{
			item2.Unwalkable = false;
		}
		foreach (CachedObject specialChest in current.SpecialChests)
		{
			specialChest.Unwalkable = false;
		}
		foreach (CachedStrongbox strongbox in current.Strongboxes)
		{
			strongbox.Unwalkable = false;
		}
		foreach (CachedObject shrine in current.Shrines)
		{
			shrine.Unwalkable = false;
		}
		foreach (CachedTransition areaTransition in current.AreaTransitions)
		{
			areaTransition.Unwalkable = false;
		}
	}

	private static void OnItemEvaluatorRefresh(object sender, ItemEvaluatorRefreshedEventArgs e)
	{
		if (dictionary_0.TryGetValue(LocalData.AreaHash, out var value))
		{
			GlobalLog.Info("[CombatAreaCache] Clearing processed items.");
			value.hashSet_1.Clear();
		}
	}
}
