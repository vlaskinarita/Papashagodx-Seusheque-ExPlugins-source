using System.ComponentModel;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;

namespace ExPlugins.TabSetuper;

public class TabSetuperSettings : JsonSettings
{
	private static TabSetuperSettings tabSetuperSettings_0;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private string string_3;

	public static TabSetuperSettings Instance => tabSetuperSettings_0 ?? (tabSetuperSettings_0 = new TabSetuperSettings());

	[DefaultValue(true)]
	public bool ShouldSetupCurrencyTab
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		set
		{
			bool_0 = value;
		}
	}

	[DefaultValue(true)]
	public bool ShouldSetupTradeTab
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
		[CompilerGenerated]
		set
		{
			bool_1 = value;
		}
	}

	[DefaultValue(null)]
	public string ReplaceTab1With
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
		[CompilerGenerated]
		set
		{
			string_0 = value;
		}
	}

	[DefaultValue(null)]
	public string ReplaceTab2With
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
		[CompilerGenerated]
		set
		{
			string_1 = value;
		}
	}

	[DefaultValue(null)]
	public string ReplaceTab3With
	{
		[CompilerGenerated]
		get
		{
			return string_2;
		}
		[CompilerGenerated]
		set
		{
			string_2 = value;
		}
	}

	[DefaultValue(null)]
	public string ReplaceTab4With
	{
		[CompilerGenerated]
		get
		{
			return string_3;
		}
		[CompilerGenerated]
		set
		{
			string_3 = value;
		}
	}

	private TabSetuperSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"TabSetuper.json"
		}))
	{
	}
}
