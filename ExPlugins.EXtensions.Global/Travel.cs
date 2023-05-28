using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.Global;

public static class Travel
{
	private static readonly HashSet<AreaInfo> hashSet_0;

	private static readonly Dictionary<AreaInfo, Func<Task>> dictionary_0;

	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static readonly TgtPosition tgtPosition_2;

	private static readonly TgtPosition tgtPosition_3;

	private static readonly TgtPosition tgtPosition_4;

	private static readonly TgtPosition tgtPosition_5;

	private static readonly TgtPosition smvajQroSX;

	private static readonly TgtPosition YeSaHvVqIk;

	private static readonly TgtPosition tgtPosition_6;

	private static readonly TgtPosition tgtPosition_7;

	private static readonly TgtPosition tgtPosition_8;

	private static readonly TgtPosition tgtPosition_9;

	private static readonly TgtPosition tgtPosition_10;

	private static readonly TgtPosition tgtPosition_11;

	private static readonly TgtPosition tgtPosition_12;

	private static readonly TgtPosition tgtPosition_13;

	private static readonly TgtPosition tgtPosition_14;

	private static readonly TgtPosition tgtPosition_15;

	private static readonly TgtPosition tgtPosition_16;

	private static readonly TgtPosition tgtPosition_17;

	private static readonly TgtPosition tgtPosition_18;

	private static readonly TgtPosition tgtPosition_19;

	private static readonly TgtPosition tgtPosition_20;

	private static readonly TgtPosition tgtPosition_21;

	private static readonly TgtPosition RusadDrrNl;

	private static readonly TgtPosition tgtPosition_22;

	private static readonly TgtPosition tgtPosition_23;

	private static readonly TgtPosition tgtPosition_24;

	private static readonly TgtPosition tgtPosition_25;

	private static readonly TgtPosition tgtPosition_26;

	private static readonly TgtPosition tgtPosition_27;

	private static readonly TgtPosition tgtPosition_28;

	private static readonly TgtPosition tgtPosition_29;

	private static readonly TgtPosition tgtPosition_30;

	private static readonly TgtPosition tgtPosition_31;

	private static readonly TgtPosition tgtPosition_32;

	private static readonly TgtPosition tgtPosition_33;

	private static readonly TgtPosition EwLaByreiw;

	private static readonly TgtPosition tgtPosition_34;

	private static readonly TgtPosition tgtPosition_35;

	private static readonly TgtPosition tgtPosition_36;

	private static readonly TgtPosition tgtPosition_37;

	private static readonly TgtPosition tgtPosition_38;

	private static readonly TgtPosition tgtPosition_39;

	private static readonly TgtPosition tgtPosition_40;

	private static readonly TgtPosition tgtPosition_41;

	private static readonly TgtPosition tgtPosition_42;

	private static readonly TgtPosition tgtPosition_43;

	private static readonly TgtPosition tgtPosition_44;

	private static readonly TgtPosition tgtPosition_45;

	private static readonly TgtPosition tgtPosition_46;

	private static readonly TgtPosition tgtPosition_47;

	private static readonly TgtPosition tgtPosition_48;

	private static readonly TgtPosition xiwazcMorg;

	private static readonly TgtPosition tgtPosition_49;

	private static readonly TgtPosition tgtPosition_50;

	private static readonly TgtPosition tgtPosition_51;

	private static readonly TgtPosition tgtPosition_52;

	private static readonly TgtPosition tgtPosition_53;

	private static readonly TgtPosition tgtPosition_54;

	private static readonly TgtPosition tgtPosition_55;

	private static readonly TgtPosition tgtPosition_56;

	private static readonly TgtPosition tgtPosition_57;

	private static readonly TgtPosition tgtPosition_58;

	private static readonly TgtPosition tgtPosition_59;

	private static readonly TgtPosition tgtPosition_60;

	private static readonly TgtPosition tgtPosition_61;

	private static readonly TgtPosition tgtPosition_62;

	private static readonly TgtPosition tgtPosition_63;

	private static readonly TgtPosition tgtPosition_64;

	private static readonly TgtPosition tgtPosition_65;

	private static readonly TgtPosition tgtPosition_66;

	private static readonly TgtPosition tgtPosition_67;

	private static readonly TgtPosition tgtPosition_68;

	private static readonly TgtPosition tgtPosition_69;

	private static readonly TgtPosition tgtPosition_70;

	private static readonly TgtPosition tgtPosition_71;

	private static readonly TgtPosition keicOaiOub;

	private static readonly TgtPosition tgtPosition_72;

	private static readonly TgtPosition tgtPosition_73;

	private static readonly TgtPosition tgtPosition_74;

	private static readonly TgtPosition tgtPosition_75;

	private static readonly TgtPosition tgtPosition_76;

	private static readonly TgtPosition tgtPosition_77;

	private static readonly TgtPosition tgtPosition_78;

	private static readonly TgtPosition tgtPosition_79;

	private static readonly TgtPosition tgtPosition_80;

	private static readonly TgtPosition tgtPosition_81;

	private static readonly TgtPosition tgtPosition_82;

	private static readonly TgtPosition tgtPosition_83;

	private static readonly TgtPosition tgtPosition_84;

	private static readonly TgtPosition tgtPosition_85;

	private static readonly TgtPosition tgtPosition_86;

	private static readonly TgtPosition tgtPosition_87;

	private static readonly TgtPosition tgtPosition_88;

	private static readonly TgtPosition tgtPosition_89;

	private static readonly TgtPosition tgtPosition_90;

	private static readonly TgtPosition tgtPosition_91;

	private static readonly TgtPosition tgtPosition_92;

	private static readonly TgtPosition tgtPosition_93;

	private static readonly TgtPosition tgtPosition_94;

	private static readonly TgtPosition tgtPosition_95;

	private static readonly TgtPosition tgtPosition_96;

	private static readonly TgtPosition tgtPosition_97;

	private static readonly TgtPosition tgtPosition_98;

	private static readonly TgtPosition tgtPosition_99;

	private static readonly TgtPosition tgtPosition_100;

	private static readonly TgtPosition tgtPosition_101;

	private static readonly TgtPosition tgtPosition_102;

	private static readonly TgtPosition tgtPosition_103;

	private static readonly TgtPosition tgtPosition_104;

	private static readonly TgtPosition tgtPosition_105;

	private static readonly TgtPosition tgtPosition_106;

	private static readonly TgtPosition tgtPosition_107;

	private static readonly TgtPosition tgtPosition_108;

	private static readonly TgtPosition tgtPosition_109;

	private static readonly TgtPosition tgtPosition_110;

	private static readonly TgtPosition tgtPosition_111;

	private static readonly TgtPosition tgtPosition_112;

	private static readonly TgtPosition tgtPosition_113;

	private static readonly TgtPosition tgtPosition_114;

	private static readonly TgtPosition tgtPosition_115;

	private static readonly TgtPosition tgtPosition_116;

	private static readonly TgtPosition tgtPosition_117;

	private static readonly TgtPosition tgtPosition_118;

	private static readonly TgtPosition tgtPosition_119;

	private static readonly TgtPosition tgtPosition_120;

	private static readonly TgtPosition tgtPosition_121;

	private static readonly TgtPosition tgtPosition_122;

	private static readonly WalkablePosition walkablePosition_0;

	private static readonly WalkablePosition walkablePosition_1;

	private static readonly WalkablePosition walkablePosition_2;

	private static readonly WalkablePosition walkablePosition_3;

	private static readonly WalkablePosition walkablePosition_4;

	private static readonly WalkablePosition walkablePosition_5;

	private static readonly WalkablePosition walkablePosition_6;

	private static readonly WalkablePosition walkablePosition_7;

	private static readonly WalkablePosition walkablePosition_8;

	private static readonly WalkablePosition walkablePosition_9;

	private static readonly WalkablePosition walkablePosition_10;

	private static readonly WalkablePosition walkablePosition_11;

	private static readonly WalkablePosition walkablePosition_12;

	private static readonly WalkablePosition walkablePosition_13;

	private static readonly WalkablePosition walkablePosition_14;

	private static readonly WalkablePosition walkablePosition_15;

	private static readonly WalkablePosition walkablePosition_16;

	private static readonly WalkablePosition walkablePosition_17;

	private static bool AnyWaypointNearby
	{
		get
		{
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			return currentArea.IsTown || currentArea.IsHideoutArea || ObjectManager.Objects.Exists((NetworkObject o) => o is Waypoint && o.Distance <= 70f && o.PathDistance() <= 73f);
		}
	}

	private static NetworkObject LooseCandle => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Library/HiddenDoorTrigger");

	private static NetworkObject DeshretSeal => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act4/MineEntranceSeal");

	public static async Task To(AreaInfo area)
	{
		if (dictionary_0.TryGetValue(area, out var handler))
		{
			await handler();
			return;
		}
		GlobalLog.Error($"[Travel] Unsupported area: {area}.");
		ErrorManager.ReportCriticalError();
	}

	public static void RequestNewInstance(AreaInfo area)
	{
		if (hashSet_0.Add(area))
		{
			GlobalLog.Debug($"[Travel] New instance requested for {area}");
		}
	}

	private static async Task UnknownState()
	{
		GlobalLog.Error($"[Travel] Lioneye's Watch waypoint is not opened and we are not inside Twilight Strand. Current area: {World.CurrentArea}");
		ErrorManager.ReportCriticalError();
	}

	private static async Task LioneyeWatch()
	{
		await WpAreaHandler(World.Act1.LioneyeWatch, tgtPosition_0, World.Act1.TwilightStrand, UnknownState);
	}

	private static async Task Coast()
	{
		await TownConnectedAreaHandler(World.Act1.Coast, walkablePosition_0, World.Act1.LioneyeWatch, LioneyeWatch);
	}

	private static async Task TidalIsland()
	{
		await NoWpAreaHandler(World.Act1.TidalIsland, tgtPosition_1, World.Act1.Coast, Coast);
	}

	private static async Task MudFlats()
	{
		await NoWpAreaHandler(World.Act1.MudFlats, tgtPosition_2, World.Act1.Coast, Coast);
	}

	private static async Task FetidPool()
	{
		await NoWpAreaHandler(World.Act1.FetidPool, tgtPosition_3, World.Act1.MudFlats, MudFlats);
	}

	private static async Task SubmergedPassage()
	{
		await WpAreaHandler(World.Act1.SubmergedPassage, tgtPosition_4, World.Act1.MudFlats, MudFlats);
	}

