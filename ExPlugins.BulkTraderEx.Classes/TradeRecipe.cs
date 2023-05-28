using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ExPlugins.BulkTraderEx.Classes;

public class TradeRecipe : INotifyPropertyChanged
{
	private int int_0;

	private string string_0;

	private int int_1 = 1;

	private bool bool_0 = true;

	private int int_2 = 5000;

	private int int_3 = 30;

	private string string_1 = "Chaos Orb";

	[CompilerGenerated]
	private List<TradeCurrency> list_0;

	public string HaveName
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("HaveName");
		}
	}

	public string WantName
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			OnPropertyChanged("WantName");
		}
	}

	public int AmountToKeep
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("AmountToKeep");
		}
	}

	public int MaxToBuy
	{
		get
		{
			return int_2;
		}
		set
		{
			int_2 = value;
			OnPropertyChanged("MaxToBuy");
		}
	}

	public int MinToBuy
	{
		get
		{
			return int_3;
		}
		set
		{
			int_3 = value;
			OnPropertyChanged("MinToBuy");
		}
	}

	public bool IsEnabled
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("IsEnabled");
		}
	}

	public int Priority
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			OnPropertyChanged("Priority");
		}
	}

	[JsonIgnore]
	public List<TradeCurrency> Prices
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
		[CompilerGenerated]
		set
		{
			list_0 = value;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
