using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.ItemFilterEx;

public class NameEntry : INotifyPropertyChanged
{
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

	public event PropertyChangedEventHandler PropertyChanged;

	public NameEntry()
	{
	}

	public NameEntry(string name)
	{
		Name = name;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