	private static async Task FloodedDepths()
	{
		await NoWpAreaHandler(World.Act1.FloodedDepths, tgtPosition_5, World.Act1.SubmergedPassage, SubmergedPassage);
	}

	private static async Task Ledge()
	{
		await WpAreaHandler(World.Act1.Ledge, smvajQroSX, World.Act1.SubmergedPassage, SubmergedPassage);
	}

	private static async Task Climb()
	{
		await WpAreaHandler(World.Act1.Climb, YeSaHvVqIk, World.Act1.Ledge, Ledge);
	}

	private static async Task LowerPrison()
	{
		await WpAreaHandler(World.Act1.LowerPrison, tgtPosition_6, World.Act1.Climb, Climb);
	}

	private static async Task UpperPrison()
	{
		await NoWpAreaHandler(World.Act1.UpperPrison, tgtPosition_7, World.Act1.LowerPrison, LowerPrison, delegate
		{
			tgtPosition_8.ResetCurrentPosition();
		});
	}

	private static async Task PrisonersGate()
	{
		await ThroughMultilevelAreaHander(World.Act1.PrisonerGate, tgtPosition_8, World.Act1.UpperPrison, UpperPrison);
	}

	private static async Task ShipGraveyard()
	{
		await WpAreaHandler(World.Act1.ShipGraveyard, tgtPosition_9, World.Act1.PrisonerGate, PrisonersGate);
	}

	private static async Task ShipGraveyardCave()
	{
		await NoWpAreaHandler(World.Act1.ShipGraveyardCave, tgtPosition_10, World.Act1.ShipGraveyard, ShipGraveyard);
	}

	private static async Task CavernOfWrath()
	{
		await WpAreaHandler(World.Act1.CavernOfWrath, tgtPosition_11, World.Act1.ShipGraveyard, ShipGraveyard);
	}

	private static async Task CavernOfAnger()
	{
		await NoWpAreaHandler(World.Act1.CavernOfAnger, tgtPosition_12, World.Act1.CavernOfWrath, CavernOfWrath, delegate
		{
			tgtPosition_13.ResetCurrentPosition();
		});
	}

	private static async Task SouthernForest()
	{
		await ThroughMultilevelAreaHander(World.Act2.SouthernForest, tgtPosition_13, World.Act1.CavernOfAnger, CavernOfAnger);
	}

	private static async Task ForestEncampment()
	{
		await WpAreaHandler(World.Act2.ForestEncampment, tgtPosition_14, World.Act2.SouthernForest, SouthernForest);
	}

	private static async Task Riverways()
	{
		await TownConnectedAreaHandler(World.Act2.Riverways, walkablePosition_1, World.Act2.ForestEncampment, ForestEncampment);
	}

	private static async Task WesternForest()
	{
		await WpAreaHandler(World.Act2.WesternForest, tgtPosition_15, World.Act2.Riverways, Riverways);
	}

	private static async Task WeaverChambers()
	{
		await NoWpAreaHandler(World.Act2.WeaverChambers, tgtPosition_16, World.Act2.WesternForest, WesternForest);
	}

	private static async Task OldFields()
	{
		await TownConnectedAreaHandler(World.Act2.OldFields, walkablePosition_2, World.Act2.ForestEncampment, ForestEncampment);
	}

	private static async Task Den()
	{
		await NoWpAreaHandler(World.Act2.Den, tgtPosition_17, World.Act2.OldFields, OldFields);
	}

	private static async Task Crossroads()
	{
		await WpAreaHandler(World.Act2.Crossroads, tgtPosition_18, World.Act2.OldFields, OldFields);
	}

	private static async Task ChamberOfSins1()
	{
		await WpAreaHandler(World.Act2.ChamberOfSins1, tgtPosition_19, World.Act2.Crossroads, Crossroads);
	}

	private static async Task ChamberOfSins2()
	{
		await NoWpAreaHandler(World.Act2.ChamberOfSins2, tgtPosition_20, World.Act2.ChamberOfSins1, ChamberOfSins1);
	}

	private static async Task BrokenBridge()
	{
		await WpAreaHandler(World.Act2.BrokenBridge, tgtPosition_21, World.Act2.Crossroads, Crossroads);
	}

	private static async Task FellshrineRuins()
	{
		if (World.Act2.FellshrineRuins.IsCurrentArea)
		{
			OuterLogicError(World.Act2.FellshrineRuins);
		}
		else if (World.Act2.Crossroads.IsCurrentArea)
		{
			WalkablePosition cachedPos = GetCachedTransitionPos(World.Act2.FellshrineRuins);
			if (cachedPos != null)
			{
				if (!cachedPos.IsFar)
				{
					await TakeTransition(World.Act2.FellshrineRuins, null);
				}
				else
				{
					cachedPos.Come();
				}
			}
			else if (walkablePosition_3.IsFar)
			{
				walkablePosition_3.Come();
			}
		}
		else
		{
			await Crossroads();
		}
	}

	private static async Task Crypt1()
	{
		await WpAreaHandler(World.Act2.Crypt1, RusadDrrNl, World.Act2.FellshrineRuins, FellshrineRuins);
	}

	private static async Task Crypt2()
	{
		await NoWpAreaHandler(World.Act2.Crypt2, tgtPosition_22, World.Act2.Crypt1, Crypt1);
	}

	private static async Task Wetlands()
	{
		await WpAreaHandler(World.Act2.Wetlands, tgtPosition_23, World.Act2.Riverways, Riverways);
	}

	private static async Task VaalRuins()
	{
		await NoWpAreaHandler(World.Act2.VaalRuins, tgtPosition_24, World.Act2.Wetlands, Wetlands);
	}

	private static async Task NorthernForest()
	{
		await WpAreaHandler(World.Act2.NorthernForest, tgtPosition_25, World.Act2.VaalRuins, VaalRuins);
	}

	private static async Task DreadThicket()
	{
		await NoWpAreaHandler(World.Act2.DreadThicket, tgtPosition_26, World.Act2.NorthernForest, NorthernForest);
	}

	private static async Task Caverns()
	{
		await WpAreaHandler(World.Act2.Caverns, tgtPosition_27, World.Act2.NorthernForest, NorthernForest);
	}

	private static async Task AncientPyramid()
	{
		await NoWpAreaHandler(World.Act2.AncientPyramid, tgtPosition_28, World.Act2.Caverns, Caverns, delegate
		{
			tgtPosition_29.ResetCurrentPosition();
		});
	}

	private static async Task CityOfSarn()
	{
		await ThroughMultilevelAreaHander(World.Act3.CityOfSarn, tgtPosition_29, World.Act2.AncientPyramid, AncientPyramid);
	}

	private static async Task SarnEncampment()
	{
		await WpAreaHandler(World.Act3.SarnEncampment, tgtPosition_30, World.Act3.CityOfSarn, CityOfSarn);
	}

	private static async Task Slums()
	{
		await TownConnectedAreaHandler(World.Act3.Slums, walkablePosition_4, World.Act3.SarnEncampment, SarnEncampment);
	}

	private static async Task Crematorium()
	{
		await WpAreaHandler(World.Act3.Crematorium, tgtPosition_31, World.Act3.Slums, Slums);
	}

	private static async Task Sewers()
	{
		await WpAreaHandler(World.Act3.Sewers, tgtPosition_32, World.Act3.Slums, Slums);
	}

	private static async Task Marketplace()
	{
		await WpAreaHandler(World.Act3.Marketplace, tgtPosition_33, World.Act3.Sewers, Sewers);
	}

	private static async Task Catacombs()
	{
		await NoWpAreaHandler(World.Act3.Catacombs, EwLaByreiw, World.Act3.Marketplace, Marketplace);
	}

	private static async Task Battlefront()
	{
		await WpAreaHandler(World.Act3.Battlefront, tgtPosition_34, World.Act3.Marketplace, Marketplace);
	}

	private static async Task Docks()
	{
		await WpAreaHandler(World.Act3.Docks, tgtPosition_35, World.Act3.Battlefront, Battlefront);
	}

	private static async Task Solaris1()
	{
		await WpAreaHandler(World.Act3.SolarisTemple1, tgtPosition_36, World.Act3.Battlefront, Battlefront);
	}

	private static async Task Solaris2()
	{
		await WpAreaHandler(World.Act3.SolarisTemple2, tgtPosition_37, World.Act3.SolarisTemple1, Solaris1);
	}

	private static async Task EbonyBarracks()
	{
		await WpAreaHandler(World.Act3.EbonyBarracks, tgtPosition_38, World.Act3.Sewers, Sewers);
	}

	private static async Task Lunaris1()
	{
		await WpAreaHandler(World.Act3.LunarisTemple1, tgtPosition_39, World.Act3.EbonyBarracks, EbonyBarracks);
	}

	private static async Task Lunaris2()
	{
		await NoWpAreaHandler(World.Act3.LunarisTemple2, tgtPosition_40, World.Act3.LunarisTemple1, Lunaris1);
	}

	private static async Task ImperialGardens()
	{
		await WpAreaHandler(World.Act3.ImperialGardens, tgtPosition_41, World.Act3.EbonyBarracks, EbonyBarracks);
	}

	private static async Task Library()
	{
		await WpAreaHandler(World.Act3.Library, tgtPosition_42, World.Act3.ImperialGardens, ImperialGardens);
	}

	private static async Task Archives()
	{
		if (!World.Act3.Archives.IsCurrentArea)
		{
			if (World.Act3.Library.IsCurrentArea)
			{
				WalkablePosition cachedPos = GetCachedTransitionPos(World.Act3.Archives);
				if (cachedPos != null)
				{
					if (cachedPos.IsFar)
					{
						cachedPos.Come();
					}
					else
					{
						await TakeTransition(World.Act3.Archives, tgtPosition_43, null);
					}
					return;
				}
				NetworkObject candle = LooseCandle;
				if (candle != (NetworkObject)null)
				{
					WalkablePosition candleWalkablePos = new WalkablePosition("Loose Candle", candle.Position);
					if (candleWalkablePos.IsFar)
					{
						candleWalkablePos.Come();
						return;
					}
					if (!candle.IsTargetable)
					{
						GlobalLog.Debug("Waiting for Archives transition.");
						await Wait.StuckDetectionSleep(200);
					}
					else
					{
						if (!(await PlayerAction.Interact(candle)))
						{
							ErrorManager.ReportError();
						}
						await Wait.SleepSafe(200);
					}
				}
				if (tgtPosition_43.IsFar)
				{
					tgtPosition_43.Come();
				}
			}
			else
			{
				await Library();
			}
		}
		else
		{
			OuterLogicError(World.Act3.Archives);
		}
	}

