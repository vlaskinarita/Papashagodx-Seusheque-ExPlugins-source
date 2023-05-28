using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.ExtraContent;

public class HandleHarvestTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static CachedHarvestIrrigator cachedHarvestIrrigator_0;

	private static bool bool_0;

	private static Stopwatch stopwatch_0;

	private static readonly Dictionary<string, int> dictionary_0;

	private static Npc Oshabi => ObjectManager.Objects.FirstOrDefault((Npc a) => ((NetworkObject)a).Name.Equals("Oshabi"));

	public string Name => "HandleHarvestTask";

	public string Description => "Task that handles harvest events on maps";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public void Tick()
	{
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.IsHideoutArea && !currentArea.IsTown)
		{
			if (bool_0 || !interval_0.Elapsed)
			{
				return;
			}
			if (!(stopwatch_0.Elapsed.TotalSeconds > 9.0))
			{
				if (CombatAreaCache.IsInHarvest)
				{
					if (!GeneralSettings.Instance.EnableHarvest)
					{
						return;
					}
					{
						foreach (CachedHarvestIrrigator item in CombatAreaCache.Current.HarvestIrrigators.Where((CachedHarvestIrrigator irrigator) => !irrigator.Visited && (NetworkObject)(object)irrigator.Object != (NetworkObject)null && irrigator.MobList.Any()).ToList())
						{
							item.Visited = true;
						}
						return;
					}
				}
				if ((NetworkObject)(object)Oshabi != (NetworkObject)null)
				{
					CombatAreaCache.IsInHarvest = true;
				}
				else if (ObjectManager.GetObjectsByType<AreaTransition>().Any((AreaTransition o) => ((NetworkObject)o).Metadata.Contains("HarvestPortalToggleableReverseReturn")))
				{
					CombatAreaCache.IsInHarvest = true;
				}
			}
			else
			{
				stopwatch_0.Reset();
				cachedHarvestIrrigator_0 = null;
			}
		}
		else
		{
			CombatAreaCache.IsInHarvest = false;
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (!(id == "MB_new_map_entered_event"))
		{
			return (MessageResult)1;
		}
		GlobalLog.Warn("[" + Name + "] Reset.");
		cachedHarvestIrrigator_0 = null;
		bool_0 = false;
		return (MessageResult)0;
	}

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame)
		{
			return false;
		}
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!area.IsHideoutArea && !area.IsTown)
		{
			CombatAreaCache combatAreaCache_0 = CombatAreaCache.Current;
			if (!GeneralSettings.Instance.EnableHarvest || bool_0)
			{
				if (CombatAreaCache.IsInHarvest)
				{
					GlobalLog.Warn("[" + Name + "] Harvest disabled, but we somehow entered it. Leaving now.");
					CachedTransition backTrans = combatAreaCache_0.AreaTransitions.FirstOrDefault((CachedTransition t) => t.Name.Equals(World.CurrentArea.Name));
					if (backTrans != null)
					{
						AreaTransition tranObj2 = backTrans.Object;
						if ((NetworkObject)(object)tranObj2 == (NetworkObject)null || ((NetworkObject)tranObj2).Distance > 20f)
						{
							return backTrans.Position.TryCome();
						}
						Vector2i vector2i_2 = LokiPoe.MyPosition;
						if (await PlayerAction.Interact((NetworkObject)(object)tranObj2, () => ((Vector2i)(ref vector2i_2)).Distance(LokiPoe.MyPosition) > 100, "transition"))
						{
							CombatAreaCache.IsInHarvest = false;
							bool_0 = true;
						}
					}
					else
					{
						CombatAreaCache.IsInHarvest = false;
					}
				}
				return false;
			}
			if (!CombatAreaCache.IsInHarvest)
			{
				CachedTransition harvestTrans = combatAreaCache_0.AreaTransitions.FirstOrDefault((CachedTransition t) => t.Type == TransitionType.Harvest);
				if (harvestTrans != null)
				{
					AreaTransition tranObj = harvestTrans.Object;
					if ((NetworkObject)(object)tranObj == (NetworkObject)null || ((NetworkObject)tranObj).Distance > 20f)
					{
						return harvestTrans.Position.TryCome();
					}
					Vector2i vector2i_ = LokiPoe.MyPosition;
					if (await PlayerAction.Interact((NetworkObject)(object)tranObj, () => ((Vector2i)(ref vector2i_)).Distance(LokiPoe.MyPosition) > 100, "transition"))
					{
						CombatAreaCache.IsInHarvest = true;
						return true;
					}
				}
				return false;
			}
			if (!(await TrackMobLogic.Execute(250)))
			{
				if (stopwatch_0.IsRunning)
				{
					if (cachedHarvestIrrigator_0 != null)
					{
						int rand = LokiPoe.Random.Next(-45, 45);
						Vector2i randVector = new Vector2i(cachedHarvestIrrigator_0.Position.AsVector.X + rand, cachedHarvestIrrigator_0.Position.AsVector.Y + rand);
						WalkablePosition randomWp = new WalkablePosition("random pos around extractor", randVector);
						randomWp.Initialize();
						randomWp.TryCome();
					}
					return true;
				}
				await PlayerAction.EnableAlwaysHighlight();
				if (!combatAreaCache_0.HarvestIrrigators.Any((CachedHarvestIrrigator i) => !i.Inactive))
				{
					if (!combatAreaCache_0.Explorer.BasicExplorer.HasLocation)
					{
						if (combatAreaCache_0.HarvestIrrigators.All((CachedHarvestIrrigator i) => i.Inactive || i.Unwalkable))
						{
							GlobalLog.Warn("[" + Name + "] No active extractors found. Leaving harvest");
							CachedTransition backTrans2 = combatAreaCache_0.AreaTransitions.FirstOrDefault((CachedTransition t) => t.Name.Equals(World.CurrentArea.Name));
							if (backTrans2 != null)
							{
								AreaTransition tranObj3 = backTrans2.Object;
								if ((NetworkObject)(object)tranObj3 == (NetworkObject)null || ((NetworkObject)tranObj3).Distance > 20f)
								{
									return backTrans2.Position.TryCome();
								}
								Vector2i vector2i_0 = LokiPoe.MyPosition;
								if (await PlayerAction.Interact((NetworkObject)(object)tranObj3, () => ((Vector2i)(ref vector2i_0)).Distance(LokiPoe.MyPosition) > 100, "transition"))
								{
									CombatAreaCache.IsInHarvest = false;
									bool_0 = true;
									return true;
								}
							}
						}
						return false;
					}
					await combatAreaCache_0.Explorer.Execute(basic: true);
					return true;
				}
				if (cachedHarvestIrrigator_0 == null)
				{
					List<CachedHarvestIrrigator> irrigatorList = combatAreaCache_0.HarvestIrrigators.Where((CachedHarvestIrrigator i) => !i.Unwalkable && !i.Inactive).ToList();
					cachedHarvestIrrigator_0 = (from i in WeightedIrrigators(irrigatorList)
						orderby i.Value descending
						select i).FirstOrDefault().Key;
					return true;
				}
				if (cachedHarvestIrrigator_0.Position.Distance <= 20)
				{
					if (cachedHarvestIrrigator_0.Object.IsUiVisible)
					{
						ProcessHookManager.ClearAllKeyStates();
						await Coroutines.FinishCurrentAction(true);
						cachedHarvestIrrigator_0.Object.Activate();
						return true;
					}
					HarvestExtraxtor harvestExtraxtor_0 = ObjectManager.GetObjectsByType<HarvestExtraxtor>().OrderBy(delegate(HarvestExtraxtor i)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						//IL_0006: Unknown result type (might be due to invalid IL or missing references)
						//IL_0014: Unknown result type (might be due to invalid IL or missing references)
						Vector2i position = ((NetworkObject)i).Position;
						return ((Vector2i)(ref position)).Distance((Vector2i)cachedHarvestIrrigator_0.Position);
					}).FirstOrDefault();
					if (!((NetworkObject)(object)harvestExtraxtor_0 != (NetworkObject)null) || !harvestExtraxtor_0.IsUiVisible)
					{
						GlobalLog.Error("Extractor null");
						cachedHarvestIrrigator_0 = null;
					}
					else
					{
						WalkablePosition pos = ((NetworkObject)(object)harvestExtraxtor_0).WalkablePosition();
						if (pos.Distance > 20)
						{
							await pos.TryComeAtOnce(10);
						}
						ProcessHookManager.ClearAllKeyStates();
						await Coroutines.FinishCurrentAction(true);
						harvestExtraxtor_0.Activate();
						if (await Wait.For(() => !harvestExtraxtor_0.IsUiVisible, "extractor activation"))
						{
							await PlayerAction.MoveAway(35, 65);
							await Wait.For(() => combatAreaCache_0.Monsters.Any(), "seeds to grow", 150, 5000);
							stopwatch_0 = Stopwatch.StartNew();
						}
					}
					return true;
				}
				if (!(await cachedHarvestIrrigator_0.Position.TryComeAtOnce(10)))
				{
					GlobalLog.Error($"[{Name}] {cachedHarvestIrrigator_0.Position} is unwalkable!");
					cachedHarvestIrrigator_0.Unwalkable = true;
					cachedHarvestIrrigator_0 = null;
				}
				return true;
			}
			return true;
		}
		return false;
	}

	private static IEnumerable<KeyValuePair<CachedHarvestIrrigator, int>> WeightedIrrigators(List<CachedHarvestIrrigator> irrigators)
	{
		List<KeyValuePair<CachedHarvestIrrigator, int>> list = new List<KeyValuePair<CachedHarvestIrrigator, int>>();
		foreach (CachedHarvestIrrigator irrigator in irrigators)
		{
			if (irrigator.Inactive)
			{
				continue;
			}
			int num = 0;
			foreach (var (num3, text2) in irrigator.MobList)
			{
				dictionary_0.TryGetValue(text2, out var value);
				double num4 = 0.0;
				int num5 = (value = Convert.ToInt32(Math.Pow(value, 2.0)));
				if (value == 4)
				{
					num += 2000 * num3;
				}
				if (text2.Contains("Vivid"))
				{
					num4 = 1.15 * (double)num5;
				}
				if (text2.Contains("Wild"))
				{
					num4 = 1.05 * (double)num5;
				}
				if (text2.Contains("Primal"))
				{
					num4 = 1.0 * (double)num5;
				}
				num += Convert.ToInt32((double)num3 * num4);
			}
			list.Add(new KeyValuePair<CachedHarvestIrrigator, int>(irrigator, num));
		}
		list = list.OrderByDescending((KeyValuePair<CachedHarvestIrrigator, int> i) => i.Value).ToList();
		foreach (KeyValuePair<CachedHarvestIrrigator, int> item in list)
		{
			GlobalLog.Warn($"[WeightedIrrigators] Adding: {item.Key.Position} with weight: {item.Value}");
		}
		return list;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static HandleHarvestTask()
	{
		interval_0 = new Interval(250);
		stopwatch_0 = new Stopwatch();
		dictionary_0 = new Dictionary<string, int>
		{
			["Wild Ursaling"] = 1,
			["Wild Hellion"] = 1,
			["Wild Thornwolf"] = 1,
			["Wild Ape"] = 1,
			["Wild Hatchling"] = 1,
			["Vivid Arachnid"] = 1,
			["Vivid Weta"] = 1,
			["Vivid Leech"] = 1,
			["Vivid Scorpion"] = 1,
			["Vivid Thornweaver"] = 1,
			["Primal Rhoa"] = 1,
			["Primal Dustspitter"] = 1,
			["Primal Feasting Horror"] = 1,
			["Primal Maw"] = 1,
			["Primal Cleaveling"] = 1,
			["Wild Bristlebeast"] = 2,
			["Wild Snap Hound"] = 2,
			["Wild Homunculus"] = 2,
			["Wild Chieftain"] = 2,
			["Wild Spikeback"] = 2,
			["Vivid Razorleg"] = 2,
			["Vivid Sapsucker"] = 2,
			["Vivid Parasite"] = 2,
			["Vivid Striketail"] = 2,
			["Vivid Nestback"] = 2,
			["Primal Rhex"] = 2,
			["Primal Dustcrab"] = 2,
			["Primal Viper"] = 2,
			["Primal Chimeral"] = 2,
			["Primal Scrabbler"] = 2,
			["Wild Bristle Matron"] = 3,
			["Wild Hellion Alpha"] = 3,
			["Wild Thornmaw"] = 3,
			["Wild Brambleback"] = 3,
			["Wild Infestation Queen"] = 3,
			["Vivid Whipleg"] = 3,
			["Vivid Watcher"] = 3,
			["Vivid Vulture"] = 3,
			["Vivid Abberarach"] = 3,
			["Vivid Devourer"] = 3,
			["Primal Rhex Matriarch"] = 3,
			["Primal Crushclaw"] = 3,
			["Primal Blisterlord"] = 3,
			["Primal Cystcaller"] = 3,
			["Primal Reborn"] = 3,
			["Ersi, Mother of Thorns"] = 4,
			["Namharim, Born of Night"] = 4,
			["Janaar, the Omen"] = 4
		};
	}
}
