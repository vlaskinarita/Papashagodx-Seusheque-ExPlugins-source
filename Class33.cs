using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.QuestBotEx;

internal class Class33 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static Npc Kirac => ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Name == "Commander Kirac" && ((NetworkObject)n).IsTargetable);

	public string Name => "AtlasQuestsTask";

	public string Author => "Seusheque";

	public string Description => "Task for finishing Tangle quests.";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (LokiPoe.IsInGame)
		{
			if (area.IsMyHideoutArea)
			{
				if (!((NetworkObject)(object)Kirac == (NetworkObject)null))
				{
					DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("tangle");
					int? eaterQuestState = ((currentQuestStateAccurate == null) ? null : new int?(currentQuestStateAccurate.Id));
					DatQuestStateWrapper currentQuestStateAccurate2 = InGameState.GetCurrentQuestStateAccurate("cleansing_fire");
					int? exarchQuestState = ((currentQuestStateAccurate2 == null) ? null : new int?(currentQuestStateAccurate2.Id));
					DatQuestStateWrapper currentQuestStateAccurate3 = InGameState.GetCurrentQuestStateAccurate("uberelder");
					if (currentQuestStateAccurate3 != null)
					{
						new int?(currentQuestStateAccurate3.Id);
					}
					DatQuestStateWrapper currentQuestStateAccurate4 = InGameState.GetCurrentQuestStateAccurate("maven_boss");
					if (currentQuestStateAccurate4 != null)
					{
						new int?(currentQuestStateAccurate4.Id);
					}
					if (eaterQuestState != 2)
					{
						if (exarchQuestState != 2)
						{
							return false;
						}
						return await Helpers.TakeQuestReward(area, ((NetworkObject)(object)Kirac).AsTownNpc(), null, "cleansing_fire", "AtlasSkillBook", shouldLogOut: true);
					}
					return await Helpers.TakeQuestReward(area, ((NetworkObject)(object)Kirac).AsTownNpc(), null, "tangle", "AtlasSkillBook", shouldLogOut: true);
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

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}
}
