using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;

namespace ExPlugins.EXtensions;

public static class World
{
	public static class Act1
	{
		public static readonly AreaInfo TwilightStrand;

		public static readonly AreaInfo LioneyeWatch;

		public static readonly AreaInfo Coast;

		public static readonly AreaInfo TidalIsland;

		public static readonly AreaInfo MudFlats;

		public static readonly AreaInfo FetidPool;

		public static readonly AreaInfo SubmergedPassage;

		public static readonly AreaInfo FloodedDepths;

		public static readonly AreaInfo Ledge;

		public static readonly AreaInfo Climb;

		public static readonly AreaInfo LowerPrison;

		public static readonly AreaInfo UpperPrison;

		public static readonly AreaInfo PrisonerGate;

		public static readonly AreaInfo ShipGraveyard;

		public static readonly AreaInfo ShipGraveyardCave;

		public static readonly AreaInfo CavernOfWrath;

		public static readonly AreaInfo CavernOfAnger;

		static Act1()
		{
			TwilightStrand = new AreaInfo("1_1_1", "The Twilight Strand");
			LioneyeWatch = new AreaInfo("1_1_town", "Lioneye's Watch");
			Coast = new AreaInfo("1_1_2", "The Coast");
			TidalIsland = new AreaInfo("1_1_2a", "The Tidal Island");
			MudFlats = new AreaInfo("1_1_3", "The Mud Flats");
			FetidPool = new AreaInfo("1_1_3a", "The Fetid Pool");
			SubmergedPassage = new AreaInfo("1_1_4_1", "The Submerged Passage");
			FloodedDepths = new AreaInfo("1_1_4_0", "The Flooded Depths");
			Ledge = new AreaInfo("1_1_5", "The Ledge");
			Climb = new AreaInfo("1_1_6", "The Climb");
			LowerPrison = new AreaInfo("1_1_7_1", "The Lower Prison");
			UpperPrison = new AreaInfo("1_1_7_2", "The Upper Prison");
			PrisonerGate = new AreaInfo("1_1_8", "Prisoner's Gate");
			ShipGraveyard = new AreaInfo("1_1_9", "The Ship Graveyard");
			ShipGraveyardCave = new AreaInfo("1_1_9a", "The Ship Graveyard Cave");
			CavernOfWrath = new AreaInfo("1_1_11_1", "The Cavern of Wrath");
			CavernOfAnger = new AreaInfo("1_1_11_2", "The Cavern of Anger");
		}
	}

	public static class Act2
	{
		public static readonly AreaInfo SouthernForest;

		public static readonly AreaInfo ForestEncampment;

		public static readonly AreaInfo OldFields;

		public static readonly AreaInfo Den;

		public static readonly AreaInfo Riverways;

		public static readonly AreaInfo WesternForest;

		public static readonly AreaInfo WeaverChambers;

		public static readonly AreaInfo Crossroads;

		public static readonly AreaInfo ChamberOfSins1;

		public static readonly AreaInfo ChamberOfSins2;

		public static readonly AreaInfo FellshrineRuins;

		public static readonly AreaInfo Crypt1;

		public static readonly AreaInfo Crypt2;

		public static readonly AreaInfo BrokenBridge;

		public static readonly AreaInfo Wetlands;

		public static readonly AreaInfo VaalRuins;

		public static readonly AreaInfo NorthernForest;

		public static readonly AreaInfo DreadThicket;

		public static readonly AreaInfo Caverns;

		public static readonly AreaInfo AncientPyramid;

