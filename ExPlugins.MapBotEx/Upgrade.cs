using System.Runtime.CompilerServices;

namespace ExPlugins.MapBotEx;

public class Upgrade
{
	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private int int_0 = 1;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private int int_1;

	public bool TierEnabled
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

	public int Tier
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

	public bool PriorityEnabled
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

	public int Priority
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
}
