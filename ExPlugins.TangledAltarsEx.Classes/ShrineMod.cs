using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.TangledAltarsEx.Classes;

public class ShrineMod
{
	private string string_0;

	private int int_0;

	private string string_1;

	public string WhoGains
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			OnPropertyChanged("WhoGains");
		}
	}

	public string ModBonus
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("ModBonus");
		}
	}

	public int Weight
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("Weight");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ShrineMod(string whoGains, string modBonus, int weight = 0)
	{
		WhoGains = whoGains;
		ModBonus = modBonus;
		Weight = weight;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