		static Act2()
		{
			SouthernForest = new AreaInfo("1_2_1", "The Southern Forest");
			ForestEncampment = new AreaInfo("1_2_town", "The Forest Encampment");
			OldFields = new AreaInfo("1_2_2", "The Old Fields");
			Den = new AreaInfo("1_2_2a", "The Den");
			Riverways = new AreaInfo("1_2_7", "The Riverways");
			WesternForest = new AreaInfo("1_2_9", "The Western Forest");
			WeaverChambers = new AreaInfo("1_2_10", "The Weaver's Chambers");
			Crossroads = new AreaInfo("1_2_3", "The Crossroads");
			ChamberOfSins1 = new AreaInfo("1_2_6_1", "The Chamber of Sins Level 1");
			ChamberOfSins2 = new AreaInfo("1_2_6_2", "The Chamber of Sins Level 2");
			FellshrineRuins = new AreaInfo("1_2_15", "The Fellshrine Ruins");
			Crypt1 = new AreaInfo("1_2_5_1", "The Crypt Level 1");
			Crypt2 = new AreaInfo("1_2_5_2", "The Crypt Level 2");
			BrokenBridge = new AreaInfo("1_2_4", "The Broken Bridge");
			Wetlands = new AreaInfo("1_2_12", "The Wetlands");
			VaalRuins = new AreaInfo("1_2_11", "The Vaal Ruins");
			NorthernForest = new AreaInfo("1_2_8", "The Northern Forest");
			DreadThicket = new AreaInfo("1_2_13", "The Dread Thicket");
			Caverns = new AreaInfo("1_2_14_2", "The Caverns");
			AncientPyramid = new AreaInfo("1_2_14_3", "The Ancient Pyramid");
		}
	}

	public static class Act3
	{
		public static readonly AreaInfo CityOfSarn;

		public static readonly AreaInfo SarnEncampment;

		public static readonly AreaInfo Slums;

		public static readonly AreaInfo Crematorium;

		public static readonly AreaInfo Sewers;

		public static readonly AreaInfo Marketplace;

		public static readonly AreaInfo Catacombs;

		public static readonly AreaInfo Battlefront;

		public static readonly AreaInfo Docks;

		public static readonly AreaInfo SolarisTemple1;

		public static readonly AreaInfo SolarisTemple2;

		public static readonly AreaInfo EbonyBarracks;

		public static readonly AreaInfo LunarisTemple1;

		public static readonly AreaInfo LunarisTemple2;

		public static readonly AreaInfo ImperialGardens;

		public static readonly AreaInfo Library;

		public static readonly AreaInfo Archives;

		public static readonly AreaInfo SceptreOfGod;

		public static readonly AreaInfo UpperSceptreOfGod;

		static Act3()
		{
			CityOfSarn = new AreaInfo("1_3_1", "The City of Sarn");
			SarnEncampment = new AreaInfo("1_3_town", "The Sarn Encampment");
			Slums = new AreaInfo("1_3_2", "The Slums");
			Crematorium = new AreaInfo("1_3_3_1", "The Crematorium");
			Sewers = new AreaInfo("1_3_10_1", "The Sewers");
			Marketplace = new AreaInfo("1_3_5", "The Marketplace");
			Catacombs = new AreaInfo("1_3_6", "The Catacombs");
			Battlefront = new AreaInfo("1_3_7", "The Battlefront");
			Docks = new AreaInfo("1_3_9", "The Docks");
			SolarisTemple1 = new AreaInfo("1_3_8_1", "The Solaris Temple Level 1");
			SolarisTemple2 = new AreaInfo("1_3_8_2", "The Solaris Temple Level 2");
			EbonyBarracks = new AreaInfo("1_3_13", "The Ebony Barracks");
			LunarisTemple1 = new AreaInfo("1_3_14_1", "The Lunaris Temple Level 1");
			LunarisTemple2 = new AreaInfo("1_3_14_2", "The Lunaris Temple Level 2");
			ImperialGardens = new AreaInfo("1_3_15", "The Imperial Gardens");
			Library = new AreaInfo("1_3_17_1", "The Library");
			Archives = new AreaInfo("1_3_17_2", "The Archives");
			SceptreOfGod = new AreaInfo("1_3_18_1", "The Sceptre of God");
			UpperSceptreOfGod = new AreaInfo("1_3_18_2", "The Upper Sceptre of God");
		}
	}

	public static class Act4
	{
		public static readonly AreaInfo Aqueduct;

		public static readonly AreaInfo Highgate;

		public static readonly AreaInfo DriedLake;

