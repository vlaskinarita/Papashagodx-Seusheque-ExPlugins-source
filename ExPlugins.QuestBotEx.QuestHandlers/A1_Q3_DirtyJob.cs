using System.Threading.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q3_DirtyJob
{
	private static bool bool_0;

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.DirtyJob) <= 3;
	}

	public static async Task<bool> ClearFetidPool()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act1.FetidPool.IsCurrentArea)
		{
			if (await TrackMobLogic.Execute())
			{
				return true;
			}
			if (!(await CombatAreaCache.Current.Explorer.Execute()))
			{
				if (QuestManager.GetState(Quests.DirtyJob) <= 3)
				{
					return false;
				}
				GlobalLog.Error("[ClearFetidPool] Fetid Pool is fully explored but not all monsters were killed. Now going to create a new Fetid Pool instance.");
				Travel.RequestNewInstance(World.Act1.FetidPool);
				if (!(await PlayerAction.TpToTown()))
				{
					ErrorManager.ReportError();
				}
			}
			return true;
		}
		await Travel.To(World.Act1.FetidPool);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Tarkleigh, "Necromancer Reward", null, "Book-a1q8");
	}
}
