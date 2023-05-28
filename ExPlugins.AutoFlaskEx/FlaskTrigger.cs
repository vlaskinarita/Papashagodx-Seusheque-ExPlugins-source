using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.GameData;

namespace ExPlugins.AutoFlaskEx;

public class FlaskTrigger
{
	[CompilerGenerated]
	private TriggerType triggerType_0 = TriggerType.Hp;

	[CompilerGenerated]
	private int int_0 = 75;

	[CompilerGenerated]
	private int int_1 = 75;

	[CompilerGenerated]
	private Rarity rarity_0 = (Rarity)0;

	[CompilerGenerated]
	private int int_2 = 1;

	[CompilerGenerated]
	private int int_3 = 40;

	[CompilerGenerated]
	private int int_4 = 0;

	public TriggerType Type
	{
		[CompilerGenerated]
		get
		{
			return triggerType_0;
		}
		[CompilerGenerated]
		set
		{
			triggerType_0 = value;
		}
	}

	public int MyHpPercent
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

	public int MyEsPercent
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

	public Rarity MobRarity
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			rarity_0 = value;
		}
	}

	public int MobCount
	{
		[CompilerGenerated]
		get
		{
			return int_2;
		}
		[CompilerGenerated]
		set
		{
			int_2 = value;
		}
	}

	public int MobRange
	{
		[CompilerGenerated]
		get
		{
			return int_3;
		}
		[CompilerGenerated]
		set
		{
			int_3 = value;
		}
	}

	public int MobHpPercent
	{
		[CompilerGenerated]
		get
		{
			return int_4;
		}
		[CompilerGenerated]
		set
		{
			int_4 = value;
		}
	}

	public override string ToString()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		if (Type != 0)
		{
			if (Type == TriggerType.Always)
			{
				return "Always";
			}
			if (Type != TriggerType.Es)
			{
				if (Type == TriggerType.Mobs)
				{
					return string.Format("{0} {1} mob{2} in {3} range", MobCount, MobRarity, (MobCount == 1) ? "" : "s", MobRange);
				}
				if (Type == TriggerType.Attack)
				{
					return $"{MobRarity} mob {MobHpPercent}% HP";
				}
				return $"Incorrect type: {Type}";
			}
			return $"{MyEsPercent}% ES";
		}
		return $"{MyHpPercent}% HP";
	}
}
