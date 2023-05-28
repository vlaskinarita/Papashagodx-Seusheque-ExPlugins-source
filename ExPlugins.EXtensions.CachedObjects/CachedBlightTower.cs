using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedBlightTower : CachedObject
{
	[CompilerGenerated]
	private readonly int int_2;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	public int Tier
	{
		[CompilerGenerated]
		get
		{
			return int_2;
		}
	}

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string Metadata
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public new BlightDefensiveTower Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (BlightDefensiveTower)(object)((@object is BlightDefensiveTower) ? @object : null);
		}
	}

	public CachedBlightTower(int id, WalkablePosition position, int tier, string name, string metadata)
		: base(id, position)
	{
		int_2 = tier;
		string_0 = name;
		string_1 = metadata;
	}
}
