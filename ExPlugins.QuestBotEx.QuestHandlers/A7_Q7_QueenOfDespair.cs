using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q7_QueenOfDespair
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster Gruthkul => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2242 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.QueenOfDespair) <= 2;
	}

	public static async Task<bool> KillGruthkul()
	{
		if (!bool_0)
		{
			if (World.Act7.DreadThicket.IsCurrentArea)
			{
				Monster gruthkul = Gruthkul;
				if ((NetworkObject)(object)gruthkul != (NetworkObject)null && ((NetworkObject)(object)gruthkul).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)gruthkul);
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act7.DreadThicket);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.EramirA7, "Gruthkul Reward", null, "Book-a7q9");
	}

	static A7_Q7_QueenOfDespair()
	{
		tgtPosition_0 = new TgtPosition("Gruthkul room", "forestcave_entrance_hole_v01_01.tgt");
	}
}
