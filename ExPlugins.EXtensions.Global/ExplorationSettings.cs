using System.Runtime.CompilerServices;

namespace ExPlugins.EXtensions.Global;

public class ExplorationSettings
{
	public const int DefaultTileKnownRadius = 7;

	public const int DefaultTileSeenRadius = 4;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private int int_1;

	public bool BasicExploration
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

	public bool FastTransition
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

	public bool Backtracking
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
		[CompilerGenerated]
		set
		{
			bool_2 = value;
		}
	}

	public bool OpenPortals
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
		[CompilerGenerated]
		set
		{
			bool_3 = value;
		}
	}

	public string PriorityTransition
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

	public int TileKnownRadius
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

	public int TileSeenRadius
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

	public ExplorationSettings(bool basicExploration = true, bool fastTransition = false, bool backtracking = false, bool openPortals = true, string priorityTransition = null, int tileKnownRadius = 7, int tileSeenRadius = 4)
	{
		BasicExploration = basicExploration;
		FastTransition = fastTransition;
		Backtracking = backtracking;
		OpenPortals = openPortals;
		PriorityTransition = priorityTransition;
		TileKnownRadius = tileKnownRadius;
		TileSeenRadius = tileSeenRadius;
	}
}
