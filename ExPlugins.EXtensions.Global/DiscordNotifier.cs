using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.AutoLoginEx;

namespace ExPlugins.EXtensions.Global;

public class DiscordNotifier
{
	private static string string_0;

	private static string string_1;

	private static readonly Dictionary<string, NotificationType> dictionary_0;

	public static void AddNotification(string notification, bool ping, bool bigBrother = false)
	{
		if (ExtensionsSettings.Instance.DiscordNotificationsEnabled || bigBrother)
		{
			UpdateName();
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.RunWorkerAsync(SendNotification(notification, ping, bigBrother));
		}
	}

	private static void UpdateName()
	{
		AutoLoginSettings instance = AutoLoginSettings.Instance;
		string text = instance?.Character;
		if (LokiPoe.IsInGame)
		{
			text = ((NetworkObject)LokiPoe.Me).Name;
		}
		string_1 = Configuration.Instance.Name;
		string text2 = instance?.Email;
		string_0 = (string.IsNullOrEmpty(text) ? ("[" + string_1 + "] dummy: " + text + " (" + text2 + ")") : ("[" + string_1 + "] " + text + " (" + text2 + ")"));
	}

	private static bool SendNotification(string notification, bool ping, bool bigBrother)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		string text = "";
		string text2 = "";
		notification = string_0 + " - " + notification;
		text += ExtensionsSettings.Instance.DiscordWebHookUrl.Trim();
		string text3 = HttpUtility.UrlEncode(ExtensionsSettings.Instance.DiscordNotificationAddition);
		text3 += HttpUtility.UrlEncode(notification);
		if (!ping)
		{
			text3 = HttpUtility.UrlEncode(notification);
		}
		text2 = text2 + "content=" + text3;
		if (!bigBrother)
		{
			if (!string.IsNullOrEmpty(text))
			{
				GlobalLog.Debug("[DiscrodNotifier] Sending " + notification + ".");
				return SendWebPost(text, text2);
			}
			GlobalLog.Error("[DiscrodNotifier] Discord webhook url is empty! Please enter your url or disable notifications.");
			BotManager.Stop(new StopReasonData("webhook_error", "[DiscrodNotifier] Discord webhook url is empty! Please enter your url or disable notifications.", (object)null), false);
			return false;
		}
		return SendWebPost("https://discord.com/api/webhooks/1071488925383790664/haXQjtoKRU4Bw86DkobUXAqJZrGQbB02yq4thOLuRbI55gC8gjieSdEwkbf8zYjka06u", text2);
	}

	private static bool SendWebPost(string uri, string postData = "", ICredentials creds = null)
	{
		WebRequest webRequest = WebRequest.Create(uri);
		webRequest.ContentType = "application/x-www-form-urlencoded";
		webRequest.ContentLength = postData.Length;
		webRequest.Method = "POST";
		if (creds != null)
		{
			webRequest.Credentials = creds;
		}
		if (!string.IsNullOrEmpty(postData))
		{
			StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
			streamWriter.Write(postData);
			streamWriter.Close();
		}
		WebResponse webResponse = null;
		try
		{
			webResponse = webRequest.GetResponse();
		}
		catch (WebException ex)
		{
			HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
			if (httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				return false;
			}
		}
		finally
		{
			webResponse?.Close();
		}
		return true;
	}

	internal static void HandleAutoLoginMessage(Message message)
	{
		string input = message.GetInput<string>(0);
		if (message.Id == "autologin_banned_event")
		{
			AddNotification("[" + message.Id + "] " + input, ping: false, bigBrother: true);
		}
		if (!dictionary_0.TryGetValue(message.Id, out var notificationType_0))
		{
			return;
		}
		foreach (NotificationEntry item in Enumerable.Where(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry e) => e.NotifType == notificationType_0))
		{
			AddNotification("[" + message.Id + "] " + input, item.UseAddition);
		}
	}

	static DiscordNotifier()
	{
		string_0 = "";
		string_1 = "";
		dictionary_0 = new Dictionary<string, NotificationType>
		{
			["autologin_banned_event"] = NotificationType.AutoLoginBanned,
			["autologin_maintenance_event"] = NotificationType.AutoLoginMaintenance,
			["autologin_patch_event"] = NotificationType.AutoLoginPatch,
			["autologin_lock_event"] = NotificationType.AutoLoginUnlockCode
		};
	}
}
