using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q5_ClovenOne
{
	private static bool bool_0;

	private static Monster Abberath => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2234 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.ClovenOne) <= 2;
	}

	public static async Task<bool> KillAbberath()
	{
		if (!bool_0)
		{
			if (World.Act6.PrisonerGate.IsCurrentArea)
			{
				Monster abberath = Abberath;
				if ((NetworkObject)(object)abberath != (NetworkObject)null && ((NetworkObject)(object)abberath).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)abberath);
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act6.PrisonerGate);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, TownNpcs.BestelA6, "rath Reward", null, "Book-a6q7");
	}
}
