using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A10_Q6_EndToHunger
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly WalkablePosition walkablePosition_0;

	private static Npc npc_0;

	private static Monster monster_0;

	private static Monster monster_1;

	public static void Tick()
	{
	}

	public static async Task<bool> KillKitava()
	{
		if (World.Act11.KaruiShores.IsWaypointOpened)
		{
			GlobalLog.Debug("[A10_Q6_EndToHunger] Karui Shores waypoint is open.");
			return false;
		}
		if (World.Act10.FeedingTrough.IsCurrentArea)
		{
			UpdateKitavaFightObjects();
			Monster deadKitava = ObjectManager.GetObjectsByType<Monster>().FirstOrDefault((Monster m) => ((NetworkObject)m).Metadata.Contains("KitavaBoss/KitavaFinal") && ((Actor)m).IsDead);
			DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("A11q0");
			if ((currentQuestStateAccurate == null || currentQuestStateAccurate.Id != 5) && !((NetworkObject)(object)deadKitava != (NetworkObject)null))
			{
				if (walkablePosition_0.PathExists)
				{
					if (!((NetworkObject)(object)monster_1 != (NetworkObject)null) || !((NetworkObject)monster_1).IsTargetable)
					{
						if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
						{
							if (((Actor)monster_0).IsDead)
							{
								await Wait.For(() => World.Act11.Oriath.IsCurrentArea, "Waiting for Kitava fight ending", 500, 7000);
								return false;
							}
							if (monster_0.IsActive)
							{
								walkablePosition_0.Come();
							}
							else
							{
								await Helpers.MoveAndWait(walkablePosition_0, "Waiting for Kitava, the Insatiable");
							}
						}
						return true;
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
					return true;
				}
				if ((NetworkObject)(object)npc_0 != (NetworkObject)null && ((NetworkObject)npc_0).IsTargetable && ((NetworkObject)npc_0).HasNpcFloatingIcon)
				{
					WalkablePosition pos = ((NetworkObject)(object)npc_0).WalkablePosition();
					if (!pos.IsFar)
					{
						await Helpers.TalkTo((NetworkObject)(object)npc_0);
					}
					else
					{
						pos.Come();
					}
					return true;
				}
				await Helpers.MoveAndTakeLocalTransitionSpecial(tgtPosition_0);
				return true;
			}
			await Wait.SleepSafe(40000);
			GlobalLog.Debug("[A10_Q6_EndToHunger] Kitava killed. Relogging.");
			LogoutError err = EscapeState.LogoutToCharacterSelection();
			return (int)err == 0;
		}
		if (World.Act11.KaruiShores.IsCurrentArea)
		{
			return false;
		}
		await Travel.To(World.Act10.FeedingTrough);
		return true;
	}

	public static async Task<bool> SailFromOriath()
	{
		if (World.Act11.KaruiShores.IsCurrentArea)
		{
			if (World.Act11.KaruiShores.IsWaypointOpened)
			{
				return false;
			}
			if (!(await PlayerAction.OpenWaypoint()))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		if (World.Act10.OriathDocks.IsCurrentArea)
		{
			if (!TownNpcs.LillyRothA10.Position.PathExists)
			{
				GlobalLog.Debug("Waiting for Kitava fight ending");
				await Wait.StuckDetectionSleep(500);
				return true;
			}
			TownNpc lilly = TownNpcs.LillyRothA10;
			uint hash = LocalData.AreaHash;
			if (await lilly.Converse("Set Sail from Oriath"))
			{
				if (await Wait.ForAreaChange(hash))
				{
					return (int)EscapeState.LogoutToCharacterSelection() == 0;
				}
				return true;
			}
			await Wait.LatencySleep();
			return true;
		}
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act10.OriathDocks, TownNpcs.LaniA10, "Kitava Reward", null, "Book-", shouldLogOut: true);
	}

	private static void UpdateKitavaFightObjects()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		npc_0 = null;
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null) || (int)val.Rarity != 3)
			{
				Npc val2 = (Npc)(object)((@object is Npc) ? @object : null);
				if ((NetworkObject)(object)val2 != (NetworkObject)null && ((NetworkObject)val2).Metadata == "Metadata/NPC/Act10/SinTrough")
				{
					npc_0 = val2;
				}
				continue;
			}
			string name = ((NetworkObject)val).Name;
			if (name == "Kitava, the Insatiable")
			{
				monster_0 = val;
			}
			else if (name == "Kitava's Heart")
			{
				monster_1 = val;
			}
		}
	}

	static A10_Q6_EndToHunger()
	{
		tgtPosition_0 = new TgtPosition("Kitava room", "act10_kitava_arena_v01_01.tgt", closest: true);
		walkablePosition_0 = new WalkablePosition("Walkable position in front of Kitava", 1830, 3155);
	}
}
