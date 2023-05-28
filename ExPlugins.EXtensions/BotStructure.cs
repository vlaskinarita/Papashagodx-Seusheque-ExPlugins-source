using System.Linq;
using DreamPoeBot.Loki.Bot;

namespace ExPlugins.EXtensions;

public static class BotStructure
{
	public const string GetTaskManagerMessage = "GetTaskManager";

	public static TaskManager TaskManager
	{
		get
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			IBot current = BotManager.Current;
			if (!(current is GInterface0 gInterface))
			{
				GlobalLog.Debug("[BotStructure] \"" + ((IAuthored)current).Name + "\" does not implement ITaskManagerHolder interface.");
				Message val = new Message("GetTaskManager", (object)null);
				((IMessageHandler)current).Message(val);
				TaskManager output = val.GetOutput<TaskManager>(0);
				if (output == null)
				{
					GlobalLog.Error("[BotStructure] \"" + ((IAuthored)current).Name + "\" does not process \"GetTaskManager\" message.");
					ErrorManager.ReportCriticalError();
					return null;
				}
				return output;
			}
			return gInterface.GetTaskManager();
		}
	}

	public static void RemoveTask(string name)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		if (!((TaskManagerBase<ITask>)(object)TaskManager).Remove(name))
		{
			GlobalLog.Error("[BotStructure] Fail to remove \"" + name + "\".");
			BotManager.Stop(new StopReasonData("task_remove_fail", "[BotStructure] Fail to remove \"" + name + "\".", (object)null), false);
		}
	}

	public static IPlugin GetPlugin(string name)
	{
		return Enumerable.FirstOrDefault(PluginManager.Plugins, (IPlugin p) => ((IAuthored)p).Name == name);
	}

	public static IPlugin GetEnabledPlugin(string name)
	{
		return PluginManager.EnabledPlugins.Find((IPlugin p) => ((IAuthored)p).Name == name);
	}

	public static bool IsPluginEnabled(string name)
	{
		return PluginManager.EnabledPlugins.Exists((IPlugin p) => ((IAuthored)p).Name == name);
	}
}
