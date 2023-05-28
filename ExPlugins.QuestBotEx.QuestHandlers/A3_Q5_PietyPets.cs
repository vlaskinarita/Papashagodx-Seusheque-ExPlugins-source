using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q5_PietyPets
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Piety => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)24 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
	}

	public static async Task<bool> KillPiety()
	{
		if (Helpers.PlayerHasQuestItem("TowerKey"))
		{
			return false;
		}
		if (World.Act3.LunarisTemple2.IsCurrentArea)
		{
			Monster piety = Piety;
			if ((NetworkObject)(object)piety != (NetworkObject)null)
			{
				await Helpers.MoveAndWait(((NetworkObject)(object)piety).WalkablePosition());
				return true;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
			return true;
		}
		await Travel.To(World.Act3.LunarisTemple2);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act3.SarnEncampment, TownNpcs.Grigor, "Piety Reward", null, "Book-a3q9");
	}

	static A3_Q5_PietyPets()
	{
		tgtPosition_0 = new TgtPosition("Piety room", "templeclean_prepiety_roundtop_center_01.tgt");
	}
}