		public static readonly AreaInfo Mines1;

		public static readonly AreaInfo Mines2;

		public static readonly AreaInfo CrystalVeins;

		public static readonly AreaInfo KaomDream;

		public static readonly AreaInfo KaomStronghold;

		public static readonly AreaInfo DaressoDream;

		public static readonly AreaInfo GrandArena;

		public static readonly AreaInfo BellyOfBeast1;

		public static readonly AreaInfo BellyOfBeast2;

		public static readonly AreaInfo Harvest;

		public static readonly AreaInfo Ascent;

		static Act4()
		{
			Aqueduct = new AreaInfo("1_4_1", "The Aqueduct");
			Highgate = new AreaInfo("1_4_town", "Highgate");
			DriedLake = new AreaInfo("1_4_2", "The Dried Lake");
			Mines1 = new AreaInfo("1_4_3_1", "The Mines Level 1");
			Mines2 = new AreaInfo("1_4_3_2", "The Mines Level 2");
			CrystalVeins = new AreaInfo("1_4_3_3", "The Crystal Veins");
			KaomDream = new AreaInfo("1_4_4_1", "Kaom's Dream");
			KaomStronghold = new AreaInfo("1_4_4_3", "Kaom's Stronghold");
			DaressoDream = new AreaInfo("1_4_5_1", "Daresso's Dream");
			GrandArena = new AreaInfo("1_4_5_2", "The Grand Arena");
			BellyOfBeast1 = new AreaInfo("1_4_6_1", "The Belly of the Beast Level 1");
			BellyOfBeast2 = new AreaInfo("1_4_6_2", "The Belly of the Beast Level 2");
			Harvest = new AreaInfo("1_4_6_3", "The Harvest");
			Ascent = new AreaInfo("1_4_7", "The Ascent");
		}
	}

	public static class Act5
	{
		public static readonly AreaInfo SlavePens;

		public static readonly AreaInfo OverseerTower;

		public static readonly AreaInfo ControlBlocks;

		public static readonly AreaInfo OriathSquare;

		public static readonly AreaInfo TemplarCourts;

		public static readonly AreaInfo ChamberOfInnocence;

		public static readonly AreaInfo TorchedCourts;

		public static readonly AreaInfo RuinedSquare;

		public static readonly AreaInfo Ossuary;

		public static readonly AreaInfo Reliquary;

		public static readonly AreaInfo CathedralRooftop;

		static Act5()
		{
			SlavePens = new AreaInfo("1_5_1", "The Slave Pens");
			OverseerTower = new AreaInfo("1_5_town", "Overseer's Tower");
			ControlBlocks = new AreaInfo("1_5_2", "The Control Blocks");
			OriathSquare = new AreaInfo("1_5_3", "Oriath Square");
			TemplarCourts = new AreaInfo("1_5_4", "The Templar Courts");
			ChamberOfInnocence = new AreaInfo("1_5_5", "The Chamber of Innocence");
			TorchedCourts = new AreaInfo("1_5_4b", "The Torched Courts");
			RuinedSquare = new AreaInfo("1_5_3b", "The Ruined Square");
			Ossuary = new AreaInfo("1_5_6", "The Ossuary");
			Reliquary = new AreaInfo("1_5_7", "The Reliquary");
			CathedralRooftop = new AreaInfo("1_5_8", "The Cathedral Rooftop");
		}
	}

	public static class Act6
	{
		public static readonly AreaInfo LioneyeWatch;

		public static readonly AreaInfo TwilightStrand;

		public static readonly AreaInfo Coast;

		public static readonly AreaInfo TidalIsland;

		public static readonly AreaInfo MudFlats;

		public static readonly AreaInfo KaruiFortress;

		public static readonly AreaInfo Ridge;

		public static readonly AreaInfo LowerPrison;

		public static readonly AreaInfo ShavronneTower;

		public static readonly AreaInfo PrisonerGate;

		public static readonly AreaInfo WesternForest;

		public static readonly AreaInfo Riverways;

		public static readonly AreaInfo Wetlands;

