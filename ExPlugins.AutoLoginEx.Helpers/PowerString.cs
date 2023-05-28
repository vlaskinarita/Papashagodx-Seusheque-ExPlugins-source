using System.Collections.Generic;
using System.Linq;
using ExPlugins.EXtensions;

namespace ExPlugins.AutoLoginEx.Helpers;

public static class PowerString
{
	public static readonly List<char> AllChars;

	static PowerString()
	{
		AllChars = "_abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList();
		AllChars.Reverse();
	}

	public static int GetPowerOfString(string str)
	{
		str = str.ToLower();
		int num = AllChars.IndexOf(str[0]) * 1000;
		int num2 = AllChars.IndexOf(str[1]) * 100;
		int num3 = AllChars.IndexOf(str[2]) * 10;
		int num4 = AllChars.IndexOf(str[3]);
		return num + num2 + num3 + num4;
	}

	public static bool ComparePower(string str1, string str2)
	{
		int powerOfString = GetPowerOfString(str1);
		int powerOfString2 = GetPowerOfString(str2);
		GlobalLog.Info($"{str1}[{powerOfString}] vs {str2}[{powerOfString2}]");
		return powerOfString > powerOfString2;
	}
}
