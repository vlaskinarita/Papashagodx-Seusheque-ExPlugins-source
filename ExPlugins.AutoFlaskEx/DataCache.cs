using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoFlaskEx;

public class DataCache
{
	private List<Aura> list_0;

	private int int_0;

	private int int_1;

	private int int_2;

	private List<Aura> Auras => list_0 ?? (list_0 = ((Actor)LokiPoe.Me).Auras.ToList());

	public int PoisonStacks => Auras.Count((Aura a) => a.InternalName == "poison");

	public int HpPercent
	{
		get
		{
			if (int_1 == -1)
			{
				int_1 = (int)((Actor)LokiPoe.Me).HealthPercent;
			}
			return int_1;
		}
	}

	public int EsPercent
	{
		get
		{
			if (int_0 == -1)
			{
				int_0 = (int)((Actor)LokiPoe.Me).EnergyShieldPercent;
			}
			return int_0;
		}
	}

	public int ManaPercent
	{
		get
		{
			if (int_2 == -1)
			{
				int_2 = (int)((Actor)LokiPoe.Me).ManaPercent;
			}
			return int_2;
		}
	}

	public bool HasAura(int slot)
	{
		Aura val = Auras.FirstOrDefault((Aura a) => a.FlaskSlot == slot);
		return (RemoteMemoryObject)(object)val != (RemoteMemoryObject)null && val.TimeLeft.TotalMilliseconds > 100.0;
	}

	public bool HasAura(string name)
	{
		Aura val = Auras.FirstOrDefault((Aura a) => a.InternalName.ContainsIgnorecase(name));
		return (RemoteMemoryObject)(object)val != (RemoteMemoryObject)null && val.TimeLeft.TotalMilliseconds > 100.0;
	}

	public int MobCountWithRarity(Rarity rarity, int range)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Vector2i vector2i_0 = LokiPoe.MyPosition;
		return (from mob in ObjectManager.Objects.OfType<Monster>()
			where mob.Rarity == rarity && mob.IsAliveHostile
			select mob).Count(delegate(Monster mob)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)mob).Position;
			return ((Vector2i)(ref position)).Distance(vector2i_0) < range;
		});
	}

	public void Clear()
	{
		int_1 = -1;
		int_0 = -1;
		list_0 = null;
	}
}
