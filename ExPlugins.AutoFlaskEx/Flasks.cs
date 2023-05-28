using System;
using System.Collections.Generic;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoFlaskEx;

public static class Flasks
{
	public static Item LifeFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Name.ContainsIgnorecase("life flask") && !f.IsInstantRecovery);

	public static Item InstantLifeFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Name.ContainsIgnorecase("life flask") && f.IsInstantRecovery);

	public static Item ManaFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Name.ContainsIgnorecase("mana flask") && !f.IsInstantRecovery);

	public static Item InstantManaFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Name.ContainsIgnorecase("mana flask") && f.IsInstantRecovery);

	public static Item HybridFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Class == "HybridFlask" && f.FullName != "Divination Distillate" && !f.IsInstantRecovery);

	public static Item InstantHybridFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Class == "HybridFlask" && f.IsInstantRecovery);

	public static Item QuicksilverFlask => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.Name == "Quicksilver Flask");

	public static Item KiaraDetermination => QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.FullName == "Kiara's Determination");

	public static Item ByProperName(string name)
	{
		return QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.ProperName() == name);
	}

	public static Item ByStat(StatTypeGGG stat)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return QuickFlaskHud.InventoryControl.Inventory.Items.HighestCharge((Item f) => f.LocalStats.ContainsKey(stat));
	}

	public static string ProperName(this Item item)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return ((int)item.Rarity == 3) ? item.FullName : item.Name;
	}

	private static Item HighestCharge(this IEnumerable<Item> flasks, Func<Item, bool> match)
	{
		Item val = null;
		foreach (Item flask in flasks)
		{
			if (match(flask) && flask.CanUse && ((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null || val.ItemLevel < flask.ItemLevel || val.CurrentCharges < flask.CurrentCharges))
			{
				val = flask;
			}
		}
		return val;
	}
}
