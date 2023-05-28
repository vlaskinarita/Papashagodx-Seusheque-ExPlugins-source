using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EXtensions;

public static class ErrorManager
{
	public const int MaxErrors = 10;

	private static Stopwatch stopwatch_0;

	public static readonly bool HighConfidenceMode;

	private static readonly Dictionary<string, int> dictionary_0;

	private static readonly Dictionary<string, int> dictionary_1;

	[CompilerGenerated]
	private static int int_0;

	public static int ErrorCount
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
		[CompilerGenerated]
		private set
		{
			int_0 = value;
		}
	}

	static ErrorManager()
	{
		stopwatch_0 = new Stopwatch();
		HighConfidenceMode = true;
		dictionary_0 = new Dictionary<string, int>();
		dictionary_1 = new Dictionary<string, int>();
		Events.AreaChanged += delegate
		{
			Reset();
		};
	}

	public static void Reset()
	{
		GlobalLog.Info("[ErrorManager] Error count has been reset.");
		ErrorCount = 0;
		dictionary_0.Clear();
	}

	public static int GetErrorCount(string error)
	{
		dictionary_0.TryGetValue(error, out var value);
		return value;
	}

	public static void ReportError()
	{
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Invalid comparison between Unknown and I4
		ErrorCount = int_0 + 1;
		GlobalLog.Error($"[ErrorManager] Error count: {int_0}/{10}");
		if (int_0 < 10)
		{
			return;
		}
		GlobalLog.Error("[ErrorManager] Error threshold has been reached.");
		if (ExtensionsSettings.Instance.KillPoeOnRareException)
		{
			if (!LokiPoe.Me.IsInHideout && !LokiPoe.Me.IsInTown && !World.Act6.KaruiFortress.IsCurrentArea && !World.Act9.Quarry.IsCurrentArea && !World.Act2.ChamberOfSins1.IsCurrentArea && !World.Act7.ChamberOfSins1.IsCurrentArea && !World.Act2.BrokenBridge.IsCurrentArea && !World.Act2.FellshrineRuins.IsCurrentArea && !World.Act7.FellshrineRuins.IsCurrentArea)
			{
				if (((IAuthored)BotManager.Current).Name == "QuestBotEx" && HighConfidenceMode && PreformLoginScreenWait())
				{
					return;
				}
			}
			else if (PreformUltimateHandling())
			{
				return;
			}
		}
		if (stopwatch_0.IsRunning && stopwatch_0.Elapsed.TotalMinutes < 3.0)
		{
			GlobalLog.Error("[ErrorManager] Logout didn't helped. Stopping the bot.");
			stopwatch_0.Reset();
			BotManager.Stop(new StopReasonData("max_erorrs", "Max errors reached. Logout didn't helped. Stopping the bot..", (object)null), false);
			Reset();
		}
		else
		{
			GlobalLog.Error("[ErrorManager] Let's try to logout first.");
			LogoutError val = EscapeState.LogoutToTitleScreen();
			if ((int)val == 0)
			{
				Reset();
				stopwatch_0 = Stopwatch.StartNew();
			}
		}
		throw new Exception("MAX_ERRORS");
	}

	public static void ReportError(string error)
	{
		dictionary_0.TryGetValue(error, out var value);
		value = (dictionary_0[error] = value + 1);
		GlobalLog.Error($"[ErrorManager] \"{error}\" error count: {value}");
	}

	public static void ReportCriticalError()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		GlobalLog.Error("[CRITICAL ERROR] Now requesting bot to stop.");
		if (LokiPoe.CurrentWorldArea.IsMenagerieArea)
		{
			GlobalLog.Error("[ErrorManager] [KillPoeOnRareException] Logout needed");
			EXtensions.AbandonCurrentArea();
			ErrorCount = 0;
			EscapeState.LogoutToTitleScreen();
		}
		if (World.CurrentArea.IsHideoutArea)
		{
			GlobalLog.Error("[ErrorManager] [KillPoeOnRareException] Hard reset needed for hideout area");
			if (PreformUltimateHandling())
			{
				return;
			}
		}
		if (!(((IAuthored)BotManager.Current).Name == "QuestBotEx") || !HighConfidenceMode || !PreformLoginScreenWait())
		{
			BotManager.Stop(new StopReasonData("critical_error", "[CRITICAL ERROR] Now requesting bot to stop", (object)null), false);
			throw new Exception("CRITICAL_ERROR");
		}
	}

	private static bool PreformLoginScreenWait()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		string name = World.CurrentArea.Name;
		dictionary_1.TryGetValue(name, out var value);
		GlobalLog.Error($"[ErrorManager] Fatal error in {name}. This is {++value} stuck here.");
		if (value < 3)
		{
			value = (dictionary_1[name] = value + 1);
			Utility.BroadcastMessage((object)null, "AL_ForceWait", new object[1] { "" });
			ErrorCount = 0;
			EscapeState.LogoutToTitleScreen();
			result = true;
		}
		return result;
	}

	private static bool PreformUltimateHandling()
	{
		DateTime now = DateTime.Now;
		bool flag = true;
		string name = ((NetworkObject)LokiPoe.Me).Name;
		string path = "error_" + name;
		if (File.Exists(path))
		{
			DateTime lastWriteTime = File.GetLastWriteTime(path);
			flag = (now - lastWriteTime).Minutes >= 7;
			GlobalLog.Debug($"[ErrorManager] - [KillPoeOnRareException] should kill? {flag}");
			File.Delete(path);
		}
		if (!flag)
		{
			GlobalLog.Error("[ErrorManager] - [KillPoeOnRareException] Another exception in short amount of time. Stopping Bot.");
			BotManager.Stop(false);
			return false;
		}
		GlobalLog.Debug("[ErrorManager] - [KillPoeOnRareException] There is no reason to force stop bot yet, lets try to kill poe.");
		File.WriteAllText(path, "sup");
		LokiPoe.Memory.Process.Kill();
		return true;
	}
}
