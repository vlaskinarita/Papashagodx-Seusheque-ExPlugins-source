using System;
using System.Threading.Tasks;

namespace ExPlugins.QuestBotEx;

public class QuestHandler
{
	public static QuestHandler QuestAddedToCache;

	public static QuestHandler AllQuestsDone;

	public readonly Func<Task<bool>> Execute;

	public readonly Action Tick;

	public QuestHandler(Func<Task<bool>> execute, Action tick)
	{
		Execute = execute;
		Tick = tick;
	}

	static QuestHandler()
	{
		QuestAddedToCache = new QuestHandler(null, null);
		AllQuestsDone = new QuestHandler(null, null);
	}
}
