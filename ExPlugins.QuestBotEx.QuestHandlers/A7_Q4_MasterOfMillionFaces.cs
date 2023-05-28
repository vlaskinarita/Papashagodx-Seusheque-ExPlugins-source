using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q4_MasterOfMillionFaces
{
	private static bool bool_0;

	private static NetworkObject RalakeshRoomObj => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/MiscellaneousObjects/ArenaMiddle");

	public static void Tick()
	{
		bool_0 = World.Act7.NorthernForest.IsWaypointOpened;
	}

	public static async Task<bool> KillRalakesh()
	{
		if (!bool_0)
		{
			if (World.Act7.AshenFields.IsCurrentArea)
			{
				NetworkObject roomObj = RalakeshRoomObj;
				if (roomObj != (NetworkObject)null && roomObj.PathExists())
				{
					Monster mob = Helpers.ClosestActiveMob;
					if (!((NetworkObject)(object)mob != (NetworkObject)null) || !((NetworkObject)(object)mob).PathExists())
					{
						await Helpers.MoveAndWait(roomObj, "Waiting for any Ralakesh fight object");
						return true;
					}
					PlayerMoverManager.MoveTowards(((NetworkObject)mob).Position, (object)null);
					return true;
				}
			}
			await Travel.To(World.Act7.NorthernForest);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act7.BridgeEncampment, TownNpcs.EramirA7, "Ralakesh Reward", null, "Book-a7q1");
	}
}
