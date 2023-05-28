using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;

internal class Class7 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool bool_0;

	private readonly TraderPluginSettings traderPluginSettings_0 = TraderPluginSettings.Instance;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "ScanTradeTabTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!bool_0)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (area.IsTown || area.IsHideoutArea)
			{
				if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
				{
					await Inventories.OpenStash();
				}
				await Inventories.OpenStashTab(traderPluginSettings_0.StashTabToTrade, Name);
				GlobalLog.Debug("[ScanTradeTabTask] Path: " + TraderPlugin.FullFileName);
				List<Class9.Class10> mnePohui = new List<Class9.Class10>();
				foreach (Item item in StashUi.InventoryControl.Inventory.Items)
				{
					GlobalLog.Debug($"[ScanTradeTabTask] Item parsed: {item.FullName}, item position: {item.LocationTopLeft}");
					mnePohui.Add(new Class9.Class10(itemPrice: string.IsNullOrEmpty(item.DisplayNote) ? "" : item.DisplayNote, itemPos: item.LocationTopLeft, itemName: item.FullName, stackCount: item.StackCount, listDate: null, influenceType: InfluenceHelper.GetInfluence(item)));
				}
				bool_0 = true;
				GlobalLog.Debug("[ScanTradeTabTask] Trade tab is scanned");
				File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui, (Formatting)1));
				return false;
			}
			return false;
		}
		return false;
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
}
