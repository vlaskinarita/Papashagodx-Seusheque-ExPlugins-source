using System;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedObject : IEquatable<CachedObject>
{
	private bool bool_0;

	[CompilerGenerated]
	private readonly int int_0;

	[CompilerGenerated]
	private WalkablePosition walkablePosition_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private int int_1;

	public int Id
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
	}

	public WalkablePosition Position
	{
		[CompilerGenerated]
		get
		{
			return walkablePosition_0;
		}
		[CompilerGenerated]
		set
		{
			walkablePosition_0 = value;
		}
	}

	public bool Unwalkable
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

	public int InteractionAttempts
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

	public NetworkObject Object => GetObject();

	public bool Ignored
	{
		get
		{
			return bool_0;
		}
		set
		{
			if (!value)
			{
				InteractionAttempts = 0;
			}
			bool_0 = value;
		}
	}

	public CachedObject(int id, WalkablePosition position)
	{
		int_0 = id;
		Position = position;
	}

	public CachedObject(NetworkObject obj)
	{
		int_0 = obj.Id;
		Position = obj.WalkablePosition();
	}

	public bool Equals(CachedObject other)
	{
		return this == other;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as CachedObject);
	}

	public static bool operator ==(CachedObject left, CachedObject right)
	{
		if ((object)left != right)
		{
			if ((object)left != null && (object)right != null)
			{
				return left.Id == right.Id;
			}
			return false;
		}
		return true;
	}

	public static bool operator !=(CachedObject left, CachedObject right)
	{
		return !(left == right);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public override string ToString()
	{
		return Position.ToString();
	}

	protected NetworkObject GetObject()
	{
		return ObjectManager.Objects.Find((NetworkObject o) => o.Id == Id);
	}
}
