using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.MapBotEx.Tasks;

public class TravelToHideoutTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static bool AnyWpNearby => ObjectManager.Objects.Any((Waypoint w) => ((NetworkObject)w).Distance <= 70f && ((NetworkObject)(object)w).PathDistance() <= 73f);

	public string Name => "TravelToHideoutTask";

	public string Description => "Task for traveling to player's hideout.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!SkillsUi.IsOpened && !AtlasSkillsUi.IsOpened)
		{
			if (area.IsMyHideoutArea || area.IsMap || area.IsMapTrialArea)
			{
				return false;
			}
			DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("a11q1");
			int? mapDeviceQuestState = ((currentQuestStateAccurate == null) ? null : new int?(currentQuestStateAccurate.Id));
			if (!(area.Name == "Karui Shores") || mapDeviceQuestState != 1)
			{
				if (!area.Id.Contains("Daily3_1") && !(area.Name == "Syndicate Hideout"))
				{
					int? num = mapDeviceQuestState;
					if (!((num.GetValueOrDefault() == 0) & num.HasValue))
					{
						return false;
					}
					GlobalLog.Debug("[TravelToHideoutTask] Now traveling to player's hideout.");
					if (area.IsTown || AnyWpNearby)
					{
						if (!(await PlayerAction.GoToHideout()))
						{
							ErrorManager.ReportError();
						}
					}
					else if (!(await PlayerAction.TpToTown()))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				return false;
			}
			await TownNpcs.KiracA11.Converse("Invite to Hideout");
			await Wait.SleepSafe(1500);
			DatQuestStateWrapper currentQuestStateAccurate2 = InGameState.GetCurrentQuestStateAccurate("a11q1");
			if ((currentQuestStateAccurate2 == null) ? (!mapDeviceQuestState.HasValue) : (currentQuestStateAccurate2.Id == mapDeviceQuestState))
			{
				await PlayerAction.Logout();
			}
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}
}
