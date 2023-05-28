using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q4_DeathToPurity
{
	private static readonly TgtPosition tgtPosition_0;

	private static NetworkObject networkObject_0;

	private static Monster monster_0;

	private static Monster monster_1;

	private static Npc npc_0;

	public static void Tick()
	{
	}

	public static async Task<bool> KillAvarius()
	{
		if (World.Act5.ChamberOfInnocence.IsCurrentArea)
		{
			UpdateAvariusFightObjects();
			if (networkObject_0 != (NetworkObject)null)
			{
				if (!((NetworkObject)(object)npc_0 != (NetworkObject)null))
				{
					if (!((NetworkObject)(object)monster_1 != (NetworkObject)null))
					{
						if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
						{
							await Helpers.MoveToBossOrAnyMob(monster_0);
							return true;
						}
						await Helpers.MoveAndWait(networkObject_0.WalkablePosition(), "Waiting for any Avarius fight object");
						return true;
					}
					await Helpers.MoveToBossOrAnyMob(monster_1);
					return true;
				}
				return false;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
			return true;
		}
		await Travel.To(World.Act5.ChamberOfInnocence);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Lani, "Avarius Reward", Quests.DeathToPurity.Id);
	}

	private static void UpdateAvariusFightObjects()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		networkObject_0 = null;
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			string metadata = @object.Metadata;
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null) || (int)val.Rarity != 3)
			{
				Npc val2 = (Npc)(object)((@object is Npc) ? @object : null);
				if (!((NetworkObject)(object)val2 != (NetworkObject)null) || !(((NetworkObject)val2).Metadata == "Metadata/NPC/Act5/SinInnocence"))
				{
					if (metadata == "Metadata/Monsters/AvariusCasticus/ArenaMiddle")
					{
						networkObject_0 = @object;
					}
				}
				else
				{
					npc_0 = val2;
				}
			}
			else if (metadata == "Metadata/Monsters/AvariusCasticus/AvariusCasticus")
			{
				monster_0 = val;
			}
			else if (metadata == "Metadata/Monsters/AvariusCasticus/AvariusCasticusDivine")
			{
				monster_1 = val;
			}
		}
	}

	static A5_Q4_DeathToPurity()
	{
		tgtPosition_0 = new TgtPosition("Sanctum of Innocence", "transition_chamber_to_boss_v01_01.tgt");
	}
}
