using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions.CommonTasks;

public class AssignPantheonTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static bool bool_0;

	public string Name => "AssignPantheonTask";

	public string Description => "This task assign pantheon as per configuration.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!bool_0)
		{
			if (!World.CurrentArea.IsTown && !World.CurrentArea.IsHideoutArea)
			{
				return false;
			}
			if (((Player)LokiPoe.Me).PantheonMajor != ExtensionsSettings.Instance.PantheonMajorGod)
			{
				await AssignPantheon(ExtensionsSettings.Instance.PantheonMajorGod);
				bool_0 = true;
			}
			if (((Player)LokiPoe.Me).PantheonMinor != ExtensionsSettings.Instance.PantheonMinorGod)
			{
				await AssignPantheon(ExtensionsSettings.Instance.PantheonMinorGod);
				bool_0 = true;
			}
			return false;
		}
		return false;
	}

	private async Task AssignPantheon(PantheonGod god)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!LokiPoe.Me.League.Contains("Ruthless") && !LokiPoe.Me.League.Contains("R ") && IsGodUnlocked(god))
		{
			bool flag = !PantheonUI.IsOpened;
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = !(await OpenPantheonUi());
			}
			if (!flag2)
			{
				PantheonUI.AssignPantheon(god);
			}
		}
	}

	private bool IsGodUnlocked(PantheonGod god)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected I4, but got Unknown
		return (int)god switch
		{
			0 => false, 
			1 => QuestIsCompleted("a6q1"), 
			2 => QuestIsCompleted("a7q4"), 
			3 => QuestIsCompleted("a8q3"), 
			4 => QuestIsCompleted("a8q2"), 
			5 => false, 
			6 => false, 
			7 => QuestIsCompleted("a6q7"), 
			8 => false, 
			9 => QuestIsCompleted("a7q9"), 
			10 => QuestIsCompleted("a8q4"), 
			11 => QuestIsCompleted("a9q5"), 
			12 => QuestIsCompleted("a6q3"), 
			13 => false, 
			14 => QuestIsCompleted("a7q1"), 
			15 => QuestIsCompleted("a9q2"), 
			16 => QuestIsCompleted("a6q6"), 
			_ => false, 
		};
	}

	private static bool QuestIsCompleted(string questId, int completedState = 0)
	{
		if (InGameState.GetCurrentQuestStates().TryGetValue(questId, out var value))
		{
			DatQuestStateWrapper value2 = value.Value;
			if (value2 == null)
			{
				return false;
			}
			if (value2.Id <= completedState)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static async Task<bool> OpenPantheonUi()
	{
		if (PantheonUI.IsOpened)
		{
			return true;
		}
		await Coroutines.CloseBlockingWindows();
		Input.SimulateKeyEvent(Binding.open_pantheon_panel, true, false, false, Keys.None);
		if (!(await Wait.For(() => PantheonUI.IsOpened, "pantheon panel opening")))
		{
			return false;
		}
		await Wait.Sleep(20);
		return true;
	}

	public MessageResult Message(Message message)
	{
		if (message.Id == "area_changed_event")
		{
			bool_0 = false;
		}
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Tick()
	{
	}

	public void Stop()
	{
	}
}
