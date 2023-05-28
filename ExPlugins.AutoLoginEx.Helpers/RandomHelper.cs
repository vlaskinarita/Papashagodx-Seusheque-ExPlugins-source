using System;
using System.Collections.Generic;
using System.Globalization;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoLoginEx.Helpers;

public class RandomHelper
{
	private readonly char[] char_0 = "bcdfghklmnprstwz".ToCharArray();

	private readonly List<string> list_0 = new List<string>();

	private readonly char[] char_1 = "aeiou".ToCharArray();

	private readonly List<string> list_1 = new List<string>();

	private Random random_0;

	private int int_0;

	private Random random_1;

	private int int_1;

	public List<CharacterClass> CharacterClasses = new List<CharacterClass>
	{
		(CharacterClass)3,
		(CharacterClass)1,
		(CharacterClass)4,
		(CharacterClass)6,
		(CharacterClass)5
	};

	public bool IsGenerated => random_1 != null;

	public RandomHelper()
	{
	}

	public RandomHelper(List<string> wordList)
	{
		list_1 = wordList;
	}

	public void Generate()
	{
		string email = AutoLoginSettings.Instance.Email;
		if (!string.IsNullOrEmpty(email))
		{
			int num = email.GetHashCode();
			try
			{
				num = ((num > 0) ? (num - Environment.TickCount) : (num + Environment.TickCount));
			}
			catch
			{
			}
			int_1 = num;
			int_0 = email.GetHashCode();
		}
		else
		{
			int_1 = Environment.TickCount;
			int_0 = 1337;
		}
		GlobalLog.Info($"[RandomHelper] Generating with seed: {int_1}");
		random_1 = new Random(int_1);
		random_0 = new Random(int_0);
		GenerateSyllables();
	}

	public string GetRandomName()
	{
		if (!IsGenerated)
		{
			Generate();
		}
		if (list_1.Count >= 100)
		{
			string text = list_1[random_1.Next(0, list_1.Count)];
			string text2 = list_1[random_1.Next(0, list_1.Count)];
			bool flag = false;
			if (text.Length >= 8)
			{
				text = text.Substring(0, random_1.Next(4, text.Length));
			}
			if (text2.Length >= 6)
			{
				int num = random_1.Next(3, 5);
				text2 = text2.Substring(num, text2.Length - num);
				flag = true;
			}
			bool flag2 = random_1.Next(0, 100) <= 50;
			bool flag3 = random_1.Next(0, 100) <= 50;
			bool flag4 = random_1.Next(0, 100) <= 25;
			if (flag2)
			{
				text = CapitalizeWord(text);
			}
			if (flag3)
			{
				text2 = CapitalizeWord(text2);
			}
			string text3 = text + ((!flag4 || flag) ? "" : "_") + text2;
			GlobalLog.Debug("[GetRandomName] " + text3 + " was generated as a next character name.");
			return text3;
		}
		throw new Exception("Not enough words.");
	}

	public void GenerateSyllables()
	{
		char[] array = char_0;
		foreach (char c in array)
		{
			char[] array2 = char_1;
			foreach (char c2 in array2)
			{
				string item = $"{c}{c2}";
				if (!list_0.Contains(item))
				{
					list_0.Add(item);
				}
				char[] array3 = char_0;
				foreach (char c3 in array3)
				{
					item = $"{c}{c2}{c3}";
					if (!list_0.Contains(item))
					{
						list_0.Add(item);
					}
				}
			}
		}
	}

	public List<CharacterClass> GetRandomClasses(int num, CharacterClass excludedClass)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		CharacterClasses.Remove(excludedClass);
		if (num >= CharacterClasses.Count)
		{
			return CharacterClasses;
		}
		List<CharacterClass> list = new List<CharacterClass>();
		for (int i = 0; i < num; i++)
		{
			int index = random_0.Next(0, CharacterClasses.Count);
			list.Add(CharacterClasses[index]);
			CharacterClasses.RemoveAt(index);
		}
		return list;
	}

	public bool Condition(int d = 5)
	{
		return random_1.Next(0, 5) == 0;
	}

	public int GetRandomNumber(int min, int max)
	{
		return random_1.Next(min, max);
	}

	public string CapitalizeWord(string str)
	{
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
	}
}
