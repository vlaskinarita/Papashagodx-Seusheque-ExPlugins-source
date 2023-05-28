using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public class QuestTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly Interval interval_0 = new Interval(200);

	private QuestHandler questHandler_0;

	public string Name => "QuestTask";

	public string Author => "Alcor75";

	public string Description => "Task that executes quest logic.";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		string Areaname = World.CurrentArea.Name;
		if (!(Areaname == "The Sewers"))
		{
			if (Areaname == "The Chamber of Sins Level 1")
			{
				Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
				if (((Vector2i)(ref position)).Distance(new Vector2i(737, 216)) < 20)
				{
					await new WalkablePosition("The Chamber of Sins Level 2 Transition UnStuck", new Vector2i(702, 230), 10, 10).TryComeAtOnce();
					return true;
				}
				position = ((NetworkObject)LokiPoe.Me).Position;
				if (((Vector2i)(ref position)).Distance(new Vector2i(1113, 735)) < 20)
				{
					await new WalkablePosition("The Chamber of Sins Level 2 Transition UnStuck", new Vector2i(1043, 698), 10, 10).TryComeAtOnce();
					return true;
				}
			}
		}
		else
		{
			Vector2i mypos = ((NetworkObject)LokiPoe.Me).Position;
			if (((Vector2i)(ref mypos)).Distance(new Vector2i(792, 817)) < 20)
			{
				new WalkablePosition("The Seewer Corner UnStuck", new Vector2i(799, 854), 10, 10).TryCome();
				return true;
			}
			if (((Vector2i)(ref mypos)).Distance(new Vector2i(1064, 1572)) < 20)
			{
				new WalkablePosition("The Seewer Exit UnStuck", new Vector2i(1110, 1595), 10, 10).TryCome();
				return true;
			}
			mypos = default(Vector2i);
		}
		if (questHandler_0 == null)
		{
			questHandler_0 = QuestManager.GetQuestHandler();
			if (questHandler_0 == null)
			{
				GlobalLog.Error("[QuestTask] QuestManager returned null. Lets wait and see maybe something will change in game memory.");
				ErrorManager.ReportError();
				await Wait.SleepSafe(500);
				return true;
			}
			if (questHandler_0 == QuestHandler.QuestAddedToCache)
			{
				GlobalLog.Debug("[QuestTask] Quest was added to Completed quests cache. Now requesting quest handler again.");
				questHandler_0 = null;
				return true;
			}
			if (questHandler_0 == QuestHandler.AllQuestsDone)
			{
				GlobalLog.Warn("[QuestTask] It seems like all quests are completed.");
				await PlayerAction.TpToTown();
				await PlayerAction.GoToHideout();
				questHandler_0 = null;
				BotManager.Stop(new StopReasonData("quests_done", "It seems like all quests are completed.", (object)null), false);
				return true;
			}
			questHandler_0.Tick?.Invoke();
		}
		if (QuestBotSettings.Instance.TalkToQuestgivers && World.CurrentArea.IsTown)
		{
			bool shouldExecute = TownQuestgiversLogic.ShouldExecute;
			bool flag = shouldExecute;
			if (flag)
			{
				flag = await TownQuestgiversLogic.Execute();
			}
			if (flag)
			{
				return true;
			}
		}
		if (!(await questHandler_0.Execute()))
		{
			questHandler_0 = null;
		}
		return true;
	}

	public void Tick()
	{
		if (interval_0.Elapsed && LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			if (QuestBotSettings.Instance.TalkToQuestgivers && World.CurrentArea.IsTown)
			{
				TownQuestgiversLogic.Tick();
			}
			questHandler_0?.Tick?.Invoke();
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "area_changed_event"))
		{
			if (!(message.Id == "QB_null_handler"))
			{
				return (MessageResult)1;
			}
			questHandler_0 = null;
			return (MessageResult)0;
		}
		TownQuestgiversLogic.Reset();
		return (MessageResult)0;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}
}
