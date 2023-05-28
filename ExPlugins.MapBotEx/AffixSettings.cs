using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using Newtonsoft.Json;

namespace ExPlugins.MapBotEx;

public class AffixSettings
{
	private class Class31
	{
		public bool RerollMagic;

		public bool RerollRare;
	}

	private static readonly string string_0;

	private static AffixSettings affixSettings_0;

	[CompilerGenerated]
	private readonly List<AffixData> nsfjYapKp1 = new List<AffixData>();

	[CompilerGenerated]
	private readonly Dictionary<string, AffixData> dictionary_0 = new Dictionary<string, AffixData>();

	public static AffixSettings Instance => affixSettings_0 ?? (affixSettings_0 = new AffixSettings());

	public List<AffixData> AffixList
	{
		[CompilerGenerated]
		get
		{
			return nsfjYapKp1;
		}
	}

	public Dictionary<string, AffixData> AffixDict
	{
		[CompilerGenerated]
		get
		{
			return dictionary_0;
		}
	}

	private AffixSettings()
	{
		InitList();
		Load();
		InitDict();
		Configuration.OnSaveAll += delegate
		{
			Save();
		};
		nsfjYapKp1 = (from a in AffixList
			orderby a.RerollMagic descending, a.RerollRare descending, a.Name
			select a).ToList();
	}

