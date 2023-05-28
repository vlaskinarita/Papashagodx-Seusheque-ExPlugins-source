using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.AutoLoginEx;

public class DummyEntry : INotifyPropertyChanged
{
	private int int_0;

	private int int_1;

	public int MinLevel
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			OnPropertyChanged("MinLevel");
		}
	}

	public int MaxLevel
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("MaxLevel");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public DummyEntry()
	{
		MinLevel = 3;
		MaxLevel = 10;
	}

	public DummyEntry(int minLevel, int maxLevel)
	{
		MinLevel = minLevel;
		MaxLevel = maxLevel;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
