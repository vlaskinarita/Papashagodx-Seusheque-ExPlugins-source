using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A4_Q4_KingOfDesire
{
	private static Monster Daresso => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1647 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster AnyUniqueMob => ((IEnumerable)ObjectManager.Objects).Closest<Monster>((Func<Monster, bool>)((Monster m) => (int)m.Rarity == 3 && m.IsActive));

	public static void Tick()
	{
	}

	public static async Task<bool> KillDaresso()
	{
		if (!Helpers.PlayerHasQuestItem("DaressoGem"))
		{
			if (!World.Act4.GrandArena.IsCurrentArea)
			{
				if (World.Act4.DaressoDream.IsCurrentArea)
				{
					if (await TrackMobLogic.Execute(100))
					{
						return true;
					}
					await Travel.To(World.Act4.GrandArena);
					return true;
				}
				await Travel.To(World.Act4.GrandArena);
				return true;
			}
			Monster daresso = Daresso;
			if (!((NetworkObject)(object)daresso != (NetworkObject)null) || !((NetworkObject)(object)daresso).PathExists())
			{
				Monster mob = AnyUniqueMob;
				if ((NetworkObject)(object)mob != (NetworkObject)null && ((NetworkObject)(object)mob).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)mob);
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Helpers.MoveAndWait((NetworkObject)(object)daresso);
			return true;
		}
		return false;
	}

	public static async Task<bool> TurnInQuest()
	{
		if (Helpers.PlayerHasQuestItem("DaressoGem"))
		{
			if (World.Act4.CrystalVeins.IsCurrentArea)
			{
				Npc dialla = Helpers.LadyDialla;
				if (!((NetworkObject)(object)dialla == (NetworkObject)null))
				{
					await Helpers.TalkTo((NetworkObject)(object)dialla);
					return true;
				}
				GlobalLog.Error("[KingOfDesire] Fail to detect Lady Dialla in Crystal Veins.");
				ErrorManager.ReportCriticalError();
				return true;
			}
			await Travel.To(World.Act4.CrystalVeins);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act4.Highgate, TownNpcs.Dialla, "Rapture Reward", Quests.EternalNightmare.Id);
	}
}
