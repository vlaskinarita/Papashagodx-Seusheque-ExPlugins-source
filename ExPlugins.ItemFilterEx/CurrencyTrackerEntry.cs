using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.ItemFilterEx;

public class CurrencyTrackerEntry : INotifyPropertyChanged
{
	private int int_0;

	private string string_0;

	public string CurrencyName
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("CurrencyName");
		}
	}

	public int CurrencyAmount
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("CurrencyAmount");
			OnPropertyChanged("HourlyRatio");
		}
	}

	public string HourlyRatio => Math.Round((double)CurrencyAmount / (ItemFilterExSettings.Instance.Runtime.Elapsed.TotalSeconds / 60.0 / 60.0), 2).ToString(CultureInfo.InvariantCulture);

	public event PropertyChangedEventHandler PropertyChanged;

	public CurrencyTrackerEntry()
	{
	}

	public CurrencyTrackerEntry(string fullName, int stackCount)
	{
		string_0 = fullName;
		int_0 = stackCount;
	}

	public void Update()
	{
		OnPropertyChanged("HourlyRatio");
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
