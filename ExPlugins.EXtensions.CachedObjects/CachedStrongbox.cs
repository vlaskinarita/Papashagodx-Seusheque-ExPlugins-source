using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedStrongbox : CachedObject
{
	[CompilerGenerated]
	private readonly Rarity rarity_0;

	public Rarity Rarity
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0;
		}
	}

	public new Chest Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (Chest)(object)((@object is Chest) ? @object : null);
		}
	}

	public CachedStrongbox(int id, WalkablePosition position, Rarity rarity)
		: base(id, position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		rarity_0 = rarity;
	}
}
