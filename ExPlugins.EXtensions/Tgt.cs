using System.Collections.Generic;
using System.Text.RegularExpressions;
using DreamPoeBot.Loki.Game;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions;

public static class Tgt
{
	public static WorldPosition FindFirst(string tgtName)
	{
		Regex regex = CreateRegex(tgtName);
		TerrainDataEntry[,] tgtEntries = TerrainData.TgtEntries;
		for (int i = 0; i < tgtEntries.GetLength(0); i++)
		{
			for (int j = 0; j < tgtEntries.GetLength(1); j++)
			{
				TerrainDataEntry val = tgtEntries[i, j];
				if (val != null && regex.IsMatch(val.TgtName))
				{
					return new WorldPosition(i * 23, j * 23);
				}
			}
		}
		return null;
	}

	public static List<WorldPosition> FindAll(string tgtName)
	{
		Regex regex = CreateRegex(tgtName);
		List<WorldPosition> list = new List<WorldPosition>();
		TerrainDataEntry[,] tgtEntries = TerrainData.TgtEntries;
		for (int i = 0; i < tgtEntries.GetLength(0); i++)
		{
			for (int j = 0; j < tgtEntries.GetLength(1); j++)
			{
				TerrainDataEntry val = tgtEntries[i, j];
				if (val != null && regex.IsMatch(val.TgtName))
				{
					list.Add(new WorldPosition(i * 23, j * 23));
				}
			}
		}
		return list;
	}

	public static WorldPosition FindWaypoint()
	{
		List<WorldPosition> list = new List<WorldPosition>();
		TerrainDataEntry[,] tgtEntries = TerrainData.TgtEntries;
		for (int i = 0; i < tgtEntries.GetLength(0); i++)
		{
			for (int j = 0; j < tgtEntries.GetLength(1); j++)
			{
				TerrainDataEntry val = tgtEntries[i, j];
				if (val != null)
				{
					string tgtName = val.TgtName;
					if (tgtName.ContainsIgnorecase("waypoint") && !tgtName.Contains("waypoint_broken"))
					{
						list.Add(new WorldPosition(i * 23, j * 23));
					}
				}
			}
		}
		if (list.Count != 0)
		{
			foreach (WorldPosition item in list)
			{
				if (item.PathExists)
				{
					return item;
				}
			}
			return list[0].GetWalkable(10, 20);
		}
		GlobalLog.Error("[FindWaypoint] Fail to find any waypoint tgt.");
		return null;
	}

	private static Regex CreateRegex(string tgtName)
	{
		return new Regex(Regex.Escape(tgtName).Replace("\\?", "[0-9]"));
	}
}
