using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;

namespace ExPlugins.ChaosRecipeEx;

public class Settings : JsonSettings
{
	private static Settings settings_0;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private int int_0 = 60;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private int[] int_1 = new int[8] { 4, 2, 2, 2, 2, 8, 20, 40 };

	public static Settings Instance => settings_0 ?? (settings_0 = new Settings());

	public string StashTab
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

	public int MinILvl
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
		[CompilerGenerated]
		set
		{
			int_0 = value;
		}
	}

	public bool AlwaysUpdateStashData
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

	public int[] MaxItemCounts
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
		[CompilerGenerated]
		set
		{
			int_1 = value;
		}
	}

	private Settings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"ChaosRecipeEx.json"
		}))
	{
	}

	public int GetMaxItemCount(int itemType)
	{
		return MaxItemCounts[itemType];
	}
}
