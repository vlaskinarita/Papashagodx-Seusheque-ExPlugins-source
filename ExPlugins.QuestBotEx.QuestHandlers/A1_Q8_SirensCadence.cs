using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q8_SirensCadence
{
	private static bool bool_0;

	private static Monster monster_0;

	private static Monster monster_1;

	public static void Tick()
	{
		bool_0 = World.Act2.SouthernForest.IsWaypointOpened;
	}

	public static async Task<bool> KillMerveil()
	{
		if (!bool_0)
		{
			if (World.Act1.CavernOfAnger.IsCurrentArea)
			{
				UpdateMerveilFightObjects();
				if ((NetworkObject)(object)monster_1 != (NetworkObject)null)
				{
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
					return true;
				}
				if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
				{
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_0).WalkablePosition(10, 50));
					return true;
				}
			}
			await Travel.To(World.Act2.SouthernForest);
			return true;
		}
		return false;
	}

	public static async Task<bool> EnterForestEncampment()
	{
		if (World.Act2.ForestEncampment.IsCurrentArea)
		{
			if (!World.Act2.ForestEncampment.IsWaypointOpened)
			{
				if (!(await PlayerAction.OpenWaypoint()))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			return false;
		}
		await Travel.To(World.Act2.ForestEncampment);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Nessa, "Merveil Reward", Quests.SirensCadence.Id);
	}

	private static void UpdateMerveilFightObjects()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null) || (int)val.Rarity != 3)
			{
				continue;
			}
			if (!((NetworkObject)val).Metadata.EqualsIgnorecase("Metadata/Monsters/seawitchillusion/BossMerveil"))
			{
				if (((NetworkObject)val).Metadata == "Metadata/Monsters/Seawitch/BossMerveil2")
				{
					monster_1 = val;
				}
			}
			else
			{
				monster_0 = val;
			}
		}
	}
}
