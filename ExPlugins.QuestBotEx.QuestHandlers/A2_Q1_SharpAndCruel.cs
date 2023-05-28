using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q1_SharpAndCruel
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Weaver => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)556 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
	}

	public static async Task<bool> KillWeaver()
	{
		if (!Helpers.PlayerHasQuestItem("PoisonSpear"))
		{
			if (World.Act2.WeaverChambers.IsCurrentArea)
			{
				Monster weaver = Weaver;
				if ((NetworkObject)(object)weaver != (NetworkObject)null && ((NetworkObject)(object)weaver).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)weaver);
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act2.WeaverChambers);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act2.ForestEncampment, TownNpcs.Silk, "Maligaro's Spike Reward", Quests.SharpAndCruel.Id);
	}

	static A2_Q1_SharpAndCruel()
	{
		tgtPosition_0 = new TgtPosition("Weaver room", "spidergrove_exit_v01_01.tgt");
	}
}
