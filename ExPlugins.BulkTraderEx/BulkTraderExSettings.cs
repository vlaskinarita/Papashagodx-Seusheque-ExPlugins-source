using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using ExPlugins.AutoLoginEx;
using ExPlugins.BulkTraderEx.Classes;
using ExPlugins.BulkTraderEx.Helpers;
using Newtonsoft.Json;

namespace ExPlugins.BulkTraderEx;

public class BulkTraderExSettings : JsonSettings
{
	private static BulkTraderExSettings bulkTraderExSettings_0;

	public static ObservableCollection<string> CurrencyNames;

	private bool bool_0;

	private string string_0;

	private int int_0;

	private bool bool_1;

	[CompilerGenerated]
	private int int_1 = 30;

	[CompilerGenerated]
	private ObservableCollection<TradeRecipe> observableCollection_0 = new ObservableCollection<TradeRecipe>();

	public static BulkTraderExSettings Instance => bulkTraderExSettings_0 ?? (bulkTraderExSettings_0 = new BulkTraderExSettings());

	public int PluginCooldown
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
		[CompilerGenerated]
		set
		{
			int_1 = value;
		}
	}

	[DefaultValue(false)]
	public bool UseSsid
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => UseSsid));
		}
	}

	[DefaultValue(false)]
	public bool ShouldTrade
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => ShouldTrade));
		}
	}

	[DefaultValue("")]
	public string Poesessid
	{
		get
		{
			return AutoLoginSettings.Instance.Poesessid;
		}
		set
		{
			AutoLoginSettings.Instance.Poesessid = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Poesessid));
		}
	}

	[DefaultValue("")]
	public string DefaultLeague
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => DefaultLeague));
		}
	}

	public ObservableCollection<TradeRecipe> Recipes
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_0;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_0 = value;
		}
	}

	[JsonIgnore]
	public int NextTrade
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => NextTrade));
		}
	}

	private BulkTraderExSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"BulkTraderEx.json"
		}))
	{
		int num = 0;
		List<TradeRecipe> list = Recipes.OrderBy((TradeRecipe r) => r.Priority).ToList();
		foreach (TradeRecipe item in list)
		{
			num = (item.Priority = num + 1);
		}
		Recipes = new ObservableCollection<TradeRecipe>((from s in list
			where !string.IsNullOrWhiteSpace(s.HaveName) && !string.IsNullOrWhiteSpace(s.WantName)
			select s into r
			orderby r.Priority descending
			select r).ToList());
	}

	static BulkTraderExSettings()
	{
		CurrencyNames = new ObservableCollection<string>(CurrencyHelper.Currencies.Select((CurrencyHelper.Currency c) => c.Name));
	}
}
