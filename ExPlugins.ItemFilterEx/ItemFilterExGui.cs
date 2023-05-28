using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DreamPoeBot.Framework.Helpers;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.ItemFilterEx;

public partial class ItemFilterExGui : UserControl, IComponentConnector, IStyleConnector
{
	private bool bool_0;

	public ItemFilterExGui()
	{
		InitializeComponent();
		base.DataContext = ItemFilterExSettings.Instance;
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		LinqHelper.ForEach<CurrencyTrackerEntry>((IEnumerable<CurrencyTrackerEntry>)ItemFilterExSettings.Instance.ShownEntries, (Action<CurrencyTrackerEntry>)delegate(CurrencyTrackerEntry t)
		{
			t.Update();
		});
	}

	private void DeleteStat_OnClick(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		string item = (string)button.DataContext;
		OfficialPricecheckEntry officialPricecheckEntry = (OfficialPricecheckEntry)button.Tag;
		officialPricecheckEntry.StatsToCheck.Remove(item);
	}

	private void DeleteRuleButton_OnClick(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		OfficialPricecheckEntry item = (OfficialPricecheckEntry)button.DataContext;
		ItemFilterExSettings.Instance.OfficialPricecheck.Remove(item);
	}

	private void AddRuleButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (LokiPoe.IsInGame)
		{
			List<Item> source = Inventories.InventoryItems.Where((Item i) => !i.Name.Contains("Scroll") && (int)i.Rarity == 3).ToList();
			if (source.Any())
			{
				foreach (Item item in source.Where((Item it) => ItemFilterExSettings.Instance.OfficialPricecheck.All((OfficialPricecheckEntry i) => i.FullName != it.FullName)))
				{
					GlobalLog.Warn("[ItemFilterEx] Adding [" + item.FullName + "] to official trade pricecheck");
					ItemFilterExSettings.Instance.OfficialPricecheck.Add(new OfficialPricecheckEntry(enabled: true, item.FullName, checkStats: false, new ObservableCollection<string>(item.Affixes.Select((ModAffix a) => a.Stats.First().Description.Replace("{0:+d}", "#").Replace("{0}", "#").Replace("\\n", " "))), checkIlvl: false, checkCorrupted: false));
				}
				return;
			}
			GlobalLog.Error("[ItemFilterEx] Can't find any unique item in inventory!");
		}
		else
		{
			GlobalLog.Error("[ItemFilterEx] Not in game!");
		}
	}
}
