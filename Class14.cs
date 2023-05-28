using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;
using ExPlugins.TabSetuper;
using ExPlugins.TraderPlugin;

internal class Class14 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly TabSetuperSettings tabSetuperSettings_0 = TabSetuperSettings.Instance;

	private static bool bool_0;

	public JsonSettings Settings => (JsonSettings)(object)TabSetuperSettings.Instance;

	public string Name => "SetupTabsTask";

	public string Description => "Д bI О";

	public string Author => "hehelmaoroflxd";

	public string Version => "13.37";

	public async Task<bool> Run()
	{
		if (!bool_0)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (area.IsTown || area.IsHideoutArea)
			{
				if (!World.CurrentArea.IsMap && !StashUi.IsOpened)
				{
					await Inventories.OpenStash();
				}
				List<string> list_0 = new List<string>();
				string currTab = "";
				string pubTab = "";
				foreach (string tab3 in StashUi.TabControl.TabNames)
				{
					await Inventories.OpenStashTab(tab3, Name);
					list_0.Add(tab3);
					if (StashUi.StashTabInfo.IsPremiumCurrency)
					{
						GlobalLog.Debug("[SetupTabsTask] Currency tab: " + tab3);
						currTab = tab3;
					}
					else if (StashUi.StashTabInfo.IsPublicFlagged && !StashUi.StashTabInfo.IsPremiumSpecial)
					{
						GlobalLog.Debug("[SetupTabsTask] Public tab: " + tab3);
						pubTab = tab3;
					}
				}
				if (!string.IsNullOrEmpty(pubTab) && tabSetuperSettings_0.ShouldSetupTradeTab)
				{
					TraderPluginSettings marketSellerSettings = TraderPluginSettings.Instance;
					marketSellerSettings.StashTabToTrade = pubTab;
				}
				ExtensionsSettings settings = ExtensionsSettings.Instance;
				if (settings == null)
				{
					GlobalLog.Debug("[SetupTabsTask] You dont have EXtenstions! Abort");
					bool_0 = true;
					return false;
				}
				List<ExtensionsSettings.StashingRule> rules = settings.GeneralStashingRules.ToList();
				IContent extensions = ContentManager.Contents.FirstOrDefault((IContent x) => ((IAuthored)x).Name == "EXtensions");
				foreach (ExtensionsSettings.StashingRule extRule in settings.GeneralStashingRules)
				{
					List<string> tabList = new List<string>();
					Parse(tabList, extRule.Tabs);
					foreach (string tab2 in tabList.Where((string tab) => !list_0.Contains(tab)))
					{
						if (!(extRule.Name == "Currency"))
						{
							switch (tab2)
							{
							case "4":
							{
								rules.Remove(extRule);
								ExtensionsSettings.StashingRule newRule2 = new ExtensionsSettings.StashingRule(extRule.Name, tabSetuperSettings_0.ReplaceTab4With);
								rules.Add(newRule2);
								break;
							}
							case "3":
							{
								rules.Remove(extRule);
								ExtensionsSettings.StashingRule newRule3 = new ExtensionsSettings.StashingRule(extRule.Name, tabSetuperSettings_0.ReplaceTab3With);
								rules.Add(newRule3);
								break;
							}
							case "2":
							{
								rules.Remove(extRule);
								ExtensionsSettings.StashingRule newRule4 = new ExtensionsSettings.StashingRule(extRule.Name, tabSetuperSettings_0.ReplaceTab2With);
								rules.Add(newRule4);
								break;
							}
							case "1":
							{
								rules.Remove(extRule);
								ExtensionsSettings.StashingRule newRule5 = new ExtensionsSettings.StashingRule(extRule.Name, tabSetuperSettings_0.ReplaceTab1With);
								rules.Add(newRule5);
								break;
							}
							}
							continue;
						}
						rules.Remove(extRule);
						if (string.IsNullOrEmpty(currTab) && tabSetuperSettings_0.ShouldSetupCurrencyTab)
						{
							if (list_0.Contains("3"))
							{
								currTab = "3";
							}
							else if (list_0.Contains("2"))
							{
								currTab = "2";
							}
							else if (list_0.Contains("1"))
							{
								currTab = "1";
							}
							else if (!list_0.Contains("4"))
							{
								GlobalLog.Error("[TabSetuper] You dont have tabs 1-4, now stopping the bot because it cant setup tabs");
								BotManager.Stop(new StopReasonData("no_tabs", "You dont have tabs 1-4, now stopping the bot because it cant setup tabs", (object)null), false);
							}
							else
							{
								currTab = "4";
							}
						}
						ExtensionsSettings.StashingRule newRule = new ExtensionsSettings.StashingRule(extRule.Name, currTab);
						rules.Add(newRule);
					}
				}
				if (extensions != null)
				{
					((IConfigurable)extensions).Settings.SetProperty("GeneralStashingRules", (object)rules);
				}
				bool_0 = true;
				return false;
			}
			return false;
		}
		return false;
	}

	private static void Parse(ICollection<string> list, string str)
	{
		list.Clear();
		string[] array = str.Split(',');
		string[] array2 = array;
		int num = 0;
		while (true)
		{
			if (num < array2.Length)
			{
				string text = array2[num];
				string text2 = text.Trim();
				if (text2 == string.Empty)
				{
					break;
				}
				list.Add(text2);
				num++;
				continue;
			}
			return;
		}
		throw new Exception("Remove double commas and/or commas from the start/end of the string.");
	}

	public void Initialize()
	{
	}

	public void Deinitialize()
	{
	}

	public void Enable()
	{
		GlobalLog.Warn("[TabSetuper] Enabled");
	}

	public void Disable()
	{
		GlobalLog.Warn("[TabSetuper] Disabled");
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}
}
