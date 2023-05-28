using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions;

public class TownNpc
{
	private readonly NetworkObject jaxyohVrcf;

	public readonly WalkablePosition Position;

	public NetworkObject NpcObject => jaxyohVrcf ?? ObjectManager.Objects.Find((NetworkObject o) => o is Npc && o.Name == Position.Name);

	public TownNpc(WalkablePosition position)
	{
		Position = position;
	}

	public TownNpc(NetworkObject obj)
	{
		jaxyohVrcf = obj;
		Position = obj.WalkablePosition();
	}

	public async Task<bool> Talk(bool ctrClick = false)
	{
		if (!NpcDialogUi.IsOpened && !RewardUi.IsOpened)
		{
			if (Position.Distance > 20 && ExtensionsSettings.Instance.HumanizerNew && LokiPoe.CurrentWorldArea.IsTown && Wait.NpcTalkPauseProbability(30))
			{
				WorldPosition pos = WorldPosition.FindRandomPositionForMove(Position, 5, 20);
				if (pos != null)
				{
					Move.Towards(pos, "To Random Location and pause.");
					await Wait.TownMoveRandomDelay();
				}
			}
			if (Position.Distance > 40)
			{
				await Position.TryComeAtOnce();
			}
			NetworkObject npcObj = NpcObject;
			if (npcObj == (NetworkObject)null)
			{
				GlobalLog.Error("[Talk] Fail to find NPC with name \"" + Position.Name + "\".");
				return false;
			}
			await Coroutines.CloseBlockingWindows();
			if (ctrClick)
			{
				ProcessHookManager.SetKeyState(Keys.ControlKey, short.MinValue, Keys.None);
				Input.HighlightObject(npcObj);
				MouseManager.ClickLMB(0, 0);
				await Coroutines.FinishCurrentAction(false);
				ProcessHookManager.ClearAllKeyStates();
				return true;
			}
			return await PlayerAction.Interact(npcObj, () => NpcDialogUi.IsOpened || RewardUi.IsOpened, "dialog panel opening");
		}
		return true;
	}

	public async Task<bool> OpenDialogPanel()
	{
		if (!(await Talk()))
		{
			return false;
		}
		if (RewardUi.IsOpened || NpcDialogUi.DialogDepth != 1)
		{
			int i = 1;
			while (true)
			{
				if (i <= 15)
				{
					GlobalLog.Debug($"[OpenDialogPanel] Pressing ESC to close the topmost NPC dialog ({i}/{15}).");
					Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
					await Wait.SleepSafe(LokiPoe.Random.Next(200, 400));
					if (!RewardUi.IsOpened && NpcDialogUi.DialogDepth == 1)
					{
						break;
					}
					int num = i + 1;
					i = num;
					continue;
				}
				GlobalLog.Error("[OpenDialogPanel] Fail to bring dialog panel to the top.");
				return false;
			}
			return true;
		}
		await Wait.Sleep(100);
		return true;
	}

	public async Task<bool> Converse(string partialDialogName)
	{
		if (!(await OpenDialogPanel()))
		{
			return false;
		}
		NpcDialogEntryWrapper obj = NpcDialogUi.DialogEntries.Find((NpcDialogEntryWrapper d) => d.Text.ContainsIgnorecase(partialDialogName));
		string dialog = ((obj != null) ? obj.Text : null);
		if (dialog == null)
		{
			GlobalLog.Error("[Converse] Fail to find any dialog with \"" + partialDialogName + "\" in it.");
			return false;
		}
		ConverseResult err = NpcDialogUi.Converse(dialog, true);
		if ((int)err > 0)
		{
			GlobalLog.Error($"[Converse] Fail to converse \"{dialog}\". Error: \"{err}\".");
			return false;
		}
		return true;
	}

	private async Task<bool> OpenRewardPanel(string partialDialogName = null)
	{
		if (partialDialogName == null)
		{
			partialDialogName = "reward";
		}
		if (!(await Converse(partialDialogName)))
		{
			return false;
		}
		if (!(await Wait.For(() => RewardUi.IsOpened, "reward panel opening")))
		{
			return false;
		}
		await Wait.Sleep(100);
		return true;
	}

