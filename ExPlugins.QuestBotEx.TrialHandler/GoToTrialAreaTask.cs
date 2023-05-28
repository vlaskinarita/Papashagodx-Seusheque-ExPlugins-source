using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.TrialHandler;

public class GoToTrialAreaTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly Dictionary<string, Tuple<AreaInfo, string>> dictionary_0 = new Dictionary<string, Tuple<AreaInfo, string>>
	{
		{
			"1_1_7_1",
			new Tuple<AreaInfo, string>(World.Act1.LowerPrison, "1_1_7_1")
		},
		{
			"1_2_5_1",
			new Tuple<AreaInfo, string>(World.Act2.Crypt1, "1_2_3")
		},
		{
			"1_2_6_2",
			new Tuple<AreaInfo, string>(World.Act2.ChamberOfSins2, "1_2_6_1")
		},
		{
			"1_3_3_1",
			new Tuple<AreaInfo, string>(World.Act3.Crematorium, "1_3_3_1")
		},
		{
			"1_3_6",
			new Tuple<AreaInfo, string>(World.Act3.Catacombs, "1_3_5")
		},
		{
			"1_3_15",
			new Tuple<AreaInfo, string>(World.Act3.ImperialGardens, "1_3_15")
		},
		{
			"2_6_7_1",
			new Tuple<AreaInfo, string>(World.Act6.LowerPrison, "2_6_7_1")
		},
		{
			"2_7_4",
			new Tuple<AreaInfo, string>(World.Act7.Crypt, "2_7_4")
		},
		{
			"2_7_5_2",
			new Tuple<AreaInfo, string>(World.Act7.ChamberOfSins2, "2_7_5_1")
		},
		{
			"2_8_5",
			new Tuple<AreaInfo, string>(World.Act8.BathHouse, "2_8_5")
		},
		{
			"2_9_7",
			new Tuple<AreaInfo, string>(World.Act9.Tunnel, "2_9_7")
		},
		{
			"2_10_9",
			new Tuple<AreaInfo, string>(World.Act10.Ossuary, "2_10_2")
		}
	};

	private AreaInfo areaInfo_0;

	public string Name => "GoToTrialAreaTask";

	public string Author => "";

	public string Description => "";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame || ((Actor)LokiPoe.Me).IsDead)
		{
			return false;
		}
		if (!PluginManager.EnabledPlugins.Any((IPlugin p) => ((IAuthored)p).Name == "DeadlyTrials"))
		{
			if (areaInfo_0 != null)
			{
				if (LokiPoe.CurrentWorldArea.Id != areaInfo_0.Id)
				{
					await Travel.To(areaInfo_0);
					return true;
				}
				areaInfo_0 = null;
				return true;
			}
			if (LokiPoe.CurrentWorldArea.IsTown || LokiPoe.CurrentWorldArea.IsHideoutArea)
			{
				foreach (string id2 in TrialSolverTask.TrialAreaIDs.Where((string id) => !((Player)LokiPoe.Me).IsAscendencyTrialCompleted(id)))
				{
					var (area, areaId) = dictionary_0[id2];
					if (World.IsWaypointOpened(areaId))
					{
						int num;
						if (area == World.Act7.ChamberOfSins2)
						{
							DatQuestStateWrapper currentQuestState = InGameState.GetCurrentQuestState("a7q3");
							num = ((currentQuestState == null || currentQuestState.Id != 0) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						if (num == 0)
						{
							areaInfo_0 = area;
							return true;
						}
						return false;
					}
				}
				return false;
			}
			return false;
		}
		return false;
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

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}
}
