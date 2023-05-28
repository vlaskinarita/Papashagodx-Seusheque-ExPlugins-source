using System.Linq;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EquipPluginEx.Helpers;
using ExPlugins.EXtensions;

namespace ExPlugins.EquipPluginEx.Classes;

public static class EquipPluginExEvaluator
{
	private static readonly Class27 Config;

	public static bool Evaluator(Item item)
	{
		return ((Player)LokiPoe.Me).Level < Class27.Instance.LevelToStopPickUpAndEquip && ShouldPickup(item);
	}

	private static bool ShouldPickup(Item item)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Invalid comparison between Unknown and I4
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Invalid comparison between Unknown and I4
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Invalid comparison between Unknown and I4
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Invalid comparison between Unknown and I4
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Invalid comparison between Unknown and I4
		if (!HasStats(item))
		{
			if (Class27.Instance.DebugMode)
			{
				GlobalLog.Debug("[EquipPluginEx] Stat requierments are not met for " + item.Name + ", skipping pickup");
			}
			return false;
		}
		if ((int)item.ItemType == 2 && Class27.Instance.ShouldEquipMainHand)
		{
			if (!CheckIfMainHandTypeEnabled(item))
			{
				return false;
			}
			Item val = InventoryUi.InventoryControl_PrimaryMainHand.Inventory.Items.FirstOrDefault();
			return (RemoteMemoryObject)(object)val == (RemoteMemoryObject)null || CanBeAnUpgrade(val, item);
		}
		if (((int)item.ItemType == 3 || (int)item.ItemType == 2) && Class27.Instance.ShouldEquipOffHand)
		{
			if (CheckIfOffHandTypeEnabled(item))
			{
				Item val2 = InventoryUi.InventoryControl_PrimaryOffHand.Inventory.Items.FirstOrDefault();
				return (RemoteMemoryObject)(object)val2 == (RemoteMemoryObject)null || CanBeAnUpgrade(val2, item);
			}
			return false;
		}
		if ((int)item.ItemType != 4 || !Class27.Instance.ShouldEquipHelmet)
		{
			if ((int)item.ItemType != 1 || !Class27.Instance.ShouldEquipBodyArmour)
			{
				if ((int)item.ItemType != 7 || !Class27.Instance.ShouldEquipGloves)
				{
					if ((int)item.ItemType != 8 || !Class27.Instance.ShouldEquipBoots)
					{
						if ((int)item.ItemType != 9 || !Class27.Instance.ShouldEquipBelt)
						{
							if ((int)item.ItemType != 5 || !Class27.Instance.ShouldEquipAmulet)
							{
								if ((int)item.ItemType != 6 || !Class27.Instance.ShouldEquipRings)
								{
									if ((int)item.ItemType == 10 && Class27.Instance.ShouldEquipFlasks)
									{
										return GetFlaskSlotToUpgrade(item) >= 0;
									}
									return false;
								}
								Item val3 = InventoryUi.InventoryControl_LeftRing.Inventory.Items.FirstOrDefault();
								Item val4 = InventoryUi.InventoryControl_RightRing.Inventory.Items.FirstOrDefault();
								return (RemoteMemoryObject)(object)val3 == (RemoteMemoryObject)null || CanBeAnUpgrade(val3, item) || (RemoteMemoryObject)(object)val4 == (RemoteMemoryObject)null || CanBeAnUpgrade(val4, item);
							}
							Item val5 = InventoryUi.InventoryControl_Neck.Inventory.Items.FirstOrDefault();
							return (RemoteMemoryObject)(object)val5 == (RemoteMemoryObject)null || CanBeAnUpgrade(val5, item);
						}
						Item val6 = InventoryUi.InventoryControl_Belt.Inventory.Items.FirstOrDefault();
						return (RemoteMemoryObject)(object)val6 == (RemoteMemoryObject)null || CanBeAnUpgrade(val6, item);
					}
					Item val7 = InventoryUi.InventoryControl_Boots.Inventory.Items.FirstOrDefault();
					return (RemoteMemoryObject)(object)val7 == (RemoteMemoryObject)null || CanBeAnUpgrade(val7, item);
				}
				Item val8 = InventoryUi.InventoryControl_Gloves.Inventory.Items.FirstOrDefault();
				return (RemoteMemoryObject)(object)val8 == (RemoteMemoryObject)null || CanBeAnUpgrade(val8, item);
			}
			Item val9 = InventoryUi.InventoryControl_Chest.Inventory.Items.FirstOrDefault();
			return (RemoteMemoryObject)(object)val9 == (RemoteMemoryObject)null || CanBeAnUpgrade(val9, item);
		}
		Item val10 = InventoryUi.InventoryControl_Head.Inventory.Items.FirstOrDefault();
		return (RemoteMemoryObject)(object)val10 == (RemoteMemoryObject)null || CanBeAnUpgrade(val10, item);
	}

	public static bool CheckIfMainHandTypeEnabled(Item item)
	{
		return item.Class switch
		{
			"Wand" => Config.ShouldEquipWands, 
			"Two Hand Axe" => Config.ShouldEquipTwoHandedAxes, 
			"Bow" => Config.ShouldEquipBows, 
			"Warstaff" => Config.ShouldEquipWarstaffs, 
			"One Hand Sword" => Config.ShouldEquipOneHandedSwords, 
			"Claw" => Config.ShouldEquipClaws, 
			"Sceptre" => Config.ShouldEquipSceptres, 
			"Two Hand Sword" => Config.ShouldEquipTwoHandedSwords, 
			"Two Hand Mace" => Config.ShouldEquipTwoHandedMaces, 
			"Dagger" => Config.ShouldEquipDaggers, 
			"One Hand Axe" => Config.ShouldEquipOneHandedAxes, 
			"One Hand Mace" => Config.ShouldEquipOneHandedMaces, 
			"Staff" => Config.ShouldEquipStaffs, 
			_ => false, 
		};
	}

	public static bool CheckIfOffHandTypeEnabled(Item item)
	{
		string @class = item.Class;
		string text = @class;
		if (!(text == "Quiver"))
		{
			if (text == "Shield")
			{
				return Config.ShouldEquipShield;
			}
			if (Config.ShouldEquipWeaponInOffHand)
			{
				return item.Class switch
				{
					"Wand" => Config.ShouldEquipWands, 
					"One Hand Sword" => Config.ShouldEquipOneHandedSwords, 
					"Claw" => Config.ShouldEquipClaws, 
					"Sceptre" => Config.ShouldEquipSceptres, 
					"Dagger" => Config.ShouldEquipDaggers, 
					"One Hand Axe" => Config.ShouldEquipOneHandedAxes, 
					"One Hand Mace" => Config.ShouldEquipOneHandedMaces, 
					_ => false, 
				};
			}
			return false;
		}
		return Config.ShouldEquipBows;
	}

	public static bool HasStats(Item newItem)
	{
		return ((Player)LokiPoe.Me).Level >= newItem.RequiredLevel && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)576) >= newItem.RequiredInt && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)573) >= newItem.RequiredStr && ((Actor)LokiPoe.Me).GetStat((StatTypeGGG)579) >= newItem.RequiredDex;
	}

	private static bool CanBeAnUpgrade(Item oldItem, Item newItem)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		if ((int)oldItem.Rarity == 1 && (int)newItem.Rarity == 1 && SocketHelper.HasBetterOrSameColors(oldItem, newItem))
		{
			return Config.ShouldUpgradeMagicToMagic;
		}
		if (oldItem.Rarity < newItem.Rarity)
		{
			return true;
		}
		if ((int)oldItem.Rarity == 2 && (int)newItem.Rarity == 2 && SocketHelper.HasBetterOrSameColors(oldItem, newItem))
		{
			return true;
		}
		if ((int)oldItem.Rarity != 3 || (int)newItem.Rarity != 2 || !SocketHelper.HasBetterOrSameColors(oldItem, newItem))
		{
			return false;
		}
		return true;
	}

	private static int GetFlaskSlotToUpgrade(Item newItem)
	{
		int int_0;
		for (int_0 = 0; int_0 < 5; int_0++)
		{
			if (CanBeFlaskUpgrade(QuickFlaskHud.InventoryControl.Inventory.Items.FirstOrDefault((Item f) => f.LocationTopLeft.X == int_0), newItem, int_0 + 1) >= 0)
			{
				return int_0;
			}
		}
		return -1;
	}

	private static int CanBeFlaskUpgrade(Item oldItem, Item newItem, int flaskSlot)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)newItem.ItemType != 10)
		{
			return -1;
		}
		string flaskType = "None";
		if (newItem.Name.ContainsIgnorecase("life"))
		{
			flaskType = "life";
		}
		if (newItem.Name.ContainsIgnorecase("mana"))
		{
			flaskType = "mana";
		}
		if (newItem.Name.ContainsIgnorecase("hybrid"))
		{
			flaskType = "hybrid";
		}
		if (newItem.Name.ContainsIgnorecase("quicksilver"))
		{
			flaskType = "quicksilver";
		}
		if (newItem.Name.ContainsIgnorecase("quartz"))
		{
			flaskType = "quartz";
		}
		if (newItem.Name.ContainsIgnorecase("granite"))
		{
			flaskType = "granite";
		}
		if (newItem.Name.ContainsIgnorecase("basalt"))
		{
			flaskType = "basalt";
		}
		if (newItem.Name.ContainsIgnorecase("jade"))
		{
			flaskType = "jade";
		}
		if (newItem.Name.ContainsIgnorecase("stibnite"))
		{
			flaskType = "stibnite";
		}
		if (newItem.Name.ContainsIgnorecase("sulphur"))
		{
			flaskType = "sulphur";
		}
		return FlaskCheck(oldItem, newItem, flaskSlot, flaskType);
	}

	private static int FlaskCheck(Item oldItem, Item newItem, int flaskSlot, string flaskType)
	{
		switch (flaskSlot)
		{
		case 1:
			if (Config.FlaskSlot1.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 0;
			}
			break;
		case 2:
			if (Config.FlaskSlot2.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 1;
			}
			break;
		case 3:
			if (Config.FlaskSlot3.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 2;
			}
			break;
		case 4:
			if (Config.FlaskSlot4.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 3;
			}
			break;
		case 5:
			if (Config.FlaskSlot5.EqualsIgnorecase(flaskType) && ((RemoteMemoryObject)(object)oldItem == (RemoteMemoryObject)null || oldItem.BaseRequiredLevel < newItem.BaseRequiredLevel))
			{
				return 4;
			}
			break;
		}
		return -1;
	}

	static EquipPluginExEvaluator()
	{
		Config = Class27.Instance;
	}
}
