using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_HelenaHideout
{
	private static bool bool_0;

	public static bool Interacted;

	public static bool Cleared;

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.HelenaHideout) <= 4;
	}

	public static async Task<bool> ClearDreadThicket()
	{
		if (!bool_0)
		{
			if (World.Act2.DreadThicket.IsCurrentArea)
			{
				AreaTransition transition = ObjectManager.Objects.FirstOrDefault((AreaTransition a) => ((NetworkObject)a).Name == "Lush Hideout");
				int state = QuestManager.GetState(Quests.HelenaHideout);
				if (state == 5)
				{
					GlobalLog.Warn("[HelenaQuest] State == 5. Killing poe.");
					Process.GetProcessesByName("PathOfExile").FirstOrDefault()?.Kill();
				}
				if ((NetworkObject)(object)transition != (NetworkObject)null)
				{
					GlobalLog.Debug("[ClearDreadThicket] Trasition detected, taking it");
					if (!(await PlayerAction.TakeTransition(transition)))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				if (await TrackMobLogic.Execute())
				{
					return true;
				}
				if (!(await CombatAreaCache.Current.Explorer.Execute()))
				{
					if (state <= 4)
					{
						Interacted = false;
						return false;
					}
					GlobalLog.Error("[ClearDreadThicket] Dread Thicket is fully explored but not all monsters were killed. Now going to create a new Dread Thicket instance.");
					Travel.RequestNewInstance(World.Act2.DreadThicket);
					if (!(await PlayerAction.TpToTown()))
					{
						ErrorManager.ReportError();
					}
				}
				return true;
			}
			await Travel.To(World.Act2.DreadThicket);
			return true;
		}
		Interacted = false;
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await CreateHideout(World.Act2.ForestEncampment, TownNpcs.Helena, "Create Hideout");
	}

	public static async Task<bool> CreateHideout(AreaInfo area, TownNpc npc, string dialog)
	{
		if (area.IsCurrentArea)
		{
			if (await npc.Converse(dialog))
			{
				await Wait.For(() => LokiPoe.Me.IsInHideout, "Hideout transition", 5000);
			}
			Interacted = true;
			return false;
		}
		await Travel.To(area);
		return true;
	}
}
