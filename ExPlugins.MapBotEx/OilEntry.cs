using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.MapBotEx;

public class OilEntry : INotifyPropertyChanged
{
	private int int_0;

	private int int_1;

	private string string_0;

	private bool bool_0;

	private bool bool_1;

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

	public int AmountToUse
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("AmountToUse");
		}
	}

	public int CurrentAmount
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			OnPropertyChanged("CurrentAmount");
		}
	}

	public bool UseOnBlighted
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("UseOnBlighted");
		}
	}

	public bool UseOnRavaged
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			OnPropertyChanged("UseOnRavaged");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public OilEntry()
	{
	}

	public OilEntry(string name, int amountToUse, int currentAmount, bool blighted, bool ravaged)
	{
		Name = name;
		AmountToUse = amountToUse;
		CurrentAmount = currentAmount;
		UseOnBlighted = blighted;
		UseOnRavaged = ravaged;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
