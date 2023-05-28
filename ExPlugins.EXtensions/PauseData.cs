using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExPlugins.EXtensions;

public class PauseData : INotifyPropertyChanged
{
	private int int_0;

	private int int_1;

	private Range range_0;

	private string qHayMjfLue;

	public string Type
	{
		get
		{
			return qHayMjfLue;
		}
		set
		{
			qHayMjfLue = value;
			NotifyPropertyChanged("Type");
		}
	}

	public int RandomRolled
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
			NotifyPropertyChanged("RandomRolled");
		}
	}

	public int Pause
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			NotifyPropertyChanged("Pause");
		}
	}

	public Range Range
	{
		get
		{
			return range_0;
		}
		set
		{
			range_0 = value;
			NotifyPropertyChanged("Range");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public PauseData(string type, int rolled, int pause, Range range)
	{
		Type = type;
		RandomRolled = rolled;
		Pause = pause;
		Range = range;
	}

	public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
