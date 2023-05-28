using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BlightPluginEx.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;

namespace ExPlugins.BlightPluginEx;

public class BlightPluginEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	private static readonly Interval interval_0;

	private BlightGui blightGui_0;

	public static Monster Cassia => ObjectManager.Objects.FirstOrDefault((Monster a) => ((NetworkObject)a).Name.Equals("Sister Cassia"));

	public static Monster ClosestCassia => ((IEnumerable)ObjectManager.Objects).Closest<Monster>((Func<Monster, bool>)((Monster a) => ((NetworkObject)a).Name.Equals("Sister Cassia")));

	public static NetworkObject ClosestBlightCore => (from o in ObjectManager.Objects
		where o.Metadata.Equals("Metadata/Terrain/Leagues/Blight/Objects/BlightPump")
		orderby o.Distance
		select o).FirstOrDefault();

	public static NetworkObject FreshBlightCore => ObjectManager.Objects.FirstOrDefault((NetworkObject o) => o.Metadata.Equals("Metadata/Terrain/Leagues/Blight/Objects/BlightPump") && o.Components.TransitionableComponent.Flag1 == 1);

	public static CachedObject CachedCassia
	{
		get
		{
			return CombatAreaCache.Current.Storage["SisterCassia"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["SisterCassia"] = value;
		}
	}

	public static CachedObject CachedBlightCore
	{
		get
		{
			return CombatAreaCache.Current.Storage["Core"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Core"] = value;
		}
	}

	public string Name => "BlightPluginEx";

	public string Description => "Plugin that handles Blight event.";

	public string Author => "Seusheque";

	public string Version => "3.1";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public UserControl Control => blightGui_0 ?? (blightGui_0 = new BlightGui());

	public JsonSettings Settings => (JsonSettings)(object)Class53.Instance;

	public MessageResult Message(Message message)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (id == "MB_new_map_entered_event")
		{
			DoWhenReset();
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[BlightPluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		TaskManager taskManager = BotStructure.TaskManager;
		((TaskManagerBase<ITask>)(object)taskManager).AddBefore((ITask)(object)new UpgradeTowerTask(), "CombatTask (Leash 50)");
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new HandleEventTask(), "CombatTask (Leash 50)");
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new StartEventTask(), "LootItemTask");
		if (Class53.Instance.TowerUpgradeDistance < 80)
		{
			Class53.Instance.TowerUpgradeDistance = 100;
		}
	}

	public void Tick()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[BlightPluginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if (!currentArea.IsMap && !currentArea.IsOverworldArea)
		{
			return;
		}
		Class53.Instance.SelectBlightControlTower();
		Class53.Instance.SelectRegularControlTower();
		Class53.Instance.SelectDpsTower();
		Class53.Instance.UpgradedTowers = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower t) => t.Tier >= 3);
		if (FreshBlightCore != (NetworkObject)null && !Blight.IsEncounterRunning)
		{
			CachedObject cachedObject = new CachedObject(FreshBlightCore);
			if (CachedBlightCore != cachedObject)
			{
				CachedBlightCore = new CachedObject(FreshBlightCore);
				DoWhenReset(resetCounter: false);
			}
		}
		if (CachedBlightCore == null && ClosestBlightCore != (NetworkObject)null)
		{
			CachedBlightCore = new CachedObject(ClosestBlightCore);
		}
		if (CachedBlightCore != null && ClosestBlightCore != (NetworkObject)null && (CachedBlightCore.Object == (NetworkObject)null || (Vector2i)CachedBlightCore.Position != ClosestBlightCore.Position))
		{
			CachedBlightCore = new CachedObject(ClosestBlightCore);
		}
		if (CachedCassia != null && CachedCassia.Object == (NetworkObject)null && (NetworkObject)(object)ClosestCassia != (NetworkObject)null)
		{
			CachedCassia = new CachedObject((NetworkObject)(object)ClosestCassia);
		}
		if (CachedCassia == null && (NetworkObject)(object)Cassia != (NetworkObject)null)
		{
			CachedCassia = new CachedObject((NetworkObject)(object)Cassia);
		}
	}

	internal static void DoWhenReset(bool resetCounter = true)
	{
		if (resetCounter)
		{
			HandleEventTask.FinishedOnThisMap = 0;
			GlobalLog.Warn("[BlightPluginEx] Reset.");
		}
		HandleEventTask.SkipEncounter = false;
		StartEventTask.Started = false;
		StartEventTask.FailedBlightInteract = 0;
		HandleEventTask.EncounterTimeoutSwSmall = null;
		HandleEventTask.EncounterTimeoutSwBig = null;
		HandleEventTask.EncounterSw = null;
		HandleEventTask.Comeback = null;
		HandleEventTask.Failcount = 0;
		HandleEventTask.SpawnerList.Clear();
		UpgradeTowerTask.stopwatch_0 = null;
		Class53.weightedTower_0 = null;
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

	static BlightPluginEx()
	{
		interval_0 = new Interval(500);
	}
}
