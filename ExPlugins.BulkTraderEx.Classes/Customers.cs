using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.BulkTraderEx.Classes;

public class Customers : INotifyPropertyChanged
{
	private string string_0;

	private decimal decimal_0;

	private string string_1;

	private decimal decimal_1;

	private string string_2;

	private bool bool_0;

	private string string_3;

	private Dictionary<DateTime, string> dictionary_0 = new Dictionary<DateTime, string>();

	private string string_4;

	private int int_0;

	private bool bool_1;

	private DateTime dateTime_0;

	public string Name
	{
		get
		{
			return string_4;
		}
		set
		{
			string_4 = value;
			OnPropertyChanged("Name");
		}
	}

	public string CurrencyHeSellPotCode
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("CurrencyHeSellPotCode");
		}
	}

	public decimal CurrencyHeSellQuantity
	{
		get
		{
			return decimal_0;
		}
		set
		{
			decimal_0 = value;
			OnPropertyChanged("CurrencyHeSellQuantity");
		}
	}

	public string CurrencyHeWantPotCode
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			OnPropertyChanged("CurrencyHeWantPotCode");
		}
	}

	public decimal CurrencyHeWantQuantity
	{
		get
		{
			return decimal_1;
		}
		set
		{
			decimal_1 = value;
			OnPropertyChanged("CurrencyHeWantQuantity");
		}
	}

	public string League
	{
		get
		{
			return string_3;
		}
		set
		{
			string_3 = value;
			OnPropertyChanged("League");
		}
	}

	public string InitialMessage
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
			OnPropertyChanged("InitialMessage");
		}
	}

	public DateTime TimeOfInitialMessage
	{
		get
		{
			return dateTime_0;
		}
		set
		{
			dateTime_0 = value;
			OnPropertyChanged("TimeOfInitialMessage");
		}
	}

	public Dictionary<DateTime, string> MessageHistory
	{
		get
		{
			return dictionary_0;
		}
		set
		{
			if (!object.Equals(value, dictionary_0))
			{
				dictionary_0 = value;
				OnPropertyChanged("MessageHistory");
			}
		}
	}

	public bool IsToBeIgnored
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("IsToBeIgnored");
		}
	}

	public int NrOfTradeTry
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("NrOfTradeTry");
		}
	}

	public bool StartTradeNotified
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			OnPropertyChanged("StartTradeNotified");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public Customers()
	{
		IsToBeIgnored = false;
		CurrencyHeSellPotCode = "";
		CurrencyHeSellQuantity = 0m;
		CurrencyHeWantPotCode = "";
		CurrencyHeWantQuantity = 0m;
		League = "";
		InitialMessage = "";
		TimeOfInitialMessage = DateTime.MinValue;
		MessageHistory = new Dictionary<DateTime, string>();
		Name = "";
		NrOfTradeTry = 0;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
