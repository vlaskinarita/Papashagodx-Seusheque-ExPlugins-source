using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.TraderPlugin.Classes;

public class ItemSearchParams
{
	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly List<string> list_0;

	[CompilerGenerated]
	private readonly List<string> list_1;

	[CompilerGenerated]
	private readonly List<string> list_2;

	[CompilerGenerated]
	private readonly List<string> list_3;

	[CompilerGenerated]
	private readonly bool bool_0;

	[CompilerGenerated]
	private readonly bool bool_1;

	[CompilerGenerated]
	private readonly bool bool_2;

	[CompilerGenerated]
	private readonly bool bool_3;

	[CompilerGenerated]
	private readonly bool bool_4;

	[CompilerGenerated]
	private readonly bool bool_5;

	[CompilerGenerated]
	private readonly Rarity rarity_0;

	[CompilerGenerated]
	private readonly int int_1;

	[CompilerGenerated]
	private decimal decimal_0;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private Vector2i vector2i_1;

	[CompilerGenerated]
	private bool bool_6;

	[CompilerGenerated]
	private string string_3;

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

	public int LocalID
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

	public Vector2i ItemPos
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
	}

	public List<string> ImplicitStrings
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
	}

	public List<string> EnchantStrings
	{
		[CompilerGenerated]
		get
		{
			return list_1;
		}
	}

	public List<string> ExplicitStrings
	{
		[CompilerGenerated]
		get
		{
			return list_2;
		}
	}

	public List<string> CraftedStrings
	{
		[CompilerGenerated]
		get
		{
			return list_3;
		}
	}

	public bool IsElderItem
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
	}

	public bool IsShaperItem
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
	}

	public bool IsHunterItem
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
	}

	public bool IsWarlordItem
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
	}

	public bool IsRedeemerItem
	{
		[CompilerGenerated]
		get
		{
			return bool_4;
		}
	}

	public bool IsCrusaderItem
	{
		[CompilerGenerated]
		get
		{
			return bool_5;
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

	public int ItemLevel
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
	}

	public decimal Price
	{
		[CompilerGenerated]
		get
		{
			return decimal_0;
		}
		[CompilerGenerated]
		set
		{
			decimal_0 = value;
		}
	}

	public string Currency
	{
		[CompilerGenerated]
		get
		{
			return string_2;
		}
		[CompilerGenerated]
		set
		{
			string_2 = value;
		}
	}

	public Vector2i CoordsToPut
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_1;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			vector2i_1 = value;
		}
	}

	public bool ForcePutToCoords
	{
		[CompilerGenerated]
		get
		{
			return bool_6;
		}
		[CompilerGenerated]
		set
		{
			bool_6 = value;
		}
	}

	public string ExactNote
	{
		[CompilerGenerated]
		get
		{
			return string_3;
		}
		[CompilerGenerated]
		set
		{
			string_3 = value;
		}
	}

	public ItemSearchParams(Item item, Vector2i itemPos)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		string_0 = item.Name;
		string_1 = item.FullName;
		LocalID = item.LocalId;
		vector2i_0 = itemPos;
		rarity_0 = item.Rarity;
		int_1 = item.ItemLevel;
	}

	public ItemSearchParams(decimal price, string currency, Vector2i itemPos, bool forcePutOnCoords = false, Vector2i coordsToPut = default(Vector2i))
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Price = price;
		Currency = currency;
		vector2i_0 = itemPos;
		ForcePutToCoords = forcePutOnCoords;
		if (forcePutOnCoords)
		{
			CoordsToPut = coordsToPut;
		}
	}

	public ItemSearchParams(string exactNote, Vector2i itemPos)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		vector2i_0 = itemPos;
		ExactNote = exactNote;
	}

	public ItemSearchParams(int localId)
	{
		LocalID = localId;
	}
}
