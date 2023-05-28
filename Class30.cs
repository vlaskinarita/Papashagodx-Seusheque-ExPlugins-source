using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EquipPluginEx.Helpers;
using ExPlugins.EXtensions;

internal class Class30 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Class27 class27_0;

	public string Name => "EquipGemsFromInventory";

	public string Description => "Д bI О";

	public string Author => "Papashagodx";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			if (World.CurrentArea.IsHideoutArea || World.CurrentArea.IsTown)
			{
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Helmet.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_Head, "Helmet");
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Body Armour.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_Chest, "BodyArmour");
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Gloves.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_Gloves, "Gloves");
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Boots.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_Boots, "Boots");
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Main Hand.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_PrimaryMainHand, "MainHand");
				if (class27_0.DebugMode)
				{
					GlobalLog.Debug("[BuyGemsFromVendor] New going to socket gems in Off-Hand.");
				}
				await SearchAndEquipAllGemsForSlot(InventoryUi.InventoryControl_PrimaryOffHand, "OffHand");
				return false;
			}
			return false;
		}
		return false;
	}

	private static async Task<bool> SearchAndEquipAllGemsForSlot(InventoryControlWrapper inventoryControlWrapper, string slot)
	{
		switch (slot)
		{
		case "Helmet":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.HelmetGems), inventoryControlWrapper);
			break;
		case "BodyArmour":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.BodyArmourGems), inventoryControlWrapper);
			break;
		case "Gloves":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.GlovesGems), inventoryControlWrapper);
			break;
		case "Boots":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.BootsGems), inventoryControlWrapper);
			break;
		case "MainHand":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.MainHandGems), inventoryControlWrapper);
			break;
		case "OffHand":
			await GemHelper.SocketGems(GetGemsStringNames(class27_0.OffHandGems), inventoryControlWrapper);
			break;
		}
		return false;
	}

	private static List<string> GetGemsStringNames(ObservableCollection<Class27.Class28> gems)
	{
		List<string> list = new List<string>();
		foreach (Class27.Class28 gem in gems)
		{
			list.Add(gem.Name);
		}
		return list;
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

	public MessageResult Message(Message message)
	{
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

	static Class30()
	{
		class27_0 = Class27.Instance;
	}
}
