using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q3_VilentaVengeance
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster Vilenta => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)96 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.VilentaVengeance) <= 2;
	}

	public static async Task<bool> KillVilenta()
	{
		if (!bool_0)
		{
			if (World.Act10.ControlBlocks.IsCurrentArea)
			{
				Monster vilenta = Vilenta;
				if ((NetworkObject)(object)vilenta != (NetworkObject)null && ((NetworkObject)(object)vilenta).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)vilenta);
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act10.ControlBlocks);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.LaniA10, "Vilenta Reward", null, "Book-a10q6");
	}

	static A10_Q3_VilentaVengeance()
	{
		tgtPosition_0 = new TgtPosition("Vilenta room", "slave_ledge_doubledoor_transition_v01_01.tgt");
	}
}
