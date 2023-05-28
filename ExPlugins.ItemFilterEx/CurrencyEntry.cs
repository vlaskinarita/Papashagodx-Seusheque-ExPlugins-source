using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.ItemFilterEx;

public class CurrencyEntry : INotifyPropertyChanged
{
	private int int_0;

	private string string_0;

	public string Name
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("Name");
		}
	}

	public int Amount
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("Amount");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public CurrencyEntry()
	{
	}

	public CurrencyEntry(string name, int amount)
	{
		Name = name;
		Amount = amount;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
