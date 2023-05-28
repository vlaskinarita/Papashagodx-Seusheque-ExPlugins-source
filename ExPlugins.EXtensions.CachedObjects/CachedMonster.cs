using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedMonster : CachedObject
{
	[CompilerGenerated]
	private readonly Rarity rarity_0;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	[CompilerGenerated]
	private readonly List<Aura> list_0;

	public Rarity Rarity
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0;
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

	public List<Aura> Auras
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
	}

	public new Monster Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (Monster)(object)((@object is Monster) ? @object : null);
		}
	}

	public CachedMonster(int id, WalkablePosition position, List<Aura> auras, Rarity rarity, string name, string metadata)
		: base(id, position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		rarity_0 = rarity;
		string_0 = name;
		list_0 = auras;
		string_1 = metadata;
	}
}
