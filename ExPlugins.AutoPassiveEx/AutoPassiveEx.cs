using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.AutoPassiveEx;

public class AutoPassiveEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider, ITickEvents
{
	private static readonly Interval interval_0;

	public static int ErrorCount;

	private readonly TaskManager taskManager_0 = new TaskManager();

	private AutoPassiveExGui autoPassiveExGui_0;

	public string Author => "Seusheque";

	public string Description => "A plugin that assigns character and atlas passives";

	public string Name => "AutoPassiveEx";

	public string Version => "0.0.1";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public JsonSettings Settings => (JsonSettings)(object)AutoPassiveExSettings.Instance;

	public UserControl Control => autoPassiveExGui_0 ?? (autoPassiveExGui_0 = new AutoPassiveExGui());

	public async Task<LogicResult> Logic(Logic logic)
	{
		if (logic.Id == "hook_post_combat")
		{
			return (LogicResult)((int)(await ((TaskManagerBase<ITask>)(object)taskManager_0).Run((TaskGroup)1, (RunBehavior)1)) != 1);
		}
		return await ((TaskManagerBase<ITask>)(object)taskManager_0).ProvideLogic((TaskGroup)1, (RunBehavior)1, logic);
	}

	public void Enable()
	{
		SkillsUi.OnPassiveDump += CharacterSkillsPanelOnOnPassiveDump;
		AtlasSkillsUi.OnPassiveDump += AtlasSkillsPanelOnOnPassiveDump;
	}

	public void Disable()
	{
		SkillsUi.OnPassiveDump -= CharacterSkillsPanelOnOnPassiveDump;
		AtlasSkillsUi.OnPassiveDump -= AtlasSkillsPanelOnOnPassiveDump;
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
		((TaskManagerBase<ITask>)(object)taskManager_0).Reset();
		((TaskManagerBase<ITask>)(object)taskManager_0).Add((ITask)(object)new Class55());
		((TaskManagerBase<ITask>)(object)taskManager_0).Freeze();
		((TaskManagerBase<ITask>)(object)taskManager_0).Start();
		GlobalLog.Debug("[" + Name + "] Start.");
	}

	public void Stop()
	{
		((TaskManagerBase<ITask>)(object)taskManager_0).Stop();
	}

	private static void CharacterSkillsPanelOnOnPassiveDump(object sender, PassiveDumpEventArgs e)
	{
		DatPassiveSkillWrapper datPassiveSkillWrapper_0 = e.PassiveSkill;
		if (AutoPassiveExSettings.Instance.AddNode)
		{
			Application.Current.Dispatcher.BeginInvoke((Action)delegate
			{
				AutoPassiveExSettings.Instance.AddCharacterPoint(datPassiveSkillWrapper_0);
			});
		}
	}

	private static void AtlasSkillsPanelOnOnPassiveDump(object sender, AtlasPassiveDumpEventArgs e)
	{
		DatPassiveSkillWrapper datPassiveSkillWrapper_0 = e.PassiveSkill;
		if (AutoPassiveExSettings.Instance.AddNode)
		{
			Application.Current.Dispatcher.BeginInvoke((Action)delegate
			{
				AutoPassiveExSettings.Instance.AddAtlasPoint(datPassiveSkillWrapper_0);
			});
		}
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

	public void Deinitialize()
	{
	}

	public void Initialize()
	{
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	static AutoPassiveEx()
	{
		interval_0 = new Interval(500);
	}
}
