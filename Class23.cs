using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

internal class Class23
{
	public static string smethod_0(string path)
	{
		List<string> list = (from p in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
			orderby p
			select p).ToList();
		MD5 mD = MD5.Create();
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			string text2 = text.Substring(path.Length + 1);
			byte[] bytes = Encoding.UTF8.GetBytes(text2.ToLower());
			mD.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			byte[] array = File.ReadAllBytes(text);
			if (i != list.Count - 1)
			{
				mD.TransformBlock(array, 0, array.Length, array, 0);
			}
			else
			{
				mD.TransformFinalBlock(array, 0, array.Length);
			}
		}
		return BitConverter.ToString(mD.Hash).Replace("-", "").ToLower();
	}
}
