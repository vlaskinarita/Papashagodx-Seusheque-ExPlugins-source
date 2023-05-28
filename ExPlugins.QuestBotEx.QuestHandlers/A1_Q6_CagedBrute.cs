using System.Diagnostics;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q6_CagedBrute
{
	private static bool bool_0;

	private static Stopwatch stopwatch_0;

	private static Npc Navali => ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Metadata == "Metadata/NPC/League/Prophecy/NavaliWild");

	private static TriggerableBlockage NavaliCage => ObjectManager.Objects.FirstOrDefault((TriggerableBlockage t) => ((NetworkObject)t).Metadata == "Metadata/NPC/League/Prophecy/NavaliCage");

	private static Monster Brutus => ObjectManager.Objects.FirstOrDefault((Monster m) => (int)m.Rarity == 3 && ((NetworkObject)m).Metadata.Contains("Monsters/Brute/BossBrute"));

	public static void Tick()
	{
		bool_0 = World.Act1.PrisonerGate.IsWaypointOpened;
	}

	public static async Task<bool> EnterPrison()
	{
		if (World.Act1.Climb.IsCurrentArea)
		{
			Npc navali = Navali;
			if ((NetworkObject)(object)navali != (NetworkObject)null)
			{
				if (!((NetworkObject)navali).IsTargetable)
				{
					WalkablePosition navaliPos2 = ((NetworkObject)(object)navali).WalkablePosition();
					if (navaliPos2.IsFar)
					{
						navaliPos2.Come();
						return true;
					}
					TriggerableBlockage triggerableBlockage_0 = NavaliCage;
					if (!((NetworkObject)(object)triggerableBlockage_0 == (NetworkObject)null))
					{
						if (((NetworkObject)triggerableBlockage_0).IsTargetable && !(await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0, () => !((NetworkObject)triggerableBlockage_0.Fresh<TriggerableBlockage>()).IsTargetable, "Navali Cage opening", 5000)))
						{
							ErrorManager.ReportError();
						}
						GlobalLog.Debug("Waiting for Navali");
						await Wait.StuckDetectionSleep(200);
						if (stopwatch_0 == null)
						{
							stopwatch_0 = Stopwatch.StartNew();
						}
						if (stopwatch_0.ElapsedMilliseconds >= 120000L)
						{
							stopwatch_0 = null;
							Travel.RequestNewInstance(World.Act1.Climb);
							await PlayerAction.TpToTown();
						}
						return true;
					}
					GlobalLog.Debug("We are near Navali but Cage object is null.");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				if (((NetworkObject)navali).HasNpcFloatingIcon)
				{
					WalkablePosition navaliPos = ((NetworkObject)(object)navali).WalkablePosition();
					if (!navaliPos.IsFar)
					{
						await Helpers.TalkTo((NetworkObject)(object)navali);
					}
					else
					{
						navaliPos.Come();
					}
					return true;
				}
			}
			stopwatch_0 = null;
		}
		if (World.Act1.LowerPrison.IsCurrentArea)
		{
			return false;
		}
		await Travel.To(World.Act1.LowerPrison);
		return true;
	}

	public static async Task<bool> KillBrutus()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act1.UpperPrison.IsCurrentArea)
		{
			Monster brutus = Brutus;
			if ((NetworkObject)(object)brutus != (NetworkObject)null && ((NetworkObject)(object)brutus).PathExists())
			{
				await Helpers.MoveAndWait((NetworkObject)(object)brutus);
				return true;
			}
		}
		await Travel.To(World.Act1.PrisonerGate);
		return true;
	}

	public static async Task<bool> TakeNessaReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Nessa, "Prison Reward", Quests.CagedBrute.Id + "b");
	}

	public static async Task<bool> TalktoTarkleigh()
	{
		return await Helpers.TravelAndTalkTo(World.Act1.LioneyeWatch, TownNpcs.Tarkleigh);
	}

	public static async Task<bool> TakeTarkleighReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Tarkleigh, "Brutus Reward", Quests.CagedBrute.Id);
	}
}
