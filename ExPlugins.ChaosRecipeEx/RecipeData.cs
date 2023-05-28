using System;
using System.Collections.Generic;
using System.IO;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using Newtonsoft.Json;

namespace ExPlugins.ChaosRecipeEx;

public class RecipeData
{
	private static readonly Dictionary<string, int> dictionary_0;

	private readonly int[] int_0 = new int[8];

	public void IncreaseItemCount(int itemType)
	{
		int_0[itemType]++;
	}

	public int GetItemCount(int itemType)
	{
		return int_0[itemType];
	}

	public void Reset()
	{
		for (int i = 0; i < int_0.Length; i++)
		{
			int_0[i] = 0;
		}
	}

	public void SyncWithStashTab()
	{
		Reset();
		foreach (Item stashTabItem in Inventories.StashTabItems)
		{
			if (IsItemForChaosRecipe(stashTabItem, out var itemType))
			{
				IncreaseItemCount(itemType);
			}
		}
		SaveToJson(ChaosRecipeEx.StashDataPath);
	}

	public bool HasCompleteSet()
	{
		return GetItemCount(0) >= 2 && GetItemCount(7) >= 2 && GetItemCount(6) >= 1 && GetItemCount(5) >= 1 && GetItemCount(4) >= 1 && GetItemCount(3) >= 1 && GetItemCount(2) >= 1 && GetItemCount(1) >= 1;
	}

	public void Log()
	{
		GlobalLog.Info($"[ChaosRecipeEx] Weapons: {GetItemCount(0)}");
		GlobalLog.Info($"[ChaosRecipeEx] Body armors: {GetItemCount(1)}");
		GlobalLog.Info($"[ChaosRecipeEx] Helmets: {GetItemCount(2)}");
		GlobalLog.Info($"[ChaosRecipeEx] Boots: {GetItemCount(3)}");
		GlobalLog.Info($"[ChaosRecipeEx] Gloves: {GetItemCount(4)}");
		GlobalLog.Info($"[ChaosRecipeEx] Belts: {GetItemCount(5)}");
		GlobalLog.Info($"[ChaosRecipeEx] Amulets: {GetItemCount(6)}");
		GlobalLog.Info($"[ChaosRecipeEx] Rings: {GetItemCount(7)}");
	}

	public void SaveToJson(string path)
	{
		string contents = JsonConvert.SerializeObject((object)int_0, (Formatting)1);
		File.WriteAllText(path, contents);
	}

	public static RecipeData LoadFromJson(string path)
	{
		RecipeData recipeData = new RecipeData();
		if (File.Exists(path))
		{
			string text = File.ReadAllText(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				GlobalLog.Info("[ChaosRecipeEx] Fail to load stash data from json. File is empty.");
				return recipeData;
			}
			int[] array;
			try
			{
				array = JsonConvert.DeserializeObject<int[]>(text);
			}
			catch (Exception)
			{
				GlobalLog.Info("[ChaosRecipeEx] Fail to load stash data from json. Exception during json deserialization.");
				return recipeData;
			}
			if (array == null)
			{
				GlobalLog.Info("[ChaosRecipeEx] Fail to load stash data from json. Json deserealizer returned null.");
				return recipeData;
			}
			Array.Copy(array, recipeData.int_0, recipeData.int_0.Length);
			return recipeData;
		}
		return recipeData;
	}

	public static bool IsItemForChaosRecipe(Item item, out int itemType)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		itemType = -1;
		if (!item.IsIdentified && (int)item.Rarity == 2 && item.SocketCount < 6 && item.ItemLevel >= Settings.Instance.MinILvl)
		{
			return dictionary_0.TryGetValue(item.Class, out itemType) && (itemType != 0 || item.Size.X * item.Size.Y <= 4);
		}
		return false;
	}

	static RecipeData()
	{
		dictionary_0 = new Dictionary<string, int>
		{
			["Claw"] = 0,
			["Dagger"] = 0,
			["One Hand Axe"] = 0,
			["One Hand Mace"] = 0,
			["One Hand Sword"] = 0,
			["Thrusting One Hand Sword"] = 0,
			["Wand"] = 0,
			["Body Armour"] = 1,
			["Helmet"] = 2,
			["Boots"] = 3,
			["Gloves"] = 4,
			["Belt"] = 5,
			["Amulet"] = 6,
			["Ring"] = 7
		};
	}
}
