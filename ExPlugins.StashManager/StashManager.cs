using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.StashManager;

public class StashManager : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider
{
	public static TaskManager BotTaskManager;

	private static readonly Interval interval_0;

	private StashManagerGui stashManagerGui_0;

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "StashManager";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "0.1";

	public JsonSettings Settings => (JsonSettings)(object)StashManagerSettings.Instance;

	public UserControl Control => stashManagerGui_0 ?? (stashManagerGui_0 = new StashManagerGui());

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
	}

	public void Disable()
	{
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
		TaskManager output = val.GetOutput<TaskManager>(0);
		if (output != null)
		{
			return output;
		}
		return null;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[StashManager] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GlobalLog.Debug("[StashManager] Start.");
		BotTaskManager = GetCurrentBotTaskManager();
		AddTasks();
	}

	private static void AddTasks()
	{
		GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class16(), "StashTask") ? "[StashManager] AddAfter StashTask, ScanTabsAndSellItemsTask Sucess" : "[StashManager] AddAfter StashTask, ScanTabsAndSellItemsTask Failed");
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class15(), "StashTask")) ? "[StashManager] AddAfter StashTask, ClusterRecipeTask Failed" : "[StashManager] AddAfter StashTask, ClusterRecipeTask Sucess");
		GlobalLog.Debug(((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new Class17(), "StashTask") ? "[StashManager] AddAfter StashTask, ScanTabsAndSellMapsTask Sucess" : "[StashManager] AddAfter StashTask, ScanTabsAndSellMapsTask Failed");
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[StashManager] Please enable PapashaCore or disable this plugin to proceed.");
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

	static StashManager()
	{
		interval_0 = new Interval(500);
	}
}
