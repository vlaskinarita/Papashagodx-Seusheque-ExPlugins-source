using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q6_PuppetMistress
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster Ryslatha => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)794 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster ClosestRyslathaNest => ((IEnumerable)ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1826 })).Closest<Monster>((Func<Monster, bool>)((Monster m) => (int)m.Rarity == 3 && !((Actor)m).IsDead));

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.PuppetMistress) <= 2;
	}

	public static async Task<bool> KillRyslatha()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act6.Wetlands.IsCurrentArea)
		{
			Monster ryslatha = Ryslatha;
			if ((NetworkObject)(object)ryslatha != (NetworkObject)null)
			{
				Monster nest = ClosestRyslathaNest;
				if ((NetworkObject)(object)nest != (NetworkObject)null)
				{
					await Helpers.MoveAndWait(((NetworkObject)(object)nest).WalkablePosition());
					return true;
				}
				await Helpers.MoveAndWait(((NetworkObject)(object)ryslatha).WalkablePosition());
				return true;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
			return true;
		}
		await Travel.To(World.Act6.Wetlands);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, TownNpcs.townNpc_0, "Puppet Mistress Reward", null, "Book-a6q6");
	}

	static A6_Q6_PuppetMistress()
	{
		tgtPosition_0 = new TgtPosition("Ryslatha room", "forest_caveentrance_v01_01.tgt", closest: true);
	}
}
