using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.ChaosRecipeEx;

public class SellRecipeTask : ErrorReporter, ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "SellRecipeTask";

	public string Description => "Task that sells items for chaos recipe";

	public string Author => "ExVault";

	public string Version => "1.0";

	public SellRecipeTask()
	{
		ErrorLimitMessage = "[SellRecipeTask] Too many errors. This task will be disabled until combat area change.";
	}

	public async Task<bool> Run()
	{
		if (!base.ErrorLimitReached)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (area.IsTown || area.IsHideoutArea)
			{
				if (ChaosRecipeEx.StashData.HasCompleteSet())
				{
					GlobalLog.Debug("[SellRecipeTask] Now going to take and sell a set of rare items for chaos recipe.");
					if (await Inventories.OpenStashTab(Settings.Instance.StashTab, Name))
					{
						ChaosRecipeEx.StashData.SyncWithStashTab();
						if (ChaosRecipeEx.StashData.HasCompleteSet())
						{
							RecipeData takenItemData = new RecipeData();
							int int_ = -1;
							Item lowestItem = Inventories.StashTabItems.OrderBy((Item i) => i.ItemLevel).FirstOrDefault((Item i) => RecipeData.IsItemForChaosRecipe(i, out int_));
							if ((RemoteMemoryObject)(object)lowestItem == (RemoteMemoryObject)null)
							{
								GlobalLog.Error("[SellRecipeTask] Unknown error. Fail to find actual item for chaos recipe.");
								ReportError();
								return true;
							}
							GlobalLog.Debug($"[SellRecipeTask] Now taking \"{lowestItem.Name}\" (iLvl: {lowestItem.ItemLevel})");
							if (!(await Inventories.FastMoveFromStashTab(lowestItem.LocationTopLeft)))
							{
								ReportError();
								return true;
							}
							takenItemData.IncreaseItemCount(int_);
							while (true)
							{
								int int_0 = -1;
								for (int j = 0; j < 8; j++)
								{
									if (takenItemData.GetItemCount(j) switch
									{
										1 => j == 7, 
										0 => true, 
										_ => false, 
									})
									{
										int_0 = j;
										break;
									}
								}
								for (int k = 0; k < 8; k++)
								{
									if (takenItemData.GetItemCount(k) switch
									{
										0 => true, 
										1 => k == 0, 
										_ => false, 
									})
									{
										int_0 = k;
										break;
									}
								}
								if (int_0 == -1)
								{
									break;
								}
								int itemType2;
								Item item = (from i in Inventories.StashTabItems
									where RecipeData.IsItemForChaosRecipe(i, out itemType2) && itemType2 == int_0
									orderby i.ItemLevel descending
									select i).FirstOrDefault();
								if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
								{
									GlobalLog.Debug($"[SellRecipeTask] Now taking \"{item.Name}\" (iLvl: {item.ItemLevel})");
									if (await Inventories.FastMoveFromStashTab(item.LocationTopLeft))
									{
										takenItemData.IncreaseItemCount(int_0);
										continue;
									}
									ReportError();
									return true;
								}
								GlobalLog.Error("[SellRecipeTask] Unknown error. Fail to find actual item for chaos recipe.");
								ReportError();
								return true;
							}
							await Wait.SleepSafe(300);
							ChaosRecipeEx.StashData.SyncWithStashTab();
							int itemType;
							List<Vector2i> recipeItems = (from i in Inventories.InventoryItems
								where RecipeData.IsItemForChaosRecipe(i, out itemType)
								select i.LocationTopLeft).ToList();
							if (recipeItems.Count == 0)
							{
								GlobalLog.Error("[SellRecipeTask] Unknown error. There are no items in player's inventory after taking them from stash.");
								ReportError();
								return true;
							}
							if (!(await TownNpcs.SellItems(recipeItems)))
							{
								ReportError();
							}
							return true;
						}
						GlobalLog.Error("[SellRecipeTask] Saved stash data does not match actual stash data.");
						await Coroutines.CloseBlockingWindows();
						return false;
					}
					ReportError();
					return true;
				}
				GlobalLog.Info("[SellRecipeTask] No chaos recipe set to sell.");
				return false;
			}
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "combat_area_changed_event")
		{
			ResetErrors();
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}
}