		public static readonly AreaInfo SouthernForest;

		public static readonly AreaInfo CavernOfAnger;

		public static readonly AreaInfo Beacon;

		public static readonly AreaInfo BrineKingReef;

		static Act6()
		{
			LioneyeWatch = new AreaInfo("2_6_town", "Lioneye's Watch");
			TwilightStrand = new AreaInfo("2_6_1", "The Twilight Strand");
			Coast = new AreaInfo("2_6_2", "The Coast");
			TidalIsland = new AreaInfo("2_6_3", "The Tidal Island");
			MudFlats = new AreaInfo("2_6_4", "The Mud Flats");
			KaruiFortress = new AreaInfo("2_6_5", "The Karui Fortress");
			Ridge = new AreaInfo("2_6_6", "The Ridge");
			LowerPrison = new AreaInfo("2_6_7_1", "The Lower Prison");
			ShavronneTower = new AreaInfo("2_6_7_2", "Shavronne's Tower");
			PrisonerGate = new AreaInfo("2_6_8", "Prisoner's Gate");
			WesternForest = new AreaInfo("2_6_9", "The Western Forest");
			Riverways = new AreaInfo("2_6_10", "The Riverways");
			Wetlands = new AreaInfo("2_6_11", "The Wetlands");
			SouthernForest = new AreaInfo("2_6_12", "The Southern Forest");
			CavernOfAnger = new AreaInfo("2_6_13", "The Cavern of Anger");
			Beacon = new AreaInfo("2_6_14", "The Beacon");
			BrineKingReef = new AreaInfo("2_6_15", "The Brine King's Reef");
		}
	}

	public static class Act7
	{
		public static readonly AreaInfo BridgeEncampment;

		public static readonly AreaInfo BrokenBridge;

		public static readonly AreaInfo Crossroads;

		public static readonly AreaInfo FellshrineRuins;

		public static readonly AreaInfo Crypt;

		public static readonly AreaInfo ChamberOfSins1;

		public static readonly AreaInfo ChamberOfSins2;

		public static readonly AreaInfo MaligaroSanctum;

		public static readonly AreaInfo Den;

		public static readonly AreaInfo AshenFields;

		public static readonly AreaInfo NorthernForest;

		public static readonly AreaInfo DreadThicket;

		public static readonly AreaInfo Causeway;

		public static readonly AreaInfo VaalCity;

		public static readonly AreaInfo TempleOfDecay1;

		public static readonly AreaInfo TempleOfDecay2;

		static Act7()
		{
			BridgeEncampment = new AreaInfo("2_7_town", "The Bridge Encampment");
			BrokenBridge = new AreaInfo("2_7_1", "The Broken Bridge");
			Crossroads = new AreaInfo("2_7_2", "The Crossroads");
			FellshrineRuins = new AreaInfo("2_7_3", "The Fellshrine Ruins");
			Crypt = new AreaInfo("2_7_4", "The Crypt");
			ChamberOfSins1 = new AreaInfo("2_7_5_1", "The Chamber of Sins Level 1");
			ChamberOfSins2 = new AreaInfo("2_7_5_2", "The Chamber of Sins Level 2");
			MaligaroSanctum = new AreaInfo("2_7_5_map", "Maligaro's Sanctum");
			Den = new AreaInfo("2_7_6", "The Den");
			AshenFields = new AreaInfo("2_7_7", "The Ashen Fields");
			NorthernForest = new AreaInfo("2_7_8", "The Northern Forest");
			DreadThicket = new AreaInfo("2_7_9", "The Dread Thicket");
			Causeway = new AreaInfo("2_7_10", "The Causeway");
			VaalCity = new AreaInfo("2_7_11", "The Vaal City");
			TempleOfDecay1 = new AreaInfo("2_7_12_1", "The Temple of Decay Level 1");
			TempleOfDecay2 = new AreaInfo("2_7_12_2", "The Temple of Decay Level 2");
		}
	}

	public static class Act8
	{
		public static readonly AreaInfo SarnRamparts;

		public static readonly AreaInfo SarnEncampment;

