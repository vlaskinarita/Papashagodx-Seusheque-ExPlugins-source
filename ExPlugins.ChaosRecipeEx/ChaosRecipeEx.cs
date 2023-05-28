using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;

namespace ExPlugins.ChaosRecipeEx;

public class ChaosRecipeEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider, ITickEvents
{
	public static readonly string StashDataPath;

	private static readonly Interval interval_0;

	public static readonly RecipeData StashData;

	public static readonly RecipeData PickupData;

	private Gui gui_0;

	public string Name => "ChaosRecipeEx";

	public string Description => "Plugin for doing chaos recipes.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public JsonSettings Settings => (JsonSettings)(object)global::ExPlugins.ChaosRecipeEx.Settings.Instance;

	public void Enable()
	{
		if (!CombatAreaCache.AddPickupItemEvaluator("ChaosRecipePickupEvaluator", ShouldPickup))
		{
			GlobalLog.Error("[ChaosRecipeEx] Fail to add pickup item evaluator.");
		}
	}

	public void Disable()
	{
		if (!CombatAreaCache.RemovePickupItemEvaluator("ChaosRecipePickupEvaluator"))
		{
			GlobalLog.Error("[ChaosRecipeEx] Fail to remove pickup item evaluator.");
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "area_changed_event")
		{
			DatWorldAreaWrapper input = message.GetInput<DatWorldAreaWrapper>(3);
			if (input.IsTown || input.IsHideoutArea)
			{
				GlobalLog.Info("[ChaosRecipeEx] Resetting pickup data.");
				PickupData.Reset();
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
			GlobalLog.Error("[ChaosRecipeEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		TaskManager taskManager = BotStructure.TaskManager;
		((TaskManagerBase<ITask>)(object)taskManager).AddBefore((ITask)(object)new StashRecipeTask(), "IdTask");
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new SellRecipeTask(), "VendorTask");
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[ChaosRecipeEx] Please enable PapashaCore or disable this plugin to proceed.");
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

	public static bool ShouldPickup(Item item)
	{
		if (RecipeData.IsItemForChaosRecipe(item, out var itemType))
		{
			if (StashData.GetItemCount(itemType) + PickupData.GetItemCount(itemType) < global::ExPlugins.ChaosRecipeEx.Settings.Instance.GetMaxItemCount(itemType))
			{
				GlobalLog.Warn($"[ChaosRecipeEx] Adding \"{item.Name}\" (iLvl: {item.ItemLevel}) for pickup.");
				PickupData.IncreaseItemCount(itemType);
				return true;
			}
			return false;
		}
		return false;
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

	static ChaosRecipeEx()
	{
		StashDataPath = Path.Combine(Configuration.Instance.Path, "ChaosRecipeStashData.json");
		interval_0 = new Interval(500);
		StashData = RecipeData.LoadFromJson(StashDataPath);
		PickupData = new RecipeData();
	}
}
