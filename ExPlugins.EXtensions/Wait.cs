using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions;

public static class Wait
{
	private static readonly Stopwatch stopwatch_0;

	public static async Task<bool> For(Func<bool> condition, string desc = "something", int step = 100, int timeout = 3000, bool log = false)
	{
		return await For(condition, desc, () => step, timeout, log);
	}

	private static async Task<bool> For(Func<bool> condition, string desc, Func<int> step, int timeout = 3000, bool log = false)
	{
		if (!condition())
		{
			Stopwatch timer = Stopwatch.StartNew();
			while (timer.ElapsedMilliseconds < timeout)
			{
				await StuckDetectionSleep(step());
				if (log)
				{
					GlobalLog.Debug($"[WaitFor] Waiting for {desc} ({Math.Round((float)timer.ElapsedMilliseconds / 1000f, 2)}/{(float)timeout / 1000f})");
				}
				if (condition())
				{
					return true;
				}
			}
			GlobalLog.Error("[WaitFor] Wait for " + desc + " timeout.");
			return false;
		}
		return true;
	}

	public static async Task Sleep(int ms)
	{
		await Coroutine.Sleep(ms);
	}

	public static async Task SleepSafe(int ms)
	{
		if (ms > 100)
		{
			int timeout = Math.Max(LatencyTracker.Current, ms);
			await Coroutine.Sleep(timeout);
		}
		await Coroutine.Sleep(ms);
	}

	public static async Task SleepSafe(int min, int max)
	{
		int latency = LatencyTracker.Current;
		if (latency <= max || max <= 100)
		{
			await Coroutine.Sleep(LokiPoe.Random.Next(min, max + 1));
		}
		else
		{
			await Coroutine.Sleep(latency);
		}
	}

	public static async Task LatencySleep(bool longer = false)
	{
		int timeout = Math.Max(LatencyTracker.Current, 15);
		if (longer)
		{
			timeout = Math.Max(LatencyTracker.Current, LokiPoe.Random.Next(100, 150));
		}
		if (timeout > 150)
		{
			GlobalLog.Debug($"[LatencySleep] {timeout} ms.");
		}
		await Coroutine.Sleep(timeout);
	}

	public static async Task<bool> ForAreaChange(uint areaHash, int timeout = 60000)
	{
		if (await For(() => StateManager.IsAreaLoadingStateActive, "loading screen"))
		{
			return await For(() => LokiPoe.IsInGame, "is ingame", 200, timeout);
		}
		return await For(() => ExilePather.AreaHash != areaHash, "area change", 500, timeout);
	}

	public static async Task<bool> ForHideoutChange(int timeout = 60000)
	{
		if (await For(() => StateManager.IsAreaLoadingStateActive, "loading screen"))
		{
			return await For(() => LokiPoe.IsInGame, "is ingame", 200, timeout);
		}
		return false;
	}

	public static async Task StuckDetectionSleep(int ms)
	{
		StuckDetection.Reset();
		try
		{
			await Coroutine.Sleep(ms);
		}
		catch (Exception ex)
		{
			Exception e = ex;
			GlobalLog.Debug(e);
		}
	}

