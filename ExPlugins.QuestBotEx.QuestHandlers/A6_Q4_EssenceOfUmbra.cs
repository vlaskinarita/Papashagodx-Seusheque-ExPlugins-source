using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q4_EssenceOfUmbra
{
	private static NetworkObject networkObject_0;

	private static Monster monster_0;

	private static Monster monster_1;

	private static bool bool_0;

	public static void Tick()
	{
		bool_0 = World.Act6.PrisonerGate.IsWaypointOpened;
	}

	public static async Task<bool> KillShavronne()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act6.ShavronneTower.IsCurrentArea)
		{
			UpdateShavronneFightObjects();
			if (networkObject_0 != (NetworkObject)null)
			{
				if (!((NetworkObject)(object)monster_1 != (NetworkObject)null) || !monster_1.IsActive)
				{
					if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
					{
						int distance = (monster_0.IsActive ? 20 : 35);
						WalkablePosition pos = ((NetworkObject)(object)monster_0).WalkablePosition();
						if (pos.Distance > distance)
						{
							pos.Come();
							return true;
						}
						GlobalLog.Debug("Waiting for " + pos.Name);
						await Coroutines.FinishCurrentAction(true);
						await Wait.StuckDetectionSleep(200);
						return true;
					}
					await Helpers.MoveAndWait(networkObject_0.WalkablePosition(), "Waiting for any Shavronne fight object");
					return true;
				}
				await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
				return true;
			}
		}
		await Travel.To(World.Act6.PrisonerGate);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, TownNpcs.townNpc_0, "Shavronne Reward", Quests.EssenceOfUmbra.Id, null, shouldLogOut: true);
	}

	private static void UpdateShavronneFightObjects()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		networkObject_0 = null;
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			string metadata = @object.Metadata;
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null) || (int)val.Rarity != 3)
			{
				if (metadata == "Metadata/Monsters/Shavronne/ShavronneArenaMiddle")
				{
					networkObject_0 = @object;
				}
			}
			else if (!(metadata == "Metadata/Monsters/Shavronne/ShavronneTower"))
			{
				if (metadata == "Metadata/Monsters/Brute/BruteTower")
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