		public static readonly AreaInfo ToxicConduits;

		public static readonly AreaInfo DoedreCesspool;

		public static readonly AreaInfo Quay;

		public static readonly AreaInfo GrainGate;

		public static readonly AreaInfo ImperialFields;

		public static readonly AreaInfo GrandPromenade;

		public static readonly AreaInfo BathHouse;

		public static readonly AreaInfo HighGardens;

		public static readonly AreaInfo SolarisConcourse;

		public static readonly AreaInfo SolarisTemple1;

		public static readonly AreaInfo SolarisTemple2;

		public static readonly AreaInfo LunarisConcourse;

		public static readonly AreaInfo LunarisTemple1;

		public static readonly AreaInfo LunarisTemple2;

		public static readonly AreaInfo HarbourBridge;

		static Act8()
		{
			SarnRamparts = new AreaInfo("2_8_1", "The Sarn Ramparts");
			SarnEncampment = new AreaInfo("2_8_town", "The Sarn Encampment");
			ToxicConduits = new AreaInfo("2_8_2_1", "The Toxic Conduits");
			DoedreCesspool = new AreaInfo("2_8_2_2", "Doedre's Cesspool");
			Quay = new AreaInfo("2_8_8", "The Quay");
			GrainGate = new AreaInfo("2_8_9", "The Grain Gate");
			ImperialFields = new AreaInfo("2_8_10", "The Imperial Fields");
			GrandPromenade = new AreaInfo("2_8_3", "The Grand Promenade");
			BathHouse = new AreaInfo("2_8_5", "The Bath House");
			HighGardens = new AreaInfo("2_8_4", "The High Gardens");
			SolarisConcourse = new AreaInfo("2_8_11", "The Solaris Concourse");
			SolarisTemple1 = new AreaInfo("2_8_12_1", "The Solaris Temple Level 1");
			SolarisTemple2 = new AreaInfo("2_8_12_2", "The Solaris Temple Level 2");
			LunarisConcourse = new AreaInfo("2_8_6", "The Lunaris Concourse");
			LunarisTemple1 = new AreaInfo("2_8_7_1_", "The Lunaris Temple Level 1");
			LunarisTemple2 = new AreaInfo("2_8_7_2", "The Lunaris Temple Level 2");
			HarbourBridge = new AreaInfo("2_8_13", "The Harbour Bridge");
		}
	}

	public static class Act9
	{
		public static readonly AreaInfo BloodAqueduct;

		public static readonly AreaInfo Highgate;

		public static readonly AreaInfo Descent;

		public static readonly AreaInfo VastiriDesert;

		public static readonly AreaInfo Oasis;

		public static readonly AreaInfo Foothills;

		public static readonly AreaInfo BoilingLake;

		public static readonly AreaInfo Tunnel;

		public static readonly AreaInfo Quarry;

		public static readonly AreaInfo Refinery;

		public static readonly AreaInfo BellyOfBeast;

		public static readonly AreaInfo RottingCore;

		static Act9()
		{
			BloodAqueduct = new AreaInfo("2_9_1", "The Blood Aqueduct");
			Highgate = new AreaInfo("2_9_town", "Highgate");
			Descent = new AreaInfo("2_9_2", "The Descent");
			VastiriDesert = new AreaInfo("2_9_3", "The Vastiri Desert");
			Oasis = new AreaInfo("2_9_4", "The Oasis");
			Foothills = new AreaInfo("2_9_5", "The Foothills");
			BoilingLake = new AreaInfo("2_9_6", "The Boiling Lake");
			Tunnel = new AreaInfo("2_9_7", "The Tunnel");
			Quarry = new AreaInfo("2_9_8", "The Quarry");
			Refinery = new AreaInfo("2_9_9", "The Refinery");
			BellyOfBeast = new AreaInfo("2_9_10_1", "The Belly of the Beast");
			RottingCore = new AreaInfo("2_9_10_2", "The Rotting Core");
		}
	}

	public static class Act10
	{
		public static readonly AreaInfo OriathDocks;

