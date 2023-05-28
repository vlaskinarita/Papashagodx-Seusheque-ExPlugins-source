using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoLoginEx.Helpers;

public static class FileHelper
{
	private static readonly string string_0;

	public static IEnumerable<string> LoadWords()
	{
		Directory.CreateDirectory(string_0);
		HashSet<string> hashSet = new HashSet<string>();
		int num = 0;
		string[] files = Directory.GetFiles(string_0, "*.txt", SearchOption.AllDirectories);
		foreach (string path in files)
		{
			string[] array = File.ReadAllLines(path);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = text.ToLowerInvariant();
					if (text2.All((char c) => PowerString.AllChars.Contains(c)))
					{
						hashSet.Add(text2);
					}
				}
			}
			num++;
		}
		GlobalLog.Info($"[LoadWords] Loaded {hashSet.Count} words from {num} files.");
		return hashSet;
	}

	static FileHelper()
	{
		string_0 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Wordlists");
	}
}
