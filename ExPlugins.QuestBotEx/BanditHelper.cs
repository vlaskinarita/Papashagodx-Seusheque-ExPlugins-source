using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public static class BanditHelper
{
	public const string EramirName = "Eramir";

	public const string AliraName = "Alira";

	public const string KraitynName = "Kraityn";

	public const string OakName = "Oak";

	public static async Task<bool> Kill(NetworkObject bandit)
	{
		if (bandit == (NetworkObject)null)
		{
			GlobalLog.Error("[KillBandit] Bandit object is null.");
			return false;
		}
		await bandit.WalkablePosition().ComeAtOnce();
		if (!(await OpenBanditPanel(bandit)))
		{
			return false;
		}
		TalkToBanditResult err = BanditPanel.KillBandit(true);
		if ((int)err > 0)
		{
			GlobalLog.Error($"[KillBandit] Fail to select \"Kill\" option. Error: \"{err}\".");
			return false;
		}
		return true;
	}

	public static async Task<bool> Help(NetworkObject bandit)
	{
		if (bandit == (NetworkObject)null)
		{
			GlobalLog.Error("[HelpBandit] Bandit object is null.");
			return false;
		}
		WalkablePosition banditPos = bandit.WalkablePosition();
		await banditPos.ComeAtOnce();
		if (!(await PlayerAction.Interact(bandit)))
		{
			return false;
		}
		int i = 1;
		while (i <= 10)
		{
			await Wait.SleepSafe(200);
			if (BanditPanel.IsOpened || !NpcDialogUi.IsOpened || NpcDialogUi.DialogDepth == 1)
			{
				break;
			}
			GlobalLog.Debug($"[HelpBandit] Pressing ESC to close the topmost NPC dialog ({i}/{10}).");
			Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
			int num = i + 1;
			i = num;
		}
		if (BanditPanel.IsOpened)
		{
			TalkToBanditResult err = BanditPanel.HelpBandit(true);
			if ((int)err > 0)
			{
				GlobalLog.Error($"[HelpBandit] Fail to select \"Help\" option. Error: \"{err}\".");
				return false;
			}
		}
		return await bandit.Fresh<NetworkObject>().AsTownNpc().TakeReward(null, "Get the Apex");
	}

	private static async Task<bool> OpenBanditPanel(NetworkObject bandit)
	{
		if (BanditPanel.IsOpened)
		{
			return true;
		}
		if (await PlayerAction.Interact(bandit))
		{
			int i = 1;
			while (i <= 10)
			{
				if (!BanditPanel.IsOpened)
				{
					GlobalLog.Debug($"[OpenBanditPanel] Pressing ESC to close the topmost NPC dialog ({i}/{10}).");
					Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
					await Wait.SleepSafe(500);
					int num = i + 1;
					i = num;
					continue;
				}
				return true;
			}
			GlobalLog.Error("[OpenBanditPanel] All attempts have been spent.");
			return false;
		}
		return false;
	}
}
