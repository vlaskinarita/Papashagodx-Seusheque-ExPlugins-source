using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q5_InMemoryOfGreust
{
	private static readonly TgtPosition tgtPosition_0;

	private static NetworkObject AzmeriShrine => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act7/GreustShrine");

	private static CachedObject CachedAzmeriShrine
	{
		get
		{
			return CombatAreaCache.Current.Storage["AzmeriShrine"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["AzmeriShrine"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act7.NorthernForest.IsCurrentArea && CachedAzmeriShrine == null)
		{
			NetworkObject azmeriShrine = AzmeriShrine;
			if (azmeriShrine != (NetworkObject)null)
			{
				CachedAzmeriShrine = new CachedObject(azmeriShrine);
			}
		}
	}

	public static async Task<bool> PlaceNecklace()
	{
		if (Helpers.PlayerHasQuestItem("GreustNecklace"))
		{
			if (!World.Act7.NorthernForest.IsCurrentArea)
			{
				await Travel.To(World.Act7.NorthernForest);
				return true;
			}
			if (!(await Helpers.HandleQuestObject(CachedAzmeriShrine)))
			{
				tgtPosition_0.Come();
				return true;
			}
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeNecklace()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.HelenaA7, "Take Greust's Necklace");
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.HelenaA7, "Greust's Necklace Reward", Quests.InMemoryOfGreust.Id);
	}

	static A7_Q5_InMemoryOfGreust()
	{
		tgtPosition_0 = new TgtPosition("Azmeri Shrine location", "forest_azmeri_shrine.tgt");
	}
}
