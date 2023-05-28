using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EquipPluginEx;
using ExPlugins.EquipPluginEx.Classes;
using ExPlugins.EquipPluginEx.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;

internal class Class26 : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider
{
	public static object object_0;

	private static readonly Interval interval_0;

	private EquipPluginExGui equipPluginExGui_0;

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "EquipPluginEx";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "13.37";

	public JsonSettings Settings => (JsonSettings)(object)Class27.Instance;

	public UserControl Control => equipPluginExGui_0 ?? (equipPluginExGui_0 = new EquipPluginExGui());

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
		GlobalLog.Warn("[EquipPluginEx] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[EquipPluginEx] Disabled");
		CombatAreaCache.RemovePickupItemEvaluator("EquipPluginExEvaluator");
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
		if (output == null)
		{
			return null;
		}
		return output;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready)
		{
			GlobalLog.Error("[EquipPluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GlobalLog.Debug("[EquipPluginEx] Start.");
		object_0 = GetCurrentBotTaskManager();
		AddTasks();
	}

	private static void AddTasks()
	{
		GlobalLog.Debug((!((TaskManagerBase<ITask>)object_0).AddBefore((ITask)(object)new CheckAndEquipItems(), "SellTask")) ? "[EquipPluginEx] AddBefore SellTask, CheckAndEquipItems Failed" : "[EquipPluginEx] AddBefore SellTask, CheckAndEquipItems Sucess");
		GlobalLog.Debug(((TaskManagerBase<ITask>)object_0).AddAfter((ITask)(object)new Class30(), "StashTask") ? "[EquipPluginEx] AddAfter StashTask, EquipGemsFromInventory Sucess" : "[EquipPluginEx] AddAfter StashTask, EquipGemsFromInventory Failed");
		GlobalLog.Debug((!((TaskManagerBase<ITask>)object_0).AddAfter((ITask)(object)new Class29(), "StashTask")) ? "[EquipPluginEx] AddAfter StashTask, BuyGemsFromVendor Failed" : "[EquipPluginEx] AddAfter StashTask, BuyGemsFromVendor Sucess");
		CombatAreaCache.AddPickupItemEvaluator("EquipPluginExEvaluator", EquipPluginExEvaluator.Evaluator);
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready))
		{
			GlobalLog.Error("[EquipPluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
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

	public void Stop()
	{
	}

	static Class26()
	{
		interval_0 = new Interval(500);
	}
}
