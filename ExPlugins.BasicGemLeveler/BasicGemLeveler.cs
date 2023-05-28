using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.BasicGemLeveler;

public class BasicGemLeveler : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, ITickEvents, IStartStopEvents
{
	private static readonly Interval interval_0;

	private Gui gui_0;

	private bool bool_0;

	public static readonly WaitTimer LevelWait;

	public static bool NeedsToUpdate;

	public string Name => "BasicGemLeveler";

	public string Description => "A plugin to automatically level skill gems.";

	public string Author => "Bossland GmbH";

	public string Version => "1.0.0.6";

	public JsonSettings Settings => (JsonSettings)(object)BasicGemLevelerSettings.Instance;

	public System.Windows.Controls.UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public void Initialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		BotManager.PostStart += new BotEvent(BotManagerOnPostStart);
	}

	public void Deinitialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		BotManager.PostStart -= new BotEvent(BotManagerOnPostStart);
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (message.Id == "area_changed_event")
		{
			Reset();
			flag = true;
		}
		else if (message.Id == "player_leveled_event")
		{
			NeedsToUpdate = true;
			flag = true;
		}
		return (MessageResult)((!flag) ? 1 : 0);
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[BasicGemLeveler] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		TaskManager currentBotTaskManager = GetCurrentBotTaskManager();
		if (currentBotTaskManager != null)
		{
			if (((TaskManagerBase<ITask>)(object)currentBotTaskManager).TaskList.Any((ITask t) => ((IAuthored)t).Name.Equals("CombatTask (Leash 50)")))
			{
				GlobalLog.Debug(((TaskManagerBase<ITask>)(object)currentBotTaskManager).AddBefore((ITask)(object)new Class54(), "CombatTask (Leash 50)") ? "[BasicGemLeveler] LevelGemsTask Task added before CombatTask (Leash 50)." : "[BasicGemLeveler] Failed to add LevelGemsTask before CombatTask (Leash 50).");
			}
			else
			{
				GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)currentBotTaskManager).AddBefore((ITask)(object)new Class54(), "CastAuraTask")) ? "[BasicGemLeveler] Failed to add LevelGemsTask before CastAuraTask" : "[BasicGemLeveler] LevelGemsTask Task added before CastAuraTask.");
			}
		}
		else
		{
			GlobalLog.Error("[BasicGemLeveler] TaskManager null. What?");
		}
	}

	public void Stop()
	{
	}

	public void Tick()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[BasicGemLeveler] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (LokiPoe.IsInGame && !bool_0)
		{
			LokiPoe.BeginDispatchIfNecessary((Action)delegate
			{
				BasicGemLevelerSettings.Instance.RefreshSkillGemsList();
			});
			bool_0 = true;
		}
	}

	private void BotManagerOnPostStart(IBot bot)
	{
		Reset();
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

	private void Reset()
	{
		bool_0 = false;
	}

	public override string ToString()
	{
		return Name + ": " + Description;
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

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public static async Task<bool> OpenInventoryPanel(int timeout = 5000)
	{
		GlobalLog.Debug("[OpenInventoryPanel]");
		Stopwatch sw = Stopwatch.StartNew();
		if (!InventoryUi.IsOpened)
		{
			await Coroutines.CloseBlockingWindows();
		}
		while (!InventoryUi.IsOpened)
		{
			GlobalLog.Debug("[OpenInventoryPanel] The InventoryUi is not opened. Now opening it.");
			if (sw.ElapsedMilliseconds <= timeout)
			{
				if (!((Actor)LokiPoe.Me).IsDead)
				{
					Input.SimulateKeyEvent(Binding.open_inventory_panel, true, false, false, Keys.None);
					await Coroutines.ReactionWait();
					continue;
				}
				GlobalLog.Debug("[OpenInventoryPanel] We are now dead.");
				return false;
			}
			GlobalLog.Debug("[OpenInventoryPanel] Timeout.");
			return false;
		}
		return true;
	}

	public static bool ContainsHelper(string name, int level)
	{
		foreach (string globalNameIgnore in BasicGemLevelerSettings.Instance.GlobalNameIgnoreList)
		{
			string[] array = globalNameIgnore.Split(',');
			if (array.Length == 1)
			{
				if (array[0].Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			else if (array[0].Equals(name, StringComparison.OrdinalIgnoreCase) && level >= Convert.ToInt32(array[1]))
			{
				return true;
			}
		}
		return false;
	}

	static BasicGemLeveler()
	{
		interval_0 = new Interval(500);
		LevelWait = WaitTimer.FiveSeconds;
		NeedsToUpdate = true;
	}
}
