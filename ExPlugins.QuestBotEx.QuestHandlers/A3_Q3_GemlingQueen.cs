using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q3_GemlingQueen
{
	private static readonly TgtPosition eAautejaeG;

	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static Chest BlackguardChest => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2570 }).FirstOrDefault<Chest>();

	private static Chest SupplyContainer => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2571 }).FirstOrDefault<Chest>();

	private static CachedObject CachedBlackguardChest
	{
		get
		{
			return CombatAreaCache.Current.Storage["BlackguardChest"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["BlackguardChest"] = value;
		}
	}

	private static CachedObject CachedSupplyContainer
	{
		get
		{
			return CombatAreaCache.Current.Storage["SupplyContainer"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["SupplyContainer"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act3.Battlefront.IsCurrentArea)
		{
			if (World.Act3.Docks.IsCurrentArea && CachedSupplyContainer == null)
			{
				Chest supplyContainer = SupplyContainer;
				if ((NetworkObject)(object)supplyContainer != (NetworkObject)null)
				{
					CachedSupplyContainer = new CachedObject((NetworkObject)(object)supplyContainer);
				}
			}
		}
		else if (CachedBlackguardChest == null)
		{
			Chest blackguardChest = BlackguardChest;
			if ((NetworkObject)(object)blackguardChest != (NetworkObject)null)
			{
				CachedBlackguardChest = new CachedObject((NetworkObject)(object)blackguardChest);
			}
		}
	}

	public static async Task<bool> GrabRibbon()
	{
		if (Helpers.PlayerHasQuestItem("RibbonSpool"))
		{
			return false;
		}
		if (!World.Act3.Battlefront.IsCurrentArea)
		{
			await Travel.To(World.Act3.Battlefront);
			return true;
		}
		if (await Helpers.OpenQuestChest(CachedBlackguardChest))
		{
			return true;
		}
		eAautejaeG.Come();
		return true;
	}

	public static async Task<bool> GrabSulphite()
	{
		if (!Helpers.PlayerHasQuestItem("SulphiteFlask"))
		{
			if (World.Act3.Docks.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedSupplyContainer))
				{
					return true;
				}
				tgtPosition_0.Come();
				return true;
			}
			await Travel.To(World.Act3.Docks);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeRibbonReward()
	{
		return await TakeDiallaReward(talc: false);
	}

	public static async Task<bool> TakeTalcReward()
	{
		return await TakeDiallaReward(talc: true);
	}

	private static async Task<bool> TakeDiallaReward(bool talc)
	{
		if (World.Act3.SolarisTemple2.IsCurrentArea)
		{
			Npc dialla = Helpers.LadyDialla;
			if ((NetworkObject)(object)dialla != (NetworkObject)null)
			{
				WalkablePosition pos = ((NetworkObject)(object)dialla).WalkablePosition();
				if (pos.IsFar)
				{
					pos.Come();
					return true;
				}
				if (!talc)
				{
					string reward = QuestBotSettings.Instance.GetRewardForQuest(Quests.RibbonSpool.Id);
					if (!(await ((NetworkObject)(object)dialla).AsTownNpc().TakeReward(reward, "Ribbon Spool Reward")))
					{
						ErrorManager.ReportError();
					}
				}
				else if (!(await ((NetworkObject)(object)dialla).AsTownNpc().TakeReward(null, "Take Infernal Talc")))
				{
					ErrorManager.ReportError();
				}
				return false;
			}
			if (!tgtPosition_1.IsFar)
			{
				GlobalLog.Debug("[GemlingQueen] We are near Dialla tgt but NPC object is null.");
				await Wait.StuckDetectionSleep(500);
				return true;
			}
			tgtPosition_1.Come();
			return true;
		}
		await Travel.To(World.Act3.SolarisTemple2);
		return true;
	}

	static A3_Q3_GemlingQueen()
	{
		eAautejaeG = new TgtPosition("Ribbon Spool location", "act3_brokenbridge_v01_01.tgt");
		tgtPosition_0 = new TgtPosition("Thaumetic Sulphite location", "templeruinforest_questcart.tgt");
		tgtPosition_1 = new TgtPosition("Lady Dialla location", "gemling_queen_throne_v01_01.tgt");
	}
}
