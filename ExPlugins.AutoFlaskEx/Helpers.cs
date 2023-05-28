using System;
using System.Collections;
using System.Collections.Generic;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoFlaskEx;

public static class Helpers
{
	public static int MyCbloodStacks
	{
		get
		{
			Aura val = ((Actor)LokiPoe.Me).Auras.Find((Aura a) => a.InternalName.StartsWith("corrupted_blood"));
			return ((RemoteMemoryObject)(object)val != (RemoteMemoryObject)null) ? val.Charges : 0;
		}
	}

	public static bool NoMobsInRange(int range, out int closestDistance)
	{
		closestDistance = -1;
		Monster val = ((IEnumerable)ObjectManager.Objects).Closest<Monster>((Func<Monster, bool>)((Monster m) => m.IsActive));
		if (!((NetworkObject)(object)val == (NetworkObject)null))
		{
			int num = (int)((NetworkObject)val).Distance;
			if (num > range)
			{
				closestDistance = num;
				return true;
			}
			return false;
		}
		return true;
	}

	public static bool ShouldTrigger(List<FlaskTrigger> triggers, DataCache cachedData, out string reason)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		reason = string.Empty;
		foreach (FlaskTrigger trigger in triggers)
		{
			switch (trigger.Type)
			{
			case TriggerType.Hp:
			{
				int hpPercent = cachedData.HpPercent;
				if (hpPercent < trigger.MyHpPercent)
				{
					reason = $"we are at {hpPercent}% HP";
					return true;
				}
				break;
			}
			case TriggerType.Es:
			{
				int esPercent = cachedData.EsPercent;
				if (esPercent < trigger.MyEsPercent)
				{
					reason = $"we are at {esPercent}% ES";
					return true;
				}
				break;
			}
			case TriggerType.Mobs:
				if (!cachedData.HasAura("grace_period"))
				{
					int num = cachedData.MobCountWithRarity(trigger.MobRarity, trigger.MobRange);
					if (num >= trigger.MobCount)
					{
						reason = ((num == 1) ? $"there is 1 {trigger.MobRarity} monster in range of {trigger.MobRange}" : $"there are {num} {trigger.MobRarity} monsters in range of {trigger.MobRange}");
						return true;
					}
					break;
				}
				return false;
			case TriggerType.Always:
				reason = "should always be active";
				return true;
			}
		}
		return false;
	}
}