	public async Task<bool> TakeReward(string reward = null, string dialogName = null)
	{
		GlobalLog.Debug("[TakeReward] Now going to take \"" + (reward ?? "Any") + "\" from " + Position.Name + ".");
		if (await OpenRewardPanel(dialogName))
		{
			InventoryControlWrapper rewardControl = null;
			List<InventoryControlWrapper> controls = RewardUi.InventoryControls;
			if (controls.Count != 0)
			{
				if (controls.Count != 1)
				{
					if (string.IsNullOrEmpty(reward) || reward.EqualsIgnorecase("any"))
					{
						rewardControl = controls[LokiPoe.Random.Next(0, controls.Count)];
					}
					else
					{
						foreach (InventoryControlWrapper control in controls)
						{
							Dictionary<Vector2i, Item>.ValueCollection item = control.InventorySlotUiElement.PlacementGraph.Values;
							foreach (Item it2 in item)
							{
								if (!((RemoteMemoryObject)(object)it2 == (RemoteMemoryObject)null) && (it2.FullName == reward || it2.Name == reward))
								{
									rewardControl = control;
									break;
								}
							}
						}
					}
				}
				else
				{
					rewardControl = controls[0];
				}
				if ((RemoteMemoryObject)(object)rewardControl == (RemoteMemoryObject)null)
				{
					GlobalLog.Error("[TakeReward] Fail to find reward control with \"" + reward + "\" item.");
					ErrorManager.ReportCriticalError();
					return false;
				}
				Item rewardItem = ((!string.IsNullOrEmpty(reward) && !reward.EqualsIgnorecase("any")) ? Enumerable.FirstOrDefault(rewardControl.Inventory.Items, (Item it) => it.FullName == reward || it.Name == reward) : rewardControl.Inventory.Items.ElementAt(LokiPoe.Random.Next(0, rewardControl.Inventory.Items.Count)));
				if (!((RemoteMemoryObject)(object)rewardItem == (RemoteMemoryObject)null))
				{
					reward = rewardItem.FullName;
					int int_0 = Inventories.InventoryItems.Count;
					FastMoveResult err = rewardControl.FastMove(rewardItem.LocalId, true, true);
					if ((int)err > 0)
					{
						GlobalLog.Error($"[TakeReward] Fail to take \"{reward}\" from {Position.Name}. Error: \"{err}\".");
						return false;
					}
					if (await Wait.For(() => Inventories.InventoryItems.Count == int_0 + 1, "quest reward appear in inventory"))
					{
						GlobalLog.Debug("[TakeReward] \"" + reward + "\" has been successfully taken from " + Position.Name + ".");
						await Wait.SleepSafe(LokiPoe.Random.Next(500, 800));
						await Coroutines.CloseBlockingWindows();
						return true;
					}
					return false;
				}
				GlobalLog.Error("[TakeReward] Fail to find item with name \"" + reward + "\".");
				ErrorManager.ReportCriticalError();
				return false;
			}
			GlobalLog.Error("[TakeReward] Unknown error. Reward panel is opened, but controls are not loaded.");
			ErrorManager.ReportCriticalError();
			return false;
		}
		return false;
	}

	public async Task<bool> OpenDefaultCtrlSell()
	{
		if (SellUi.IsOpened)
		{
			return true;
		}
		await Coroutines.CloseBlockingWindows();
		GlobalLog.Debug("[OpenDefaultCtrlSell] Now going to open sell dialog with " + Position.Name + ".");
		if (!(await Talk(ctrClick: true)))
		{
			return false;
		}
		if (!(await Wait.For(() => SellUi.IsOpened, "sell panel opening")))
		{
			return false;
		}
		GlobalLog.Debug("[OpenDefaultCtrlSell] Sell panel has been successfully opened.");
		await Wait.Sleep(100);
		return true;
	}

	public async Task<bool> OpenDefaultCtrlAnoint()
	{
		if (AnointingUi.IsOpened)
		{
			return true;
		}
		await Coroutines.CloseBlockingWindows();
		GlobalLog.Debug("[OpenDefaultCtrlAnoint] Now going to open anoint dialog with " + Position.Name + ".");
		if (!(await Talk(ctrClick: true)))
		{
			return false;
		}
		if (await Wait.For(() => AnointingUi.IsOpened, "anoint panel opening"))
		{
			GlobalLog.Debug("[OpenDefaultCtrlAnoint] Anoint panel has been successfully opened.");
			await Wait.Sleep(100);
			return true;
		}
		return false;
	}

	public async Task<bool> OpenDefaultCtrlPurchase()
	{
		if (PurchaseUi.IsOpened)
		{
			return true;
		}
		GlobalLog.Debug("[OpenDefaultCtrlPurchase] Now going to open purchase dialog with " + Position.Name + ".");
		if (!(await Talk(ctrClick: true)))
		{
			return false;
		}
		if (await Wait.For(() => PurchaseUi.IsOpened, "purchase panel opening"))
		{
			GlobalLog.Debug("[OpenDefaultCtrlPurchase] Purchase panel has been successfully opened.");
			await Wait.Sleep(100);
			return true;
		}
		return false;
	}

	public async Task<bool> OpenSellPanel()
	{
		if (!SellUi.IsOpened)
		{
			GlobalLog.Debug("[OpenSellPanel] Now going to open sell dialog with " + Position.Name + ".");
			if (!(await Converse("Sell Items")))
			{
				return false;
			}
			if (await Wait.For(() => SellUi.IsOpened, "sell panel opening"))
			{
				GlobalLog.Debug("[OpenSellPanel] Sell panel has been successfully opened.");
				await Wait.Sleep(100);
				return true;
			}
			return false;
		}
		return true;
	}

	public async Task<bool> OpenPurchasePanel()
	{
		if (!PurchaseUi.IsOpened)
		{
			GlobalLog.Debug("[OpenPurchasePanel] Now going to open purchase dialog with " + Position.Name + ".");
			if (!(await Converse("Purchase Items")))
			{
				return false;
			}
			if (!(await Wait.For(() => PurchaseUi.IsOpened, "purchase panel opening")))
			{
				return false;
			}
			GlobalLog.Debug("[OpenPurchasePanel] Purchase panel has been successfully opened.");
			await Wait.Sleep(100);
			return true;
		}
		return true;
	}
}
