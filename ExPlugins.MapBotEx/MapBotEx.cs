using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.EXtensions.Global;
using ExPlugins.MapBotEx.ExtraContent;
using ExPlugins.MapBotEx.ExtraContent.Breach;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.MapBotEx.Tasks;
using ExPlugins.PapashaCore;
using Newtonsoft.Json.Linq;

namespace ExPlugins.MapBotEx;

public class MapBotEx : IBot, IAuthored, IBase, IConfigurable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, GInterface0, IUrlProvider
{
	public static class Messages
	{
		public const string NewMapEntered = "MB_new_map_entered_event";

		public const string MapPortalEntered = "MB_map_portal_entered_event";

		public const string KiracMissionOpened = "MB_kirac_mission_opened_event";

		public const string GetIsOnRun = "MB_get_is_on_run";

		public const string SetIsOnRun = "MB_set_is_on_run";

		public const string GetMapSettings = "MB_get_map_settings";
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static Func<IPlugin, bool> func_0;

		public static GetExplorerDelegate getExplorerDelegate_0;

		public static Func<IPlugin, bool> func_1;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003CStart_003Eb__6_1(IPlugin n)
		{
			return ((IAuthored)n).Name == "PapashaCore";
		}

		internal IExplorer _003CStart_003Eb__6_0(dynamic[] user)
		{
			return (IExplorer)(object)CombatAreaCache.Current.Explorer.BasicExplorer;
		}

		internal bool _003CTick_003Eb__7_1(IPlugin n)
		{
			return ((IAuthored)n).Name == "PapashaCore";
		}
	}

	private static readonly Dictionary<string, int> dictionary_0;

	public static TaskManager BotTaskManager;

	private Coroutine coroutine_0;

	private Gui gui_0;

	private static int TileSeenRadius
	{
		get
		{
			int value;
			return (!dictionary_0.TryGetValue(World.CurrentArea.Name, out value)) ? 4 : value;
		}
	}

	public string Name => "MapBotEx";

	public string Description => "Bot for running maps.";

	public string Author => "ExVault / Mod by Papashagodx + Seusheque";

	public string Version => "1.0.3";

	public JsonSettings Settings => (JsonSettings)(object)GeneralSettings.Instance;

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public string Url => "https://discord.gg/HeqYtkujWW";

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Invalid comparison between Unknown and I4
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[MapBotEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (LokiPoe.IsInGame && World.CurrentArea.IsMap && Statistics.Instance.MapTimer != null)
		{
			Statistics.Instance.MapTimer.Start();
		}
		BotTaskManager = GetCurrentBotTaskManager();
		ItemEvaluator.Instance = (IItemEvaluator)(object)DefaultItemEvaluator.Instance;
		object obj = _003C_003Ec.getExplorerDelegate_0;
		if (obj == null)
		{
			GetExplorerDelegate val = (dynamic[] user) => (IExplorer)(object)CombatAreaCache.Current.Explorer.BasicExplorer;
			obj = (object)val;
			_003C_003Ec.getExplorerDelegate_0 = val;
		}
		Explorer.CurrentDelegate = (GetExplorerDelegate)obj;
		ComplexExplorer.ResetSettingsProviders();
		ComplexExplorer.AddSettingsProvider("MapBotEx", MapBotExploration, ProviderPriority.Low);
		Binding.Update();
		GlobalLog.Debug($"[Start] MsBetweenTicks: {BotManager.MsBetweenTicks}.");
		GlobalLog.Debug($"[Start] NetworkingMode: {ConfigManager.NetworkingMode}.");
		GlobalLog.Debug($"[Start] KeyPickup: {ConfigManager.KeyPickup}.");
		ProcessHookManager.Enable();
		coroutine_0 = null;
		ExilePather.Reload(false);
		((TaskManagerBase<ITask>)(object)BotTaskManager).Reset();
		AddTasks();
		Events.Start();
		PluginManager.Start();
		RoutineManager.Start();
		PlayerMoverManager.Start();
		((TaskManagerBase<ITask>)(object)BotTaskManager).Start();
		if ((int)ExilePather.BlockTrialOfAscendancy == 0)
		{
			ExilePather.BlockTrialOfAscendancy = (FeatureEnum)2;
		}
	}

