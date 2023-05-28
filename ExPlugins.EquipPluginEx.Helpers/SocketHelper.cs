using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.EquipPluginEx.Helpers;

public static class SocketHelper
{
	public static bool HasBetterOrSameColors(Item oldItem, Item newItem)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		string colorsForItemType = GetColorsForItemType(oldItem.ItemType);
		if (!string.IsNullOrEmpty(colorsForItemType))
		{
			int itemSocketsWeight = GetItemSocketsWeight(oldItem, colorsForItemType);
			int itemSocketsWeight2 = GetItemSocketsWeight(newItem, colorsForItemType);
			return itemSocketsWeight2 >= itemSocketsWeight;
		}
		return true;
	}

	public static int GetColorsWeight(Item item)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		string colorsForItemType = GetColorsForItemType(item.ItemType);
		if (string.IsNullOrEmpty(colorsForItemType))
		{
			return 0;
		}
		return GetItemSocketsWeight(item, colorsForItemType);
	}

	public static int GetItemSocketsWeight(Item item, string colors)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		List<char> list = new List<char>();
		list.AddRange(colors);
		int num = 0;
		SocketColor[] array = item.LinkedSocketColors.OrderByDescending((SocketColor[] x) => x.Count()).FirstOrDefault();
		int num2 = 0;
		if (array != null)
		{
			SocketColor[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				SocketColor val = array2[i];
				char c = ((object)(SocketColor)(ref val)).ToString()[0];
				if (list.Contains(c))
				{
					list.Remove(c);
					num2++;
				}
				if (c == 'W')
				{
					num++;
				}
			}
		}
		return num2 + num;
	}

	public static string GetColorsForItemType(InventoryType itemType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected I4, but got Unknown
		return (itemType - 1) switch
		{
			0 => Class27.Instance.BodyArmourColors, 
			1 => Class27.Instance.MainHandColors, 
			2 => Class27.Instance.OffHandColors, 
			3 => Class27.Instance.HelmetColors, 
			6 => Class27.Instance.GlovesColors, 
			7 => Class27.Instance.BootsColors, 
			_ => "", 
		};
	}
}