	private void InitList()
	{
		string newLine = Environment.NewLine;
		AffixList.Add(new AffixData("Abhorrent", "Area is inhabited by Abominations"));
		AffixList.Add(new AffixData("Anarchic", "Area is inhabited by 2 additional Rogue Exiles"));
		AffixList.Add(new AffixData("Bipedal", "Area is inhabited by Humanoids"));
		AffixList.Add(new AffixData("Capricious", "Area is inhabited by Goatmen"));
		AffixList.Add(new AffixData("Undead", "Area is inhabited by Undead"));
		AffixList.Add(new AffixData("Demonic", "Area is inhabited by Demons"));
		AffixList.Add(new AffixData("Emanant", "Area is inhabited by ranged monsters"));
		AffixList.Add(new AffixData("Feasting", "Area is inhabited by Cultists of Kitava"));
		AffixList.Add(new AffixData("Feral", "Area is inhabited by Animals"));
		AffixList.Add(new AffixData("Haunting", "Area is inhabited by Ghosts"));
		AffixList.Add(new AffixData("Lunar", "Area is inhabited by Lunaris fanatics"));
		AffixList.Add(new AffixData("Skeletal", "Area is inhabited by Skeletons"));
		AffixList.Add(new AffixData("Slithering", "Area is inhabited by Sea Witches and their Spawn"));
		AffixList.Add(new AffixData("Solar", "Area is inhabited by Solaris fanatics"));
		AffixList.Add(new AffixData("Ceremonial", "Area contains many Totems"));
		AffixList.Add(new AffixData("Chaining", "Monsters' skills Chain 2 additional times"));
		AffixList.Add(new AffixData("Conflagrating", "All Monster Damage from Hits always Ignites"));
		AffixList.Add(new AffixData("Multifarious", "Area has increased monster variety"));
		AffixList.Add(new AffixData("Otherworldly", "Slaying Enemies close together can attract monsters from Beyond"));
		AffixList.Add(new AffixData("Twinned", "Area contains two Unique Bosses"));
		AffixList.Add(new AffixData("Antagonist's", "Rare Monsters each have a Nemesis Mod" + newLine + "X% more Rare Monsters"));
		AffixList.Add(new AffixData("Unstoppable", "Monsters cannot be slowed below base speed" + newLine + "Monsters cannot be Taunted"));
		AffixList.Add(new AffixData("Armoured", "+X% Monster Physical Damage Reduction"));
		AffixList.Add(new AffixData("Burning", "Monsters deal X% extra Damage as Fire"));
		AffixList.Add(new AffixData("Fecund", "X% more Monster Life"));
		AffixList.Add(new AffixData("Fleet", "X% increased Monster Movement Speed" + newLine + "X% increased Monster Attack Speed" + newLine + "X% increased Monster Cast Speed"));
		AffixList.Add(new AffixData("Freezing", "Monsters deal X% extra Damage as Cold"));
		AffixList.Add(new AffixData("Hexwarded", "X% less effect of Curses on Monsters"));
		AffixList.Add(new AffixData("Hexproof", "Monsters are Hexproof"));
		AffixList.Add(new AffixData("Impervious", "Monsters have a X% chance to avoid Poison, Blind, and Bleed"));
		AffixList.Add(new AffixData("Mirrored", "Monsters reflect X% of Elemental Damage"));
		AffixList.Add(new AffixData("Overlord's", "Unique Boss deals X% increased Damage" + newLine + "Unique Boss has X% increased Attack and Cast Speed"));
		AffixList.Add(new AffixData("Punishing", "Monsters reflect X% of Physical Damage"));
		AffixList.Add(new AffixData("Resistant", "+X% Monster Chaos Resistance" + newLine + "+X% Monster Elemental Resistance"));
		AffixList.Add(new AffixData("Savage", "X% increased Monster Damage"));
		AffixList.Add(new AffixData("Shocking", "Monsters deal X% extra Damage as Lightning"));
		AffixList.Add(new AffixData("Splitting", "Monsters fire 2 additional Projectiles"));
		AffixList.Add(new AffixData("Titan's", "Unique Boss has X% increased Life" + newLine + "Unique Boss has X% increased Area of Effect"));
		AffixList.Add(new AffixData("Unwavering", "Monsters cannot be Stunned" + newLine + "X% more Monster Life"));
		AffixList.Add(new AffixData("Empowered", "Monsters have a X% chance to cause Elemental Ailments on Hit"));
		AffixList.Add(new AffixData("Impaling", "Monsters have X% chance to Impale with Attacks"));
		AffixList.Add(new AffixData("Oppressive", "Monsters have +X% chance to Suppress Spell Damage"));
		AffixList.Add(new AffixData("Buffered", "Monsters gain X% of Maximum Life as Extra Maximum Energy Shield"));
		AffixList.Add(new AffixData("Enthralled", "Unique Bosses are Possessed"));
		AffixList.Add(new AffixData("Subterranean", "X% increased Experience gain"));
		AffixList.Add(new AffixData("Profane", "Monsters deal (31-35)% extra Physical Damage as Chaos" + newLine + "Monsters Inflict Withered for 2 seconds on Hit"));
		AffixList.Add(new AffixData("of Balance", "Players have Elemental Equilibrium"));
		AffixList.Add(new AffixData("of Bloodlines", "Magic Monster Packs each have a Bloodline Mod" + newLine + "X% more Magic Monsters"));
		AffixList.Add(new AffixData("of Endurance", "Monsters gain an Endurance Charge on Hit"));
		AffixList.Add(new AffixData("of Frenzy", "Monsters gain a Frenzy Charge on Hit"));
		AffixList.Add(new AffixData("of Power", "Monsters gain a Power Charge on Hit"));
		AffixList.Add(new AffixData("of Skirmishing", "Players have Point Blank"));
		AffixList.Add(new AffixData("of Venom", "Monsters Poison on Hit"));
		AffixList.Add(new AffixData("of Deadliness", "Monsters have X% increased Critical Strike Chance" + newLine + "+X% to Monster Critical Strike Multiplier"));
		AffixList.Add(new AffixData("of Drought", "Players gain X% reduced Flask Charges"));
		AffixList.Add(new AffixData("of Giants", "Monsters have X% increased Area of Effect"));
		AffixList.Add(new AffixData("of Impotence", "Players have X% less Area of Effect"));
		AffixList.Add(new AffixData("of Insulation", "Monsters have X% chance to Avoid Elemental Status Ailments"));
		AffixList.Add(new AffixData("of Miring", "Player Dodge chance is Unlucky" + newLine + "Monsters have X% increased Accuracy Rating"));
		AffixList.Add(new AffixData("of Rust", "Players have X% reduced Block Chance" + newLine + "Players have X% less Armour"));
		AffixList.Add(new AffixData("of Smothering", "Players have X% less Recovery Rate of Life and Energy Shield"));
		AffixList.Add(new AffixData("of Toughness", "Monsters take X% reduced Extra Damage from Critical Strikes"));
		AffixList.Add(new AffixData("of Elemental Weakness", "Players are Cursed with Elemental Weakness"));
		AffixList.Add(new AffixData("of Enfeeblement", "Players are Cursed with Enfeeble"));
		AffixList.Add(new AffixData("of Exposure", "-X% maximum Player Resistances"));
		AffixList.Add(new AffixData("of Stasis", "Players cannot Regenerate Life, Mana or Energy Shield"));
		AffixList.Add(new AffixData("of Temporal Chains", "Players are Cursed with Temporal Chains"));
		AffixList.Add(new AffixData("of Vulnerability", "Players are Cursed with Vulnerability"));
		AffixList.Add(new AffixData("of Congealment", "Cannot Leech Life from Monsters" + newLine + "Cannot Leech Mana from Monsters"));
		AffixList.Add(new AffixData("of Ice", "Area has patches of chilled ground"));
		AffixList.Add(new AffixData("of Flames", "Area has patches of burning ground"));
		AffixList.Add(new AffixData("of Lightning", "Area has patches of shocking ground"));
		AffixList.Add(new AffixData("of Desecration", "Area has patches of desecrated ground"));
		AffixList.Add(new AffixData("of Consecration", "Area has patches of Consecrated Ground"));
		AffixList.Add(new AffixData("of Blinding", "Monsters Blind on Hit"));
		AffixList.Add(new AffixData("of Carnage", "Monsters Maim on Hit with Attacks"));
		AffixList.Add(new AffixData("of Impedance", "Monsters Hinder on Hit with Spells"));
		AffixList.Add(new AffixData("of Enervation", "Monsters steal Power, Frenzy and Endurance charges on Hit"));
		AffixList.Add(new AffixData("of Fatigue", "Players have X% less Cooldown Recovery Rate"));
		AffixList.Add(new AffixData("of Transience", "Buffs on Players expire X% faster"));
		AffixList.Add(new AffixData("of Doubt", "Players have X% reduced effect of Non-Curse Auras from Skills"));
		AffixList.Add(new AffixData("of Imprecision", "Players have 25% less Accuracy Rating"));
	}

