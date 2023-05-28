using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.AutoLoginEx;

namespace ExPlugins.EXtensions;

public static class CurrencyNames
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ItemMetadata : Attribute
	{
		[CompilerGenerated]
		private string string_0;

		public string Metadata
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public ItemMetadata(string metadata)
		{
			Metadata = metadata;
		}
	}

	[ItemMetadata("Metadata/Items/Currency/CurrencyIdentification")]
	public static readonly string Wisdom;

	[ItemMetadata("Metadata/Items/Currency/CurrencyPortal")]
	public static readonly string Portal;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToMagic")]
	public static readonly string Transmutation;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAddModToMagic")]
	public static readonly string Augmentation;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollMagic")]
	public static readonly string Alteration;

	[ItemMetadata("Metadata/Items/Currency/CurrencyArmourQuality")]
	public static readonly string Scrap;

	[ItemMetadata("Metadata/Items/Currency/CurrencyWeaponQuality")]
	public static readonly string Whetstone;

	[ItemMetadata("Metadata/Items/Currency/CurrencyFlaskQuality")]
	public static readonly string Glassblower;

	[ItemMetadata("Metadata/Items/Currency/CurrencyMapQuality")]
	public static readonly string Chisel;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollSocketColours")]
	public static readonly string Chromatic;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeRandomly")]
	public static readonly string Chance;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToRare")]
	public static readonly string Alchemy;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollSocketNumbers")]
	public static readonly string Jeweller;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollSocketLinks")]
	public static readonly string Fusing;

	[ItemMetadata("Metadata/Items/Currency/CurrencyConvertToNormal")]
	public static readonly string Scouring;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollImplicit")]
	public static readonly string Blessed;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeMagicToRare")]
	public static readonly string Regal;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollRare")]
	public static readonly string Chaos;

	[ItemMetadata("Metadata/Items/Currency/CurrencyCorrupt")]
	public static readonly string Vaal;

	[ItemMetadata("Metadata/Items/Currency/CurrencyPassiveRefund")]
	public static readonly string Regret;

	[ItemMetadata("Metadata/Items/Currency/CurrencyGemQuality")]
	public static readonly string Gemcutter;

	[ItemMetadata("Metadata/Items/Currency/CurrencyModValues")]
	public static readonly string Divine;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAddModToRare")]
	public static readonly string Exalted;

	[ItemMetadata("Metadata/Items/Currency/CurrencyImprintOrb")]
	public static readonly string Eternal;

	[ItemMetadata("Metadata/Items/Currency/CurrencyDuplicate")]
	public static readonly string Mirror;

	[ItemMetadata("Metadata/Items/Currency/CurrencyPerandusCoin")]
	public static readonly string PerandusCoin;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachFireShard")]
	public static readonly string SplinterXoph;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachColdShard")]
	public static readonly string SplinterTul;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachLightningShard")]
	public static readonly string SplinterEsh;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachPhysicalShard")]
	public static readonly string SplinterUulNetol;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachChaosShard")]
	public static readonly string SplinterChayula;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachUpgradeUniqueFire")]
	public static readonly string BlessingXoph;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachUpgradeUniqueCold")]
	public static readonly string BlessingTul;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachUpgradeUniqueLightning")]
	public static readonly string BlessingEsh;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachUpgradeUniquePhysical")]
	public static readonly string BlessingUulNetol;

	[ItemMetadata("Metadata/Items/Currency/CurrencyBreachUpgradeUniqueChaos")]
	public static readonly string BlessingChayula;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRemoveMod")]
	public static readonly string Annulment;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToRareAndSetSockets")]
	public static readonly string Binding;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollMapType")]
	public static readonly string Horizon;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeMapTier")]
	public static readonly string Harbinger;

	[ItemMetadata("Metadata/Items/Currency/CurrencyStrongboxQuality")]
	public static readonly string Engineer;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollUnique")]
	public static readonly string Ancient;

	[ItemMetadata("Metadata/Items/Currency/CurrencyIdentificationShard")]
	public static readonly string ScrollFragment;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToMagicShard")]
	public static readonly string TransmutationShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollMagicShard")]
	public static readonly string AlterationShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToRareShard")]
	public static readonly string AlchemyShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRemoveModShard")]
	public static readonly string AnnulmentShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeToRareAndSetSocketsShard")]
	public static readonly string BindingShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollMapTypeShard")]
	public static readonly string HorizonShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeMapTierShard")]
	public static readonly string HarbingerShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyStrongboxQualityShard")]
	public static readonly string EngineerShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollUniqueShard")]
	public static readonly string AncientShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeMagicToRareShard")]
	public static readonly string RegalShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollRareShard")]
	public static readonly string ChaosShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAddModToRareShard")]
	public static readonly string ExaltedShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyDuplicateShard")]
	public static readonly string MirrorShard;

	[ItemMetadata("Metadata/Items/AtlasExiles/AddModToRareCrusader")]
	public static readonly string CrusadersExalted;

	[ItemMetadata("Metadata/Items/AtlasExiles/AddModToRareWarlord")]
	public static readonly string WarlordsExalted;

	[ItemMetadata("Metadata/Items/AtlasExiles/AddModToRareRedeemer")]
	public static readonly string RedeemersExalted;

	[ItemMetadata("Metadata/Items/AtlasExiles/AddModToRareHunter")]
	public static readonly string HuntersExalted;

	[ItemMetadata("Metadata/Items/AtlasExiles/ApplyInfluence")]
	public static readonly string Awakeners;

	[ItemMetadata("Metadata/Items/Currency/CurrencyInstillingOrb")]
	public static readonly string Instilling;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEnkindlingOrb")]
	public static readonly string Enkindling;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAtlasPassiveRefund")]
	public static readonly string Unmaking;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchIchor1")]
	public static readonly string IchorLesser;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchIchor2")]
	public static readonly string IchorGreater;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchIchor3")]
	public static readonly string IchorGrand;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchIchor4")]
	public static readonly string IchorExceptional;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchEmber1")]
	public static readonly string EmberLesser;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchEmber2")]
	public static readonly string EmberGreater;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchEmber3")]
	public static readonly string EmberGrand;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchEmber4")]
	public static readonly string EmberExceptional;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollDefences")]
	public static readonly string Sacred;

	[ItemMetadata("Metadata/Items/Currency/HarvestSeedGreen")]
	public static readonly string VividLifeforce;

	[ItemMetadata("Metadata/Items/Currency/HarvestSeedBlue")]
	public static readonly string PrimalLifeforce;

	[ItemMetadata("Metadata/Items/Currency/HarvestSeedRed")]
	public static readonly string WildLifeforce;

	[ItemMetadata("Metadata/Items/Currency/HarvestSeedBoss")]
	public static readonly string SacredLifeforce;

	[ItemMetadata("Metadata/Items/Currency/CurrencyRerollRareVeiled")]
	public static readonly string VeiledChaos;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAddAtlasModHigh")]
	public static readonly string AwakenedSextant;

	[ItemMetadata("Metadata/Items/Currency/CurrencyAddAtlasModMaven")]
	public static readonly string ElevatedSextant;

	[ItemMetadata("Metadata/Items/Currency/CurrencyFractureRareShard")]
	public static readonly string FracturingShard;

	[ItemMetadata("Metadata/Items/Currency/CurrencyFractureRare")]
	public static readonly string FracturingOrb;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeRerollRare")]
	public static readonly string TaintedChaos;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeAddModToRare")]
	public static readonly string TaintedExalted;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeWeaponQuality")]
	public static readonly string TaintedWhetstone;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeArmourQuality")]
	public static readonly string TaintedScrap;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeUpgradeModTier")]
	public static readonly string TaintedDivine;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeUpgradeToUnique")]
	public static readonly string TaintedMythic;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeRerollSocketLinks")]
	public static readonly string TaintedFusing;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeRerollSocketNumbers")]
	public static readonly string TaintedJeweller;

	[ItemMetadata("Metadata/Items/Currency/CurrencyHellscapeRerollSocketColours")]
	public static readonly string TaintedChromatic;

	[ItemMetadata("Metadata/Items/Currency/CurrencyConflictOrb")]
	public static readonly string Conflict;

	[ItemMetadata("Metadata/Items/Currency/CurrencyUpgradeInfluenceMod")]
	public static readonly string Dominance;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchRerollRare")]
	public static readonly string EldrichChaos;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchRemoveMod")]
	public static readonly string EldrichAnnulment;

	[ItemMetadata("Metadata/Items/Currency/CurrencyEldritchAddModToRare")]
	public static readonly string EldrichExalted;

	internal static Dictionary<string, string> dictionary_0;

	public static List<string> CurrencyNameList;

	static CurrencyNames()
	{
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		CurrencyNameList = new List<string>();
		if (!LokiPoe.IsInGame)
		{
			if (!string.IsNullOrWhiteSpace(AutoLoginSettings.Instance.LastLeague))
			{
				PoeNinjaTracker.Init(AutoLoginSettings.Instance.LastLeague).GetAwaiter().GetResult();
			}
		}
		else
		{
			PoeNinjaTracker.Init().GetAwaiter().GetResult();
		}
		Dictionary<string, FieldInfo> dictionary = new Dictionary<string, FieldInfo>();
		FieldInfo[] fields = typeof(CurrencyNames).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			ItemMetadata customAttribute = fieldInfo.GetCustomAttribute<ItemMetadata>();
			if (customAttribute != null)
			{
				dictionary.Add(customAttribute.Metadata, fieldInfo);
			}
		}
		int count = dictionary.Count;
		int num = 0;
		foreach (DatBaseItemTypeWrapper baseItemType in Dat.BaseItemTypes)
		{
			if (num < count)
			{
				if (dictionary.TryGetValue(baseItemType.Metadata, out var value))
				{
					value.SetValue(null, baseItemType.Name);
					CurrencyNameList.Add(baseItemType.Name);
					num++;
				}
				continue;
			}
			break;
		}
		if (num < count)
		{
			GlobalLog.Error("[CurrencyNames] Update is required. Not all fields were initialized.");
			BotManager.Stop(new StopReasonData("currency_init_fail", "Update is required. Not all fields were initialized.", (object)null), false);
		}
		dictionary_0 = new Dictionary<string, string>
		{
			[ScrollFragment] = Wisdom,
			[TransmutationShard] = Transmutation,
			[AlterationShard] = Alteration,
			[AlchemyShard] = Alchemy,
			[AnnulmentShard] = Annulment,
			[BindingShard] = Binding,
			[HorizonShard] = Horizon,
			[HarbingerShard] = Harbinger,
			[EngineerShard] = Engineer,
			[AncientShard] = Ancient,
			[RegalShard] = Regal,
			[ChaosShard] = Chaos,
			[ExaltedShard] = Exalted,
			[MirrorShard] = Mirror
		};
		CurrencyNameList = CurrencyNameList.OrderBy((string x) => x).ToList();
	}
}
