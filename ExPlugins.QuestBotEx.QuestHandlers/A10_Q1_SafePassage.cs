using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q1_SafePassage
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Npc Bannon => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)98 }).FirstOrDefault<Npc>();

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.SafePassage) <= 4;
	}

	public static async Task<bool> SaveBannon()
	{
		if (!bool_0)
		{
			if (World.Act10.CathedralRooftop.IsCurrentArea)
			{
				Npc bannon = Bannon;
				if ((NetworkObject)(object)bannon != (NetworkObject)null && ((NetworkObject)(object)bannon).PathExists())
				{
					Monster mob = Helpers.ClosestActiveMob;
					if ((NetworkObject)(object)mob != (NetworkObject)null && ((NetworkObject)(object)mob).PathExists())
					{
						await Helpers.MoveAndWait((NetworkObject)(object)mob);
						return true;
					}
					await Helpers.MoveAndWait((NetworkObject)(object)bannon, "Waiting for any active monster");
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act10.CathedralRooftop);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.LaniA10, "Bannon Reward", Quests.SafePassage.Id);
	}

	static A10_Q1_SafePassage()
	{
		tgtPosition_0 = new TgtPosition("Bannon room", "cathedralroof_boss_area_v02_01.tgt");
	}
}
