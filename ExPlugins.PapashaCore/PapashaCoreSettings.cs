using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using Newtonsoft.Json;

namespace ExPlugins.PapashaCore;

public class PapashaCoreSettings : JsonSettings
{
	private static PapashaCoreSettings papashaCoreSettings_0;

	private string string_0;

	private string string_1;

	private string string_2;

	private string string_3;

	private string string_4;

	private string string_5;

	private string string_6;

	private string string_7;

	private string string_8;

	private string string_9;

	private string string_10;

	public static PapashaCoreSettings Instance => papashaCoreSettings_0 ?? (papashaCoreSettings_0 = new PapashaCoreSettings());

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin1Time
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin1Time));
		}
	}

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin2Time
	{
		get
		{
			return string_3;
		}
		set
		{
			string_3 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin2Time));
		}
	}

	[DefaultValue("Not fetched yet!")]
	[JsonIgnore]
	public string Plugin3Time
	{
		get
		{
			return string_4;
		}
		set
		{
			string_4 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin3Time));
		}
	}

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin4Time
	{
		get
		{
			return string_5;
		}
		set
		{
			string_5 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin4Time));
		}
	}

	[DefaultValue("Not fetched yet!")]
	[JsonIgnore]
	public string Plugin5Time
	{
		get
		{
			return string_6;
		}
		set
		{
			string_6 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin5Time));
		}
	}

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin6Time
	{
		get
		{
			return string_7;
		}
		set
		{
			string_7 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin6Time));
		}
	}

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin7Time
	{
		get
		{
			return string_8;
		}
		set
		{
			string_8 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin7Time));
		}
	}

	[DefaultValue("Not fetched yet!")]
	[JsonIgnore]
	public string Plugin8Time
	{
		get
		{
			return string_9;
		}
		set
		{
			string_9 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin8Time));
		}
	}

	[JsonIgnore]
	[DefaultValue("Not fetched yet!")]
	public string Plugin9Time
	{
		get
		{
			return string_10;
		}
		set
		{
			string_10 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin9Time));
		}
	}

	[DefaultValue("Not fetched yet!")]
	[JsonIgnore]
	public string Plugin10Time
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Plugin10Time));
		}
	}

	public string Key
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<string>((Expression<Func<string>>)(() => Key));
		}
	}

	private PapashaCoreSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"PapashaCore.json"
		}))
	{
	}
}
