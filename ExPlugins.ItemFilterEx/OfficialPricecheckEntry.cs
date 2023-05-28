using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.ItemFilterEx;

public class OfficialPricecheckEntry : INotifyPropertyChanged
{
	private bool bool_0;

	private bool bool_1;

	private bool bool_2;

	private bool bool_3;

	private string string_0;

	private ObservableCollection<string> observableCollection_0;

	public bool Enabled
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("Enabled");
		}
	}

	public string FullName
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("FullName");
		}
	}

	public bool CheckStats
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			OnPropertyChanged("CheckStats");
		}
	}

	public ObservableCollection<string> StatsToCheck
	{
		get
		{
			return observableCollection_0;
		}
		set
		{
			observableCollection_0 = value;
			OnPropertyChanged("StatsToCheck");
		}
	}

	public bool CheckIlvl
	{
		get
		{
			return bool_2;
		}
		set
		{
			bool_2 = value;
			OnPropertyChanged("CheckIlvl");
		}
	}

	public bool CheckCorrupted
	{
		get
		{
			return bool_3;
		}
		set
		{
			bool_3 = value;
			OnPropertyChanged("CheckCorrupted");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public OfficialPricecheckEntry()
	{
	}

	public OfficialPricecheckEntry(bool enabled, string fullName, bool checkStats, ObservableCollection<string> statsToCheck, bool checkIlvl, bool checkCorrupted)
	{
		Enabled = enabled;
		FullName = fullName;
		CheckStats = checkStats;
		StatsToCheck = statsToCheck;
		CheckIlvl = checkIlvl;
		CheckCorrupted = checkCorrupted;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
