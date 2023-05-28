using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ExPlugins.EquipPluginEx.Classes;

public class ItemPrioritiesClass : INotifyPropertyChanged
{
	private float float_0;

	private float float_1;

	private float float_2;

	private float float_3;

	private float float_4;

	private float float_5;

	private float float_6;

	private float float_7;

	private float float_8;

	private float float_9;

	private float float_10;

	private float float_11;

	private float float_12;

	private float float_13;

	private float float_14;

	private float float_15;

	private float float_16;

	private float float_17;

	private float float_18;

	private float float_19;

	private float float_20;

	private float float_21;

	private float float_22;

	private float float_23;

	private float float_24;

	private float float_25;

	private float float_26;

	private float float_27;

	private float float_28;

	private float wocebiwChw;

	private float float_29;

	private float float_30;

	private float float_31;

	private float float_32;

	private float float_33;

	private float float_34;

	private float float_35;

	private float float_36;

	private float float_37;

	private float float_38;

	private float float_39;

	private float float_40;

	private float float_41;

	private float float_42;

	private float float_43;

	private float float_44;

	private float float_45;

	private float float_46;

	private float float_47;

	private float float_48;

	public float BaseArmorWeight
	{
		get
		{
			return float_1;
		}
		set
		{
			float_1 = value;
			OnPropertyChanged("BaseArmorWeight");
		}
	}

	public float BaseEvasionWeight
	{
		get
		{
			return float_5;
		}
		set
		{
			float_5 = value;
			OnPropertyChanged("BaseEvasionWeight");
		}
	}

	public float BaseEnergyShieldWeight
	{
		get
		{
			return float_4;
		}
		set
		{
			float_4 = value;
			OnPropertyChanged("BaseEnergyShieldWeight");
		}
	}

	public float MovementSpeedWeight
	{
		get
		{
			return float_42;
		}
		set
		{
			float_42 = value;
			OnPropertyChanged("MovementSpeedWeight");
		}
	}

	public float BasePhysDamageWeight
	{
		get
		{
			return float_8;
		}
		set
		{
			float_8 = value;
			OnPropertyChanged("BasePhysDamageWeight");
		}
	}

	public float BaseFireDamageWeight
	{
		get
		{
			return float_6;
		}
		set
		{
			float_6 = value;
			OnPropertyChanged("BaseFireDamageWeight");
		}
	}

	public float BaseColdDamageWeight
	{
		get
		{
			return float_3;
		}
		set
		{
			float_3 = value;
			OnPropertyChanged("BaseColdDamageWeight");
		}
	}

	public float BaseLightningDamageWeight
	{
		get
		{
			return float_7;
		}
		set
		{
			float_7 = value;
			OnPropertyChanged("BaseLightningDamageWeight");
		}
	}

	public float BaseChaosDamageWeight
	{
		get
		{
			return float_2;
		}
		set
		{
			float_2 = value;
			OnPropertyChanged("BaseChaosDamageWeight");
		}
	}

	public float LifeWeight
	{
		get
		{
			return float_35;
		}
		set
		{
			float_35 = value;
			OnPropertyChanged("LifeWeight");
		}
	}

	public float LifePercentWeight
	{
		get
		{
			return float_34;
		}
		set
		{
			float_34 = value;
			OnPropertyChanged("LifePercentWeight");
		}
	}

	public float ManaWeight
	{
		get
		{
			return float_41;
		}
		set
		{
			float_41 = value;
			OnPropertyChanged("ManaWeight");
		}
	}

	public float ManaPercentWeight
	{
		get
		{
			return float_40;
		}
		set
		{
			float_40 = value;
			OnPropertyChanged("ManaPercentWeight");
		}
	}

	public float EnergyShieldWeight
	{
		get
		{
			return float_28;
		}
		set
		{
			float_28 = value;
			OnPropertyChanged("EnergyShieldWeight");
		}
	}

	public float EnergyShieldPercentWeight
	{
		get
		{
			return float_27;
		}
		set
		{
			float_27 = value;
			OnPropertyChanged("EnergyShieldPercentWeight");
		}
	}

	public float FireResWeight
	{
		get
		{
			return float_32;
		}
		set
		{
			float_32 = value;
			OnPropertyChanged("FireResWeight");
		}
	}

	public float ColdResWeight
	{
		get
		{
			return float_19;
		}
		set
		{
			float_19 = value;
			OnPropertyChanged("ColdResWeight");
		}
	}

	public float LightningResWeight
	{
		get
		{
			return float_39;
		}
		set
		{
			float_39 = value;
			OnPropertyChanged("LightningResWeight");
		}
	}

	public float ChaosResWeight
	{
		get
		{
			return float_14;
		}
		set
		{
			float_14 = value;
			OnPropertyChanged("ChaosResWeight");
		}
	}

	public float IntWeight
	{
		get
		{
			return float_33;
		}
		set
		{
			float_33 = value;
			OnPropertyChanged("IntWeight");
		}
	}

	public float StrWeight
	{
		get
		{
			return float_48;
		}
		set
		{
			float_48 = value;
			OnPropertyChanged("StrWeight");
		}
	}

	public float DexWeight
	{
		get
		{
			return float_25;
		}
		set
		{
			float_25 = value;
			OnPropertyChanged("DexWeight");
		}
	}

	public float PhysDamageFlatToAttacksWeight
	{
		get
		{
			return float_43;
		}
		set
		{
			float_43 = value;
			OnPropertyChanged("PhysDamageFlatToAttacksWeight");
		}
	}

