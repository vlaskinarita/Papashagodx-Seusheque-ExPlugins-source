using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;

namespace ExPlugins.EXtensions.CommonTasks;

public class FallbackTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "FallbackTask";

	public string Description => "This task is the last task executed. It should not execute.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		GlobalLog.Debug("[FallbackTask] The Fallback task is executing. The bot does not know what to do.");
		await Wait.Sleep(1000);
		return true;
	}

	public MessageResult Message(Message message)
	{
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
