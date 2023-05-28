using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q1_LostInLove
{
	private static readonly WalkablePosition walkablePosition_0;

	private static readonly TgtPosition tgtPosition_0;

	private static WalkablePosition walkablePosition_1;

	private static WalkablePosition walkablePosition_2;

	private static WalkablePosition walkablePosition_3;

	private static WalkablePosition walkablePosition_4;

	private static Npc Clarissa => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)20 }).FirstOrDefault<Npc>();

	private static Monster GuardCaptain => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)708 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3 && !((Actor)m).IsDead);

	private static Monster Piety => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)24 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Chest Tolman => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)105 }).FirstOrDefault<Chest>();

	private static Monster TolmanGuard => ObjectManager.Objects.FirstOrDefault((Monster m) => !((Actor)m).IsDead && ((NetworkObject)m).Metadata.Contains("GuardTolman"));

	private static Monster ClarissaGuard => ObjectManager.Objects.FirstOrDefault((Monster m) => !((Actor)m).IsDead && ((NetworkObject)m).Metadata.Contains("GuardClarissa"));

	private static CachedObject CachedTolman
	{
		get
		{
			return CombatAreaCache.Current.Storage["Tolman"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Tolman"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act3.CityOfSarn.IsCurrentArea)
		{
			walkablePosition_1 = ((NetworkObject)(object)GuardCaptain)?.WalkablePosition();
			walkablePosition_2 = ((NetworkObject)(object)ClarissaGuard)?.WalkablePosition();
		}
		else
		{
			if (!World.Act3.Crematorium.IsCurrentArea)
			{
				return;
			}
			walkablePosition_3 = ((NetworkObject)(object)Piety)?.WalkablePosition();
			walkablePosition_4 = ((NetworkObject)(object)TolmanGuard)?.WalkablePosition();
			if (CachedTolman == null)
			{
				Chest tolman = Tolman;
				if ((NetworkObject)(object)tolman != (NetworkObject)null)
				{
					CachedTolman = new CachedObject((NetworkObject)(object)tolman);
				}
			}
		}
	}

	public static async Task<bool> EnterSarnEncampment()
	{
		if (World.Act3.SarnEncampment.IsCurrentArea)
		{
			if (!World.Act3.SarnEncampment.IsWaypointOpened)
			{
				if (!(await PlayerAction.OpenWaypoint()))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			return false;
		}
		await Travel.To(World.Act3.SarnEncampment);
		return true;
	}

	public static async Task<bool> FreeClarissa()
	{
		if (World.Act3.CityOfSarn.IsCurrentArea)
		{
			if (!(walkablePosition_1 != null))
			{
				if (!(walkablePosition_2 != null))
				{
					Npc clarissa = Clarissa;
					if (!((NetworkObject)(object)clarissa != (NetworkObject)null))
					{
						walkablePosition_0.Come();
						return true;
					}
					WalkablePosition clarissaPos = ((NetworkObject)(object)clarissa).WalkablePosition();
					if (clarissaPos.IsFar)
					{
						clarissaPos.Come();
						return true;
					}
					if (((NetworkObject)clarissa).IsTargetable)
					{
						if (!((NetworkObject)clarissa).HasNpcFloatingIcon)
						{
							if (QuestManager.GetState(Quests.LostInLove) >= 10)
							{
								return true;
							}
							return false;
						}
						await Helpers.TalkTo((NetworkObject)(object)clarissa);
						return true;
					}
					GlobalLog.Debug("Waiting for Clarissa");
					await Wait.StuckDetectionSleep(200);
					return true;
				}
				walkablePosition_2.Come();
				return true;
			}
			walkablePosition_1.Come();
			return true;
		}
		await Travel.To(World.Act3.CityOfSarn);
		return true;
	}

	public static async Task<bool> GrabBracelet()
	{
		if (!Helpers.PlayerHasQuestItem("TolmanBracelet"))
		{
			if (!Helpers.PlayerHasQuestItem("SewerKeys"))
			{
				if (World.Act3.Crematorium.IsCurrentArea)
				{
					if (!(walkablePosition_3 != null))
					{
						if (walkablePosition_4 != null)
						{
							walkablePosition_4.Come();
							return true;
						}
						if (await Helpers.OpenQuestChest(CachedTolman))
						{
							return true;
						}
						tgtPosition_0.Come();
						return true;
					}
					await Helpers.MoveAndWait(walkablePosition_3);
					return true;
				}
				await Travel.To(World.Act3.Crematorium);
				return true;
			}
			return false;
		}
		return false;
	}

	public static async Task<bool> TakeClarissaReward()
	{
		return await Helpers.TakeQuestReward(World.Act3.SarnEncampment, TownNpcs.Clarissa, "Take Sewer Keys", Quests.LostInLove.Id, null, shouldLogOut: true);
	}

	public static async Task<bool> TakeMaramoaReward()
	{
		return await Helpers.TakeQuestReward(World.Act3.SarnEncampment, TownNpcs.Maramoa, "Clarissa Reward", Quests.LostInLove.Id, null, shouldLogOut: true);
	}

	static A3_Q1_LostInLove()
	{
		walkablePosition_0 = new WalkablePosition("Clarissa position", 560, 1278);
		tgtPosition_0 = new TgtPosition("Tolman location", "quest_marker.tgt");
	}
}
