using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.QuestBotEx;

namespace ExPlugins.MapBotEx.Tasks;

public class StartMapInEpilogueTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "StartMapInEpilogueTask";

	public string Description => "Task for traveling to The Eternal Laboratory.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("a11q1");
		int? mapDeviceQuestState = ((currentQuestStateAccurate == null) ? null : new int?(currentQuestStateAccurate.Id));
		int? num = mapDeviceQuestState;
		if (!((num.GetValueOrDefault() == 0) & num.HasValue))
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsMap)
			{
				if (!(area.Name == "Karui Shores"))
				{
					if (area.IsTown || area.IsHideoutArea)
					{
						if (!World.Act11.KaruiShores.IsWaypointOpened)
						{
							GlobalLog.Debug("[StartMapInEpilogueTask] We need to open map in Karui Shores");
							await Travel.To(World.Act11.KaruiShores);
							return true;
						}
						if (!(await PlayerAction.TakeWaypoint(World.Act11.KaruiShores)))
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
				if (!TownNpcs.KiracA11.NpcObject.HasNpcFloatingIcon && mapDeviceQuestState != 5)
				{
					return false;
				}
				await global::ExPlugins.QuestBotEx.Helpers.TakeQuestReward(World.Act11.KaruiShores, TownNpcs.KiracA11, "Take Map", "a11q1");
				return true;
			}
			return false;
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
