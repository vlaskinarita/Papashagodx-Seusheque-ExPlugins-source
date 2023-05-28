using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q1_EnemyAtTheGate
{
	private static string string_0;

	private static Monster Hillock => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)630 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedHillockPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["HillockPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["HillockPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act1.TwilightStrand.IsCurrentArea)
		{
			Monster hillock = Hillock;
			if ((NetworkObject)(object)hillock != (NetworkObject)null)
			{
				CachedHillockPos = ((!((Actor)hillock).IsDead) ? ((NetworkObject)(object)hillock).WalkablePosition() : null);
			}
		}
	}

	public static async Task<bool> EnterLioneyeWatch()
	{
		if (World.Act1.TwilightStrand.IsCurrentArea)
		{
			if (!InGameState.IsSkipAllTutorialsButtonShowing)
			{
				Skill moveSkill = SkillBarHud.Skills.FirstOrDefault((Skill s) => s.Name == "Move" && !s.IsOnSkillBar);
				if (!((RemoteMemoryObject)(object)moveSkill != (RemoteMemoryObject)null))
				{
					Skill tutorialSkill = SkillBarHud.Skills.FirstOrDefault((Skill s) => s.Cost > 0);
					if ((RemoteMemoryObject)(object)tutorialSkill != (RemoteMemoryObject)null)
					{
						string_0 = tutorialSkill.Name;
						Skill newSlot = SkillBarHud.Slot(3);
						if ((RemoteMemoryObject)(object)newSlot == (RemoteMemoryObject)null || newSlot.Name != string_0)
						{
							SetSlotResult setSlotResult = SkillBarHud.SetSlot(3, tutorialSkill);
							GlobalLog.Debug($"[A1_Q1_EnemyAtTheGate] setSlot error: {setSlotResult}");
							await Coroutines.ReactionWait();
							await Coroutines.LatencyWait();
							return true;
						}
					}
					Item equippedMainHand = InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault();
					if (!((RemoteMemoryObject)(object)equippedMainHand == (RemoteMemoryObject)null))
					{
						Npc npc = ObjectManager.GetObjectsByType<Npc>().FirstOrDefault((Npc n) => ((NetworkObject)n).HasNpcFloatingIcon);
						if (!((NetworkObject)(object)npc != (NetworkObject)null))
						{
							WorldItem closestItemOnTheGround = ObjectManager.GetObjectsByType<WorldItem>().FirstOrDefault((WorldItem i) => (int)i.Item.Rarity == 4);
							if ((NetworkObject)(object)closestItemOnTheGround != (NetworkObject)null)
							{
								Item item = closestItemOnTheGround.Item;
								if ((RemoteMemoryObject)(object)item != (RemoteMemoryObject)null && !IsAlreadySocketed(equippedMainHand, ((NetworkObject)closestItemOnTheGround).Name))
								{
									await PlayerAction.Interact((NetworkObject)(object)closestItemOnTheGround);
									return true;
								}
							}
							Item invGem = Inventories.InventoryItems.Find((Item i) => (int)i.Rarity == 4);
							if (!((RemoteMemoryObject)(object)invGem != (RemoteMemoryObject)null) || IsAlreadySocketed(equippedMainHand, invGem.Name))
							{
								WalkablePosition hillockPos = CachedHillockPos;
								if (!(hillockPos != null))
								{
									await Travel.To(World.Act1.LioneyeWatch);
								}
								else
								{
									await Helpers.MoveAndWait(hillockPos);
								}
								return true;
							}
							GlobalLog.Debug("[A1_Q1_EnemyAtTheGate] gem detected in inventory");
							int ind = 0;
							Item equipped = LokiPoe.Me.EquippedItems.FirstOrDefault((Item x) => (int)x.ItemType == 2);
							if ((RemoteMemoryObject)(object)equipped != (RemoteMemoryObject)null && equipped.SocketedGems.Any((Item g) => (RemoteMemoryObject)(object)g != (RemoteMemoryObject)null))
							{
								ind = equipped.SocketedGems.Count((Item g) => (RemoteMemoryObject)(object)g != (RemoteMemoryObject)null);
							}
							await EquipGem(invGem, ind);
							return true;
						}
						await PlayerAction.Interact((NetworkObject)(object)npc);
						await Wait.SleepSafe(LokiPoe.Random.Next(100, 500));
						await Coroutines.CloseBlockingWindows();
						return true;
					}
					Item inventoryWeapon = InventoryUi.InventoryControl_Main.Inventory.Items.FirstOrDefault((Item i) => (int)i.ItemType == 2);
					if (!((RemoteMemoryObject)(object)inventoryWeapon != (RemoteMemoryObject)null))
					{
						WorldItem itemOnGround = ObjectManager.GetObjectByType<WorldItem>();
						if ((NetworkObject)(object)itemOnGround == (NetworkObject)null)
						{
							GlobalLog.Error("[A1_Q1_EnemyAtTheGate] no weapon found on the ground");
							BotManager.Stop(new StopReasonData("no_weapon", "[A1_Q1_EnemyAtTheGate] no weapon found on the ground", (object)null), false);
							return true;
						}
						await PlayerAction.Interact((NetworkObject)(object)itemOnGround);
						return true;
					}
					await Coroutines.CloseBlockingWindows();
					await Inventories.OpenInventory();
					await InventoryUi.InventoryControl_Main.PickItemToCursor(inventoryWeapon.LocationTopLeft);
					PlaceCursorIntoResult res2 = InventoryUi.InventoryControl_PrimaryMainHand.PlaceCursorInto(true, true);
					if ((int)res2 > 0)
					{
						GlobalLog.Error($"[A1_Q1_EnemyAtTheGate] PlaceCursorInto error: {res2}");
						return false;
					}
					return await InventoryUi.InventoryControl_Main.PlaceItemFromCursor(inventoryWeapon.LocationTopLeft);
				}
				SkillBarHud.SetSlot(8, moveSkill);
				await Coroutines.ReactionWait();
				await Coroutines.LatencyWait();
				return true;
			}
			GlobalLog.Debug("[A1_Q1_EnemyAtTheGate] skip tutorial button is detected");
			bool res = InGameState.SkipAllTutorials(true);
			GlobalLog.Debug($"[A1_Q1_EnemyAtTheGate] skip tutorial result: {res}");
			return true;
		}
		return false;
	}

	private static bool IsAlreadySocketed(Item item, string name)
	{
		return item.SocketedGems.Where((Item g) => (RemoteMemoryObject)(object)g != (RemoteMemoryObject)null).Any((Item g) => g.FullName.Equals(name));
	}

	public static async Task<bool> EquipGem(Item item, int index)
	{
		await Inventories.OpenInventory();
		Item itemToHostGem = LokiPoe.Me.EquippedItems.FirstOrDefault((Item x) => (int)x.ItemType == 2);
		if (!((RemoteMemoryObject)(object)itemToHostGem != (RemoteMemoryObject)null))
		{
			return false;
		}
		await InventoryUi.InventoryControl_Main.PickItemToCursor(item.LocationTopLeft);
		await Inventories.WaitForCursorToHaveItem();
		InventoryUi.InventoryControl_PrimaryMainHand.EquipSkillGem(index, true);
		await Inventories.WaitForCursorToBeEmpty();
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Tarkleigh, "Hillock Reward", Quests.EnemyAtTheGate.Id);
	}
}
