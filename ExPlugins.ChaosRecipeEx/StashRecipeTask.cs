using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.ChaosRecipeEx;

public class StashRecipeTask : ErrorReporter, ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class48 : CachedItem
	{
		[CompilerGenerated]
		private readonly Vector2i vector2i_1;

		[CompilerGenerated]
		private readonly int int_4;

		public Vector2i Position
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return vector2i_1;
			}
		}

		public int ItemType
		{
			[CompilerGenerated]
			get
			{
				return int_4;
			}
		}

		public Class48(Item item, int itemType)
			: base(item)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			vector2i_1 = item.LocationTopLeft;
			int_4 = itemType;
		}
	}

	private static bool bool_1;

	private bool bool_2;

	private static bool AnyItemToStash
	{
		get
		{
			foreach (Item inventoryItem in Inventories.InventoryItems)
			{
				if (!ItemEvaluator.Match(inventoryItem, (EvaluationType)6) && RecipeData.IsItemForChaosRecipe(inventoryItem, out var itemType) && ChaosRecipeEx.StashData.GetItemCount(itemType) < Settings.Instance.GetMaxItemCount(itemType))
				{
					return true;
				}
			}
			return false;
		}
	}

	private static List<Class48> ItemsToStash
	{
		get
		{
			List<Class48> list = new List<Class48>();
			RecipeData recipeData = new RecipeData();
			foreach (Item inventoryItem in Inventories.InventoryItems)
			{
				if (!ItemEvaluator.Match(inventoryItem, (EvaluationType)6) && RecipeData.IsItemForChaosRecipe(inventoryItem, out var itemType) && ChaosRecipeEx.StashData.GetItemCount(itemType) + recipeData.GetItemCount(itemType) < Settings.Instance.GetMaxItemCount(itemType))
				{
					recipeData.IncreaseItemCount(itemType);
					list.Add(new Class48(inventoryItem, itemType));
				}
			}
			return list;
		}
	}

	public string Name => "StashRecipeTask";

	public string Description => "Task that stashes items for chaos recipe.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public StashRecipeTask()
	{
		ErrorLimitMessage = "[StashRecipeTask] Too many errors. This task will be disabled until combat area change.";
	}

	public async Task<bool> Run()
	{
		if (!bool_2 && !base.ErrorLimitReached)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsTown && !area.IsHideoutArea)
			{
				return false;
			}
			if (!bool_1)
			{
				if (!AnyItemToStash)
				{
					GlobalLog.Info("[StashRecipeTask] No items to stash for chaos recipe.");
					await Coroutines.CloseBlockingWindows();
					return false;
				}
				GlobalLog.Debug("[StashRecipeTask] Updating chaos recipe stash data before actually stashing the items.");
				if (!(await OpenRecipeTab()))
				{
					return true;
				}
				ChaosRecipeEx.StashData.SyncWithStashTab();
			}
			else
			{
				GlobalLog.Debug("[StashRecipeTask] Updating chaos recipe stash data (every Start)");
				if (!(await OpenRecipeTab()))
				{
					return true;
				}
				ChaosRecipeEx.StashData.SyncWithStashTab();
				bool_1 = false;
			}
			List<Class48> itemsToStash = ItemsToStash;
			if (itemsToStash.Count != 0)
			{
				GlobalLog.Info($"[StashRecipeTask] {itemsToStash.Count} items to stash for chaos recipe.");
				foreach (Class48 item in itemsToStash.OrderBy((Class48 i) => i.Position, Position.Comparer.Instance))
				{
					GlobalLog.Debug("[StashRecipeTask] Now stashing \"" + item.Name + "\" for chaos recipe.");
					Vector2i itemPos = item.Position;
					if (Inventories.StashTabCanFitItem(itemPos))
					{
						if (await Inventories.FastMoveFromInventory(itemPos))
						{
							ChaosRecipeEx.StashData.IncreaseItemCount(item.ItemType);
							GlobalLog.Info("[Events] Item stashed (" + item.FullName + ")");
							Utility.BroadcastMessage((object)this, "item_stashed_event", new object[1] { item });
							continue;
						}
						ReportError();
						return true;
					}
					GlobalLog.Error("[StashRecipeTask] Stash tab for chaos recipe is full and must be cleaned.");
					bool_2 = true;
					return true;
				}
				await Wait.SleepSafe(300);
				ChaosRecipeEx.StashData.SyncWithStashTab();
				ChaosRecipeEx.StashData.Log();
				return true;
			}
			GlobalLog.Info("[StashRecipeTask] No items to stash for chaos recipe.");
			await Coroutines.CloseBlockingWindows();
			return false;
		}
		return false;
	}

	public void Start()
	{
		if (Settings.Instance.AlwaysUpdateStashData)
		{
			bool_1 = true;
		}
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

	private async Task<bool> OpenRecipeTab()
	{
		if (!(await Inventories.OpenStashTab(Settings.Instance.StashTab, Name)))
		{
			ReportError();
			return false;
		}
		StashTabInfo tabInfo = StashUi.StashTabInfo;
		if (!tabInfo.IsPremiumSpecial)
		{
			return true;
		}
		GlobalLog.Error($"[StashRecipeTask] Invalid stash tab {tabInfo.DisplayName} type: {tabInfo.TabType}. This tab cannot be used for chaos recipe.");
		BotManager.Stop(new StopReasonData("wrong_tab_type", $"Invalid stash tab {tabInfo.DisplayName} type: {tabInfo.TabType}. This tab cannot be used for chaos recipe.", (object)null), false);
		return false;
	}

	public void Tick()
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
