using System.ComponentModel;

namespace ExPlugins.AutoFlaskEx;

public enum TriggerType
{
	[Description("HP percent")]
	Hp,
	[Description("ES percent")]
	Es,
	[Description("Monsters nearby")]
	Mobs,
	[Description("Before attacking")]
	Attack,
	[Description("Always")]
	Always
}
