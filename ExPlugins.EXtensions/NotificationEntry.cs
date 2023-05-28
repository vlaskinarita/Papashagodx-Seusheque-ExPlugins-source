using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.EXtensions;

public class NotificationEntry : INotifyPropertyChanged
{
	private string string_0;

	private NotificationType notificationType_0;

	private bool bool_0;

	public NotificationType NotifType
	{
		get
		{
			return notificationType_0;
		}
		set
		{
			notificationType_0 = value;
			OnPropertyChanged("NotifType");
		}
	}

	public string Content
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			OnPropertyChanged("Content");
		}
	}

	public bool UseAddition
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("UseAddition");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public NotificationEntry()
	{
	}

	public NotificationEntry(NotificationType notifType, string content, bool useAddition)
	{
		NotifType = notifType;
		Content = content;
		UseAddition = useAddition;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
