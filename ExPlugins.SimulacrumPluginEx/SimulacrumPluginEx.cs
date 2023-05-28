using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;
using ExPlugins.PapashaCore;
using ExPlugins.SimulacrumPluginEx.Tasks;

namespace ExPlugins.SimulacrumPluginEx;

public class SimulacrumPluginEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	private static readonly Interval interval_0;

	public static TaskManager BotTaskManager;

	public static int CurrentWave;

	private SimulacrumGui simulacrumGui_0;

	private static int int_0;

	public string Name => "SimulacrumPluginEx";

	public string Description => "Plugin that handles Simulacrums.";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public UserControl Control => simulacrumGui_0 ?? (simulacrumGui_0 = new SimulacrumGui());

	public JsonSettings Settings => (JsonSettings)(object)SimulSett.Instance;

	public MessageResult Message(Message message)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (id == "MB_new_map_entered_event")
		{
			CurrentWave = 0;
			int_0 = new Random().Next(-10, 10);
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[" + Name + "] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		BotTaskManager = GetCurrentBotTaskManager();
		((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new StartSimulacrumTask(), "SortInventoryTask");
		((TaskManagerBase<ITask>)(object)BotTaskManager).AddAfter((ITask)(object)new StayInSimulacrumTask(), "LootItemTask");
		((TaskManagerBase<ITask>)(object)BotTaskManager).AddBefore((ITask)(object)new PrepareForWaveTask(), "CombatTask (Leash 50)");
		int_0 = new Random().Next(-10, 10);
	}

	public void Tick()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[" + Name + "] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
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

	public static WalkablePosition GetCoords(string areaName, bool findStash = false)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		SimulSett.SimulSettNotify @class = SimulSett.Instance.AnchorPoints.FirstOrDefault((SimulSett.SimulSettNotify p) => p.Layout.Equals(areaName));
		if (@class != null)
		{
			Vector2i stashCoords = default(Vector2i);
			((Vector2i)(ref stashCoords))._002Ector(@class.Coords.X + int_0, @class.Coords.Y + int_0);
			string text = "anchor";
			if (findStash)
			{
				text = "stash";
				stashCoords = @class.StashCoords;
			}
			return new WalkablePosition(areaName + " " + text, stashCoords, 5, 10);
		}
		GlobalLog.Warn("[SimulacrumPluginEX] Error: Can't find anchor point for layout " + areaName);
		return null;
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

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public void Stop()
	{
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static SimulacrumPluginEx()
	{
		interval_0 = new Interval(500);
	}
}
