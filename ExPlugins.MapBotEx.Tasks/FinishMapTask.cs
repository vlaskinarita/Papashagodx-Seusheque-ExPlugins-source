using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.ChaosRecipeEx;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class FinishMapTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private class Class35
	{
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private int int_2;

		[CompilerGenerated]
		private double double_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private Vector2i vector2i_0;

		public string FullName
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public int StackCount
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		public int LocalId
		{
			[CompilerGenerated]
			get
			{
				return int_1;
			}
			[CompilerGenerated]
			set
			{
				int_1 = value;
			}
		}

		public int TributesCost
		{
			[CompilerGenerated]
			get
			{
				return int_2;
			}
			[CompilerGenerated]
			set
			{
				int_2 = value;
			}
		}

		public double ChaosValue
		{
			[CompilerGenerated]
			get
			{
				return double_0;
			}
			[CompilerGenerated]
			set
			{
				double_0 = value;
			}
		}

		public string ItemClass
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public Vector2i Size
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return vector2i_0;
			}
			[CompilerGenerated]
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				vector2i_0 = value;
			}
		}
	}

	private static int int_0;

	private readonly bool bool_0 = ObjectManager.Objects.Any((Monster o) => ((NetworkObject)o).Metadata.Contains("MetamorphosisBoss") && o.IsAliveHostile);

	private string string_0;

	private static readonly Interval interval_0;

	private static int MaxPulses
	{
		get
		{
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			string name = currentArea.Name;
			if (!LocalData.MapMods.ContainsKey((StatTypeGGG)10342) && !currentArea.Id.Contains("Affliction"))
			{
				switch (name)
				{
				default:
					if (!(name == "Polaric Void"))
					{
						if (name == MapNames.JungleValley || name == MapNames.Mausoleum || name == MapNames.UndergroundSea || name == MapNames.Conservatory)
						{
							return 20;
						}
						if (!(name == MapNames.ArachnidNest) && !(name == MapNames.Lookout))
						{
							if (!(name == MapNames.Geode))
							{
								return 3;
							}
							return 1;
						}
						return 8;
					}
					goto case "Absence of Patience and Wisdom";
				case "Absence of Patience and Wisdom":
				case "Absence of Symmetry and Harmony":
				case "Seething Chyme":
					return 25;
				}
			}
			return 1;
		}
	}

	public string Name => "FinishMapTask";

	public string Description => "Task for leaving current map.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsMap)
		{
			if (area.Id.Contains("Affliction") && GeneralSettings.Instance.IsOnRun)
			{
				return true;
			}
			if (GeneralSettings.Instance.ReturnForChaosRecipe)
			{
				if (World.CurrentArea.Id == string_0)
				{
					GlobalLog.Debug("[FinishMapTask] Item evaluator refreshed.");
					ItemEvaluator.Refresh();
					string_0 = null;
					return true;
				}
				if (ShouldCombackForChaosRecipe())
				{
					GlobalLog.Debug("[FinishMapTask] Chaos recipe ready. Return needed.");
					string_0 = World.CurrentArea.Id;
					if (!(await PlayerAction.TpToTown()))
					{
						ErrorManager.ReportError();
						return true;
					}
				}
			}
			if (!bool_0)
			{
				if (!DeliriumUi.IsDeliriumActive)
				{
					int maxPulses = ((MapData.Current.FinalPulse > 0) ? MapData.Current.FinalPulse : MaxPulses);
					if (int_0 < maxPulses)
					{
						StuckDetection.Reset();
						if (interval_0.Elapsed)
						{
							int_0++;
							GlobalLog.Info($"[FinishMapTask] Final pulse {int_0}/{maxPulses}");
							return true;
						}
						return true;
					}
					int mobsLeft = InstanceInfo.MonstersRemaining;
					GlobalLog.Warn($"[FinishMapTask] Now leaving current map. Mobs left {mobsLeft}.");
					await PlayerAction.TpToTown();
					GeneralSettings.Instance.IsOnRun = false;
					Statistics.Instance.OnMapFinish();
					return true;
				}
				DeliriumUi.FinishDelirium();
				GlobalLog.Info("[FinishMapTask] Pressed end Delirium button, resetting pulse.");
				int_0 = 0;
				await Wait.SleepSafe(5000);
				return true;
			}
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "MB_new_map_entered_event")
		{
			int_0 = 0;
			return (MessageResult)0;
		}
		if (!(message.Id == "methamorph_engaged"))
		{
			return (MessageResult)1;
		}
		int_0 = 0;
		return (MessageResult)0;
	}

	public void Tick()
	{
	}

	private async Task<bool> TakeRitualReward()
	{
		int retryCounter = 0;
		while (!RitualFavorsUi.IsOpened)
		{
			if (retryCounter < 3)
			{
				ShowRitualRewardUi.OpenRitualRewardPannel();
				await Coroutines.ReactionWait();
				retryCounter++;
				continue;
			}
			GlobalLog.Info("[MapBotEx] Failed to open the Favor Reward Ui 3 time, abort.");
			await Coroutines.CloseBlockingWindows();
			return false;
		}
		Stopwatch sw = Stopwatch.StartNew();
		while ((RemoteMemoryObject)(object)RitualFavorsUi.InventoryControl == (RemoteMemoryObject)null)
		{
			if (sw.ElapsedMilliseconds > 5000L)
			{
				GlobalLog.Info("[MapBotEx] Ritual Reward Inventorywrapper is null after 5 seconds, abort.");
				await Coroutines.CloseBlockingWindows();
				return false;
			}
			await Coroutines.LatencyWait();
		}
		List<Class35> itemList2 = new List<Class35>();
		int cost = default(int);
		bool canAfford = default(bool);
		foreach (Item item_0 in RitualFavorsUi.InventoryControl.Inventory.Items.Where((Item i) => i.Class == "MapFragment" || i.Class == "Map" || (int)i.Rarity == 3 || i.Class == "StackableCurrency" || i.Class == "DivinationCard"))
		{
			RitualFavorsUi.InventoryControl.ViewItemsInInventory((ShouldViewItemDelegate)((Inventory inv, Item itm) => itm.LocalId.Equals(item_0.LocalId)), (Func<bool>)(() => RitualFavorsUi.IsOpened));
			RitualFavorsUi.InventoryControl.GetItemTributeCostEx(item_0.LocalId, ref cost, ref canAfford);
			if (!canAfford)
			{
				continue;
			}
			Class35 reward3 = new Class35
			{
				FullName = item_0.FullName,
				ItemClass = item_0.Class,
				StackCount = item_0.StackCount,
				LocalId = item_0.LocalId,
				Size = item_0.Size,
				TributesCost = cost
			};
			Message msg = new Message("RequestPoeNinjaItemPrice", (object)this, new object[1] { item_0 });
			IPlugin plugin = PluginManager.EnabledPlugins.FirstOrDefault((IPlugin x) => ((IAuthored)x).Name == "PoeNinjaTracker");
			if (plugin == null)
			{
				GlobalLog.Error("[MapBotEx] PoeNinja Not Enabled.");
				reward3.ChaosValue = 0.0;
			}
			else
			{
				MessageResult res = ((IMessageHandler)plugin).Message(msg);
				if ((int)res != 0)
				{
					GlobalLog.Error("[MapBotEx] PoeNinja failed to answer.");
					reward3.ChaosValue = 0.0;
				}
				else
				{
					reward3.ChaosValue = (double)reward3.StackCount * msg.GetOutput<double>(0);
					GlobalLog.Warn($"[MapBotEx] PoeNinja answered: {reward3.FullName} {reward3.ChaosValue} chaos.");
				}
			}
			itemList2.Add(reward3);
		}
		int availableTributes = Ritual.Tributes;
		itemList2 = (from x in itemList2
			where x.ChaosValue >= 6.0 || x.ItemClass == "StackableCurrency"
			orderby x.ChaosValue
			select x).ToList();
		for (int j = itemList2.Count - 1; j >= 0; j--)
		{
			Class35 reward = itemList2[j];
			if (InventoryUi.InventoryControl_Main.Inventory.CanFitItem(reward.Size))
			{
				if (reward.TributesCost <= availableTributes)
				{
					GlobalLog.Warn($"[MapBotEx] Will buy {reward.FullName}, value: {reward.ChaosValue} chaos, cost: {reward.TributesCost} tributes.");
					availableTributes -= reward.TributesCost;
				}
				else
				{
					GlobalLog.Error($"[MapBotEx] Can't afford {reward.FullName}, value: {reward.ChaosValue} chaos, cost: {reward.TributesCost} tributes.");
					if (!(reward.ChaosValue >= 100.0))
					{
						itemList2.RemoveAt(j);
					}
					else
					{
						string errorName2 = $"[Ritual] Can't afford {reward.FullName}, value: {reward.ChaosValue} chaos, cost: {reward.TributesCost} tributes. Now stopping the bot.";
						GlobalLog.Error(errorName2);
						Utility.BroadcastMessage((object)null, "ritual_buy_fail", new object[1] { errorName2 });
						BotManager.Stop(new StopReasonData("ritual_buy_fail", errorName2, (object)null), false);
					}
				}
			}
			else
			{
				GlobalLog.Info($"[MapBotEx] No inventory space for {reward.FullName}, value: {reward.ChaosValue} chaos, cost: {reward.TributesCost} tributes.");
				if (!(reward.ChaosValue >= 100.0))
				{
					itemList2.RemoveAt(j);
				}
				else
				{
					string errorName = $"[Ritual] No inventory space for {reward.FullName}, value: {reward.ChaosValue} chaos, cost: {reward.TributesCost} tributes. Now stopping the bot.";
					GlobalLog.Error(errorName);
					Utility.BroadcastMessage((object)null, "ritual_buy_fail", new object[1] { errorName });
					BotManager.Stop(new StopReasonData("ritual_buy_fail", errorName, (object)null), false);
				}
			}
		}
		foreach (Class35 reward2 in itemList2)
		{
			int int_0 = Ritual.Tributes;
			GlobalLog.Info("[MapBotEx] Buying " + reward2.FullName + ".");
			RitualFavorsUi.InventoryControl.FastMove(reward2.LocalId, true, true);
			await Wait.For(() => Ritual.Tributes < int_0, "For Tributtes to decrease.");
		}
		await Coroutines.CloseBlockingWindows();
		return true;
	}

	private static bool ShouldCombackForChaosRecipe()
	{
		Item[] source = Inventories.InventoryItems.Where((Item i) => (int)i.Rarity == 2 && !i.IsIdentified).ToArray();
		if (!source.Any())
		{
			return false;
		}
		int num = ObjectManager.GetObjectsByType<WorldItem>().Count((WorldItem x) => !x.Item.IsIdentified && (int)x.Item.Rarity == 2);
		if (num >= 5)
		{
			return global::ExPlugins.ChaosRecipeEx.ChaosRecipeEx.StashData.HasCompleteSet();
		}
		return false;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static FinishMapTask()
	{
		interval_0 = new Interval(LokiPoe.Random.Next(750, 1250));
	}
}
