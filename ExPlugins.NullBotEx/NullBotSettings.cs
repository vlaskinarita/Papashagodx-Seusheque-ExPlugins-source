using System;
using System.Linq.Expressions;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;

namespace ExPlugins.NullBotEx;

public class NullBotSettings : JsonSettings
{
	private static NullBotSettings nullBotSettings_0;

	private bool bool_0;

	private bool bool_1;

	public static NullBotSettings Instance => nullBotSettings_0 ?? (nullBotSettings_0 = new NullBotSettings());

	public bool ShouldEnableHooks
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => ShouldEnableHooks));
		}
	}

	public bool ShouldEnableCombat
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
			((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => ShouldEnableCombat));
		}
	}

	private NullBotSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"NullBot.json"
		}))
	{
	}
}