	public void Tick()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[MapBotEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (coroutine_0 == null)
		{
			coroutine_0 = new Coroutine((Func<Task>)(() => MainCoroutine()));
		}
		ExilePather.Reload(false);
		Events.Tick();
		CombatAreaCache.Tick();
		((TaskManagerBase<ITask>)(object)BotTaskManager).Tick();
		PluginManager.Tick();
		RoutineManager.Tick();
		PlayerMoverManager.Tick();
		StuckDetection.Tick();
		Statistics.Instance.Tick();
		if (!coroutine_0.IsFinished)
		{
			try
			{
				coroutine_0.Resume();
				return;
			}
			catch (Exception ex)
			{
				Coroutine val = coroutine_0;
				coroutine_0 = null;
				val.Dispose();
				ErrorManager.ReportError(ex.ToString());
				throw;
			}
		}
		GlobalLog.Debug($"The bot coroutine has finished in a state of {coroutine_0.Status}");
		BotManager.Stop(new StopReasonData("coroutine_finished", $"The bot coroutine has finished in a state of {coroutine_0.Status}", (object)null), false);
	}

	public void Stop()
	{
		Statistics.Instance.MapTimer.Stop();
		((TaskManagerBase<ITask>)(object)BotTaskManager).Stop();
		PluginManager.Stop();
		RoutineManager.Stop();
		PlayerMoverManager.Stop();
		ProcessHookManager.Disable();
		if (coroutine_0 != null)
		{
			try
			{
				coroutine_0.Dispose();
			}
			catch (CoroutineStoppedException)
			{
			}
			coroutine_0 = null;
		}
	}

	public void Deinitialize()
	{
		BotManager.OnBotChanged -= BotManagerOnOnBotChanged;
	}

	public void Initialize()
	{
		ExilePather.Reload(true);
		Statistics.Instance.Load();
		BotManager.OnBotChanged += BotManagerOnOnBotChanged;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return await ((TaskManagerBase<ITask>)(object)BotTaskManager).ProvideLogic((TaskGroup)1, (RunBehavior)1, logic);
	}

