using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q6_SwigOfHope
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static Chest OrnateChest => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2568 }).FirstOrDefault<Chest>();

	private static Chest PlumTree => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2567 }).FirstOrDefault<Chest>();

	private static Npc Fairgraves => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)5 }).FirstOrDefault<Npc>();

	private static CachedObject CachedOrnateChest
	{
		get
		{
			return CombatAreaCache.Current.Storage["OrnateChest"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["OrnateChest"] = value;
		}
	}

	private static CachedObject CachedPlumTree
	{
		get
		{
			return CombatAreaCache.Current.Storage["PlumTree"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["PlumTree"] = value;
		}
	}

	private static CachedObject CachedFairgraves
	{
		get
		{
			return CombatAreaCache.Current.Storage["CaptainFairgraves"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["CaptainFairgraves"] = value;
		}
	}

	public static void Tick()
	{
		string id = World.CurrentArea.Id;
		if (id == World.Act3.Marketplace.Id)
		{
			if (CachedOrnateChest == null)
			{
				Chest ornateChest = OrnateChest;
				if ((NetworkObject)(object)ornateChest != (NetworkObject)null)
				{
					CachedOrnateChest = new CachedObject((NetworkObject)(object)ornateChest);
				}
			}
		}
		else if (!(id == World.Act3.ImperialGardens.Id))
		{
			if (id == World.Act3.Docks.Id && CachedFairgraves == null)
			{
				Npc fairgraves = Fairgraves;
				if ((NetworkObject)(object)fairgraves != (NetworkObject)null)
				{
					CachedFairgraves = new CachedObject((NetworkObject)(object)fairgraves);
				}
			}
		}
		else if (CachedPlumTree == null)
		{
			Chest plumTree = PlumTree;
			if ((NetworkObject)(object)plumTree != (NetworkObject)null)
			{
				CachedPlumTree = new CachedObject((NetworkObject)(object)plumTree);
			}
		}
	}

	public static async Task<bool> GrabDecanter()
	{
		if (Helpers.PlayerHasQuestItem("Fairgraves/Decanter"))
		{
			return false;
		}
		if (!World.Act3.Marketplace.IsCurrentArea)
		{
			if (ErrorManager.ErrorCount >= 8)
			{
				ErrorManager.Reset();
				Travel.RequestNewInstance(World.Act3.Sewers);
				await PlayerAction.TpToTown();
			}
			await Travel.To(World.Act3.Marketplace);
			return true;
		}
		if (await Helpers.OpenQuestChest(CachedOrnateChest))
		{
			return true;
		}
		tgtPosition_0.Come();
		return true;
	}

	public static async Task<bool> GrabPlum()
	{
		if (!Helpers.PlayerHasQuestItem("Fairgraves/Fruit"))
		{
			if (!World.Act3.ImperialGardens.IsCurrentArea)
			{
				await Travel.To(World.Act3.ImperialGardens);
				return true;
			}
			if (await Helpers.OpenQuestChest(CachedPlumTree))
			{
				return true;
			}
			tgtPosition_1.Come();
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act3.Docks.IsCurrentArea)
		{
			CachedObject fairgraves = CachedFairgraves;
			if (fairgraves != null)
			{
				WalkablePosition pos = fairgraves.Position;
				if (!pos.IsFar)
				{
					string reward = QuestBotSettings.Instance.GetRewardForQuest(Quests.SwigOfHope.Id);
					if (!(await fairgraves.Object.AsTownNpc().TakeReward(reward, "Swig of Hope Reward")))
					{
						ErrorManager.ReportError();
					}
					return false;
				}
				pos.Come();
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act3.Docks);
		return true;
	}

	static A3_Q6_SwigOfHope()
	{
		tgtPosition_0 = new TgtPosition("Decanter Spiritus location", "market_place_straight_v01_01_unique1.tgt");
		tgtPosition_1 = new TgtPosition("Plum tree location", "fruittree.tgt");
	}
}
