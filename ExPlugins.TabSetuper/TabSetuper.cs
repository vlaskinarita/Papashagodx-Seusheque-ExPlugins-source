using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.TabSetuper;

public class TabSetuper : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider
{
	public static TaskManager BotTaskManager;

	private static readonly Interval interval_0;

	private TabSetuperGui tabSetuperGui_0;

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "TabSetuper";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "0.1";

	public JsonSettings Settings => (JsonSettings)(object)TabSetuperSettings.Instance;

	public UserControl Control => tabSetuperGui_0 ?? (tabSetuperGui_0 = new TabSetuperGui());

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
		GlobalLog.Warn("[" + Name + "] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[" + Name + "] Disabled");
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
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

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[TabSetuper] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GlobalLog.Debug("[TabSetuper] Start.");
		BotTaskManager = GetCurrentBotTaskManager();
		AddTasks();
	}

	private static void AddTasks()
	{
		GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class14(), "ClearCursorTask") ? "[TabSetuper] AddAfter ClearCursorTask, SetupTabsTask Sucess" : "[TabSetuper] AddAfter ClearCursorTask, SetupTabsTask Failed");
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[TabSetuper] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
	}

	public void Stop()
	{
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

	static TabSetuper()
	{
		interval_0 = new Interval(500);
	}
}
