using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedMapItem
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
	private readonly Rarity rarity_0;

	[CompilerGenerated]
	private readonly int int_0;

	[CompilerGenerated]
	private readonly bool bool_0;

	[CompilerGenerated]
	private readonly bool bool_1;

	[CompilerGenerated]
	private readonly bool bool_2;

	[CompilerGenerated]
	private readonly bool bool_3;

	[CompilerGenerated]
	private readonly int int_1;

	[CompilerGenerated]
	private readonly string string_4;

	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly List<KeyValuePair<string, string>> HomfAdZaCo;

	[CompilerGenerated]
	private readonly Dictionary<StatTypeGGG, int> dictionary_0;

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

	public bool IsIdentified
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
	}

	public bool IsCorrupted
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
	}

	public bool IsMirrored
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
	}

	public bool IsFractured
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
	}

	public int MapTier
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
	}

	public string StashTab
	{
		[CompilerGenerated]
		get
		{
			return string_4;
		}
	}

	public Vector2i LocationTopLeft
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
	}

	public List<KeyValuePair<string, string>> AffixList
	{
		[CompilerGenerated]
		get
		{
			return HomfAdZaCo;
		}
	}

	public Dictionary<StatTypeGGG, int> Stats
	{
		[CompilerGenerated]
		get
		{
			return dictionary_0;
		}
	}

	public CachedMapItem()
	{
	}

	public CachedMapItem(Item item)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		string_0 = item.Name;
		string_1 = item.FullName;
		string_2 = item.Metadata;
		string_3 = item.Class;
		rarity_0 = item.Rarity;
		int_0 = item.Quality;
		bool_0 = item.IsIdentified;
		dictionary_0 = item.Stats;
		bool_1 = item.IsCorrupted;
		int_1 = item.MapTier;
		bool_2 = item.IsMirrored;
		bool_3 = item.IsFractured;
		string_4 = StashUi.TabControl.CurrentTabName;
		vector2i_0 = item.LocationTopLeft;
		HomfAdZaCo = new List<KeyValuePair<string, string>>();
		foreach (ModAffix affix in item.Affixes)
		{
			string key = "";
			StatContainer val = Enumerable.FirstOrDefault(affix.Stats, (StatContainer a) => !string.IsNullOrWhiteSpace(a.Description) && !a.Description.ContainsIgnorecase("quantity") && !a.Description.ContainsIgnorecase("rarity") && !a.Description.ContainsIgnorecase("pack size"));
			string value = ((val != null) ? val.Description.Replace("{0:+d}", "#").Replace("{0}", "#").Replace("\\n", " ") : "");
			if (!string.IsNullOrWhiteSpace(affix.DisplayName))
			{
				key = affix.DisplayName;
			}
			AffixList.Add(new KeyValuePair<string, string>(key, value));
		}
	}
}
