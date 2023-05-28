using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q5_DeathAndRebirth
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TownNpc townNpc_0;

	private static Monster Avarius => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1973 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
	}

	public static async Task<bool> KillAvarius()
	{
		if (!Helpers.PlayerHasQuestItem("AvariusStaff"))
		{
			if (World.Act10.DesecratedChambers.IsCurrentArea)
			{
				Monster avarius = Avarius;
				if ((NetworkObject)(object)avarius != (NetworkObject)null && ((NetworkObject)(object)avarius).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)avarius);
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act10.DesecratedChambers);
			return true;
		}
		return false;
	}

	public static async Task<bool> TurnInStaff()
	{
		if (Helpers.PlayerHasQuestItem("AvariusStaff"))
		{
			if (World.Act10.OriathDocks.IsCurrentArea)
			{
				if (!(await townNpc_0.Talk()))
				{
					ErrorManager.ReportError();
				}
				else
				{
					await Coroutines.CloseBlockingWindows();
				}
				return true;
			}
			await Travel.To(World.Act10.OriathDocks);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.LaniA10, "Innocence Reward", Quests.DeathAndRebirth.Id);
	}

	static A10_Q5_DeathAndRebirth()
	{
		tgtPosition_0 = new TgtPosition("Avarius room", "transition_chamber_to_boss_shattered_v01_01.tgt");
		townNpc_0 = new TownNpc(new WalkablePosition("Bannon", 470, 295));
	}
}
