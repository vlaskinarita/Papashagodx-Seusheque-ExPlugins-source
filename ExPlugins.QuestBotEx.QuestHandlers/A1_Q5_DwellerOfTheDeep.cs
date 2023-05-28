using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q5_DwellerOfTheDeep
{
	private static bool bool_0;

	private static Monster Dweller => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)382 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3 && !((Actor)m).IsDead);

	private static WalkablePosition CachedDwellerPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["DwellerPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["DwellerPosition"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.DwellerOfTheDeep) <= 3;
		if (World.Act1.FloodedDepths.IsCurrentArea)
		{
			Monster dweller = Dweller;
			if ((NetworkObject)(object)dweller != (NetworkObject)null)
			{
				CachedDwellerPos = ((NetworkObject)(object)dweller).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillDweller()
	{
		if (!bool_0)
		{
			if (World.Act1.FloodedDepths.IsCurrentArea)
			{
				WalkablePosition dwellerPos = CachedDwellerPos;
				if (dwellerPos != null)
				{
					await Helpers.MoveAndWait(dwellerPos);
					return true;
				}
				if (!(await CombatAreaCache.Current.Explorer.Execute()))
				{
					if (QuestManager.GetState(Quests.DwellerOfTheDeep) <= 3)
					{
						return false;
					}
					GlobalLog.Error("[KillDweller] Flooded Depths area is fully explored but Deep Dweller was not killed. Now going to create a new Flooded Depths instance.");
					Travel.RequestNewInstance(World.Act1.FloodedDepths);
					if (!(await PlayerAction.TpToTown()))
					{
						ErrorManager.ReportError();
					}
				}
				return true;
			}
			await Travel.To(World.Act1.FloodedDepths);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Tarkleigh, "Dweller Reward", null, "Book-a1q7");
	}
}