	public MessageResult Message(Message message)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		switch (message.Id)
		{
		case "GetTaskManager":
			message.AddOutput<TaskManager>((IMessageHandler)(object)this, BotTaskManager, "");
			flag = true;
			break;
		case "MB_get_is_on_run":
			message.AddOutput<bool>((IMessageHandler)(object)this, GeneralSettings.Instance.IsOnRun, "");
			flag = true;
			break;
		case "MB_set_is_on_run":
		{
			bool input = message.GetInput<bool>(0);
			GlobalLog.Info($"[MapBotEx] SetIsOnRun: {input}");
			GeneralSettings.Instance.IsOnRun = input;
			flag = true;
			break;
		}
		case "MB_get_map_settings":
			message.AddOutput<JObject>((IMessageHandler)(object)this, JObject.FromObject((object)MapSettings.Instance.MapDict), "");
			flag = true;
			break;
		}
		Events.FireEventsFromMessage(message);
		MessageResult val = ((TaskManagerBase<ITask>)(object)BotTaskManager).SendMessage((TaskGroup)1, message);
		if ((int)val == 0)
		{
			flag = true;
		}
		return (MessageResult)((!flag) ? 1 : 0);
	}

	public TaskManager GetTaskManager()
	{
		return BotTaskManager;
	}

	public static TaskManager GetCurrentBotTaskManager()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		IBot current = BotManager.Current;
		Message val = new Message("GetTaskManager", (object)null);
		((IMessageHandler)current).Message(val);
		return val.GetOutput<TaskManager>(0);
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		bool flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		bool flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	private async Task MainCoroutine()
	{
		while (true)
		{
			if (!LokiPoe.IsInLoginScreen)
			{
				if (!LokiPoe.IsInCharacterSelectionScreen)
				{
					if (!LokiPoe.IsInGame || !ExilePather.IsReady)
					{
						await Coroutine.Sleep(50);
						continue;
					}
					bool hooked = false;
					Logic logic3 = new Logic("hook_ingame", (object)this);
					foreach (IPlugin plugin2 in PluginManager.EnabledPlugins)
					{
						if ((int)(await ((ILogicProvider)plugin2).Logic(logic3)) != 0)
						{
							continue;
						}
						hooked = true;
						break;
					}
					if (!hooked)
					{
						if (InstanceInfo.IsGamePaused)
						{
							GlobalLog.Debug("Waiting for game pause");
							await Wait.StuckDetectionSleep(200);
						}
						else if (((Actor)LokiPoe.Me).IsDead)
						{
							await ResurrectionLogic.Execute();
						}
						else
						{
							try
							{
								await ((TaskManagerBase<ITask>)(object)BotTaskManager).Run((TaskGroup)1, (RunBehavior)1);
							}
							catch (Exception ex)
							{
								Exception e = ex;
								GlobalLog.Debug("=================");
								GlobalLog.Error(e.Source + ": " + e.Message);
								GlobalLog.Debug(e.StackTrace);
								GlobalLog.Debug("=================");
							}
						}
					}
				}
				else
				{
					Logic logic = new Logic("hook_character_selection", (object)this);
					foreach (IPlugin plugin in PluginManager.EnabledPlugins)
					{
						if ((int)(await ((ILogicProvider)plugin).Logic(logic)) != 0)
						{
							continue;
						}
						break;
					}
				}
			}
			else
			{
				Logic logic2 = new Logic("hook_login_screen", (object)this);
				foreach (IPlugin plugin3 in PluginManager.EnabledPlugins)
				{
					if ((int)(await ((ILogicProvider)plugin3).Logic(logic2)) == 0)
					{
						break;
					}
				}
			}
			await Coroutine.Yield();
		}
	}

	private void BotManagerOnOnBotChanged(object sender, BotChangedEventArgs e)
	{
		if (e.New == this)
		{
			ItemEvaluator.Instance = (IItemEvaluator)(object)DefaultItemEvaluator.Instance;
		}
	}

	private static void AddTasks()
	{
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new ClearCursorTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new LeaveAreaTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new AfkTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new HandleBlockingChestsTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new HandleBlockingObjectTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new FollowerHandlerTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CombatTask(50));
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new TalkToNpcOnMapTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new DeliriumInitiatorTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new HandleBreachesTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new PostCombatHookTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CastAuraTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new SpecialObjectTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new LootItemTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new OpenChestTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CorruptedAreaTask(GeneralSettings.Instance.EnterCorruptedAreas));
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CraftingRecipeLootTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CombatTask(-1));
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new HandleHarvestTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new MetamorphTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new TravelToHideoutTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new AssignPantheonTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new Class34());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new IdTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new SellTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new StashTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new CurrencyRestockTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new SortInventoryTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new VendorTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new Class33());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new SellMapTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new StartMapInEpilogueTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new TakeMapTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new OpenMapTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new DeviceAreaTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new ProximityTriggerTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new KillBossTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new TrackMobTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new TransitionTriggerTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new MapExplorationTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new FinishMapTask());
		((TaskManagerBase<ITask>)(object)BotTaskManager).Add((ITask)(object)new FallbackTask());
	}

	private static ExplorationSettings MapBotExploration()
	{
		if (!World.CurrentArea.IsMap)
		{
			return null;
		}
		OnNewMapEnter();
		MapData current = MapData.Current;
		MapType mapType = current.Type;
		int tileSeenRadius = TileSeenRadius;
		if (GeneralSettings.Instance.BossRushMode || current.BossRush)
		{
			tileSeenRadius = 6;
		}
		if (mapType == MapType.Regular && AtlasHelper.IsAtlasBossPresent)
		{
			mapType = MapType.Bossroom;
		}
		if (!(World.CurrentArea.Name == MapNames.Core))
		{
			if (!(World.CurrentArea.Name == MapNames.Cage))
			{
				if (World.CurrentArea.Name == MapNames.ArachnidNest)
				{
					return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "The Loom Chamber", 7, tileSeenRadius);
				}
				if (!(World.CurrentArea.Name == MapNames.Carcass))
				{
					if (!(World.CurrentArea.Name == MapNames.Pier))
					{
						if (World.CurrentArea.Name == MapNames.DesertSpring)
						{
							return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "The Sand Pit", 7, tileSeenRadius);
						}
						if (World.CurrentArea.Name == MapNames.Caldera)
						{
							return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: true, GeneralSettings.Instance.OpenPortals, "Caldera of The King", 7, tileSeenRadius);
						}
						return new ExplorationSettings(basicExploration: false, openPortals: GeneralSettings.Instance.OpenPortals, backtracking: mapType == MapType.Complex, fastTransition: current.FastTransition.HasValue && current.FastTransition.Value, priorityTransition: "Arena", tileKnownRadius: 7, tileSeenRadius: tileSeenRadius);
					}
					return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "Gauntlet", 7, tileSeenRadius);
				}
				return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "The Black Heart", 7, tileSeenRadius);
			}
			return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "Ladder", 7, tileSeenRadius);
		}
		return new ExplorationSettings(basicExploration: false, fastTransition: false, backtracking: false, GeneralSettings.Instance.OpenPortals, "The Black Core", 7, tileSeenRadius);
	}

	public static async Task<ChatResult> SendChatMsg(string msg)
	{
		if (LokiPoe.IsInGame)
		{
			if (string.IsNullOrEmpty(msg))
			{
				return (ChatResult)0;
			}
			await Coroutines.FinishCurrentAction(true);
			if (!ChatPanel.IsOpened)
			{
				ChatPanel.ToggleChat(true);
			}
			ChatResult result = ChatPanel.Chat(msg, true, true);
			if (ChatPanel.IsOpened)
			{
				ChatPanel.ToggleChat(true);
			}
			return result;
		}
		return (ChatResult)0;
	}

	private static void OnNewMapEnter()
	{
		string name = World.CurrentArea.Name;
		GeneralSettings.Instance.IsOnRun = true;
		MapData.ResetCurrent();
		Statistics.Instance.OnNewMapEnter();
		Utility.BroadcastMessage((object)null, "MB_new_map_entered_event", new object[1] { name });
		GlobalLog.Warn("[MapBotEx] New map has been entered: " + Statistics.Instance.CurrentMapName + ".");
	}

	public override string ToString()
	{
		return "[" + Name + "]: " + Description;
	}

	static MapBotEx()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		dictionary_0 = new Dictionary<string, int>
		{
			[MapNames.MaoKun] = 3,
			[MapNames.Caldera] = 3,
			[MapNames.Arena] = 3,
			[MapNames.CastleRuins] = 3,
			[MapNames.UndergroundRiver] = 3,
			[MapNames.TropicalIsland] = 3,
			[MapNames.Beach] = 5,
			[MapNames.Strand] = 5,
			[MapNames.Port] = 5,
			[MapNames.Glacier] = 5,
			[MapNames.Alleyways] = 5,
			[MapNames.AcidCaverns] = 5,
			[MapNames.Phantasmagoria] = 5,
			[MapNames.Wharf] = 5,
			[MapNames.Cemetery] = 5,
			[MapNames.MineralPools] = 5,
			[MapNames.Temple] = 5,
			[MapNames.Malformation] = 5,
			[MapNames.FrozenCabins] = 2
		};
		BotTaskManager = new TaskManager();
	}
}
