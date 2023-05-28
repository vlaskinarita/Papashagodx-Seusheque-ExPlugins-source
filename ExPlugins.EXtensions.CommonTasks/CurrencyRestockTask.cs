using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.CommonTasks;

public class CurrencyRestockTask : ErrorReporter, ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly List<string> list_0;

	public string Name => "CurrencyRestockTask";

	public string Description => "Task that takes currency from stash if needed.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea || area.Id.Contains("Affliction"))
		{
			if (!(Inventories.Stash == (NetworkObject)null))
			{
				HashSet<string> errors = GetUserErrors();
				if (errors.Count > 0)
				{
					foreach (string error in errors)
					{
						GlobalLog.Error(error);
					}
					BotManager.Stop(new StopReasonData("cant_restock", "Can't restock currency. Max errors reached. Stopping the bot", (object)null), false);
					return true;
				}
				List<string> currencyToRestock = new List<string>();
				foreach (ExtensionsSettings.InventoryCurrency currency2 in ExtensionsSettings.Instance.InventoryCurrencies)
				{
					string name = currency2.Name;
					int restock = currency2.Restock;
					if (restock < 0)
					{
						continue;
					}
					GetCurrentAndMaxStackCount(name, out var count, out var maxStackCount);
					if (restock < maxStackCount)
					{
						if (count <= restock)
						{
							if (!list_0.Contains(name))
							{
								GlobalLog.Debug($"[CurrencyRestockTask] Restock is needed for \"{name}\". Current count: {count}. Count to restock: {restock}.");
								currencyToRestock.Add(name);
							}
							else
							{
								GlobalLog.Debug("[CurrencyRestockTask] Skipping \"" + name + "\" restock because it is marked as unavailable.");
							}
						}
						continue;
					}
					GlobalLog.Error($"[InventoryCurrency] Invalid restock value for \"{name}\". Restock: {restock}. Max stack count: {maxStackCount}.");
					GlobalLog.Error("[InventoryCurrency] Restock value must be less than item's max stack count. Please correct your settings.");
					BotManager.Stop(new StopReasonData("wrong_config", $"Invalid restock value for \"{name}\". Restock: {restock}. Max stack count: {maxStackCount}.", (object)null), false);
					return true;
				}
				if (currencyToRestock.Count != 0)
				{
					foreach (string currency in currencyToRestock)
					{
						GlobalLog.Debug("[CurrencyRestockTask] Now going to restock \"" + currency + "\".");
						switch (await Inventories.WithdrawCurrency(currency))
						{
						case WithdrawResult.Unavailable:
							GlobalLog.Warn("[CurrencyRestockTask] There are no \"" + currency + "\" in all tabs assigned to them. Now marking this currency as unavailable.");
							list_0.Add(currency);
							break;
						case WithdrawResult.Error:
							ReportError();
							return true;
						}
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Start()
	{
		ResetErrors();
		list_0.Clear();
	}

	public MessageResult Message(Message message)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "combat_area_changed_event")
		{
			ResetErrors();
			list_0.Clear();
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static HashSet<string> GetUserErrors()
	{
		HashSet<string> hashSet = new HashSet<string>();
		ObservableCollection<ExtensionsSettings.InventoryCurrency> inventoryCurrencies = ExtensionsSettings.Instance.InventoryCurrencies;
		foreach (ExtensionsSettings.InventoryCurrency item in inventoryCurrencies)
		{
			string string_0 = item.Name;
			if (!(string_0 == "CurrencyName"))
			{
				if (inventoryCurrencies.Count((ExtensionsSettings.InventoryCurrency c) => c.Name == string_0) > 1)
				{
					hashSet.Add("[InventoryCurrency] Duplicate name \"" + string_0 + "\". Please remove all duplicates.");
				}
				int int_0 = item.Row;
				int int_1 = item.Column;
				if (int_0 > 5)
				{
					hashSet.Add("[InventoryCurrency] Invalid Row value for \"" + string_0 + "\". Row cannot be greater than 5.");
				}
				if (int_1 > 12)
				{
					hashSet.Add("[InventoryCurrency] Invalid Column value for \"" + string_0 + "\". Column cannot be greater than 12.");
				}
				if (int_0 >= 1 && int_1 >= 1 && inventoryCurrencies.Count((ExtensionsSettings.InventoryCurrency c) => c.Row == int_0 && c.Column == int_1) > 1)
				{
					hashSet.Add($"[InventoryCurrency] Duplicate Row and Column combination {int_0},{int_1}. Please remove all duplicates.");
				}
			}
		}
		return hashSet;
	}

	private static void GetCurrentAndMaxStackCount(string name, out int count, out int maxStackCount)
	{
		count = 0;
		maxStackCount = PoeNinjaTracker.GetStackSize(name);
		foreach (Item item in Enumerable.Where(Inventories.InventoryItems, (Item i) => i.Name == name))
		{
			count += item.StackCount;
			maxStackCount = item.MaxStackCount;
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Stop()
	{
	}

	static CurrencyRestockTask()
	{
		list_0 = new List<string>();
	}
}
