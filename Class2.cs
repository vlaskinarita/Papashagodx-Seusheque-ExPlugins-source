using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;

internal class Class2 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool bool_0;

	public static int int_0;

	private readonly int int_1 = 600;

	public JsonSettings Settings => (JsonSettings)(object)TraderPluginSettings.Instance;

	public string Name => "CacheTradeTabTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!(TraderPlugin.Stopwatch.Elapsed.TotalSeconds - (double)int_0 < (double)int_1) || int_0 == 0 || bool_0)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsTown && !area.IsHideoutArea)
			{
				return false;
			}
			List<Class9.Class10> mnePohui;
			while (true)
			{
				if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
				{
					await Inventories.OpenStash();
				}
				await Inventories.OpenStashTab(TraderPluginSettings.Instance.StashTabToTrade, Name);
				Thread.Sleep(500);
				mnePohui = new List<Class9.Class10>();
				try
				{
					GlobalLog.Debug("[CacheTradeTabTask] Path: " + TraderPlugin.FullFileName);
					foreach (Item item in StashUi.InventoryControl.Inventory.Items)
					{
						Thread.Sleep(5);
						string note = ((!string.IsNullOrEmpty(item.DisplayNote)) ? item.DisplayNote : "");
						if (TraderPluginSettings.Instance.DebugMode)
						{
							string name = item.Name;
							Vector2i locationTopLeft = item.LocationTopLeft;
							GlobalLog.Debug("[CacheTradeTabTask] Item parsed: " + name + ", item position: " + ((object)(Vector2i)(ref locationTopLeft)).ToString());
						}
						mnePohui.Add(new Class9.Class10(item.LocationTopLeft, item.FullName, note, item.StackCount, null, InfluenceHelper.GetInfluence(item)));
					}
				}
				catch (Exception ex)
				{
					GlobalLog.Error($"[CacheException] {ex}");
					continue;
				}
				break;
			}
			int_0 = (int)TraderPlugin.Stopwatch.Elapsed.TotalSeconds;
			GlobalLog.Debug("[CacheTradeTabTask] Trade tab is scanned");
			File.WriteAllText(TraderPlugin.FullFileName, JsonConvert.SerializeObject((object)mnePohui, (Formatting)1));
			bool_0 = false;
			return false;
		}
		return false;
	}

	public void Initialize()
	{
		GlobalLog.Debug("[TraderPlugin] Initialize");
	}

	public void Deinitialize()
	{
		GlobalLog.Debug("[TraderPlugin] Deinitialize");
	}

	public void Enable()
	{
		GlobalLog.Warn("[TraderPlugin] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[TraderPlugin] Disabled");
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