	private void InitDict()
	{
		foreach (AffixData affix in AffixList)
		{
			AffixDict.Add(affix.Name, affix);
		}
	}

	private void Load()
	{
		if (!File.Exists(string_0))
		{
			return;
		}
		string text = File.ReadAllText(string_0);
		if (!string.IsNullOrWhiteSpace(text))
		{
			Dictionary<string, Class31> dictionary = JsonConvert.DeserializeObject<Dictionary<string, Class31>>(text);
			if (dictionary != null)
			{
				foreach (AffixData affix in AffixList)
				{
					if (dictionary.TryGetValue(affix.Name, out var value))
					{
						affix.RerollMagic = value.RerollMagic;
						affix.RerollRare = value.RerollRare;
					}
				}
				return;
			}
			GlobalLog.Error("[MapBotEx] Fail to load \"AffixSettings.json\". Json deserealizer returned null.");
		}
		else
		{
			GlobalLog.Error("[MapBotEx] Fail to load \"AffixSettings.json\". File is empty.");
		}
	}

	private void Save()
	{
		Dictionary<string, Class31> dictionary = new Dictionary<string, Class31>(AffixList.Count);
		foreach (AffixData affix in AffixList)
		{
			Class31 value = new Class31
			{
				RerollMagic = affix.RerollMagic,
				RerollRare = affix.RerollRare
			};
			dictionary.Add(affix.Name, value);
		}
		string contents = JsonConvert.SerializeObject((object)dictionary, (Formatting)1);
		File.WriteAllText(string_0, contents);
	}

	static AffixSettings()
	{
		string_0 = Path.Combine(Configuration.Instance.Path, "MapBotEx", "AffixSettings.json");
	}
}
