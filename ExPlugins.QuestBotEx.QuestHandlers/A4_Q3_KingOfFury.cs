using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A4_Q3_KingOfFury
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Kaom => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)27 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
	}

	public static async Task<bool> KillKaom()
	{
		if (!Helpers.PlayerHasQuestItem("KaomGem"))
		{
			if (World.Act4.KaomStronghold.IsCurrentArea)
			{
				Monster kaom = Kaom;
				if ((NetworkObject)(object)kaom != (NetworkObject)null && ((NetworkObject)(object)kaom).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)kaom);
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
				return true;
			}
			await Travel.To(World.Act4.KaomStronghold);
			return true;
		}
		return false;
	}

	public static async Task<bool> TurnInQuest()
	{
		if (Helpers.PlayerHasQuestItem("KaomGem"))
		{
			if (World.Act4.CrystalVeins.IsCurrentArea)
			{
				Npc dialla = Helpers.LadyDialla;
				if ((NetworkObject)(object)dialla == (NetworkObject)null)
				{
					GlobalLog.Error("[KingOfFury] Fail to detect Lady Dialla in Crystal Veins.");
					ErrorManager.ReportCriticalError();
					return true;
				}
				await Helpers.TalkTo((NetworkObject)(object)dialla);
				return true;
			}
			await Travel.To(World.Act4.CrystalVeins);
			return true;
		}
		return false;
	}

	static A4_Q3_KingOfFury()
	{
		tgtPosition_0 = new TgtPosition("Kaom room", "lava_lake_throne_room_v0?_0?.tgt | lava_lake_throne_room_v0?_0?.tgt");
	}
}
