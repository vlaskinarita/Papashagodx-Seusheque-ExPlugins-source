using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;

namespace ExPlugins.IncursionEx;

public class IncursionEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	private static readonly Interval interval_0;

	public static RoomEntry CurrentRoomSettings;

	public static List<Vector2i> BlacklistedTimeportalPositions;

	private Gui gui_0;

	public static Npc Alva => ObjectManager.Objects.FirstOrDefault((Npc a) => ((NetworkObject)a).Name.Equals("Alva, Master Explorer"));

	public static Monster Omnitect => ObjectManager.Objects.FirstOrDefault((Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata == "Metadata/Monsters/LeagueIncursion/VaalSaucerBoss");

	public static List<MinimapIconWrapper> PortalMinimapIcon => InstanceInfo.MinimapIcons.Where(delegate(MinimapIconWrapper x)
	{
		DatMinimapIconWrapper minimapIcon = x.MinimapIcon;
		return ((minimapIcon != null) ? minimapIcon.Name : null) == "IncursionCraftingBench";
	}).ToList();

	public static CachedObject CachedAlva
	{
		get
		{
			return CombatAreaCache.Current.Storage["AlvaValai"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["AlvaValai"] = value;
		}
	}

	public static CachedObject CachedPortal
	{
		get
		{
			return CombatAreaCache.Current.Storage["IncursionMinimapIcon"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["IncursionMinimapIcon"] = value;
		}
	}

	public static CachedObject CachedOmnitect
	{
		get
		{
			return CombatAreaCache.Current.Storage["VaalOmnitect"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["VaalOmnitect"] = value;
		}
	}

	public string Url => "https://discord.gg/HeqYtkujWW";

	public string Name => "IncursionEx";

	public string Description => "Plugin that handles incursions.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public JsonSettings Settings => (JsonSettings)(object)IncursionSettings.Instance;

	public MessageResult Message(Message message)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "explorer_local_transition_entered_event")
		{
			CombatAreaCache.ObjectDictionary storage = CombatAreaCache.Current.Storage;
			CachedObject cachedObject = storage["AlvaValai"] as CachedObject;
			if (cachedObject != null)
			{
				cachedObject.Unwalkable = false;
			}
			CachedObject cachedObject2 = storage["VaalOmnitect"] as CachedObject;
			if (cachedObject2 != null)
			{
				cachedObject2.Unwalkable = false;
			}
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
			GlobalLog.Error("[IncursionEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		ExilePather.BlockLockedTempleDoors = (FeatureEnum)1;
		ComplexExplorer.AddSettingsProvider("IncursionPlugin", IncursionExploration, ProviderPriority.High);
		TaskManager taskManager = BotStructure.TaskManager;
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new EnterIncursionTask(), "OpenChestTask");
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new HandleIncursionTask(), "CombatTask (Leash -1)");
		((TaskManagerBase<ITask>)(object)taskManager).AddBefore((ITask)(object)new ItemizeTempleTask(), "EnterIncursionTask");
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
			GlobalLog.Error("[IncursionEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if ((!currentArea.IsMap && (!currentArea.IsOverworldArea || CombatAreaCache.IsInIncursion)) || !(CachedPortal == null))
		{
			return;
		}
		List<MinimapIconWrapper> portalMinimapIcon = PortalMinimapIcon;
		if (portalMinimapIcon != null && portalMinimapIcon.Count != 0)
		{
			MinimapIconWrapper minimapIconWrapper_0 = portalMinimapIcon.Where((MinimapIconWrapper x) => x.NetworkObject != (NetworkObject)null && x.NetworkObject.Components.TransitionableComponent.Flag1 < 3).OrderBy(delegate(MinimapIconWrapper x)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				Vector2i position = x.NetworkObject.Position;
				return ((Vector2i)(ref position)).Distance(((NetworkObject)LokiPoe.Me).Position);
			}).FirstOrDefault();
			if (minimapIconWrapper_0 != null && !(minimapIconWrapper_0.NetworkObject == (NetworkObject)null) && !BlacklistedTimeportalPositions.Any((Vector2i x) => x == minimapIconWrapper_0.NetworkObject.Position) && minimapIconWrapper_0.NetworkObject.Components.TransitionableComponent.Flag1 != 3)
			{
				CachedPortal = new CachedObject(minimapIconWrapper_0.NetworkObject);
			}
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

	private static ExplorationSettings IncursionExploration()
	{
		return CombatAreaCache.IsInIncursion ? new ExplorationSettings(basicExploration: true, fastTransition: false, backtracking: false, openPortals: true, null, 7, 5) : null;
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

	static IncursionEx()
	{
		interval_0 = new Interval(1000);
		BlacklistedTimeportalPositions = new List<Vector2i>();
	}
}
