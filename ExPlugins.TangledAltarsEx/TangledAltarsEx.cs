using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.TangledAltarsEx;

public class TangledAltarsEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider
{
	public static TaskManager BotTaskManager;

	private static readonly Interval interval_0;

	private TangledAltarsGui tangledAltarsGui_0;

	public static List<TangleAltar> TangleAltars => ObjectManager.Objects.Where((TangleAltar a) => (((NetworkObject)a).Name.ContainsIgnorecase("TangleAltar") || ((NetworkObject)a).Name.ContainsIgnorecase("CleansingFireAltar")) && a.Options != null && a.Options.Count > 0).ToList();

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "TangledAltarsEx";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "13.37";

	public JsonSettings Settings => (JsonSettings)(object)TangledAltarsExSettings.Instance;

	public UserControl Control => tangledAltarsGui_0 ?? (tangledAltarsGui_0 = new TangledAltarsGui());

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
		GlobalLog.Warn("[TangledAltarsEx] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[TangledAltarsEx] Disabled");
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
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[TangledAltarsEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GlobalLog.Debug("[TangledAltarsEx] Start.");
		BotTaskManager = GetCurrentBotTaskManager();
		AddTasks();
	}

	private static void AddTasks()
	{
		GlobalLog.Debug((!((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new Class13(), "LootItemTask")) ? "[TangledAltarsEx] AddBefore LootItemTask, CheckAndBuildArchnemesis Failed" : "[TangledAltarsEx] AddBefore LootItemTask, CheckAndBuildArchnemesis Sucess");
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[TangledAltarsEx] Please enable PapashaCore or disable this plugin to proceed.");
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

	static TangledAltarsEx()
	{
		interval_0 = new Interval(500);
	}
}
