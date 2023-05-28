using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.PapashaCore;

namespace ExPlugins.EXtensions;

public class EXtensions : IContent, IAuthored, IBase, IConfigurable, ILogicProvider, IMessageHandler, IUrlProvider, ITickEvents, IStartStopEvents
{
	private static readonly Interval interval_0;

	private Gui gui_0;

	public string Name => "EXtensions";

	public string Description => "Global logic used by bot bases.";

	public string Author => "ExVault / Mod by Papashagodx + Seusheque";

	public string Version => "1.3";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public JsonSettings Settings => (JsonSettings)(object)ExtensionsSettings.Instance;

	public UserControl Control => gui_0 ?? (gui_0 = new Gui());

	public MessageResult Message(Message message)
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		if (ExtensionsSettings.Instance.PromtMessagesToLog && message.Id != "Raycast_Result")
		{
			GlobalLog.Warn("[Message] " + message.Id);
		}
		if (message.Id == "player_died_event")
		{
			foreach (NotificationEntry item in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == NotificationType.Death))
			{
				int input = message.GetInput<int>(0);
				DiscordNotifier.AddNotification($" died {input} times in current session!", item.UseAddition);
			}
			return (MessageResult)0;
		}
		if (message.Id == "player_leveled_event")
		{
			foreach (NotificationEntry item2 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == NotificationType.LevelUp))
			{
				DiscordNotifier.AddNotification($" leveled up! current lvl: {message.GetInput<int>(0)}", item2.UseAddition);
			}
			return (MessageResult)0;
		}
		if (!(message.Id == "item_stashed_event"))
		{
			if (message.Id == "simulacrum_wave_start")
			{
				foreach (NotificationEntry item3 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.SimulacrumWaveStart))
				{
					dynamic input2 = message.GetInput<object>(0);
					DiscordNotifier.AddNotification(input2.ToString(), item3.UseAddition);
				}
				return (MessageResult)0;
			}
			if (!(message.Id == "crucible_found"))
			{
				if (message.Id == "stat_map_finished")
				{
					foreach (NotificationEntry item4 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.MapFinished))
					{
						dynamic input3 = message.GetInput<object>(0);
						DiscordNotifier.AddNotification(input3.ToString(), item4.UseAddition);
					}
					return (MessageResult)0;
				}
				if (!(message.Id == "blight_finished"))
				{
					if (!(message.Id == "blight_engaged"))
					{
						if (message.Id == "box_opened")
						{
							foreach (NotificationEntry item5 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.StrongboxOpened))
							{
								dynamic input4 = message.GetInput<object>(0);
								DiscordNotifier.AddNotification(input4.ToString(), item5.UseAddition);
							}
							return (MessageResult)0;
						}
						if (message.Id.StartsWith("autologin_"))
						{
							DiscordNotifier.HandleAutoLoginMessage(message);
							return (MessageResult)0;
						}
						if (!message.Id.Equals("lootitem_mirror_fail"))
						{
							if (Enumerable.Any(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == NotificationType.Custom && e.Content.EqualsIgnorecase(message.Id)))
							{
								using IEnumerator<NotificationEntry> enumerator6 = Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == NotificationType.Custom && e.Content.EqualsIgnorecase(message.Id)).GetEnumerator();
								if (enumerator6.MoveNext())
								{
									NotificationEntry current6 = enumerator6.Current;
									dynamic input5 = message.GetInput<object>(0);
									DiscordNotifier.AddNotification(input5.ToString(), current6.UseAddition);
									return (MessageResult)0;
								}
							}
							return (MessageResult)1;
						}
						string input6 = message.GetInput<string>(0);
						foreach (NotificationEntry item6 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == NotificationType.MirrorFailedToPickup))
						{
							DiscordNotifier.AddNotification("[Mirror pickup fail] " + input6, item6.UseAddition);
						}
						return (MessageResult)0;
					}
					foreach (NotificationEntry item7 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.BlightStarted))
					{
						dynamic input7 = message.GetInput<object>(0);
						DiscordNotifier.AddNotification(input7.ToString(), item7.UseAddition);
					}
					return (MessageResult)0;
				}
				foreach (NotificationEntry item8 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.BlightFinished))
				{
					dynamic input8 = message.GetInput<object>(0);
					DiscordNotifier.AddNotification(input8.ToString(), item8.UseAddition);
				}
				return (MessageResult)0;
			}
			foreach (NotificationEntry item9 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.CrucibleFound))
			{
				dynamic input9 = message.GetInput<object>(0);
				DiscordNotifier.AddNotification(input9.ToString(), item9.UseAddition);
			}
			return (MessageResult)0;
		}
		CachedItem cachedItem_0 = message.GetInput<CachedItem>(0);
		foreach (NotificationEntry item10 in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry i) => i.NotifType == NotificationType.ItemStashed && i.Content.Equals(cachedItem_0.FullName)))
		{
			DiscordNotifier.AddNotification($"{cachedItem_0.StackCount}x {cachedItem_0.FullName} was stashed!", item10.UseAddition);
		}
		return (MessageResult)0;
	}

	public static void AbandonCurrentArea()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		string name = ((IAuthored)BotManager.Current).Name;
		if (name.Contains("QuestBotEx"))
		{
			Travel.RequestNewInstance(World.CurrentArea);
		}
		else if (name.Contains("MapBotEx"))
		{
			((IMessageHandler)BotManager.Current).Message(new Message("MB_set_is_on_run", (object)null, new object[1] { false }));
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Initialize()
	{
		if (!Directory.Exists("State/"))
		{
			GlobalLog.Warn("[EXtensions] Creating State folder");
			Directory.CreateDirectory("State/");
		}
	}

	public void Deinitialize()
	{
	}

	public void Tick()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (interval_0.Elapsed && (Enumerable.FirstOrDefault(PluginManager.EnabledPlugins, (IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready))
		{
			GlobalLog.Error("[EXtensions] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (Enumerable.FirstOrDefault(PluginManager.EnabledPlugins, (IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(global::ExPlugins.PapashaCore.PapashaCore.Fork()) || !global::ExPlugins.PapashaCore.PapashaCore.ready)
		{
			GlobalLog.Error("[EXtensions] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
	}

	private static bool MoveNext(string s)
	{
		bool flag = false;
		for (int i = 7; i <= 12; i++)
		{
			if (s[i] == 'y')
			{
				flag = true;
				break;
			}
		}
		string text = Regex.Replace(s, "[^0-9.]", "");
		bool flag2 = char.GetNumericValue(text[0]) + char.GetNumericValue(text[1]) == 9.0;
		bool flag3 = char.GetNumericValue(text[3]) + char.GetNumericValue(text[4]) + char.GetNumericValue(text[5]) + char.GetNumericValue(text[6]) == 23.0;
		return flag && flag2 && flag3;
	}

	public void Stop()
	{
	}

	static EXtensions()
	{
		interval_0 = new Interval(1500);
	}
}
