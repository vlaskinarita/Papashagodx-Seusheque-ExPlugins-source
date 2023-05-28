using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.MapBotEx;

namespace ExPlugins.EXtensions.CommonTasks;

public class AfkTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly ExtensionsSettings Config;

	private readonly Stopwatch stopwatch_0 = new Stopwatch();

	private readonly Stopwatch stopwatch_1 = Stopwatch.StartNew();

	private readonly Random random_0 = new Random();

	private int int_0;

	private bool bool_0;

	private int int_1;

	public string Author => "Lajt";

	public string Description => "AfkTask";

	public string Name => "AfkTask";

	public string Version => "1.0";

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<bool> Run()
	{
		if (GeneralSettings.Instance.IsOnRun)
		{
			return false;
		}
		if (!Config.BreaksEnabled)
		{
			return false;
		}
		if (LokiPoe.Me.IsInTown || LokiPoe.Me.IsInHideout)
		{
			if (stopwatch_0.IsRunning)
			{
				if (stopwatch_0.Elapsed.TotalMinutes < (double)int_0)
				{
					GlobalLog.Debug($"[AfkTask] Still on {int_0} minutes break: {stopwatch_0.Elapsed:hh\\:mm\\:ss}");
					if (LokiPoe.Me.IsInTown && ((Player)LokiPoe.Me).Hideout != null && !(await PlayerAction.GoToHideout()))
					{
						ErrorManager.ReportError();
					}
					await Wait.Sleep(random_0.Next(10000, 25000));
					return true;
				}
				GlobalLog.Warn("[AfkTask] Break is over");
				stopwatch_0.Reset();
				stopwatch_1.Restart();
				int_1 = random_0.Next(Config.BreakEveryMinutes - 5, Config.BreakEveryMinutes + 25);
				GlobalLog.Warn($"[AfkTask] Next break in {int_1} minutes");
			}
			if (stopwatch_1.Elapsed.TotalMinutes >= (double)int_1)
			{
				int_0 = random_0.Next(Config.MinBreak, Config.MaxBreak);
				GlobalLog.Warn($"[AfkTask] Afk time! For: {int_0} minutes");
				stopwatch_0.Restart();
				return true;
			}
			return false;
		}
		return false;
	}

	public void Start()
	{
		if (!bool_0 && Config.BreaksEnabled)
		{
			int_1 = random_0.Next(Config.BreakEveryMinutes - 25, Config.BreakEveryMinutes + 25);
			bool_0 = true;
			GlobalLog.Warn($"[AfkTask] Next break in {int_1} minutes");
		}
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static AfkTask()
	{
		Config = ExtensionsSettings.Instance;
	}
}
