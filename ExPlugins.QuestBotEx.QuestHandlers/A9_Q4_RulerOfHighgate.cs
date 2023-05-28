using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A9_Q4_RulerOfHighgate
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Garukhan => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2263 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
	}

	public static async Task<bool> KillGarukhan()
	{
		if (!Helpers.PlayerHasQuestItem("SekhemaFeather"))
		{
			if (World.Act9.Quarry.IsCurrentArea)
			{
				Monster garukhan = Garukhan;
				if ((NetworkObject)(object)garukhan != (NetworkObject)null)
				{
					if (!garukhan.IsActive)
					{
						Monster mob = Helpers.ClosestActiveMob;
						if ((NetworkObject)(object)mob != (NetworkObject)null && ((NetworkObject)(object)mob).PathExists())
						{
							PlayerMoverManager.MoveTowards(((NetworkObject)mob).Position, (object)null);
							return true;
						}
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)garukhan).WalkablePosition());
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act9.Quarry);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeIrashaReward()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.Irasha, "Feather Reward", null, "Book-a9q2");
	}

	public static async Task<bool> TakeTasuniReward()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.TasuniA9, "Feather Reward", null, "Book-a9q2");
	}

	static A9_Q4_RulerOfHighgate()
	{
		tgtPosition_0 = new TgtPosition("Garukhan room", "GarukhanArena_entrance_v01_01.tgt", closest: false, 8, 20);
	}
}
