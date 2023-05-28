using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.EXtensions;

public class FollowerEntry : INotifyPropertyChanged
{
	private int int_0;

	private string string_0;

	private bool bool_0;

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

	public bool WaitForSameZone
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("WaitForSameZone");
		}
	}

	public int MaxDistance
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("MaxDistance");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public FollowerEntry()
	{
	}

	public FollowerEntry(string name, bool waitForSameZone, int maxDistance)
	{
		Name = name;
		WaitForSameZone = waitForSameZone;
		MaxDistance = maxDistance;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