	public float FireDamageFlatToAttacksWeight
	{
		get
		{
			return wocebiwChw;
		}
		set
		{
			wocebiwChw = value;
			OnPropertyChanged("FireDamageFlatToAttacksWeight");
		}
	}

	public float ColdDamageFlaToAttackstWeight
	{
		get
		{
			return float_15;
		}
		set
		{
			float_15 = value;
			OnPropertyChanged("ColdDamageFlaToAttackstWeight");
		}
	}

	public float LightningDamageFlatToAttacksWeight
	{
		get
		{
			return float_36;
		}
		set
		{
			float_36 = value;
			OnPropertyChanged("LightningDamageFlatToAttacksWeight");
		}
	}

	public float ChaosDamageFlatToAttacksWeight
	{
		get
		{
			return float_10;
		}
		set
		{
			float_10 = value;
			OnPropertyChanged("ChaosDamageFlatToAttacksWeight");
		}
	}

	public float PhysDamageFlatToSpellsWeight
	{
		get
		{
			return float_44;
		}
		set
		{
			float_44 = value;
			OnPropertyChanged("PhysDamageFlatToSpellsWeight");
		}
	}

	public float FireDamageFlatToSpellsWeight
	{
		get
		{
			return float_29;
		}
		set
		{
			float_29 = value;
			OnPropertyChanged("FireDamageFlatToSpellsWeight");
		}
	}

	public float ColdDamageFlaToSpellstWeight
	{
		get
		{
			return float_16;
		}
		set
		{
			float_16 = value;
			OnPropertyChanged("ColdDamageFlaToSpellstWeight");
		}
	}

	public float LightningDamageFlatToSpellsWeight
	{
		get
		{
			return float_37;
		}
		set
		{
			float_37 = value;
			OnPropertyChanged("LightningDamageFlatToSpellsWeight");
		}
	}

	public float ChaosDamageFlatToSpellsWeight
	{
		get
		{
			return float_11;
		}
		set
		{
			float_11 = value;
			OnPropertyChanged("ChaosDamageFlatToSpellsWeight");
		}
	}

	public float PhysDamagePercentWeight
	{
		get
		{
			return float_45;
		}
		set
		{
			float_45 = value;
			OnPropertyChanged("PhysDamagePercentWeight");
		}
	}

	public float ColdDamagePercentWeight
	{
		get
		{
			return float_17;
		}
		set
		{
			float_17 = value;
			OnPropertyChanged("ColdDamagePercentWeight");
		}
	}

	public float FireDamagePercentWeight
	{
		get
		{
			return float_30;
		}
		set
		{
			float_30 = value;
			OnPropertyChanged("FireDamagePercentWeight");
		}
	}

	public float LightningDamagePercentWeight
	{
		get
		{
			return float_38;
		}
		set
		{
			float_38 = value;
			OnPropertyChanged("LightningDamagePercentWeight");
		}
	}

	public float ChaosDamagePercentWeight
	{
		get
		{
			return float_12;
		}
		set
		{
			float_12 = value;
			OnPropertyChanged("ChaosDamagePercentWeight");
		}
	}

	public float ElementalDamagePercentToAttacksWeight
	{
		get
		{
			return float_26;
		}
		set
		{
			float_26 = value;
			OnPropertyChanged("ElementalDamagePercentToAttacksWeight");
		}
	}

	public float CritMultiWeight
	{
		get
		{
			return float_20;
		}
		set
		{
			float_20 = value;
			OnPropertyChanged("CritMultiWeight");
		}
	}

	public float AttackSpeedWeight
	{
		get
		{
			return float_0;
		}
		set
		{
			float_0 = value;
			OnPropertyChanged("AttackSpeedWeight");
		}
	}

	public float CastSpeedWeight
	{
		get
		{
			return float_9;
		}
		set
		{
			float_9 = value;
			OnPropertyChanged("CastSpeedWeight");
		}
	}

	public float DamagePercentWeight
	{
		get
		{
			return float_22;
		}
		set
		{
			float_22 = value;
			OnPropertyChanged("DamagePercentWeight");
		}
	}

	public float DamageOverTimeWeight
	{
		get
		{
			return float_21;
		}
		set
		{
			float_21 = value;
			OnPropertyChanged("DamageOverTimeWeight");
		}
	}

	public float DamageWithBleedingWeight
	{
		get
		{
			return float_23;
		}
		set
		{
			float_23 = value;
			OnPropertyChanged("DamageWithBleedingWeight");
		}
	}

	public float DamageWithPoisonWeight
	{
		get
		{
			return float_24;
		}
		set
		{
			float_24 = value;
			OnPropertyChanged("DamageWithPoisonWeight");
		}
	}

	public float SpellDamage
	{
		get
		{
			return float_47;
		}
		set
		{
			float_47 = value;
			OnPropertyChanged("SpellDamage");
		}
	}

	public float ChaosDoTMultiWeight
	{
		get
		{
			return float_13;
		}
		set
		{
			float_13 = value;
			OnPropertyChanged("ChaosDoTMultiWeight");
		}
	}

	public float FireDoTMultiWeight
	{
		get
		{
			return float_31;
		}
		set
		{
			float_31 = value;
			OnPropertyChanged("FireDoTMultiWeight");
		}
	}

	public float ColdDoTMultiWeight
	{
		get
		{
			return float_18;
		}
		set
		{
			float_18 = value;
			OnPropertyChanged("ColdDoTMultiWeight");
		}
	}

	public float PhysDoTMultiWeight
	{
		get
		{
			return float_46;
		}
		set
		{
			float_46 = value;
			OnPropertyChanged("PhysDoTMultiWeight");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
