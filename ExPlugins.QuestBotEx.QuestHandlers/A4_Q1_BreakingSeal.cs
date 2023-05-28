using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A4_Q1_BreakingSeal
{
	private static readonly WalkablePosition walkablePosition_0;

	private static Monster Voll => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1541 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static NetworkObject DeshretSeal => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act4/MineEntranceSeal");

	private static WalkablePosition CachedVollPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["VollPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["VollPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act4.DriedLake.IsCurrentArea)
		{
			Monster voll = Voll;
			if ((NetworkObject)(object)voll != (NetworkObject)null)
			{
				CachedVollPos = ((NetworkObject)(object)voll).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillVoll()
	{
		if (!Helpers.PlayerHasQuestItem("RedBanner"))
		{
			if (World.Act4.DriedLake.IsCurrentArea)
			{
				WalkablePosition vollPos = CachedVollPos;
				if (vollPos != null)
				{
					await Helpers.MoveAndWait(vollPos);
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act4.DriedLake);
			return true;
		}
		return false;
	}

	public static async Task<bool> BreakSeal()
	{
		if (!Helpers.PlayerHasQuestItem("RedBanner"))
		{
			return false;
		}
		if (World.Act4.Highgate.IsCurrentArea)
		{
			await walkablePosition_0.ComeAtOnce();
			NetworkObject networkObject_0 = DeshretSeal;
			if (!(networkObject_0 == (NetworkObject)null))
			{
				if (!networkObject_0.IsTargetable)
				{
					GlobalLog.Debug("[BreakingSeal] Deshret Seal is not targetable.");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Deshret Seal interaction")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			GlobalLog.Debug("[BreakingSeal] We are near Deshret Seal position but Seal object is null.");
			await Wait.StuckDetectionSleep(500);
			return true;
		}
		await Travel.To(World.Act4.Highgate);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act4.Highgate, TownNpcs.Oyun, "Red Banner Reward", Quests.BreakingSeal.Id);
	}

	static A4_Q1_BreakingSeal()
	{
		walkablePosition_0 = new WalkablePosition("Deshret Seal location", 330, 620);
	}
}
