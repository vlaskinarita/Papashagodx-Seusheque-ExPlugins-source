using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using Newtonsoft.Json;

namespace ExPlugins.MapBotEx;

public class MapSettings
{
	private class Class32
	{
		public int Priority;

		public bool Ignore;

		public bool IgnoreBossroom;

		public int MobRemaining;

		public int ExplorationPercent;

		public int FinalPulse;

		public bool? FastTransition;

		public bool BossRush;
	}

	private static readonly string string_0;

	private static MapSettings mapSettings_0;

	[CompilerGenerated]
	private readonly List<MapData> list_0 = new List<MapData>();

	[CompilerGenerated]
	private readonly Dictionary<string, MapData> dictionary_0 = new Dictionary<string, MapData>();

	public static MapSettings Instance => mapSettings_0 ?? (mapSettings_0 = new MapSettings());

	public List<MapData> MapList
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
	}

	public Dictionary<string, MapData> MapDict
	{
		[CompilerGenerated]
		get
		{
			return dictionary_0;
		}
	}

	private MapSettings()
	{
		InitList();
		Load();
		InitDict();
		Configuration.OnSaveAll += delegate
		{
			Save();
		};
		list_0 = (from m in MapList
			orderby m.Priority descending, m.Name
			select m).ToList();
	}

	private void InitList()
	{
		MapList.Add(new MapData(MapNames.AridLake, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Channel, MapType.Regular));
		MapList.Add(new MapData(MapNames.SpiderForest, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.UndergroundSea, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Museum, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Tower, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Peninsula, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Dungeon, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Bog, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.AcidCaverns, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.FrozenCabins, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Plaza, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.MoonTemple, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Necropolis, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Ramparts, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Basilica, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Reef, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.InfestedValley, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Carcass, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Volcano, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Estuary, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Geode, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Graveyard, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.HauntedMansion, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Iceberg, MapType.Regular));
		MapList.Add(new MapData(MapNames.AncientCity, MapType.Regular));
		MapList.Add(new MapData(MapNames.Academy, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.ArachnidTomb, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.DefiledCathedral, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Racecourse, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Shipyard, MapType.Regular));
		MapList.Add(new MapData(MapNames.WhakawairuaTuahu, MapType.Multilevel)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Park, MapType.Regular));
		MapList.Add(new MapData(MapNames.Beach, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Mesa, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Alleyways, MapType.Regular));
		MapList.Add(new MapData(MapNames.ArachnidNest, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Arcade, MapType.Regular));
		MapList.Add(new MapData(MapNames.Arena, MapType.Complex));
		MapList.Add(new MapData(MapNames.Armoury, MapType.Regular));
		MapList.Add(new MapData(MapNames.Arsenal, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.AshenWood, MapType.Regular));
		MapList.Add(new MapData(MapNames.Atoll, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Barrows, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Bazaar, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Beachhead, MapType.Bossroom)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Belfry, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.BoneCrypt, MapType.Regular));
		MapList.Add(new MapData(MapNames.BrambleValley, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.BurialChambers, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.CaerBlaiddWolfpackDen, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Cage, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Caldera, MapType.Complex));
		MapList.Add(new MapData(MapNames.Canyon, MapType.Regular));
		MapList.Add(new MapData(MapNames.CastleRuins, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Cells, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Cemetery, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Chateau, MapType.Regular));
		MapList.Add(new MapData(MapNames.CitySquare, MapType.Regular));
		MapList.Add(new MapData(MapNames.ColdRiver, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Colonnade, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Colosseum, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Conservatory, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.CoralRuins, MapType.Regular));
		MapList.Add(new MapData(MapNames.Core, MapType.Complex)
		{
			Unsupported = true
		});
		MapList.Add(new MapData(MapNames.Courthouse, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Courtyard, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Coves, MapType.Regular));
		MapList.Add(new MapData(MapNames.Crater, MapType.Regular));
		MapList.Add(new MapData(MapNames.CrimsonTemple, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.CrimsonTownship, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.CrystalOre, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.CursedCrypt, MapType.Regular));
		MapList.Add(new MapData(MapNames.DarkForest, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Desert, MapType.Regular));
		MapList.Add(new MapData(MapNames.DesertSpring, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Dig, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.DrySea, MapType.Regular));
		MapList.Add(new MapData(MapNames.Dunes, MapType.Regular));
		MapList.Add(new MapData(MapNames.Excavation, MapType.Complex));
		MapList.Add(new MapData(MapNames.Factory, MapType.Regular));
		MapList.Add(new MapData(MapNames.Fields, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.FloodedMine, MapType.Regular));
		MapList.Add(new MapData(MapNames.ForbiddenWoods, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.ForgeOfPhoenix, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.ForkingRiver, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Foundry, MapType.Complex));
		MapList.Add(new MapData(MapNames.FungalHollow, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Gardens, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Ghetto, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Glacier, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.GraveTrough, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Grotto, MapType.Regular));
		MapList.Add(new MapData(MapNames.IvoryTemple, MapType.Complex));
		MapList.Add(new MapData(MapNames.JungleValley, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Laboratory, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Lair, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.LairOfHydra, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.LavaChamber, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.LavaLake, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Leyline, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Lighthouse, MapType.Regular));
		MapList.Add(new MapData(MapNames.Lookout, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.MaelstromOfChaos, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Malformation, MapType.Complex));
		MapList.Add(new MapData(MapNames.MaoKun, MapType.Regular)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Marshes, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Mausoleum, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Maze, MapType.Regular));
		MapList.Add(new MapData(MapNames.MazeOfMinotaur, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.MineralPools, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.MudGeyser, MapType.Regular));
		MapList.Add(new MapData(MapNames.OlmecSanctum, MapType.Complex)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Orchard, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.OvergrownRuin, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.OvergrownShrine, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Palace, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Pen, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Phantasmagoria, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Pier, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Pit, MapType.Regular));
		MapList.Add(new MapData(MapNames.PitOfChimera, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Plateau, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.PoorjoyAsylum, MapType.Regular)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Port, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Precinct, MapType.Regular));
		MapList.Add(new MapData(MapNames.PrimordialBlocks, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.PrimordialPool, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Promenade, MapType.Regular));
		MapList.Add(new MapData(MapNames.PutridCloister, MapType.Multilevel)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.RelicChambers, MapType.Regular));
		MapList.Add(new MapData(MapNames.Residence, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.Scriptorium, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Sepulchre, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Shore, MapType.Regular));
		MapList.Add(new MapData(MapNames.Shrine, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Siege, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Silo, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.SpiderLair, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Stagnation, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Strand, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.SulphurVents, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Summit, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.SunkenCity, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Temple, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Terrace, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Thicket, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.ToxicSewer, MapType.Regular));
		MapList.Add(new MapData(MapNames.TropicalIsland, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.UndergroundRiver, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.VaalPyramid, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.VaalTemple, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Vault, MapType.Bossroom)
		{
			Unsupported = true
		});
		MapList.Add(new MapData(MapNames.VaultsOfAtziri, MapType.Regular)
		{
			Ignored = true
		});
		MapList.Add(new MapData(MapNames.Villa, MapType.Multilevel));
		MapList.Add(new MapData(MapNames.WastePool, MapType.Bossroom));
		MapList.Add(new MapData(MapNames.Wasteland, MapType.Regular));
		MapList.Add(new MapData(MapNames.Waterways, MapType.Regular));
		MapList.Add(new MapData(MapNames.Wharf, MapType.Regular));
		MapList.Add(new MapData(MapNames.ObasCursedTrove, MapType.Multilevel));
	}

	private void InitDict()
	{
		foreach (MapData map in MapList)
		{
			MapDict.Add(map.Name, map);
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
			Dictionary<string, Class32> dictionary = JsonConvert.DeserializeObject<Dictionary<string, Class32>>(text);
			if (dictionary != null)
			{
				foreach (MapData map in MapList)
				{
					if (dictionary.TryGetValue(map.Name, out var value))
					{
						map.Priority = value.Priority;
						map.Ignored = value.Ignore;
						map.IgnoreBossroom = value.IgnoreBossroom;
						map.MobRemaining = value.MobRemaining;
						map.ExplorationPercent = value.ExplorationPercent;
						map.FinalPulse = ((value.FinalPulse == 0) ? (-1) : value.FinalPulse);
						map.FastTransition = value.FastTransition;
						map.BossRush = value.BossRush;
					}
				}
				return;
			}
			GlobalLog.Error("[MapBotEx] Fail to load \"MapSettings.json\". Json deserealizer returned null.");
		}
		else
		{
			GlobalLog.Error("[MapBotEx] Fail to load \"MapSettings.json\". File is empty.");
		}
	}

	private void Save()
	{
		Dictionary<string, Class32> dictionary = new Dictionary<string, Class32>(MapList.Count);
		foreach (MapData map in MapList)
		{
			Class32 value = new Class32
			{
				Priority = map.Priority,
				Ignore = map.Ignored,
				IgnoreBossroom = map.IgnoreBossroom,
				MobRemaining = map.MobRemaining,
				ExplorationPercent = map.ExplorationPercent,
				FinalPulse = map.FinalPulse,
				FastTransition = map.FastTransition,
				BossRush = map.BossRush
			};
			dictionary.Add(map.Name, value);
		}
		string contents = JsonConvert.SerializeObject((object)dictionary, (Formatting)1);
		File.WriteAllText(string_0, contents);
	}

	static MapSettings()
	{
		string_0 = Path.Combine(Configuration.Instance.Path, "MapBotEx", "MapSettings.json");
	}
}
