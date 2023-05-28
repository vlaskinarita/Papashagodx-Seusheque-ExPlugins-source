using System;
using System.Runtime.CompilerServices;

namespace ExPlugins.IncursionEx;

public class RoomEntry
{
	private readonly int[] int_0 = new int[3];

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private PriorityAction priorityAction_0;

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
		[CompilerGenerated]
		set
		{
			string_0 = value;
		}
	}

	public PriorityAction PriorityAction
	{
		[CompilerGenerated]
		get
		{
			return priorityAction_0;
		}
		[CompilerGenerated]
		set
		{
			priorityAction_0 = value;
		}
	}

	public bool NoChange
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

	public bool NoUpgrade
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

	public RoomEntry(string name, int id1, int id2, int id3)
	{
		Name = name;
		int_0[0] = id1;
		int_0[1] = id2;
		int_0[2] = id3;
	}

	public bool IdEquals(int id)
	{
		return Array.IndexOf(int_0, id) >= 0;
	}
}
