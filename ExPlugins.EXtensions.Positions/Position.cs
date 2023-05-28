using System;
using System.Collections.Generic;
using DreamPoeBot.Common;

namespace ExPlugins.EXtensions.Positions;

public class Position : IEquatable<Position>
{
	public class Comparer : IComparer<Vector2i>
	{
		public static readonly Comparer Instance;

		static Comparer()
		{
			Instance = new Comparer();
		}

		private Comparer()
		{
		}

		public int Compare(Vector2i first, Vector2i second)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			int num = first.X - second.X;
			if (num == 0)
			{
				return first.Y - second.Y;
			}
			return num;
		}
	}

	protected Vector2i Vector;

	public Vector2i AsVector => Vector;

	public Position(Vector2i vector)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Vector = vector;
	}

	public Position(int x, int y)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Vector = new Vector2i(x, y);
	}

	public bool Equals(Position other)
	{
		return this == other;
	}

	public int Distance(Vector2i toPoint)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ((Vector2i)(ref Vector)).Distance(toPoint);
	}

	public static implicit operator Vector2i(Position position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return position.Vector;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as Position);
	}

	public static bool operator ==(Position left, Position right)
	{
		if ((object)left == right)
		{
			return true;
		}
		if ((object)left != null && (object)right != null)
		{
			return left.Vector.X == right.Vector.X && left.Vector.Y == right.Vector.Y;
		}
		return false;
	}

	public static bool operator !=(Position left, Position right)
	{
		return !(left == right);
	}

	public override int GetHashCode()
	{
		return (Vector.X * 11) ^ Vector.Y;
	}

	public override string ToString()
	{
		return ((object)(Vector2i)(ref Vector)).ToString();
	}
}
