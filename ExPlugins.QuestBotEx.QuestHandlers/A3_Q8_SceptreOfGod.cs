using System;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q8_SceptreOfGod
{
	private static NetworkObject networkObject_0;

	private static Monster monster_0;

	private static Monster monster_1;

	private static Monster monster_2;

	private static bool bool_0;

	public static void Tick()
	{
		bool_0 = World.Act4.Aqueduct.IsWaypointOpened;
	}

	public static async Task<bool> KillDominus()
	{
		if (!bool_0)
		{
			if (World.Act3.UpperSceptreOfGod.IsCurrentArea)
			{
				UpdateDominusFightObjects();
				Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
				if (((Vector2i)(ref position)).Distance(new Vector2i(3403, 286)) < 20)
				{
					new WalkablePosition("Dominus Stair unstucker", new Vector2i(3533, 284), 10, 10).TryCome();
					return true;
				}
				if ((NetworkObject)(object)monster_1 != (NetworkObject)null)
				{
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
					return true;
				}
				if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
				{
					if (((NetworkObject)monster_0).IsTargetable || !((NetworkObject)(object)monster_2 != (NetworkObject)null))
					{
						if (((Actor)monster_0).Health == 1)
						{
							int id = ((NetworkObject)monster_0).Id;
							if (!Blacklist.Contains(id))
							{
								Blacklist.Add(id, TimeSpan.FromDays(1.0), "Dominus first form waiting for death");
							}
						}
						await Helpers.MoveAndWait(((NetworkObject)(object)monster_0).WalkablePosition());
						return true;
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)monster_2).WalkablePosition());
					return true;
				}
				if (networkObject_0 != (NetworkObject)null)
				{
					await Helpers.MoveAndWait(networkObject_0.WalkablePosition(), "Waiting for any Dominus fight object");
					return true;
				}
			}
			await Travel.To(World.Act4.Aqueduct);
			return true;
		}
		return false;
	}

	public static async Task<bool> EnterHighgate()
	{
		if (World.Act4.Highgate.IsCurrentArea)
		{
			if (World.Act4.Highgate.IsWaypointOpened)
			{
				return false;
			}
			if (!(await PlayerAction.OpenWaypoint()))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		await Travel.To(World.Act4.Highgate);
		return true;
	}

	private static void UpdateDominusFightObjects()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		networkObject_0 = null;
		monster_0 = null;
		monster_1 = null;
		monster_2 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null) || (int)val.Rarity != 3)
			{
				if (@object.Metadata == "Metadata/Monsters/Demonmodular/TowerSpawners/TowerSpawnerBoss2")
				{
					networkObject_0 = @object;
				}
			}
			else if (!(((NetworkObject)val).Metadata == "Metadata/Monsters/Pope/Pope"))
			{
				if (!(((NetworkObject)val).Metadata == "Metadata/Monsters/Dominusdemon/Dominusdemon"))
				{
					if (!((Actor)val).IsDead && ((NetworkObject)val).IsTargetable && !((NetworkObject)val).IsFriendly)
					{
						monster_2 = val;
					}
				}
				else
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
