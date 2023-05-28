using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.TrialHandler;

public class TrialSolverTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class21
	{
		[CompilerGenerated]
		private readonly int int_0;

		[CompilerGenerated]
		private readonly Vector2i vector2i_0;

		[CompilerGenerated]
		private bool bool_0;

		public int Id
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
		}

		public Vector2i Position
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return vector2i_0;
			}
		}

		public bool IsTargetable
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

		public Class21(int id, Vector2i position, bool isTargetable)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			int_0 = id;
			vector2i_0 = position;
			IsTargetable = isTargetable;
		}
	}

	public static readonly List<string> TrialAreaIDs;

	public static readonly Dictionary<string, TgtPosition> AreaIDtoPlaque;

	public static readonly Dictionary<string, int> AttemptsInCurrentArea;

	private readonly Interval interval_0 = new Interval(500);

	public static TgtPosition TrialPlaqueTgt
	{
		get
		{
			return CombatAreaCache.Current.Storage["PlaqueTgt"] as TgtPosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["PlaqueTgt"] = value;
		}
	}

	private static List<Class21> CachedTrialLevers
	{
		get
		{
			if (CombatAreaCache.Current.Storage["CachedLevers"] is List<Class21> result)
			{
				return result;
			}
			object obj = (CombatAreaCache.Current.Storage["CachedLevers"] = new List<Class21>());
			return obj as List<Class21>;
		}
	}

	private static NetworkObject TrialPlaque => ObjectManager.Objects.FirstOrDefault((NetworkObject o) => o.Metadata.Contains("LabyrinthTrialPlaque"));

	private static NetworkObject TrialReturnPortal => ObjectManager.Objects.FirstOrDefault((NetworkObject o) => o.Metadata.Contains("LabyrinthTrialReturnPortal") && o.IsTargetable && o.PathDistance() < 120f);

	private static bool TrialCompleted
	{
		get
		{
			object obj = CombatAreaCache.Current.Storage["Trial Completed"];
			return obj is bool && (bool)obj;
		}
		set
		{
			CombatAreaCache.Current.Storage["Trial Completed"] = value;
		}
	}

	private static WalkablePosition CachedWaypointPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["WaypointPos"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["WaypointPos"] = value;
		}
	}

	public string Name => "TrialSolverTask";

	public string Author => "Seusheque";

	public string Description => "TrialSolverTask";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame || ((Actor)LokiPoe.Me).IsDead || !LokiPoe.CurrentWorldArea.IsCombatArea)
		{
			return false;
		}
		if (AreaContainsTrial())
		{
			if (PluginManager.EnabledPlugins.Any((IPlugin p) => ((IAuthored)p).Name == "DeadlyTrials"))
			{
				return false;
			}
			if (!TrialCompleted)
			{
				if (LokiPoe.CurrentWorldArea.Id == "2_10_9")
				{
					AreaTransition bonePits = ObjectManager.AreaTransition("The Bone Pits");
					if ((NetworkObject)(object)bonePits != (NetworkObject)null && ((NetworkObject)(object)bonePits).PathDistance() < 150f)
					{
						AreaTransition transition = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => ((NetworkObject)a).Name == "The Bone Pits"));
						await PlayerAction.TakeTransition(transition);
						TgtPosition trialPlaque = new TgtPosition("Trial Plaque", "Ossuary_thickwall_to_thinwall_corner_v01_01.tgt");
						if (trialPlaque.Initialize())
						{
							TrialPlaqueTgt = trialPlaque;
							return true;
						}
						return false;
					}
				}
				if (!(await OpenWaypoint()))
				{
					WalkablePosition leverWalkablePosition = new WalkablePosition("dummy vector", Vector2i.Zero);
					Class21 cachedLeverBath = (from l in CachedTrialLevers
						where l.IsTargetable && ExilePather.PathExistsBetween(((NetworkObject)LokiPoe.Me).Position, l.Position, false)
						orderby l.Id
						select l).FirstOrDefault();
					Class21 cachedLeverCommon = (from l in CachedTrialLevers
						where l.IsTargetable && ExilePather.PathExistsBetween(((NetworkObject)LokiPoe.Me).Position, l.Position, false)
						orderby ExilePather.PathDistance(((NetworkObject)LokiPoe.Me).Position, l.Position, false, false)
						select l).FirstOrDefault();
					Class21 class21_0 = ((!World.Act8.BathHouse.IsCurrentArea) ? cachedLeverCommon : cachedLeverBath);
					if (class21_0 == null)
					{
						class21_0 = (from l in CachedTrialLevers
							where l.IsTargetable && ExilePather.PathExistsBetween(((NetworkObject)LokiPoe.Me).Position, ExilePather.FastWalkablePositionFor(l.Position, 30, true), false)
							orderby ExilePather.PathDistance(((NetworkObject)LokiPoe.Me).Position, l.Position, false, false)
							select l).FirstOrDefault();
						if (class21_0 != null)
						{
							leverWalkablePosition = new WalkablePosition("lever position", class21_0.Position);
						}
					}
					else
					{
						leverWalkablePosition = new WalkablePosition("lever position", class21_0.Position);
					}
					if (class21_0 != null)
					{
						if (!(leverWalkablePosition.PathDistance > 30f))
						{
							NetworkObject leverBath = ObjectManager.GetObjectsByType<NetworkObject>().FirstOrDefault((NetworkObject l) => l.Id == class21_0.Id && l.Name == "Lever");
							NetworkObject leverCommon = ObjectManager.GetObjectsByType<NetworkObject>().FirstOrDefault((NetworkObject l) => l.Id == class21_0.Id);
							NetworkObject networkObject_0 = (World.Act8.BathHouse.IsCurrentArea ? leverBath : leverCommon);
							if (!(networkObject_0 != (NetworkObject)null))
							{
								CachedTrialLevers.Remove(class21_0);
								return true;
							}
							if (await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "lever interaction"))
							{
								class21_0.IsTargetable = false;
								ExilePather.PolyPathfinder.ClearObstacles();
								Blacklist.Clear();
								foreach (CachedObject recipe in CombatAreaCache.Current.CraftingRecipe.Where((CachedObject r) => r.Unwalkable))
								{
									recipe.Unwalkable = false;
								}
								return true;
							}
							return true;
						}
						leverWalkablePosition.TryCome();
						return true;
					}
					TriggerableBlockage lockedDoor = (from o in ObjectManager.GetObjectsByType<TriggerableBlockage>()
						where ((NetworkObject)o).Metadata.Contains("Puzzle_Parts/Door_Closed") && !o.IsOpened && ExilePather.PathExistsBetween(((NetworkObject)LokiPoe.Me).Position, ((NetworkObject)o).Position, false)
						orderby ((NetworkObject)(object)o).PathDistance()
						select o).FirstOrDefault();
					if (!((NetworkObject)(object)lockedDoor != (NetworkObject)null) || !((RemoteMemoryObject)(object)((NetworkObject)lockedDoor).Components.TransitionableComponent != (RemoteMemoryObject)null) || ((NetworkObject)lockedDoor).Components.TransitionableComponent.Flag1 != 2 || !(((NetworkObject)lockedDoor).Distance > 20f))
					{
						if (TrialPlaque != (NetworkObject)null && ExilePather.PathExistsBetween(((NetworkObject)LokiPoe.Me).Position, TrialPlaque.Position, false))
						{
							if (TrialPlaque.Distance > 20f)
							{
								Move.Towards(TrialPlaque.Position, TrialPlaque.Name);
								return true;
							}
							if (!(await PlayerAction.Interact(TrialPlaque, 3)))
							{
								return true;
							}
							TrialCompleted = true;
							return true;
						}
						if (!(TrialPlaqueTgt != null) || !TrialPlaqueTgt.PathExists || TrialPlaqueTgt.Distance <= 30)
						{
							if (TrialPlaqueTgt != null)
							{
								TrialPlaqueTgt.ProceedToNext();
								return true;
							}
							if (!(await CombatAreaCache.Current.Explorer.Execute()))
							{
								GlobalLog.Error("[" + Name + "] Can't pathfind to Trial Plaque! Skipping this trial.");
								TrialCompleted = true;
								global::ExPlugins.EXtensions.EXtensions.AbandonCurrentArea();
								return false;
							}
							return true;
						}
						TrialPlaqueTgt.TryCome();
						return true;
					}
					Move.TowardsWalkable(((NetworkObject)lockedDoor).Position, ((NetworkObject)lockedDoor).Name);
					return true;
				}
				return true;
			}
			if (QuestBotSettings.Instance.CurrentQuestName == null || QuestBotSettings.Instance.CurrentQuestName == "Grinding" || QuestBotSettings.Instance.CurrentQuestName == "Travel")
			{
				WorldPosition walkablePos = WorldPosition.FindPathablePositionAtDistance(30, 150, 5);
				await Move.AtOnce(walkablePos, "portal position");
				await PlayerAction.TpToTown();
				return true;
			}
			if (!(TrialReturnPortal != (NetworkObject)null))
			{
				if (!((Player)LokiPoe.Me).IsAscendencyTrialCompleted(LokiPoe.CurrentWorldArea.Id))
				{
					if (World.CurrentArea.IsMap || World.CurrentArea.IsMapTrialArea)
					{
						return false;
					}
					Travel.RequestNewInstance(World.CurrentArea);
					await PlayerAction.TpToTown();
					return true;
				}
				return false;
			}
			NetworkObject portal = TrialReturnPortal;
			if (!(portal.Distance <= 30f))
			{
				if (!Move.Towards(portal.Position, portal.Name))
				{
					await PlayerAction.TpToTown();
				}
				return true;
			}
			Vector2i vector2i_0 = LokiPoe.MyPosition;
			await PlayerAction.Interact(portal, 3);
			await Wait.For(() => LokiPoe.MyPosition != vector2i_0, "teleport to enterance", 200, 2000);
			return true;
		}
		return false;
	}

	public void Tick()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if (!interval_0.Elapsed || !AreaContainsTrial())
		{
			return;
		}
		IEnumerable<NetworkObject> enumerable = from o in ObjectManager.GetObjectsByType<NetworkObject>()
			where (o.Metadata.EndsWith("Switch_Once") || o.Metadata.EndsWith("Switch_Once_Multi")) && o.IsTargetable
			select o;
		foreach (NetworkObject networkObject_0 in enumerable)
		{
			Class21 @class = CachedTrialLevers.FirstOrDefault((Class21 l) => l.Id == networkObject_0.Id);
			if (@class == null)
			{
				CachedTrialLevers.Add(new Class21(networkObject_0.Id, networkObject_0.Position, networkObject_0.IsTargetable));
			}
			else
			{
				@class.IsTargetable = networkObject_0.IsTargetable;
			}
		}
		foreach (Monster item in from m in ObjectManager.GetObjectsByType<Monster>()
			where ((NetworkObject)m).Metadata.Contains("LabyrinthPopUpTotem")
			select m)
		{
			Blacklist.Add(((NetworkObject)item).Id, TimeSpan.FromHours(1.0), "");
		}
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

	public static bool AreaContainsTrial()
	{
		if (TrialAreaIDs.Contains(LokiPoe.CurrentWorldArea.Id))
		{
			return !((Player)LokiPoe.Me).IsAscendencyTrialCompleted(LokiPoe.CurrentWorldArea.Id);
		}
		return false;
	}

	private static async Task<bool> OpenWaypoint()
	{
		if (World.CurrentArea.HasWaypoint && !World.IsWaypointOpened(World.CurrentArea.Id))
		{
			if (CachedWaypointPos != null)
			{
				if (CachedWaypointPos.Distance <= 30)
				{
					if (!(await PlayerAction.OpenWaypoint()))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				CachedWaypointPos.Come();
				return true;
			}
			WorldPosition pos = Tgt.FindWaypoint();
			if (pos != null)
			{
				CachedWaypointPos = new WalkablePosition("Waypoint location", pos);
			}
		}
		return false;
	}

	static TrialSolverTask()
	{
		TrialAreaIDs = new List<string>
		{
			"1_1_7_1", "1_2_5_1", "1_2_6_2", "1_3_3_1", "1_3_6", "1_3_15", "2_6_7_1", "2_7_4", "2_7_5_2", "2_8_5",
			"2_9_7", "2_10_9"
		};
		AreaIDtoPlaque = new Dictionary<string, TgtPosition>
		{
			{
				"1_1_7_1",
				new TgtPosition("Plaque", "dungeon_prison_stairs_convex_v01_01.tgt")
			},
			{
				"1_2_5_1",
				new TgtPosition("Plaque", "dungeon_church_stairs_to_abyss_edge_v01_02.tgt")
			},
			{
				"1_2_6_2",
				new TgtPosition("Plaque", "templeruinforest_stairs_convex_v01_01.tgt")
			},
			{
				"1_3_3_1",
				new TgtPosition("Plaque", "dugeon_prison_lava_fence_concave_v01_01.tgt")
			},
			{
				"1_3_6",
				new TgtPosition("Plaque", "dungeon_church_floor_height_convex_stairs_v01_02.tgt")
			},
			{
				"1_3_15",
				new TgtPosition("Plaque", "garden_lowwall_v03_01.tgt")
			},
			{
				"2_6_7_1",
				new TgtPosition("Plaque", "dungeon_prison_stairs_convex_v01_01.tgt")
			},
			{
				"2_7_4",
				new TgtPosition("Plaque", "dungeonchurch_fence_crevice_stairs_v01_01.tgt")
			},
			{
				"2_7_5_2",
				new TgtPosition("Plaque", "templeruinforest_stairs_convex_v01_01.tgt")
			},
			{
				"2_8_5",
				new TgtPosition("Plaque", "eleganthouse_ledge_lava_straight_to_wall_v01_01.tgt")
			},
			{
				"2_9_7",
				new TgtPosition("Plaque", "mine_labyrinthtrial_doorframe_v01_01.tgt")
			},
			{
				"2_10_9",
				new TgtPosition("Plaque", "Ossuary_thickwall_to_thinwall_corner_v01_01.tgt")
			}
		};
		AttemptsInCurrentArea = new Dictionary<string, int>();
	}
}
