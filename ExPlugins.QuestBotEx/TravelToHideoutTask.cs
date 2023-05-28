using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.QuestBotEx;

public class TravelToHideoutTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	internal static bool bool_0;

	public static bool ShouldSkip;

	public string Name => "TravelToHideoutTask";

	public string Description => "Task for traveling to player's hideout.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (bool_0)
		{
			return false;
		}
		if (!ShouldSkip)
		{
			QuestBotSettings settings = QuestBotSettings.Instance;
			if (settings.UseHideout && !(settings.CurrentQuestName != "Grinding"))
			{
				if (((Player)LokiPoe.Me).Hideout != null)
				{
					if (World.CurrentArea.IsTown)
					{
						if (!(await PlayerAction.GoToHideout()))
						{
							ErrorManager.ReportError();
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (QuestBotSettings.Instance.UseHideout && message.Id == "area_changed_event")
		{
			DatWorldAreaWrapper input = message.GetInput<DatWorldAreaWrapper>(2);
			if (input != null && !input.IsHideoutArea)
			{
				bool_0 = false;
			}
			return (MessageResult)0;
		}
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
