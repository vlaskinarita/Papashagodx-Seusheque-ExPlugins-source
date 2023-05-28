using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.BasicGemLeveler;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

internal class Class54 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "BasicGemLeveler";

	public string Description => "A plugin to automatically level skill gems.";

	public string Author => "Bossland GmbH modded by Alcor75";

	public string Version => "0.3.1.6";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (!InstanceInfo.IsGamePaused)
			{
				if (StateManager.IsEscapeStateActive)
				{
					return false;
				}
				if (((Actor)LokiPoe.Me).IsDead)
				{
					return false;
				}
				if (SkillsUi.IsOpened)
				{
					return false;
				}
				if (BasicGemLeveler.NeedsToUpdate || BasicGemLeveler.LevelWait.IsFinished)
				{
					if (RitualFavorsUi.IsOpened)
					{
						return false;
					}
					if (SkillGemHud.AreIconsDisplayed && !InventoryUi.IsOpened)
					{
						await Coroutines.CloseBlockingWindows();
						await Coroutines.FinishCurrentAction(true);
						await Coroutines.LatencyWait();
						HandlePendingLevelUpResult res2 = SkillGemHud.HandlePendingLevelUps((Func<Inventory, Item, Item, bool>)delegate(Inventory inv, Item holder, Item gem)
						{
							//IL_0055: Unknown result type (might be due to invalid IL or missing references)
							//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
							if (BasicGemLeveler.ContainsHelper(gem.Name, gem.SkillGemLevel))
							{
								if (BasicGemLevelerSettings.Instance.DebugStatements)
								{
									GlobalLog.Debug($"[LevelSkillGemTask] {gem.Name}[Lev: {gem.SkillGemLevel}]");
								}
								return false;
							}
							string value2 = $"{gem.Name} [{inv.PageSlot}: {holder.GetSocketIndexOfGem(gem)}]";
							ObservableCollection<BasicGemLevelerSettings.SkillGemEntry> userSkillGems2 = BasicGemLevelerSettings.Instance.UserSkillGems;
							ObservableCollection<string> observableCollection2 = new ObservableCollection<string>();
							if (BasicGemLevelerSettings.Instance.AutoLevel)
							{
								foreach (BasicGemLevelerSettings.SkillGemEntry item in userSkillGems2)
								{
									observableCollection2.Add($"{item.Name} [{item.InventorySlot}: {item.SocketIndex}]");
								}
							}
							else
							{
								observableCollection2 = BasicGemLevelerSettings.Instance.SkillGemsToLevelList;
							}
							foreach (string item2 in observableCollection2)
							{
								if (item2.Equals(value2, StringComparison.OrdinalIgnoreCase))
								{
									if (BasicGemLevelerSettings.Instance.DebugStatements)
									{
										GlobalLog.Debug("[LevelSkillGemTask] " + gem.Name + " => " + item2 + ".");
									}
									return true;
								}
							}
							return false;
						}, true);
						StuckDetection.Reset();
						GlobalLog.Info($"[LevelSkillGemTask] SkillGemHud.HandlePendingLevelUps returned {res2}.");
						if ((int)res2 == 4 || (int)res2 == 5)
						{
							await Coroutines.LatencyWait();
							return false;
						}
					}
					if (BasicGemLeveler.NeedsToUpdate || InventoryUi.IsOpened)
					{
						if (!(await BasicGemLeveler.OpenInventoryPanel()))
						{
							GlobalLog.Error("[LevelSkillGemTask] OpenInventoryPanel failed.");
							return true;
						}
						if (InventoryUi.AreIconsDisplayed)
						{
							HandlePendingLevelUpResult res = InventoryUi.HandlePendingLevelUps((Func<Inventory, Item, Item, bool>)delegate(Inventory inv, Item holder, Item gem)
							{
								//IL_0055: Unknown result type (might be due to invalid IL or missing references)
								//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
								if (BasicGemLeveler.ContainsHelper(gem.Name, gem.SkillGemLevel))
								{
									if (BasicGemLevelerSettings.Instance.DebugStatements)
									{
										GlobalLog.Debug($"[LevelSkillGemTask] {gem.Name}[Lev: {gem.SkillGemLevel}]");
									}
									return false;
								}
								string value = $"{gem.Name} [{inv.PageSlot}: {holder.GetSocketIndexOfGem(gem)}]";
								ObservableCollection<BasicGemLevelerSettings.SkillGemEntry> userSkillGems = BasicGemLevelerSettings.Instance.UserSkillGems;
								ObservableCollection<string> observableCollection = new ObservableCollection<string>();
								if (BasicGemLevelerSettings.Instance.AutoLevel)
								{
									foreach (BasicGemLevelerSettings.SkillGemEntry item3 in userSkillGems)
									{
										observableCollection.Add($"{item3.Name} [{item3.InventorySlot}: {item3.SocketIndex}]");
									}
								}
								else
								{
									observableCollection = BasicGemLevelerSettings.Instance.SkillGemsToLevelList;
								}
								foreach (string item4 in observableCollection)
								{
									if (item4.Equals(value, StringComparison.OrdinalIgnoreCase))
									{
										if (BasicGemLevelerSettings.Instance.DebugStatements)
										{
											GlobalLog.Debug("[LevelSkillGemTask] " + gem.Name + " => " + item4 + ".");
										}
										return true;
									}
								}
								return false;
							}, true);
							StuckDetection.Reset();
							GlobalLog.Info($"[LevelSkillGemTask] InventoryUi.HandlePendingLevelUps returned {res}.");
							if ((int)res == 4 || (int)res == 5)
							{
								await Coroutines.LatencyWait();
								return true;
							}
						}
					}
					BasicGemLeveler.LevelWait.Reset(TimeSpan.FromMilliseconds(LokiPoe.Random.Next(5000, 10000)));
					await Coroutines.CloseBlockingWindows();
					BasicGemLeveler.NeedsToUpdate = false;
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
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

	[CompilerGenerated]
	internal static bool smethod_0(Inventory inv, Item holder, Item gem)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (BasicGemLeveler.ContainsHelper(gem.Name, gem.SkillGemLevel))
		{
			if (BasicGemLevelerSettings.Instance.DebugStatements)
			{
				GlobalLog.Debug($"[LevelSkillGemTask] {gem.Name}[Lev: {gem.SkillGemLevel}]");
			}
			return false;
		}
		string value = $"{gem.Name} [{inv.PageSlot}: {holder.GetSocketIndexOfGem(gem)}]";
		ObservableCollection<BasicGemLevelerSettings.SkillGemEntry> userSkillGems = BasicGemLevelerSettings.Instance.UserSkillGems;
		ObservableCollection<string> observableCollection = new ObservableCollection<string>();
		if (BasicGemLevelerSettings.Instance.AutoLevel)
		{
			foreach (BasicGemLevelerSettings.SkillGemEntry item in userSkillGems)
			{
				observableCollection.Add($"{item.Name} [{item.InventorySlot}: {item.SocketIndex}]");
			}
		}
		else
		{
			observableCollection = BasicGemLevelerSettings.Instance.SkillGemsToLevelList;
		}
		foreach (string item2 in observableCollection)
		{
			if (item2.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				if (BasicGemLevelerSettings.Instance.DebugStatements)
				{
					GlobalLog.Debug("[LevelSkillGemTask] " + gem.Name + " => " + item2 + ".");
				}
				return true;
			}
		}
		return false;
	}
}
