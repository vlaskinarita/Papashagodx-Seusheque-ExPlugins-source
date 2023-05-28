using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.EXtensions.Global;

public class ComplexExplorer
{
	private class Class43
	{
		public readonly Func<ExplorationSettings> func_0;

		public readonly string string_0;

		public readonly ProviderPriority Priority;

		public Class43(string ownerId, Func<ExplorationSettings> getSettings, ProviderPriority priority)
		{
			string_0 = ownerId;
			func_0 = getSettings;
			Priority = priority;
		}
	}

	public const string LocalTransitionEnteredMessage = "explorer_local_transition_entered_event";

	private static readonly List<string> list_0;

	private static readonly Interval interval_0;

	private static readonly List<Class43> list_1;

	private readonly GridExplorer gridExplorer_0;

	private readonly Dictionary<WorldPosition, GridExplorer> dictionary_0;

	private bool bool_0;

	private WorldPosition worldPosition_0;

	private WorldPosition worldPosition_1;

	private CachedTransition cachedTransition_0;

	[CompilerGenerated]
	private readonly ExplorationSettings explorationSettings_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private static Action action_0;

	public ExplorationSettings Settings
	{
		[CompilerGenerated]
		get
		{
			return explorationSettings_0;
		}
	}

	public bool Finished
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

	public GridExplorer BasicExplorer => Settings.BasicExploration ? gridExplorer_0 : dictionary_0[worldPosition_0];

	private CachedTransition FrontTransition
	{
		get
		{
			List<CachedTransition> source = CombatAreaCache.Current.AreaTransitions.FindAll((CachedTransition t) => !t.Ignored && !t.Unwalkable);
			CachedTransition cachedTransition = Enumerable.FirstOrDefault(source, (CachedTransition t) => list_0.Contains(t.Position.Name) || ((NetworkObject)(object)t.Object != (NetworkObject)null && ((NetworkObject)t.Object).Metadata.Contains("ElderPortal")));
			if (!(cachedTransition != null))
			{
				if (Settings.PriorityTransition != null)
				{
					CachedTransition cachedTransition2 = Enumerable.FirstOrDefault(source, (CachedTransition t) => !t.Unwalkable && t.Type == TransitionType.Local && !t.Visited && !t.LeadsBack && t.Position.Name == Settings.PriorityTransition);
					if (cachedTransition2 != null)
					{
						return cachedTransition2;
					}
				}
				return CombatAreaCache.Current.AreaTransitions.ClosestValid((CachedTransition t) => t.Type == TransitionType.Local && !t.Visited && !t.LeadsBack && (!t.Position.Name.Contains("Hideout") || !ExtensionsSettings.Instance.IgnoreHidoutAreaTransitions));
			}
			return cachedTransition;
		}
	}

	private static CachedTransition BackTransition => CombatAreaCache.Current.AreaTransitions.OrderByDescending((CachedTransition t) => t.LeadsBack).ClosestValid((CachedTransition t) => t.Type == TransitionType.Local && !t.Visited && (!t.Position.Name.Contains("Hideout") || !ExtensionsSettings.Instance.IgnoreHidoutAreaTransitions));

