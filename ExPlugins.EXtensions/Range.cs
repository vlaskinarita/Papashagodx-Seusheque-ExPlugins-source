using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExPlugins.EXtensions;

public class Range : INotifyPropertyChanged
{
	private int int_0;

	private int int_1;

	public int Min
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			NotifyPropertyChanged("Min");
		}
	}

	public int Max
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			NotifyPropertyChanged("Max");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public Range(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public override string ToString()
	{
		return $"{Min}-{Max}";
	}

	public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
