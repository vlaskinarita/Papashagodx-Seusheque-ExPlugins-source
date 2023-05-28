using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;

namespace ExPlugins.EXtensions.CommonTasks;

public class PostCombatHookTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public const string MessageId = "hook_post_combat";

	public string Name => "PostCombatHookTask";

	public string Description => "This task provides a coroutine hook for executing user logic after combat has completed.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		foreach (IPlugin plugin in PluginManager.EnabledPlugins)
		{
			if ((int)(await ((ILogicProvider)plugin).Logic(new Logic("hook_post_combat", (object)this))) == 0)
			{
				GlobalLog.Info("[PostCombatHookTask] \"" + ((IAuthored)plugin).Name + "\" returned true.");
				return true;
			}
		}
		foreach (IContent content in ContentManager.Contents)
		{
			if ((int)(await ((ILogicProvider)content).Logic(new Logic("hook_post_combat", (object)this))) == 0)
			{
				GlobalLog.Info("[PostCombatHookTask] \"" + ((IAuthored)content).Name + "\" returned true.");
				return true;
			}
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
