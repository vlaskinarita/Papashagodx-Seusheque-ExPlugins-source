using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Tasks;
using log4net;

namespace ExPlugins.NullBotEx;

public class NullBotEx : IBot, IAuthored, IBase, IConfigurable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003Ec_0;

		public static GetExplorerDelegate getExplorerDelegate_0;

		static _003C_003Ec()
		{
			_003C_003Ec_0 = new _003C_003Ec();
		}

		internal IExplorer _003CStart_003Eb__7_0(dynamic[] user)
		{
			return null;
		}
	}

	private static readonly ILog ilog_0;

	private NullBotExGui nullBotExGui_0;

	private Coroutine coroutine_0;

	private bool bool_0;

	public const string GetTaskManagerMessage = "GetTaskManager";

	private readonly TaskManager taskManager_0 = new TaskManager();

	public string Name => "NullBotEx";

	public string Description => "A Bot that does nothing. But actually does something.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public JsonSettings Settings => (JsonSettings)(object)NullBotSettings.Instance;

	public UserControl Control => nullBotExGui_0 ?? (nullBotExGui_0 = new NullBotExGui());

	public MessageResult Message(Message message)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		string id = message.Id;
		if (id == "GetTaskManager")
		{
			message.AddOutput<TaskManager>((IMessageHandler)(object)this, taskManager_0, "");
			flag = true;
		}
		MessageResult val = ((TaskManagerBase<ITask>)(object)taskManager_0).SendMessage((TaskGroup)1, message);
		if ((int)val == 0)
		{
			flag = true;
		}
		return (MessageResult)((!flag) ? 1 : 0);
	}

	public void Start()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		GeneralSettings.Instance.IsOnRun = false;
		ItemEvaluator.Instance = (IItemEvaluator)(object)DefaultItemEvaluator.Instance;
		object obj = _003C_003Ec.getExplorerDelegate_0;
		if (obj == null)
		{
			GetExplorerDelegate val = (dynamic[] user) => null;
			obj = (object)val;
			_003C_003Ec.getExplorerDelegate_0 = val;
		}
		Explorer.CurrentDelegate = (GetExplorerDelegate)obj;
		Binding.Update();
		BotManager.MsBetweenTicks = 30;
		ilog_0.Debug((object)$"[Start] MsBetweenTicks: {BotManager.MsBetweenTicks}.");
		ilog_0.Debug((object)$"[Start] KeyPickup: {ConfigManager.KeyPickup}.");
		if (NullBotSettings.Instance.ShouldEnableHooks)
		{
			ProcessHookManager.Enable();
		}
		coroutine_0 = null;
		ExilePather.Reload(false);
		((TaskManagerBase<ITask>)(object)taskManager_0).Reset();
		AddTasks();
		PluginManager.Start();
		RoutineManager.Start();
		PlayerMoverManager.Start();
		((TaskManagerBase<ITask>)(object)taskManager_0).Start();
		ilog_0.Debug((object)("[Start] Current PlayerMover: " + ((IAuthored)PlayerMoverManager.Current).Name + "."));
		ilog_0.Debug((object)("[Start] Current Routine " + ((IAuthored)RoutineManager.Current).Name + "."));
		foreach (IPlugin enabledPlugin in PluginManager.EnabledPlugins)
		{
			ilog_0.Debug((object)("[Start] The plugin " + ((IAuthored)enabledPlugin).Name + " is enabled."));
		}
	}

	public void Stop()
	{
		((TaskManagerBase<ITask>)(object)taskManager_0).Stop();
		PluginManager.Stop();
		RoutineManager.Stop();
		PlayerMoverManager.Stop();
		if (NullBotSettings.Instance.ShouldEnableHooks)
		{
			ProcessHookManager.Disable();
		}
		if (coroutine_0 != null)
		{
			coroutine_0.Dispose();
			coroutine_0 = null;
		}
	}

	public void Tick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (coroutine_0 == null)
		{
			coroutine_0 = new Coroutine((Func<Task>)(() => MainCoroutine()));
		}
		if (LokiPoe.IsInGame)
		{
			if (!bool_0)
			{
				ExilePather.Reload(false);
			}
			else
			{
				ExilePather.Reload(true);
				bool_0 = false;
			}
		}
		else
		{
			bool_0 = true;
		}
		((TaskManagerBase<ITask>)(object)taskManager_0).Tick();
		PluginManager.Tick();
		RoutineManager.Tick();
		PlayerMoverManager.Tick();
		if (!coroutine_0.IsFinished)
		{
			try
			{
				coroutine_0.Resume();
				return;
			}
			catch
			{
				Coroutine val = coroutine_0;
				coroutine_0 = null;
				val.Dispose();
				throw;
			}
		}
		ilog_0.Debug((object)$"The bot coroutine has finished in a state of {coroutine_0.Status}");
		BotManager.Stop(false);
	}

	public override string ToString()
	{
		return "[" + Name + "]: " + Description;
	}

	public void Deinitialize()
	{
		BotManager.OnBotChanged -= BotManagerOnOnBotChanged;
	}

	public void Initialize()
	{
		BotManager.OnBotChanged += BotManagerOnOnBotChanged;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return await ((TaskManagerBase<ITask>)(object)taskManager_0).ProvideLogic((TaskGroup)1, (RunBehavior)1, logic);
	}

	private void BotManagerOnOnBotChanged(object sender, BotChangedEventArgs e)
	{
		if (e.New == this)
		{
			ItemEvaluator.Instance = (IItemEvaluator)(object)DefaultItemEvaluator.Instance;
		}
	}

	public TaskManager GetTaskManager()
	{
		return taskManager_0;
	}

	private void AddTasks()
	{
		if (NullBotSettings.Instance.ShouldEnableCombat)
		{
			((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CombatTask(50));
			((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new PostCombatHookTask());
			((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new CombatTask(-1));
		}
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new ClearCursorTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new TravelToHideoutTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new SellTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new StashTask());
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new FallbackTask());
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
						await Coroutine.Sleep(1000);
						continue;
					}
					bool hooked = false;
					Logic logic3 = new Logic("hook_ingame", (object)this);
					foreach (IPlugin plugin3 in PluginManager.EnabledPlugins)
					{
						if ((int)(await ((ILogicProvider)plugin3).Logic(logic3)) != 0)
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
							ilog_0.Debug((object)"Waiting for game pause");
						}
						else if (!((Actor)LokiPoe.Me).IsDead)
						{
							await ((TaskManagerBase<ITask>)(object)taskManager_0).Run((TaskGroup)1, (RunBehavior)1);
						}
					}
				}
				else
				{
					Logic logic2 = new Logic("hook_character_selection", (object)this);
					foreach (IPlugin plugin2 in PluginManager.EnabledPlugins)
					{
						if ((int)(await ((ILogicProvider)plugin2).Logic(logic2)) == 0)
						{
							break;
						}
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

	static NullBotEx()
	{
		ilog_0 = Logger.GetLoggerInstanceForType();
	}
}
