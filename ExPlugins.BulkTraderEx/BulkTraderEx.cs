using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using ExPlugins.AutoLoginEx;
using ExPlugins.BulkTraderEx.Helpers;
using ExPlugins.BulkTraderEx.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

namespace ExPlugins.BulkTraderEx;

public class BulkTraderEx : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, ITickEvents, IStartStopEvents
{
	private static readonly Interval interval_0;

	public static Dictionary<string, Stopwatch> BlacklistedSellers;

	public static Dictionary<string, Stopwatch> BlacklistedRecipes;

	public static int PluginCooldown;

	private static readonly BulkTraderExSettings Config;

	private readonly Interval interval_1 = new Interval(30000);

	private BulkTraderExGui bulkTraderExGui_0;

	private bool bool_0;

	private bool bool_1;

	public JsonSettings Settings => (JsonSettings)(object)BulkTraderExSettings.Instance;

	public UserControl Control => bulkTraderExGui_0 ?? (bulkTraderExGui_0 = new BulkTraderExGui());

	public string Name => "BulkTraderEx";

	public string Description => "A plugin to bulk trade using official trade website.";

	public string Author => "Alcor75/Seusheque";

	public string Version => "4.7.0.0";

	public void Initialize()
	{
		PluginCooldown = BulkTraderExSettings.Instance.PluginCooldown + LokiPoe.Random.Next(-15, 30);
		GlobalLog.Warn($"[CurrencyExchangePlugin] Plugin cooldown set to {PluginCooldown}");
		PopulateCurrencies().GetAwaiter().GetResult();
	}

	public void Disable()
	{
		Config.ShouldTrade = false;
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[BulkTraderEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		GlobalLog.Debug("[BulkTraderEx] Start.");
		if (bool_0)
		{
			string text = "[" + Name + "] Failed to populate currency list. Please restart DPB.";
			GlobalLog.Error(text);
			BotManager.Stop(new StopReasonData("populate_fail", text, (object)null), false);
		}
		Config.NextTrade = (int)Math.Round((double)PluginCooldown - TradeTask.TaskSw.Elapsed.TotalMinutes);
		Utility.BroadcastMessage((object)null, "next_trade_in", new object[1] { Config.NextTrade });
		TaskManager taskManager = BotStructure.TaskManager;
		((TaskManagerBase<ITask>)(object)taskManager).AddAfter((ITask)(object)new TradeTask(), "StashTask");
	}

	public void Tick()
	{
		if (interval_0.Elapsed)
		{
			foreach (KeyValuePair<string, Stopwatch> item in BlacklistedSellers.Where((KeyValuePair<string, Stopwatch> s) => s.Value.Elapsed.TotalMinutes > 4.0).ToList())
			{
				GlobalLog.Warn("[Blacklist] Removing " + item.Key);
				BlacklistedSellers.Remove(item.Key);
			}
			foreach (KeyValuePair<string, Stopwatch> item2 in BlacklistedRecipes.Where((KeyValuePair<string, Stopwatch> r) => r.Value.Elapsed.TotalMinutes > 30.0).ToList())
			{
				BlacklistedRecipes.Remove(item2.Key);
			}
		}
		if (interval_1.Elapsed)
		{
			Config.NextTrade = (int)Math.Round((double)PluginCooldown - TradeTask.TaskSw.Elapsed.TotalMinutes);
			Utility.BroadcastMessage((object)this, "next_trade_in", new object[1] { Config.NextTrade });
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

	private async Task PopulateCurrencies()
	{
		if (bool_1)
		{
			return;
		}
		string league = BulkTraderExSettings.Instance.DefaultLeague;
		if (string.IsNullOrWhiteSpace(league))
		{
			if (LokiPoe.IsInGame)
			{
				league = LokiPoe.Me.League;
				BulkTraderExSettings.Instance.DefaultLeague = league;
			}
			else
			{
				if (string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.LastLeague))
				{
					bool_0 = true;
					string error = "[" + Name + "] Default league is null or empty. Please check settings.";
					GlobalLog.Error(error);
					BotManager.Stop(new StopReasonData("league_null", error, (object)null), false);
					return;
				}
				BulkTraderExSettings.Instance.DefaultLeague = AutoLoginSettings.Instance.LastLeague;
				league = BulkTraderExSettings.Instance.DefaultLeague;
			}
		}
		await PoeNinjaTracker.Init(league);
		if (WebHelper.StaticJson == null)
		{
			await WebHelper.GetStaticJson();
		}
		if (WebHelper.StaticJson != null)
		{
			foreach (List<WebHelper.StaticJsonEntry> entries in WebHelper.StaticJson.result.Select((WebHelper.StaticJsonResult r) => r.entries))
			{
				foreach (WebHelper.StaticJsonEntry staticJsonEntry_0 in entries.Where((WebHelper.StaticJsonEntry e) => !e.text.ContainsIgnorecase("tier")))
				{
					CurrencyHelper.Currency existing = CurrencyHelper.Currencies.FirstOrDefault((CurrencyHelper.Currency c) => c.Name.EqualsIgnorecase(staticJsonEntry_0.text));
					if (existing == null)
					{
						CurrencyHelper.Currencies.Add(new CurrencyHelper.Currency(staticJsonEntry_0.text, staticJsonEntry_0.id, -1));
					}
					else
					{
						existing.Id = staticJsonEntry_0.id;
					}
				}
			}
		}
		int i = -1;
		foreach (CurrencyHelper.Currency currency in CurrencyHelper.Currencies.OrderBy((CurrencyHelper.Currency a) => a.Name))
		{
			i++;
			int stackSize = PoeNinjaTracker.GetStackSize(currency.Name);
			if (currency.Number != i)
			{
				currency.Number = i;
			}
			if (stackSize != -1)
			{
				currency.Stack = stackSize;
			}
			if (currency.Stack == -1 || currency.Stack == 0)
			{
				GlobalLog.Error($"[{Name}] Check MaxStack for {currency.Name} [{currency.Stack}]");
			}
		}
		HashSet<string> newList = (from c in CurrencyHelper.Currencies
			where c.Stack != -2
			orderby c.Name
			select c.Name).ToHashSet();
		BulkTraderExSettings.CurrencyNames = new ObservableCollection<string>(newList);
		bool_1 = true;
	}

	public override string ToString()
	{
		return Name + ": " + Description;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public void Stop()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
	}

	static BulkTraderEx()
	{
		interval_0 = new Interval(60000);
		BlacklistedSellers = new Dictionary<string, Stopwatch>();
		BlacklistedRecipes = new Dictionary<string, Stopwatch>();
		Config = BulkTraderExSettings.Instance;
	}
}
