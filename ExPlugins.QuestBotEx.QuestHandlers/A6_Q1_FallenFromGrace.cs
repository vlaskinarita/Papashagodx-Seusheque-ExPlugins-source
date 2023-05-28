using System.Threading.Tasks;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q1_FallenFromGrace
{
	private static bool bool_0;

	private static TownNpc townNpc_0;

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.FallenFromGrace) <= 2;
	}

	public static async Task<bool> ClearStrand()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act6.TwilightStrand.IsCurrentArea)
		{
			if (await TrackMobLogic.Execute())
			{
				return true;
			}
			if (!(await CombatAreaCache.Current.Explorer.Execute()))
			{
				if (QuestManager.GetState(Quests.FallenFromGrace) <= 2)
				{
					return false;
				}
				GlobalLog.Error("[ClearTwilightStrand] Twilight Strand is fully explored but not all monsters were killed. Now going to create a new Twilight Strand instance.");
				Travel.RequestNewInstance(World.Act6.TwilightStrand);
				if (!(await PlayerAction.TpToTown()))
				{
					ErrorManager.ReportError();
				}
			}
			return true;
		}
		await Travel.To(World.Act6.TwilightStrand);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act6.LioneyeWatch.IsCurrentArea)
		{
			await townNpc_0.Position.ComeAtOnce();
			if (townNpc_0.NpcObject == (NetworkObject)null)
			{
				if (townNpc_0 == TownNpcs.LillyRoth)
				{
					GlobalLog.Error("[FallenFromGrace] Unknown error. There is no Lilly Roth.");
					ErrorManager.ReportCriticalError();
					return true;
				}
				GlobalLog.Warn("[FallenFromGrace] There is no Lilly Roth near ship. Now going to her default position.");
				townNpc_0 = TownNpcs.LillyRoth;
				return true;
			}
			return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, townNpc_0, "Twilight Strand Reward", null, "Book-a6q4");
		}
		await Travel.To(World.Act6.LioneyeWatch);
		return true;
	}

	static A6_Q1_FallenFromGrace()
	{
		townNpc_0 = new TownNpc(new WalkablePosition("Lilly Roth", 229, 158));
	}
}
