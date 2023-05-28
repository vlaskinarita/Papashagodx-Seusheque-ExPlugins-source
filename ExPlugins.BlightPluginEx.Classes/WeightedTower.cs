using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.BlightPluginEx.Classes;

public class WeightedTower
{
	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private BlightDefensiveTower blightDefensiveTower_0;

	[CompilerGenerated]
	private CachedBlightTower cachedBlightTower_0;

	[CompilerGenerated]
	private WalkablePosition walkablePosition_0;

	[CompilerGenerated]
	private int int_1;

	public int Id
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

	public BlightDefensiveTower TowerObject
	{
		[CompilerGenerated]
		get
		{
			return blightDefensiveTower_0;
		}
		[CompilerGenerated]
		set
		{
			blightDefensiveTower_0 = value;
		}
	}

	public CachedBlightTower TowerCached
	{
		[CompilerGenerated]
		get
		{
			return cachedBlightTower_0;
		}
		[CompilerGenerated]
		set
		{
			cachedBlightTower_0 = value;
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

	public int TotalWeight
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

	public WeightedTower(int id, BlightDefensiveTower towerObj, CachedBlightTower towerCached, WalkablePosition position, int totalWeight)
	{
		Id = id;
		TowerObject = towerObj;
		TowerCached = towerCached;
		Position = position;
		TotalWeight = totalWeight;
	}
}
