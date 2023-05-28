using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions.CommonTasks;

public class HandleBlockingChestsTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly HashSet<int> hashSet_0;

	public static bool Enabled;

	public string Name => "HandleBlockingChestsTask";

	public string Description => "This task will handle breaking any blocking chests that interfere with movement.";

	public string Author => "Alcor75";

	public string Version => "0.0.1.1";

	public async Task<bool> Run()
	{
		if (!Enabled || !World.CurrentArea.IsCombatArea)
		{
			return false;
		}
		List<Chest> chests = (from c in ObjectManager.Objects
			where ((NetworkObject)c).Distance <= 10f && !c.IsOpened && c.IsStompable && !hashSet_0.Contains(((NetworkObject)c).Id)
			orderby ((NetworkObject)c).Distance
			select c).ToList();
		if (chests.Count != 0)
		{
			ProcessHookManager.Reset();
			await Coroutines.CloseBlockingWindows();
			List<Vector2i> positions1 = new List<Vector2i> { LokiPoe.MyPosition };
			List<Vector2> positions2 = new List<Vector2> { LokiPoe.MyWorldPosition };
			foreach (Chest chest in chests)
			{
				hashSet_0.Add(((NetworkObject)chest).Id);
				positions1.Add(((NetworkObject)chest).Position);
				positions2.Add(((NetworkObject)chest).WorldPosition);
			}
			foreach (Vector2i position2 in positions1)
			{
				MouseManager.SetMousePosition(position2, true);
				await Click();
			}
			foreach (Vector2 position in positions2)
			{
				MouseManager.SetMousePosition(position, true);
				await Click();
			}
			return true;
		}
		Enabled = false;
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "area_changed_event"))
		{
			return (MessageResult)1;
		}
		hashSet_0.Clear();
		return (MessageResult)0;
	}

	private static async Task Click()
	{
		StuckDetection.Reset();
		await Wait.LatencySleep();
		NetworkObject target = InGameState.CurrentTarget;
		if (target != (NetworkObject)null)
		{
			GlobalLog.Info($"[HandleBlockingChestsTask] \"{target.Name}\" ({target.Id}) is under the cursor. Now clicking on it.");
			MouseManager.ClickLMB(0, 0);
			Thread.Sleep(1);
			await Coroutines.FinishCurrentAction(false);
		}
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

	static HandleBlockingChestsTask()
	{
		hashSet_0 = new HashSet<int>();
	}
}
