using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public static class Helpers
{
	public static Npc LadyDialla => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[2]
	{
		(PoeObjectEnum)64,
		(PoeObjectEnum)17
	}).FirstOrDefault<Npc>();

	public static Monster ClosestActiveMob => ((IEnumerable)ObjectManager.Objects).Closest<Monster>((Func<Monster, bool>)((Monster m) => m.IsActive && !Blacklist.Contains(((NetworkObject)m).Id)));

	public static bool PlayerHasQuestItem(string metadata)
	{
		return Inventories.InventoryItems.Exists((Item i) => i.Class == "QuestItem" && i.Metadata.ContainsIgnorecase(metadata));
	}

	public static bool PlayerHasQuestItemAmount(string metadata, int amount)
	{
		return Inventories.InventoryItems.Count((Item item) => item.Class == "QuestItem" && item.Metadata.ContainsIgnorecase(metadata)) == amount;
	}

	public static async Task TalkTo(NetworkObject npc)
	{
		if (await npc.AsTownNpc().Talk())
		{
			await Coroutines.CloseBlockingWindows();
		}
		else
		{
			ErrorManager.ReportError();
		}
	}

	public static async Task MoveAndTakeLocalTransition(TgtPosition tgtPos)
	{
		if (tgtPos.IsFar)
		{
			tgtPos.Come();
			return;
		}
		AreaTransition transition = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>();
		if ((NetworkObject)(object)transition == (NetworkObject)null)
		{
			GlobalLog.Warn("[MoveAndTakeLocalTransition] There is no area transition near tgt position.");
			tgtPos.ProceedToNext();
		}
		else if ((int)transition.TransitionType != 1)
		{
			GlobalLog.Warn("[MoveAndTakeLocalTransition] Area transition is not local.");
			tgtPos.ProceedToNext();
		}
		else if (!(await PlayerAction.TakeTransition(transition)))
		{
			ErrorManager.ReportError();
		}
	}

	public static async Task MoveAndTakeLocalTransitionSpecial(TgtPosition tgtPos)
	{
		if (tgtPos.IsFar)
		{
			tgtPos.Come();
			return;
		}
		List<AreaTransition> transition = ((IEnumerable)ObjectManager.Objects).All<AreaTransition>();
		if (transition.Count <= 0)
		{
			GlobalLog.Warn("[MoveAndTakeLocalTransition] There is no area transition near tgt position.");
			tgtPos.ProceedToNext();
			return;
		}
		foreach (AreaTransition areaTransition in transition.OrderByDescending((AreaTransition t) => ((NetworkObject)t).Name == "Altar of Hunger"))
		{
			if ((int)areaTransition.TransitionType != 1)
			{
				GlobalLog.Warn("[MoveAndTakeLocalTransition] Area transition is not local.");
			}
			else if (await PlayerAction.TakeTransition(areaTransition))
			{
				return;
			}
		}
		GlobalLog.Warn("[MoveAndTakeLocalTransition] Area transition is not local.");
		tgtPos.ProceedToNext();
	}

	public static async Task MoveAndWait(WalkablePosition pos, string log = null, int distance = 20)
	{
		if (pos.Distance <= distance)
		{
			GlobalLog.Debug(log ?? ("[QBMoveAndWait] Waiting for " + pos.Name));
			await Wait.StuckDetectionSleep(200);
		}
		else
		{
			pos.Come();
		}
	}

	public static async Task MoveAndWait(NetworkObject obj, string log = null, int distance = 20)
	{
		Vector2i pos = obj.Position;
		Vector2i myPosition = LokiPoe.MyPosition;
		if (((Vector2i)(ref myPosition)).Distance(pos) > distance)
		{
			PlayerMoverManager.MoveTowards(pos, (object)null);
			return;
		}
		GlobalLog.Debug(log ?? ("[MoveAndWait] Waiting for " + obj.Name));
		await Wait.StuckDetectionSleep(200);
	}

	public static async Task MoveToBossOrAnyMob(Monster boss)
	{
		if (!boss.IsActive)
		{
			Monster mob = ((IEnumerable)ObjectManager.Objects).Closest<Monster>((Func<Monster, bool>)((Monster m) => m.IsActive && !Blacklist.Contains(((NetworkObject)m).Id)));
			if ((NetworkObject)(object)mob != (NetworkObject)null)
			{
				GlobalLog.Debug("\"" + ((NetworkObject)boss).Name + "\" is not targetable. Now going to the closest active monster.");
				if (!PlayerMoverManager.MoveTowards(((NetworkObject)mob).Position, (object)null))
				{
					GlobalLog.Error($"Fail to move towards \"{((NetworkObject)mob).Name}\" at {((NetworkObject)mob).Position}. Now blacklisting it.");
					Blacklist.Add(((NetworkObject)mob).Id, TimeSpan.FromSeconds(10.0), "fail to move to");
				}
				return;
			}
		}
		await MoveAndWait(((NetworkObject)(object)boss).WalkablePosition());
	}

	public static async Task<bool> OpenQuestChest(CachedObject chest)
	{
		if (!(chest == null))
		{
			WalkablePosition chestPos = chest.Position;
			if (chestPos.Distance <= 15)
			{
				Chest chest_0 = (Chest)chest.Object;
				string name = ((NetworkObject)chest_0).Name;
				if (!chest_0.IsOpened)
				{
					if (!((NetworkObject)chest_0).IsTargetable)
					{
						GlobalLog.Debug("Waiting for chest: " + name);
						await Wait.StuckDetectionSleep(200);
					}
					else if (!(await PlayerAction.Interact((NetworkObject)(object)chest_0, () => chest_0.Fresh<Chest>().IsOpened, name + " opening")))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				GlobalLog.Debug(name + " is opened. Waiting for quest progress.");
				await Wait.StuckDetectionSleep(500);
				return true;
			}
			chestPos.TryCome();
			return true;
		}
		return false;
	}

	public static async Task<bool> HandleQuestObject(CachedObject cachedObj)
	{
		if (!(cachedObj == null))
		{
			WalkablePosition objPos = cachedObj.Position;
			if (!objPos.IsFar && !objPos.IsFarByPath)
			{
				NetworkObject networkObject_0 = cachedObj.Object;
				string name = networkObject_0.Name;
				if (networkObject_0.IsTargetable)
				{
					if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, name + " interaction")))
					{
						ErrorManager.ReportError();
					}
				}
				else
				{
					GlobalLog.Debug(name + " is not targetable. Waiting for quest progress.");
					await Wait.StuckDetectionSleep(500);
				}
				return true;
			}
			objPos.Come();
			return true;
		}
		return false;
	}

	public static async Task Explore(string prioTransition = null)
	{
		if (!string.IsNullOrWhiteSpace(prioTransition))
		{
			CombatAreaCache.Current.Explorer.Settings.PriorityTransition = prioTransition;
		}
		if (!(await CombatAreaCache.Current.Explorer.Execute()))
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			QuestBotSettings settings = QuestBotSettings.Instance;
			GlobalLog.Error(area.Name + " is fully explored but quest goal was not achieved (" + settings.CurrentQuestName + " - " + settings.CurrentQuestState + ")");
			Travel.RequestNewInstance(area);
			if (!(await PlayerAction.TpToTown()))
			{
				ErrorManager.ReportError();
			}
		}
	}

	public static async Task<bool> TakeQuestReward(AreaInfo area, TownNpc npc, string dialog, string questId = null, string book = null, bool shouldLogOut = false)
	{
		if (area.IsCurrentArea)
		{
			if (book == null || !PlayerHasQuestItem(book))
			{
				int oldQuestState = QuestManager.GetState(questId);
				string reward = ((questId != null) ? QuestBotSettings.Instance.GetRewardForQuest(questId) : null);
				if (!(await npc.TakeReward(reward, dialog)))
				{
					ErrorManager.ReportError();
				}
				if (book != null && PlayerHasQuestItem(book) && !(await UseQuestItem(book)))
				{
					ErrorManager.ReportError();
				}
				if (QuestManager.GetState(questId) == oldQuestState)
				{
					shouldLogOut = true;
				}
				if (shouldLogOut)
				{
					GlobalLog.Debug($"QUEST STATE: {oldQuestState} DID NOT CHANGE AND WE HAVE TO LOGOUT for quest {questId}");
					EscapeState.LogoutToCharacterSelection();
					await Wait.Sleep(3500);
				}
			}
			else if (!(await UseQuestItem(book)))
			{
				ErrorManager.ReportError();
			}
			return false;
		}
		await Travel.To(area);
		return true;
	}

	public static async Task<bool> UseQuestReward(string book = null)
	{
		if (!(await UseQuestItem(book)))
		{
			ErrorManager.ReportError();
		}
		return false;
	}

	public static async Task<bool> TravelAndTalkTo(AreaInfo area, TownNpc npc)
	{
		if (area.IsCurrentArea)
		{
			await TalkTo(npc.NpcObject);
			return true;
		}
		await Travel.To(area);
		return true;
	}

	public static async Task<bool> UseQuestItem(string metadata)
	{
		Item item = Inventories.InventoryItems.Find((Item i) => i.Class == "QuestItem" && i.Metadata.ContainsIgnorecase(metadata));
		if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null))
		{
			Vector2i vector2i_0 = item.LocationTopLeft;
			int id = item.LocalId;
			string name = item.Name;
			GlobalLog.Debug("[UseQuestItem] Now going to use \"" + name + "\".");
			if (!(await Inventories.OpenInventory()))
			{
				ErrorManager.ReportError();
				return false;
			}
			UseItemResult err = InventoryUi.InventoryControl_Main.UseItem(id, true);
			if ((int)err > 0)
			{
				GlobalLog.Error($"[UseQuestItem] Fail to use \"{name}\". Error: \"{err}\".");
				return false;
			}
			if (await Wait.For(() => (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(vector2i_0) == (RemoteMemoryObject)null, "quest item despawn", 100, 2000))
			{
				if ((RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null)
				{
					GlobalLog.Error("[UseQuestItem] Error. \"" + name + "\" has been picked to cursor.");
					return false;
				}
				await Wait.SleepSafe(500);
				await Coroutines.CloseBlockingWindows();
				return true;
			}
			return false;
		}
		GlobalLog.Error("[UseQuestItem] Fail to find item with metadata \"" + metadata + "\" in inventory.");
		return false;
	}
}
