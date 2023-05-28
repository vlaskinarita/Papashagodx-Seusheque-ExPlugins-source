using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.TraderPlugin.Classes;

namespace ExPlugins.TraderPlugin.Tasks;

public class EvaluateItemTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static List<int> BlackList;

	private readonly CachedItem cachedItem_0 = new CachedItem(new Item());

	private readonly Vector2i vector2i_0 = default(Vector2i);

	private Vector2i vector2i_1;

	private CachedItem cachedItem_1 = new CachedItem(new Item());

	public string Name => "EvaluateItemTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!(vector2i_1 == vector2i_0) || cachedItem_1 != cachedItem_0)
		{
			bool found = false;
			Item item = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(vector2i_1);
			if ((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null || vector2i_1 == vector2i_0)
			{
				if ((cachedItem_1 != cachedItem_0 && (cachedItem_1.Class != "StackableCurrency" || (cachedItem_1.Name.Contains("Delirium Orb") && TraderPluginSettings.Instance.ShouldSellDeliriumOrbs) || (cachedItem_1.Name.Contains("Oil") && TraderPluginSettings.Instance.ShouldSellOils) || (cachedItem_1.Name.Contains("Catalyst") && TraderPluginSettings.Instance.ShouldSellCatalysts) || (cachedItem_1.Name.Contains("Blessing") && TraderPluginSettings.Instance.ShouldSellBlessings) || (cachedItem_1.Name.Contains("Remnant of Corruption") && TraderPluginSettings.Instance.ShouldSellEssences) || (cachedItem_1.Name.Contains("Essence") && TraderPluginSettings.Instance.ShouldSellEssences)) && !cachedItem_1.Class.Contains("Flask")) || (cachedItem_1.Metadata.Contains("ItemisedProphecy") && TraderPluginSettings.Instance.ShouldSellProphecy) || (cachedItem_1.Class == "ExpeditionLogbook" && TraderPluginSettings.Instance.ShouldSellLogbooks) || (cachedItem_1.Name.ContainsIgnorecase("Blueprint:") && TraderPluginSettings.Instance.ShouldSellBlueprints) || (cachedItem_1.Name.ContainsIgnorecase("Contract:") && TraderPluginSettings.Instance.ShouldSellContracts))
				{
					if (TraderPluginSettings.Instance.DebugMode)
					{
						GlobalLog.Debug("[EvaluateItem] trying to find identified item");
					}
					foreach (Item item_0 in InventoryUi.InventoryControl_Main.Inventory.Items)
					{
						if (TraderPluginSettings.Instance.ItemsToIgnore.Any((TraderPluginSettings.NameEntry i) => i.Name == item_0.Name) || (item_0.Stats.ContainsKey((StatTypeGGG)10342) && !TraderPluginSettings.Instance.ShouldSellBlightedMaps) || (item_0.Stats.ContainsKey((StatTypeGGG)14763) && !TraderPluginSettings.Instance.ShouldSellBlightRavagedMaps) || (item_0.Class == "DivinationCard" && item_0.HasFullStack) || !TraderPluginSettings.Instance.ShouldSellDivCards || ((BlackList.Contains(item_0.LocalId) || (!(item_0.Class != "StackableCurrency") && (!item_0.Name.Contains("Delirium Orb") || !TraderPluginSettings.Instance.ShouldSellDeliriumOrbs) && (!item_0.Name.Contains("Oil") || !TraderPluginSettings.Instance.ShouldSellOils) && (!item_0.Name.Contains("Catalyst") || !TraderPluginSettings.Instance.ShouldSellCatalysts) && (!item_0.Name.Contains("Stacked Deck") || !TraderPluginSettings.Instance.ShouldSellStackedDicks))) && (!item_0.Metadata.Contains("ItemisedProphecy") || !TraderPluginSettings.Instance.ShouldSellProphecy) && !Class6.ItemsToList.Contains(item_0.Name) && (!(item_0.Class == "ExpeditionLogbook") || !TraderPluginSettings.Instance.ShouldSellLogbooks)) || !(item_0.FullName + item_0.Name == cachedItem_1.FullName + cachedItem_1.Name))
						{
							continue;
						}
						if ((int)item_0.Rarity == 2 || (int)item_0.Rarity == 1)
						{
							await Inventories.OpenInventory();
							InventoryUi.InventoryControl_Main.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory y, Item x) => x.FullName + x.Name == cachedItem_1.FullName + cachedItem_1.Name), (Func<bool>)(() => InventoryUi.IsOpened));
							await Coroutines.CloseBlockingWindows();
						}
						found = true;
						item = item_0;
						BlackList.Add(item_0.LocalId);
						break;
					}
					if (found)
					{
						if (TraderPluginSettings.Instance.DebugMode)
						{
							GlobalLog.Debug("[EvaluateItem] " + item.Name + " has been added to the list1");
						}
						ItemSearchParams Par = new ItemSearchParams(item, item.LocationTopLeft);
						TraderPlugin.ItemSearch.Enqueue(Par);
					}
					vector2i_1 = vector2i_0;
					cachedItem_1 = cachedItem_0;
					return false;
				}
				vector2i_1 = vector2i_0;
				cachedItem_1 = cachedItem_0;
				return false;
			}
			if ((((item.Class == "Ring" || item.Class == "Amulet" || item.Class == "Jewel" || item.Class == "AbyssJewel" || item.Class == "Belt") && (int)item.Rarity == 2) || (item.Class == "Map" && TraderPluginSettings.Instance.ShouldSellMaps) || ((int)item.Rarity == 3 && TraderPluginSettings.Instance.ShouldSellUniques) || (item.Class == "MapFragment" && TraderPluginSettings.Instance.ShouldSellMapFragments) || (item.Class == "DivinationCard" && TraderPluginSettings.Instance.ShouldSellDivCards) || ((int)item.Rarity == 4 && TraderPluginSettings.Instance.ShouldSellGems) || (item.Class == "ExpeditionLogbook" && TraderPluginSettings.Instance.ShouldSellLogbooks) || (item.Name.ContainsIgnorecase("Blueprint:") && TraderPluginSettings.Instance.ShouldSellBlueprints) || (item.Name.ContainsIgnorecase("Contract:") && TraderPluginSettings.Instance.ShouldSellContracts)) && !BlackList.Contains(item.LocalId))
			{
				if (item.Class == "Map" && ((item.Stats.ContainsKey((StatTypeGGG)10342) && !TraderPluginSettings.Instance.ShouldSellBlightedMaps) || (item.Stats.ContainsKey((StatTypeGGG)14763) && !TraderPluginSettings.Instance.ShouldSellBlightRavagedMaps)))
				{
					return false;
				}
				if (TraderPluginSettings.Instance.DebugMode)
				{
					GlobalLog.Debug("[EvaluateItem] " + item.Name + " has been added to the list");
				}
				BlackList.Add(item.LocalId);
				ItemSearchParams Params = new ItemSearchParams(item, item.LocationTopLeft);
				TraderPlugin.ItemSearch.Enqueue(Params);
			}
			vector2i_1 = vector2i_0;
			cachedItem_1 = cachedItem_0;
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "ItemIdentified")
		{
			vector2i_1 = message.GetInput<Vector2i>(0);
			if (TraderPluginSettings.Instance.DebugMode)
			{
				GlobalLog.Debug("[EvaluateItem] Recieved message about IDing " + ((object)(Vector2i)(ref vector2i_1)).ToString());
			}
			return (MessageResult)0;
		}
		if (message.Id == "Identified Item Looted")
		{
			cachedItem_1 = message.GetInput<CachedItem>(0);
			if (TraderPluginSettings.Instance.DebugMode)
			{
				GlobalLog.Debug("[EvaluateItem] Recieved message about picking ID item " + cachedItem_1.FullName);
			}
			return (MessageResult)0;
		}
		if (message.Id == "items_stashed_event")
		{
			if (TraderPluginSettings.Instance.DebugMode)
			{
				GlobalLog.Debug("[EvaluateItem] All items were stashed, clearing the blacklist");
			}
			BlackList.Clear();
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static EvaluateItemTask()
	{
		BlackList = new List<int>();
	}
}
