using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EssencePluginEx;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

internal class Class25 : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	private static readonly Interval interval_0;

	private ExPlugins.EssencePluginEx.Gui gui_0;

	public string Name => "EssencePluginEx";

	public string Author => "Bossland GmbH //Seusheque mod";

	public string Description => "Ported EB Monoliths plugin.";

	public string Version => "2.0.0.0";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public JsonSettings Settings => (JsonSettings)(object)EssencePluginExSettings.Instance;

	public UserControl Control => gui_0 ?? (gui_0 = new ExPlugins.EssencePluginEx.Gui());

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public override string ToString()
	{
		return Name + ": " + Description;
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
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

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready)
		{
			GlobalLog.Error("[EssencePluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		TaskManager taskManager = BotStructure.TaskManager;
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new HandleEssenceTask(), "CombatTask (Leash -1)");
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready))
		{
			GlobalLog.Error("[EssencePluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
	}

	public void Stop()
	{
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	static Class25()
	{
		interval_0 = new Interval(500);
	}
}
