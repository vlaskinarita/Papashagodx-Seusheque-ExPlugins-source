using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q2_BestelEpic
{
	private static readonly TgtPosition tgtPosition_0;

	private static Chest StorageChest => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2577 }).FirstOrDefault<Chest>();

	private static CachedObject CachedStorageChest
	{
		get
		{
			return CombatAreaCache.Current.Storage["StorageChest"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["StorageChest"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act6.TidalIsland.IsCurrentArea && CachedStorageChest == null)
		{
			Chest storageChest = StorageChest;
			if ((NetworkObject)(object)storageChest != (NetworkObject)null)
			{
				CachedStorageChest = new CachedObject((NetworkObject)(object)storageChest);
			}
		}
	}

	public static async Task<bool> GrabManuscript()
	{
		if (Helpers.PlayerHasQuestItem("BestelsManuscript"))
		{
			return false;
		}
		if (World.Act6.TidalIsland.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedStorageChest))
			{
				return true;
			}
			tgtPosition_0.Come();
			return true;
		}
		await Travel.To(World.Act6.TidalIsland);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, TownNpcs.BestelA6, "Bestel's Epic Reward", Quests.BestelEpic.Id);
	}

	static A6_Q2_BestelEpic()
	{
		tgtPosition_0 = new TgtPosition("Storage chest location", "kyrenia_boat_medicinequest_v01_01.tgt");
	}
}
