using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedWorldItem : CachedObject
{
	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly Rarity rarity_0;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	public Vector2i Size
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
	}

	public Rarity Rarity
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0;
		}
	}

	public string Class
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public Vector2 LabelPos => ((NetworkObject)(object)Object == (NetworkObject)null) ? Vector2.Zero : Object.WorldItemLabel.Coordinate;

	public new WorldItem Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (WorldItem)(object)((@object is WorldItem) ? @object : null);
		}
	}

	public CachedWorldItem(int id, WalkablePosition position, Vector2i size, Rarity rarity, string itemclass, string name)
		: base(id, position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		vector2i_0 = size;
		rarity_0 = rarity;
		string_0 = itemclass;
		string_1 = name;
	}
}
