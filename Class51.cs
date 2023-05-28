using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BulkTraderEx.Classes;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx;

internal class Class51
{
	public static bool IsInZone(string name)
	{
		return ObjectManager.GetObjectsByType<Player>().Any((Player x) => ((NetworkObject)x).Name == name);
	}

	public static async Task<bool> GotoPartyZone(string playerName)
	{
		PartyMember partyMember = InstanceInfo.PartyMembers.FirstOrDefault((PartyMember x) => x.PlayerEntry.Name == playerName);
		if ((RemoteMemoryObject)(object)partyMember == (RemoteMemoryObject)null)
		{
			return false;
		}
		int trynr = 0;
		while (true)
		{
			trynr++;
			if (trynr <= 2)
			{
				if (IsInZone(playerName))
				{
					return true;
				}
				await Coroutines.CloseBlockingWindows();
				await Coroutines.LatencyWait();
				if (!(await PlayerAction.FastTravelTo(partyMember.PlayerEntry.Name)))
				{
					await Wait.SleepSafe(5000, 7000);
					if (!(await PlayerAction.FastTravelTo(partyMember.PlayerEntry.Name)))
					{
						break;
					}
				}
				continue;
			}
			GlobalLog.Warn($"[GotoPartyZone] Failed to go to seller zone {trynr} times, ABORT!");
			return false;
		}
		GlobalLog.Error("[GotoPartyzone] Swirly button didn't worked. Trying to go to hideout.");
		await MapBotEx.SendChatMsg("/hideout " + partyMember.PlayerEntry.Name);
		return await Wait.ForHideoutChange(45000);
	}

	public static async Task Kick(string name)
	{
		await MapBotEx.SendChatMsg("/kick " + name);
	}

	public static async Task<bool> LeaveGroup()
	{
		if (!InstanceInfo.PartyMembers.Any())
		{
			return true;
		}
		await Kick(((NetworkObject)LokiPoe.Me).Name);
		await Coroutines.CloseBlockingWindows();
		await Coroutines.ReactionWait();
		return true;
	}

	public static async Task<bool> AcceptParty(IReadOnlyCollection<TradeCurrency> alreadyRequested)
	{
		if (NotificationHud.NotificationList.Where((NotificationInfo x) => x.IsVisible).ToList().Count > 0)
		{
			GlobalLog.Warn($"[TradeTask] Visible Notifications: {NotificationHud.NotificationList.Where((NotificationInfo x) => x.IsVisible).ToList().Count}");
			if (NotificationHud.NotificationList.Any((NotificationInfo x) => x.IsVisible))
			{
				GlobalLog.Debug("[TradeTask] small wait");
				await Wait.Sleep(700);
			}
			HandleNotificationResult ret = NotificationHud.HandleNotificationEx((ProcessNotificationEx)delegate(NotificationData x, NotificationType y)
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Invalid comparison between Unknown and I4
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				bool flag = alreadyRequested.Any((TradeCurrency c) => c.SellerAccount == x.AccountName || c.SellerIgn == x.CharacterName) && (int)y == 1;
				GlobalLog.Warn($"[TradeTask] Detected {y} request from char: {x.CharacterName} [AccountName: {x.AccountName}] Accepting? {flag}");
				return flag;
			}, true);
			await Coroutines.LatencyWait();
			if ((int)ret == 0)
			{
				Utility.BroadcastMessage((object)null, "SuspendTrades", new object[1] { "" });
				return true;
			}
		}
		return false;
	}
}
