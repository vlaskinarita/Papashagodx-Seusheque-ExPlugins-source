using System;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;

namespace ExPlugins.EXtensions.Global;

public static class ResurrectionLogic
{
	public static async Task Execute()
	{
		if (!(await Resurrect(toCheckpoint: true)) && !(await Resurrect(toCheckpoint: false)))
		{
			GlobalLog.Error("[ResurrectionLogic] Resurrection failed. Now going to logout.");
			if (!(await Logout()))
			{
				GlobalLog.Error("[ResurrectionLogic] Logout failed. Now stopping the bot because it cannot continue.");
				BotManager.Stop(new StopReasonData("cant_resurrect", "Resurrection failed. Logout failed. Now stopping the bot because it cannot continue.", (object)null), false);
				return;
			}
		}
		GlobalLog.Info("[Events] Player resurrected.");
		Skill move = SkillBarHud.LastBoundMoveSkill;
		SkillBarHud.Use(move.Slots.Last(), true, true);
		Utility.BroadcastMessage((object)null, "player_resurrected_event", Array.Empty<object>());
	}

	private static async Task<bool> Resurrect(bool toCheckpoint, int attempts = 3)
	{
		GlobalLog.Debug("[Resurrect] Now going to resurrect to " + (toCheckpoint ? "checkpoint" : "town") + ".");
		if (!(await Wait.For(() => ResurrectPanel.IsOpened, "ResurrectPanel opening")))
		{
			return false;
		}
		await Wait.SleepSafe(100);
		int i = 1;
		while (i <= attempts)
		{
			GlobalLog.Debug($"[Resurrect] Attempt: {i}/{attempts}");
			if (LokiPoe.IsInGame)
			{
				if (((Actor)LokiPoe.Me).IsDead)
				{
					ResurrectResult err = (toCheckpoint ? ResurrectPanel.ResurrectToCheckPoint(true) : ResurrectPanel.ResurrectToTown(true));
					if ((int)err == 0)
					{
						if (await Wait.For(AliveInGame, "resurrection", 200, 5000))
						{
							GlobalLog.Debug("[Resurrect] Player has been successfully resurrected.");
							await Wait.SleepSafe(250);
							return true;
						}
					}
					else
					{
						GlobalLog.Error($"[Resurrect] Fail to resurrect. Error: \"{err}\".");
						await Wait.SleepSafe(1000, 1500);
					}
					int num = i + 1;
					i = num;
					continue;
				}
				GlobalLog.Debug("[Resurrect] Now exiting this logic because we are no longer dead.");
				return true;
			}
			GlobalLog.Debug("[Resurrect] Now exiting this logic because we are no longer in game.");
			return true;
		}
		GlobalLog.Error("[Resurrect] All resurrection attempts have been spent.");
		return false;
	}

	private static async Task<bool> Logout(int attempts = 5)
	{
		int i = 1;
		while (i <= attempts)
		{
			GlobalLog.Debug($"[Logout] Attempt: {i}/{attempts}");
			if (LokiPoe.IsInGame)
			{
				if (((Actor)LokiPoe.Me).IsDead)
				{
					LogoutError err = EscapeState.LogoutToCharacterSelection();
					if ((int)err == 0)
					{
						if (await Wait.For(() => LokiPoe.IsInLoginScreen, "log out", 200, 5000))
						{
							GlobalLog.Debug("[Logout] Player has been successfully logged out.");
							return true;
						}
					}
					else
					{
						GlobalLog.Error($"[Logout] Fail to log out. Error: \"{err}\".");
						await Wait.SleepSafe(2000, 3000);
					}
					int num = i + 1;
					i = num;
					continue;
				}
				GlobalLog.Debug("[Logout] Now exiting this logic because we are no longer dead.");
				return true;
			}
			GlobalLog.Debug("[Logout] Now exiting this logic because we are no longer in game.");
			return true;
		}
		GlobalLog.Error("[Logout] All logout attempts have been spent.");
		return false;
	}

	private static bool AliveInGame()
	{
		if (LokiPoe.IsInLoginScreen)
		{
			GlobalLog.Error("[Resurrect] Disconnected while waiting for resurrection.");
			return true;
		}
		return !((Actor)LokiPoe.Me).IsDead;
	}
}
