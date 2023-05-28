using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using ExPlugins.EXtensions;

internal class Class1 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private bool bool_0 = true;

	private Interval interval_0 = new Interval(300000);

	public string Name => "ChangeLocationTask";

	public string Description => "Task for changing locations on nullbot.";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public void Tick()
	{
	}

	public async Task<bool> Run()
	{
		await Wait.SleepSafe(1);
		return false;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
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
}
