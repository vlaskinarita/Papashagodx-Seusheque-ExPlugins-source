using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.EXtensions;

public class SpecialStashingRule : INotifyPropertyChanged
{
	private string string_0;

	private string string_1;

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

	public string TabName
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			OnPropertyChanged("TabName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public SpecialStashingRule()
	{
	}

	public SpecialStashingRule(string name, string tabName)
	{
		Name = name;
		TabName = tabName;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
