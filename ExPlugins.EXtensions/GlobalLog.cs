using DreamPoeBot.Loki.Common;
using log4net;

namespace ExPlugins.EXtensions;

public static class GlobalLog
{
	private static readonly ILog ilog_0;

	public static void Debug(object message)
	{
		ilog_0.Debug(message);
	}

	public static void Debug(string message)
	{
		ilog_0.Debug((object)message);
	}

	public static void Info(object message)
	{
		ilog_0.Info(message);
	}

	public static void Info(string message)
	{
		ilog_0.Info((object)message);
	}

	public static void Warn(object message)
	{
		ilog_0.Warn(message);
	}

	public static void Warn(string message)
	{
		ilog_0.Warn((object)message);
	}

	public static void Error(object message)
	{
		ilog_0.Error(message);
	}

	public static void Error(string message)
	{
		ilog_0.Error((object)message);
	}

	public static void Fatal(object message)
	{
		ilog_0.Fatal(message);
	}

	public static void Fatal(string message)
	{
		ilog_0.Fatal((object)message);
	}

	static GlobalLog()
	{
		ilog_0 = Logger.GetLoggerInstanceForType();
	}
}
