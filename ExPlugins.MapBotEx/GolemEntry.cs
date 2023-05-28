using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.MapBotEx;

public class GolemEntry : INotifyPropertyChanged
{
	private int int_0;

	private string string_0;

	public string Golem
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("Golem");
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

	public GolemEntry()
	{
	}

	public GolemEntry(string golem, int amount)
	{
		Golem = golem;
		Amount = amount;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
