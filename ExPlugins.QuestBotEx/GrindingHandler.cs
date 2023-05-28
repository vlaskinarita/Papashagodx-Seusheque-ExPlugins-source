using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public static class GrindingHandler
{
	public const string Name = "Grinding";

	public const int MaxPulses = 3;

	private static QuestBotSettings.GrindingRule grindingRule_0;

	private static AreaInfo areaInfo_0;

	private static readonly Vector2i vector2i_0;

	private static int GrindingPulses
	{
		get
		{
			return ((int?)CombatAreaCache.Current.Storage["GrindingPulses"]).GetValueOrDefault();
		}
		set
		{
			CombatAreaCache.Current.Storage["GrindingPulses"] = value;
		}
	}

	public static async Task<bool> Execute()
	{
		if ((World.CurrentArea.IsTown || World.CurrentArea.IsHideoutArea) && ((Player)LokiPoe.Me).Level >= grindingRule_0.LevelCap)
		{
			GlobalLog.Warn(string.Format("[{0}] Level cap has been reached for \"{1}\" ({2})", "Grinding", grindingRule_0.Quest.Name, grindingRule_0.LevelCap));
			grindingRule_0 = null;
			return false;
		}
		if (areaInfo_0.IsCurrentArea)
		{
			TravelToHideoutTask.ShouldSkip = false;
			QuestBotSettings settings = QuestBotSettings.Instance;
			bool trackMob = settings.TrackMob;
			bool flag = trackMob;
			if (flag)
			{
				flag = await TrackMobLogic.Execute();
			}
			if (flag)
			{
				return true;
			}
			ComplexExplorer explorer = CombatAreaCache.Current.Explorer;
			if (World.Act9.BloodAqueduct.IsCurrentArea)
			{
				WalkablePosition pos = new WalkablePosition("Highgate", vector2i_0);
				if (pos.IsFar)
				{
					pos.TryCome();
					return true;
				}
				GlobalLog.Info("Destination reached!");
				await PlayerAction.TakeTransitionByName("Highgate");
				return true;
			}
			bool flag2 = explorer.BasicExplorer.PercentComplete >= (float)settings.ExplorationPercent;
			bool flag3 = flag2;
			if (!flag3)
			{
				flag3 = !(await explorer.Execute());
			}
			if (flag3)
			{
				if (await TrackMobLogic.Execute(80))
				{
					return true;
				}
				if (GrindingPulses < 3)
				{
					await Coroutines.FinishCurrentAction(true);
					GrindingPulses++;
					GlobalLog.Info($"[Grinding] Final pulse {GrindingPulses}/{3}");
					await Wait.SleepSafe(500);
					return true;
				}
				if (!(await PlayerAction.TpToTown()))
				{
					ErrorManager.ReportError();
					return true;
				}
			}
			return true;
		}
		TravelToHideoutTask.ShouldSkip = true;
		Travel.RequestNewInstance(areaInfo_0);
		await Travel.To(areaInfo_0);
		return true;
	}

	internal static void OnPlayerDied(int deathCount)
	{
		QuestBotSettings instance = QuestBotSettings.Instance;
		if (instance.MaxDeaths > 0 && !LeaveAreaTask.IsActive && !(instance.CurrentQuestName != "Grinding") && deathCount >= instance.MaxDeaths)
		{
			GlobalLog.Error("[Grinding] Too many deaths in current area (" + World.CurrentArea.Name + "). Now leaving it.");
			LeaveAreaTask.IsActive = true;
		}
	}

	internal static void SetGrindingRule(QuestBotSettings.GrindingRule rule)
	{
		if (grindingRule_0 == rule)
		{
			areaInfo_0 = new AreaInfo(grindingRule_0.GrindArea.Id, grindingRule_0.GrindArea.Name);
		}
		else
		{
			grindingRule_0 = rule;
			areaInfo_0 = new AreaInfo(grindingRule_0.GrindArea.Id, grindingRule_0.GrindArea.Name);
		}
		QuestManager.UpdateGuiAndLog("Grinding", areaInfo_0.Name);
	}

	static GrindingHandler()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		vector2i_0 = new Vector2i(366, 3694);
	}
}
