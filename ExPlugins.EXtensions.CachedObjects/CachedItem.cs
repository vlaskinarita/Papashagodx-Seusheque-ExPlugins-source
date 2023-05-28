using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedItem
{
	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	[CompilerGenerated]
	private readonly string string_2;

	[CompilerGenerated]
	private readonly string string_3;

	[CompilerGenerated]
	private readonly CompositeItemType compositeItemType_0;

	[CompilerGenerated]
	private readonly Rarity rarity_0;

	[CompilerGenerated]
	private readonly int int_0;

	[CompilerGenerated]
	private readonly int int_1;

	[CompilerGenerated]
	private readonly int int_2;

	[CompilerGenerated]
	private readonly bool bool_0;

	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly int int_3;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string FullName
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public string Metadata
	{
		[CompilerGenerated]
		get
		{
			return string_2;
		}
	}

	public string Class
	{
		[CompilerGenerated]
		get
		{
			return string_3;
		}
	}

	public CompositeItemType Type
	{
		[CompilerGenerated]
		get
		{
			return compositeItemType_0;
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

	public int Quality
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
	}

	public int ItemLevel
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
	}

	public int StackCount
	{
		[CompilerGenerated]
		get
		{
			return int_2;
		}
	}

	public bool IsIdentified
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
	}

	public Vector2i Size
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
	}

	public int SkillGemLevel
	{
		[CompilerGenerated]
		get
		{
			return int_3;
		}
	}

	public CachedItem(Item item)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		string_0 = item.Name;
		string_1 = item.FullName;
		string_2 = item.Metadata;
		string_3 = item.Class;
		compositeItemType_0 = item.CompositeType;
		rarity_0 = item.Rarity;
		int_0 = item.Quality;
		int_1 = item.ItemLevel;
		int_2 = item.StackCount;
		bool_0 = item.IsIdentified;
		vector2i_0 = item.Size;
		int_3 = item.SkillGemLevel;
	}
}
