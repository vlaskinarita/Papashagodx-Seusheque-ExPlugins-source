using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.ItemFilterEx;

public class EnchantEntry : INotifyPropertyChanged
{
	private string string_0;

	private int int_0;

	public string InternalName
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("InternalName");
		}
	}

	public int Value
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("Value");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public EnchantEntry()
	{
	}

	public EnchantEntry(string name, int value)
	{
		InternalName = name;
		Value = value;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