	public static async Task IdRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand2 = LokiPoe.Random.Next(0, 100000);
		rand2 = (int)((double)rand2 * (0.8 + ExtensionsSettings.Instance.GenericPauseDynamicFactor + (double)stopwatch_0.ElapsedMilliseconds / 10000000.0) * ExtensionsSettings.Instance.IdPauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand2 = (int)((double)rand2 * 1.1);
		}
		if (rand2 > 99990)
		{
			stopwatch_0.Restart();
			ExtensionsSettings.Instance.GenericPauseDynamicFactor = 0.01;
			ms = LokiPoe.Random.Next(20000, 240000);
		}
		else if (rand2 > 99000)
		{
			ms = LokiPoe.Random.Next(1000, 5000);
		}
		else if (rand2 > 96000)
		{
			ms = LokiPoe.Random.Next(600, 1300);
		}
		else if (rand2 <= 93000)
		{
			if (rand2 > 80000)
			{
				ms = LokiPoe.Random.Next(70, 300);
			}
			else if (rand2 > 70000)
			{
				ms = LokiPoe.Random.Next(10, 100);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(200, 700);
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[IdRandomDelay(rolled:{rand2})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.IdPause].Add(new PauseData("IdRandomDelay", rand2, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
			ExtensionsSettings.Instance.GenericPauseDynamicFactor -= 0.01;
		}
		else
		{
			ExtensionsSettings.Instance.GenericPauseDynamicFactor += 0.0005;
		}
	}

	public static async Task StashRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand2 = LokiPoe.Random.Next(0, 100000);
		rand2 = (int)((double)rand2 * (0.8 + ExtensionsSettings.Instance.GenericPauseDynamicFactor + (double)stopwatch_0.ElapsedMilliseconds / 10000000.0) * ExtensionsSettings.Instance.StashPauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand2 = (int)((double)rand2 * 1.1);
		}
		if (rand2 > 99990)
		{
			stopwatch_0.Restart();
			ExtensionsSettings.Instance.GenericPauseDynamicFactor = 0.01;
			ms = LokiPoe.Random.Next(20000, 240000);
		}
		else if (rand2 <= 99900)
		{
			if (rand2 <= 99000)
			{
				if (rand2 <= 95000)
				{
					if (rand2 <= 90000)
					{
						if (rand2 > 85000)
						{
							ms = LokiPoe.Random.Next(10, 100);
						}
					}
					else
					{
						ms = LokiPoe.Random.Next(60, 300);
					}
				}
				else
				{
					ms = LokiPoe.Random.Next(200, 700);
				}
			}
			else
			{
				ms = LokiPoe.Random.Next(500, 1000);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(1000, 5000);
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[StashRandomDelay(rolled:{rand2})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.StashPause].Add(new PauseData("StashRandomDelay", rand2, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
			ExtensionsSettings.Instance.GenericPauseDynamicFactor -= 0.01;
		}
		else
		{
			ExtensionsSettings.Instance.GenericPauseDynamicFactor += 0.0005;
		}
	}

	public static async Task SellRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand2 = LokiPoe.Random.Next(0, 100000);
		rand2 = (int)((double)rand2 * (0.7 + ExtensionsSettings.Instance.GenericPauseDynamicFactor + (double)stopwatch_0.ElapsedMilliseconds / 10000000.0) * ExtensionsSettings.Instance.SellPauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand2 = (int)((double)rand2 * 1.1);
		}
		if (rand2 > 99990)
		{
			stopwatch_0.Restart();
			ExtensionsSettings.Instance.GenericPauseDynamicFactor = 0.01;
			ms = LokiPoe.Random.Next(20000, 240000);
		}
		else if (rand2 > 99900)
		{
			ms = LokiPoe.Random.Next(1000, 5000);
		}
		else if (rand2 <= 99000)
		{
			if (rand2 > 95000)
			{
				ms = LokiPoe.Random.Next(150, 600);
			}
			else if (rand2 > 90000)
			{
				ms = LokiPoe.Random.Next(60, 300);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(500, 1000);
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[SellRandomDelay(rolled:{rand2})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.SellPause].Add(new PauseData("SellRandomDelay", rand2, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
			ExtensionsSettings.Instance.GenericPauseDynamicFactor -= 0.01;
		}
		else
		{
			ExtensionsSettings.Instance.GenericPauseDynamicFactor += 0.0005;
		}
	}

	public static async Task FastMoveFromStashRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand = (int)((double)LokiPoe.Random.Next(0, 100000) * ExtensionsSettings.Instance.StashFastMovePauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand = (int)((double)rand * 1.1);
		}
		if (rand > 99995)
		{
			ms = LokiPoe.Random.Next(20000, 240000);
		}
		else if (rand <= 99950)
		{
			if (rand > 99000)
			{
				ms = LokiPoe.Random.Next(500, 1000);
			}
			else if (rand > 95000)
			{
				ms = LokiPoe.Random.Next(150, 600);
			}
			else if (rand > 90000)
			{
				ms = LokiPoe.Random.Next(60, 300);
			}
			else if (rand > 80000)
			{
				ms = LokiPoe.Random.Next(10, 100);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(1000, 5000);
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[FastMoveFromStashRandomDelay(rolled:{rand})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.StashFastMovePause].Add(new PauseData("FastMoveFromStashRandomDelay", rand, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
		}
	}

	public static async Task FastMoveToVendorRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand = (int)((double)LokiPoe.Random.Next(0, 100000) * ExtensionsSettings.Instance.VendorFastMovePauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand = (int)((double)rand * 1.1);
		}
		if (rand > 99000)
		{
			ms = LokiPoe.Random.Next(500, 1000);
		}
		else if (rand <= 95000)
		{
			if (rand > 90000)
			{
				ms = LokiPoe.Random.Next(60, 300);
			}
			else if (rand > 80000)
			{
				ms = LokiPoe.Random.Next(10, 100);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(150, 600);
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[FastMoveToVendorRandomDelay(rolled:{rand})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.VendorFastMovePause].Add(new PauseData("FastMoveToVendorRandomDelay", rand, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
		}
	}

	public static async Task TownMoveRandomDelay()
	{
		Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool isMuling = default(bool);
		if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
		{
			return;
		}
		int ms = 0;
		int rand2 = LokiPoe.Random.Next(0, 100000);
		rand2 = (int)((double)rand2 * (0.9 + ExtensionsSettings.Instance.TownPauseDynamicFactor + (double)stopwatch_0.ElapsedMilliseconds / 10000000.0) * ExtensionsSettings.Instance.TownMovePauseFactor);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			rand2 = (int)((double)rand2 * 1.1);
		}
		if (rand2 > 99990)
		{
			stopwatch_0.Restart();
			ExtensionsSettings.Instance.TownPauseDynamicFactor = 0.01;
			ms = LokiPoe.Random.Next(20000, 120000);
		}
		else if (rand2 > 99900)
		{
			stopwatch_0.Restart();
			ExtensionsSettings.Instance.TownPauseDynamicFactor = 0.01;
			ms = LokiPoe.Random.Next(10000, 20000);
		}
		else if (rand2 <= 95000)
		{
			if (rand2 <= 55000)
			{
				if (rand2 > 45000)
				{
					ms = LokiPoe.Random.Next(40, 100);
				}
			}
			else
			{
				ms = LokiPoe.Random.Next(100, 500);
			}
		}
		else
		{
			ms = LokiPoe.Random.Next(3000, 10000);
			ExtensionsSettings.Instance.TownPauseDynamicFactor -= 0.01;
		}
		if (ms > 0)
		{
			GlobalLog.Warn($"[TownMoveRandomDelay(rolled:{rand2})] Pause will last {ms} ms. [range: 0, 0]");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.TownMovePause].Add(new PauseData("TownMoveRandomDelay", rand2, ms, new Range(0, 0)));
			await Coroutine.Sleep(ms);
		}
		else
		{
			ExtensionsSettings.Instance.TownPauseDynamicFactor += 0.001;
		}
	}

	public static bool NpcTalkPauseProbability(int probability)
	{
		Message val = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool flag = default(bool);
		if (val.TryGetOutput<bool>(0, ref flag) && flag)
		{
			return false;
		}
		int num = LokiPoe.Random.Next(0, 10000);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			num = (int)((double)num * 1.1);
		}
		if (num > probability * 100)
		{
			return false;
		}
		GlobalLog.Warn($"[NpcTalkPauseProbability(rolled:{num})] Trigger random move before to talk with npc");
		ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.NpcTalkPauseProbability].Add(new PauseData("NpcTalkPauseProbability", num, 0, new Range(0, 0)));
		return true;
	}

	public static bool StashPauseProbability(int probability)
	{
		Message val = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool flag = default(bool);
		if (val.TryGetOutput<bool>(0, ref flag) && flag)
		{
			return false;
		}
		int num = LokiPoe.Random.Next(0, 10000);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			num = (int)((double)num * 1.1);
		}
		if (num <= probability * 100)
		{
			GlobalLog.Warn($"[StashPauseProbability(rolled:{num})] Trigger random move before to interact stash");
			ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.StashPauseProbability].Add(new PauseData("StashPauseProbability", num, 0, new Range(0, 0)));
			return true;
		}
		return false;
	}

	public static bool WaypointPauseProbability(int probability)
	{
		Message val = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
		bool flag = default(bool);
		if (val.TryGetOutput<bool>(0, ref flag) && flag)
		{
			return false;
		}
		int num = LokiPoe.Random.Next(0, 10000);
		if (((IAuthored)BotManager.Current).Name == "QuestBotEx")
		{
			num = (int)((double)num * 1.1);
		}
		if (num > probability * 100)
		{
			return false;
		}
		GlobalLog.Warn($"[WaypointPauseProbability(rolled:{num})] Trigger random move before to interact Waypoint");
		ExtensionsSettings.Instance.PauseDataCollection[PauseTypeEnum.WaypointPauseProbability].Add(new PauseData("WaypointPauseProbability", num, 0, new Range(0, 0)));
		return true;
	}

	static Wait()
	{
		stopwatch_0 = Stopwatch.StartNew();
	}
}
