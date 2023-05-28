using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A8_Q5_ReflectionOfTerror
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static NetworkObject YugulRoomObj => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Monsters/Frog/FrogGod/SilverStatueRoots");

	private static Monster Yugul => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2245 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.ReflectionOfTerror) <= 2;
	}

	public static async Task<bool> KillYugul()
	{
		if (!bool_0)
		{
			if (World.Act8.HighGardens.IsCurrentArea)
			{
				NetworkObject roomobj = YugulRoomObj;
				if (roomobj != (NetworkObject)null)
				{
					Monster yugul = Yugul;
					if ((NetworkObject)(object)yugul != (NetworkObject)null)
					{
						await Helpers.MoveToBossOrAnyMob(yugul);
						return true;
					}
					await Helpers.MoveAndWait(roomobj.WalkablePosition(), "Waiting for any Yugul fight object");
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act8.HighGardens);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act8.SarnEncampment, TownNpcs.HarganA8, "Yugul Reward", null, "Book-a8q4", shouldLogOut: true);
	}

	static A8_Q5_ReflectionOfTerror()
	{
		tgtPosition_0 = new TgtPosition("Yugul room", "garden_wall_entrance_v01_01.tgt");
	}
}
