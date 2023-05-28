using System.Runtime.CompilerServices;

namespace ExPlugins.MapBotEx.Helpers;

public class AffixData
{
	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public bool RerollMagic
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

	public bool RerollRare
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

	public AffixData(string name, string description)
	{
		string_0 = name;
		string_1 = description;
	}
}