	private static async Task SceptreOfGod()
	{
		await WpAreaHandler(World.Act3.SceptreOfGod, tgtPosition_44, World.Act3.ImperialGardens, ImperialGardens, delegate
		{
			tgtPosition_45.ResetCurrentPosition();
		});
	}

	private static async Task UpperSceptreOfGod()
	{
		await ThroughMultilevelAreaHander(World.Act3.UpperSceptreOfGod, tgtPosition_45, World.Act3.SceptreOfGod, SceptreOfGod, delegate
		{
			tgtPosition_46.ResetCurrentPosition();
		});
	}

	private static async Task Aqueduct()
	{
		await ThroughMultilevelAreaHander(World.Act4.Aqueduct, tgtPosition_46, World.Act3.UpperSceptreOfGod, UpperSceptreOfGod);
	}

	private static async Task Highgate()
	{
		await WpAreaHandler(World.Act4.Highgate, tgtPosition_47, World.Act4.Aqueduct, Aqueduct);
	}

	private static async Task DriedLake()
	{
		await TownConnectedAreaHandler(World.Act4.DriedLake, walkablePosition_5, World.Act4.Highgate, Highgate);
	}

	private static async Task Mines1()
	{
		if (!World.Act4.Mines1.IsCurrentArea)
		{
			if (World.Act4.Highgate.IsCurrentArea)
			{
				await walkablePosition_6.ComeAtOnce();
				NetworkObject seal = DeshretSeal;
				if (!(seal != (NetworkObject)null) || !seal.IsTargetable)
				{
					await TakeTransition(World.Act4.Mines1, null);
					return;
				}
				GlobalLog.Error($"[Travel] Cannot travel to {World.Act4.Mines1}. Deshret Seal is active.");
				ErrorManager.ReportCriticalError();
			}
			else
			{
				await Highgate();
			}
		}
		else
		{
			OuterLogicError(World.Act4.Mines1);
		}
	}

	private static async Task Mines2()
	{
		await NoWpAreaHandler(World.Act4.Mines2, tgtPosition_48, World.Act4.Mines1, Mines1);
	}

	private static async Task CrystalVeins()
	{
		await WpAreaHandler(World.Act4.CrystalVeins, xiwazcMorg, World.Act4.Mines2, Mines2);
	}

	private static async Task KaomDream()
	{
		await NoWpAreaHandler(World.Act4.KaomDream, tgtPosition_49, World.Act4.CrystalVeins, CrystalVeins);
	}

	private static async Task KaomStronghold()
	{
		await WpAreaHandler(World.Act4.KaomStronghold, tgtPosition_50, World.Act4.KaomDream, KaomDream);
	}

	private static async Task DaressoDream()
	{
		await NoWpAreaHandler(World.Act4.DaressoDream, tgtPosition_49, World.Act4.CrystalVeins, CrystalVeins);
	}

	private static async Task GrandArena()
	{
		await WpAreaHandler(World.Act4.GrandArena, tgtPosition_51, World.Act4.DaressoDream, DaressoDream);
	}

	private static async Task Belly1()
	{
		await NoWpAreaHandler(World.Act4.BellyOfBeast1, tgtPosition_49, World.Act4.CrystalVeins, CrystalVeins);
	}

	private static async Task Belly2()
	{
		await NoWpAreaHandler(World.Act4.BellyOfBeast2, tgtPosition_52, World.Act4.BellyOfBeast1, Belly1);
	}

	private static async Task Harvest()
	{
		await WpAreaHandler(World.Act4.Harvest, tgtPosition_53, World.Act4.BellyOfBeast2, Belly2);
	}

	private static async Task Ascent()
	{
		await TownConnectedAreaHandler(World.Act4.Ascent, walkablePosition_7, World.Act4.Highgate, Highgate);
	}

	private static async Task SlavePens()
	{
		await StrictlyWpAreaHandler(World.Act5.SlavePens, "Use QuestBot to enter Act 5 first");
	}

	private static async Task OverseerTower()
	{
		await WpAreaHandler(World.Act5.OverseerTower, tgtPosition_54, World.Act5.SlavePens, SlavePens);
	}

	private static async Task ControlBlocks()
	{
		await TownConnectedAreaHandler(World.Act5.ControlBlocks, walkablePosition_8, World.Act5.OverseerTower, OverseerTower);
	}

	private static async Task OriathSquare()
	{
		await WpAreaHandler(World.Act5.OriathSquare, tgtPosition_55, World.Act5.ControlBlocks, ControlBlocks);
	}

	private static async Task TemplarCourts()
	{
		await WpAreaHandler(World.Act5.TemplarCourts, tgtPosition_56, World.Act5.OriathSquare, OriathSquare);
	}

	private static async Task ChamberOfInnocence()
	{
		await WpAreaHandler(World.Act5.ChamberOfInnocence, tgtPosition_57, World.Act5.TemplarCourts, TemplarCourts);
	}

	private static async Task TorchedCourts()
	{
		await NoWpAreaHandler(World.Act5.TorchedCourts, tgtPosition_58, World.Act5.ChamberOfInnocence, ChamberOfInnocence);
	}

	private static async Task RuinedSquare()
	{
		await WpAreaHandler(World.Act5.RuinedSquare, tgtPosition_59, World.Act5.TorchedCourts, TorchedCourts);
	}

	private static async Task Reliquary()
	{
		await WpAreaHandler(World.Act5.Reliquary, tgtPosition_60, World.Act5.RuinedSquare, RuinedSquare);
	}

	private static async Task Ossuary()
	{
		await NoWpAreaHandler(World.Act5.Ossuary, tgtPosition_61, World.Act5.RuinedSquare, RuinedSquare);
	}

	private static async Task CathedralRooftop()
	{
		await WpAreaHandler(World.Act5.CathedralRooftop, tgtPosition_62, World.Act5.RuinedSquare, RuinedSquare);
	}

	private static async Task LioneyeWatch_A6()
	{
		await StrictlyWpAreaHandler(World.Act6.LioneyeWatch, "Use QuestBot to enter Act 6 first");
	}

	private static async Task TwilightStrand_A6()
	{
		await TownConnectedAreaHandler(World.Act6.TwilightStrand, walkablePosition_9, World.Act6.LioneyeWatch, LioneyeWatch_A6);
	}

	private static async Task Coast_A6()
	{
		await TownConnectedAreaHandler(World.Act6.Coast, walkablePosition_10, World.Act6.LioneyeWatch, LioneyeWatch_A6);
	}

	private static async Task TidalIsland_A6()
	{
		await NoWpAreaHandler(World.Act6.TidalIsland, tgtPosition_63, World.Act6.Coast, Coast_A6);
	}

	private static async Task MudFlats_A6()
	{
		await NoWpAreaHandler(World.Act6.MudFlats, tgtPosition_64, World.Act6.Coast, Coast_A6);
	}

	private static async Task KaruiFortress()
	{
		await NoWpAreaHandler(World.Act6.KaruiFortress, tgtPosition_65, World.Act6.MudFlats, MudFlats_A6);
	}

	private static async Task Ridge()
	{
		await WpAreaHandler(World.Act6.Ridge, tgtPosition_66, World.Act6.KaruiFortress, KaruiFortress);
	}

	private static async Task LowerPrison_A6()
	{
		await WpAreaHandler(World.Act6.LowerPrison, tgtPosition_67, World.Act6.Ridge, Ridge);
	}

	private static async Task ShavronneTower()
	{
		await NoWpAreaHandler(World.Act6.ShavronneTower, tgtPosition_68, World.Act6.LowerPrison, LowerPrison_A6, delegate
		{
			tgtPosition_69.ResetCurrentPosition();
		});
	}

	private static async Task PrisonersGate_A6()
	{
		await ThroughMultilevelAreaHander(World.Act6.PrisonerGate, tgtPosition_69, World.Act6.ShavronneTower, ShavronneTower);
	}

	private static async Task WesternForest_A6()
	{
		await WpAreaHandler(World.Act6.WesternForest, tgtPosition_70, World.Act6.PrisonerGate, PrisonersGate_A6);
	}

	private static async Task Riverways_A6()
	{
		await WpAreaHandler(World.Act6.Riverways, tgtPosition_71, World.Act6.WesternForest, WesternForest_A6);
	}

	private static async Task Wetlands_A6()
	{
		await NoWpAreaHandler(World.Act6.Wetlands, keicOaiOub, World.Act6.Riverways, Riverways_A6);
	}

	private static async Task SouthernForest_A6()
	{
		await WpAreaHandler(World.Act6.SouthernForest, tgtPosition_72, World.Act6.Riverways, Riverways_A6);
	}

	private static async Task CavernOfAnger_A6()
	{
		await NoWpAreaHandler(World.Act6.CavernOfAnger, tgtPosition_73, World.Act6.SouthernForest, SouthernForest_A6);
	}

