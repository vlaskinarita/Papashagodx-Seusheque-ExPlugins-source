using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.FilesInMemory;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.BlightPluginEx;
using ExPlugins.BlightPluginEx.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.MapBotEx;
using ExPlugins.PapashaCore;

namespace ExPlugins.SqRoutine;

public class SqRoutine : IRoutine, IAuthored, IBase, IConfigurable, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents, IUrlProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass98_0
	{
		public Monster monster_0;

		public Vector2i vector2i_0;

		internal bool method_0(string skillname, int range = 50, int mobsInRange = 10)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Invalid comparison between Unknown and I4
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Invalid comparison between Unknown and I4
			return (skillname.EqualsIgnorecase("molten shell") && (RemoteMemoryObject)(object)skill_41 != (RemoteMemoryObject)null && (NetworkObject)(object)monster_0 != (NetworkObject)null) || EsHealthCombinedPct < 60 || ((NetworkObject)(object)monster_0 != (NetworkObject)null && ((NetworkObject)monster_0).Distance <= (float)range && (int)monster_0.Rarity >= 2 && EsHealthCombinedPct < 85) || ((NetworkObject)(object)monster_0 != (NetworkObject)null && ((NetworkObject)monster_0).Distance <= (float)range && (int)monster_0.Rarity >= 2 && InstanceInfo.MonstersLevel >= 80) || NumberOfMobsNear((NetworkObject)(object)LokiPoe.Me, range) > mobsInRange;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass98_1
	{
		public Rarity rarity_0;

		public Vector2i vector2i_0;

		public _003C_003Ec__DisplayClass98_0 _003C_003Ec__DisplayClass98_0_0;

		internal bool _003CLogic_003Eb__138(SqRoutineSettings.VaalSkillEntry e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0 >= e.MinRarity;
		}

		internal bool _003CLogic_003Eb__139(SqRoutineSettings.VaalSkillEntry e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0 >= e.MinRarity;
		}

		internal bool _003CLogic_003Eb__142(SqRoutineSettings.TotemSkillEntry e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0 >= e.MinRarity;
		}

		internal bool _003CLogic_003Eb__143(SqRoutineSettings.TotemSkillEntry e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return rarity_0 >= e.MinRarity && e.UsageCase != SqRoutineSettings.TotemUsageCase.DontUse;
		}

		internal bool method_0(Monster m, bool lite = false)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (((NetworkObject)m).Id == ((NetworkObject)_003C_003Ec__DisplayClass98_0_0.monster_0).Id)
			{
				return false;
			}
			if (((NetworkObject)m).Name.Contains("Totem"))
			{
				return false;
			}
			if (m.IsActiveDead)
			{
				Vector2i position;
				if (lite)
				{
					position = ((NetworkObject)m).Position;
					return ((Vector2i)(ref position)).Distance(vector2i_0) < 25;
				}
				position = ((NetworkObject)m).Position;
				int result;
				if (((Vector2i)(ref position)).Distance(vector2i_0) < 30)
				{
					result = 1;
				}
				else
				{
					position = ((NetworkObject)m).Position;
					result = ((((Vector2i)(ref position)).Distance(_003C_003Ec__DisplayClass98_0_0.vector2i_0) < 30) ? 1 : 0);
				}
				return (byte)result != 0;
			}
			return false;
		}

		internal bool _003CLogic_003Eb__159(Monster m)
		{
			return method_0(m, lite: true);
		}

		internal bool _003CLogic_003Eb__162(Monster m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)m).Position;
			return ((Vector2i)(ref position)).Distance(vector2i_0) < 20;
		}

		internal bool _003CLogic_003Eb__163(Monster m)
		{
			return method_0(m);
		}

		internal bool _003CLogic_003Eb__165(Monster m)
		{
			return method_0(m, lite: true);
		}

		internal bool method_1(NetworkObject geyser)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if ((int)rarity_0 >= 2)
			{
				Vector2i position = geyser.Position;
				if (((Vector2i)(ref position)).Distance(vector2i_0) < 25)
				{
					return true;
				}
			}
			if (geyser.Distance > 60f)
			{
				return false;
			}
			return NumberOfMobsNear(geyser, 25f) > 2;
		}

		internal bool _003CLogic_003Eb__171(Monster m)
		{
			return method_0(m);
		}

		internal int _003CLogic_003Eb__172(Monster m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)m).Position;
			return ((Vector2i)(ref position)).Distance(vector2i_0);
		}

		internal int _003CLogic_003Eb__175(Effect g)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)g).Position;
			return ((Vector2i)(ref position)).Distance(vector2i_0);
		}

		internal int _003CLogic_003Eb__176(Effect m)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)m).Position;
			return ((Vector2i)(ref position)).Distance(vector2i_0);
		}
	}

	private static readonly SqRoutineSettings Config;

	private static int FwkghBykgT;

	private static readonly Interval interval_0;

	private static readonly Interval interval_1;

	private static readonly Interval interval_2;

	private static readonly Interval interval_3;

	private static readonly Interval interval_4;

	private static readonly Interval interval_5;

	private static readonly Interval interval_6;

	private static readonly HashSet<int> hashSet_0;

	private static readonly Dictionary<int, DateTime> dictionary_0;

	private static readonly Dictionary<int, DateTime> dictionary_1;

	private static readonly HashSet<int> hashSet_1;

	private bool bool_0;

	private static List<Skill> list_0;

	private static List<Skill> list_1;

	private static List<Skill> list_2;

	private static Skill skill_0;

	private static Skill skill_1;

	private static Skill skill_2;

	private static Skill skill_3;

	private static Skill skill_4;

	private static Skill skill_5;

	private static Skill skill_6;

	private static Skill skill_7;

	private static Skill skill_8;

	private static Skill skill_9;

	private static Skill skill_10;

	private static Skill skill_11;

	private static Skill skill_12;

	private static Skill skill_13;

	private static Skill skill_14;

	private static Skill skill_15;

	private static Skill skill_16;

	private static Skill skill_17;

	private static Skill skill_18;

	private static Skill skill_19;

	private static Skill skill_20;

	private static Skill skill_21;

	private static Skill skill_22;

	private static Skill skill_23;

	private static Skill skill_24;

	private static Skill skill_25;

	private static Skill skill_26;

	private static Skill skill_27;

	private static Skill skill_28;

	private static Skill skill_29;

	private static Skill skill_30;

	private static Skill skill_31;

	private static Skill skill_32;

	private static Skill skill_33;

	private static Skill skill_34;

	private static Skill skill_35;

	private static Skill skill_36;

	private static Skill skill_37;

	private static Skill skill_38;

	private static Skill skill_39;

	private static List<Skill> list_3;

	private static bool bool_1;

	private static bool bool_2;

	private static Skill skill_40;

	private static Skill skill_41;

	private static Skill skill_42;

	private static int int_0;

	private int int_1 = -1;

	private bool bool_3;

	private Dictionary<string, Func<Tuple<object, string>[], object>> dictionary_2;

	private Stopwatch stopwatch_0;

	private Stopwatch stopwatch_1;

	private Stopwatch stopwatch_2;

	private Stopwatch stopwatch_3;

	private Stopwatch stopwatch_4;

	private Stopwatch stopwatch_5;

	private Stopwatch stopwatch_6;

	private Stopwatch stopwatch_7;

	private Stopwatch stopwatch_8;

	private Stopwatch stopwatch_9;

	private Stopwatch stopwatch_10;

	private Stopwatch stopwatch_11;

	private Stopwatch stopwatch_12;

	private Stopwatch stopwatch_13;

	private Func<Rarity, int, bool> func_0;

	private Monster monster_0;

	private bool bool_4;

	private bool bool_5;

	private DateTime dateTime_0 = DateTime.Now;

	private bool bool_6;

	private bool bool_7 = true;

	private bool bool_8;

	private bool bool_9;

	private bool bool_10;

	[CompilerGenerated]
	private readonly Targeting targeting_0 = new Targeting();

	private readonly string[] string_0 = new string[4] { "shrine_godmode", "bloodlines_invulnerable", "god_mode", "bloodlines_necrovigil" };

	private SqRoutineGui vttGqkJgva;

	private static NetworkObject LegionMonolith => ObjectManager.GetObjectByMetadata("Metadata/Terrain/Leagues/Legion/Objects/LegionInitiator");

	private static NetworkObject CrucibleDevice => ObjectManager.GetObjectByMetadata("Metadata/Terrain/Leagues/Crucible/Objects/CrucibleDevice");

	private static CachedObject CachedMonolith
	{
		get
		{
			return CombatAreaCache.Current.Storage["Monolith"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Monolith"] = value;
		}
	}

	public Targeting CombatTargeting
	{
		[CompilerGenerated]
		get
		{
			return targeting_0;
		}
	}

	private static Monster BestDeadTarget => (from m in ObjectManager.GetObjectsByType<Monster>().ToHashSet()
		where ((NetworkObject)m).Distance < 40f && m.IsActiveDead && (int)m.Rarity != 3 && m.CorpseUsable && !((NetworkObject)m).Metadata.Contains("KitavaDemon") && !((NetworkObject)m).Metadata.Contains("KitavaCultist")
		orderby ExilePather.CanObjectSee((NetworkObject)(object)LokiPoe.Me, (NetworkObject)(object)m, false, false) descending, NumberOfMobsNear((NetworkObject)(object)m, 12f, dead: true) descending, ((NetworkObject)m).Distance
		select m).FirstOrDefault();

	private static int EsHealthCombinedPct
	{
		get
		{
			double num = ((Actor)LokiPoe.Me).EnergyShield + ((Actor)LokiPoe.Me).Health;
			double num2 = ((Actor)LokiPoe.Me).EnergyShieldMax + ((Actor)LokiPoe.Me).MaxHealth;
			return (int)Math.Round(num / num2 * 100.0, 0);
		}
	}

	public string Name => "SqRoutine";

	public string Description => "Advanced combat routine";

	public string Author => "Seusheque";

	public string Version => "4.2.4";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public JsonSettings Settings => (JsonSettings)(object)Config;

	public System.Windows.Controls.UserControl Control => vttGqkJgva ?? (vttGqkJgva = new SqRoutineGui());

	public async Task<LogicResult> Logic(Logic logic)
	{
		_003C_003Ec__DisplayClass98_0 _003C_003Ec__DisplayClass98_ = new _003C_003Ec__DisplayClass98_0();
		bool flag = bool_5;
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = await PlayerAction.Logout(toTitle: false);
		}
		int deployedSrs;
		int deployedPhantasm;
		Item writhingJarFlask;
		List<Monster> aliveWorms;
		bool needSpidersCyclone;
		List<Skill> questBotSkills;
		List<Skill> pointTargetedSkills;
		List<Skill> rarePlusSkills;
		_003C_003Ec__DisplayClass98_1 CS_0024_003C_003E8__locals2;
		string cachedMetadata;
		string cachedName;
		List<Aura> cachedAuras;
		int cachedDistance;
		bool canSee;
		int cachedCurseCount;
		bool cachedIsCursable;
		bool nonSim;
		int num2;
		if (!flag2)
		{
			if (!(DateTime.Now < dateTime_0))
			{
				if (!((IAuthored)BotManager.Current).Name.EqualsIgnorecase("FollowBot") || !World.CurrentArea.Name.EqualsIgnorecase("Domain of Timeless Conflict"))
				{
					PerformanceTimer timer = new PerformanceTimer("update", 1, (FinishedMeasuringCallback)null, (bool?)null);
					timer.Start();
					CombatTargeting.Update();
					timer.StopAndPrint();
					if (timer.ElapsedMilliseconds > 70L && Config.DebugMode)
					{
						GlobalLog.Warn($"[SqRoutine] Combat recalculation took too long! {timer.ElapsedMilliseconds}ms");
					}
					_003C_003Ec__DisplayClass98_.monster_0 = CombatTargeting.Targets<Monster>().FirstOrDefault();
					NetworkObject payload = ObjectManager.GetObjectByMetadata("Metadata/QuestObjects/Act6/BeaconPayload");
					bool skip = (Config.SkipNormalMobs && NumberOfMobsNear((NetworkObject)(object)LokiPoe.Me, 20f) < 10) || payload != (NetworkObject)null;
					if (World.CurrentArea.Id.Equals("2_6_1") || World.CurrentArea.Id.Equals("1_1_3a"))
					{
						skip = false;
					}
					if ((NetworkObject)(object)_003C_003Ec__DisplayClass98_.monster_0 != (NetworkObject)null && !CombatAreaCache.IsInHarvest && !CombatAreaCache.IsInIncursion && !World.CurrentArea.Id.Contains("Affliction") && skip && !Blight.IsEncounterRunning && (int)_003C_003Ec__DisplayClass98_.monster_0.Rarity < 1 && !((Actor)_003C_003Ec__DisplayClass98_.monster_0).IsStrongboxMinion && EsHealthCombinedPct > 90)
					{
						_003C_003Ec__DisplayClass98_.monster_0 = null;
					}
					_003C_003Ec__DisplayClass98_.vector2i_0 = LokiPoe.MyPosition;
					List<CachedWorldItem> validItems = CombatAreaCache.Current.Items.FindAll((CachedWorldItem i) => !i.Ignored && !i.Unwalkable && i.Position.Distance < 50);
					bool shouldLoot = Config.LootInCombat && validItems.Any();
					if (!(LocalData.MapMods.ContainsKey((StatTypeGGG)10342) || shouldLoot) || (BlightUi.IsOpened && !Blight.IsEncounterFinishedLootAvailable && !Blight.IsEncounterCompletedPumpDestroied && !Blight.IsEncounterFailed))
					{
						if (!bool_0 && list_1.Any())
						{
							SqRoutine.skill_0 = list_1.FirstOrDefault((Skill s) => s.InternalId == "call_of_steel");
							skill_1 = list_1.FirstOrDefault((Skill s) => s.InternalId == "flicker_strike");
							skill_2 = list_1.FirstOrDefault((Skill s) => s.InternalId == "blade_flurry");
							skill_3 = list_1.FirstOrDefault((Skill s) => s.InternalId == "snapping_adder" && !s.SkillType.Contains("vaal"));
							skill_4 = list_1.FirstOrDefault((Skill s) => (s.InternalId == "detonate_dead" || s.InternalId == "volatile_dead" || (s.InternalId == "bodyswap" && s.Supports.Count >= 3)) && !s.SkillType.Contains("vaal"));
							skill_5 = list_1.FirstOrDefault((Skill s) => s.InternalId == "circle_of_power");
							skill_6 = list_1.FirstOrDefault((Skill s) => s.Stats.ContainsKey((StatTypeGGG)7079));
							skill_7 = list_1.FirstOrDefault((Skill s) => s.InternalId == "blood_sand_stance");
							skill_8 = list_1.FirstOrDefault((Skill s) => s.Stats.ContainsKey((StatTypeGGG)15583));
							skill_9 = list_1.FirstOrDefault((Skill s) => s.InternalId == "dark_pact");
							skill_10 = list_1.FirstOrDefault((Skill s) => s.InternalId == "berserk");
							skill_11 = list_1.FirstOrDefault((Skill s) => s.InternalId == "divine_tempest");
							skill_12 = list_1.FirstOrDefault((Skill s) => s.InternalId == "corrosive_shroud");
							skill_13 = list_1.FirstOrDefault((Skill s) => s.InternalId == "righteous_fire");
							skill_14 = list_1.FirstOrDefault((Skill s) => s.SkillTags != null && s.SkillTags.Any((string t) => t.Contains("brand")));
							skill_15 = list_1.FirstOrDefault((Skill s) => s.InternalId == "wave_of_conviction");
							skill_16 = list_1.FirstOrDefault((Skill s) => s.InternalId == "cremation");
							skill_17 = list_1.FirstOrDefault((Skill s) => s.InternalId == "new_new_blade_vortex");
							skill_18 = list_1.FirstOrDefault((Skill s) => s.InternalId == "banner_armour_evasion");
							skill_19 = list_1.FirstOrDefault((Skill s) => s.InternalId == "banner_dread");
							skill_20 = list_1.FirstOrDefault((Skill s) => s.InternalId == "summon_raging_spirit");
							skill_21 = list_1.FirstOrDefault((Skill s) => s.Name == "Cyclone" && s.Stats.ContainsKey((StatTypeGGG)5300));
							skill_22 = list_1.FirstOrDefault((Skill s) => s.Name == "Cyclone");
							skill_23 = SqRoutine.list_0.FirstOrDefault((Skill s) => s.InternalId == "triggered_summon_phantasm");
							skill_24 = SqRoutine.list_0.FirstOrDefault((Skill s) => s.InternalId == "triggered_summon_spider");
							skill_25 = list_1.FirstOrDefault((Skill s) => s.InternalId == "summon_skeletons");
							skill_26 = list_1.FirstOrDefault((Skill s) => (!s.SkillType.Contains("vaal") && s.SkillTags != null && s.SkillTags.Any((string t) => t.EqualsIgnorecase("guard"))) || s.InternalId.Equals("bone_armour"));
							skill_27 = list_1.FirstOrDefault((Skill s) => s.Name == "Convocation");
							skill_28 = list_1.FirstOrDefault((Skill s) => s.Name == "Frost Bomb");
							skill_29 = list_1.FirstOrDefault((Skill s) => s.Name == "Signal Prey");
							skill_30 = list_1.FirstOrDefault((Skill s) => s.Name == "Sniper's Mark" || s.Name == "Assassin's Mark");
							skill_31 = list_1.FirstOrDefault((Skill s) => s.Name == "Desecrate");
							skill_32 = list_1.FirstOrDefault((Skill s) => s.InternalId == "unearth");
							skill_33 = list_1.FirstOrDefault((Skill s) => s.Name == "Flesh Offering" || s.Name == "Bone Offering" || s.Name == "Spirit Offering");
							skill_34 = list_1.FirstOrDefault((Skill s) => s.Name == "Flesh Offering");
							skill_35 = list_1.FirstOrDefault((Skill s) => s.Name == "Bone Offering");
							skill_36 = list_1.FirstOrDefault((Skill s) => s.Name == "Spirit Offering");
							skill_37 = list_1.FirstOrDefault((Skill s) => s.Name == "Corrupting Fever");
							skill_38 = list_1.FirstOrDefault((Skill s) => s.Name == "Blood Rage");
							SqRoutine.list_3 = list_1.Where((Skill s) => s.SkillTags != null && s.SkillTags.Any((string t) => t.Equals("warcry"))).ToList();
							skill_39 = list_1.FirstOrDefault((Skill s) => s.Name == "Tornado");
							skill_40 = list_1.FirstOrDefault((Skill s) => s.Name == "Vaal Discipline");
							skill_41 = list_1.FirstOrDefault((Skill s) => s.Name == "Vaal Molten Shell");
							skill_42 = list_1.FirstOrDefault((Skill s) => s.Name == "Vaal Summon Skeletons");
							bool_2 = InventoryUi.AllInventoryControls.Any((InventoryControlWrapper x) => x.Inventory.Items.Any((Item i) => i.FullName == "Leash of Oblation"));
							bool_1 = InventoryUi.AllInventoryControls.Any((InventoryControlWrapper x) => (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main && (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryMainHand && (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryOffHand && x.Inventory.Items.Any((Item i) => i.FullName == "Soulwrest"));
							bool_0 = true;
							List<Skill> list = new List<Skill>();
							list.Add(SqRoutine.skill_0);
							list.Add(skill_1);
							list.Add(skill_2);
							list.Add(skill_3);
							list.Add(skill_4);
							list.Add(skill_5);
							list.Add(skill_7);
							list.Add(skill_8);
							list.Add(skill_9);
							list.Add(skill_10);
							list.Add(skill_11);
							list.Add(skill_12);
							list.Add(skill_13);
							list.Add(skill_14);
							list.Add(skill_15);
							list.Add(skill_16);
							list.Add(skill_17);
							list.Add(skill_18);
							list.Add(skill_19);
							list.Add(skill_20);
							list.Add(skill_21);
							list.Add(skill_22);
							list.Add(skill_23);
							list.Add(skill_24);
							list.Add(skill_25);
							list.Add(skill_26);
							list.Add(skill_27);
							list.Add(skill_28);
							list.Add(skill_29);
							list.Add(skill_30);
							list.Add(skill_31);
							list.Add(skill_32);
							list.Add(skill_33);
							list.Add(skill_34);
							list.Add(skill_35);
							list.Add(skill_36);
							list.Add(skill_37);
							list.Add(skill_38);
							list.Add(skill_39);
							SqRoutine.list_2 = list;
							SqRoutine.list_2.AddRange(SqRoutine.list_3);
							SqRoutine.list_2 = SqRoutine.list_2.Where((Skill s) => (RemoteMemoryObject)(object)s != (RemoteMemoryObject)null).ToList();
						}
						deployedSrs = ((!((RemoteMemoryObject)(object)skill_20 != (RemoteMemoryObject)null)) ? 20 : skill_20.NumberDeployed);
						deployedPhantasm = (((RemoteMemoryObject)(object)skill_23 != (RemoteMemoryObject)null) ? skill_23.NumberDeployed : 20);
						int deployedSpiders = (((RemoteMemoryObject)(object)skill_24 != (RemoteMemoryObject)null) ? skill_24.NumberDeployed : 20);
						int deployedSkeletons = (((RemoteMemoryObject)(object)skill_25 != (RemoteMemoryObject)null) ? skill_25.NumberDeployed : 0);
						int deployedVaalSkeletons = (((RemoteMemoryObject)(object)skill_42 != (RemoteMemoryObject)null) ? skill_42.NumberDeployed : 0);
						int maxPhantasms = (((RemoteMemoryObject)(object)skill_23 != (RemoteMemoryObject)null) ? skill_23.GetStat((StatTypeGGG)8371) : 0);
						int maxSkeletons = (((RemoteMemoryObject)(object)skill_25 != (RemoteMemoryObject)null) ? skill_25.GetStat((StatTypeGGG)727) : 0);
						writhingJarFlask = QuickFlaskHud.InventoryControl.Inventory.Items.FirstOrDefault((Item f) => f.FullName == "The Writhing Jar");
						aliveWorms = (from m in ObjectManager.GetObjectsByType<Monster>()
							where m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"
							select m).ToList();
						bool shouldCast = (RemoteMemoryObject)(object)skill_33 != (RemoteMemoryObject)null && ((Actor)LokiPoe.Me).Auras.All((Aura a) => a.InternalName != "active_offering");
						bool needSpidersHardCast = deployedSpiders < 17 && (RemoteMemoryObject)(object)writhingJarFlask != (RemoteMemoryObject)null && writhingJarFlask.CurrentCharges > writhingJarFlask.ChargesPerUse + 10 && (RemoteMemoryObject)(object)skill_21 == (RemoteMemoryObject)null;
						needSpidersCyclone = deployedSpiders < 17 && (RemoteMemoryObject)(object)writhingJarFlask != (RemoteMemoryObject)null && writhingJarFlask.CurrentCharges > writhingJarFlask.ChargesPerUse + 10 && (RemoteMemoryObject)(object)skill_21 != (RemoteMemoryObject)null;
						questBotSkills = list_1.Where((Skill s) => s.InternalId == "glacial_hammer" || s.InternalId == "viper_strike" || s.InternalId == "heavy_strike" || s.InternalId == "spectral_throw" || s.InternalId == "burning_arrow" || s.InternalId == "double_strike" || s.InternalId == "spark" || s.InternalId == "fireball" || s.InternalId == "melee").ToList();
						pointTargetedSkills = list_1.FindAll((Skill s) => !s.SkillType.Contains("vaal")).FindAll((Skill s) => s.Stats.ContainsKey((StatTypeGGG)12411) || s.InternalId == "freezing_pulse" || s.InternalId == "snapping_adder" || s.InternalId == "blazing_salvo" || s.InternalId == "frost_bolt" || s.InternalId == "splitting_steel" || s.InternalId == "lancing_steel_new" || s.InternalId == "shattering_steel" || s.InternalId == "melee");
						rarePlusSkills = new List<Skill>();
						if (Config.DefaultSkillEnabled)
						{
							IEnumerable<Skill> toAdd = list_1.Where((Skill s) => !SqRoutine.list_2.Contains(s) && Config.DefaultSkill.ContainsIgnorecase(s.Name));
							pointTargetedSkills.AddRange(toAdd);
						}
						if (Config.RarePlusSkillEnabled)
						{
							IEnumerable<Skill> toAdd2 = list_1.Where((Skill s) => !SqRoutine.list_2.Contains(s) && Config.RarePlusSkill.ContainsIgnorecase(s.Name));
							rarePlusSkills.AddRange(toAdd2);
						}
						if (!bool_7 || !((RemoteMemoryObject)(object)skill_25 != (RemoteMemoryObject)null) || skill_25.Stats.ContainsKey((StatTypeGGG)174) || skill_25.Stats.ContainsKey((StatTypeGGG)6235))
						{
							if (!bool_7 || !((RemoteMemoryObject)(object)skill_25 != (RemoteMemoryObject)null))
							{
								if (logic.Id == "hook_combat")
								{
									CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass98_1();
									CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0 = _003C_003Ec__DisplayClass98_;
									if (!SqRoutine.list_0.Any())
									{
										SqRoutine.list_0 = SkillBarHud.Skills.Where((Skill s) => (RemoteMemoryObject)(object)s != (RemoteMemoryObject)null).ToList();
									}
									if (!list_1.Any())
									{
										list_1 = SqRoutine.list_0.FindAll((Skill s) => s.IsCastable && !s.SkillTags.Contains("totem") && !s.Stats.ContainsKey((StatTypeGGG)2330));
									}
									if (interval_4.Elapsed && (RemoteMemoryObject)(object)skill_7 != (RemoteMemoryObject)null)
									{
										bool bloodStance = ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("blood_stance"));
										bool sandStance = ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("sand_stance"));
										if (sandStance || bloodStance)
										{
											Monster rareNearby = ObjectManager.GetObjectsByType<Monster>().FirstOrDefault((Monster m) => m.IsAliveHostile && ((NetworkObject)m).Distance < 110f && (int)m.Rarity >= 3);
											if (!bloodStance && (NetworkObject)(object)rareNearby != (NetworkObject)null)
											{
												UseResult useResult11 = await MethodExtensions.SqUse(skill_7);
												GlobalLog.Warn($"[{Name}] {((NetworkObject)rareNearby).Name}:{rareNearby.Rarity} nearby. Switching to Blood stance.");
												if ((int)useResult11 == 0)
												{
													return (LogicResult)0;
												}
											}
											if (!sandStance && (NetworkObject)(object)rareNearby == (NetworkObject)null)
											{
												UseResult useResult12 = await MethodExtensions.SqUse(skill_7);
												GlobalLog.Warn("[" + Name + "] No Unique mobs nearby. Switching to Sand stance.");
												if ((int)useResult12 == 0)
												{
													return (LogicResult)0;
												}
											}
										}
									}
									if (Config.TotemSkillsList.Any((SqRoutineSettings.TotemSkillEntry e) => e.Every3Sec))
									{
										List<string> list_3 = (from e in Config.TotemSkillsList
											where e.Every3Sec && e.UsageCase != SqRoutineSettings.TotemUsageCase.DontUse
											select e into s
											select s.Name).ToList();
										List<Skill> usable3 = SqRoutine.list_0.Where((Skill s) => list_3.Contains(s.Name) && s.CanUse(false, false, true) && (s.SkillTags.Contains("totem") || s.Stats.ContainsKey((StatTypeGGG)2330))).ToList();
										if (usable3.Any() && CombatAreaCache.Current.Monsters.Any())
										{
											Skill skill_ = usable3.OrderBy((Skill t) => t.NumberDeployed).First();
											int farTotems = skill_.DeployedObjects.Count((NetworkObject t) => t.Distance > (float)Config.CombatRange);
											int deployed = skill_.NumberDeployed;
											DateTime lastUsed = Config.TotemSkillsList.FirstOrDefault((SqRoutineSettings.TotemSkillEntry s) => s.Name == skill_.Name).LastUsed;
											if ((deployed == 0 || farTotems != 0) && lastUsed.AddSeconds(3.0) < DateTime.Now && (int)(await MethodExtensions.SqUse(skill_)) == 0)
											{
												Config.TotemSkillsList.FirstOrDefault((SqRoutineSettings.TotemSkillEntry s) => s.Name == skill_.Name).LastUsed = DateTime.Now;
												return (LogicResult)0;
											}
										}
									}
									if (Config.VaalSkillsList.Any((SqRoutineSettings.VaalSkillEntry e) => e.SoulEater) && InventoryUi.AllInventoryControls.Any((InventoryControlWrapper x) => (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main && x.Inventory.Items.Any((Item i) => i.FullName == "Zerphi's Heart")) && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("soul_eater_aura")))
									{
										List<string> list_2 = (from e in Config.VaalSkillsList
											where e.SoulEater
											select e into s
											select s.Name).ToList();
										List<Skill> usable4 = SqRoutine.list_0.Where((Skill s) => list_2.Contains(s.Name) && s.CanUse(false, false, true)).ToList();
										if (usable4.Any())
										{
											Vector2i rand2 = new Vector2i(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.X + LokiPoe.Random.Next(-10, 10), CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.Y + LokiPoe.Random.Next(-10, 10));
											int index3 = LokiPoe.Random.Next(usable4.Count);
											Skill rnd = usable4[index3];
											GlobalLog.Warn("[" + Name + "] We can use vaal skill: " + rnd.Name + " (Zerphi's Heart)");
											await MethodExtensions.SqUseAt(rnd, rand2);
										}
									}
									if (skill_6.SqCanUse())
									{
										Aura archerAura = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName.Equals("mirage_archer_visual_buff"));
										if ((RemoteMemoryObject)(object)archerAura == (RemoteMemoryObject)null || archerAura.TimeLeft.TotalMilliseconds < 1500.0)
										{
											Monster anyMobNear = CombatTargeting.Targets<Monster>().FirstOrDefault((Monster m) => ExilePather.CanObjectSee((NetworkObject)(object)LokiPoe.Me, (NetworkObject)(object)m, false, false));
											if ((NetworkObject)(object)anyMobNear != (NetworkObject)null && (int)(await MethodExtensions.SqUseAt(skill_6, ((NetworkObject)anyMobNear).Position)) == 0)
											{
												return (LogicResult)0;
											}
										}
									}
									if (skill_12.SqCanUse() && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("corrosive_shroud_buff")) && (int)(await MethodExtensions.SqUse(skill_12)) == 0)
									{
										return (LogicResult)0;
									}
									if (skill_8.SqCanUse())
									{
										string string_0 = skill_8.Name;
										if ((!((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.Name.EqualsIgnorecase(string_0) || a.Name.EqualsIgnorecase(string_0 + " aura")) || ((Actor)LokiPoe.Me).Auras.Any((Aura x) => (x.Name.EqualsIgnorecase(string_0) || x.Name.EqualsIgnorecase(string_0 + " aura")) && x.TimeLeft.TotalSeconds <= 3.0)) && (int)(await MethodExtensions.SqUse(skill_8)) == 0)
										{
											return (LogicResult)0;
										}
									}
									if (skill_12.SqCanUse())
									{
										Aura bearerBuff = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName.Equals("corrosive_shroud_buff"));
										Aura bearerAura = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName.Equals("corrosive_shroud_aura"));
										if ((RemoteMemoryObject)(object)bearerAura == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)bearerBuff != (RemoteMemoryObject)null && (bearerBuff.StacksPct > 95 || ((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 != (NetworkObject)null && (int)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0.Rarity >= 2 && bearerBuff.StacksPct > 60)) && (int)(await MethodExtensions.SqUse(skill_12)) == 0)
										{
											return (LogicResult)0;
										}
									}
									if (skill_3.SqCanUse() && interval_2.Elapsed)
									{
										Aura aura2 = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName == "snapping_adder_projectile");
										if ((RemoteMemoryObject)(object)aura2 != (RemoteMemoryObject)null && aura2.Charges > 16 && aura2.TimeLeft.TotalSeconds < 7.0 && (int)(await MethodExtensions.SqUseAt(pos: new Vector2i(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.X + LokiPoe.Random.Next(-10, 10), CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.Y + LokiPoe.Random.Next(-10, 10)), skill: skill_3)) == 0)
										{
											return (LogicResult)0;
										}
									}
									if (SqRoutine.skill_0.SqCanUse() && interval_3.Elapsed)
									{
										Aura aura3 = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName == "steel_skill_ammo_buff");
										if (((RemoteMemoryObject)(object)aura3 == (RemoteMemoryObject)null || aura3.Charges < 4) && (int)(await MethodExtensions.SqUse(SqRoutine.skill_0)) == 0)
										{
											return (LogicResult)0;
										}
									}
									if (skill_13.SqCanUse() && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("righteous_fire")) && (int)(await MethodExtensions.SqUse(skill_13)) == 0)
									{
										return (LogicResult)0;
									}
									if (skill_20.SqCanUse() && (deployedSrs < 2 || stopwatch_2.ElapsedMilliseconds > 5500L) && (int)(await MethodExtensions.SqUse(skill_20)) == 0)
									{
										stopwatch_2.Restart();
										return (LogicResult)0;
									}
									int precastInt;
									int skellyInt;
									if ((Blight.IsEncounterRunning && !Blight.IsEncounterFinishedLootAvailable) || LokiPoe.CurrentWorldArea.Name == "Aspirant's Trial")
									{
										precastInt = maxPhantasms - 2;
										skellyInt = maxSkeletons - 2;
									}
									else
									{
										precastInt = maxPhantasms - 11;
										skellyInt = ((!Config.SkipNormalMobs || needSpidersCyclone || needSpidersHardCast) ? (bool_8 ? 4 : 2) : 0);
									}
									if (skill_37.SqCanUse())
									{
										bool hasBuff = ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.Name.ContainsIgnorecase("corrupting fever"));
										bool champAdrenaline = ((Actor)LokiPoe.Me).Stats.ContainsKey((StatTypeGGG)9654);
										bool hasAdrenaline = ((Actor)LokiPoe.Me).Auras.Any((Aura x) => x.InternalName == "adrenaline");
										bool lowLife = ((Actor)LokiPoe.Me).HealthPercent >= 51f;
										if (champAdrenaline && !lowLife)
										{
											if ((!hasBuff || !hasAdrenaline) && (int)(await MethodExtensions.SqUse(skill_37)) == 0)
											{
												return (LogicResult)0;
											}
										}
										else if (!hasBuff && (int)(await MethodExtensions.SqUse(skill_37)) > 0)
										{
											return (LogicResult)1;
										}
									}
									if ((RemoteMemoryObject)(object)skill_23 != (RemoteMemoryObject)null && bool_1)
									{
										if (deployedPhantasm < precastInt)
										{
											shouldCast = true;
										}
										if (deployedPhantasm < maxPhantasms && (NetworkObject)(object)BestDeadTarget != (NetworkObject)null)
										{
											shouldCast = true;
										}
										if ((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 != (NetworkObject)null && (int)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0.Rarity >= 2 && deployedPhantasm < maxPhantasms - 3)
										{
											shouldCast = true;
										}
										if ((RemoteMemoryObject)(object)skill_21 != (RemoteMemoryObject)null)
										{
											shouldCast = false;
										}
									}
									if ((RemoteMemoryObject)(object)skill_21 == (RemoteMemoryObject)null && bool_2 && stopwatch_12.ElapsedMilliseconds > 2000L)
									{
										if (!((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Bone Offering") == (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)skill_35 != (RemoteMemoryObject)null))
										{
											if (!((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Flesh Offering") == (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)skill_34 != (RemoteMemoryObject)null))
											{
												if ((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Spirit Offering") == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)skill_36 != (RemoteMemoryObject)null)
												{
													shouldCast = true;
												}
											}
											else
											{
												shouldCast = true;
											}
										}
										else
										{
											shouldCast = true;
										}
									}
									if (skill_18.SqCanUse() && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("armour_evasion_banner_buff_aura")))
									{
										await MethodExtensions.SqUse(skill_18);
									}
									if (skill_19.SqCanUse() && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("puresteel_banner_buff_aura")))
									{
										await MethodExtensions.SqUse(skill_19);
									}
									if (!skill_26.SqCanUse() || skill_26.SkillActiveToken != 0 || ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.Name.Equals("Vaal Molten Shell")) || !CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.method_0(skill_26.Name) || (int)(await MethodExtensions.SqUse(skill_26)) != 0)
									{
										if (!skill_41.SqCanUse() || ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.Name.Equals("Molten Shell")) || !CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.method_0(skill_41.Name) || (int)(await MethodExtensions.SqUse(skill_41)) != 0)
										{
											if (skellyInt == 0 || !skill_25.SqCanUse() || deployedSkeletons >= skellyInt || (int)(await MethodExtensions.SqUseAt(pos: new Vector2i(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.X + LokiPoe.Random.Next(-10, 10), CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0.Y + LokiPoe.Random.Next(-10, 10)), skill: skill_25)) != 0)
											{
												if ((RemoteMemoryObject)(object)skill_17 != (RemoteMemoryObject)null && ((Actor)LokiPoe.Me).BladeVortexCharges < 1 && (int)(await MethodExtensions.SqUse(skill_17)) == 0)
												{
													return (LogicResult)0;
												}
												if (skill_9.SqCanUse())
												{
													aliveWorms = (from m in ObjectManager.GetObjectsByType<Monster>()
														where m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"
														select m).ToList();
													if (aliveWorms.Any() && (int)(await MethodExtensions.SqUseAt(pos: ((NetworkObject)aliveWorms.FirstOrDefault()).Position, skill: skill_9)) == 0)
													{
														return (LogicResult)0;
													}
												}
												if (skill_11.SqCanUse())
												{
													aliveWorms = (from m in ObjectManager.GetObjectsByType<Monster>()
														where m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"
														select m).ToList();
													if (aliveWorms.Any())
													{
														Vector2i wormPos = ((NetworkObject)aliveWorms.FirstOrDefault()).Position;
														if (!((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("divine_tempest_stage") && a.Charges > 5))
														{
															UseResult useResult15 = MethodExtensions.SqBeginUseAt(skill_11, wormPos);
															await Wait.LatencySleep();
															await Wait.SleepSafe(500);
															if ((int)useResult15 == 0)
															{
																return (LogicResult)0;
															}
														}
													}
												}
												if (shouldCast || needSpidersHardCast)
												{
													await Coroutines.CloseBlockingWindows();
													if (skill_31.SqCanUse(ignoreSkillbar: true) && ((NetworkObject)(object)BestDeadTarget == (NetworkObject)null || ((NetworkObject)(object)BestDeadTarget != (NetworkObject)null && NumberOfMobsNear((NetworkObject)(object)BestDeadTarget, 25f, dead: true) < 7 && needSpidersHardCast)))
													{
														if (skill_31.Slot == -1)
														{
															GlobalLog.Debug(string.Format(arg2: SkillBarHud.SetSlot(GeneralSettings.Instance.AuraSwapSlot, skill_31), format: "[{0}] {1} is not on skillbar, set slot result: {2}", arg0: Name, arg1: skill_31.Name));
															return (LogicResult)0;
														}
														UseResult useResult16 = await MethodExtensions.SqUseAt(pos: ((NetworkObject)LokiPoe.Me).Position + new Vector2i(LokiPoe.Random.Next(-10, 10), LokiPoe.Random.Next(-10, 10)), skill: skill_31);
														if ((int)useResult16 > 0)
														{
															GlobalLog.Error($"[{Name}] desecrate usage error: {useResult16}");
															return (LogicResult)0;
														}
													}
													if (needSpidersHardCast && !aliveWorms.Any())
													{
														int flaskSlot2 = writhingJarFlask.LocationTopLeft.X + 1;
														if (writhingJarFlask.CurrentCharges > writhingJarFlask.ChargesPerUse + 10)
														{
															QuickFlaskHud.UseFlaskInSlot(flaskSlot2);
															await Wait.For(() => ObjectManager.GetObjectsByType<Monster>().Any((Monster m) => m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"), "worms", 20, 500, Config.DebugMode);
															await Wait.LatencySleep(longer: true);
															return (LogicResult)0;
														}
													}
													if (bool_2)
													{
														if (!((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Bone Offering") == (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)skill_35 != (RemoteMemoryObject)null))
														{
															if (!((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Flesh Offering") == (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)skill_34 != (RemoteMemoryObject)null))
															{
																if ((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Spirit Offering") == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)skill_36 != (RemoteMemoryObject)null)
																{
																	skill_33 = skill_36;
																}
															}
															else
															{
																skill_33 = skill_34;
															}
														}
														else
														{
															skill_33 = skill_35;
														}
													}
													if (FwkghBykgT < 20 && skill_33.SqCanUse(ignoreSkillbar: true) && (NetworkObject)(object)BestDeadTarget != (NetworkObject)null && !needSpidersHardCast)
													{
														Vector2i walkPos = ((NetworkObject)BestDeadTarget).Position;
														if (((Vector2i)(ref walkPos)).Distance(LokiPoe.MyPosition) < 40)
														{
															if (skill_33.Slot == -1)
															{
																GlobalLog.Debug(string.Format(arg2: SkillBarHud.SetSlot(GeneralSettings.Instance.AuraSwapSlot, skill_33), format: "[{0}] {1} is not on skillbar, set slot result: {2}", arg0: Name, arg1: skill_33.Name));
																return (LogicResult)0;
															}
															UseResult useResult17 = await MethodExtensions.SqUseAt(skill_33, walkPos);
															if ((RemoteMemoryObject)(object)skill_31 == (RemoteMemoryObject)null)
															{
																FwkghBykgT++;
															}
															if ((int)useResult17 == 0)
															{
																stopwatch_12.Restart();
																return (LogicResult)0;
															}
														}
														walkPos = default(Vector2i);
													}
												}
												if ((needSpidersHardCast || needSpidersCyclone) && (NetworkObject)(object)BestDeadTarget == (NetworkObject)null && skill_31.SqCanUse() && (int)(await MethodExtensions.SqUseAt(skill_31, LokiPoe.MyPosition)) == 0 && !(await Wait.For(() => (NetworkObject)(object)BestDeadTarget != (NetworkObject)null, "corpses to appear", 20, 1000, Config.DebugMode)))
												{
													return (LogicResult)0;
												}
												if (needSpidersCyclone && (NetworkObject)(object)BestDeadTarget != (NetworkObject)null && !CombatAreaCache.Current.Monsters.Any((CachedMonster m) => m.Position.Distance < 30) && (RemoteMemoryObject)(object)writhingJarFlask != (RemoteMemoryObject)null && CombatAreaCache.Current.Monsters.All((CachedMonster m) => m.Name != "Writhing Worm"))
												{
													int flaskSlot3 = writhingJarFlask.LocationTopLeft.X + 1;
													if (writhingJarFlask.CurrentCharges > writhingJarFlask.ChargesPerUse + 10 && QuickFlaskHud.UseFlaskInSlot(flaskSlot3) && await Wait.For(() => CombatAreaCache.Current.Monsters.Any((CachedMonster m) => m.Name == "Writhing Worm"), "worms", 20, 500, Config.DebugMode))
													{
														await Wait.LatencySleep(longer: true);
														aliveWorms = (from m in ObjectManager.GetObjectsByType<Monster>()
															where m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"
															select m).ToList();
														CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 = aliveWorms.FirstOrDefault();
													}
												}
												if (!skill_42.SqCanUse() || deployedVaalSkeletons >= 18 || ((!Blight.IsEncounterRunning || global::ExPlugins.BlightPluginEx.BlightPluginEx.CachedBlightCore.Position.Distance >= 30) && (!((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 != (NetworkObject)null) || (int)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0.Rarity != 3)) || (int)(await MethodExtensions.SqUse(skill_42)) != 0)
												{
													if (CachedMonolith != null && VisibleTimersUi.IsOpened && VisibleTimersUi.Timer.TotalSeconds < 4.0 && CachedMonolith.Position.PathExists)
													{
														GlobalLog.Warn("[" + Name + "] Getting closer to Monolith before event starts.");
														if (CachedMonolith.Position.IsFar)
														{
															CachedMonolith.Position.TryCome();
														}
														if ((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 == (NetworkObject)null)
														{
															return (LogicResult)0;
														}
													}
													if (!((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 == (NetworkObject)null))
													{
														if (Blight.IsEncounterRunning && VisibleTimersUi.IsOpened && HandleEventTask.EncounterTimeoutSwSmall != null)
														{
															HandleEventTask.EncounterTimeoutSwSmall.Restart();
														}
														CS_0024_003C_003E8__locals2.rarity_0 = CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0.Rarity;
														cachedMetadata = ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Metadata;
														bool skipPathing = (int)CS_0024_003C_003E8__locals2.rarity_0 == 3 && !cachedMetadata.Contains("KitavaBoss/KitavaFinalHeart") && !cachedMetadata.Contains("KitavaBoss/KitavaHeart") && (cachedMetadata.Contains("KitavaBoss/Kitava") || cachedMetadata.Contains("KitavaFinal") || cachedMetadata.Contains("VaalSpiderGod/Arakaali"));
														CS_0024_003C_003E8__locals2.vector2i_0 = ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Position;
														if (skipPathing)
														{
															int offset2 = LokiPoe.Random.Next(-6, 6);
															CS_0024_003C_003E8__locals2.vector2i_0 = new Vector2i(((NetworkObject)LokiPoe.Me).Position.X + offset2, ((NetworkObject)LokiPoe.Me).Position.Y + offset2);
														}
														cachedName = ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Name;
														cachedAuras = ((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Auras.ToList();
														cachedDistance = ((Vector2i)(ref CS_0024_003C_003E8__locals2.vector2i_0)).Distance(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0);
														float pathDistance = ExilePather.PathDistance(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0, CS_0024_003C_003E8__locals2.vector2i_0, false, false);
														canSee = ExilePather.CanObjectSee((NetworkObject)(object)LokiPoe.Me, (NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0, false, false);
														bool blockedByDoor = ClosedDoorBetween((NetworkObject)(object)LokiPoe.Me, (NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0);
														cachedCurseCount = ((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).CurseCount;
														cachedIsCursable = CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0.IsCursable;
														int cachedHealth = ((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Health;
														int cachedMaxHealth = ((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).MaxHealth;
														if (cachedDistance < 10)
														{
															canSee = true;
														}
														bool canMove = true;
														if (skill_40.SqCanUse() && (CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.method_0(skill_40.Name) || (int)CS_0024_003C_003E8__locals2.rarity_0 == 3))
														{
															await MethodExtensions.SqUse(skill_40);
														}
														if (LocalData.WorldArea.Id == "2_6_14")
														{
															List<NetworkObject> fuelCarts = ObjectManager.GetObjectsByMetadata("Metadata/QuestObjects/Act6/BeaconPayload").ToList();
															using IEnumerator<NetworkObject> enumerator = fuelCarts.Where((NetworkObject fuelCart) => fuelCart.Components.TransitionableComponent.Flag2 == 1 && fuelCart.Distance < 30f).GetEnumerator();
															if (enumerator.MoveNext())
															{
																NetworkObject fuelCart2 = enumerator.Current;
																canMove = false;
																if (stopwatch_6.ElapsedMilliseconds > 3000L)
																{
																	await fuelCart2.WalkablePosition().ComeAtOnce(5);
																	stopwatch_6.Restart();
																	dateTime_0 = DateTime.Now + TimeSpan.FromMilliseconds(2000.0);
																	ProcessHookManager.ClearAllKeyStates();
																	return (LogicResult)0;
																}
															}
														}
														if (cachedDistance <= 10 || skipPathing || ExilePather.PathExistsBetween(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0, CS_0024_003C_003E8__locals2.vector2i_0, false))
														{
															if (pathDistance > (float)Config.CombatRange && !skipPathing && !((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Metadata.Contains("Metadata/Monsters/Tukohama/TukohamaShieldTotem"))
															{
																if ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || dictionary_0.ContainsKey(((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Id))
																{
																	return (LogicResult)1;
																}
																dictionary_0.Add(((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Id, DateTime.Now);
															}
															if (!((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0 != (NetworkObject)(object)monster_0))
															{
																if (stopwatch_5.Elapsed.TotalSeconds > 5.0)
																{
																	if (cachedMaxHealth - cachedHealth == 0)
																	{
																		if (Config.DebugMode)
																		{
																			GlobalLog.Error("[" + Name + "] Seems like we can't deal any damage to " + cachedName + ". Ignoring");
																		}
																		hashSet_0.Add(((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Id);
																		return (LogicResult)0;
																	}
																	if (stopwatch_5.ElapsedMilliseconds > 15000L && (int)CS_0024_003C_003E8__locals2.rarity_0 < 2)
																	{
																		if (Config.DebugMode)
																		{
																			GlobalLog.Error("[" + Name + "] Seems like we can't deal any damage to " + cachedName + " [Timer]. Ignoring");
																		}
																		hashSet_0.Add(((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Id);
																		return (LogicResult)0;
																	}
																}
															}
															else
															{
																if (Config.DebugMode)
																{
																	GlobalLog.Debug($"[{Name}] Best target: {((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).WalkablePosition()} canSee: {canSee}");
																}
																monster_0 = CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0;
																stopwatch_5.Restart();
															}
															if (skill_38.SqCanUse() && !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.Name.ContainsIgnorecase("blood rage")))
															{
																await MethodExtensions.SqUse(skill_38);
															}
															if (Config.KiteMobs && stopwatch_10.ElapsedMilliseconds > 450L && !World.Act6.Beacon.IsCurrentArea && !World.Act6.KaruiFortress.IsCurrentArea && !((Actor)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).HasProxShieldEx() && !questBotSkills.Any((Skill s) => s.SkillTags.Contains("melee")) && !Blight.IsEncounterRunning && ObjectManager.GetObjectsByType<Monster>().Any((Monster m) => (int)m.Rarity >= 2 && m.IsAliveHostile && ((NetworkObject)m).Distance < 35f))
															{
																if (!canMove)
																{
																	GlobalLog.Info("[" + Name + "-Kite] Not moving towards the target because we should not move currently.");
																	return (LogicResult)1;
																}
																Vector2i kitePos = new Vector2i(FindCombatPosition(CS_0024_003C_003E8__locals2.vector2i_0, CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0));
																GlobalLog.Warn($"[{Name}-Kite] Kiting to pos:{kitePos}. Distance: {((Vector2i)(ref kitePos)).Distance(CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0)}");
																ProcessHookManager.ClearAllKeyStates();
																if (!PlayerMoverManager.Current.MoveTowards(kitePos, Array.Empty<object>()))
																{
																	GlobalLog.Error($"[{Name}] MoveTowards failed for {kitePos}.");
																	return (LogicResult)1;
																}
																stopwatch_10.Restart();
																return (LogicResult)0;
															}
															if ((!canSee && !skipPathing) || blockedByDoor)
															{
																if (!canMove)
																{
																	if (Config.DebugMode)
																	{
																		GlobalLog.Debug("[" + Name + "] Not moving towards the target because we should not move currently.");
																	}
																	return (LogicResult)1;
																}
																if (!PlayerMoverManager.Current.MoveTowards(CS_0024_003C_003E8__locals2.vector2i_0, Array.Empty<object>()))
																{
																	GlobalLog.Error($"[{Name}] MoveTowards failed for {CS_0024_003C_003E8__locals2.vector2i_0}.");
																	await Coroutines.FinishCurrentAction(true);
																}
																return (LogicResult)0;
															}
															if (!skill_27.SqCanUse() || (int)(await MethodExtensions.SqUse(skill_27)) != 0)
															{
																if (shouldCast || needSpidersHardCast)
																{
																	await Coroutines.CloseBlockingWindows();
																	if (skill_31.SqCanUse(ignoreSkillbar: true) && ((NetworkObject)(object)BestDeadTarget == (NetworkObject)null || ((NetworkObject)(object)BestDeadTarget != (NetworkObject)null && NumberOfMobsNear((NetworkObject)(object)BestDeadTarget, 25f, dead: true) < 7 && needSpidersHardCast)))
																	{
																		if (skill_31.Slot == -1)
																		{
																			GlobalLog.Debug(string.Format(arg2: SkillBarHud.SetSlot(GeneralSettings.Instance.AuraSwapSlot, skill_31), format: "[{0}] {1} is not on skillbar, set slot result: {2}", arg0: Name, arg1: skill_31.Name));
																			return (LogicResult)0;
																		}
																		if ((int)(await MethodExtensions.SqUseAt(pos: ((NetworkObject)LokiPoe.Me).Position + new Vector2i(LokiPoe.Random.Next(-10, 10), LokiPoe.Random.Next(-10, 10)), skill: skill_31)) == 0)
																		{
																			return (LogicResult)0;
																		}
																	}
																	if (needSpidersHardCast && !aliveWorms.Any())
																	{
																		int flaskSlot4 = writhingJarFlask.LocationTopLeft.X + 1;
																		if (writhingJarFlask.CanUse && QuickFlaskHud.UseFlaskInSlot(flaskSlot4))
																		{
																			await Wait.For(() => ObjectManager.GetObjectsByType<Monster>().Any((Monster m) => m.IsAliveHostile && ((NetworkObject)m).Name == "Writhing Worm"), "worms", 20, 500, Config.DebugMode);
																			await Wait.LatencySleep(longer: true);
																			return (LogicResult)0;
																		}
																	}
																	if (bool_2)
																	{
																		if (!((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Bone Offering") == (RemoteMemoryObject)null) || !((RemoteMemoryObject)(object)skill_35 != (RemoteMemoryObject)null))
																		{
																			if ((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Flesh Offering") == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)skill_34 != (RemoteMemoryObject)null)
																			{
																				skill_33 = skill_34;
																			}
																			else if ((RemoteMemoryObject)(object)((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.Name == "Spirit Offering") == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)skill_36 != (RemoteMemoryObject)null)
																			{
																				skill_33 = skill_36;
																			}
																		}
																		else
																		{
																			skill_33 = skill_35;
																		}
																	}
																	if (FwkghBykgT < 20 && skill_33.SqCanUse(ignoreSkillbar: true) && (NetworkObject)(object)BestDeadTarget != (NetworkObject)null && !needSpidersHardCast)
																	{
																		Vector2i walkPos2 = ((NetworkObject)BestDeadTarget).Position;
																		if (((Vector2i)(ref walkPos2)).Distance(LokiPoe.MyPosition) < 40)
																		{
																			if (skill_33.Slot == -1)
																			{
																				GlobalLog.Debug(string.Format(arg2: SkillBarHud.SetSlot(GeneralSettings.Instance.AuraSwapSlot, skill_33), format: "[{0}] {1} is not on skillbar, set slot result: {2}", arg0: Name, arg1: skill_33.Name));
																				return (LogicResult)0;
																			}
																			UseResult useResult18 = await MethodExtensions.SqUseAt(skill_33, walkPos2);
																			if ((RemoteMemoryObject)(object)skill_31 == (RemoteMemoryObject)null)
																			{
																				FwkghBykgT++;
																			}
																			if ((int)useResult18 == 0)
																			{
																				stopwatch_12.Restart();
																				return (LogicResult)0;
																			}
																		}
																		walkPos2 = default(Vector2i);
																	}
																}
																bool questSkeletons = ((IAuthored)BotManager.Current).Name != "MapBotEx" && (stopwatch_0.ElapsedMilliseconds > 2000L || (World.Act10.FeedingTrough.IsCurrentArea && stopwatch_0.ElapsedMilliseconds > 750L));
																if (Blight.IsEncounterRunning && skill_25.SqCanUse() && (questSkeletons || deployedSkeletons < maxSkeletons || (bool_8 && skill_25.DeployedObjects.Any() && skill_25.DeployedObjects.Count((NetworkObject o) => o.Distance > 60f) > 2)) && (int)(await MethodExtensions.SqUseAt(skill_25, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
																{
																	stopwatch_0.Restart();
																	return (LogicResult)0;
																}
																if (cachedDistance <= Config.MaxRangeRange && (!((Actor)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).HasProxShieldEx() || cachedDistance <= 32) && (!((Actor)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).ManaBurnDonut() || cachedDistance >= 50 || cachedDistance <= 12))
																{
																	if (!skill_5.SqCanUse() || ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("circle_of_power_buff")) || ((int)CS_0024_003C_003E8__locals2.rarity_0 < 3 && !World.CurrentArea.Id.Contains("Affliction")) || (int)(await MethodExtensions.SqUse(skill_5)) != 0)
																	{
																		if (skill_10.SqCanUse())
																		{
																			Aura rageBuff = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName.Equals("rage"));
																			int num;
																			if (!World.CurrentArea.Id.Contains("Affliction"))
																			{
																				if (rageBuff != null && rageBuff.Charges >= 25 && (int)CS_0024_003C_003E8__locals2.rarity_0 >= 2)
																				{
																					num = 1;
																				}
																				else
																				{
																					num = ((rageBuff != null && rageBuff.Charges >= 45) ? 1 : 0);
																				}
																			}
																			else
																			{
																				num = 0;
																			}
																			nonSim = (byte)num != 0;
																			if (World.CurrentArea.Id.Contains("Affliction"))
																			{
																				if (rageBuff != null && rageBuff.Charges >= 40)
																				{
																					num2 = (((int)CS_0024_003C_003E8__locals2.rarity_0 >= 3) ? 1 : 0);
																					goto IL_5721;
																				}
																			}
																			num2 = 0;
																			goto IL_5721;
																		}
																		goto IL_57ce;
																	}
																	return (LogicResult)0;
																}
																if (!skipPathing)
																{
																	if (canMove)
																	{
																		if (!skill_22.SqCanUse())
																		{
																			Vector2i comePos = ((Config.MaxRangeRange > 45) ? FindCombatPosition(CS_0024_003C_003E8__locals2.vector2i_0, CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0) : CS_0024_003C_003E8__locals2.vector2i_0);
																			ProcessHookManager.ClearAllKeyStates();
																			if (PlayerMoverManager.Current.MoveTowards(comePos, Array.Empty<object>()))
																			{
																				return (LogicResult)0;
																			}
																			GlobalLog.Error($"[{Name}] MoveTowards failed for {comePos}.");
																			await Coroutines.FinishCurrentAction(true);
																			return (LogicResult)0;
																		}
																		UseResult useResult = MethodExtensions.SqBeginUseAt(pos: new Vector2i(CS_0024_003C_003E8__locals2.vector2i_0.X + LokiPoe.Random.Next(-15, 15), CS_0024_003C_003E8__locals2.vector2i_0.Y + LokiPoe.Random.Next(-15, 15)), skill: skill_22);
																		if ((int)useResult <= 0)
																		{
																			stopwatch_1.Restart();
																			return (LogicResult)0;
																		}
																		return (LogicResult)0;
																	}
																	GlobalLog.Info("[" + Name + "-Range] Not moving towards the target because we should not move currently.");
																	return (LogicResult)1;
																}
																GlobalLog.Info($"[{Name}-Range] Cannot move towards {cachedName}. We will rely on QuestBot to bring us close to him. CanSee {canSee} Dist: {cachedDistance}");
																return (LogicResult)1;
															}
															return (LogicResult)0;
														}
														GlobalLog.Error("[" + Name + "] Could not determine the path distance to the " + ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Name + ". Now blacklisting it.");
														if (!cachedMetadata.Contains("AtlasBosses/TheElder"))
														{
															hashSet_0.Add(((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Id);
														}
														else
														{
															await Wait.SleepSafe(1500);
														}
														return (LogicResult)0;
													}
													return (LogicResult)1;
												}
												return (LogicResult)0;
											}
											return (LogicResult)0;
										}
										return (LogicResult)0;
									}
									return (LogicResult)0;
								}
								return (LogicResult)1;
							}
							if (Config.DebugMode)
							{
								GlobalLog.Warn("[" + Name + "] Skellies seem to be Mages. Applying mage skeletons logic adjustments until bot restart!");
							}
							bool_8 = true;
							bool_7 = false;
							return (LogicResult)0;
						}
						if (Config.DebugMode)
						{
							GlobalLog.Warn("[" + Name + "] Skellies seem to be Melee. Applying melee skeletons logic adjustments until bot restart!");
						}
						bool_8 = false;
						bool_7 = false;
						return (LogicResult)0;
					}
					return (LogicResult)1;
				}
				Skill rmbSkill = SkillBarHud.Skills.FirstOrDefault((Skill s) => s.IsCastable && s.Slot == 3);
				Vector2i rand = new Vector2i(LokiPoe.MyPosition.X + LokiPoe.Random.Next(-10, 10), LokiPoe.MyPosition.Y + LokiPoe.Random.Next(-10, 10));
				MethodExtensions.SqBeginUseAt(rmbSkill, rand);
				return (LogicResult)0;
			}
			ProcessHookManager.ClearAllKeyStates();
			return (LogicResult)1;
		}
		bool_5 = false;
		return (LogicResult)0;
		IL_5721:
		bool sim = (byte)num2 != 0;
		if ((sim || nonSim) && (int)(await MethodExtensions.SqUse(skill_10)) == 0)
		{
			return (LogicResult)0;
		}
		goto IL_57ce;
		IL_57ce:
		if (hashSet_1.Any() && ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || NumberOfMobsNear((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0, 20f) >= 3))
		{
			foreach (int curseSlot in hashSet_1)
			{
				Skill skill = SkillBarHud.Slot(curseSlot);
				if (stopwatch_9.ElapsedMilliseconds > 2000L && skill.SqCanUse() && cachedIsCursable && cachedCurseCount < Config.CursesAllowed && !((Actor)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).HasCurseFromEx(skill))
				{
					await MethodExtensions.SqUseAt(skill, CS_0024_003C_003E8__locals2.vector2i_0);
				}
			}
			stopwatch_9.Restart();
		}
		Monster markedMob = null;
		if (skill_30.SqCanUse() && stopwatch_11.ElapsedMilliseconds > 1000L && !cachedAuras.Any((Aura a) => a.InternalName == "curse_snipers_mark" || a.InternalName == "curse_assassins_mark") && ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || NumberOfMobsNear((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0, 50f) > 15))
		{
			markedMob = ObjectManager.GetObjectsByType<Monster>().FirstOrDefault((Monster m) => !((Actor)m).IsDead && ((Actor)m).Auras.Any((Aura a) => a.InternalName == "curse_snipers_mark" || a.InternalName == "curse_assassins_mark"));
			string skillName5 = skill_30.Name;
			if ((NetworkObject)(object)markedMob == (NetworkObject)null || markedMob.Rarity < CS_0024_003C_003E8__locals2.rarity_0)
			{
				GlobalLog.Warn("[" + Name + "] Using " + skillName5 + " on " + cachedName);
				UseResult useResult20 = await MethodExtensions.SqUseAt(skill_30, CS_0024_003C_003E8__locals2.vector2i_0);
				stopwatch_11.Restart();
				if ((int)useResult20 == 0)
				{
					return (LogicResult)0;
				}
			}
		}
		if (skill_29.SqCanUse(ignoreSkillbar: true))
		{
			if (skill_29.Slot == -1)
			{
				GlobalLog.Debug(string.Format(arg2: SkillBarHud.SetSlot(GeneralSettings.Instance.AuraSwapSlot, skill_29), format: "[{0}] {1} is not on skillbar, set slot result: {2}", arg0: Name, arg1: skill_29.Name));
				return (LogicResult)0;
			}
			if (((Vector2i)(ref CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0)).Distance(CS_0024_003C_003E8__locals2.vector2i_0) < 70 && ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || (cachedMetadata.Contains("Totems/Labyrinth") && stopwatch_13.ElapsedMilliseconds > 1000L)))
			{
				Vector2i usePos = (((NetworkObject)(object)markedMob != (NetworkObject)null) ? ((NetworkObject)markedMob).Position : CS_0024_003C_003E8__locals2.vector2i_0);
				bool anySignalsAlready = CombatAreaCache.Current.Monsters.Any((CachedMonster m) => m.Auras.Any((Aura a) => a.InternalName == "minion_focussed_fire_target"));
				string skillName4 = skill_29.Name;
				if (!anySignalsAlready)
				{
					GlobalLog.Warn("[" + Name + "] Using " + skillName4 + " on " + cachedName);
					UseResult useResult19 = await MethodExtensions.SqUseAt(skill_29, usePos);
					stopwatch_13.Restart();
					if ((int)useResult19 == 0)
					{
						return (LogicResult)0;
					}
				}
			}
		}
		if (SqRoutine.list_3.Any((Skill s) => s.CanUse(false, false, true)))
		{
			List<Skill> usable7 = SqRoutine.list_3.Where((Skill s) => s.CanUse(false, false, true)).ToList();
			int j = LokiPoe.Random.Next(usable7.Count);
			Skill randCry = usable7[j];
			if (CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.method_0(randCry.Name, 35, 15) && (int)(await MethodExtensions.SqUseAt(randCry, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
			{
				return (LogicResult)0;
			}
		}
		if (!skill_14.SqCanUse() || ((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Auras.Any((Aura a) => a.InternalName.Contains("sigil")) || (skill_14.Name.ContainsIgnorecase("arcanist") && (int)CS_0024_003C_003E8__locals2.rarity_0 < 2 && !interval_1.Elapsed) || (int)(await MethodExtensions.SqUseAt(skill_14, CS_0024_003C_003E8__locals2.vector2i_0)) != 0)
		{
			if (!skill_39.SqCanUse() || stopwatch_3.ElapsedMilliseconds <= 5000L || ObjectManager.GetObjectsByType<Monster>().Any((Monster m) => m.IsAliveHostile && ((NetworkObject)m).Distance < 40f) || (int)(await MethodExtensions.SqUseAt(skill_39, CS_0024_003C_003E8__locals2.vector2i_0)) != 0)
			{
				if (Config.VaalSkillsList.Any((SqRoutineSettings.VaalSkillEntry e) => CS_0024_003C_003E8__locals2.rarity_0 >= e.MinRarity))
				{
					List<string> list_ = (from e in Config.VaalSkillsList
						where CS_0024_003C_003E8__locals2.rarity_0 >= e.MinRarity
						select e into s
						select s.Name).ToList();
					List<Skill> usable6 = SqRoutine.list_0.Where((Skill s) => list_.Contains(s.Name) && s.CanUse(false, false, true)).ToList();
					if (usable6.Any())
					{
						int index4 = LokiPoe.Random.Next(usable6.Count);
						Skill rnd2 = usable6[index4];
						GlobalLog.Warn("[" + Name + "] We can use vaal skill: " + rnd2.Name);
						await MethodExtensions.SqUseAt(rnd2, CS_0024_003C_003E8__locals2.vector2i_0);
					}
				}
				if (Config.TotemSkillsList.Any((SqRoutineSettings.TotemSkillEntry e) => CS_0024_003C_003E8__locals2.rarity_0 >= e.MinRarity))
				{
					List<string> list_0 = (from e in Config.TotemSkillsList
						where CS_0024_003C_003E8__locals2.rarity_0 >= e.MinRarity && e.UsageCase != SqRoutineSettings.TotemUsageCase.DontUse
						select e into s
						select s.Name).ToList();
					List<Skill> usable5 = SqRoutine.list_0.Where((Skill s) => list_0.Contains(s.Name) && s.CanUse(false, false, true) && (s.SkillTags.Contains("totem") || s.Stats.ContainsKey((StatTypeGGG)2330))).ToList();
					if (usable5.Any())
					{
						Skill skill_0 = usable5.OrderBy((Skill t) => t.NumberDeployed).First();
						SqRoutineSettings.TotemUsageCase useCase = Config.TotemSkillsList.FirstOrDefault((SqRoutineSettings.TotemSkillEntry s) => s.Name == skill_0.Name).UsageCase;
						int nearbyTotems = skill_0.DeployedObjects.Count((NetworkObject t) => (double)t.Distance <= (double)Config.CombatRange * 1.2);
						int deployed2 = skill_0.NumberDeployed;
						skill_0.Stats.TryGetValue((StatTypeGGG)955, out var max);
						if (useCase == SqRoutineSettings.TotemUsageCase.BuffOnly)
						{
							DateTime lastUsed2 = Config.TotemSkillsList.FirstOrDefault((SqRoutineSettings.TotemSkillEntry s) => s.Name == skill_0.Name).LastUsed;
							if ((deployed2 == 0 || nearbyTotems != 0) && lastUsed2.AddSeconds(4.0) < DateTime.Now && (int)(await MethodExtensions.SqUseAt(skill_0, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
							{
								Config.TotemSkillsList.FirstOrDefault((SqRoutineSettings.TotemSkillEntry s) => s.Name == skill_0.Name).LastUsed = DateTime.Now;
								return (LogicResult)0;
							}
						}
						if (useCase == SqRoutineSettings.TotemUsageCase.MaxTotems && (deployed2 < max || max - nearbyTotems > 1))
						{
							if (Config.DebugMode)
							{
								GlobalLog.Warn($"{skill_0.Name} Totem Deployed: {deployed2}/{max} nearby: {nearbyTotems}");
							}
							UseResult useResult14 = MethodExtensions.SqBeginUseAt(skill_0, CS_0024_003C_003E8__locals2.vector2i_0);
							if ((int)useResult14 == 0)
							{
								return (LogicResult)0;
							}
						}
					}
				}
				if (skill_17.SqCanUse() && ((Actor)LokiPoe.Me).BladeVortexCharges < 10 && (int)(await MethodExtensions.SqUse(skill_17)) == 0)
				{
					return (LogicResult)0;
				}
				if (skill_28.SqCanUse() && ((Vector2i)(ref CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0)).Distance(CS_0024_003C_003E8__locals2.vector2i_0) < 70 && deployedPhantasm > 12 && ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || NumberOfMobsNear((NetworkObject)(object)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0, 20f) > 10))
				{
					string skillName3 = skill_28.Name;
					GlobalLog.Warn("[" + Name + "] Using " + skillName3 + " on " + cachedName);
					if ((int)(await MethodExtensions.SqUseAt(skill_28, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
					{
						return (LogicResult)0;
					}
				}
				if (!skill_20.SqCanUse() || (deployedSrs >= 15 && stopwatch_2.ElapsedMilliseconds <= 2500L) || (int)(await MethodExtensions.SqUse(skill_20)) != 0)
				{
					if (skill_15.SqCanUse() && !World.Act6.Beacon.IsCurrentArea && canSee && !((Actor)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Auras.Select((Aura a) => a.InternalName).Any((string a) => a.Equals("reduced_fire_resistance_from_skill") || a.Equals("reduced_lightning_resistance_from_skill")) && ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 || !((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("righteous_fire"))))
					{
						if (stopwatch_7.ElapsedMilliseconds <= 750L || cachedDistance >= 45)
						{
							PlayerMoverManager.Current.MoveTowards(CS_0024_003C_003E8__locals2.vector2i_0, Array.Empty<object>());
							return (LogicResult)0;
						}
						if ((int)(await MethodExtensions.SqUseAt(skill_15, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
						{
							stopwatch_7.Restart();
							return (LogicResult)0;
						}
					}
					if (Config.DropBanners && (RemoteMemoryObject)(object)skill_18 != (RemoteMemoryObject)null && skill_18.IsOnSkillBar && stopwatch_4.ElapsedMilliseconds > 2500L && (int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 && ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("armour_evasion_banner_debuff_aura")))
					{
						string skillName2 = skill_18.Name;
						UseResult useResult13 = await MethodExtensions.SqUse(skill_18);
						GlobalLog.Debug("[" + Name + "] Dropping " + skillName2 + ".");
						stopwatch_4.Restart();
						if ((int)useResult13 == 0)
						{
							return (LogicResult)0;
						}
					}
					if (Config.DropBanners && (RemoteMemoryObject)(object)skill_19 != (RemoteMemoryObject)null && skill_19.IsOnSkillBar && stopwatch_4.ElapsedMilliseconds > 2500L && (int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 && ((Actor)LokiPoe.Me).Auras.Any((Aura a) => a.InternalName.Equals("puresteel_banner_stage")))
					{
						string skillName = skill_19.Name;
						UseResult useResult10 = await MethodExtensions.SqUse(skill_19);
						GlobalLog.Debug("[" + Name + "] Dropping " + skillName + ".");
						stopwatch_4.Restart();
						if ((int)useResult10 == 0)
						{
							return (LogicResult)0;
						}
					}
					if (skill_2.SqCanUse())
					{
						Aura aura = ((Actor)LokiPoe.Me).Auras.FirstOrDefault((Aura a) => a.InternalName == "snapping_adder_projectile");
						if ((RemoteMemoryObject)(object)aura != (RemoteMemoryObject)null && aura.Charges >= 20 && (int)(await MethodExtensions.SqUseAt(skill_2, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
						{
							return (LogicResult)0;
						}
					}
					if ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2 && rarePlusSkills.Any((Skill s) => s.CanUse(false, false, true)))
					{
						List<Skill> usable2 = rarePlusSkills.Where((Skill s) => s.CanUse(false, false, true)).ToList();
						int index2 = LokiPoe.Random.Next(usable2.Count);
						Skill randomTargetted2 = usable2[index2];
						UseResult useResult9 = MethodExtensions.SqBeginUseAt(randomTargetted2, CS_0024_003C_003E8__locals2.vector2i_0);
						if ((int)useResult9 == 0)
						{
							return (LogicResult)0;
						}
					}
					if (skill_1.SqCanUse())
					{
						UseResult useResult8 = MethodExtensions.SqBeginUseAt(skill_1, CS_0024_003C_003E8__locals2.vector2i_0);
						if ((int)useResult8 == 0)
						{
							return (LogicResult)0;
						}
					}
					if (skill_4.SqCanUse())
					{
						if (cachedDistance < 12)
						{
							CS_0024_003C_003E8__locals2.vector2i_0 = CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0;
						}
						bool bodyswap = skill_4.InternalId.Equals("bodyswap");
						Monster usableCorpse2 = (from m in ObjectManager.GetObjectsByType<Monster>()
							where CS_0024_003C_003E8__locals2.method_0(m, lite: true)
							orderby NumberOfMobsNear((NetworkObject)(object)m, 35f) descending
							select m).FirstOrDefault((Monster m) => !dictionary_1.ContainsKey(((NetworkObject)m).Id));
						if (bodyswap)
						{
							usableCorpse2 = (from m in ObjectManager.GetObjectsByType<Monster>().Where(delegate(Monster m)
								{
									//IL_0001: Unknown result type (might be due to invalid IL or missing references)
									//IL_0006: Unknown result type (might be due to invalid IL or missing references)
									//IL_000b: Unknown result type (might be due to invalid IL or missing references)
									Vector2i position7 = ((NetworkObject)m).Position;
									return ((Vector2i)(ref position7)).Distance(CS_0024_003C_003E8__locals2.vector2i_0) < 20;
								})
								orderby CS_0024_003C_003E8__locals2.method_0(m) descending, NumberOfMobsNear((NetworkObject)(object)m, 35f) descending
								select m).FirstOrDefault();
						}
						if ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2)
						{
							usableCorpse2 = (from m in ObjectManager.GetObjectsByType<Monster>()
								where CS_0024_003C_003E8__locals2.method_0(m, lite: true)
								orderby ((Actor)m).MaxHealth descending
								select m).FirstOrDefault((Monster m) => !dictionary_1.ContainsKey(((NetworkObject)m).Id));
						}
						bool corpsewalker = InventoryUi.InventoryControl_Boots.Inventory.Items.Any((Item i) => i.FullName == "Corpsewalker");
						if ((NetworkObject)(object)usableCorpse2 != (NetworkObject)null)
						{
							UseResult useResult7 = MethodExtensions.SqBeginUseAt(skill_4, ((NetworkObject)usableCorpse2).Position);
							if ((int)useResult7 == 0)
							{
								if (!dictionary_1.ContainsKey(((NetworkObject)usableCorpse2).Id))
								{
									dictionary_1.Add(((NetworkObject)usableCorpse2).Id, DateTime.Now);
								}
								return (LogicResult)0;
							}
						}
						else if (skill_31.SqCanUse() && (!corpsewalker || (corpsewalker && cachedDistance > 45)))
						{
							if (Config.DebugMode)
							{
								GlobalLog.Debug("[" + Name + "] No corpses nearby for detonation. Using " + skill_31.Name);
							}
							if ((int)(await MethodExtensions.SqUseAt(skill_31, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
							{
								await Wait.LatencySleep();
								await MethodExtensions.SqUseAt(skill_4, CS_0024_003C_003E8__locals2.vector2i_0);
								return (LogicResult)0;
							}
						}
						else if (!skill_32.SqCanUse())
						{
							if (corpsewalker)
							{
								if (cachedDistance > 20 && Move.Towards(CS_0024_003C_003E8__locals2.vector2i_0, ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Name ?? ""))
								{
									return (LogicResult)0;
								}
								if (cachedDistance <= 20 && Move.Towards(FindCombatPosition(CS_0024_003C_003E8__locals2.vector2i_0, CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0), ((NetworkObject)CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.monster_0).Name ?? ""))
								{
									return (LogicResult)0;
								}
							}
						}
						else
						{
							if (Config.DebugMode)
							{
								GlobalLog.Debug("[" + Name + "] No corpses nearby for detonation. Using " + skill_32.Name);
							}
							if ((int)(await MethodExtensions.SqUseAt(skill_32, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
							{
								await Wait.LatencySleep();
								await MethodExtensions.SqUseAt(skill_4, CS_0024_003C_003E8__locals2.vector2i_0);
								return (LogicResult)0;
							}
						}
					}
					if ((RemoteMemoryObject)(object)skill_16 != (RemoteMemoryObject)null || (RemoteMemoryObject)(object)skill_32 != (RemoteMemoryObject)null)
					{
						if (cachedDistance < 12)
						{
							CS_0024_003C_003E8__locals2.vector2i_0 = CS_0024_003C_003E8__locals2._003C_003Ec__DisplayClass98_0_0.vector2i_0;
						}
						List<Effect> cremateGeysers = (from e in ObjectManager.GetObjectsByType<Effect>()
							where ((NetworkObject)e).AnimatedPropertiesMetadata.Contains("CorpseEruptionGroundFX")
							select e).ToList();
						List<Effect> properGeysers = ((IEnumerable<Effect>)cremateGeysers).Where((Func<Effect, bool>)delegate(NetworkObject geyser)
						{
							//IL_0001: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Invalid comparison between Unknown and I4
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							//IL_000f: Unknown result type (might be due to invalid IL or missing references)
							//IL_0014: Unknown result type (might be due to invalid IL or missing references)
							if ((int)CS_0024_003C_003E8__locals2.rarity_0 >= 2)
							{
								Vector2i position6 = geyser.Position;
								if (((Vector2i)(ref position6)).Distance(CS_0024_003C_003E8__locals2.vector2i_0) < 25)
								{
									return true;
								}
							}
							return !(geyser.Distance > 60f) && NumberOfMobsNear(geyser, 25f) > 2;
						}).ToList();
						if (skill_16.SqCanUse() && (cremateGeysers.Count < 2 || !properGeysers.Any() || stopwatch_8.ElapsedMilliseconds > 3000L))
						{
							Monster usableCorpse = (from m in ObjectManager.GetObjectsByType<Monster>()
								where CS_0024_003C_003E8__locals2.method_0(m)
								select m).OrderBy(delegate(Monster m)
							{
								//IL_0001: Unknown result type (might be due to invalid IL or missing references)
								//IL_0006: Unknown result type (might be due to invalid IL or missing references)
								//IL_000b: Unknown result type (might be due to invalid IL or missing references)
								Vector2i position5 = ((NetworkObject)m).Position;
								return ((Vector2i)(ref position5)).Distance(CS_0024_003C_003E8__locals2.vector2i_0);
							}).FirstOrDefault((Monster m) => !dictionary_1.ContainsKey(((NetworkObject)m).Id));
							if (!((NetworkObject)(object)usableCorpse != (NetworkObject)null))
							{
								if (skill_31.SqCanUse())
								{
									if (Config.DebugMode)
									{
										GlobalLog.Debug("[" + Name + "] No corpses nearby for cremation. Using " + skill_31.Name);
									}
									if ((int)(await MethodExtensions.SqUseAt(skill_31, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
									{
										await Wait.LatencySleep();
										await MethodExtensions.SqUseAt(skill_16, CS_0024_003C_003E8__locals2.vector2i_0);
										stopwatch_8.Restart();
										return (LogicResult)0;
									}
								}
								else if (skill_32.SqCanUse())
								{
									if (Config.DebugMode)
									{
										GlobalLog.Debug("[" + Name + "] No corpses nearby for cremation. Using " + skill_32.Name);
									}
									if ((int)(await MethodExtensions.SqUseAt(skill_32, CS_0024_003C_003E8__locals2.vector2i_0)) == 0)
									{
										await Wait.LatencySleep();
										await MethodExtensions.SqUseAt(skill_16, CS_0024_003C_003E8__locals2.vector2i_0);
										stopwatch_8.Restart();
										return (LogicResult)0;
									}
								}
							}
							else
							{
								UseResult useResult6 = await MethodExtensions.SqUseAt(skill_16, ((NetworkObject)usableCorpse).Position);
								dictionary_1.Add(((NetworkObject)usableCorpse).Id, DateTime.Now);
								if ((int)useResult6 == 0)
								{
									stopwatch_8.Restart();
									return (LogicResult)0;
								}
							}
						}
						bool shouldSpamUnearth = (RemoteMemoryObject)(object)skill_16 != (RemoteMemoryObject)null && (int)skill_16.SkillGem.SkillGemQualityType == 3;
						if (skill_32.SqCanUse() && properGeysers.Any() && shouldSpamUnearth)
						{
							Vector2i pos = ((NetworkObject)properGeysers.OrderByDescending((Effect g) => NumberOfMobsNear((NetworkObject)(object)g, 23f)).ThenBy(delegate(Effect g)
							{
								//IL_0001: Unknown result type (might be due to invalid IL or missing references)
								//IL_0006: Unknown result type (might be due to invalid IL or missing references)
								//IL_000b: Unknown result type (might be due to invalid IL or missing references)
								Vector2i position4 = ((NetworkObject)g).Position;
								return ((Vector2i)(ref position4)).Distance(CS_0024_003C_003E8__locals2.vector2i_0);
							}).FirstOrDefault()).Position;
							if ((int)CS_0024_003C_003E8__locals2.rarity_0 == 3)
							{
								Effect closestGeyser = cremateGeysers.OrderBy(delegate(Effect m)
								{
									//IL_0001: Unknown result type (might be due to invalid IL or missing references)
									//IL_0006: Unknown result type (might be due to invalid IL or missing references)
									//IL_000b: Unknown result type (might be due to invalid IL or missing references)
									Vector2i position3 = ((NetworkObject)m).Position;
									return ((Vector2i)(ref position3)).Distance(CS_0024_003C_003E8__locals2.vector2i_0);
								}).FirstOrDefault();
								int num3;
								if ((NetworkObject)(object)closestGeyser != (NetworkObject)null)
								{
									Vector2i position2 = ((NetworkObject)closestGeyser).Position;
									num3 = ((((Vector2i)(ref position2)).Distance(CS_0024_003C_003E8__locals2.vector2i_0) < 23) ? 1 : 0);
								}
								else
								{
									num3 = 0;
								}
								if (num3 != 0)
								{
									pos = ((NetworkObject)closestGeyser).Position;
								}
							}
							int offset = LokiPoe.Random.Next(-6, 6);
							UseResult useResult5 = MethodExtensions.SqBeginUseAt(pos: new Vector2i(pos.X + offset, pos.Y + offset), skill: skill_32);
							if ((int)useResult5 == 0)
							{
								return (LogicResult)0;
							}
						}
					}
					if (skill_22.SqCanUse())
					{
						if (needSpidersCyclone && !aliveWorms.Any())
						{
							int flaskSlot = writhingJarFlask.LocationTopLeft.X + 1;
							if (writhingJarFlask.CanUse && QuickFlaskHud.UseFlaskInSlot(flaskSlot))
							{
								return (LogicResult)0;
							}
						}
						UseResult useResult4 = MethodExtensions.SqBeginUseAt(pos: new Vector2i(CS_0024_003C_003E8__locals2.vector2i_0.X + LokiPoe.Random.Next(-10, 15), CS_0024_003C_003E8__locals2.vector2i_0.Y + LokiPoe.Random.Next(-10, 15)), skill: skill_22);
						if ((int)useResult4 == 0)
						{
							return (LogicResult)0;
						}
					}
					if (pointTargetedSkills.Any((Skill s) => s.CanUse(false, false, true)))
					{
						List<Skill> usable = pointTargetedSkills.Where((Skill s) => s.CanUse(false, false, true) && s.InternalId != "melee").ToList();
						if (usable.Any())
						{
							int index = LokiPoe.Random.Next(usable.Count);
							Skill randomTargetted = usable[index];
							UseResult useResult3 = MethodExtensions.SqBeginUseAt(randomTargetted, CS_0024_003C_003E8__locals2.vector2i_0);
							if ((int)useResult3 == 0)
							{
								return (LogicResult)0;
							}
						}
					}
					bool questCheck = !World.Act6.Beacon.IsCurrentArea && (questBotSkills.Count > 1 || World.Act1.TwilightStrand.IsCurrentArea);
					if (((IAuthored)BotManager.Current).Name != "MapBotEx" && questCheck)
					{
						Skill skillToUse = questBotSkills.OrderBy((Skill s) => s.InternalId == "melee").FirstOrDefault();
						UseResult useResult2 = MethodExtensions.SqBeginUseAt(skillToUse, CS_0024_003C_003E8__locals2.vector2i_0);
						if ((int)useResult2 == 0)
						{
							return (LogicResult)0;
						}
					}
					ProcessHookManager.ClearAllKeyStates();
					return (LogicResult)0;
				}
				stopwatch_2.Restart();
				return (LogicResult)0;
			}
			stopwatch_3.Restart();
			return (LogicResult)0;
		}
		return (LogicResult)0;
	}

	public override string ToString()
	{
		return Name + ": " + Description;
	}

	private bool CombatTargetingOnInclusionCalcuation(NetworkObject entity)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Invalid comparison between Unknown and I4
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Invalid comparison between Unknown and I4
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Invalid comparison between Unknown and I4
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Invalid comparison between Unknown and I4
		try
		{
			Monster val = (Monster)(object)((entity is Monster) ? entity : null);
			if ((NetworkObject)(object)val == (NetworkObject)null)
			{
				return false;
			}
			if (Blacklist.Contains((NetworkObject)(object)val))
			{
				return false;
			}
			if (dictionary_0.ContainsKey(((NetworkObject)val).Id))
			{
				return false;
			}
			if (hashSet_0.Contains(((NetworkObject)val).Id))
			{
				return false;
			}
			if (CrucibleDevice != (NetworkObject)null)
			{
				Vector2i position = ((NetworkObject)val).Position;
				if (((Vector2i)(ref position)).Distance(CrucibleDevice.Position) < 20)
				{
					return false;
				}
			}
			Rarity rarity = val.Rarity;
			string metadata = ((NetworkObject)val).Metadata;
			string name = ((NetworkObject)val).Name;
			if (name.ContainsIgnorecase("banner"))
			{
				return false;
			}
			if (metadata.Contains("RyslathaEgg"))
			{
				return false;
			}
			if (((Actor)(object)val).HasProxShieldEx() && (int)rarity < 2)
			{
				return false;
			}
			if (((Actor)val).HasAura("monster_aura_cannot_die") && (int)rarity <= 2 && !((NetworkObject)val).Type.Contains("/Totems/"))
			{
				return false;
			}
			if (((Actor)val).IsDead)
			{
				return false;
			}
			if (((NetworkObject)val).Id == int_0)
			{
				return true;
			}
			if (Blight.IsEncounterRunning && ((Actor)(object)val).IsBlightMob())
			{
				return true;
			}
			if (metadata.Contains("Avatar"))
			{
				return true;
			}
			if (!val.IsActive)
			{
				return false;
			}
			if (((Actor)val).IsStrongboxMinion)
			{
				return true;
			}
			if (name.Equals("Guardian of the Goddess"))
			{
				return false;
			}
			if (((Actor)val).HealthPercent < 1f && (int)rarity != 3 && !metadata.Contains("Legion"))
			{
				return false;
			}
			if (World.Act1.TwilightStrand.IsCurrentArea)
			{
				if (name == "Hungry Corpse")
				{
					return true;
				}
				if ((int)rarity != 3)
				{
					return false;
				}
			}
			if (World.Act1.PrisonerGate.IsCurrentArea && (int)val.Rarity == 3)
			{
				return false;
			}
			if (World.Act1.MudFlats.IsCurrentArea && ((NetworkObject)val).Name == "Dripping Dead" && (int)rarity == 0)
			{
				return false;
			}
			if (metadata == "Metadata/Monsters/Tukohama/TukohamaShieldTotem")
			{
				return true;
			}
			if (((NetworkObject)val).Distance > (float)((int_1 != -1) ? int_1 : Config.CombatRange))
			{
				return false;
			}
			if (((Actor)val).HasAura((IEnumerable<string>)string_0))
			{
				return false;
			}
			if (metadata.Contains("Metadata/Monsters/Daemon/IzaroChargeDisruptorDaemon"))
			{
				return false;
			}
			if (name == "Miscreation")
			{
				Monster val2 = CombatAreaCache.Current.Monsters.FirstOrDefault((CachedMonster mo) => mo.Name.Equals("Dominus, High Templar"))?.Object;
				if ((NetworkObject)(object)val2 != (NetworkObject)null && !((Actor)val2).IsDead && (((NetworkObject)val2).Components.TransitionableComponent.Flag1 == 6 || ((NetworkObject)val2).Components.TransitionableComponent.Flag1 == 5))
				{
					Blacklist.Add(((NetworkObject)val).Id, TimeSpan.FromHours(1.0), "Miscreation");
					return false;
				}
			}
			if (name == "Chilling Portal" || name == "Burning Portal")
			{
				Blacklist.Add(((NetworkObject)val).Id, TimeSpan.FromHours(1.0), "Piety portal");
				return false;
			}
			if (name == "Summoned Phantasm")
			{
				return false;
			}
			if (metadata.Contains("DoedreStonePillar"))
			{
				Blacklist.Add(((NetworkObject)val).Id, TimeSpan.FromHours(1.0), "Doedre Pillar");
				return false;
			}
		}
		catch (Exception arg)
		{
			GlobalLog.Error($"[CombatOnInclusionCalcuation] {arg}");
			return false;
		}
		return true;
	}

	private static void CombatTargetingOnWeightCalculation(NetworkObject entity, ref float weight)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Invalid comparison between Unknown and I4
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Invalid comparison between Unknown and I4
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Invalid comparison between Unknown and I4
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Invalid comparison between Unknown and I4
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Invalid comparison between Unknown and I4
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Invalid comparison between Unknown and I4
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		Monster val = (Monster)(object)((entity is Monster) ? entity : null);
		if ((NetworkObject)(object)val == (NetworkObject)null)
		{
			return;
		}
		if (((NetworkObject)val).Id == int_0)
		{
			weight += 20000f;
		}
		if (!val.IsActive)
		{
			weight -= 5000f;
		}
		if (!ExilePather.CanObjectSee((NetworkObject)(object)LokiPoe.Me, (NetworkObject)(object)val, false, false))
		{
			weight -= 2000f;
		}
		Rarity rarity = val.Rarity;
		string metadata = ((NetworkObject)val).Metadata;
		string name = ((NetworkObject)val).Name;
		List<Aura> auras = ((Actor)val).Auras;
		Vector2i position;
		if (Blight.IsEncounterRunning && global::ExPlugins.BlightPluginEx.BlightPluginEx.CachedBlightCore != null)
		{
			float num = weight;
			position = ((NetworkObject)val).Position;
			weight = num + (float)(400 - ((Vector2i)(ref position)).Distance((Vector2i)global::ExPlugins.BlightPluginEx.BlightPluginEx.CachedBlightCore.Position) * 2);
		}
		weight -= ((NetworkObject)val).Distance * 2f;
		if (auras.Any((Aura a) => a.InternalName.Contains("sigil")))
		{
			weight -= 80f;
		}
		if (metadata.Contains("AtlasExiles/AtlasExile"))
		{
			weight += 300f;
		}
		if (((Actor)val).HasAura("monster_aura_cannot_die"))
		{
			weight += 50f;
		}
		if (((Actor)(object)val).HasProxShieldEx() && (int)rarity >= 2)
		{
			weight += 300f;
		}
		if (val.AffixToFocus())
		{
			weight += 500f;
		}
		if (((Actor)(object)val).ManaBurnDonut() && (int)rarity >= 2)
		{
			weight += 3000f;
		}
		if (World.CurrentArea.Id.Contains("Affliction"))
		{
			if (World.CurrentArea.Name == "Lunacy's Watch")
			{
				position = ((NetworkObject)val).Position;
				int num2 = ((Vector2i)(ref position)).Distance(new Vector2i(271, 687));
				if (num2 < 60)
				{
					weight -= num2 * 10 - 800;
				}
			}
			Rarity val2 = rarity;
			Rarity val3 = val2;
			if ((int)val3 != 2)
			{
				if ((int)val3 == 3)
				{
					if (SimulSett.Instance.KillBossAtTheEnd)
					{
						if (SimulSett.Instance.KillBossAtTheEnd)
						{
							weight -= 2650f;
						}
					}
					else
					{
						weight += 2650f;
					}
				}
			}
			else
			{
				weight += 200f;
			}
		}
		if ((int)rarity == 2)
		{
			weight += 80f;
		}
		if ((int)rarity == 3)
		{
			weight += 350f;
		}
		if (metadata.Contains("MonsterChest"))
		{
			weight += 800f;
		}
		if (metadata.Contains("Legion") || metadata.Contains("LeagueExpedition"))
		{
			weight += 200f;
		}
		string text = name;
		string text2 = text;
		if (text2 == "Summoned Skeleton")
		{
			weight -= 85f;
		}
		else if (text2 == "Raised Zombie")
		{
			weight -= 65f;
		}
		if ((int)rarity == 0 && ((NetworkObject)val).Type.Contains("/Totems/"))
		{
			weight += 15f;
		}
		if (val.ExplicitAffixes.Any((ModRecord a) => a.InternalName.Contains("RaisesUndead")) || val.ImplicitAffixes.Any((ModRecord a) => a.InternalName.Contains("RaisesUndead")))
		{
			weight += 45f;
		}
		if (((NetworkObject)val).Type.Contains("TaniwhaTail"))
		{
			weight -= 30f;
		}
		if ((RemoteMemoryObject)(object)((NetworkObject)val).Components.DiesAfterTimeComponent != (RemoteMemoryObject)null)
		{
			weight -= 15f;
		}
		if (((NetworkObject)val).Type.Contains("/BeastHeart"))
		{
			weight += 75f;
		}
		if (metadata.Contains("KitavaBoss/KitavaFinalHeart"))
		{
			weight += 450f;
		}
		if (metadata.Contains("KitavaBoss/KitavaHeart"))
		{
			weight += 450f;
		}
		if (metadata == "Metadata/Monsters/Tukohama/TukohamaShieldTotem")
		{
			weight += 4750f;
		}
		if (name == "Izaro")
		{
			weight += 1000f;
		}
		if (((Actor)val).IsStrongboxMinion || ((Actor)val).IsHarbingerMinion)
		{
			weight += 395f;
		}
		if (((NetworkObject)(object)val).IsSyndicateMember() && PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "SyndicatePlugin") != null)
		{
			weight += 250f;
		}
	}

	public static int NumberOfMobsNear(NetworkObject target, float distance, bool dead = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		Vector2i position = target.Position;
		int num = 0;
		foreach (Monster item in ObjectManager.Objects.OfType<Monster>())
		{
			if (((NetworkObject)item).Id == target.Id)
			{
				continue;
			}
			if (dead)
			{
				if (!((Actor)item).IsDead)
				{
					continue;
				}
			}
			else if (!item.IsAliveHostile && !((Actor)(object)item).IsBlightMob())
			{
				continue;
			}
			Vector2i position2 = ((NetworkObject)item).Position;
			if ((float)((Vector2i)(ref position2)).Distance(position) < distance)
			{
				num++;
			}
		}
		return num;
	}

	private static Vector2i FindCombatPosition(Vector2i cachedPosition, Monster target)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pt = ((Vector2i)(ref cachedPosition)).ToVector2();
		Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
		double angleBetweenPoints = GetAngleBetweenPoints(pt, ((Vector2i)(ref position)).ToVector2());
		Vector2i val = CalcSafePosition(angleBetweenPoints, 35f, cachedPosition, target);
		return (!(val == Vector2i.Zero)) ? val : cachedPosition;
	}

	public static Vector2i CalcSafePosition(double escapeAngle, float safeDistance, Vector2i center, Monster target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		Vector2i zero = Vector2i.Zero;
		int num = Config.MaxRangeRange;
		while ((float)num > safeDistance)
		{
			for (int i = 0; i < 360; i += 3)
			{
				double radian = escapeAngle + Math.Ceiling((double)i / 2.0) * 0.174533 * (double)((i % 2 == 0) ? 1 : (-1));
				Vector2i pointOnCircle = GetPointOnCircle(center, radian, num);
				bool flag;
				try
				{
					flag = !ExilePather.IsWalkable(pointOnCircle) || !ExilePather.CanObjectSee((NetworkObject)(object)LokiPoe.Me, pointOnCircle, true, false) || ClosedDoorBetween((NetworkObject)(object)LokiPoe.Me, pointOnCircle, 10, 10, dontLeaveFrame: true);
					if ((NetworkObject)(object)target != (NetworkObject)null)
					{
						flag = !ExilePather.CanObjectSee((NetworkObject)(object)target, pointOnCircle, true, false);
					}
				}
				catch
				{
					flag = true;
				}
				if (!flag)
				{
					return pointOnCircle;
				}
			}
			num -= 4;
		}
		return zero;
	}

	private static double GetAngleBetweenPoints(Vector2 pt1, Vector2 pt2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return Math.Atan2(pt2.Y - pt1.Y, pt2.X - pt1.X);
	}

	private static Vector2i GetPointOnCircle(Vector2i center, double radian, double radius)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Vector2i result = default(Vector2i);
		result.X = center.X + (int)(radius * Math.Cos(radian));
		result.Y = center.Y + (int)(radius * Math.Sin(radian));
		return result;
	}

	private void RegisterExposedSettings()
	{
		if (dictionary_2 != null)
		{
			return;
		}
		dictionary_2 = new Dictionary<string, Func<Tuple<object, string>[], object>>
		{
			{
				"SetLeash",
				delegate(Tuple<object, string>[] param)
				{
					int_1 = (int)param[0].Item1;
					return null;
				}
			},
			{
				"GetLeash",
				(Tuple<object, string>[] param) => int_1
			}
		};
		PropertyInfo[] properties = typeof(SqRoutineSettings).GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo_0 in array)
		{
			if ((propertyInfo_0.PropertyType != typeof(int) && propertyInfo_0.PropertyType != typeof(bool)) || !propertyInfo_0.CanWrite || !propertyInfo_0.CanRead)
			{
				continue;
			}
			MethodInfo getMethod = propertyInfo_0.GetGetMethod(nonPublic: false);
			MethodInfo setMethod = propertyInfo_0.GetSetMethod(nonPublic: false);
			if (!(getMethod == null) && !(setMethod == null))
			{
				dictionary_2.Add("Set" + propertyInfo_0.Name, delegate(Tuple<object, string>[] param)
				{
					propertyInfo_0.SetValue(Config, param[0]);
					return null;
				});
				dictionary_2.Add("Get" + propertyInfo_0.Name, (Tuple<object, string>[] param) => propertyInfo_0.GetValue(Config));
			}
		}
	}

	public static bool ClosedDoorBetween(NetworkObject start, NetworkObject end, int distanceFromPoint = 10, int stride = 10, bool dontLeaveFrame = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ClosedDoorBetween(start.Position, end.Position, distanceFromPoint, stride, dontLeaveFrame);
	}

	public static bool ClosedDoorBetween(NetworkObject start, Vector2i end, int distanceFromPoint = 10, int stride = 10, bool dontLeaveFrame = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ClosedDoorBetween(start.Position, end, distanceFromPoint, stride, dontLeaveFrame);
	}

	public static bool ClosedDoorBetween(Vector2i start, NetworkObject end, int distanceFromPoint = 10, int stride = 10, bool dontLeaveFrame = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return ClosedDoorBetween(start, end.Position, distanceFromPoint, stride, dontLeaveFrame);
	}

	public static bool ClosedDoorBetween(Vector2i start, Vector2i end, int distanceFromPoint = 10, int stride = 10, bool dontLeaveFrame = false)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		List<TriggerableBlockage> list = ObjectManager.AnyDoors.Where((TriggerableBlockage d) => !d.IsOpened).ToList();
		if (list.Any())
		{
			List<Vector2i> pointsOnSegment = ExilePather.GetPointsOnSegment(start, end, dontLeaveFrame);
			for (int i = 0; i < pointsOnSegment.Count; i += stride)
			{
				foreach (TriggerableBlockage item in list)
				{
					Vector2i position = ((NetworkObject)item).Position;
					if (((Vector2i)(ref position)).Distance(pointsOnSegment[i]) <= distanceFromPoint)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public void Initialize()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		CombatTargeting.InclusionCalcuation += new InclusionCalculator(CombatTargetingOnInclusionCalcuation);
		CombatTargeting.WeightCalculation += new WeightCalcuator(CombatTargetingOnWeightCalculation);
		RegisterExposedSettings();
		stopwatch_0 = Stopwatch.StartNew();
		stopwatch_2 = Stopwatch.StartNew();
		stopwatch_13 = Stopwatch.StartNew();
		stopwatch_3 = Stopwatch.StartNew();
		stopwatch_6 = Stopwatch.StartNew();
		stopwatch_11 = Stopwatch.StartNew();
		stopwatch_10 = Stopwatch.StartNew();
		stopwatch_12 = Stopwatch.StartNew();
		stopwatch_1 = Stopwatch.StartNew();
		stopwatch_9 = Stopwatch.StartNew();
		stopwatch_7 = Stopwatch.StartNew();
		stopwatch_8 = Stopwatch.StartNew();
		stopwatch_5 = Stopwatch.StartNew();
		stopwatch_4 = Stopwatch.StartNew();
	}

	public void Deinitialize()
	{
	}

	public MessageResult Message(Message message)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "SetBestTarget"))
		{
			if (!(message.Id == "SetCombatRange"))
			{
				if (!(message.Id == "GetCombatRange"))
				{
					if (!dictionary_2.TryGetValue(message.Id, out var value))
					{
						if (message.Id == "GetCombatTargeting")
						{
							message.AddOutput<Targeting>((IMessageHandler)(object)this, CombatTargeting, "");
							return (MessageResult)0;
						}
						if (message.Id == "ResetCombatTargeting")
						{
							CombatTargeting.ResetInclusionCalcuation();
							CombatTargeting.ResetWeightCalculation();
							CombatTargeting.InclusionCalcuation += new InclusionCalculator(CombatTargetingOnInclusionCalcuation);
							CombatTargeting.WeightCalculation += new WeightCalcuator(CombatTargetingOnWeightCalculation);
							return (MessageResult)0;
						}
						if (!(message.Id == "UpdateCombatTargeting"))
						{
							if (!(message.Id == "player_leveled_event"))
							{
								if (!(message.Id == "player_died_event"))
								{
									if (!(message.Id == "area_changed_event"))
									{
										if (!(message.Id == "local_transition_entered"))
										{
											if (message.Id == "SetFlaskHook")
											{
												Func<Rarity, int, bool> input = message.GetInput<Func<Rarity, int, bool>>(0);
												func_0 = input;
												GlobalLog.Info("[" + Name + "] Flask hook has been set.");
												return (MessageResult)0;
											}
											return (MessageResult)1;
										}
										int_0 = 999999999;
										hashSet_0.Clear();
										return (MessageResult)0;
									}
									FwkghBykgT = 0;
									int_0 = 999999999;
									hashSet_0.Clear();
									return (MessageResult)0;
								}
								int input2 = message.GetInput<int>(0);
								GlobalLog.Info($"[{Name}] Total Deaths For Instance {input2}!");
								return (MessageResult)0;
							}
							GlobalLog.Info($"[{Name}] We are now level {message.GetInput<int>(0)}!");
							return (MessageResult)0;
						}
						CombatTargeting.Update();
						return (MessageResult)0;
					}
					message.AddOutput<object>((IMessageHandler)(object)this, value(message.Inputs.ToArray()), "");
					return (MessageResult)0;
				}
				message.AddOutput<int>((IMessageHandler)(object)this, Config.CombatRange, "");
				return (MessageResult)0;
			}
			int input3 = message.GetInput<int>(0);
			Config.CombatRange = input3;
			return (MessageResult)0;
		}
		int input4 = message.GetInput<int>(0);
		int_0 = input4;
		return (MessageResult)0;
	}

	public void Start()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[" + Name + "] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		BotManager.MsBetweenTicks = 60;
		bool_7 = true;
		bool_6 = false;
		bool_4 = false;
		bool_9 = false;
		bool_0 = false;
		list_0.Clear();
		list_1.Clear();
		if (Config.CombatRange < 70)
		{
			Config.CombatRange = 70;
		}
		if (Config.MaxRangeRange > Config.CombatRange)
		{
			Config.MaxRangeRange = Config.CombatRange - 5;
		}
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		bool flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		bool flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	public void Tick()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Invalid comparison between Unknown and I4
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[" + Name + "] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		if (interval_6.Elapsed)
		{
			int_0 = 9999999;
			hashSet_0.Clear();
		}
		if (!bool_6 && list_0.Any() && Config.DebugMode)
		{
			GlobalLog.Warn("[" + Name + "] Castable skills:");
			GlobalLog.Warn("======================");
			foreach (Skill item in from s in list_0
				where s.IsCastable && (s.Stats.Any() || s.Name == "Move" || s.Name == "Interaction")
				orderby s.Cost descending
				select s)
			{
				string text = "[" + item.Name + "]";
				if ((RemoteMemoryObject)(object)item.SkillGem != (RemoteMemoryObject)null && (int)item.SkillGem.SkillGemQualityType > 0)
				{
					text = $"{item.SkillGem.SkillGemQualityType} [{item.SkillGem.Name}]";
				}
				GlobalLog.Warn($"{text} | {item.BoundKey} | {item.Cost} mana | canUse: {item.SqCanUse(ignoreSkillbar: true)}");
			}
			GlobalLog.Warn("======================");
			bool_6 = true;
		}
		if (!bool_4 && list_0.Any())
		{
			Skill val = list_0.FirstOrDefault((Skill s) => s.BoundKeys.All((Keys k) => k == Keys.LButton) && !s.Name.Equals("Default Attack"));
			if ((RemoteMemoryObject)(object)val != (RemoteMemoryObject)null)
			{
				GlobalLog.Error("Please do not bind anything other than Default Attack on Left Mouse button! Current: " + val.Name);
				BotManager.Stop(new StopReasonData("wrong_keybind", "Please do not bind anything other than Default Attack on Left Mouse button! Current: " + val.Name, (object)null), false);
			}
			bool_4 = true;
		}
		if (!bool_9 && list_0.Any())
		{
			Skill skill_0 = list_0.FirstOrDefault((Skill s) => s.Name == "Vaal Discipline");
			Skill skill_1 = list_0.FirstOrDefault((Skill s) => s.Name == "Vaal Molten Shell");
			Skill skill_2 = list_0.FirstOrDefault((Skill s) => s.Name == "Vaal Summon Skeletons");
			List<Skill> source = list_0.Where((Skill s) => s.SkillType.Contains("vaal") && !s.SkillTags.Contains("totem") && s.IsCastable && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_0 && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_1 && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_2).ToList();
			List<Skill> source2 = source.Where((Skill s) => !Config.VaalSkillsList.Any((SqRoutineSettings.VaalSkillEntry e) => e.Name.Equals(s.Name))).ToList();
			if (source2.Any())
			{
				string text2 = "Vaal skills changed. Please check your SqRoutine settings and refresh vaal skills. Added skills: " + string.Join(",", source2.Select((Skill sk) => sk.Name));
				GlobalLog.Error(text2);
				BotManager.Stop(new StopReasonData("vaal_skills_changed", text2, (object)null), false);
			}
			bool_9 = true;
		}
		if (!bool_10 && list_0.Any())
		{
			List<Skill> source3 = list_0.Where((Skill s) => (s.SkillTags.Contains("totem") || s.Stats.ContainsKey((StatTypeGGG)2330)) && s.IsCastable).ToList();
			List<Skill> source4 = source3.Where((Skill s) => !Config.TotemSkillsList.Any((SqRoutineSettings.TotemSkillEntry e) => e.Name.Equals(s.Name))).ToList();
			if (source4.Any())
			{
				string text3 = "Totems skills changed. Please check your SqRoutine settings and refresh totem skills. Added skills: " + string.Join(",", source4.Select((Skill sk) => sk.Name));
				GlobalLog.Error(text3);
				BotManager.Stop(new StopReasonData("totem_skills_changed", text3, (object)null), false);
			}
			bool_10 = true;
		}
		if (!bool_3 && list_0.Any())
		{
			foreach (Skill item2 in list_0.Where((Skill s) => (s.SkillTags.Contains("curse") || s.Stats.ContainsKey((StatTypeGGG)1614)) && !s.Name.Contains("Mark") && s.Slot != -1 && s.IsCastable && !s.IsAurifiedCurse))
			{
				bool flag = item2.Stats.ContainsKey((StatTypeGGG)1614);
				if (!item2.Stats.ContainsKey((StatTypeGGG)12411))
				{
					GlobalLog.Warn($"[{Name}] Found curse: {item2.Name} [{item2.BoundKey}]. Hextouch? {flag}");
					hashSet_1.Add(item2.Slot);
				}
				else
				{
					GlobalLog.Warn($"[{Name}] Found curse: {item2.Name} [{item2.BoundKey}]. But it's supported by Impending Doom. Treating it as spammable skill.");
				}
			}
			bool_3 = true;
		}
		if (LokiPoe.Me.IsInHideout || LokiPoe.Me.IsInTown)
		{
			return;
		}
		if (interval_5.Elapsed)
		{
			bool_0 = false;
			list_0.Clear();
			list_1.Clear();
			list_0 = SkillBarHud.Skills.Where((Skill s) => (RemoteMemoryObject)(object)s != (RemoteMemoryObject)null).ToList();
			list_1 = list_0.FindAll((Skill s) => s.IsCastable && !s.SkillTags.Contains("totem") && !s.Stats.ContainsKey((StatTypeGGG)2330));
		}
		foreach (KeyValuePair<int, DateTime> item3 in dictionary_1.Where((KeyValuePair<int, DateTime> kv) => kv.Value.AddSeconds(2.0) < DateTime.Now).ToList())
		{
			dictionary_1.Remove(item3.Key);
		}
		foreach (KeyValuePair<int, DateTime> item4 in dictionary_0.Where((KeyValuePair<int, DateTime> kv) => kv.Value.AddSeconds(5.0) < DateTime.Now).ToList())
		{
			dictionary_0.Remove(item4.Key);
		}
		if ((CachedMonolith == null || CachedMonolith.Object == (NetworkObject)null) && LegionMonolith != (NetworkObject)null)
		{
			CachedMonolith = new CachedObject(LegionMonolith);
		}
		if (!Config.SaveAnime || bool_5 || !((IAuthored)BotManager.Current).Name.Equals("MapBotEx"))
		{
			return;
		}
		Skill val2 = list_0.FirstOrDefault((Skill s) => s.InternalId.Equals("animate_armour"));
		Monster val3 = null;
		if ((RemoteMemoryObject)(object)val2 != (RemoteMemoryObject)null && val2.DeployedObjects.Any())
		{
			NetworkObject obj = val2.DeployedObjects.FirstOrDefault();
			val3 = (Monster)(object)((obj is Monster) ? obj : null);
		}
		if ((NetworkObject)(object)val3 != (NetworkObject)null)
		{
			double num = Math.Floor(((Actor)val3).HealthPercent);
			if (num <= (double)Config.SaveAnimePct)
			{
				GlobalLog.Warn($"[{Name}] Animate Guardian is on low health! ({num:0.00}%)/{Config.SaveAnimePct}%");
				GlobalLog.Warn("[" + Name + "] Going to logout now to save it.");
				bool_5 = true;
			}
		}
	}

	public void Stop()
	{
	}

	static SqRoutine()
	{
		Config = SqRoutineSettings.Instance;
		interval_0 = new Interval(450);
		interval_1 = new Interval(LokiPoe.Random.Next(1700, 2400));
		interval_2 = new Interval(LokiPoe.Random.Next(1700, 2400));
		interval_3 = new Interval(LokiPoe.Random.Next(700, 1100));
		interval_4 = new Interval(LokiPoe.Random.Next(1000, 1400));
		interval_5 = new Interval(5000);
		interval_6 = new Interval(15000);
		hashSet_0 = new HashSet<int>();
		dictionary_0 = new Dictionary<int, DateTime>();
		dictionary_1 = new Dictionary<int, DateTime>();
		hashSet_1 = new HashSet<int>();
		list_0 = new List<Skill>();
		list_1 = new List<Skill>();
		list_2 = new List<Skill>();
		int_0 = 999999;
	}
}