		public static readonly AreaInfo CathedralRooftop;

		public static readonly AreaInfo RavagedSquare;

		public static readonly AreaInfo Ossuary;

		public static readonly AreaInfo ControlBlocks;

		public static readonly AreaInfo Reliquary;

		public static readonly AreaInfo TorchedCourts;

		public static readonly AreaInfo DesecratedChambers;

		public static readonly AreaInfo Canals;

		public static readonly AreaInfo FeedingTrough;

		static Act10()
		{
			OriathDocks = new AreaInfo("2_10_town", "Oriath Docks");
			CathedralRooftop = new AreaInfo("2_10_1", "The Cathedral Rooftop");
			RavagedSquare = new AreaInfo("2_10_2", "The Ravaged Square");
			Ossuary = new AreaInfo("2_10_9", "The Ossuary");
			ControlBlocks = new AreaInfo("2_10_7", "The Control Blocks");
			Reliquary = new AreaInfo("2_10_8", "The Reliquary");
			TorchedCourts = new AreaInfo("2_10_3", "The Torched Courts");
			DesecratedChambers = new AreaInfo("2_10_4", "The Desecrated Chambers");
			Canals = new AreaInfo("2_10_5", "The Canals");
			FeedingTrough = new AreaInfo("2_10_6", "The Feeding Trough");
		}
	}

	public static class Act11
	{
		public static readonly AreaInfo Oriath;

		public static readonly AreaInfo KaruiShores;

		public static readonly AreaInfo TemplarLaboratory;

		public static readonly AreaInfo FallenCourts;

		public static readonly AreaInfo HauntedReliquary;

		static Act11()
		{
			Oriath = new AreaInfo("2_11_town", "Oriath");
			KaruiShores = new AreaInfo("2_11_endgame_town", "Karui Shores");
			TemplarLaboratory = new AreaInfo("2_11_lab", "The Templar Laboratory");
			FallenCourts = new AreaInfo("2_11_1", "The Fallen Courts");
			HauntedReliquary = new AreaInfo("2_11_2", "The Haunted Reliquary");
		}
	}

	public static DatWorldAreaWrapper CurrentArea => LocalData.WorldArea;

	public static DatWorldAreaWrapper LastOpenedAct
	{
		get
		{
			DatWorldAreaWrapper val = (from a in Enumerable.Where(InstanceInfo.AvailableWaypoints.Values, (DatWorldAreaWrapper a) => a.IsTown)
				orderby a.Act descending
				select a).FirstOrDefault();
			if (val == null)
			{
				GlobalLog.Error("[GetLastOpenedAct] Unknown error. Fail to get any opened act.");
				ErrorManager.ReportCriticalError();
				return null;
			}
			return val;
		}
	}

	static World()
	{
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		Dictionary<string, string> dictionary = Dat.WorldAreas.ToDictionary((DatWorldAreaWrapper a) => a.Id, (DatWorldAreaWrapper a) => a.Name);
		string text = "";
		Type[] nestedTypes = typeof(World).GetNestedTypes();
		foreach (Type type in nestedTypes)
		{
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				AreaInfo areaInfo = (AreaInfo)fieldInfo.GetValue(fieldInfo);
				if (dictionary.TryGetValue(areaInfo.Id, out var value))
				{
					if (value != areaInfo.Name)
					{
						text = "Invalid area info in \"" + fieldInfo.Name + "\" field. Area name: \"" + areaInfo.Name + "\". Correct name: \"" + value + "\"";
						GlobalLog.Error("[World] " + text + ".");
					}
				}
				else
				{
					text = "[World] Invalid area info in \"" + fieldInfo.Name + "\" field. DatWorldAreas does not contain an area with \"" + areaInfo.Id + "\" id.";
					GlobalLog.Error("[World] " + text + ".");
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			BotManager.Stop(new StopReasonData("world_init_error", text, (object)null), false);
		}
	}

	public static bool IsWaypointOpened(string areaId)
	{
		return InstanceInfo.AvailableWaypoints.ContainsKey(areaId);
	}
}
