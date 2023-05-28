using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;

namespace ExPlugins.PapashaCore.Core;

public static class Core
{
	public struct flWX1mkkDGmd
	{
		public List<int> sdbmewknwekb;

		public flWX1mkkDGmd(int KLDGNgo3pmgv, int KVLSD32ioynlk, int DnGHMk42, int int_0, int lmshiod3nksd, int int_1, int DSKLNgkjl32w, int int_2, int int_3, int int_4, int int_5, int int_6, int int_7)
		{
			sdbmewknwekb = new List<int>
			{
				KLDGNgo3pmgv, KVLSD32ioynlk, DnGHMk42, int_0, lmshiod3nksd, DSKLNgkjl32w, int_2, int_3, int_5, int_6,
				int_5, int_6, int_7
			};
		}
	}

	private static int int_0;

	private static int int_1;

	private static readonly int int_2;

	private static readonly string string_0;

	private static readonly string string_1;

	private static Socket socket_0;

	private static void SocketInit()
	{
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(string_0), int_2);
		socket_0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket_0.Connect(remoteEP);
	}
    private static void smethod_0() {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(string_0), int_2);
        socket_0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket_0.Connect(remoteEP);
    }
    private static void SocketClose()
	{
		if (socket_0.Connected)
		{
			socket_0.Shutdown(SocketShutdown.Both);
		}
		socket_0.Close();
	}

	

	private static void oGysVgAeb4()
	{
		if (socket_0.Connected)
		{
			socket_0.Shutdown(SocketShutdown.Both);
		}
		socket_0.Close();
	}

	private static string smethod_1(string seed)
	{
		byte[] array = new byte[1024];
		StringBuilder stringBuilder = new StringBuilder();
		do
		{
			int count = socket_0.Receive(array, array.Length, SocketFlags.None);
			stringBuilder.Append(Encoding.UTF8.GetString(array, 0, count));
		}
		while (socket_0.Available > 0);
		return CustomDecode(stringBuilder.ToString(), seed);
	}

	private static void smethod_2(string message, string seed)
	{
		message = smethod_4(message, seed);
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		socket_0.Send(bytes);
	}

	private static string smethod_3()
	{
		string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		char[] array = new char[6];
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		for (int i = 0; i < array.Length; i++)
		{
			byte[] array2 = new byte[4];
			randomNumberGenerator.GetBytes(array2);
			int num = BitConverter.ToInt32(array2, 0);
			array[i] = text[Math.Abs(num % (text.Length - 1))];
		}
		return new string(array);
	}

	private static string smethod_4(string message, string seed)
	{
		char[] array = message.ToCharArray();
		char[] array2 = seed.ToCharArray();
		for (int i = 0; i < array2.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = (char)(array[j] ^ array2[i]);
			}
		}
		char[] array3 = new char[array2.Length + array.Length];
		int num = 0;
		for (int k = 0; k < array.Length; k++)
		{
			array3[num] = array[k];
			num++;
			if (num == 3)
			{
				array3[num] = array2[0];
				num++;
			}
			if (num == 5)
			{
				array3[num] = array2[1];
				num++;
			}
			if (num == 6)
			{
				array3[num] = array2[2];
				num++;
			}
			if (num == 8)
			{
				array3[num] = array2[3];
				num++;
			}
			if (num == 10)
			{
				array3[num] = array2[4];
				num++;
			}
			if (num == 11)
			{
				array3[num] = array2[5];
				num++;
			}
		}
		return new string(array3);
	}

	private static string ReceiveFromServer(string seed)
	{
		byte[] array = new byte[1024];
		StringBuilder stringBuilder = new StringBuilder();
		do
		{
			int count = socket_0.Receive(array, array.Length, SocketFlags.None);
			stringBuilder.Append(Encoding.UTF8.GetString(array, 0, count));
		}
		while (socket_0.Available > 0);
		return CustomDecode(stringBuilder.ToString(), seed);
	}

	private static void SendToServer(string message, string seed)
	{
		message = CustomEncode(message, seed);
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		socket_0.Send(bytes);
	}

	private static string SeedGen()
	{
		char[] array = new char[6];
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		for (int i = 0; i < array.Length; i++)
		{
			byte[] array2 = new byte[4];
			randomNumberGenerator.GetBytes(array2);
			int num = BitConverter.ToInt32(array2, 0);
			array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[Math.Abs(num % ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length - 1))];
		}
		return new string(array);
	}

	private static string CustomEncode(string message, string seed)
	{
		char[] array = message.ToCharArray();
		char[] array2 = seed.ToCharArray();
		for (int i = 0; i < array2.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = (char)(array[j] ^ array2[i]);
			}
		}
		char[] array3 = new char[array2.Length + array.Length];
		int num = 0;
		for (int k = 0; k < array.Length; k++)
		{
			array3[num] = array[k];
			num++;
			if (num == 3)
			{
				array3[num] = array2[0];
				num++;
			}
			if (num == 5)
			{
				array3[num] = array2[1];
				num++;
			}
			if (num == 6)
			{
				array3[num] = array2[2];
				num++;
			}
			if (num == 8)
			{
				array3[num] = array2[3];
				num++;
			}
			if (num == 10)
			{
				array3[num] = array2[4];
				num++;
			}
			if (num == 11)
			{
				array3[num] = array2[5];
				num++;
			}
		}
		return new string(array3);
	}

	private static string CustomDecode(string message, string seed)
	{
		char[] array = message.ToCharArray();
		char[] array2 = seed.ToCharArray();
		for (int num = array2.Length - 1; num > -1; num--)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (char)(array[i] ^ array2[num]);
			}
		}
		return new string(array);
	}

	public static flWX1mkkDGmd smethod_5()
	{
		flWX1mkkDGmd result = new flWX1mkkDGmd(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		try
		{
			smethod_0();
			try
			{
				string seed = smethod_3();
				string message = "AU" + string_1;
				smethod_2(message, seed);
				string[] array = smethod_1(seed).Split(' ');
				result.sdbmewknwekb[0] = int.Parse(array[0]);
				result.sdbmewknwekb[1] = int.Parse(array[1]);
				result.sdbmewknwekb[2] = int.Parse(array[2]);
				result.sdbmewknwekb[3] = int.Parse(array[3]);
				result.sdbmewknwekb[4] = int.Parse(array[4]);
				result.sdbmewknwekb[5] = int.Parse(array[5]);
				result.sdbmewknwekb[6] = int.Parse(array[6]);
				result.sdbmewknwekb[7] = int.Parse(array[7]);
				result.sdbmewknwekb[8] = int.Parse(array[8]);
				result.sdbmewknwekb[9] = int.Parse(array[9]);
				result.sdbmewknwekb[10] = int.Parse(array[10]);
				result.sdbmewknwekb[11] = int.Parse(array[11]);
				result.sdbmewknwekb[12] = int.Parse(array[12]);
				oGysVgAeb4();
				return result;
			}
			catch (Exception ex)
			{
				oGysVgAeb4();
				d1f45308(ex);
				return result;
			}
		}
		catch (Exception ex2)
		{
			oGysVgAeb4();
			d1f45308(ex2);
			return result;
		}
	}

	public static flWX1mkkDGmd smethod_6(string saGewqgwe, int rjoekh43iadsf)
	{
		flWX1mkkDGmd result = new flWX1mkkDGmd(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		try
		{
			smethod_0();
			try
			{
				string seed = smethod_3();
				if (rjoekh43iadsf >= 1 && rjoekh43iadsf <= 9)
				{
					string message = "UP" + saGewqgwe + rjoekh43iadsf + string_1;
					smethod_2(message, seed);
					string text = smethod_1(seed);
					oGysVgAeb4();
					if (!(text == "NO TIME LEFT"))
					{
						string[] array = text.Split(' ');
						result.sdbmewknwekb[0] = int.Parse(array[0]);
						result.sdbmewknwekb[1] = int.Parse(array[1]);
						result.sdbmewknwekb[2] = int.Parse(array[2]);
						result.sdbmewknwekb[3] = int.Parse(array[3]);
						result.sdbmewknwekb[4] = int.Parse(array[4]);
						result.sdbmewknwekb[5] = int.Parse(array[5]);
						result.sdbmewknwekb[6] = int.Parse(array[6]);
						result.sdbmewknwekb[7] = int.Parse(array[7]);
						result.sdbmewknwekb[8] = int.Parse(array[8]);
						result.sdbmewknwekb[9] = int.Parse(array[9]);
						result.sdbmewknwekb[10] = int.Parse(array[10]);
						result.sdbmewknwekb[11] = int.Parse(array[11]);
						result.sdbmewknwekb[12] = int.Parse(array[12]);
						PapashaCore.additionalTimeUse = 0;
						int_0 = 0;
						return result;
					}
					smethod_7();
					return result;
				}
				return result;
			}
			catch (Exception ex)
			{
				oGysVgAeb4();
				d1f45308(ex);
				return result;
			}
		}
		catch (Exception ex2)
		{
			oGysVgAeb4();
			d1f45308(ex2);
			return result;
		}
	}

	public static string GetActualCheckSum()
	{
		try
		{
			SocketInit();
			try
			{
				string seed = SeedGen();
				string message = "CS" + string_1;
				SendToServer(message, seed);
				string result = ReceiveFromServer(seed);
				SocketClose();
				return result;
			}
			catch (Exception ex)
			{
				SocketClose();
				CustomErrorHandler(ex);
				return null;
			}
		}
		catch (Exception ex2)
		{
			SocketClose();
			CustomErrorHandler(ex2);
			return null;
		}
	}

	private static void CustomErrorHandler(Exception ex)
	{
		int_0++;
		PapashaCore.additionalTimeUse++;
		PapashaCore.Log.ErrorFormat($"[PapashaCore] Error while contacting server! {int_0}/5", Array.Empty<object>());
		Console.WriteLine(ex.Message);
		if (int_0 >= 5)
		{
			PapashaCore.Log.ErrorFormat("[PapashaCore] An error threshold has been reached. Now terminating poe.", Array.Empty<object>());
			LokiPoe.Memory.Process.Kill();
		}
	}

	private static void NoTimeHandler()
	{
		PapashaCore.Log.ErrorFormat("[PapashaCore] There is no time left for some of your enabled plugin. Please check your time!", Array.Empty<object>());
		if (int_1 == 0)
		{
			int_1++;
			BotManager.Stop(false);
		}
		else
		{
			LokiPoe.Memory.Process.Kill();
		}
	}

	public static string f8f756b6b3def(bool saklgnedjlkg, bool dsgkni42ngo432, bool ASLKFGngjk432g4, bool sdAGKLhngio32, bool ASJGKh3io2unoi23, bool DAKJGNoi32gn32, bool DASGKJbnuoik23h, bool bool_0, bool bool_1 = false, bool rgieower3 = false, bool bool_2 = false, bool bool_3 = false, bool KMNEIjn843343 = false)
	{
		string text = "";
		text += (saklgnedjlkg ? "1" : "0");
		text += ((!dsgkni42ngo432) ? "0" : "1");
		text += ((!ASLKFGngjk432g4) ? "0" : "1");
		text += (sdAGKLhngio32 ? "1" : "0");
		text += (ASJGKh3io2unoi23 ? "1" : "0");
		text += (DAKJGNoi32gn32 ? "1" : "0");
		text += (DASGKJbnuoik23h ? "1" : "0");
		text += (bool_0 ? "1" : "0");
		text += (bool_1 ? "1" : "0");
		text += ((!rgieower3) ? "0" : "1");
		text += (bool_2 ? "1" : "0");
		text += ((!bool_3) ? "0" : "1");
		return text + ((!KMNEIjn843343) ? "0" : "1");
	}

	private static void d1f45308(Exception ex)
	{
		int_0++;
		PapashaCore.additionalTimeUse++;
		PapashaCore.Log.ErrorFormat($"[PapashaCore] Error while contacting server! {int_0}/5 in a row.", Array.Empty<object>());
		Console.WriteLine(ex.Message);
		if (int_0 >= 5)
		{
			PapashaCore.Log.ErrorFormat("[PapashaCore] An error threshold has been reached. Now terminating poe.", Array.Empty<object>());
			LokiPoe.Memory.Process.Kill();
		}
	}

	private static void smethod_7()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		PapashaCore.Log.ErrorFormat("[PapashaCore] There is no time left for some of your enabled plugin. Please check your time!", Array.Empty<object>());
		if (int_1 == 0)
		{
			int_1++;
			BotManager.Stop(new StopReasonData("plugin_time_ended", "There is no time left for some of your enabled plugin. Please check your time!", (object)null), false);
		}
		else
		{
			LokiPoe.Memory.Process.Kill();
		}
	}

	static Core()
	{
		int_2 = 1488;
		string_0 = "212.224.112.104";
		string_1 = PapashaCoreSettings.Instance.Key;
	}
}
