using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.StashManager;

public class MapEntry : INotifyPropertyChanged
{
	private int int_0;

	private int int_1;

	public int MapTier
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			OnPropertyChanged("MapTier");
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

	public MapEntry(int tier, int amount)
	{
		MapTier = tier;
		Amount = amount;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
