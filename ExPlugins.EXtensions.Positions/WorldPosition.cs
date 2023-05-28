using System;
using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.Positions;

public class WorldPosition : Position
{
	public class ComparerByDistanceSqr : IComparer<WorldPosition>
	{
		public static readonly ComparerByDistanceSqr Instance;

		private ComparerByDistanceSqr()
		{
		}

		public int Compare(WorldPosition first, WorldPosition second)
		{
			return first.DistanceSqr - second.DistanceSqr;
		}

		static ComparerByDistanceSqr()
		{
			Instance = new ComparerByDistanceSqr();
		}
	}

	public class ComparerByPathDistance : IComparer<WorldPosition>
	{
		public static readonly ComparerByPathDistance Instance;

		private ComparerByPathDistance()
		{
		}

		public int Compare(WorldPosition first, WorldPosition second)
		{
			return first.PathDistance.CompareTo(second.PathDistance);
		}

		static ComparerByPathDistance()
		{
			Instance = new ComparerByPathDistance();
		}
	}

	public new int Distance
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Vector2i myPosition = LokiPoe.MyPosition;
			return ((Vector2i)(ref myPosition)).Distance(Vector);
		}
	}

	public int DistanceSqr
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Vector2i myPosition = LokiPoe.MyPosition;
			return ((Vector2i)(ref myPosition)).DistanceSqr(Vector);
		}
	}

	public float PathDistance => ExilePather.PathDistance(LokiPoe.MyPosition, Vector, false, false);

	public bool IsNear => Distance <= 20;

	public bool IsFar => Distance > 20;

	public bool IsNearByPath => PathDistance <= 23f;

	public bool IsFarByPath => PathDistance > 23f;

	public bool PathExists => ExilePather.PathExistsBetween(LokiPoe.MyPosition, Vector, false);

	public WorldPosition(Vector2i vector)
		: base(vector)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public WorldPosition(int x, int y)
		: base(x, y)
	{
	}

	public WorldPosition GetWalkable(int step = 5, int radius = 30)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return (!PathExists) ? FindPositionForMove(this, step, radius) : this;
	}

	public override string ToString()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return $"{Vector} (distance: {Distance})";
	}

	public static WorldPosition FindPositionForMove(Vector2i pos, int step = 5, int radius = 30)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		WorldPosition worldPosition = FindWalkablePosition(pos, radius);
		if (!(worldPosition != null))
		{
			return FindPathablePosition(pos, step, radius);
		}
		return worldPosition;
	}

	public static WorldPosition FindWalkablePosition(Vector2i pos, int radius = 20)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Vector2i val = ExilePather.FastWalkablePositionFor(pos, radius, true);
		return ExilePather.PathExistsBetween(LokiPoe.MyPosition, val, false) ? new WorldPosition(val) : null;
	}

	public static WorldPosition FindPathablePosition(Vector2i pos, int step = 5, int radius = 30)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		Vector2i myPosition = LokiPoe.MyPosition;
		int x = pos.X;
		int y = pos.Y;
		Vector2i val = default(Vector2i);
		for (int i = step; i <= radius; i += step)
		{
			int num = x - i;
			int num2 = y - i;
			int num3 = x + i;
			int num4 = y + i;
			for (int j = num; j <= num3; j += step)
			{
				for (int k = num2; k <= num4; k += step)
				{
					if (j == num || j == num3 || k == num2 || k == num4)
					{
						((Vector2i)(ref val))._002Ector(j, k);
						if (ExilePather.PathExistsBetween(myPosition, val, false))
						{
							return new WorldPosition(val);
						}
					}
				}
			}
		}
		return null;
	}

	public static WorldPosition FindPathablePositionAtDistance(int min, int max, int step)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		int x = LokiPoe.MyPosition.X;
		int y = LokiPoe.MyPosition.Y;
		int num = max + 5;
		int num2 = min;
		WorldPosition worldPosition;
		while (true)
		{
			if (num2 <= max)
			{
				worldPosition = new WorldPosition(x, y + num2);
				if (worldPosition.PathDistance <= (float)num)
				{
					break;
				}
				worldPosition = new WorldPosition(x + num2, y + num2);
				if (!(worldPosition.PathDistance <= (float)num))
				{
					worldPosition = new WorldPosition(x - num2, y + num2);
					if (!(worldPosition.PathDistance <= (float)num))
					{
						worldPosition = new WorldPosition(x + num2, y);
						if (!(worldPosition.PathDistance <= (float)num))
						{
							worldPosition = new WorldPosition(x - num2, y);
							if (!(worldPosition.PathDistance <= (float)num))
							{
								worldPosition = new WorldPosition(x + num2, y - num2);
								if (!(worldPosition.PathDistance <= (float)num))
								{
									worldPosition = new WorldPosition(x - num2, y - num2);
									if (!(worldPosition.PathDistance <= (float)num))
									{
										worldPosition = new WorldPosition(x, y - num2);
										if (!(worldPosition.PathDistance <= (float)num))
										{
											num2 += step;
											continue;
										}
										return worldPosition;
									}
									return worldPosition;
								}
								return worldPosition;
							}
							return worldPosition;
						}
						return worldPosition;
					}
					return worldPosition;
				}
				return worldPosition;
			}
			return null;
		}
		return worldPosition;
	}

	public static WorldPosition FindRandomPositionForMove(Vector2i pos, int step = 10, int radius = 60, int angle = 359)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
		int num = LokiPoe.Random.Next(0, angle);
		radius = LokiPoe.Random.Next(radius / 4, radius);
		for (int num2 = radius; num2 > 0; num2 -= step)
		{
			for (int i = num; i < num + angle; i += step)
			{
				int num3 = ((i > angle) ? (i - angle) : i);
				Vector2i pointOnCircle = GetPointOnCircle(pos, ConvertToRadians(num3), num2);
				if (ExilePather.IsWalkable(pointOnCircle) && ExilePather.PathExistsBetween(position, pointOnCircle, false))
				{
					return new WalkablePosition("Random Walkable Position", pointOnCircle);
				}
			}
		}
		return null;
	}

	protected static Vector2 GetAveragePoint(List<Vector2> pts)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = default(Vector2);
		result.X = pts.Average((Vector2 p) => p.X);
		result.Y = pts.Average((Vector2 p) => p.Y);
		return result;
	}

	protected static double GetAngleBetweenPoints(Vector2 pt1, Vector2 pt2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X);
	}

	protected static Vector2i GetPointOnCircle(Vector2i center, double radian, double radius)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Vector2i result = default(Vector2i);
		result.X = center.X + (int)(radius * Math.Cos(radian));
		result.Y = center.Y + (int)(radius * Math.Sin(radian));
		return result;
	}

	public static double ConvertToRadians(double angle)
	{
		return 0.174532922 * angle;
	}
}
