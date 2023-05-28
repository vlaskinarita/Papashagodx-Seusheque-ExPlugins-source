using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions.CommonTasks;

public class ClearCursorTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static bool bool_0;

	private static readonly Interval interval_1;

	public string Name => "ClearCursorTask";

	public string Description => "This task places any item left on the cursor into the inventory.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (interval_1.Elapsed || bool_0)
		{
			await PoeNinjaTracker.Init();
			bool_0 = false;
		}
		Skill moveSkill = Enumerable.FirstOrDefault(SkillBarHud.Skills, (Skill s) => s.Name == "Move" && !s.IsOnSkillBar);
		if (!((RemoteMemoryObject)(object)moveSkill != (RemoteMemoryObject)null))
		{
			CursorItemModes mode = CursorItemOverlay.Mode;
			if ((int)mode != 3 && (int)mode != 2)
			{
				if ((int)mode != 0)
				{
					Item cursorItem = CursorItemOverlay.Item;
					if ((RemoteMemoryObject)(object)cursorItem == (RemoteMemoryObject)null)
					{
						GlobalLog.Error($"[ClearCursorTask] Unexpected error. Cursor mode = \"{mode}\", but there is no item under cursor.");
						return false;
					}
					bool flag = ((IAuthored)BotManager.Current).Name == "MapBotEx";
					bool flag2 = flag;
					if (flag2)
					{
						flag2 = await EquipItem(cursorItem);
					}
					if (!flag2)
					{
						GlobalLog.Error("[ClearCursorTask] \"" + cursorItem.Name + "\" is under cursor. Now going to place it into inventory.");
						if (await Inventories.OpenInventory())
						{
							int col = default(int);
							int row = default(int);
							if (!InventoryUi.InventoryControl_Main.Inventory.CanFitItem(CursorItemOverlay.ItemSize, ref col, ref row))
							{
								GlobalLog.Error("[ClearCursorTask] There is no space in main inventory. Now stopping the bot because it cannot continue.");
								BotManager.Stop(new StopReasonData("inventory_cant_fit", "There is no space in main inventory", (object)null), false);
								return true;
							}
							if (!(await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(col, row))))
							{
								ErrorManager.ReportError();
							}
							return true;
						}
						ErrorManager.ReportError();
						return true;
					}
					return true;
				}
				return false;
			}
			GlobalLog.Error("[ClearCursorTask] A virtual item is on the cursor. Now pressing Escape to clear it.");
			Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
			await Wait.LatencySleep();
			return true;
		}
		SetSlotResult err = SkillBarHud.SetSlot(8, moveSkill);
		await Coroutines.ReactionWait();
		await Coroutines.LatencyWait();
		GlobalLog.Warn($"No move skil on bar! Setting to 8 slot. Errors: {err}.");
		return true;
	}

	public void Tick()
	{
	}

	public void Stop()
	{
		NotificationEntry notificationEntry = Enumerable.FirstOrDefault(ExtensionsSettings.Instance.NotificationsList, (NotificationEntry n) => n.NotifType == NotificationType.BotStop);
		if (notificationEntry != null)
		{
			string text = null;
			if (BotManager.StopReason != null && !string.IsNullOrEmpty(BotManager.StopReason.Reason))
			{
				text = BotManager.StopReason.Reason;
			}
			DiscordNotifier.AddNotification(string.IsNullOrEmpty(text) ? "Bot stopped!" : ("Bot stopped! [" + text + "]"), notificationEntry.UseAddition);
		}
	}

	private static async Task<bool> EquipItem(Item cursorItem)
	{
		if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2682 }))
		{
			List<Item> invFlasks = QuickFlaskHud.InventoryControl.Inventory.Items;
			if (invFlasks.Count >= 5)
			{
				return false;
			}
			GlobalLog.Warn("[ClearCursorTask] Found a Flask in the cursor and a missing flask in equipment, trying to Equip it.");
			for (int i = 0; i < 5; i++)
			{
				int int_0 = i;
				if ((RemoteMemoryObject)(object)QuickFlaskHud.InventoryControl.Inventory.Items.FirstOrDefault((Item x) => x.LocationTopLeft.X == int_0) == (RemoteMemoryObject)null)
				{
					if (await Inventories.OpenInventory())
					{
						InventoryUi.InventoryControl_Flasks.PlaceCursorInto(int_0, 0, true, true);
						if (await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null, "for flask to disappear from cursor."))
						{
							return true;
						}
						GlobalLog.Warn("[ClearCursorTask] Failed to reequip flask.");
						return false;
					}
					ErrorManager.ReportError();
					return true;
				}
			}
		}
		if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2434 }))
		{
			InventoryControlWrapper wrapper7 = InventoryUi.InventoryControl_Chest;
			List<Item> inv8 = wrapper7.Inventory.Items;
			if (inv8.Count < 1)
			{
				GlobalLog.Warn("[ClearCursorTask] Found a BodyArmours in the cursor and a missing BodyArmours in equipment, trying to Equip it.");
				return await ActuallyEquip(wrapper7);
			}
			return false;
		}
		if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2358 }))
		{
			InventoryControlWrapper wrapper8 = InventoryUi.InventoryControl_Head;
			List<Item> inv11 = wrapper8.Inventory.Items;
			if (inv11.Count < 1)
			{
				GlobalLog.Warn("[ClearCursorTask] Found a Helmets in the cursor and a missing Helmets in equipment, trying to Equip it.");
				return await ActuallyEquip(wrapper8);
			}
			return false;
		}
		if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2618 }))
		{
			InventoryControlWrapper wrapper10 = InventoryUi.InventoryControl_Gloves;
			List<Item> inv13 = wrapper10.Inventory.Items;
			if (inv13.Count < 1)
			{
				GlobalLog.Warn("[ClearCursorTask] Found a Gloves in the cursor and a missing Gloves in equipment, trying to Equip it.");
				return await ActuallyEquip(wrapper10);
			}
			return false;
		}
		if (!cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2549 }))
		{
			if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2247 }))
			{
				InventoryControlWrapper wrapper11 = InventoryUi.InventoryControl_Belt;
				List<Item> inv12 = wrapper11.Inventory.Items;
				if (inv12.Count < 1)
				{
					GlobalLog.Warn("[ClearCursorTask] Found a Belts in the cursor and a missing Belts in equipment, trying to Equip it.");
					return await ActuallyEquip(wrapper11);
				}
				return false;
			}
			if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2151 }))
			{
				InventoryControlWrapper wrapper9 = InventoryUi.InventoryControl_LeftRing;
				List<Item> inv10 = wrapper9.Inventory.Items;
				string side2 = "Left";
				if (inv10.Count >= 1)
				{
					wrapper9 = InventoryUi.InventoryControl_RightRing;
					inv10 = wrapper9.Inventory.Items;
					side2 = "Right";
					if (inv10.Count >= 1)
					{
						return false;
					}
				}
				GlobalLog.Warn("[ClearCursorTask] Found a Ring in the cursor and a missing " + side2 + " Ring in equipment, trying to Equip it.");
				return await ActuallyEquip(wrapper9);
			}
			if (!cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2177 }))
			{
				if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2262 }))
				{
					InventoryControlWrapper wrapper6 = InventoryUi.InventoryControl_PrimaryOffHand;
					List<Item> inv7 = wrapper6.Inventory.Items;
					if (inv7.Count < 1)
					{
						Item mainHand2 = InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault();
						if (!((RemoteMemoryObject)(object)mainHand2 != (RemoteMemoryObject)null) || !mainHand2.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)1773 }))
						{
							GlobalLog.Warn("[ClearCursorTask] Found a Shields in the cursor and a missing Off_Hand [One_Hand in prim. slot] in equipment, trying to Equip it.");
							return await ActuallyEquip(wrapper6);
						}
						return false;
					}
					return false;
				}
				if (cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2735 }))
				{
					InventoryControlWrapper wrapper5 = InventoryUi.InventoryControl_PrimaryOffHand;
					List<Item> inv6 = wrapper5.Inventory.Items;
					if (inv6.Count < 1)
					{
						Item mainHand = InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault();
						if (!((RemoteMemoryObject)(object)mainHand != (RemoteMemoryObject)null) || mainHand.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2018 }))
						{
							GlobalLog.Warn("[ClearCursorTask] Found a Quivers in the cursor and a missing Off_Hand [Bow in prim. slot] in equipment, trying to Equip it.");
							return await ActuallyEquip(wrapper5);
						}
						return false;
					}
					return false;
				}
				if (!cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)1773 }))
				{
					if (!cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)1776 }))
					{
						return false;
					}
					InventoryControlWrapper wrapper4 = InventoryUi.InventoryControl_PrimaryMainHand;
					List<Item> inv5 = wrapper4.Inventory.Items;
					string side = "MainHand";
					if (inv5.Count >= 1 && !cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)1773 }))
					{
						side = "OffHand";
						wrapper4 = InventoryUi.InventoryControl_PrimaryOffHand;
						inv5 = wrapper4.Inventory.Items;
						if (inv5.Count >= 1)
						{
							return false;
						}
					}
					GlobalLog.Warn("[ClearCursorTask] Found a OneHandWeapons in the cursor and a missing " + side + " OneHandWeapons in equipment, trying to Equip it.");
					return await ActuallyEquip(wrapper4);
				}
				InventoryControlWrapper wrapper3 = InventoryUi.InventoryControl_PrimaryMainHand;
				List<Item> inv3 = wrapper3.Inventory.Items;
				if (inv3.Count < 1)
				{
					Item offHand = InventoryUi.InventoryControl_PrimaryOffHand.Inventory.Items.FirstOrDefault();
					bool quiver = false;
					if ((RemoteMemoryObject)(object)offHand != (RemoteMemoryObject)null && offHand.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2737 }))
					{
						quiver = true;
						if (!cursorItem.HasMetadataFlags((MetadataFlags[])(object)new MetadataFlags[1] { (MetadataFlags)2018 }))
						{
							return false;
						}
					}
					GlobalLog.Warn($"[ClearCursorTask] Found a TwoHandWeapons in the cursor and a missing PrimaryMainHand [Quiver?: {quiver}] in equipment, trying to Equip it.");
					return await ActuallyEquip(wrapper3);
				}
				return false;
			}
			InventoryControlWrapper wrapper2 = InventoryUi.InventoryControl_Neck;
			List<Item> inv2 = wrapper2.Inventory.Items;
			if (inv2.Count >= 1)
			{
				return false;
			}
			GlobalLog.Warn("[ClearCursorTask] Found a Amulet in the cursor and a missing Amulet in equipment, trying to Equip it.");
			return await ActuallyEquip(wrapper2);
		}
		InventoryControlWrapper wrapper = InventoryUi.InventoryControl_Boots;
		List<Item> inv = wrapper.Inventory.Items;
		if (inv.Count < 1)
		{
			GlobalLog.Warn("[ClearCursorTask] Found a Boots in the cursor and a missing Boots in equipment, trying to Equip it.");
			return await ActuallyEquip(wrapper);
		}
		return false;
	}

	private static async Task<bool> ActuallyEquip(InventoryControlWrapper wrapper)
	{
		if (await Inventories.OpenInventory())
		{
			wrapper.PlaceCursorInto(true, true);
			if (!(await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item == (RemoteMemoryObject)null, "for item to disappear from cursor.")))
			{
				GlobalLog.Warn("[ClearCursorTask] Failed to reequip.");
				return false;
			}
			return true;
		}
		ErrorManager.ReportError();
		return true;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	static ClearCursorTask()
	{
		interval_0 = new Interval(1750);
		bool_0 = true;
		interval_1 = new Interval(180000);
	}
}