	private static async Task Beacon()
	{
		if (World.Act6.Beacon.IsCurrentArea)
		{
			OuterLogicError(World.Act6.Beacon);
		}
		else if (World.Act6.CavernOfAnger.IsCurrentArea)
		{
			Chest flagChest = ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2578 }).FirstOrDefault<Chest>();
			if ((NetworkObject)(object)flagChest != (NetworkObject)null && ((NetworkObject)(object)flagChest).PathExists())
			{
				AreaTransition roomExit = ObjectManager.Objects.FirstOrDefault((AreaTransition a) => ((NetworkObject)a).Metadata == "Metadata/QuestObjects/BossArenaEntranceTransition");
				if (!(await PlayerAction.TakeTransition(roomExit)))
				{
					ErrorManager.ReportError();
				}
			}
			else
			{
				await MoveAndEnter(World.Act6.Beacon, tgtPosition_74, null);
			}
		}
		else if (World.Act6.Beacon.IsWaypointOpened)
		{
			if (AnyWaypointNearby)
			{
				await TakeWaypoint(World.Act6.Beacon, null);
			}
			else
			{
				await TpToTown();
			}
		}
		else
		{
			await CavernOfAnger_A6();
		}
	}

	private static async Task BrineKingReef()
	{
		await StrictlyWpAreaHandler(World.Act6.BrineKingReef, "Use QuestBot to traverse The Beacon area.");
	}

	private static async Task BridgeEncampment()
	{
		await StrictlyWpAreaHandler(World.Act7.BridgeEncampment, "Use QuestBot to enter Act 7 first");
	}

	private static async Task BrokenBridge_A7()
	{
		await TownConnectedAreaHandler(World.Act7.BrokenBridge, walkablePosition_11, World.Act7.BridgeEncampment, BridgeEncampment);
	}

	private static async Task Crossroads_A7()
	{
		await WpAreaHandler(World.Act7.Crossroads, tgtPosition_75, World.Act7.BrokenBridge, BrokenBridge_A7);
	}

	private static async Task FellshrineRuins_A7()
	{
		await NoWpAreaHandler(World.Act7.FellshrineRuins, tgtPosition_76, World.Act7.Crossroads, Crossroads_A7);
	}

	private static async Task Crypt_A7()
	{
		await WpAreaHandler(World.Act7.Crypt, tgtPosition_77, World.Act7.FellshrineRuins, FellshrineRuins_A7);
	}

	private static async Task ChamberOfSins1_A7()
	{
		await WpAreaHandler(World.Act7.ChamberOfSins1, tgtPosition_78, World.Act7.Crossroads, Crossroads_A7);
	}

	private static async Task ChamberOfSins2_A7()
	{
		await NoWpAreaHandler(World.Act7.ChamberOfSins2, tgtPosition_79, World.Act7.ChamberOfSins1, ChamberOfSins1_A7);
	}

	private static async Task Den_A7()
	{
		await WpAreaHandler(World.Act7.Den, tgtPosition_80, World.Act7.ChamberOfSins2, ChamberOfSins2_A7);
	}

	private static async Task AshenFields()
	{
		await WpAreaHandler(World.Act7.AshenFields, tgtPosition_81, World.Act7.Den, Den_A7, delegate
		{
			tgtPosition_82.ResetCurrentPosition();
		});
	}

	private static async Task NorthernForest_A7()
	{
		await ThroughMultilevelAreaHander(World.Act7.NorthernForest, tgtPosition_82, World.Act7.AshenFields, AshenFields);
	}

	private static async Task DreadThicket_A7()
	{
		await NoWpAreaHandler(World.Act7.DreadThicket, tgtPosition_83, World.Act7.NorthernForest, NorthernForest_A7);
	}

	private static async Task Causeway()
	{
		await WpAreaHandler(World.Act7.Causeway, tgtPosition_84, World.Act7.NorthernForest, NorthernForest_A7);
	}

	private static async Task VaalCity()
	{
		await WpAreaHandler(World.Act7.VaalCity, tgtPosition_85, World.Act7.Causeway, Causeway);
	}

	private static async Task TempleOfDecay1()
	{
		if (!World.Act7.TempleOfDecay1.IsCurrentArea)
		{
			if (!World.Act7.VaalCity.IsCurrentArea)
			{
				await VaalCity();
				return;
			}
			if (Inventories.InventoryItems.Count((Item i) => i.Class == "QuestItem" && i.Metadata.ContainsIgnorecase("Act7/Firefly")) == 7)
			{
				NetworkObject yeena = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/NPC/Act7/YeenaVaalCity");
				if (yeena != (NetworkObject)null && yeena.IsTargetable)
				{
					WalkablePosition pos = yeena.WalkablePosition();
					if (pos.IsFar)
					{
						pos.Come();
					}
					else if (await PlayerAction.Interact(yeena))
					{
						await Coroutines.CloseBlockingWindows();
						await Wait.SleepSafe(1000);
					}
					else
					{
						ErrorManager.ReportError();
					}
					return;
				}
			}
			await MoveAndEnter(World.Act7.TempleOfDecay1, tgtPosition_86, delegate
			{
				tgtPosition_87.ResetCurrentPosition();
			});
		}
		else
		{
			OuterLogicError(World.Act7.TempleOfDecay1);
		}
	}

	private static async Task TempleOfDecay2()
	{
		await ThroughMultilevelAreaHander(World.Act7.TempleOfDecay2, tgtPosition_87, World.Act7.TempleOfDecay1, TempleOfDecay1, delegate
		{
			tgtPosition_88.ResetCurrentPosition();
		});
	}

	private static async Task SarnRamparts()
	{
		await ThroughMultilevelAreaHander(World.Act8.SarnRamparts, tgtPosition_88, World.Act7.TempleOfDecay2, TempleOfDecay2, delegate
		{
			tgtPosition_89.ResetCurrentPosition();
		});
	}

	private static async Task SarnEncampment_A8()
	{
		await ThroughMultilevelAreaHander(World.Act8.SarnEncampment, tgtPosition_89, World.Act8.SarnRamparts, SarnRamparts);
	}

	private static async Task ToxicConduits()
	{
		await TownConnectedAreaHandler(World.Act8.ToxicConduits, walkablePosition_12, World.Act8.SarnEncampment, SarnEncampment_A8);
	}

	private static async Task DoedreCesspool()
	{
		await WpAreaHandler(World.Act8.DoedreCesspool, tgtPosition_90, World.Act8.ToxicConduits, ToxicConduits, delegate
		{
			tgtPosition_91.ResetCurrentPosition();
			tgtPosition_92.ResetCurrentPosition();
		});
	}

	private static async Task GrandPromenade()
	{
		await ThroughMultilevelAreaHander(World.Act8.GrandPromenade, tgtPosition_91, World.Act8.DoedreCesspool, DoedreCesspool);
	}

	private static async Task Quay()
	{
		await ThroughMultilevelAreaHander(World.Act8.Quay, tgtPosition_92, World.Act8.DoedreCesspool, DoedreCesspool);
	}

	private static async Task GrainGate()
	{
		await WpAreaHandler(World.Act8.GrainGate, tgtPosition_93, World.Act8.Quay, Quay);
	}

	private static async Task ImperialFields()
	{
		await WpAreaHandler(World.Act8.ImperialFields, tgtPosition_94, World.Act8.GrainGate, GrainGate);
	}

	private static async Task Solaris1_A8()
	{
		await WpAreaHandler(World.Act8.SolarisTemple1, tgtPosition_95, World.Act8.ImperialFields, ImperialFields);
	}

	private static async Task Solaris2_A8()
	{
		await NoWpAreaHandler(World.Act8.SolarisTemple2, tgtPosition_96, World.Act8.SolarisTemple1, Solaris1_A8);
	}

	private static async Task SolarisConcourse()
	{
		await WpAreaHandler(World.Act8.SolarisConcourse, tgtPosition_97, World.Act8.SolarisTemple1, Solaris1_A8);
	}

	private static async Task BathHouse()
	{
		await WpAreaHandler(World.Act8.BathHouse, tgtPosition_98, World.Act8.GrandPromenade, GrandPromenade);
	}

	private static async Task HighGardens()
	{
		await NoWpAreaHandler(World.Act8.HighGardens, tgtPosition_99, World.Act8.BathHouse, BathHouse);
	}

	private static async Task LunarisConcourse()
	{
		await WpAreaHandler(World.Act8.LunarisConcourse, tgtPosition_100, World.Act8.BathHouse, BathHouse);
	}

	private static async Task Lunaris1_A8()
	{
		await WpAreaHandler(World.Act8.LunarisTemple1, tgtPosition_101, World.Act8.LunarisConcourse, LunarisConcourse);
	}

	private static async Task Lunaris2_A8()
	{
		await NoWpAreaHandler(World.Act8.LunarisTemple2, tgtPosition_102, World.Act8.LunarisTemple1, Lunaris1_A8);
	}

	private static async Task HarbourBridge()
	{
		await NoWpAreaHandler(World.Act8.HarbourBridge, tgtPosition_103, World.Act8.LunarisConcourse, LunarisConcourse, delegate
		{
			tgtPosition_104.ResetCurrentPosition();
		});
	}

	private static async Task BloodAqueduct()
	{
		await ThroughMultilevelAreaHander(World.Act9.BloodAqueduct, tgtPosition_104, World.Act8.HarbourBridge, HarbourBridge);
	}

	private static async Task Highgate_A9()
	{
		await WpAreaHandler(World.Act9.Highgate, tgtPosition_105, World.Act9.BloodAqueduct, BloodAqueduct);
	}

	private static async Task Descent()
	{
		await TownConnectedAreaHandler(World.Act9.Descent, walkablePosition_13, World.Act9.Highgate, Highgate_A9, delegate
		{
			tgtPosition_106.ResetCurrentPosition();
		});
	}

	private static async Task VastiriDesert()
	{
		await ThroughMultilevelAreaHander(World.Act9.VastiriDesert, tgtPosition_106, World.Act9.Descent, Descent);
	}

	private static async Task Oasis()
	{
		await NoWpAreaHandler(World.Act9.Oasis, tgtPosition_107, World.Act9.VastiriDesert, VastiriDesert);
	}

	private static async Task Foothills()
	{
		await WpAreaHandler(World.Act9.Foothills, tgtPosition_108, World.Act9.VastiriDesert, VastiriDesert);
	}

	private static async Task BoilingLake()
	{
		await NoWpAreaHandler(World.Act9.BoilingLake, tgtPosition_109, World.Act9.Foothills, Foothills);
	}

	private static async Task Tunnel()
	{
		await WpAreaHandler(World.Act9.Tunnel, tgtPosition_110, World.Act9.Foothills, Foothills);
	}

	private static async Task Quarry()
	{
		await WpAreaHandler(World.Act9.Quarry, tgtPosition_111, World.Act9.Tunnel, Tunnel);
	}

	private static async Task Refinery()
	{
		await NoWpAreaHandler(World.Act9.Refinery, tgtPosition_112, World.Act9.Quarry, Quarry);
	}

	private static async Task Belly_A9()
	{
		if (!World.Act9.BellyOfBeast.IsCurrentArea)
		{
			if (World.Act9.Quarry.IsCurrentArea)
			{
				NetworkObject sin = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/NPC/Act9/SinQuarry");
				if (sin != (NetworkObject)null && sin.HasNpcFloatingIcon)
				{
					WalkablePosition pos = sin.WalkablePosition();
					if (pos.IsFar)
					{
						pos.Come();
					}
					else if (await PlayerAction.Interact(sin))
					{
						await Coroutines.CloseBlockingWindows();
						await Wait.SleepSafe(1000);
					}
					else
					{
						ErrorManager.ReportError();
					}
				}
				else
				{
					await MoveAndEnter(World.Act9.BellyOfBeast, tgtPosition_113, null);
				}
			}
			else
			{
				await Quarry();
			}
		}
		else
		{
			OuterLogicError(World.Act9.BellyOfBeast);
		}
	}

	private static async Task RottingCore()
	{
		await NoWpAreaHandler(World.Act9.RottingCore, tgtPosition_114, World.Act9.BellyOfBeast, Belly_A9);
	}

	private static async Task OriathDocks()
	{
		await StrictlyWpAreaHandler(World.Act10.OriathDocks, "Use QuestBot to enter Act 10 first");
	}

	private static async Task CathedralRooftop_A10()
	{
		await TownConnectedAreaHandler(World.Act10.CathedralRooftop, walkablePosition_14, World.Act10.OriathDocks, OriathDocks);
	}

	private static async Task RavagedSquare()
	{
		await WpAreaHandler(World.Act10.RavagedSquare, tgtPosition_115, World.Act10.CathedralRooftop, CathedralRooftop_A10);
	}

	private static async Task Ossuary_A10()
	{
		await NoWpAreaHandler(World.Act10.Ossuary, tgtPosition_116, World.Act10.RavagedSquare, RavagedSquare);
	}

	private static async Task TorchedCourts_A10()
	{
		await NoWpAreaHandler(World.Act10.TorchedCourts, tgtPosition_117, World.Act10.RavagedSquare, RavagedSquare);
	}

	private static async Task Reliquary_A10()
	{
		await WpAreaHandler(World.Act10.Reliquary, tgtPosition_118, World.Act10.RavagedSquare, RavagedSquare);
	}

	private static async Task ControlBlocks_A10()
	{
		await WpAreaHandler(World.Act10.ControlBlocks, tgtPosition_119, World.Act10.RavagedSquare, RavagedSquare);
	}

	private static async Task DesecratedChambers()
	{
		await WpAreaHandler(World.Act10.DesecratedChambers, tgtPosition_120, World.Act10.TorchedCourts, TorchedCourts_A10);
	}

	private static async Task Canals()
	{
		if (!World.Act10.Canals.IsCurrentArea)
		{
			if (World.Act10.RavagedSquare.IsCurrentArea)
			{
				NetworkObject innocence = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/NPC/Act10/InnocenceSquare");
				if (innocence != (NetworkObject)null && innocence.HasNpcFloatingIcon)
				{
					WalkablePosition pos = innocence.WalkablePosition();
					if (pos.IsFar)
					{
						pos.Come();
						return;
					}
					if (!(await PlayerAction.Interact(innocence)))
					{
						ErrorManager.ReportError();
						return;
					}
					await Coroutines.CloseBlockingWindows();
					await Wait.SleepSafe(1000);
				}
				else
				{
					await MoveAndEnter(World.Act10.Canals, tgtPosition_121, null);
				}
			}
			else
			{
				await RavagedSquare();
			}
		}
		else
		{
			OuterLogicError(World.Act10.Canals);
		}
	}

	private static async Task FeedingTrough()
	{
		await NoWpAreaHandler(World.Act10.FeedingTrough, tgtPosition_122, World.Act10.Canals, Canals);
	}

	private static async Task Oriath()
	{
		await StrictlyWpAreaHandler(World.Act11.Oriath, "Use QuestBot to enter Act 11 first");
	}

	private static async Task KaruiShores()
	{
		await StrictlyWpAreaHandler(World.Act11.KaruiShores, "Use QuestBot to enter Endgame Act 11 first");
	}

	private static async Task TemplarLaboratory()
	{
		await TownConnectedAreaHandler(World.Act11.TemplarLaboratory, walkablePosition_15, World.Act11.Oriath, Oriath);
	}

	private static async Task FallenCourts()
	{
		await TownConnectedAreaHandler(World.Act11.FallenCourts, walkablePosition_16, World.Act11.Oriath, Oriath);
	}

	private static async Task HauntedReliquary()
	{
		await TownConnectedAreaHandler(World.Act11.HauntedReliquary, walkablePosition_17, World.Act11.Oriath, Oriath);
	}

	private static async Task WpAreaHandler(AreaInfo area, TgtPosition tgtPos, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
	{
		if (!area.IsCurrentArea)
		{
			if (!prevArea.IsCurrentArea)
			{
				if (area.IsWaypointOpened)
				{
					if (!AnyWaypointNearby)
					{
						await TpToTown();
					}
					else
					{
						await TakeWaypoint(area, postEnter);
					}
				}
				else
				{
					await prevAreaHandler();
				}
			}
			else
			{
				await MoveAndEnter(area, tgtPos, postEnter);
			}
		}
		else
		{
			OuterLogicError(area);
		}
	}

	private static async Task NoWpAreaHandler(AreaInfo area, TgtPosition tgtPos, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
	{
		if (!area.IsCurrentArea)
		{
			if (!prevArea.IsCurrentArea)
			{
				await prevAreaHandler();
			}
			else
			{
				await MoveAndEnter(area, tgtPos, postEnter);
			}
		}
		else
		{
			OuterLogicError(area);
		}
	}

	private static async Task ThroughMultilevelAreaHander(AreaInfo area, TgtPosition nextLevelTgt, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
	{
		if (!area.IsCurrentArea)
		{
			if (!area.IsWaypointOpened)
			{
				if (prevArea.IsCurrentArea)
				{
					await MoveAndEnterMultilevel(area, nextLevelTgt, postEnter);
				}
				else
				{
					await prevAreaHandler();
				}
			}
			else if (!AnyWaypointNearby)
			{
				await TpToTown();
			}
			else
			{
				await TakeWaypoint(area, postEnter);
			}
		}
		else
		{
			OuterLogicError(area);
		}
	}

	private static async Task TownConnectedAreaHandler(AreaInfo area, WalkablePosition transitionPos, AreaInfo town, Func<Task> townHandler, Action postEnter = null)
	{
		if (!area.IsCurrentArea)
		{
			if (area.IsWaypointOpened)
			{
				if (!AnyWaypointNearby)
				{
					await TpToTown();
				}
				else
				{
					await TakeWaypoint(area, postEnter);
				}
			}
			else if (!town.IsCurrentArea)
			{
				await townHandler();
			}
			else
			{
				await transitionPos.ComeAtOnce();
				await TakeTransition(area, postEnter);
			}
		}
		else
		{
			OuterLogicError(area);
		}
	}

	private static async Task StrictlyWpAreaHandler(AreaInfo area, string hint, Action postEnter = null)
	{
		if (area.IsWaypointOpened)
		{
			if (!AnyWaypointNearby)
			{
				await TpToTown();
			}
			else
			{
				await TakeWaypoint(area, postEnter);
			}
			return;
		}
		GlobalLog.Error("[Travel] " + area.Name + " waypoint is not available. " + hint + ".");
		ErrorManager.ReportCriticalError();
	}

	private static async Task MoveAndEnter(AreaInfo area, TgtPosition tgtPos, Action postEnter)
	{
		WalkablePosition pos = GetCachedTransitionPos(area);
		if (pos != null)
		{
			if (pos.Distance <= 50 && !(pos.PathDistance > 50f))
			{
				await TakeTransition(area, tgtPos, postEnter);
			}
			else
			{
				pos.TryCome();
			}
		}
		else if (!tgtPos.IsFar)
		{
			await TakeTransition(area, tgtPos, postEnter);
		}
		else
		{
			tgtPos.TryCome();
		}
	}

	private static async Task MoveAndEnterMultilevel(AreaInfo area, TgtPosition tgtPos, Action postEnter)
	{
		if (!tgtPos.IsFar)
		{
			AreaTransition transition = await GetTransitionObject(tgtPos, null);
			if ((NetworkObject)(object)transition == (NetworkObject)null)
			{
				return;
			}
			bool isDestination = transition.LeadsTo(area);
			bool newInstance = isDestination && hashSet_0.Contains(area);
			if (await PlayerAction.TakeTransition(transition, newInstance))
			{
				if (isDestination)
				{
					if (newInstance)
					{
						hashSet_0.Remove(area);
					}
					postEnter?.Invoke();
				}
				else
				{
					tgtPos.ResetCurrentPosition();
				}
			}
			else
			{
				ErrorManager.ReportError();
			}
		}
		else
		{
			tgtPos.Come();
		}
	}

	private static async Task TakeWaypoint(AreaInfo area, Action postEnter)
	{
		bool newInstance = hashSet_0.Contains(area);
		if (!(await PlayerAction.TakeWaypoint(area, newInstance)))
		{
			ErrorManager.ReportError();
			return;
		}
		if (newInstance)
		{
			hashSet_0.Remove(area);
		}
		postEnter?.Invoke();
	}

	private static async Task TakeTransition(AreaInfo area, Action postEnter)
	{
		AreaTransition transition = ObjectManager.Objects.FirstOrDefault((AreaTransition a) => a.LeadsTo(area));
		if (!((NetworkObject)(object)transition == (NetworkObject)null))
		{
			bool newInstance = hashSet_0.Contains(area);
			if (await PlayerAction.TakeTransition(transition, newInstance))
			{
				if (newInstance)
				{
					hashSet_0.Remove(area);
				}
				postEnter?.Invoke();
			}
			else
			{
				ErrorManager.ReportError();
			}
		}
		else
		{
			GlobalLog.Error($"[Travel] There is no transition that leads to {area}");
			ErrorManager.ReportError();
		}
	}

	private static async Task TakeTransition(AreaInfo area, TgtPosition tgtPos, Action postEnter)
	{
		AreaTransition transition = await GetTransitionObject(tgtPos, area);
		if ((NetworkObject)(object)transition == (NetworkObject)null)
		{
			return;
		}
		bool newInstance = hashSet_0.Contains(area);
		if (!(await PlayerAction.TakeTransition(transition, newInstance)))
		{
			ErrorManager.ReportError();
			return;
		}
		if (newInstance)
		{
			hashSet_0.Remove(area);
		}
		postEnter?.Invoke();
	}

	private static async Task<AreaTransition> GetTransitionObject(TgtPosition tgtPos, AreaInfo area)
	{
		AreaTransition transition = (from t in ObjectManager.GetObjectsByType<AreaTransition>()
			orderby area != null && (((NetworkObject)t).Name.Contains(area.Name) || (t.Destination != null && t.Destination == area)) descending
			select t).FirstOrDefault();
		if (!((NetworkObject)(object)transition == (NetworkObject)null))
		{
			if ((int)transition.TransitionType != 2)
			{
				if (((NetworkObject)transition).IsTargetable)
				{
					if (area != null)
					{
						DatWorldAreaWrapper dest = transition.Destination;
						if (area != dest)
						{
							GlobalLog.Warn("[Travel] Transition leads to \"" + dest.Name + "\". Expected: \"" + area.Name + "\".");
							tgtPos.ProceedToNext();
							return null;
						}
					}
					return transition;
				}
				GlobalLog.Debug((!(area == null)) ? ("[Travel] Waiting for \"" + area.Name + "\" transition activation.") : "[Travel] Waiting for transition activation.");
				await Wait.StuckDetectionSleep(200);
				return null;
			}
			GlobalLog.Warn("[Travel] Corrupted area entrance has the same tgt as our destination.");
			tgtPos.ProceedToNext();
			return null;
		}
		if (area == World.Act5.OverseerTower)
		{
			NetworkObject ladder = ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Terrain/Act5/Area1/Objects/ProximitySpearLadderOnce");
			if (ladder != (NetworkObject)null)
			{
				GlobalLog.Debug("[Travel] Ladder detected");
				if (!(ladder.Distance > 10f) || !(ladder.Distance < 90f))
				{
					if (ladder.Distance > 20f)
					{
						PlayerMoverManager.MoveTowards(ladder.Position, (object)null);
					}
				}
				else
				{
					await new WalkablePosition("Ladder activation", ladder.Position).TryComeAtOnce(10);
				}
			}
			GlobalLog.Debug("[Travel] Waiting for \"" + area.Name + "\" transition activation.");
			await Wait.StuckDetectionSleep(200);
			return null;
		}
		GlobalLog.Warn("[Travel] There is no area transition near tgt position.");
		tgtPos.ProceedToNext();
		return null;
	}

	private static async Task TpToTown()
	{
		if (!(await PlayerAction.TpToTown()))
		{
			ErrorManager.ReportError();
		}
	}

	private static WalkablePosition GetCachedTransitionPos(AreaInfo area)
	{
		return CombatAreaCache.Current.AreaTransitions.Find((CachedTransition t) => t.Destination == area)?.Position;
	}

	private static void OuterLogicError(AreaInfo area)
	{
		GlobalLog.Error($"[Travel] Outer logic error. Travel to {area} has been called, but we are already here.");
		ErrorManager.ReportError();
	}

	static Travel()
	{
		hashSet_0 = new HashSet<AreaInfo>();
		dictionary_0 = new Dictionary<AreaInfo, Func<Task>>
		{
			[World.Act1.LioneyeWatch] = LioneyeWatch,
			[World.Act1.Coast] = Coast,
			[World.Act1.TidalIsland] = TidalIsland,
			[World.Act1.MudFlats] = MudFlats,
			[World.Act1.FetidPool] = FetidPool,
			[World.Act1.SubmergedPassage] = SubmergedPassage,
			[World.Act1.FloodedDepths] = FloodedDepths,
			[World.Act1.Ledge] = Ledge,
			[World.Act1.Climb] = Climb,
			[World.Act1.LowerPrison] = LowerPrison,
			[World.Act1.UpperPrison] = UpperPrison,
			[World.Act1.PrisonerGate] = PrisonersGate,
			[World.Act1.ShipGraveyard] = ShipGraveyard,
			[World.Act1.ShipGraveyardCave] = ShipGraveyardCave,
			[World.Act1.CavernOfWrath] = CavernOfWrath,
			[World.Act1.CavernOfAnger] = CavernOfAnger,
			[World.Act2.SouthernForest] = SouthernForest,
			[World.Act2.ForestEncampment] = ForestEncampment,
			[World.Act2.Riverways] = Riverways,
			[World.Act2.WesternForest] = WesternForest,
			[World.Act2.WeaverChambers] = WeaverChambers,
			[World.Act2.OldFields] = OldFields,
			[World.Act2.Den] = Den,
			[World.Act2.Crossroads] = Crossroads,
			[World.Act2.ChamberOfSins1] = ChamberOfSins1,
			[World.Act2.ChamberOfSins2] = ChamberOfSins2,
			[World.Act2.BrokenBridge] = BrokenBridge,
			[World.Act2.FellshrineRuins] = FellshrineRuins,
			[World.Act2.Crypt1] = Crypt1,
			[World.Act2.Crypt2] = Crypt2,
			[World.Act2.Wetlands] = Wetlands,
			[World.Act2.VaalRuins] = VaalRuins,
			[World.Act2.NorthernForest] = NorthernForest,
			[World.Act2.DreadThicket] = DreadThicket,
			[World.Act2.Caverns] = Caverns,
			[World.Act2.AncientPyramid] = AncientPyramid,
			[World.Act3.CityOfSarn] = CityOfSarn,
			[World.Act3.SarnEncampment] = SarnEncampment,
			[World.Act3.Slums] = Slums,
			[World.Act3.Crematorium] = Crematorium,
			[World.Act3.Sewers] = Sewers,
			[World.Act3.Marketplace] = Marketplace,
			[World.Act3.Catacombs] = Catacombs,
			[World.Act3.Battlefront] = Battlefront,
			[World.Act3.Docks] = Docks,
			[World.Act3.SolarisTemple1] = Solaris1,
			[World.Act3.SolarisTemple2] = Solaris2,
			[World.Act3.EbonyBarracks] = EbonyBarracks,
			[World.Act3.LunarisTemple1] = Lunaris1,
			[World.Act3.LunarisTemple2] = Lunaris2,
			[World.Act3.ImperialGardens] = ImperialGardens,
			[World.Act3.Library] = Library,
			[World.Act3.Archives] = Archives,
			[World.Act3.SceptreOfGod] = SceptreOfGod,
			[World.Act3.UpperSceptreOfGod] = UpperSceptreOfGod,
			[World.Act4.Aqueduct] = Aqueduct,
			[World.Act4.Highgate] = Highgate,
			[World.Act4.DriedLake] = DriedLake,
			[World.Act4.Mines1] = Mines1,
			[World.Act4.Mines2] = Mines2,
			[World.Act4.CrystalVeins] = CrystalVeins,
			[World.Act4.KaomDream] = KaomDream,
			[World.Act4.KaomStronghold] = KaomStronghold,
			[World.Act4.DaressoDream] = DaressoDream,
			[World.Act4.GrandArena] = GrandArena,
			[World.Act4.BellyOfBeast1] = Belly1,
			[World.Act4.BellyOfBeast2] = Belly2,
			[World.Act4.Harvest] = Harvest,
			[World.Act4.Ascent] = Ascent,
			[World.Act5.SlavePens] = SlavePens,
			[World.Act5.OverseerTower] = OverseerTower,
			[World.Act5.ControlBlocks] = ControlBlocks,
			[World.Act5.OriathSquare] = OriathSquare,
			[World.Act5.TemplarCourts] = TemplarCourts,
			[World.Act5.ChamberOfInnocence] = ChamberOfInnocence,
			[World.Act5.TorchedCourts] = TorchedCourts,
			[World.Act5.RuinedSquare] = RuinedSquare,
			[World.Act5.Reliquary] = Reliquary,
			[World.Act5.Ossuary] = Ossuary,
			[World.Act5.CathedralRooftop] = CathedralRooftop,
			[World.Act6.LioneyeWatch] = LioneyeWatch_A6,
			[World.Act6.TwilightStrand] = TwilightStrand_A6,
			[World.Act6.Coast] = Coast_A6,
			[World.Act6.TidalIsland] = TidalIsland_A6,
			[World.Act6.MudFlats] = MudFlats_A6,
			[World.Act6.KaruiFortress] = KaruiFortress,
			[World.Act6.Ridge] = Ridge,
			[World.Act6.LowerPrison] = LowerPrison_A6,
			[World.Act6.ShavronneTower] = ShavronneTower,
			[World.Act6.PrisonerGate] = PrisonersGate_A6,
			[World.Act6.WesternForest] = WesternForest_A6,
			[World.Act6.Riverways] = Riverways_A6,
			[World.Act6.Wetlands] = Wetlands_A6,
			[World.Act6.SouthernForest] = SouthernForest_A6,
			[World.Act6.CavernOfAnger] = CavernOfAnger_A6,
			[World.Act6.Beacon] = Beacon,
			[World.Act6.BrineKingReef] = BrineKingReef,
			[World.Act7.BridgeEncampment] = BridgeEncampment,
			[World.Act7.BrokenBridge] = BrokenBridge_A7,
			[World.Act7.Crossroads] = Crossroads_A7,
			[World.Act7.FellshrineRuins] = FellshrineRuins_A7,
			[World.Act7.Crypt] = Crypt_A7,
			[World.Act7.ChamberOfSins1] = ChamberOfSins1_A7,
			[World.Act7.ChamberOfSins2] = ChamberOfSins2_A7,
			[World.Act7.Den] = Den_A7,
			[World.Act7.AshenFields] = AshenFields,
			[World.Act7.NorthernForest] = NorthernForest_A7,
			[World.Act7.DreadThicket] = DreadThicket_A7,
			[World.Act7.Causeway] = Causeway,
			[World.Act7.VaalCity] = VaalCity,
			[World.Act7.TempleOfDecay1] = TempleOfDecay1,
			[World.Act7.TempleOfDecay2] = TempleOfDecay2,
			[World.Act8.SarnRamparts] = SarnRamparts,
			[World.Act8.SarnEncampment] = SarnEncampment_A8,
			[World.Act8.ToxicConduits] = ToxicConduits,
			[World.Act8.DoedreCesspool] = DoedreCesspool,
			[World.Act8.GrandPromenade] = GrandPromenade,
			[World.Act8.Quay] = Quay,
			[World.Act8.GrainGate] = GrainGate,
			[World.Act8.ImperialFields] = ImperialFields,
			[World.Act8.SolarisTemple1] = Solaris1_A8,
			[World.Act8.SolarisTemple2] = Solaris2_A8,
			[World.Act8.SolarisConcourse] = SolarisConcourse,
			[World.Act8.BathHouse] = BathHouse,
			[World.Act8.HighGardens] = HighGardens,
			[World.Act8.LunarisConcourse] = LunarisConcourse,
			[World.Act8.LunarisTemple1] = Lunaris1_A8,
			[World.Act8.LunarisTemple2] = Lunaris2_A8,
			[World.Act8.HarbourBridge] = HarbourBridge,
			[World.Act9.BloodAqueduct] = BloodAqueduct,
			[World.Act9.Highgate] = Highgate_A9,
			[World.Act9.Descent] = Descent,
			[World.Act9.VastiriDesert] = VastiriDesert,
			[World.Act9.Oasis] = Oasis,
			[World.Act9.Foothills] = Foothills,
			[World.Act9.BoilingLake] = BoilingLake,
			[World.Act9.Tunnel] = Tunnel,
			[World.Act9.Quarry] = Quarry,
			[World.Act9.Refinery] = Refinery,
			[World.Act9.BellyOfBeast] = Belly_A9,
			[World.Act9.RottingCore] = RottingCore,
			[World.Act10.OriathDocks] = OriathDocks,
			[World.Act10.CathedralRooftop] = CathedralRooftop_A10,
			[World.Act10.RavagedSquare] = RavagedSquare,
			[World.Act10.Ossuary] = Ossuary_A10,
			[World.Act10.TorchedCourts] = TorchedCourts_A10,
			[World.Act10.Reliquary] = Reliquary_A10,
			[World.Act10.ControlBlocks] = ControlBlocks_A10,
			[World.Act10.DesecratedChambers] = DesecratedChambers,
			[World.Act10.Canals] = Canals,
			[World.Act10.FeedingTrough] = FeedingTrough,
			[World.Act11.Oriath] = Oriath,
			[World.Act11.KaruiShores] = KaruiShores,
			[World.Act11.TemplarLaboratory] = TemplarLaboratory,
			[World.Act11.FallenCourts] = FallenCourts,
			[World.Act11.HauntedReliquary] = HauntedReliquary
		};
		tgtPosition_0 = new TgtPosition(World.Act1.LioneyeWatch.Name, "beachtown_south_entrance.tgt");
		tgtPosition_1 = new TgtPosition(World.Act1.TidalIsland.Name, "act1_karui_coast_to_island_transition_v01_01.tgt");
		tgtPosition_2 = new TgtPosition(World.Act1.MudFlats.Name, "act1_area2_transition_v01_01.tgt");
		tgtPosition_3 = new TgtPosition(World.Act1.FetidPool.Name, "act1_beach_toswamp_fetid_v01_01.tgt");
		tgtPosition_4 = new TgtPosition(World.Act1.SubmergedPassage.Name, "Beach_to_watercave_v2.tgt");
		tgtPosition_5 = new TgtPosition(World.Act1.FloodedDepths.Name, "watery_depth_entrance_v01_01.tgt");
		smvajQroSX = new TgtPosition(World.Act1.Ledge.Name, "caveup_exit_v01_01.tgt");
		YeSaHvVqIk = new TgtPosition(World.Act1.Climb.Name, "beach_passageway_v01_01.tgt");
		tgtPosition_6 = new TgtPosition(World.Act1.LowerPrison.Name, "beach_prisonback.tgt");
		tgtPosition_7 = new TgtPosition(World.Act1.UpperPrison.Name, "dungeon_prison_exit_up_v01_01.tgt");
		tgtPosition_8 = new TgtPosition("Next prison level", "dungeon_prison_exit_up_v01_01.tgt | dungeon_prison_boss_exit_v01_02.tgt | dungeon_prison_door_up_v01_01.tgt", closest: true);
		tgtPosition_9 = new TgtPosition(World.Act1.ShipGraveyard.Name, "shipgraveyard_passageway_v01_01.tgt");
		tgtPosition_10 = new TgtPosition(World.Act1.ShipGraveyardCave.Name, "ship_entrance_v01_01.tgt");
		tgtPosition_11 = new TgtPosition(World.Act1.CavernOfWrath.Name, "beach_caveentranceskeleton_v01_01.tgt | beach_caveentranceskeleton_v01_02.tgt");
		tgtPosition_12 = new TgtPosition(World.Act1.CavernOfAnger.Name, "caveup_exit_v01_01.tgt");
		tgtPosition_13 = new TgtPosition(World.Act2.SouthernForest.Name, "caveup_exit_v01_01.tgt | merveil_exit_clean_v01_01.tgt", closest: true);
		tgtPosition_14 = new TgtPosition(World.Act2.ForestEncampment.Name, "forestcamp_dock_v01_01.tgt");
		tgtPosition_15 = new TgtPosition(World.Act2.WesternForest.Name, "roadtothickforest_entrance_v01_01.tgt");
		tgtPosition_16 = new TgtPosition(World.Act2.WeaverChambers.Name, "spidergrove_entrance_v01_01.tgt");
		tgtPosition_17 = new TgtPosition(World.Act2.Den.Name, "forestcave_entrance_hole_v01_01.tgt");
		tgtPosition_18 = new TgtPosition(World.Act2.Crossroads.Name, "wall_gate_v01_01.tgt");
		tgtPosition_19 = new TgtPosition(World.Act2.ChamberOfSins1.Name, "temple_entrance_v01_01.tgt");
		tgtPosition_20 = new TgtPosition(World.Act2.ChamberOfSins2.Name, "templeruinforest_exit_down_v01_01.tgt");
		tgtPosition_21 = new TgtPosition(World.Act2.BrokenBridge.Name, "bridgeconnection_v01_01.tgt");
		RusadDrrNl = new TgtPosition(World.Act2.Crypt1.Name, "church_dungeon_entrance_v01_01.tgt");
		tgtPosition_22 = new TgtPosition(World.Act2.Crypt2.Name, "dungeon_church_exit_down_v01_01.tgt");
		tgtPosition_23 = new TgtPosition(World.Act2.Wetlands.Name, "bridgeconnection_v01_01.tgt");
		tgtPosition_24 = new TgtPosition(World.Act2.VaalRuins.Name, "forest_caveentrance_inca_v01_01.tgt");
		tgtPosition_25 = new TgtPosition(World.Act2.NorthernForest.Name, "dungeon_inca_exit_v01_01.tgt");
		tgtPosition_26 = new TgtPosition(World.Act2.DreadThicket.Name, "grovewall_entrance_v01_01.tgt");
		tgtPosition_27 = new TgtPosition(World.Act2.Caverns.Name, "waterfall_cave_entrance_v01_01.tgt");
		tgtPosition_28 = new TgtPosition(World.Act2.AncientPyramid.Name, "dungeon_stairs_up_v01_01.tgt");
		tgtPosition_29 = new TgtPosition("Next pyramid level", "dungeon_stairs_up_v01_01.tgt | dungeon_huangdoor_v01_01.tgt", closest: true);
		tgtPosition_30 = new TgtPosition(World.Act3.SarnEncampment.Name, "act3_docks_to_town_lower_01_01.tgt");
		tgtPosition_31 = new TgtPosition(World.Act3.Crematorium.Name, "act3_prison_entrance_01_01.tgt");
		tgtPosition_32 = new TgtPosition(World.Act3.Sewers.Name, "slum_sewer_entrance_v02_01.tgt");
		tgtPosition_33 = new TgtPosition(World.Act3.Marketplace.Name, "sewerwall_exit_v01_01.tgt");
		EwLaByreiw = new TgtPosition(World.Act3.Catacombs.Name, "markettochurchdungeon_v01_01.tgt");
		tgtPosition_34 = new TgtPosition(World.Act3.Battlefront.Name, "market_to_battlefront_v01_01.tgt");
		tgtPosition_35 = new TgtPosition(World.Act3.Docks.Name, "battlefield_arch_v01_03.tgt");
		tgtPosition_36 = new TgtPosition(World.Act3.SolarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");
		tgtPosition_37 = new TgtPosition(World.Act3.SolarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");
		tgtPosition_38 = new TgtPosition(World.Act3.EbonyBarracks.Name, "sewerexit_v01_01.tgt");
		tgtPosition_39 = new TgtPosition(World.Act3.LunarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");
		tgtPosition_40 = new TgtPosition(World.Act3.LunarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");
		tgtPosition_41 = new TgtPosition(World.Act3.ImperialGardens.Name, "garden_arch_v01_01.tgt");
		tgtPosition_42 = new TgtPosition(World.Act3.Library.Name, "Library_LargeBuilding_entrance_v01_01.tgt");
		tgtPosition_43 = new TgtPosition(World.Act3.Archives.Name, "library_entrance_v02_01.tgt");
		tgtPosition_44 = new TgtPosition(World.Act3.SceptreOfGod.Name, "Act3_EpicDoor_v02_01.tgt");
		tgtPosition_45 = new TgtPosition("Next tower level", "tower_transition_up_01_01.tgt", closest: true);
		tgtPosition_46 = new TgtPosition("Next tower level", "tower_transition_up_01_01.tgt | tower_totowertop_v01_01.tgt | Act3_tower_01_01.tgt", closest: true, 10, 40);
		tgtPosition_47 = new TgtPosition(World.Act4.Highgate.Name, "mountiantown_connection.tgt");
		tgtPosition_48 = new TgtPosition(World.Act4.Mines2.Name, "mine_areatransition_v0?_0?.tgt");
		xiwazcMorg = new TgtPosition(World.Act4.CrystalVeins.Name, "mine_areatransition_v03_01.tgt");
		tgtPosition_49 = new TgtPosition("Rapture Device", "crystals_openAnimation_v01_01.tgt");
		tgtPosition_50 = new TgtPosition(World.Act4.KaomStronghold.Name, "lava_abyss_transition_entrance_v01_01.tgt", closest: false, 10, 50);
		tgtPosition_51 = new TgtPosition(World.Act4.GrandArena.Name, "arena_areatransition_v01_01.tgt");
		tgtPosition_52 = new TgtPosition(World.Act4.BellyOfBeast2.Name, "belly_tunnel_v01_01.tgt");
		tgtPosition_53 = new TgtPosition(World.Act4.Harvest.Name, "belly_tunnel_level2_v01_02.tgt");
		tgtPosition_54 = new TgtPosition(World.Act5.OverseerTower.Name, "tower_v01_01.tgt", closest: false, 10, 25);
		tgtPosition_55 = new TgtPosition(World.Act5.OriathSquare.Name, "security_exit_v01_01.tgt");
		tgtPosition_56 = new TgtPosition(World.Act5.TemplarCourts.Name, "Oriath_AreaTransition_v01_03.tgt");
		tgtPosition_57 = new TgtPosition(World.Act5.TemplarCourts.Name, "templar_to_innocents_v01_01.tgt");
		tgtPosition_58 = new TgtPosition(World.Act5.TorchedCourts.Name, "transition_chamber_to_courts_v01_01.tgt");
		tgtPosition_59 = new TgtPosition(World.Act5.RuinedSquare.Name, "templar_oriath_transition_v01_01.tgt");
		tgtPosition_60 = new TgtPosition(World.Act5.Reliquary.Name, "Oriath_AreaTransition_v01_02.tgt");
		tgtPosition_61 = new TgtPosition(World.Act5.Ossuary.Name, "Oriath_AreaTransition_v01_04.tgt");
		tgtPosition_62 = new TgtPosition(World.Act5.CathedralRooftop.Name, "chitus_statuewall_transition_v01_01.tgt");
		tgtPosition_63 = new TgtPosition(World.Act6.TidalIsland.Name, "karui_coast_to_island_transition_v01_01.tgt");
		tgtPosition_64 = new TgtPosition(World.Act6.MudFlats.Name, "act6_area2_transition_v01_01.tgt");
		tgtPosition_65 = new TgtPosition(World.Act6.KaruiFortress.Name, "beach_karuipools_v01_01.tgt");
		tgtPosition_66 = new TgtPosition(World.Act6.Ridge.Name, "swamp_to_ridge_v01_01.tgt");
		tgtPosition_67 = new TgtPosition(World.Act6.LowerPrison.Name, "ledge_prisonback.tgt");
		tgtPosition_68 = new TgtPosition(World.Act6.ShavronneTower.Name, "shavronne_prison_door_up_v01_01.tgt");
		tgtPosition_69 = new TgtPosition("Next tower level", "dungeon_prison_exit_up_v01_01.tgt | prison_ladder_v01_01.tgt | tower_spiral_stair_v01_01.tgt | dungeon_prison_door_up_v01_01.tgt", closest: true);
		tgtPosition_70 = new TgtPosition(World.Act6.WesternForest.Name, "beach_passageblock_v01_01.tgt");
		tgtPosition_71 = new TgtPosition(World.Act6.Riverways.Name, "roadtothickforest_entrance_v01_01.tgt");
		keicOaiOub = new TgtPosition(World.Act6.Wetlands.Name, "bridgeconnection_v01_01.tgt");
		tgtPosition_72 = new TgtPosition(World.Act6.SouthernForest.Name, "forest_to_river_v01_01.tgt");
		tgtPosition_73 = new TgtPosition(World.Act6.CavernOfAnger.Name, "forest_caveentrance_v01_01.tgt");
		tgtPosition_74 = new TgtPosition(World.Act6.Beacon.Name, "caveup_exit_v01_01.tgt");
		tgtPosition_75 = new TgtPosition(World.Act7.Crossroads.Name, "bridgeconnection_v01_01.tgt", closest: false, 10, 50);
		tgtPosition_76 = new TgtPosition(World.Act7.FellshrineRuins.Name, "wall_gate_v01_01.tgt", closest: false, 10, 50);
		tgtPosition_77 = new TgtPosition(World.Act7.Crypt.Name, "church_dungeon_entrance_v01_01.tgt");
		tgtPosition_78 = new TgtPosition(World.Act7.ChamberOfSins1.Name, "temple_entrance_v01_01.tgt");
		tgtPosition_79 = new TgtPosition(World.Act7.ChamberOfSins2.Name, "templeruinforest_exit_down_v01_01.tgt");
		tgtPosition_80 = new TgtPosition(World.Act7.Den.Name, "templeruinforest_maligaro_passage.tgt");
		tgtPosition_81 = new TgtPosition(World.Act7.AshenFields.Name, "forestcaveup_exit_v01_01.tgt");
		tgtPosition_82 = new TgtPosition(World.Act7.NorthernForest.Name, "oldfields_campboss_v01_01.tgt", closest: true, 10, 20);
		tgtPosition_83 = new TgtPosition(World.Act7.DreadThicket.Name, "grovewall_entrance_v01_01.tgt");
		tgtPosition_84 = new TgtPosition(World.Act7.Causeway.Name, "forestriver_plinthtransition_v01_01.tgt");
		tgtPosition_85 = new TgtPosition(World.Act7.VaalCity.Name, "vaal_stairs_bottom_v01_01.tgt", closest: false, 15);
		tgtPosition_86 = new TgtPosition(World.Act7.TempleOfDecay1.Name, "BanteaySrei_Web.tgt");
		tgtPosition_87 = new TgtPosition("Next temple level", "dungeon_web_stairs_down_v01_01.tgt", closest: true);
		tgtPosition_88 = new TgtPosition("Next temple level", "dungeon_web_stairs_down_v01_01.tgt | dungeon_web_inca_exit_v03_01.tgt | dungeon_web_inca_exit_v01_01.tgt", closest: true);
		tgtPosition_89 = new TgtPosition("Next ramparts level", "ramparts_wall_accesss_v01_01.tgt | act8_docks_v01_01.tgt");
		tgtPosition_90 = new TgtPosition(World.Act8.DoedreCesspool.Name, "sewerwall_end_tunnel_v01_0?.tgt");
		tgtPosition_91 = new TgtPosition(World.Act8.GrandPromenade.Name, "slum_sewer_entrance_v03_01.tgt | doedre_sewer_grate_v01_01.tgt | sewer_ladder_up_v01_01.tgt", closest: true);
		tgtPosition_92 = new TgtPosition(World.Act8.Quay.Name, "slum_sewer_entrance_v03_01.tgt | doedre_sewer_grate_v01_01.tgt | sewerwall_exit_v01_01.tgt", closest: true, 10, 17);
		tgtPosition_93 = new TgtPosition(World.Act8.GrainGate.Name, "market_transition_warehouse_v01_01.tgt");
		tgtPosition_94 = new TgtPosition(World.Act8.ImperialFields.Name, "act8_grain_gate_transition_v01_01.tgt");
		tgtPosition_95 = new TgtPosition(World.Act8.SolarisTemple1.Name, "act8_temple_entrance_v01_01.tgt");
		tgtPosition_96 = new TgtPosition(World.Act8.SolarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");
		tgtPosition_97 = new TgtPosition(World.Act8.SolarisConcourse.Name, "temple_to_battlefront_v01_01.tgt");
		tgtPosition_98 = new TgtPosition(World.Act8.BathHouse.Name, "arch_promenade_to_arsenal_v01_01.tgt");
		tgtPosition_99 = new TgtPosition(World.Act8.HighGardens.Name, "bathhouse_transition_v01_01.tgt", closest: true);
		tgtPosition_100 = new TgtPosition(World.Act8.LunarisConcourse.Name, "bathhouse_transition_v01_01.tgt");
		tgtPosition_101 = new TgtPosition(World.Act8.LunarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");
		tgtPosition_102 = new TgtPosition(World.Act8.LunarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");
		tgtPosition_103 = new TgtPosition(World.Act8.HarbourBridge.Name, "act3_riverbridge_transition_v01_01.tgt");
		tgtPosition_104 = new TgtPosition(World.Act9.BloodAqueduct.Name, "bridge_arena_v01_01.tgt | bridge_arena_v01_01.tgt", closest: true);
		tgtPosition_105 = new TgtPosition(World.Act9.Highgate.Name, "mountiantown_connection_blood.tgt");
		tgtPosition_106 = new TgtPosition("Next Descent level", "descent_top_winch_v01_01.tgt | descent_ravine_convex_winch_v01_01.tgt | decent_ravinestraight_tocliff_winch_v01_01.tgt", closest: true);
		tgtPosition_107 = new TgtPosition(World.Act9.Oasis.Name, "wagons_transition_v01_01.tgt");
		tgtPosition_108 = new TgtPosition(World.Act9.Foothills.Name, "desert_to_foothills_v01_01.tgt");
		tgtPosition_109 = new TgtPosition(World.Act9.BoilingLake.Name, "foothills_to_boilinglakev01_01.tgt");
		tgtPosition_110 = new TgtPosition(World.Act9.Tunnel.Name, "foothills_tunnel_exit_v01_01.tgt");
		tgtPosition_111 = new TgtPosition(World.Act9.Quarry.Name, "tunnels_to_quarry_transition_v01_01.tgt");
		tgtPosition_112 = new TgtPosition(World.Act9.Refinery.Name, "warehouse_entrance_v01_01.tgt");
		tgtPosition_113 = new TgtPosition(World.Act9.BellyOfBeast.Name, "BeastMembrane.tgt");
		tgtPosition_114 = new TgtPosition(World.Act9.RottingCore.Name, "belly_to_rottingcore_v01_01.tgt");
		tgtPosition_115 = new TgtPosition(World.Act10.RavagedSquare.Name, "cathedral_roof_transition_v01_01.tgt");
		tgtPosition_116 = new TgtPosition(World.Act10.Ossuary.Name, "Oriath_AreaTransition_v01_04.tgt");
		tgtPosition_117 = new TgtPosition(World.Act10.TorchedCourts.Name, "Oriath_AreaTransition_v01_03.tgt");
		tgtPosition_118 = new TgtPosition(World.Act10.Reliquary.Name, "Oriath_AreaTransition_v01_02.tgt");
		tgtPosition_119 = new TgtPosition(World.Act10.ControlBlocks.Name, "slaveden_entrance_steps_v01_01.tgt");
		tgtPosition_120 = new TgtPosition(World.Act10.DesecratedChambers.Name, "templar_to_innocents_v01_01.tgt");
		tgtPosition_121 = new TgtPosition(World.Act10.Canals.Name, "OriathBlockage_v02_01.tgt");
		tgtPosition_122 = new TgtPosition(World.Act10.FeedingTrough.Name, "CanalBridgeTransition_v01_01.tgt");
		walkablePosition_0 = new WalkablePosition(World.Act1.Coast.Name, 384, 217);
		walkablePosition_1 = new WalkablePosition(World.Act2.Riverways.Name, 81, 262);
		walkablePosition_2 = new WalkablePosition(World.Act2.OldFields.Name, 303, 264);
		walkablePosition_3 = new WalkablePosition(World.Act2.FellshrineRuins.Name, 1000, 280);
		walkablePosition_4 = new WalkablePosition(World.Act3.Slums.Name, 597, 524);
		walkablePosition_5 = new WalkablePosition(World.Act4.DriedLake.Name, 88, 442);
		walkablePosition_6 = new WalkablePosition(World.Act4.Mines1.Name, 330, 620);
		walkablePosition_7 = new WalkablePosition(World.Act4.Ascent.Name, 600, 403);
		walkablePosition_8 = new WalkablePosition(World.Act5.ControlBlocks.Name, 356, 410);
		walkablePosition_9 = new WalkablePosition(World.Act6.TwilightStrand.Name, 121, 447);
		walkablePosition_10 = new WalkablePosition(World.Act6.Coast.Name, 378, 356);
		walkablePosition_11 = new WalkablePosition(World.Act7.BrokenBridge.Name, 550, 710);
		walkablePosition_12 = new WalkablePosition(World.Act8.ToxicConduits.Name, 176, 659);
		walkablePosition_13 = new WalkablePosition(World.Act9.Descent.Name, 600, 403);
		walkablePosition_14 = new WalkablePosition(World.Act10.CathedralRooftop.Name, 620, 347);
		walkablePosition_15 = new WalkablePosition(World.Act11.TemplarLaboratory.Name, 993, 792);
		walkablePosition_16 = new WalkablePosition(World.Act11.FallenCourts.Name, 920, 483);
		walkablePosition_17 = new WalkablePosition(World.Act11.HauntedReliquary.Name, 471, 686);
	}
}
