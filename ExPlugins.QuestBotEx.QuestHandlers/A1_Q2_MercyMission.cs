using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q2_MercyMission
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Hailrake => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1149 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedHailrakePos
	{
		get
		{
			return CombatAreaCache.Current.Storage["HailrakePosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["HailrakePosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act1.TidalIsland.IsCurrentArea)
		{
			Monster hailrake = Hailrake;
			if ((NetworkObject)(object)hailrake != (NetworkObject)null)
			{
				CachedHailrakePos = ((NetworkObject)(object)hailrake).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillHailrake()
	{
		if (Helpers.PlayerHasQuestItem("MedicineSet"))
		{
			return false;
		}
		if (World.Act1.TidalIsland.IsCurrentArea)
		{
			WalkablePosition hailrakePos = CachedHailrakePos;
			if (!(hailrakePos != null))
			{
				tgtPosition_0.Come();
			}
			else
			{
				await Helpers.MoveAndWait(hailrakePos);
			}
			return true;
		}
		await Travel.To(World.Act1.TidalIsland);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act1.LioneyeWatch.IsCurrentArea)
		{
			if (QuestManager.GetState(Quests.MercyMission) == 0)
			{
				return false;
			}
			while (true)
			{
				if (await TownNpcs.Nessa.OpenDialogPanel())
				{
					if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Medicine Chest Reward 2")))
					{
						string reward2 = QuestBotSettings.Instance.GetRewardForQuest(Quests.MercyMission.Id + "b");
						if (await TownNpcs.Nessa.TakeReward(reward2, "Medicine Chest Reward 2"))
						{
							continue;
						}
						ErrorManager.ReportError();
						return true;
					}
					if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Medicine Chest Reward")))
					{
						string reward = QuestBotSettings.Instance.GetRewardForQuest(Quests.MercyMission.Id);
						if (!(await TownNpcs.Nessa.TakeReward(reward, "Medicine Chest Reward")))
						{
							ErrorManager.ReportError();
						}
					}
					break;
				}
				ErrorManager.ReportError();
				return true;
			}
			return true;
		}
		await Travel.To(World.Act1.LioneyeWatch);
		return true;
	}

	static A1_Q2_MercyMission()
	{
		tgtPosition_0 = new TgtPosition("Medicine Chest location", "kyrenia_boat_medicinequest_v01_01.tgt");
	}
}
