using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.CommonTasks;

public class SellTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "SellTask";

	public string Description => "Task that handles item selling.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea)
		{
			string fullParseProgressPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "State", "InParsingProcess.txt");
			while (File.Exists(fullParseProgressPath))
			{
				Thread.Sleep(1000);
			}
			List<Item> itemsToSell = new List<Item>();
			IItemEvaluator itemFilter = ItemEvaluator.Instance;
			foreach (Item item2 in Inventories.InventoryItems)
			{
				string c = item2.Class;
				if (!(c == "QuestItem") && !(c == "PantheonSoul") && !(item2.LocationTopLeft == StashTask.ProtectedSlot) && !item2.HasMicrotransitionAttachment && !item2.HasSkillGemsEquipped && itemFilter.Match(item2, (EvaluationType)3) && !itemFilter.Match(item2, (EvaluationType)2))
				{
					itemsToSell.Add(item2);
				}
			}
			if (itemsToSell.Count == 0)
			{
				return false;
			}
			GlobalLog.Info($"[SellTask] {itemsToSell.Count} items to sell.");
			foreach (Item item in itemsToSell)
			{
				double price = PoeNinjaTracker.LookupChaosValue(item);
				if ((price >= ExtensionsSettings.Instance.ValuableTreshold || !(price * (double)item.StackCount <= ExtensionsSettings.Instance.ValuableTreshold)) && ExtensionsSettings.Instance.StopBotOnVendoringValuableItem)
				{
					GlobalLog.Error($"[{Name}] Bot tried to vendor valuable item: [{item.FullName} stackCount:{item.StackCount} chaosValue: {price}]. Consider adding it to Always Keep list.");
					BotManager.Stop(new StopReasonData("valuable_item_vendored", $"Bot tried to vendor valuable item: [{item.FullName} stackCount:{item.StackCount} chaosValue: {price}]. Consider adding it to Always Keep list.", (object)null), false);
					return false;
				}
			}
			List<Vector2i> itemPosittions = itemsToSell.Select((Item i) => i.LocationTopLeft).ToList();
			if (!(await TownNpcs.SellItems(itemPosittions)))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
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
