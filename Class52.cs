using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.AutoLoginEx;
using ExPlugins.AutoLoginEx.Helpers;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore;

internal class Class52 : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, ITickEvents, IStartStopEvents, IUrlProvider
{
	public static RandomHelper randomHelper_0;

	private static readonly Interval interval_0;

	private static readonly Interval interval_1;

	private static bool bool_0;

	private ExPlugins.AutoLoginEx.Gui gui_0;

	private DateTime dateTime_0 = DateTime.Now;

	private bool bool_1;

	private bool bool_2;

	private string string_0 = "";

	private CharacterClass characterClass_0 = (CharacterClass)199;

	private int int_0;

	private int int_1;

	private Stopwatch stopwatch_0;

	private static readonly AutoLoginSettings Config;

	public List<string> list_0;

	public JsonSettings Settings => (JsonSettings)(object)Config;

	public System.Windows.Controls.UserControl Control => gui_0 ?? (gui_0 = new ExPlugins.AutoLoginEx.Gui());

	public string Name => "AutoLoginEx";

	public string Author => "Lajt";

	public string Description => "AutoLoginEx";

	public string Version => "1.0";

	public string Url => "https://discord.gg/HeqYtkujWW";

	public async Task<LogicResult> Logic(Logic logic)
	{
		if (!(logic.Id == "hook_login_screen"))
		{
			if (!(logic.Id == "hook_character_selection"))
			{
				if (!(logic.Id == "hook_character_creation"))
				{
					return (LogicResult)1;
				}
				if (!(await character_creation(logic)))
				{
					return (LogicResult)1;
				}
				return (LogicResult)0;
			}
			if (!(await character_selection(logic)))
			{
				return (LogicResult)1;
			}
			return (LogicResult)0;
		}
		if (await login_screen(logic))
		{
			return (LogicResult)0;
		}
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "SetNextLoginTime")
		{
			Config.NextLoginTime = message.GetInput<DateTime>(0);
			GlobalLog.Info($"[AutoLoginEx] Execute({message.Id}) = {Config.NextLoginTime}.");
			return (MessageResult)0;
		}
		if (message.Id == "GetNextLoginTime")
		{
			message.AddOutput<DateTime>((IMessageHandler)(object)this, Config.NextLoginTime, "");
			return (MessageResult)0;
		}
		if (message.Id == "AL_ForceWait" && ExtensionsSettings.Instance.LoginPauseAfterError)
		{
			stopwatch_0 = Stopwatch.StartNew();
			int_1++;
			GlobalLog.Info($"[AutoLoginEx] AL_ForceWait, this is {int_1} wait requested.");
			return (MessageResult)0;
		}
		if (message.Id == "player_leveled_event")
		{
			if (!string.IsNullOrEmpty(Config.Character))
			{
				return (MessageResult)1;
			}
			CharacterEntry characterEntry = Config.CharacterSetup.FirstOrDefault((CharacterEntry d) => !d.IsCompleted && d.CharacterClass == ((Player)LokiPoe.Me).Class);
			if (characterEntry == null)
			{
				GlobalLog.Error("[AutoLoginEx] current is null, why?");
				BotManager.Stop(false);
				return (MessageResult)0;
			}
			if (((Player)LokiPoe.Me).Level >= characterEntry.TargetLevel)
			{
				characterEntry.IsCompleted = true;
				Config.DummyCharacter = "";
				EscapeState.LogoutToTitleScreen();
				return (MessageResult)0;
			}
			HashSet<CharacterEntry> hashSet = Config.CharacterSetup.Where((CharacterEntry d) => (int)d.CharacterClass != 199 && d.TargetLevel != 0).ToHashSet();
			int count = hashSet.Count;
			int num = hashSet.Count((CharacterEntry i) => i.IsCompleted);
			if (count - 1 == num)
			{
				GlobalLog.Debug("[AutoLoginEx] All completed, setting this as final character");
				Config.DummyCharacter = "";
				Config.Character = ((NetworkObject)LokiPoe.Me).Name;
				return (MessageResult)0;
			}
		}
		return (MessageResult)1;
	}

	public void Enable()
	{
		Config.LoginAttempts = 0;
		Config.NextLoginTime = DateTime.Now;
		Config.SelectCharacterAttempts = 0;
	}

	public void Initialize()
	{
		list_0 = FileHelper.LoadWords().ToList();
	}

	public void Start()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready)
		{
			GlobalLog.Error("[AutoLoginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (list_0 == null || list_0.Count < 100)
		{
			GlobalLog.Error("[AutoLoginEx] Wordlist is null or has not enough elements. Minimum 100 words required.");
			BotManager.Stop(new StopReasonData("wordlist_error", "Wordlist is null or has not enough elements. Minimum 100 words required.", (object)null), false);
			return;
		}
		randomHelper_0 = new RandomHelper(list_0);
		randomHelper_0.Generate();
		Config.DummyCharacter = "";
		Config.CharacterSetup.Clear();
		List<CharacterClass> randomClasses = randomHelper_0.GetRandomClasses(Config.DummyAmount, Config.FinalClass);
		int num = 0;
		foreach (CharacterClass item in randomClasses)
		{
			int minLevel = Config.DummySetup[num].MinLevel;
			int maxLevel = Config.DummySetup[num].MaxLevel;
			Config.CharacterSetup.Add(new CharacterEntry(item, randomHelper_0.GetRandomNumber(minLevel, maxLevel)));
			num++;
		}
		Config.CharacterSetup.Add(new CharacterEntry(Config.FinalClass, 101));
		foreach (CharacterEntry item2 in Config.CharacterSetup)
		{
			GlobalLog.Debug($"[AutoLoginEx] {item2.CharacterClass} {item2.TargetLevel} {item2.IsCompleted}");
		}
	}

	public void Tick()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "PapashaCore") == null || !MoveNext(PapashaCore.Fork()) || !PapashaCore.ready)
		{
			GlobalLog.Error("[AutoLoginEx] Please enable PapashaCore or disable this plugin to proceed.");
			BotManager.Stop(new StopReasonData("enable_papashacore", "Please enable PapashaCore or disable this plugin to proceed.", (object)null), false);
		}
		if (LokiPoe.IsInGame)
		{
			if (int_0 != 0)
			{
				GlobalLog.Info($"[AutoLoginEx] Now clearing _unexpected ({int_0}).");
				int_0 = 0;
			}
			if (interval_1.Elapsed && (string.IsNullOrWhiteSpace(Config.LastLeague) || !Config.LastLeague.Equals(LokiPoe.Me.League)))
			{
				Config.LastLeague = LokiPoe.Me.League;
			}
		}
		if (bool_2 && (LokiPoe.IsInGame || LokiPoe.IsInCharacterSelectionScreen))
		{
			Config.LoginAttempts = 0;
			Config.NextLoginTime = DateTime.Now;
			bool_2 = false;
		}
		if (bool_1 && (LokiPoe.IsInGame || LokiPoe.IsInLoginScreen))
		{
			Config.SelectCharacterAttempts = 0;
			bool_1 = false;
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

	public async Task<bool> login_screen(Logic logic)
	{
		if (LokiPoe.IsInLoginScreen)
		{
			bool_2 = true;
			if (stopwatch_0 != null)
			{
				GlobalLog.Debug($"[AutoLoginEx] Forced wait is enabled, waiting 15minutes, time elapsed: {stopwatch_0.Elapsed:hh\\:mm\\:ss}");
				bool needToBreackToMule = false;
				Message i = Utility.BroadcastMessage((object)null, "Is_In_Mule_State", Array.Empty<object>());
				bool isMuling = default(bool);
				if (i.TryGetOutput<bool>(0, ref isMuling) && isMuling)
				{
					needToBreackToMule = true;
				}
				if (stopwatch_0.Elapsed.Minutes >= 15 || needToBreackToMule)
				{
					GlobalLog.Debug("[AutoLoginEx] time for login");
					stopwatch_0 = null;
					Utility.BroadcastMessage((object)null, "AL_ForceWait_Finished", new object[1] { "" });
				}
				return true;
			}
			if (!(DateTime.Now < Config.NextLoginTime))
			{
				if (!Config.DelayBeforeLoginAttempt)
				{
					int sleepTime = LokiPoe.Random.Next(100, 1500);
					GlobalLog.Debug($"[AutoLoginEx] The bot will wait {sleepTime}ms before logging in.");
					await Coroutine.Sleep(sleepTime);
				}
				else
				{
					GlobalLog.Debug($"[AutoLoginEx] DelayBeforeLoginAttempt | LoginAttemptDelay: {Config.LoginAttemptDelay}.");
					float modifier = (float)LokiPoe.Random.Next(100, 125) / 100f;
					int sleepTime2 = (int)((double)modifier * Config.LoginAttemptDelay.TotalMilliseconds);
					if (sleepTime2 < 1000)
					{
						sleepTime2 = 1000;
					}
					GlobalLog.Debug($"[AutoLoginEx] The bot will wait {TimeSpan.FromSeconds(sleepTime2)} before logging in.");
					await Coroutine.Sleep(sleepTime2);
				}
				if (LokiPoe.IsInLoginScreen)
				{
					if (Config.MaxLoginAttempts > 0 && Config.LoginAttempts >= Config.MaxLoginAttempts)
					{
						GlobalLog.Debug($"[AutoLoginEx] LoginAttempts ({Config.LoginAttempts}) >= MaxLoginAttempts ({Config.MaxLoginAttempts}).");
						BotManager.Stop(new StopReasonData("autologin_max_login_attempts", "", (object)null), false);
						return true;
					}
					if (PreGameState.IsMessageBoxActive)
					{
						string title2 = PreGameState.MessageBoxTitle.ToLowerInvariant();
						string text2 = PreGameState.MessageBoxText.ToLowerInvariant();
						GlobalLog.Info("[AutoLoginEx][login_screen] " + title2 + ": " + text2);
						PreGameState.ConfirmMessageBox();
						if (text2.Contains("has ended"))
						{
							BotManager.Stop(false);
							return true;
						}
						if ((DateTime.Now - dateTime_0).TotalSeconds < 15.0 && Config.LoginAttempts > 0)
						{
							BotManager.Stop(false);
							return true;
						}
					}
					if (PreGameState.IsLeagueInformationWindowOpen)
					{
						GlobalLog.Info("[AutoLoginEx][login_screen] League Information window is open, closing it");
						Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
						await Coroutine.Sleep(500);
						return true;
					}
					GlobalLog.Debug($"[AutoLoginEx] Client Version: {LokiPoe.ClientVersion}");
					if ((int)LokiPoe.ClientVersion == 1)
					{
						string email = LoginState.LoginTextboxText;
						GatewayEnum gate = LoginState.GetGateway();
						LoginError err2;
						if (Config.LoginUsingUserCredentials)
						{
							if (!Config.LoginUsingGateway)
							{
								err2 = ((!string.IsNullOrEmpty(email) && email.EqualsIgnorecase(Config.Email) && !bool_0) ? LoginState.Login(true) : LoginState.Login(Config.Email, Config.Password));
							}
							else
							{
								if (gate != Config.Gate)
								{
									LoginState.SetGateway(Config.Gate);
									if ((int)Config.Gate == 0)
									{
										await Wait.SleepSafe(2000);
									}
								}
								err2 = ((!string.IsNullOrEmpty(email) && email.EqualsIgnorecase(Config.Email) && !bool_0) ? LoginState.Login(true) : LoginState.Login(Config.Email, Config.Password));
							}
						}
						else
						{
							if (Config.LoginUsingGateway && gate != Config.Gate)
							{
								LoginState.SetGateway(Config.Gate);
								if ((int)Config.Gate == 0)
								{
									await Wait.SleepSafe(2000);
								}
							}
							err2 = LoginState.Login(false);
						}
						GlobalLog.Debug($"[AutoLoginEx] Login errors: {err2}.");
						if ((int)err2 > 0)
						{
							if ((int)err2 == 4)
							{
								return true;
							}
							if ((int)err2 == 1 || (int)err2 == 2 || (int)err2 == 10 || (int)err2 == 3)
							{
								BotManager.Stop(false);
								return true;
							}
							if ((int)err2 == 9 || (int)err2 == 8 || (int)err2 == 6 || (int)err2 == 7)
							{
								Config.NextLoginTime = DateTime.Now + TimeSpan.FromMilliseconds(LokiPoe.Random.Next(3000, 15000));
								return true;
							}
						}
						Config.LoginAttempts++;
						while (LoginState.IsConnecting)
						{
							GlobalLog.Info("[AutoLoginEx] Connecting...");
							await Coroutine.Sleep(100);
						}
						List<string> logEntries = PreGameState.LogEntries;
						foreach (string entry in logEntries)
						{
							GlobalLog.Info("[AutoLoginEx] " + entry);
						}
						err2 = LoginState.Login(true);
						if ((int)err2 != 4)
						{
							if ((int)err2 == 5)
							{
								string text = PreGameState.MessageBoxText;
								string title = PreGameState.MessageBoxTitle;
								GlobalLog.Error("[AutoLoginEx] " + title + ": " + text);
								StopReasonData stopReason = new StopReasonData("autologin_error", title + ": " + text, (object)null);
								if (text.ContainsIgnorecase("account does not exist"))
								{
									BotManager.Stop(stopReason, false);
								}
								if (text.ContainsIgnorecase("password is not correct"))
								{
									if (bool_0)
									{
										BotManager.Stop(stopReason, false);
									}
									bool_0 = true;
								}
								if (!text.ContainsIgnorecase("banned"))
								{
									if (text.ContainsIgnorecase("maintenance"))
									{
										Utility.BroadcastMessage((object)null, "autologin_maintenance_event", new object[1] { text });
										BotManager.Stop(stopReason, false);
									}
									else if (text.ContainsIgnorecase("patch"))
									{
										Utility.BroadcastMessage((object)null, "autologin_patch_event", new object[1] { text });
										BotManager.Stop(stopReason, false);
									}
								}
								else
								{
									Utility.BroadcastMessage((object)null, "autologin_banned_event", new object[1] { text });
									BotManager.Stop(stopReason, false);
								}
							}
							else
							{
								if ((int)err2 == 3)
								{
									BotManager.Stop(new StopReasonData("TermsOfUsePresent", "Terms of use present", (object)null), false);
									return true;
								}
								if ((int)err2 == 2)
								{
									string msg = "[AutoLoginEx] Your account has been locked until you can provide an unlock code from your email.";
									GlobalLog.Error(msg);
									BotManager.Stop(new StopReasonData("account_locked", msg, (object)null), false);
									return true;
								}
							}
							Config.NextLoginTime = DateTime.Now + TimeSpan.FromMilliseconds(LokiPoe.Random.Next(1000, 15000));
							return true;
						}
						bool_0 = false;
						return true;
					}
					GlobalLog.Error($"[AutoLoginEx] Unknown client version: {LokiPoe.ClientVersion}.");
					BotManager.Stop(false);
					return true;
				}
				GlobalLog.Debug("[AutoLoginEx] !IsInLoginScreen");
				return true;
			}
			int val = (int)(Config.NextLoginTime - DateTime.Now).TotalMilliseconds;
			if (val < 0)
			{
				val = 1;
			}
			GlobalLog.Debug($"[AutoLoginEx] NextLoginTime: {Config.NextLoginTime}. Now waiting {val} ms to login again.");
			await Coroutine.Sleep(val);
			return true;
		}
		GlobalLog.Warn("!IsInLoginScreen");
		return false;
	}

	public async Task<bool> character_selection(Logic logic)
	{
		if (!LokiPoe.IsInCreateCharacterScreen)
		{
			if (!LokiPoe.IsInCharacterSelectionScreen)
			{
				GlobalLog.Warn("!IsInCharacterSelectionScreen");
				return false;
			}
			if (Config.AutoSelectCharacter)
			{
				bool_1 = true;
				if (Config.SelectCharacterAttempts <= 5)
				{
					if (Config.DelayBeforeSelectingCharacter)
					{
						GlobalLog.Debug($"[AutoLoginEx] DelayBeforeSelectingCharacter | SelectCharacterDelay: {Config.SelectCharacterDelay}.");
						float modifier = (float)LokiPoe.Random.Next(100, 125) / 100f;
						int sleepTime = (int)((double)modifier * Config.SelectCharacterDelay.TotalMilliseconds);
						if (sleepTime < 1000)
						{
							sleepTime = 1000;
						}
						GlobalLog.Debug($"[AutoLoginEx] The bot will wait {TimeSpan.FromMilliseconds(sleepTime)} before selecting a character.");
						await Coroutine.Sleep(sleepTime);
					}
					await Wait.For(() => LokiPoe.IsInCharacterSelectionScreen && SelectCharacterState.IsCharacterListLoaded, "characters to load", 100, 5000);
					if (!LokiPoe.IsInCharacterSelectionScreen)
					{
						GlobalLog.Debug("[AutoLoginEx] !IsInCharacterSelectionScreen");
						return true;
					}
					if (SelectCharacterState.IsCharacterListLoaded)
					{
						if (PreGameState.IsMessageBoxActive)
						{
							string title = PreGameState.MessageBoxTitle.ToLowerInvariant();
							string text = PreGameState.MessageBoxText.ToLowerInvariant();
							GlobalLog.Info("[AutoLoginEx][character_selection] " + title + ": " + text);
							PreGameState.ConfirmMessageBox();
							await Wait.SleepSafe(100);
							return true;
						}
						dateTime_0 = DateTime.Now;
						string loginCharName = Config.Character;
						if (string.IsNullOrEmpty(loginCharName))
						{
							loginCharName = Config.DummyCharacter;
						}
						if (!string.IsNullOrEmpty(loginCharName))
						{
							List<CharacterEntry> characters = SelectCharacterState.Characters.ToList();
							foreach (CharacterEntry c2 in from c in characters
								orderby c.League, c.Level descending
								select c)
							{
								string ascend = "Not ascended";
								AscendancyClass ascendancyClass = c2.AscendancyClass;
								if (((object)(AscendancyClass)(ref ascendancyClass)).ToString() != "None")
								{
									ascendancyClass = c2.AscendancyClass;
									ascend = ((object)(AscendancyClass)(ref ascendancyClass)).ToString();
								}
								if (!c2.Name.Equals(loginCharName))
								{
									GlobalLog.Info($"[{c2.Name}-{c2.League}] {c2.CharacterClass} [{ascend}] ({c2.Level})");
								}
								else
								{
									Config.LastLeague = c2.League;
									GlobalLog.Warn($"[{c2.Name}-{c2.League}] {c2.CharacterClass} [{ascend}] ({c2.Level})");
								}
							}
							SelectCharacterError err = SelectCharacterState.SelectCharacter(loginCharName);
							GlobalLog.Debug($"[AutoLoginEx] SelectCharacter errors: {err}.");
							if ((int)err <= 0)
							{
								Config.SelectCharacterAttempts++;
								await Coroutine.Sleep(100);
								int idx = 0;
								while (StateManager.IsWaitingStateActive)
								{
									GlobalLog.Debug("[AutoLoginEx] The WaitingState is active...");
									await Coroutine.Sleep(100);
									int num = idx + 1;
									idx = num;
									if (idx > 150)
									{
										break;
									}
								}
								return true;
							}
							if ((int)err != 3)
							{
								BotManager.Stop(new StopReasonData("select_char_error", ((object)(SelectCharacterError)(ref err)).ToString(), (object)null), false);
								return true;
							}
							return true;
						}
						List<CharacterEntry> allChars = SelectCharacterState.Characters.ToList();
						CharacterClass targetClass = (CharacterClass)199;
						int charactersAmount = allChars.Count;
						if (charactersAmount >= 5)
						{
							GlobalLog.Debug($"[AutoLoginEx] No need for more characters, this accounts already has: {charactersAmount}");
							characterClass_0 = Config.FinalClass;
							CharacterEntry cl = allChars.FirstOrDefault((CharacterEntry c) => c.CharacterClass == Config.FinalClass && ((!Config.CreateMainCharOnStandard && !c.League.Contains("Standard") && !c.League.Contains("Hardcore")) || (Config.CreateMainCharOnStandard && c.League == "Standard")));
							if (cl != null)
							{
								GlobalLog.Debug($"[AutoLoginEx] Found: {cl.Name} {cl.Level} {cl.CharacterClass} in {cl.League}");
								Config.Character = cl.Name;
								return true;
							}
							GlobalLog.Debug($"[AutoLoginEx] Need to create {Config.FinalClass}");
							string_0 = allChars.ElementAt(4).Name;
							GlobalLog.Debug("[AutoLoginEx] Clicking Create.");
							SelectCharacterState.PressCreateCharacter();
							return true;
						}
						bool allCompleted = true;
						List<string> list_0 = new List<string>();
						foreach (CharacterEntry characterEntry_0 in Config.CharacterSetup.Where((CharacterEntry e) => e.TargetLevel != 0))
						{
							GlobalLog.Debug($"[AutoLoginEx] Now looking for: lvl: <{characterEntry_0.TargetLevel}  {characterEntry_0.CharacterClass}");
							CharacterEntry character = allChars.FirstOrDefault((CharacterEntry c) => c.CharacterClass == characterEntry_0.CharacterClass && c.Level >= characterEntry_0.TargetLevel && !list_0.Contains(c.Name));
							if (character != null)
							{
								GlobalLog.Debug($"[AutoLoginEx] {characterEntry_0.CharacterClass} with level: {characterEntry_0.TargetLevel} is already completed: " + $"{character.Name} - {character.Level} {character.CharacterClass} - {character.League}");
								list_0.Add(character.Name);
								characterEntry_0.IsCompleted = true;
								continue;
							}
							CharacterEntry possibleNext = allChars.FirstOrDefault((CharacterEntry e) => e.CharacterClass == characterEntry_0.CharacterClass && e.Level < characterEntry_0.TargetLevel);
							if (possibleNext != null)
							{
								if (characterEntry_0.TargetLevel == 101)
								{
									GlobalLog.Debug($"[AutoLoginEx] Final char detected: {possibleNext.Name}. {possibleNext.Level}lvl {possibleNext.CharacterClass} in {possibleNext.League}");
									Config.Character = possibleNext.Name;
									return true;
								}
								GlobalLog.Debug("[AutoLoginEx] Possible next character, using it as current Dummy.");
								Config.DummyCharacter = possibleNext.Name;
								return true;
							}
							GlobalLog.Debug($"[AutoLoginEx] Need to create: {characterEntry_0.CharacterClass} and to {characterEntry_0.TargetLevel} level.");
							targetClass = (characterClass_0 = characterEntry_0.CharacterClass);
							allCompleted = false;
							break;
						}
						if (allCompleted)
						{
							string last = list_0.LastOrDefault();
							if (!string.IsNullOrEmpty(last))
							{
								GlobalLog.Debug("[AutoLoginEx] All done, our last character is: " + last);
								Config.Character = last;
								Config.DummyCharacter = "";
								return true;
							}
							GlobalLog.Error("[AutoLoginEx] Last is null");
							BotManager.Stop(false);
							return true;
						}
						if ((int)targetClass != 199)
						{
							GlobalLog.Debug("[AutoLoginEx] Clicking Create.");
							SelectCharacterState.PressCreateCharacter();
							return true;
						}
						GlobalLog.Error("[AutoLoginEx] Fatal error targetClass: " + ((object)(CharacterClass)(ref targetClass)).ToString());
						BotManager.Stop(false);
						return true;
					}
					GlobalLog.Debug("[AutoLoginEx] !IsCharacterListLoaded");
					return true;
				}
				GlobalLog.Debug("[AutoLoginEx] We have tried 5 times to select a character and was unsuccessful.");
				BotManager.Stop(new StopReasonData("select_character_fail", "We have tried 5 times to select a character and was unsuccessful.", (object)null), false);
				return true;
			}
			return false;
		}
		GlobalLog.Debug("[AutoLoginEx] inside character_selection, this should never be executed");
		return false;
	}

	public async Task<bool> character_creation(Logic logic)
	{
		if (LokiPoe.IsInCreateCharacterScreen)
		{
			if ((int)characterClass_0 != 199)
			{
				string charName = randomHelper_0.GetRandomName();
				if (!string.IsNullOrEmpty(string_0))
				{
					while (!PowerString.ComparePower(charName, string_0))
					{
						charName = randomHelper_0.GetRandomName();
					}
					GlobalLog.Debug("[AutoLoginEx] " + charName + " string is stronger.");
				}
				League league = (League)1;
				bool isDummyAccount = characterClass_0 != Config.FinalClass;
				if (isDummyAccount && Config.CreateDummiesOnStandard)
				{
					league = (League)0;
				}
				if (!isDummyAccount && Config.CreateMainCharOnStandard)
				{
					league = (League)0;
				}
				CharacterCreationResult res = await CaracterCreationState.CreateNonRaceCharacter(league, characterClass_0, charName);
				GlobalLog.Debug($"[AutoLoginEx] Creation result: {res}");
				if ((int)res == 0)
				{
					Config.DummyCharacter = charName;
					return true;
				}
				return true;
			}
			GlobalLog.Debug("[AutoLoginEx] None is selected, its probably first character.");
			CharacterEntry firstChar = Config.CharacterSetup.FirstOrDefault((CharacterEntry d) => d != null && (int)d.CharacterClass != 199);
			if (firstChar != null)
			{
				characterClass_0 = firstChar.CharacterClass;
				return true;
			}
			GlobalLog.Error("[AutoLoginEx] firstChar is null, do you miss CharacterSetup?");
			BotManager.Stop(false);
			return true;
		}
		GlobalLog.Debug("[AutoLoginEx] Not in character creation screen.");
		return false;
	}

	public void Disable()
	{
	}

	public void Stop()
	{
	}

	public void Deinitialize()
	{
	}

	static Class52()
	{
		randomHelper_0 = new RandomHelper();
		interval_0 = new Interval(500);
		interval_1 = new Interval(5000);
		Config = AutoLoginSettings.Instance;
	}
}
