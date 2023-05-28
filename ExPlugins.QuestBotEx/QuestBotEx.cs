using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.EXtensions.Global;
using ExPlugins.MapBotEx;
using ExPlugins.PapashaCore;
using ExPlugins.QuestBotEx.TrialHandler;

namespace ExPlugins.QuestBotEx;

public class QuestBotEx : IBot, IAuthored, IBase, IConfigurable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, GInterface0, IUrlProvider
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static Func<IPlugin, bool> func_0;

		public static GetExplorerDelegate getExplorerDelegate_0;

		public static Func<IPlugin, bool> func_1;

		public static Func<IBot, bool> func_2;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal bool _003CStart_003Eb__4_1(IPlugin n)
		{
			return ((IAuthored)n).Name == "PapashaCore";
		}

		internal IExplorer _003CStart_003Eb__4_0(dynamic[] user)
		{
			return (IExplorer)(object)CombatAreaCache.Current.Explorer.BasicExplorer;
		}

		internal bool _003CTick_003Eb__5_1(IPlugin n)
		{
			return ((IAuthored)n).Name == "PapashaCore";
		}

		internal bool _003CMessage_003Eb__10_1(IBot b)
		{
			return ((IAuthored)b).Name == "MapBotEx";
		}
	}

	private static readonly HashSet<string> hashSet_0;

	private readonly TaskManager taskManager_0 = new TaskManager();

	private Coroutine coroutine_0;

	private Gui gui_0;

	public string Name => "QuestBotEx";

	public string Description => "Bot to complete questline.";

	public string Author => "Alcor75 / Mod by Papashagodx + Seusheque";

	public string Version => "0.0.0.1";

	public JsonSettings Settings => (JsonSettings)(object)QuestBotSettings.Instance;

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public string Url => "https://discord.gg/HeqYtkujWW";

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[QuestBotEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GeneralSettings.Instance.IsOnRun = false;
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
		ComplexExplorer.AddSettingsProvider("QuestBotEx", QuestBotExploration, ProviderPriority.Low);
		Binding.Update();
		GlobalLog.Debug($"[Start] MsBetweenTicks: {BotManager.MsBetweenTicks}.");
		GlobalLog.Debug($"[Start] NetworkingMode: {ConfigManager.NetworkingMode}.");
		GlobalLog.Debug($"[Start] KeyPickup: {ConfigManager.KeyPickup}.");
		ProcessHookManager.Enable();
		coroutine_0 = null;
		ExilePather.Reload(false);
		((TaskManagerBase<ITask>)(object)taskManager_0).Reset();
		AddTasks();
		Events.Start();
		PluginManager.Start();
		RoutineManager.Start();
		PlayerMoverManager.Start();
		((TaskManagerBase<ITask>)(object)taskManager_0).Start();
		if ((int)ExilePather.BlockTrialOfAscendancy == 0)
		{
			ExilePather.BlockTrialOfAscendancy = (FeatureEnum)1;
		}
	}

	public void Tick()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[QuestBotEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (coroutine_0 == null)
		{
			coroutine_0 = new Coroutine((Func<Task>)(() => MainCoroutine()));
		}
		ExilePather.Reload(false);
		Events.Tick();
		CombatAreaCache.Tick();
		((TaskManagerBase<ITask>)(object)taskManager_0).Tick();
		PluginManager.Tick();
		RoutineManager.Tick();
		PlayerMoverManager.Tick();
		StuckDetection.Tick();
		if (coroutine_0.IsFinished)
		{
			GlobalLog.Debug($"The bot coroutine has finished in a state of {coroutine_0.Status}");
			BotManager.Stop(new StopReasonData("coroutine_finished", $"The bot coroutine has finished in a state of {coroutine_0.Status}", (object)null), false);
			return;
		}
		try
		{
			coroutine_0.Resume();
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

	public void Stop()
	{
		((TaskManagerBase<ITask>)(object)taskManager_0).Stop();
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
		BotManager.OnBotChanged += BotManagerOnOnBotChanged;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return await ((TaskManagerBase<ITask>)(object)taskManager_0).ProvideLogic((TaskGroup)1, (RunBehavior)1, logic);
	}

	public MessageResult Message(Message message)
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Invalid comparison between Unknown and I4
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		string id = message.Id;
		if (message.Id == "combat_area_changed_event" || message.Id == "area_changed_event")
		{
			if (TrialSolverTask.AreaContainsTrial())
			{
				ExilePather.BlockTrialOfAscendancy = (FeatureEnum)2;
				ExilePather.BlockLockedDoors = (FeatureEnum)2;
				if (TrialSolverTask.TrialPlaqueTgt == null && TrialSolverTask.AreaIDtoPlaque.TryGetValue(LokiPoe.CurrentWorldArea.Id, out var value) && value.Initialize())
				{
					TrialSolverTask.TrialPlaqueTgt = value;
				}
			}
			flag = true;
		}
		if (!(id == "GetTaskManager"))
		{
			if (id == "ingame_bot_start_event")
			{
				QuestManager.CompletedQuests.Instance.Verify();
				flag = true;
			}
			else if (message.Id == "player_died_event")
			{
				int input = message.GetInput<int>(0);
				GrindingHandler.OnPlayerDied(input);
				flag = true;
			}
			else if (!(message.Id == "QB_get_current_quest"))
			{
				if (!(message.Id == "QB_finish_grinding"))
				{
					if (ExtensionsSettings.Instance.SwitchBotOnNoMaps && message.Id == "item_stashed_event")
					{
						CachedItem input2 = message.GetInput<CachedItem>(0);
						if (input2.Name.Contains("Map") && ExtensionsSettings.Instance.SwitchBotOnNoMaps)
						{
							BackgroundWorker backgroundWorker = new BackgroundWorker();
							backgroundWorker.DoWork += delegate
							{
								Stopwatch stopwatch = Stopwatch.StartNew();
								while (BotManager.IsRunning && stopwatch.ElapsedMilliseconds <= 30000L)
								{
								}
								if (!BotManager.IsRunning)
								{
									IBot val2 = BotManager.Bots.FirstOrDefault((IBot b) => ((IAuthored)b).Name == "MapBotEx");
									if (val2 != null)
									{
										GlobalLog.Debug("switching bot");
										BotManager.Current = val2;
										BotManager.Start();
										Utility.BroadcastMessage((object)this, "botswitch_event", new object[1] { "QuestBotEx to MapBotEx" });
									}
									else
									{
										GlobalLog.Error("cant switch bot, just stopping");
									}
								}
							};
							backgroundWorker.RunWorkerAsync();
							BotManager.Stop(new StopReasonData("cant_switch_bot", "cant switch bot, just stopping", (object)null), false);
						}
					}
				}
				else
				{
					GlobalLog.Debug("[QuestBotEx] Grinding force finish: true");
					flag = true;
				}
			}
			else
			{
				QuestBotSettings instance = QuestBotSettings.Instance;
				message.AddOutputs((IMessageHandler)(object)this, new object[2] { instance.CurrentQuestName, instance.CurrentQuestState });
				flag = true;
			}
		}
		else
		{
			message.AddOutput<TaskManager>((IMessageHandler)(object)this, taskManager_0, "");
			flag = true;
		}
		Events.FireEventsFromMessage(message);
		MessageResult val = ((TaskManagerBase<ITask>)(object)taskManager_0).SendMessage((TaskGroup)1, message);
		if ((int)val == 0)
		{
			flag = true;
		}
		return (MessageResult)((!flag) ? 1 : 0);
	}

	public TaskManager GetTaskManager()
	{
		return taskManager_0;
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

	private void AddTasks()
	{
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new ClearCursorTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new LeaveAreaTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new AfkTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new TravelToHideoutTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new HandleBlockingChestsTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new HandleBlockingObjectTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new TrialDoorTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CastAuraTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CombatTask(50));
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new ReturnAfterDeathTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new PostCombatHookTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new FollowerHandlerTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CraftingRecipeLootTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new LootItemTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new OpenChestTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CombatTask(-1));
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new AssignPantheonTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new IdTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new SellTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new StashTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CurrencyRestockTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new SortInventoryTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new VendorTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new ReturnAfterTownrunTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new OpenWaypointTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new TrialSolverTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new GoToTrialAreaTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CorruptedAreaTask(QuestBotSettings.Instance.EnterCorruptedAreas));
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new QuestTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new FallbackTask());
	}

	public async Task MainCoroutine()
	{
		while (true)
		{
			if (!LokiPoe.IsInLoginScreen)
			{
				if (!LokiPoe.IsInCharacterSelectionScreen)
				{
					if (!LokiPoe.IsInCreateCharacterScreen)
					{
						if (!LokiPoe.IsInGame || !ExilePather.IsReady)
						{
							await Coroutine.Sleep(1000);
							continue;
						}
						bool hooked = false;
						Logic logic4 = new Logic("hook_ingame", (object)this);
						foreach (IPlugin plugin4 in PluginManager.EnabledPlugins)
						{
							if ((int)(await ((ILogicProvider)plugin4).Logic(logic4)) == 0)
							{
								hooked = true;
								break;
							}
						}
						if (!hooked)
						{
							if (InstanceInfo.IsGamePaused)
							{
								GlobalLog.Debug("Waiting for game pause");
								await Wait.StuckDetectionSleep(200);
							}
							else if (!((Actor)LokiPoe.Me).IsDead)
							{
								await ((TaskManagerBase<ITask>)(object)taskManager_0).Run((TaskGroup)1, (RunBehavior)1);
							}
							else
							{
								await ResurrectionLogic.Execute();
							}
						}
					}
					else
					{
						Logic logic3 = new Logic("hook_character_creation", (object)this);
						foreach (IPlugin plugin3 in PluginManager.EnabledPlugins)
						{
							if ((int)(await ((ILogicProvider)plugin3).Logic(logic3)) != 0)
							{
								continue;
							}
							break;
						}
					}
				}
				else
				{
					Logic logic2 = new Logic("hook_character_selection", (object)this);
					foreach (IPlugin plugin2 in PluginManager.EnabledPlugins)
					{
						if ((int)(await ((ILogicProvider)plugin2).Logic(logic2)) != 0)
						{
							continue;
						}
						break;
					}
				}
			}
			else
			{
				Logic logic = new Logic("hook_login_screen", (object)this);
				foreach (IPlugin plugin in PluginManager.EnabledPlugins)
				{
					if ((int)(await ((ILogicProvider)plugin).Logic(logic)) == 0)
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

	private static ExplorationSettings QuestBotExploration()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (currentArea.IsOverworldArea)
		{
			string id = currentArea.Id;
			if (id == World.Act4.GrandArena.Id)
			{
				return new ExplorationSettings(basicExploration: false, fastTransition: true, backtracking: true, openPortals: false, null, 7, 3);
			}
			if (!(id == World.Act7.MaligaroSanctum.Id))
			{
				if (!hashSet_0.Contains(id))
				{
					return null;
				}
				return new ExplorationSettings(basicExploration: false, fastTransition: true, backtracking: false, openPortals: false);
			}
			return new ExplorationSettings(basicExploration: false, fastTransition: true);
		}
		return null;
	}

	public override string ToString()
	{
		return "[" + Name + "]: " + Description;
	}

	static QuestBotEx()
	{
		hashSet_0 = new HashSet<string>
		{
			World.Act2.AncientPyramid.Id,
			World.Act3.SceptreOfGod.Id,
			World.Act3.UpperSceptreOfGod.Id,
			World.Act6.PrisonerGate.Id,
			World.Act7.Crypt.Id,
			World.Act7.TempleOfDecay1.Id,
			World.Act7.TempleOfDecay2.Id,
			World.Act9.Descent.Id,
			World.Act9.Oasis.Id,
			World.Act9.RottingCore.Id,
			World.Act10.Ossuary.Id
		};
	}
}
