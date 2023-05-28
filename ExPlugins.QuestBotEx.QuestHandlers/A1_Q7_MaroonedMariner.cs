using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q7_MaroonedMariner
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static Chest SlaveGirl => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2564 }).FirstOrDefault<Chest>();

	private static Npc CaptainFairgraves => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)5 }).FirstOrDefault((Npc n) => (int)((NetworkObject)n).Reaction == -1);

	private static Monster SkeleFairgraves => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)5 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3 && (int)((NetworkObject)m).Reaction == 1);

	private static CachedObject CachedSlaveGirl
	{
		get
		{
			return CombatAreaCache.Current.Storage["SlaveGirl"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["SlaveGirl"] = value;
		}
	}

	private static WalkablePosition CachedFairgravesPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["FairgravesPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["FairgravesPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act1.ShipGraveyardCave.IsCurrentArea)
		{
			if (World.Act1.ShipGraveyard.IsCurrentArea && CachedFairgravesPos == null)
			{
				Npc captainFairgraves = CaptainFairgraves;
				if ((NetworkObject)(object)captainFairgraves != (NetworkObject)null)
				{
					CachedFairgravesPos = ((NetworkObject)(object)captainFairgraves).WalkablePosition();
				}
			}
		}
		else if (CachedSlaveGirl == null)
		{
			Chest slaveGirl = SlaveGirl;
			if ((NetworkObject)(object)slaveGirl != (NetworkObject)null)
			{
				CachedSlaveGirl = new CachedObject((NetworkObject)(object)slaveGirl);
			}
		}
	}

	public static async Task<bool> GrabAllflame()
	{
		if (!Helpers.PlayerHasQuestItem("AllFlameLantern"))
		{
			if (!World.Act1.ShipGraveyardCave.IsCurrentArea)
			{
				await Travel.To(World.Act1.ShipGraveyardCave);
				return true;
			}
			if (await Helpers.OpenQuestChest(CachedSlaveGirl))
			{
				return true;
			}
			tgtPosition_0.Come();
			return true;
		}
		return false;
	}

	public static async Task<bool> KillFairgraves()
	{
		if (!World.Act1.ShipGraveyard.IsCurrentArea)
		{
			if (World.Act1.ShipGraveyardCave.IsCurrentArea)
			{
				AreaTransition transition = ObjectManager.Objects.FirstOrDefault((AreaTransition a) => a.LeadsTo(World.Act1.ShipGraveyard));
				if ((NetworkObject)(object)transition != (NetworkObject)null)
				{
					if (!(await PlayerAction.TakeTransition(transition)))
					{
						ErrorManager.ReportError();
					}
				}
				else if (!(await PlayerAction.TpToTown()))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			await Travel.To(World.Act1.ShipGraveyard);
			return true;
		}
		Monster skeleFairgraves = SkeleFairgraves;
		if ((NetworkObject)(object)skeleFairgraves != (NetworkObject)null)
		{
			if (((Actor)skeleFairgraves).IsDead)
			{
				return false;
			}
			await Helpers.MoveAndWait(((NetworkObject)(object)skeleFairgraves).WalkablePosition());
			return true;
		}
		WalkablePosition fairgravesPos = CachedFairgravesPos;
		if (fairgravesPos != null)
		{
			if (!fairgravesPos.IsFar)
			{
				Npc fairgraves = CaptainFairgraves;
				if ((NetworkObject)(object)fairgraves == (NetworkObject)null)
				{
					GlobalLog.Debug("[MaroonedMariner] We are near Fairgraves position but neither NPC nor Monster Fairgraves exists.");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				if (!((NetworkObject)fairgraves).IsTargetable)
				{
					GlobalLog.Debug("[MaroonedMariner] We are near Fairgraves position but NPC Fairgraves is not targetable and Monster does not exist.");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				if (!(await PlayerAction.Interact((NetworkObject)(object)fairgraves)))
				{
					ErrorManager.ReportError();
					return true;
				}
				await Coroutines.CloseBlockingWindows();
				await Coroutines.ReactionWait();
				await Coroutines.ReactionWait();
				await Coroutines.ReactionWait();
				return true;
			}
			fairgravesPos.Come();
			return true;
		}
		tgtPosition_1.Come();
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (!World.Act1.ShipGraveyard.IsCurrentArea)
		{
			return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Bestel, "Fairgraves Reward", null, "Book-a1q6");
		}
		await Travel.To(World.Act1.CavernOfWrath);
		return true;
	}

	static A1_Q7_MaroonedMariner()
	{
		tgtPosition_0 = new TgtPosition("Slave Girl location", "boat_small_damaged_allflame_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Captain Fairgraves location", "shipwreck_quest_v01_01.tgt");
	}
}