	public static event Action LocalTransitionEntered
	{
		[CompilerGenerated]
		add
		{
			Action action = action_0;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = action_0;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public ComplexExplorer()
	{
		explorationSettings_0 = ProvideSettings();
		Settings.LogProperties();
		if (Settings.BasicExploration)
		{
			gridExplorer_0 = CreateExplorer();
		}
		else
		{
			dictionary_0 = new Dictionary<WorldPosition, GridExplorer>();
			InitNewLevel();
		}
		Events.NewTransitionCached += delegate
		{
			Finished = false;
		};
	}

	public async Task<bool> Execute(bool basic = false)
	{
		if (!Finished)
		{
			if (Settings.BasicExploration || basic)
			{
				if (BasicExploration())
				{
					return true;
				}
				Finished = true;
			}
			else
			{
				if (await ComplexExploration())
				{
					return true;
				}
				Finished = true;
			}
			return true;
		}
		return false;
	}

	private bool BasicExploration()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		GridExplorer basicExplorer = BasicExplorer;
		if (basicExplorer.HasLocation)
		{
			Vector2i location = basicExplorer.Location;
			if (interval_0.Elapsed)
			{
				Vector2i myPosition = LokiPoe.MyPosition;
				int num = ((Vector2i)(ref myPosition)).Distance(location);
				double num2 = Math.Round(basicExplorer.PercentComplete, 1);
				GlobalLog.Debug($"[ComplexExplorer] Exploring to the location {location} ({num}) [{num2} %].");
			}
			if (!PlayerMoverManager.MoveTowards(location, (object)null))
			{
				GlobalLog.Error($"[ComplexExplorer] MoveTowards failed for {location}. Adding this location to ignore list.");
				basicExplorer.Ignore(location);
			}
			return true;
		}
		return false;
	}

	private async Task<bool> ComplexExploration()
	{
		if (AtlasHelper.IsAtlasBossPresent && Enumerable.Any(ObjectManager.Objects, (NetworkObject o) => o.Metadata.Contains("GlyphSirus")))
		{
			CachedTransition conqTrans = Enumerable.FirstOrDefault(CombatAreaCache.Current.AreaTransitions, (CachedTransition a) => a.Type == TransitionType.Local && !a.Ignored && list_0.Contains(a.Name));
			if (conqTrans != null)
			{
				conqTrans.Visited = true;
				conqTrans.Ignored = true;
			}
			AreaTransition backTrans = ObjectManager.GetObjectsByType<AreaTransition>().Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition t) => ((NetworkObject)t).Name.Equals(World.CurrentArea.Name) && ((NetworkObject)t).Distance < 80f));
			while ((NetworkObject)(object)backTrans == (NetworkObject)null)
			{
				GlobalLog.Debug("[ComplexExplorer] Waiting for back transition to appear");
				backTrans = ObjectManager.GetObjectsByType<AreaTransition>().Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition t) => ((NetworkObject)t).Name.Equals(World.CurrentArea.Name) && ((NetworkObject)t).Distance < 100f));
				await Wait.StuckDetectionSleep(1000);
			}
			CachedTransition cache = new CachedTransition(((NetworkObject)backTrans).Id, ((NetworkObject)(object)backTrans).WalkablePosition(), TransitionType.Local, backTrans.Destination)
			{
				Visited = false,
				LeadsBack = true
			};
			CombatAreaCache.Current.AreaTransitions.Add(cache);
			cachedTransition_0 = cache;
			await EnterTransition();
			return true;
		}
		if (worldPosition_1.Distance >= 100 && !worldPosition_1.PathExists)
		{
			GlobalLog.Error("[ComplexExplorer] Cannot pathfind to my last position. Now resetting current level and all visited transitions.");
			HandleExternalJump();
		}
		worldPosition_1 = new WorldPosition(LokiPoe.MyPosition);
		string trans = FrontTransition?.Name;
		if (Settings.FastTransition)
		{
			if (cachedTransition_0 != null || (cachedTransition_0 = FrontTransition) != null)
			{
				await EnterTransition();
				return true;
			}
		}
		else if (trans == Settings.PriorityTransition || list_0.Contains(trans))
		{
			cachedTransition_0 = FrontTransition;
			if (cachedTransition_0 != null || (cachedTransition_0 = FrontTransition) != null)
			{
				await EnterTransition();
				return true;
			}
		}
		if (BasicExploration())
		{
			return true;
		}
		if (!(cachedTransition_0 != null))
		{
			cachedTransition_0 = FrontTransition;
			if (!(cachedTransition_0 != null))
			{
				bool prioVisited = Enumerable.Any(CombatAreaCache.Current.AreaTransitions, (CachedTransition a) => a.Type == TransitionType.Local && !a.LeadsBack && a.Visited && a.Name.Equals(Settings.PriorityTransition));
				bool conqVisited = Enumerable.Any(CombatAreaCache.Current.AreaTransitions, (CachedTransition a) => a.Type == TransitionType.Local && a.Visited && list_0.Contains(a.Name));
				if ((Settings.Backtracking || conqVisited || prioVisited) && (cachedTransition_0 = BackTransition) != null)
				{
					return true;
				}
				GlobalLog.Warn("[ComplexExplorer] Out of area transitions. Now finishing the exploration.");
				return false;
			}
			return true;
		}
		await EnterTransition();
		return true;
	}

	private async Task EnterTransition()
	{
		WalkablePosition pos = cachedTransition_0.Position;
		if (pos.IsFar)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Debug($"[ComplexExplorer] Fail to move to {pos}. Marking this transition as unwalkable.");
				cachedTransition_0.Unwalkable = true;
				cachedTransition_0 = null;
			}
			return;
		}
		if (ConfigManager.IsAlwaysHighlightEnabled)
		{
			GlobalLog.Info("[DisableAlwaysHiglight] Now disabling Always Highlight to avoid transition issues.");
			await PlayerAction.DisableAlwaysHighlight();
		}
		AreaTransition transitionObj = cachedTransition_0.Object;
		if (!((NetworkObject)(object)transitionObj == (NetworkObject)null))
		{
			if (((NetworkObject)transitionObj).IsTargetable)
			{
				string transitionName = ((NetworkObject)transitionObj).Name;
				if (list_0.Contains(transitionName) || ((NetworkObject)transitionObj).Metadata.Contains("ElderPortal"))
				{
					Portal portal = ObjectManager.GetObjectByType<Portal>();
					int num;
					if (!((NetworkObject)(object)portal == (NetworkObject)null))
					{
						Vector2i position = ((NetworkObject)portal).Position;
						num = ((((Vector2i)(ref position)).Distance(((NetworkObject)transitionObj).Position) > 40) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					if (num != 0 && (NetworkObject)(object)PlayerAction.AnyPortalInRangeOf(200) == (NetworkObject)null)
					{
						GlobalLog.Info("[ComplexExplorer] Opening a portal before entering atlas boss area");
						WorldPosition walkablePos = WorldPosition.FindPathablePositionAtDistance(45, 150, 30);
						await Move.AtOnce(walkablePos, "portal position");
						await Coroutines.FinishCurrentAction(true);
						await PlayerAction.CreateTownPortal();
					}
				}
				bool_0 = true;
				if (await PlayerAction.TakeTransition(transitionObj))
				{
					PostEnter();
					action_0?.Invoke();
					Utility.BroadcastMessage((object)this, "explorer_local_transition_entered_event", Array.Empty<object>());
					if (Settings.OpenPortals && transitionName.Equals(Settings.PriorityTransition))
					{
						GlobalLog.Info("[ComplexExplorer] Opening a portal.");
						await PlayerAction.CreateTownPortal();
					}
				}
				else
				{
					int attempts = ++cachedTransition_0.InteractionAttempts;
					GlobalLog.Error($"[ComplexExplorer] Fail to enter {pos}. Attempt: {attempts}/{5}");
					if (attempts >= 5)
					{
						GlobalLog.Error("[ComplexExplorer] All attempts to enter an area transition have been spent. Now ignoring it.");
						cachedTransition_0.Ignored = true;
						cachedTransition_0 = null;
					}
					else
					{
						await Wait.SleepSafe(500);
					}
				}
				bool_0 = false;
				return;
			}
			bool flag = ((NetworkObject)transitionObj).Metadata.Contains("sarcophagus_transition");
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await HandleSarcophagus();
			}
			if (!flag2 || !((NetworkObject)transitionObj.Fresh<AreaTransition>()).IsTargetable)
			{
				if (cachedTransition_0.InteractionAttempts < 5)
				{
					GlobalLog.Debug(string.Format(arg1: ++cachedTransition_0.InteractionAttempts, format: "[ComplexExplorer] Waiting for \"{0}\" to become targetable ({1}/{2})", arg0: pos.Name, arg2: 5));
					await Wait.SleepSafe(1000);
				}
				else
				{
					GlobalLog.Error("[ComplexExplorer] Area transition did not become targetable. Now ignoring it.");
					cachedTransition_0.Ignored = true;
					cachedTransition_0 = null;
				}
			}
		}
		else
		{
			GlobalLog.Error("[ComplexExplorer] Unknown error. There is no transition near cached position.");
			cachedTransition_0.Ignored = true;
			cachedTransition_0 = null;
		}
	}

	private void PostEnter()
	{
		cachedTransition_0.Visited = true;
		cachedTransition_0 = null;
		ResetLevelAnchor();
		MarkBackTransition();
		Blacklist.Reset();
	}

	private static void MarkBackTransition()
	{
		CombatAreaCache.Tick();
		CachedTransition cachedTransition = (from a in Enumerable.Where(CombatAreaCache.Current.AreaTransitions, (CachedTransition a) => a.Type == TransitionType.Local && !a.LeadsBack && !a.Visited && a.Position.Distance < 50)
			orderby a.Position.Distance
			select a).FirstOrDefault();
		if (!(cachedTransition == null))
		{
			cachedTransition.LeadsBack = true;
			GlobalLog.Debug($"[ComplexExplorer] Marking {cachedTransition.Position} as back transition.");
		}
		else
		{
			GlobalLog.Debug("[ComplexExplorer] No back transition detected.");
		}
	}

	private void HandleExternalJump()
	{
		cachedTransition_0 = null;
		foreach (CachedTransition areaTransition in CombatAreaCache.Current.AreaTransitions)
		{
			areaTransition.Visited = false;
		}
		bool_0 = true;
		BasicExplorer.Reset();
		ResetLevelAnchor();
		bool_0 = false;
	}

	private static async Task<bool> HandleSarcophagus()
	{
		NetworkObject SoulpsJjQr = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata.Contains("sarcophagus_door"));
		if (!(SoulpsJjQr == (NetworkObject)null))
		{
			if (!SoulpsJjQr.IsTargetable)
			{
				GlobalLog.Error("[ComplexExplorer] Sarcophagus is not targetable.");
				return false;
			}
			return await PlayerAction.Interact(SoulpsJjQr, () => !SoulpsJjQr.Fresh<NetworkObject>().IsTargetable, "Sarcophagus interaction");
		}
		GlobalLog.Error("[ComplexExplorer] There is no sarcophagus.");
		return false;
	}

	private void ResetLevelAnchor()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<WorldPosition, GridExplorer>.KeyCollection keys = dictionary_0.Keys;
		foreach (WorldPosition item in keys)
		{
			if (item.PathExists)
			{
				GlobalLog.Debug($"[ComplexExplorer] Resetting anchor point to existing one {item}");
				worldPosition_0 = item;
				worldPosition_1 = new WorldPosition(LokiPoe.MyPosition);
				return;
			}
		}
		InitNewLevel();
	}

	private void InitNewLevel()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		worldPosition_0 = new WorldPosition(LokiPoe.MyPosition);
		worldPosition_1 = worldPosition_0;
		dictionary_0.Add(worldPosition_0, CreateExplorer());
		GlobalLog.Debug($"[ComplexExplorer] Creating new level anchor {worldPosition_0}");
	}

	private GridExplorer CreateExplorer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		GridExplorer val = new GridExplorer
		{
			AutoResetOnAreaChange = false,
			TileKnownRadius = Settings.TileKnownRadius,
			TileSeenRadius = Settings.TileSeenRadius
		};
		val.Start();
		return val;
	}

	public void Tick()
	{
		if (!bool_0)
		{
			BasicExplorer.Tick();
		}
	}

	public static bool AddSettingsProvider(string ownerId, Func<ExplorationSettings> getSettings, ProviderPriority priority)
	{
		if (!list_1.Exists((Class43 s) => s.string_0 == ownerId))
		{
			list_1.Add(new Class43(ownerId, getSettings, priority));
			GlobalLog.Info("[ComplexExplorer] " + ownerId + " settings provider has been added.");
			return true;
		}
		return false;
	}

	public static bool RemoveSettingsProvider(string ownerId)
	{
		int num = list_1.FindIndex((Class43 s) => s.string_0 == ownerId);
		if (num >= 0)
		{
			list_1.RemoveAt(num);
			GlobalLog.Info("[ComplexExplorer] " + ownerId + " settings provider has been removed.");
			return true;
		}
		return false;
	}

	public static void ResetSettingsProviders()
	{
		list_1.Clear();
	}

	private static ExplorationSettings ProvideSettings()
	{
		foreach (Class43 item in list_1.OrderBy((Class43 p) => p.Priority))
		{
			ExplorationSettings explorationSettings = item.func_0();
			if (explorationSettings != null)
			{
				GlobalLog.Info("[ComplexExplorer] Exploration settings provided by " + item.string_0 + ".");
				return explorationSettings;
			}
		}
		GlobalLog.Info("[ComplexExplorer] Exploration settings was not provided. Using default.");
		return new ExplorationSettings();
	}

	static ComplexExplorer()
	{
		list_0 = new List<string> { "Warlord's Keep", "Redeemer's Eyrie", "Crusader's Sanctum", "Hunter's Ambush" };
		interval_0 = new Interval(750);
		list_1 = new List<Class43>();
	}
}
