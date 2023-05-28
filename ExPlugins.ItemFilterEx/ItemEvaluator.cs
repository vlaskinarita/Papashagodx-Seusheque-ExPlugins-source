using System;
using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.MapBotEx;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.StashManager;

namespace ExPlugins.ItemFilterEx;

public class ItemEvaluator : IItemEvaluator
{
	private static readonly ItemEvaluator itemEvaluator_0;

	public static ItemEvaluator Instance;

	private static readonly ItemFilterExSettings Config;

	public string Name => "ItemEvaluatorEx";

	public bool Match(Item item, EvaluationType type)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Invalid comparison between Unknown and I4
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Invalid comparison between Unknown and I4
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Invalid comparison between Unknown and I4
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Invalid comparison between Unknown and I4
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Invalid comparison between Unknown and I4
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Invalid comparison between Unknown and I4
		//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Invalid comparison between Unknown and I4
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Invalid comparison between Unknown and I4
		//IL_14e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e6: Invalid comparison between Unknown and I4
		//IL_161a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1620: Invalid comparison between Unknown and I4
		//IL_1652: Unknown result type (might be due to invalid IL or missing references)
		//IL_1658: Invalid comparison between Unknown and I4
		//IL_173b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1741: Invalid comparison between Unknown and I4
		//IL_1aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Invalid comparison between Unknown and I4
		//IL_1bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc3: Invalid comparison between Unknown and I4
		//IL_1c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1c: Invalid comparison between Unknown and I4
		//IL_1d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1b: Invalid comparison between Unknown and I4
		//IL_1e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3e: Invalid comparison between Unknown and I4
		//IL_1f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2f: Invalid comparison between Unknown and I4
		//IL_1f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f40: Invalid comparison between Unknown and I4
		//IL_2029: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Invalid comparison between Unknown and I4
		try
		{
			if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null) && (int)type != 0)
			{
				if ((int)type != 6)
				{
					if ((int)type == 5)
					{
						if (item.SocketCount != 6 || (int)item.Rarity > 2)
						{
							if (!item.IsSmallRgb() || (int)item.Rarity > 2)
							{
								if (item.IsMap())
								{
									return (int)item.Rarity == 3 || (int)item.Rarity == 1 || Config.IdMaps;
								}
								return true;
							}
							return false;
						}
						return false;
					}
					if ((int)type == 1)
					{
						if ((int)item.Rarity != 6)
						{
							if (!Config.AlwaysSellItemNames.Any((NameEntry i) => i.Name.EqualsIgnorecase(item.Name)))
							{
								if (Config.AlwaysPickupItemNames.Any((NameEntry i) => i.Name.EqualsIgnorecase(item.Name)))
								{
									return true;
								}
								if (!World.Act1.TwilightStrand.IsCurrentArea)
								{
									if (item.Name.Equals("Stone of Passage"))
									{
										return true;
									}
									if (item.Name.ContainsIgnorecase("Contract") && !item.Metadata.Contains("HeistContractTutorial") && !item.Metadata.Contains("HeistContractQuest"))
									{
										Dictionary<string, int> dictionary = new Dictionary<string, int>();
										foreach (NameEntry item2 in Config.ContractJobsToSave)
										{
											int value = 0;
											string key;
											if (item2.Name.Contains(","))
											{
												key = item2.Name.Split(',')[0];
												value = int.Parse(item2.Name.Split(',')[1]);
											}
											else
											{
												key = item2.Name;
											}
											dictionary.Add(key, value);
										}
										foreach (KeyValuePair<string, int> jobsRequire in item.Components.HeistContractComponent.JobsRequires)
										{
											if (dictionary.ContainsKey(jobsRequire.Key) && (dictionary[jobsRequire.Key] == 0 || dictionary[jobsRequire.Key] >= jobsRequire.Value))
											{
												return true;
											}
										}
									}
									if (!Config.PickupAllRares || (int)item.Rarity != 2)
									{
										if (LocalData.MapMods.ContainsKey((StatTypeGGG)10342) || (int)item.Rarity != 2 || ((!(item.Class == "Ring") || !Config.PickupRareRings) && (!(item.Class == "Amulet") || !Config.PickupRareAmulets) && (!item.Class.Contains("Jewel") || !Config.PickupRareJewels) && (!(item.Class == "Belt") || !Config.PickupRareBelts)))
										{
											if (item.MaxLinkCount != 6)
											{
												if (item.FullName.Contains("Reliquary Key"))
												{
													return true;
												}
												if (!item.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }) || (int)item.Rarity == 3)
												{
													if (item.Metadata.ContainsIgnorecase("Metadata/Items/Currency/ScoutingReports/AtlasScoutingReport"))
													{
														return Config.MinScoutingReportPriceToKeep <= -1.0 || item.Price() >= Config.MinScoutingReportPriceToKeep;
													}
													if (!(item.Class == "ExpeditionLogbook") || !Config.PickupLogBooks)
													{
														if (!((IAuthored)BotManager.Current).Name.Equals("QuestBotEx") || item.MaxLinkCount < 4 || (int)item.Rarity != 2)
														{
															if (!item.Metadata.Contains("Metadata/Items/Currency/CurrencyEldritch"))
															{
																if (!item.Metadata.Contains("Metadata/Items/AtlasUpgrades"))
																{
																	if (!item.Name.Contains("Oil Extractor"))
																	{
																		if (!item.Name.Contains("Timeless"))
																		{
																			if (!item.Name.Contains("Simulacrum"))
																			{
																				if (!item.Name.Contains("Delirium Orb"))
																				{
																					if (item.Name.Contains("Breachstone"))
																					{
																						return !(Config.MinBreachstonePriceToKeep > -1.0) || item.Price() >= Config.MinBreachstonePriceToKeep;
																					}
																					if (!item.Name.Contains("Offering to the Goddess") || InstanceInfo.TotalAscendencyPoints >= 8)
																					{
																						if (!item.Metadata.Contains("Delve"))
																						{
																							if (item.Class == "MapFragment" && !item.Name.Contains("to the Goddess"))
																							{
																								return true;
																							}
																							if (!item.Metadata.StartsWith("Metadata/Items/Currency/CurrencyJewelleryQuality"))
																							{
																								if (!item.Name.Contains("Golden Key"))
																								{
																									if (!item.Metadata.StartsWith("Metadata/Items/Currency/Mushrune"))
																									{
																										if (!item.Name.Contains("Scarab"))
																										{
																											if (!item.Name.Contains("Incubator"))
																											{
																												if (!item.Name.Contains("Watchstone"))
																												{
																													if (!(item.Class == "ExpeditionLogbook"))
																													{
																														if (item.Class == "StackableCurrency" || (int)item.Rarity == 5)
																														{
																															string string_ = item.Name;
																															if (string_.Contains("Essence") && (string_.Contains("Horror") || string_.Contains("Delirium") || string_.Contains("Hysteria") || string_.Contains("Insanity") || string_.Contains("Screaming") || string_.Contains("Shrieking") || string_.Contains("Deafening")))
																															{
																																return Config.PickupEssences;
																															}
																															if (!string_.Contains("Essence"))
																															{
																																if (!(item.Class == "StackableCurrency") || !string_.Contains("Scroll"))
																																{
																																	if (Config.CustomScrollHandler && Config.IsStashCached && (string_ == "Blacksmith's Whetstone" || string_ == "Orb of Transmutation" || string_ == "Armourer's Scrap"))
																																	{
																																		Config.CachedCurrency.TryGetValue(CurrencyNames.Wisdom, out var value2);
																																		if (value2 < Config.CustomScrollAmount)
																																		{
																																			return true;
																																		}
																																	}
																																	CurrencyEntry currencyEntry = Config.CurrencyLimits.FirstOrDefault((CurrencyEntry l) => l.Name.Equals(string_, StringComparison.InvariantCultureIgnoreCase));
																																	if (currencyEntry == null)
																																	{
																																		return true;
																																	}
																																	if (!Config.IsStashCached)
																																	{
																																		return Config.ForceLimitCurrencyPickup;
																																	}
																																	Config.CachedCurrency.TryGetValue(string_, out var value3);
																																	int num = Inventories.InventoryItems.Count((Item c) => c.FullName == string_);
																																	int num2 = value3 + num;
																																	bool flag = num2 < currencyEntry.Amount;
																																	GlobalLog.Debug($"[Pickup] {string_}, limit: {num2}/{currencyEntry.Amount}, shouldPickup: {flag}");
																																	return flag;
																																}
																																int num3 = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Name.Equals(string_)).Sum((Item i) => i.StackCount);
																																return num3 < 35 && (item.StackCount >= Config.MinScrollStackToPickup || ((IAuthored)BotManager.Current).Name == "QuestBotEx");
																															}
																															return Config.PickupLowTierEssences;
																														}
																														if ((int)item.Rarity != 3)
																														{
																															if (item.IsMap())
																															{
																																string name = item.Name;
																																int int_0 = item.MapTier;
																																int maxMapAmountPickup = Config.MaxMapAmountPickup;
																																int num4 = Config.MapAmount + Inventories.InventoryItems.Count((Item m) => m.IsMap() && !m.IsBlightedMap() && !m.IsInfluencedMap());
																																if (!item.Stats.ContainsKey((StatTypeGGG)6548) && !item.Stats.ContainsKey((StatTypeGGG)13845) && !CheckIfShaperGuardian(item))
																																{
																																	if (item.Stats.ContainsKey((StatTypeGGG)10342))
																																	{
																																		return Config.PickupBlightedMaps;
																																	}
																																	if (!Config.MapsToKeep.Any((NameEntry g) => g.Name.EqualsIgnorecase(item.Name)))
																																	{
																																		bool flag2;
																																		if (GeneralSettings.Instance.AtlasExplorationEnabled && !MapExtensions.AtlasData.IsCompleted(item))
																																		{
																																			flag2 = (Config.MapOverLimit ? (num4 <= maxMapAmountPickup + 20) : (num4 <= maxMapAmountPickup));
																																			GlobalLog.Debug($"[Pickup] {name}(T{int_0}), ignored? {item.Ignored()} limit: {num4}/{maxMapAmountPickup} MapOverLimit: {Config.MapOverLimit}, shouldPickup: {flag2}");
																																			return flag2;
																																		}
																																		List<CachedMapItem> source = new List<CachedMapItem>();
																																		if (CachedMaps.Instance.MapCache != null && CachedMaps.Instance.MapCache.Maps.Any())
																																		{
																																			source = CachedMaps.Instance.MapCache.Maps.ToList();
																																		}
																																		int num5 = source.Count((CachedMapItem m) => m.MapTier == int_0);
																																		int num6 = Inventories.InventoryItems.Count((Item m) => m.MapTier == int_0 && !m.Stats.ContainsKey((StatTypeGGG)10342));
																																		int num7 = num5 + num6;
																																		int? num8 = Config.MapLimits.FirstOrDefault((MapEntry m) => m.MapTier == int_0)?.Amount;
																																		flag2 = num8 > num7;
																																		if (item.Ignored())
																																		{
																																			flag2 = false;
																																		}
																																		GlobalLog.Debug($"[Pickup] {name}(T{int_0}) ignored? {item.Ignored()} , limit: {num7}/{num8}, shouldPickup: {flag2}");
																																		return flag2;
																																	}
																																	GlobalLog.Debug($"[Pickup] {name}(T{int_0}), ignored? {item.Ignored()} limit: {num4}/{maxMapAmountPickup} MapOverLimit: {Config.MapOverLimit}");
																																	return true;
																																}
																																return true;
																															}
																															if (!item.IsDivinationCard())
																															{
																																if ((int)item.Rarity != 4)
																																{
																																	if (item.SocketCount != 6)
																																	{
																																		if (!item.Name.Equals("Stacked Deck"))
																																		{
																																			if (!Config.PickupSmallRgb || !item.IsSmallRgb() || LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
																																			{
																																				if (Config.PickupSmallRgb && item.IsTinyRgb() && LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
																																				{
																																					return true;
																																				}
																																				if (!item.IsVeiled)
																																				{
																																					if (!item.IsFractured || !ItemFilterExSettings.FractureBasesList.Contains(item.Name))
																																					{
																																						if (!item.Affixes.Any((ModAffix a) => a.InternalName.Contains("SynthesisImplicit")))
																																						{
																																							if (item.IsEnchanted())
																																							{
																																								Dictionary<string, int> dictionary2 = item.Affixes.Where((ModAffix a) => a.Category.Equals("SkillEnchantment")).ToDictionary((ModAffix a) => a.InternalName, (ModAffix a) => a.Values.First());
																																								bool flag3 = false;
																																								foreach (KeyValuePair<string, int> keyValuePair_0 in dictionary2)
																																								{
																																									flag3 = Config.EnchantsToSave.Any((EnchantEntry e) => e.InternalName.ContainsIgnorecase(keyValuePair_0.Key) && keyValuePair_0.Value >= e.Value);
																																								}
																																								GlobalLog.Debug(string.Format("[ItemEvaluator] {0} has enhants: [{1}] keep: {2}", item.FullName, string.Join(",", dictionary2.Keys), flag3));
																																								return flag3;
																																							}
																																							return false;
																																						}
																																						return Config.PickupSynthItems;
																																					}
																																					return Config.PickupFracturedItems;
																																				}
																																				return Config.PickupVeiledItems;
																																			}
																																			return true;
																																		}
																																		return true;
																																	}
																																	return Config.Pickup6Socket;
																																}
																																if (item.Quality >= Config.MinQualityForGemToPickup)
																																{
																																	return true;
																																}
																																if (!Config.AlwaysPickupGems.Any((NameEntry g) => g.Name.EqualsIgnorecase(item.Name)))
																																{
																																	return item.SkillGemLevel >= 20 && item.Price() >= Config.MinGemPriceToKeep;
																																}
																																return true;
																															}
																															if (!Config.DivCardsToKeep.Any((NameEntry g) => g.Name.EqualsIgnorecase(item.Name)))
																															{
																																double num9 = item.Price();
																																bool flag4 = Config.MinDivCardPriceToKeep <= -1.0 || num9 >= Config.MinDivCardPriceToKeep || item.MaxStackCount == 1;
																																GlobalLog.Debug($"[Pickup] {item.Name} card, price: {num9}, shouldPickup: {flag4}");
																																return flag4;
																															}
																															return true;
																														}
																														string string_0 = PoeNinjaTracker.GetUnidItemFullName(item.RenderArt);
																														double num10 = item.Price();
																														bool flag5 = !(num10 < Config.MinUniquePriceToKeep) || Config.UniquesToKeep.Any((NameEntry g) => g.Name.ContainsIgnorecase(string_0));
																														if (item.IsMap() && GeneralSettings.Instance.AtlasExplorationEnabled && !MapExtensions.AtlasData.IsCompleted(string_0) && !flag5 && MapData.SupportedUniqueMapNames().Contains(string_0))
																														{
																															flag5 = true;
																														}
																														if (Config.OfficialPricecheck.Any((OfficialPricecheckEntry i) => i.FullName.Equals(string_0)) && !item.IsIdentified)
																														{
																															flag5 = true;
																														}
																														if (Config.UniquesToAvoid.Any((NameEntry g) => g.Name.ContainsIgnorecase(string_0)))
																														{
																															flag5 = false;
																														}
																														GlobalLog.Warn(flag5 ? $"[Pickup] Picking up {string_0}. Price: {num10} Treshold: {Config.MinUniquePriceToKeep}" : $"[Pickup] Skipping {string_0}. Price: {num10} Treshold: {Config.MinUniquePriceToKeep}");
																														return flag5;
																													}
																													return true;
																												}
																												return true;
																											}
																											return true;
																										}
																										return Config.MinScarabPriceToKeep <= -1.0 || item.Price() >= Config.MinScarabPriceToKeep;
																									}
																									return Config.MinOilPriceToKeep <= -1.0 || item.Price() >= Config.MinOilPriceToKeep;
																								}
																								return true;
																							}
																							return true;
																						}
																						return Config.MinFossilPriceToKeep <= -1.0 || item.Price() >= Config.MinFossilPriceToKeep;
																					}
																					return true;
																				}
																				return true;
																			}
																			return true;
																		}
																		return true;
																	}
																	return true;
																}
																return true;
															}
															return true;
														}
														return true;
													}
													return true;
												}
												if (item.ItemLevel < Config.MinClusterIlvlToKeep)
												{
													return item.Price() >= Config.MinClusterPriceToKeep;
												}
												return true;
											}
											return true;
										}
										return true;
									}
									return true;
								}
								return true;
							}
							return false;
						}
						return true;
					}
					if ((int)type == 3)
					{
						string name2 = item.Name;
						if (Config.OfficialPricecheck.Any((OfficialPricecheckEntry e) => e.FullName.Equals(item.FullName)))
						{
							item.Price();
						}
						if (item.IsFractured)
						{
							if (Config.PriceCheckFracturedItems)
							{
								if (!ItemFilterExSettings.FractureBasesList.Contains(name2))
								{
									return true;
								}
								double num11 = item.Price();
								if (num11 > 0.0)
								{
									GlobalLog.Warn($"Fractured: {item.FullName} {item.Name} price: {num11}c");
								}
								return num11 < Config.MinFracturedPriceToKeep;
							}
							return !Config.PickupFracturedItems;
						}
						if (Config.AlwaysPickupItemNames.Any((NameEntry x) => x.Name.EqualsIgnorecase(item.Name)))
						{
							return false;
						}
						if (Config.AlwaysSellItemNames.Any((NameEntry x) => x.Name.EqualsIgnorecase(item.Name)))
						{
							return true;
						}
						if ((int)item.Rarity == 3 && Config.UniquesToKeep.Any((NameEntry g) => g.Name.ContainsIgnorecase(item.FullName)))
						{
							return false;
						}
						if ((int)item.Rarity == 3 && Config.AlwaysSellUniques.Any((NameEntry g) => g.Name.ContainsIgnorecase(item.FullName)))
						{
							return true;
						}
						if (PluginManager.EnabledPlugins.Any((IPlugin n) => ((IAuthored)n).Name == "StashManager") && StashManagerSettings.Instance.ItemsToSellInStack.Any((global::ExPlugins.StashManager.NameEntry i) => i.Name.EqualsIgnorecase(item.Name)) && item.StackCount >= 3)
						{
							return true;
						}
						if (item.IsMap())
						{
							return false;
						}
						if (item.Name.ContainsIgnorecase("Contract"))
						{
							return false;
						}
						if (item.Metadata.Equals("Metadata/Items/Incursion/ItemisedTemple"))
						{
							return false;
						}
						if ((int)item.Rarity == 2 && ((item.Class == "Ring" && Config.PickupRareRings) || (item.Class == "Amulet" && Config.PickupRareAmulets) || (item.Class.Contains("Jewel") && Config.PickupRareJewels) || (item.Class == "Belt" && Config.PickupRareBelts)))
						{
							return false;
						}
						if (item.Metadata.Contains("CurrencyItemisedSextantModifier"))
						{
							return false;
						}
						if (item.FullName.Contains("Reliquary Key"))
						{
							return false;
						}
						if (item.Metadata.StartsWith("Metadata/Items/Currency/CurrencyJewelleryQuality"))
						{
							return false;
						}
						if (item.Metadata.ContainsIgnorecase("Metadata/Items/Currency/ScoutingReports/AtlasScoutingReport"))
						{
							return item.Price() < Config.MinScoutingReportPriceToKeep;
						}
						if (item.Name.Contains("Quicksilver Flask") && ((Player)LokiPoe.Me).Level < 12)
						{
							return false;
						}
						if (item.Name.Contains("Bismuth Flask") && ((Player)LokiPoe.Me).Level < 24)
						{
							return false;
						}
						if (item.Metadata.Contains("Metadata/Items/Currency/CurrencyEldritch"))
						{
							return false;
						}
						if (item.Metadata.Contains("Metadata/Items/AtlasUpgrades"))
						{
							return false;
						}
						if (item.Name.Contains("Oil Extractor"))
						{
							return false;
						}
						if (item.Class == "ExpeditionLogbook")
						{
							return false;
						}
						if (item.Metadata.Contains("CurrencyHellscape"))
						{
							return false;
						}
						if (item.Metadata.Contains("Delve"))
						{
							return item.Price() < Config.MinFossilPriceToKeep;
						}
						if ((item.Metadata.StartsWith("Metadata/Items/Currency/Mushrune") || name2.Contains("Essence")) && Config.UpgradeOilEssences && item.Price() <= Config.MaxOilEssencePriceToSellInStack && item.StackCount >= Config.OilEssenceStackCountToUpgrade && !GeneralSettings.Instance.AnointOils.Any((OilEntry o) => item.FullName.Equals(o.Name) && o.CurrentAmount < 200 && (o.UseOnBlighted || o.UseOnRavaged)) && !name2.Contains("Deafening") && !name2.Contains("Horror") && !name2.Contains("Delirium") && !name2.Contains("Hysteria") && !name2.Contains("Insanity"))
						{
							return true;
						}
						if (item.Name.Equals("Stacked Deck"))
						{
							return false;
						}
						if (item.IsEnchanted())
						{
							return false;
						}
						if (item.Class == "StackableCurrency" || (int)item.Rarity == 5)
						{
							if (!(((IAuthored)BotManager.Current).Name == "QuestBotEx") || ((Player)LokiPoe.Me).Level > 11 || (!(item.Name == CurrencyNames.Transmutation) && !(item.Name == CurrencyNames.Scrap) && !(item.Name == CurrencyNames.Whetstone)))
							{
								if (Config.CustomScrollHandler && Config.IsStashCached && (name2 == "Blacksmith's Whetstone" || name2 == "Orb of Transmutation" || name2 == "Armourer's Scrap"))
								{
									Config.CachedCurrency.TryGetValue(CurrencyNames.Wisdom, out var value4);
									if (value4 < Config.CustomScrollAmount)
									{
										return true;
									}
								}
								return false;
							}
							return true;
						}
						if ((int)item.Rarity == 4 && ((IAuthored)BotManager.Current).Name != "QuestBotEx")
						{
							return item.Quality == 0 && item.Price() < Config.MinGemPriceToKeep;
						}
						if ((int)item.Rarity == 4 && Config.AlwaysPickupGems.Any((NameEntry g) => g.Name.EqualsIgnorecase(item.Name)))
						{
							return false;
						}
						if (item.Name.Contains("Breachstone"))
						{
							return item.Price() < Config.MinBreachstonePriceToKeep;
						}
						if (item.Class == "MapFragment" && !item.Name.Contains("to the Goddess") && !item.Name.Contains("Vessel"))
						{
							return false;
						}
						if (item.Name.Contains("Timeless"))
						{
							return false;
						}
						if (item.Metadata == "Metadata/Items/Heist/HeistCoin")
						{
							return false;
						}
						if (item.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)3335 }) && (int)item.Rarity != 3)
						{
							if (item.ItemLevel >= Config.MinClusterIlvlToKeep)
							{
								return false;
							}
							return item.Price() < Config.MinClusterPriceToKeep;
						}
						if (item.Name.Contains("Offering to the Goddess"))
						{
							return false;
						}
						if (item.Name.Contains("Simulacrum"))
						{
							return false;
						}
						if (item.Name.Contains("Delirium Orb"))
						{
							return false;
						}
						if (item.Name.Contains("Scarab"))
						{
							return item.Price() < Config.MinScarabPriceToKeep;
						}
						if (item.Name.Contains("Incubator"))
						{
							return false;
						}
						if (item.Name.Contains("Watchstone"))
						{
							return false;
						}
						if (item.MaxLinkCount == 6)
						{
							if ((int)item.Rarity != 3)
							{
								return true;
							}
							double num12 = item.Price();
							double num13 = PoeNinjaTracker.LookupCurrencyChaosValueByFullName("Orb of Fusing");
							double num14 = num13 * 40.0;
							bool flag6 = item.Price() < num14;
							GlobalLog.Debug($"[Sell] Unique 6-link item {item.FullName}, price: {num12} less than 40x fusing {num14}c, shouldSell: {flag6}");
							return flag6;
						}
						if ((int)item.Rarity == 3)
						{
							if (GeneralSettings.Instance.AtlasExplorationEnabled && !MapExtensions.AtlasData.IsCompleted(item) && MapData.SupportedUniqueMapNames().Contains(item.FullName))
							{
								return false;
							}
							bool flag7 = item.Price() < Config.MinUniquePriceToKeep;
							GlobalLog.Debug($"[Sell] {item.FullName} unique, price: {item.Price()}, shouldSell: {flag7}");
							return flag7;
						}
						if (item.IsDivinationCard())
						{
							if (!Config.DivCardsToKeep.Any((NameEntry g) => g.Name.EqualsIgnorecase(item.Name)))
							{
								double num15 = item.Price();
								bool flag8 = item.Price() < Config.MinDivCardPriceToKeep && item.MaxStackCount != 1;
								GlobalLog.Debug($"[Sell] {item.FullName} card, price: {num15}, shouldSell: {flag8}");
								return flag8;
							}
							return false;
						}
						if (item.SocketCount == 6)
						{
							return true;
						}
						if (item.IsVeiled)
						{
							return !Config.PickupVeiledItems;
						}
						if (Config.PickupAllRares && (int)item.Rarity == 2)
						{
							return Config.SellAllRares;
						}
						if ((int)item.Rarity <= 2 && item.Price() < 0.0)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
			return false;
		}
		catch (Exception message)
		{
			GlobalLog.Error(message);
			return false;
		}
	}

	private static bool CheckIfShaperGuardian(Item map)
	{
		if (map.Stats.ContainsKey((StatTypeGGG)6827) && map.Stats[(StatTypeGGG)6827] == 1 && (map.Name.Contains("Chimera") || map.Name.Contains("Hydra") || map.Name.Contains("Phoenix") || map.Name.Contains("Minotaur")))
		{
			return true;
		}
		return false;
	}

	static ItemEvaluator()
	{
		Instance = itemEvaluator_0 ?? (itemEvaluator_0 = new ItemEvaluator());
		Config = ItemFilterExSettings.Instance;
	}
}
