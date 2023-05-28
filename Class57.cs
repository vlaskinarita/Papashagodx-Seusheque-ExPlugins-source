using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

internal class Class57
{
	private delegate void Delegate1(object o);

	internal class Attribute0 : Attribute
	{
		internal class Class58<T>
		{
			internal static object object_0;

			internal static bool smethod_0()
			{
				return object_0 == null;
			}

			internal static object smethod_1()
			{
				return object_0;
			}
		}

		public Attribute0(object object_0)
		{
		}
	}

	internal class Class59
	{
		internal static string smethod_0(string string_0, string string_1)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(string_0);
			byte[] key = new byte[32]
			{
				82, 102, 104, 110, 32, 77, 24, 34, 118, 181,
				51, 17, 18, 51, 12, 109, 10, 32, 77, 24,
				34, 158, 161, 41, 97, 28, 118, 181, 5, 25,
				1, 88
			};
			byte[] iV = smethod_8(Encoding.Unicode.GetBytes(string_1));
			MemoryStream memoryStream = new MemoryStream();
			SymmetricAlgorithm symmetricAlgorithm = smethod_6();
			symmetricAlgorithm.Key = key;
			symmetricAlgorithm.IV = iV;
			CryptoStream cryptoStream = new CryptoStream(memoryStream, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.Close();
			return Convert.ToBase64String(memoryStream.ToArray());
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	internal delegate uint Delegate2(IntPtr classthis, IntPtr comp, IntPtr info, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr nativeEntry, ref uint nativeSizeOfCode);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate IntPtr Delegate3();

	internal struct Struct6
	{
		internal bool bool_0;

		internal byte[] byte_0;
	}

	internal class Class60
	{
		private BinaryReader binaryReader_0;

		public Class60(Stream stream_0)
		{
			binaryReader_0 = new BinaryReader(stream_0);
		}

		[SpecialName]
		internal Stream method_0()
		{
			return binaryReader_0.BaseStream;
		}

		internal byte[] method_1(int int_0)
		{
			return binaryReader_0.ReadBytes(int_0);
		}

		internal int method_2(byte[] byte_0, int int_0, int int_1)
		{
			return binaryReader_0.Read(byte_0, int_0, int_1);
		}

		internal int method_3()
		{
			return binaryReader_0.ReadInt32();
		}

		internal void method_4()
		{
			binaryReader_0.Close();
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	private delegate IntPtr Delegate4(IntPtr hModule, string lpName, uint lpType);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate IntPtr Delegate5(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int Delegate6(IntPtr hProcess, IntPtr lpBaseAddress, [In][Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int Delegate7(IntPtr lpAddress, int dwSize, int flNewProtect, ref int lpflOldProtect);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate IntPtr Delegate8(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int Delegate9(IntPtr ptr);

	[Flags]
	private enum Enum2
	{

	}

	private static List<int> list_0;

	private static List<string> list_1;

	private static int int_0;

	private static Delegate5 delegate5_0;

	private static bool bool_0;

	private static IntPtr intptr_0;

	private static Delegate8 delegate8_0;

	private static int int_1;

	private static int[] int_2;

	private static byte[] byte_0;

	private static bool bool_1;

	private static byte[] byte_1;

	private static string[] string_0;

	internal static Delegate2 delegate2_0;

	private static IntPtr intptr_1;

	internal static Assembly assembly_0;

	private static Delegate4 delegate4_0;

	private static long long_0;

	internal static object object_0;

	private static SortedList sortedList_0;

	private static int int_3;

	private static long long_1;

	private static int int_4;

	private static bool bool_2;

	private static int int_5;

	internal static Delegate2 delegate2_1;

	private static object object_1;

	private static Dictionary<int, int> dictionary_0;

	private static object object_2;

	[Attribute0(typeof(Attribute0.Class58<object>[]))]
	private static bool bool_3;

	private static bool bool_4;

	private static Delegate6 delegate6_0;

	internal static Hashtable hashtable_0;

	private static IntPtr intptr_2;

	private static Delegate7 delegate7_0;

	private static Delegate9 delegate9_0;

	private static bool bool_5;

	private static bool bool_6;

	private static uint[] uint_0;

	private static IntPtr intptr_3;

	static Class57()
	{
		bool_6 = false;
		assembly_0 = typeof(Class57).Assembly;
		uint_0 = new uint[64]
		{
			3614090360u, 3905402710u, 606105819u, 3250441966u, 4118548399u, 1200080426u, 2821735955u, 4249261313u, 1770035416u, 2336552879u,
			4294925233u, 2304563134u, 1804603682u, 4254626195u, 2792965006u, 1236535329u, 4129170786u, 3225465664u, 643717713u, 3921069994u,
			3593408605u, 38016083u, 3634488961u, 3889429448u, 568446438u, 3275163606u, 4107603335u, 1163531501u, 2850285829u, 4243563512u,
			1735328473u, 2368359562u, 4294588738u, 2272392833u, 1839030562u, 4259657740u, 2763975236u, 1272893353u, 4139469664u, 3200236656u,
			681279174u, 3936430074u, 3572445317u, 76029189u, 3654602809u, 3873151461u, 530742520u, 3299628645u, 4096336452u, 1126891415u,
			2878612391u, 4237533241u, 1700485571u, 2399980690u, 4293915773u, 2240044497u, 1873313359u, 4264355552u, 2734768916u, 1309151649u,
			4149444226u, 3174756917u, 718787259u, 3951481745u
		};
		bool_0 = false;
		bool_5 = false;
		object_0 = null;
		dictionary_0 = null;
		object_2 = new object();
		int_1 = 0;
		object_1 = new object();
		list_1 = null;
		list_0 = null;
		byte_0 = new byte[0];
		byte_1 = new byte[0];
		intptr_2 = IntPtr.Zero;
		intptr_0 = IntPtr.Zero;
		string_0 = new string[0];
		int_2 = new int[0];
		int_0 = 1;
		bool_2 = false;
		sortedList_0 = new SortedList();
		int_4 = 0;
		long_1 = 0L;
		delegate2_0 = null;
		delegate2_1 = null;
		long_0 = 0L;
		int_5 = 0;
		bool_1 = false;
		bool_4 = false;
		int_3 = 0;
		intptr_3 = IntPtr.Zero;
		bool_3 = false;
		hashtable_0 = new Hashtable();
		delegate4_0 = null;
		delegate5_0 = null;
		delegate6_0 = null;
		delegate7_0 = null;
		delegate8_0 = null;
		delegate9_0 = null;
		intptr_1 = IntPtr.Zero;
		try
		{
			RSACryptoServiceProvider.UseMachineKeyStore = true;
		}
		catch
		{
		}
	}

	private void method_0()
	{
	}

	internal static byte[] smethod_0(byte[] byte_2)
	{
		uint[] array = new uint[16];
		uint num = (uint)((448 - byte_2.Length * 8 % 512 + 512) % 512);
		if (num == 0)
		{
			num = 512u;
		}
		uint num2 = (uint)(byte_2.Length + num / 8u + 8L);
		ulong num3 = (ulong)(byte_2.Length * 8L);
		byte[] array2 = new byte[num2];
		for (int i = 0; i < byte_2.Length; i++)
		{
			array2[i] = byte_2[i];
		}
		array2[byte_2.Length] |= 128;
		for (int num4 = 8; num4 > 0; num4--)
		{
			array2[num2 - num4] = (byte)((num3 >> (8 - num4) * 8) & 0xFFuL);
		}
		uint num5 = (uint)(array2.Length * 8) / 32u;
		uint uint_ = 1732584193u;
		uint uint_2 = 4023233417u;
		uint uint_3 = 2562383102u;
		uint uint_4 = 271733878u;
		for (uint num6 = 0u; num6 < num5 / 16u; num6++)
		{
			uint num7 = num6 << 6;
			for (uint num8 = 0u; num8 < 61; num8 += 4)
			{
				array[num8 >> 2] = (uint)((array2[num7 + (num8 + 3)] << 24) | (array2[num7 + (num8 + 2)] << 16) | (array2[num7 + (num8 + 1)] << 8) | array2[num7 + num8]);
			}
			uint num9 = uint_;
			uint num10 = uint_2;
			uint num11 = uint_3;
			uint num12 = uint_4;
			ReZxSxiJZ(ref uint_, uint_2, uint_3, uint_4, 0u, 7, 1u, array);
			ReZxSxiJZ(ref uint_4, uint_, uint_2, uint_3, 1u, 12, 2u, array);
			ReZxSxiJZ(ref uint_3, uint_4, uint_, uint_2, 2u, 17, 3u, array);
			ReZxSxiJZ(ref uint_2, uint_3, uint_4, uint_, 3u, 22, 4u, array);
			ReZxSxiJZ(ref uint_, uint_2, uint_3, uint_4, 4u, 7, 5u, array);
			ReZxSxiJZ(ref uint_4, uint_, uint_2, uint_3, 5u, 12, 6u, array);
			ReZxSxiJZ(ref uint_3, uint_4, uint_, uint_2, 6u, 17, 7u, array);
			ReZxSxiJZ(ref uint_2, uint_3, uint_4, uint_, 7u, 22, 8u, array);
			ReZxSxiJZ(ref uint_, uint_2, uint_3, uint_4, 8u, 7, 9u, array);
			ReZxSxiJZ(ref uint_4, uint_, uint_2, uint_3, 9u, 12, 10u, array);
			ReZxSxiJZ(ref uint_3, uint_4, uint_, uint_2, 10u, 17, 11u, array);
			ReZxSxiJZ(ref uint_2, uint_3, uint_4, uint_, 11u, 22, 12u, array);
			ReZxSxiJZ(ref uint_, uint_2, uint_3, uint_4, 12u, 7, 13u, array);
			ReZxSxiJZ(ref uint_4, uint_, uint_2, uint_3, 13u, 12, 14u, array);
			ReZxSxiJZ(ref uint_3, uint_4, uint_, uint_2, 14u, 17, 15u, array);
			ReZxSxiJZ(ref uint_2, uint_3, uint_4, uint_, 15u, 22, 16u, array);
			smethod_1(ref uint_, uint_2, uint_3, uint_4, 1u, 5, 17u, array);
			smethod_1(ref uint_4, uint_, uint_2, uint_3, 6u, 9, 18u, array);
			smethod_1(ref uint_3, uint_4, uint_, uint_2, 11u, 14, 19u, array);
			smethod_1(ref uint_2, uint_3, uint_4, uint_, 0u, 20, 20u, array);
			smethod_1(ref uint_, uint_2, uint_3, uint_4, 5u, 5, 21u, array);
			smethod_1(ref uint_4, uint_, uint_2, uint_3, 10u, 9, 22u, array);
			smethod_1(ref uint_3, uint_4, uint_, uint_2, 15u, 14, 23u, array);
			smethod_1(ref uint_2, uint_3, uint_4, uint_, 4u, 20, 24u, array);
			smethod_1(ref uint_, uint_2, uint_3, uint_4, 9u, 5, 25u, array);
			smethod_1(ref uint_4, uint_, uint_2, uint_3, 14u, 9, 26u, array);
			smethod_1(ref uint_3, uint_4, uint_, uint_2, 3u, 14, 27u, array);
			smethod_1(ref uint_2, uint_3, uint_4, uint_, 8u, 20, 28u, array);
			smethod_1(ref uint_, uint_2, uint_3, uint_4, 13u, 5, 29u, array);
			smethod_1(ref uint_4, uint_, uint_2, uint_3, 2u, 9, 30u, array);
			smethod_1(ref uint_3, uint_4, uint_, uint_2, 7u, 14, 31u, array);
			smethod_1(ref uint_2, uint_3, uint_4, uint_, 12u, 20, 32u, array);
			smethod_2(ref uint_, uint_2, uint_3, uint_4, 5u, 4, 33u, array);
			smethod_2(ref uint_4, uint_, uint_2, uint_3, 8u, 11, 34u, array);
			smethod_2(ref uint_3, uint_4, uint_, uint_2, 11u, 16, 35u, array);
			smethod_2(ref uint_2, uint_3, uint_4, uint_, 14u, 23, 36u, array);
			smethod_2(ref uint_, uint_2, uint_3, uint_4, 1u, 4, 37u, array);
			smethod_2(ref uint_4, uint_, uint_2, uint_3, 4u, 11, 38u, array);
			smethod_2(ref uint_3, uint_4, uint_, uint_2, 7u, 16, 39u, array);
			smethod_2(ref uint_2, uint_3, uint_4, uint_, 10u, 23, 40u, array);
			smethod_2(ref uint_, uint_2, uint_3, uint_4, 13u, 4, 41u, array);
			smethod_2(ref uint_4, uint_, uint_2, uint_3, 0u, 11, 42u, array);
			smethod_2(ref uint_3, uint_4, uint_, uint_2, 3u, 16, 43u, array);
			smethod_2(ref uint_2, uint_3, uint_4, uint_, 6u, 23, 44u, array);
			smethod_2(ref uint_, uint_2, uint_3, uint_4, 9u, 4, 45u, array);
			smethod_2(ref uint_4, uint_, uint_2, uint_3, 12u, 11, 46u, array);
			smethod_2(ref uint_3, uint_4, uint_, uint_2, 15u, 16, 47u, array);
			smethod_2(ref uint_2, uint_3, uint_4, uint_, 2u, 23, 48u, array);
			smethod_3(ref uint_, uint_2, uint_3, uint_4, 0u, 6, 49u, array);
			smethod_3(ref uint_4, uint_, uint_2, uint_3, 7u, 10, 50u, array);
			smethod_3(ref uint_3, uint_4, uint_, uint_2, 14u, 15, 51u, array);
			smethod_3(ref uint_2, uint_3, uint_4, uint_, 5u, 21, 52u, array);
			smethod_3(ref uint_, uint_2, uint_3, uint_4, 12u, 6, 53u, array);
			smethod_3(ref uint_4, uint_, uint_2, uint_3, 3u, 10, 54u, array);
			smethod_3(ref uint_3, uint_4, uint_, uint_2, 10u, 15, 55u, array);
			smethod_3(ref uint_2, uint_3, uint_4, uint_, 1u, 21, 56u, array);
			smethod_3(ref uint_, uint_2, uint_3, uint_4, 8u, 6, 57u, array);
			smethod_3(ref uint_4, uint_, uint_2, uint_3, 15u, 10, 58u, array);
			smethod_3(ref uint_3, uint_4, uint_, uint_2, 6u, 15, 59u, array);
			smethod_3(ref uint_2, uint_3, uint_4, uint_, 13u, 21, 60u, array);
			smethod_3(ref uint_, uint_2, uint_3, uint_4, 4u, 6, 61u, array);
			smethod_3(ref uint_4, uint_, uint_2, uint_3, 11u, 10, 62u, array);
			smethod_3(ref uint_3, uint_4, uint_, uint_2, 2u, 15, 63u, array);
			smethod_3(ref uint_2, uint_3, uint_4, uint_, 9u, 21, 64u, array);
			uint_ += num9;
			uint_2 += num10;
			uint_3 += num11;
			uint_4 += num12;
		}
		byte[] array3 = new byte[16];
		Array.Copy(BitConverter.GetBytes(uint_), 0, array3, 0, 4);
		Array.Copy(BitConverter.GetBytes(uint_2), 0, array3, 4, 4);
		Array.Copy(BitConverter.GetBytes(uint_3), 0, array3, 8, 4);
		Array.Copy(BitConverter.GetBytes(uint_4), 0, array3, 12, 4);
		return array3;
	}

	private static void ReZxSxiJZ(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] uint_7)
	{
		uint_1 = uint_2 + smethod_4(uint_1 + ((uint_2 & uint_3) | (~uint_2 & uint_4)) + uint_7[uint_5] + uint_0[uint_6 - 1], ushort_0);
	}

	private static void smethod_1(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] uint_7)
	{
		uint_1 = uint_2 + smethod_4(uint_1 + ((uint_2 & uint_4) | (uint_3 & ~uint_4)) + uint_7[uint_5] + uint_0[uint_6 - 1], ushort_0);
	}

	private static void smethod_2(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] uint_7)
	{
		uint_1 = uint_2 + smethod_4(uint_1 + (uint_2 ^ uint_3 ^ uint_4) + uint_7[uint_5] + uint_0[uint_6 - 1], ushort_0);
	}

	private static void smethod_3(ref uint uint_1, uint uint_2, uint uint_3, uint uint_4, uint uint_5, ushort ushort_0, uint uint_6, uint[] uint_7)
	{
		uint_1 = uint_2 + smethod_4(uint_1 + (uint_3 ^ (uint_2 | ~uint_4)) + uint_7[uint_5] + uint_0[uint_6 - 1], ushort_0);
	}

	private static uint smethod_4(uint uint_1, ushort ushort_0)
	{
		return (uint_1 >> 32 - ushort_0) | (uint_1 << (int)ushort_0);
	}

	internal static bool smethod_5()
	{
		if (!bool_0)
		{
			smethod_7();
			bool_0 = true;
		}
		return bool_5;
	}

	internal Class57()
	{
	}

	private void method_1(byte[] byte_2, byte[] byte_3, byte[] byte_4)
	{
		int num = byte_4.Length % 4;
		int num2 = byte_4.Length / 4;
		byte[] array = new byte[byte_4.Length];
		int num3 = byte_2.Length / 4;
		uint num4 = 0u;
		uint num5 = 0u;
		uint num6 = 0u;
		if (num > 0)
		{
			num2++;
		}
		uint num7 = 0u;
		for (int i = 0; i < num2; i++)
		{
			int num8 = i % num3;
			int num9 = i * 4;
			num7 = (uint)(num8 * 4);
			num5 = (uint)((byte_2[num7 + 3] << 24) | (byte_2[num7 + 2] << 16) | (byte_2[num7 + 1] << 8) | byte_2[num7]);
			uint num10 = 255u;
			int num11 = 0;
			if (i == num2 - 1 && num > 0)
			{
				num6 = 0u;
				num4 += num5;
				for (int j = 0; j < num; j++)
				{
					if (j > 0)
					{
						num6 <<= 8;
					}
					num6 |= byte_4[byte_4.Length - (1 + j)];
				}
			}
			else
			{
				num4 += num5;
				num7 = (uint)num9;
				num6 = (uint)((byte_4[num7 + 3] << 24) | (byte_4[num7 + 2] << 16) | (byte_4[num7 + 1] << 8) | byte_4[num7]);
			}
			uint num12 = num4;
			num4 = 0u;
			uint num13 = num12;
			num13 -= 2426553847u;
			num13 ^= num13 >> 24;
			num13 += num13;
			num13 ^= num13 << 25;
			num13 += 3871009369u;
			num13 ^= num13 >> 3;
			num13 += 1313165438;
			num13 = (0x8F8B62AAu ^ num13) - num13;
			num4 = num12 + (uint)(double)num13;
			if (i == num2 - 1 && num > 0)
			{
				uint num14 = num4 ^ num6;
				for (int k = 0; k < num; k++)
				{
					if (k > 0)
					{
						num10 <<= 8;
						num11 += 8;
					}
					array[num9 + k] = (byte)((num14 & num10) >> num11);
				}
			}
			else
			{
				uint num15 = num4 ^ num6;
				array[num9] = (byte)(num15 & 0xFFu);
				array[num9 + 1] = (byte)((num15 & 0xFF00) >> 8);
				array[num9 + 2] = (byte)((num15 & 0xFF0000) >> 16);
				array[num9 + 3] = (byte)((num15 & 0xFF000000u) >> 24);
			}
		}
		byte_0 = array;
	}

	internal static SymmetricAlgorithm smethod_6()
	{
		SymmetricAlgorithm symmetricAlgorithm = null;
		if (smethod_5())
		{
			return new AesCryptoServiceProvider();
		}
		try
		{
			return new RijndaelManaged();
		}
		catch
		{
			try
			{
				return (SymmetricAlgorithm)Activator.CreateInstance("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
			}
			catch
			{
				return (SymmetricAlgorithm)Activator.CreateInstance("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Security.Cryptography.AesCryptoServiceProvider").Unwrap();
			}
		}
	}

	internal static void smethod_7()
	{
		try
		{
			new MD5CryptoServiceProvider();
		}
		catch
		{
			bool_5 = true;
			return;
		}
		try
		{
			bool_5 = CryptoConfig.AllowOnlyFipsAlgorithms;
		}
		catch
		{
		}
	}

	internal static byte[] smethod_8(byte[] byte_2)
	{
		if (!smethod_5())
		{
			return new MD5CryptoServiceProvider().ComputeHash(byte_2);
		}
		return smethod_0(byte_2);
	}

	internal static void smethod_9(HashAlgorithm hashAlgorithm_0, Stream stream_0, uint uint_1, byte[] byte_2)
	{
		while (uint_1 != 0)
		{
			int num = ((uint_1 > (uint)byte_2.Length) ? byte_2.Length : ((int)uint_1));
			stream_0.Read(byte_2, 0, num);
			VeZjNyNqf(hashAlgorithm_0, byte_2, 0, num);
			uint_1 -= (uint)num;
		}
	}

	internal static void VeZjNyNqf(HashAlgorithm hashAlgorithm_0, byte[] byte_2, int int_6, int int_7)
	{
		hashAlgorithm_0.TransformBlock(byte_2, int_6, int_7, byte_2, int_6);
	}

	internal static uint smethod_10(uint uint_1, int int_6, long long_2, BinaryReader binaryReader_0)
	{
		int num = 0;
		uint num3;
		uint num4;
		while (true)
		{
			if (num < int_6)
			{
				binaryReader_0.BaseStream.Position = long_2 + (num * 40 + 8);
				uint num2 = binaryReader_0.ReadUInt32();
				num3 = binaryReader_0.ReadUInt32();
				binaryReader_0.ReadUInt32();
				num4 = binaryReader_0.ReadUInt32();
				if (num3 <= uint_1 && uint_1 < num3 + num2)
				{
					break;
				}
				num++;
				continue;
			}
			return 0u;
		}
		return num4 + uint_1 - num3;
	}

	private static void smethod_11(Stream stream_0, int int_6)
	{
		Class62.smethod_2(0, new object[2] { stream_0, int_6 }, null);
	}

	internal static string smethod_12(string string_1)
	{
		"{11111-22222-50001-00000}".Trim();
		byte[] array = Convert.FromBase64String(string_1);
		return Encoding.Unicode.GetString(array, 0, array.Length);
	}

	internal static uint smethod_13(IntPtr intptr_4, IntPtr intptr_5, IntPtr intptr_6, [MarshalAs(UnmanagedType.U4)] uint uint_1, IntPtr intptr_7, ref uint uint_2)
	{
		IntPtr ptr = intptr_6;
		if (bool_6)
		{
			ptr = intptr_5;
		}
		long num = 0L;
		num = ((IntPtr.Size != 4) ? Marshal.ReadInt64(ptr, IntPtr.Size * 2) : Marshal.ReadInt32(ptr, IntPtr.Size * 2));
		object obj = hashtable_0[num];
		if (obj != null)
		{
			Struct6 @struct = (Struct6)obj;
			IntPtr intPtr = Marshal.AllocCoTaskMem(@struct.byte_0.Length);
			Marshal.Copy(@struct.byte_0, 0, intPtr, @struct.byte_0.Length);
			if (@struct.bool_0)
			{
				intptr_7 = intPtr;
				uint_2 = (uint)@struct.byte_0.Length;
				smethod_22(intptr_7, @struct.byte_0.Length, 64, ref int_3);
				return 0u;
			}
			Marshal.WriteIntPtr(ptr, IntPtr.Size * 2, intPtr);
			Marshal.WriteInt32(ptr, IntPtr.Size * 3, @struct.byte_0.Length);
			uint result = 0u;
			if (uint_1 == 216669565 && !bool_3)
			{
				bool_3 = true;
			}
			else
			{
				result = delegate2_0(intptr_4, intptr_5, intptr_6, uint_1, intptr_7, ref uint_2);
				Marshal.WriteIntPtr(ptr, IntPtr.Size * 2, IntPtr.Zero);
			}
			return result;
		}
		return delegate2_0(intptr_4, intptr_5, intptr_6, uint_1, intptr_7, ref uint_2);
	}

	private static int smethod_14()
	{
		return 5;
	}

	private static void smethod_15()
	{
		try
		{
			RSACryptoServiceProvider.UseMachineKeyStore = true;
		}
		catch
		{
		}
	}

	private static Delegate smethod_16(IntPtr intptr_4, Type type_0)
	{
		return (Delegate)typeof(Marshal).GetMethod("GetDelegateForFunctionPointer", new Type[2]
		{
			typeof(IntPtr),
			typeof(Type)
		}).Invoke(null, new object[2] { intptr_4, type_0 });
	}

	internal unsafe static void smethod_17()
	{
		if (bool_2)
		{
			return;
		}
		bool_2 = true;
		long num = 0L;
		Marshal.ReadIntPtr(new IntPtr(&num), 0);
		Marshal.ReadInt32(new IntPtr(&num), 0);
		Marshal.ReadInt64(new IntPtr(&num), 0);
		Marshal.WriteIntPtr(new IntPtr(&num), 0, IntPtr.Zero);
		Marshal.WriteInt32(new IntPtr(&num), 0, 0);
		Marshal.WriteInt64(new IntPtr(&num), 0, 0L);
		Marshal.Copy(new byte[1], 0, Marshal.AllocCoTaskMem(8), 1);
		smethod_15();
		if (IntPtr.Size == 4 && Type.GetType("System.Reflection.ReflectionContext", throwOnError: false) != null)
		{
			foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
			{
				if (module.ModuleName.ToLower() == "clrjit.dll")
				{
					Version version = new Version(module.FileVersionInfo.ProductMajorPart, module.FileVersionInfo.ProductMinorPart, module.FileVersionInfo.ProductBuildPart, module.FileVersionInfo.ProductPrivatePart);
					Version version2 = new Version(4, 0, 30319, 17020);
					Version version3 = new Version(4, 0, 30319, 17921);
					if (version >= version2 && version < version3)
					{
						bool_6 = true;
						break;
					}
				}
			}
		}
		Class60 @class = new Class60(assembly_0.GetManifestResourceStream("jiVjwCG457R4BMbBkf.1oj9QL2o69CZS0P1F0"));
		@class.method_0().Position = 0L;
		byte[] array = @class.method_1((int)@class.method_0().Length);
		byte[] array2 = new byte[32];
		array2[0] = 84;
		array2[0] = 117;
		array2[0] = 108;
		array2[0] = 104;
		array2[0] = 155;
		array2[1] = 162;
		array2[1] = 154;
		array2[1] = 102;
		array2[1] = 229;
		array2[1] = 102;
		array2[2] = 95;
		array2[2] = 161;
		array2[2] = 90;
		array2[3] = 142;
		array2[3] = 104;
		array2[3] = 209;
		array2[4] = 57;
		array2[4] = 78;
		array2[4] = 89;
		array2[4] = 165;
		array2[5] = 85;
		array2[5] = 201;
		array2[5] = 164;
		array2[5] = 253;
		array2[6] = 136;
		array2[6] = 147;
		array2[6] = 84;
		array2[7] = 117;
		array2[7] = 87;
		array2[7] = 190;
		array2[7] = 148;
		array2[8] = 145;
		array2[8] = 154;
		array2[8] = 110;
		array2[8] = 95;
		array2[9] = 103;
		array2[9] = 77;
		array2[9] = 127;
		array2[9] = 70;
		array2[10] = 123;
		array2[10] = 164;
		array2[10] = 7;
		array2[11] = 11;
		array2[11] = 109;
		array2[11] = 206;
		array2[11] = 133;
		array2[11] = 67;
		array2[11] = 214;
		array2[12] = 129;
		array2[12] = 110;
		array2[12] = 87;
		array2[12] = 149;
		array2[13] = 124;
		array2[13] = 27;
		array2[13] = 155;
		array2[13] = 170;
		array2[13] = 76;
		array2[13] = 89;
		array2[14] = 154;
		array2[14] = 18;
		array2[14] = 134;
		array2[14] = 149;
		array2[14] = 174;
		array2[14] = 156;
		array2[15] = 65;
		array2[15] = 121;
		array2[15] = 80;
		array2[15] = 110;
		array2[15] = 88;
		array2[16] = 75;
		array2[16] = 160;
		array2[16] = 89;
		array2[16] = 109;
		array2[16] = 118;
		array2[16] = 237;
		array2[17] = 110;
		array2[17] = 155;
		array2[17] = 133;
		array2[17] = 100;
		array2[17] = 96;
		array2[17] = 247;
		array2[18] = 150;
		array2[18] = 98;
		array2[18] = 87;
		array2[18] = 91;
		array2[19] = 125;
		array2[19] = 117;
		array2[19] = 42;
		array2[20] = 156;
		array2[20] = 104;
		array2[20] = 39;
		array2[21] = 110;
		array2[21] = 220;
		array2[21] = 227;
		array2[22] = 113;
		array2[22] = 126;
		array2[22] = 171;
		array2[23] = 74;
		array2[23] = 97;
		array2[23] = 160;
		array2[23] = 131;
		array2[23] = 33;
		array2[24] = 136;
		array2[24] = 121;
		array2[24] = 189;
		array2[24] = 86;
		array2[25] = 149;
		array2[25] = 133;
		array2[25] = 193;
		array2[25] = 166;
		array2[25] = 145;
		array2[26] = 108;
		array2[26] = 45;
		array2[26] = 165;
		array2[26] = 122;
		array2[27] = 124;
		array2[27] = 160;
		array2[27] = 125;
		array2[27] = 82;
		array2[27] = 147;
		array2[27] = 212;
		array2[28] = 139;
		array2[28] = 144;
		array2[28] = 124;
		array2[28] = 161;
		array2[28] = 87;
		array2[29] = 119;
		array2[29] = 142;
		array2[29] = 110;
		array2[29] = 113;
		array2[29] = 155;
		array2[29] = 168;
		array2[30] = 144;
		array2[30] = 70;
		array2[30] = 218;
		array2[30] = 168;
		array2[30] = 137;
		array2[30] = 215;
		array2[31] = 188;
		array2[31] = 154;
		array2[31] = 121;
		array2[31] = 140;
		array2[31] = 124;
		array2[31] = 29;
		byte[] array3 = array2;
		byte[] array4 = new byte[16];
		array4[0] = 181;
		array4[0] = 94;
		array4[0] = 125;
		array4[0] = 82;
		array4[1] = 89;
		array4[1] = 112;
		array4[1] = 47;
		array4[2] = 111;
		array4[2] = 122;
		array4[2] = 137;
		array4[2] = 50;
		array4[3] = 130;
		array4[3] = 122;
		array4[3] = 206;
		array4[4] = 110;
		array4[4] = 140;
		array4[4] = 208;
		array4[4] = 172;
		array4[5] = 170;
		array4[5] = 62;
		array4[5] = 114;
		array4[5] = 50;
		array4[6] = 106;
		array4[6] = 142;
		array4[6] = 59;
		array4[7] = 176;
		array4[7] = 31;
		array4[7] = 117;
		array4[7] = 142;
		array4[7] = 13;
		array4[8] = 154;
		array4[8] = 169;
		array4[8] = 158;
		array4[8] = 60;
		array4[8] = 60;
		array4[9] = 134;
		array4[9] = 88;
		array4[9] = 79;
		array4[10] = 93;
		array4[10] = 199;
		array4[10] = 182;
		array4[10] = 131;
		array4[10] = 223;
		array4[11] = 165;
		array4[11] = 11;
		array4[11] = 207;
		array4[11] = 157;
		array4[11] = 107;
		array4[11] = 138;
		array4[12] = 95;
		array4[12] = 88;
		array4[12] = 153;
		array4[12] = 172;
		array4[13] = 191;
		array4[13] = 74;
		array4[13] = 17;
		array4[13] = 54;
		array4[14] = 87;
		array4[14] = 102;
		array4[14] = 161;
		array4[14] = 93;
		array4[14] = 118;
		array4[14] = 233;
		array4[15] = 156;
		array4[15] = 142;
		array4[15] = 114;
		array4[15] = 140;
		array4[15] = 203;
		byte[] array5 = array4;
		Array.Reverse(array5);
		byte[] publicKeyToken = assembly_0.GetName().GetPublicKeyToken();
		if (publicKeyToken != null && publicKeyToken.Length != 0)
		{
			array5[1] = publicKeyToken[0];
			array5[3] = publicKeyToken[1];
			array5[5] = publicKeyToken[2];
			array5[7] = publicKeyToken[3];
			array5[9] = publicKeyToken[4];
			array5[11] = publicKeyToken[5];
			array5[13] = publicKeyToken[6];
			array5[15] = publicKeyToken[7];
			Array.Clear(publicKeyToken, 0, publicKeyToken.Length);
		}
		for (int i = 0; i < array5.Length; i++)
		{
			array3[i] = (byte)(array3[i] ^ array5[i]);
		}
		byte[] array6 = array;
		int num2 = array6.Length % 4;
		int num3 = array6.Length / 4;
		byte[] array7 = new byte[array6.Length];
		int num4 = array3.Length / 4;
		uint num5 = 0u;
		uint num6 = 0u;
		uint num7 = 0u;
		if (num2 > 0)
		{
			num3++;
		}
		uint num8 = 0u;
		for (int j = 0; j < num3; j++)
		{
			int num9 = j % num4;
			int num10 = j * 4;
			num8 = (uint)(num9 * 4);
			num6 = (uint)((array3[num8 + 3] << 24) | (array3[num8 + 2] << 16) | (array3[num8 + 1] << 8) | array3[num8]);
			uint num11 = 255u;
			int num12 = 0;
			if (j == num3 - 1 && num2 > 0)
			{
				num5 += num6;
				num7 = 0u;
				for (int k = 0; k < num2; k++)
				{
					if (k > 0)
					{
						num7 <<= 8;
					}
					num7 |= array6[array6.Length - (1 + k)];
				}
			}
			else
			{
				num8 = (uint)num10;
				num5 += num6;
				num7 = (uint)((array6[num8 + 3] << 24) | (array6[num8 + 2] << 16) | (array6[num8 + 1] << 8) | array6[num8]);
			}
			num5 = num5;
			uint num13 = num5;
			uint num14 = num5;
			num14 -= 2426553847u;
			num14 ^= num14 >> 24;
			num14 += num14;
			num14 ^= num14 << 25;
			num14 += 3871009369u;
			num14 ^= num14 >> 3;
			num14 += 1313165438;
			num14 = (0x8F8B62AAu ^ num14) - num14;
			num5 = num13 + (uint)(double)num14;
			if (j == num3 - 1 && num2 > 0)
			{
				uint num15 = num5 ^ num7;
				for (int l = 0; l < num2; l++)
				{
					if (l > 0)
					{
						num11 <<= 8;
						num12 += 8;
					}
					array7[num10 + l] = (byte)((num15 & num11) >> num12);
				}
			}
			else
			{
				uint num16 = num5 ^ num7;
				array7[num10] = (byte)(num16 & 0xFFu);
				array7[num10 + 1] = (byte)((num16 & 0xFF00) >> 8);
				array7[num10 + 2] = (byte)((num16 & 0xFF0000) >> 16);
				array7[num10 + 3] = (byte)((num16 & 0xFF000000u) >> 24);
			}
		}
		byte[] array8 = array7;
		int num17 = array8.Length / 8;
		fixed (byte* ptr = array8)
		{
			for (int m = 0; m < num17; m++)
			{
				*(long*)(ptr + m * 8) ^= 2109078L;
			}
		}
		@class = new Class60(new MemoryStream(array8));
		@class.method_0().Position = 0L;
		long num18 = Marshal.GetHINSTANCE(assembly_0.GetModules()[0]).ToInt64();
		int int_ = 0;
		int num19 = 0;
		if (assembly_0.Location == null || assembly_0.Location.Length == 0)
		{
			num19 = 7680;
		}
		@class.method_3();
		@class.method_3();
		@class.method_3();
		@class.method_3();
		int num20 = @class.method_3();
		int num21 = @class.method_3();
		if (num21 == 4)
		{
			SymmetricAlgorithm symmetricAlgorithm = smethod_6();
			symmetricAlgorithm.Mode = CipherMode.CBC;
			ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(array3, array5);
			Array.Clear(array3, 0, array3.Length);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			array8 = memoryStream.ToArray();
			Array.Clear(array5, 0, array5.Length);
			memoryStream.Close();
			cryptoStream.Close();
			@class.method_4();
			num20 = @class.method_3();
			num21 = @class.method_3();
		}
		if (num21 == 1)
		{
			IntPtr zero = IntPtr.Zero;
			zero = smethod_23(56u, 1, (uint)Process.GetCurrentProcess().Id);
			if (IntPtr.Size == 4)
			{
				int_4 = Marshal.GetHINSTANCE(assembly_0.GetModules()[0]).ToInt32();
			}
			long_1 = Marshal.GetHINSTANCE(assembly_0.GetModules()[0]).ToInt64();
			IntPtr intptr_ = IntPtr.Zero;
			for (int n = 0; n < num20; n++)
			{
				IntPtr intPtr = new IntPtr(long_1 + @class.method_3() - num19);
				if (smethod_22(intPtr, 4, 4, ref int_) == 0)
				{
					smethod_22(intPtr, 4, 8, ref int_);
				}
				if (IntPtr.Size == 4)
				{
					smethod_21(zero, intPtr, BitConverter.GetBytes(@class.method_3()), 4u, out intptr_);
				}
				else
				{
					smethod_21(zero, intPtr, BitConverter.GetBytes(@class.method_3()), 4u, out intptr_);
				}
				smethod_22(intPtr, 4, int_, ref int_);
			}
			while (@class.method_0().Position < @class.method_0().Length - 1L)
			{
				int num22 = @class.method_3();
				IntPtr intptr_2 = new IntPtr(long_1 + num22 - num19);
				int num23 = @class.method_3();
				if (smethod_22(intptr_2, num23 * 4, 4, ref int_) == 0)
				{
					smethod_22(intptr_2, num23 * 4, 8, ref int_);
				}
				for (int num24 = 0; num24 < num23; num24++)
				{
					Marshal.WriteInt32(new IntPtr(intptr_2.ToInt64() + num24 * 4), @class.method_3());
				}
				smethod_22(intptr_2, num23 * 4, int_, ref int_);
			}
			smethod_24(zero);
			return;
		}
		for (int num25 = 0; num25 < num20; num25++)
		{
			IntPtr intPtr2 = new IntPtr(num18 + @class.method_3() - num19);
			if (smethod_22(intPtr2, 4, 4, ref int_) == 0)
			{
				smethod_22(intPtr2, 4, 8, ref int_);
			}
			Marshal.WriteInt32(intPtr2, @class.method_3());
			smethod_22(intPtr2, 4, int_, ref int_);
		}
		hashtable_0 = new Hashtable(@class.method_3() + 1);
		Struct6 @struct = default(Struct6);
		@struct.byte_0 = new byte[1] { 42 };
		@struct.bool_0 = false;
		hashtable_0.Add(0L, @struct);
		bool flag = false;
		while (@class.method_0().Position < @class.method_0().Length - 1L)
		{
			int num26 = @class.method_3() - num19;
			int num27 = @class.method_3();
			flag = false;
			if (num27 >= 1879048192)
			{
				flag = true;
			}
			int num28 = @class.method_3();
			byte[] array9 = @class.method_1(num28);
			Struct6 struct2 = default(Struct6);
			struct2.byte_0 = array9;
			struct2.bool_0 = flag;
			hashtable_0.Add(num18 + num26, struct2);
		}
		long_0 = Marshal.GetHINSTANCE(typeof(Class57).Assembly.GetModules()[0]).ToInt64();
		if (IntPtr.Size == 4)
		{
			int_5 = Convert.ToInt32(long_0);
		}
		byte[] bytes = new byte[12]
		{
			109, 115, 99, 111, 114, 106, 105, 116, 46, 100,
			108, 108
		};
		string @string = Encoding.UTF8.GetString(bytes);
		IntPtr intPtr3 = IntPtr.Zero;
		if (intPtr3 == IntPtr.Zero)
		{
			bytes = new byte[10] { 99, 108, 114, 106, 105, 116, 46, 100, 108, 108 };
			@string = Encoding.UTF8.GetString(bytes);
			intPtr3 = LoadLibrary(@string);
		}
		byte[] bytes2 = new byte[6] { 103, 101, 116, 74, 105, 116 };
		string string2 = Encoding.UTF8.GetString(bytes2);
		IntPtr ptr2 = ((Delegate3)smethod_16(GetProcAddress(intPtr3, string2), typeof(Delegate3)))();
		long num29 = 0L;
		num29 = ((IntPtr.Size != 4) ? Marshal.ReadInt64(ptr2) : Marshal.ReadInt32(ptr2));
		Marshal.ReadIntPtr(ptr2, 0);
		delegate2_1 = smethod_13;
		IntPtr zero2 = IntPtr.Zero;
		zero2 = Marshal.GetFunctionPointerForDelegate((Delegate)delegate2_1);
		long num30 = 0L;
		num30 = ((IntPtr.Size != 4) ? Marshal.ReadInt64(new IntPtr(num29)) : Marshal.ReadInt32(new IntPtr(num29)));
		Process currentProcess = Process.GetCurrentProcess();
		try
		{
			foreach (ProcessModule module2 in currentProcess.Modules)
			{
				if (module2.ModuleName == @string && (num30 < module2.BaseAddress.ToInt64() || num30 > module2.BaseAddress.ToInt64() + module2.ModuleMemorySize) && typeof(Class57).Assembly.EntryPoint != null)
				{
					return;
				}
			}
		}
		catch
		{
		}
		try
		{
			foreach (ProcessModule module3 in currentProcess.Modules)
			{
				if (module3.BaseAddress.ToInt64() == long_0)
				{
					num19 = 0;
					break;
				}
			}
		}
		catch
		{
		}
		delegate2_0 = null;
		try
		{
			delegate2_0 = (Delegate2)smethod_16(new IntPtr(num30), typeof(Delegate2));
		}
		catch
		{
			try
			{
				Delegate @delegate = smethod_16(new IntPtr(num30), typeof(Delegate2));
				delegate2_0 = (Delegate2)Delegate.CreateDelegate(typeof(Delegate2), @delegate.Method);
			}
			catch
			{
			}
		}
		int int_2 = 0;
		if (typeof(Class57).Assembly.EntryPoint != null && typeof(Class57).Assembly.EntryPoint.GetParameters().Length == 2 && typeof(Class57).Assembly.Location != null && typeof(Class57).Assembly.Location.Length > 0)
		{
			return;
		}
		try
		{
			object value = typeof(Class57).Assembly.ManifestModule.ModuleHandle.GetType().GetField("m_ptr", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(typeof(Class57).Assembly.ManifestModule.ModuleHandle);
			if (value is IntPtr)
			{
				intptr_3 = (IntPtr)value;
			}
			if (value.GetType().ToString() == "System.Reflection.RuntimeModule")
			{
				intptr_3 = (IntPtr)value.GetType().GetField("m_pData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(value);
			}
			MemoryStream memoryStream2 = new MemoryStream();
			memoryStream2.Write(new byte[IntPtr.Size], 0, IntPtr.Size);
			if (IntPtr.Size == 4)
			{
				memoryStream2.Write(BitConverter.GetBytes(intptr_3.ToInt32()), 0, 4);
			}
			else
			{
				memoryStream2.Write(BitConverter.GetBytes(intptr_3.ToInt64()), 0, 8);
			}
			memoryStream2.Write(new byte[IntPtr.Size], 0, IntPtr.Size);
			memoryStream2.Write(new byte[IntPtr.Size], 0, IntPtr.Size);
			memoryStream2.Position = 0L;
			byte[] array10 = memoryStream2.ToArray();
			memoryStream2.Close();
			uint nativeSizeOfCode = 0u;
			try
			{
				fixed (byte* value2 = array10)
				{
					delegate2_1(new IntPtr(value2), new IntPtr(value2), new IntPtr(value2), 216669565u, new IntPtr(value2), ref nativeSizeOfCode);
				}
			}
			finally
			{
			}
		}
		catch
		{
		}
		RuntimeHelpers.PrepareDelegate(delegate2_0);
		RuntimeHelpers.PrepareMethod(delegate2_0.Method.MethodHandle);
		RuntimeHelpers.PrepareDelegate(delegate2_1);
		RuntimeHelpers.PrepareMethod(delegate2_1.Method.MethodHandle);
		byte[] array11 = null;
		array11 = ((IntPtr.Size == 4) ? new byte[30]
		{
			85, 139, 236, 139, 69, 16, 129, 120, 4, 125,
			29, 234, 12, 116, 7, 184, 182, 177, 74, 6,
			235, 5, 184, 182, 146, 64, 12, 93, 255, 224
		} : new byte[40]
		{
			72, 184, 0, 0, 0, 0, 0, 0, 0, 0,
			73, 57, 64, 8, 116, 12, 72, 184, 0, 0,
			0, 0, 0, 0, 0, 0, 255, 224, 72, 184,
			0, 0, 0, 0, 0, 0, 0, 0, 255, 224
		});
		IntPtr intPtr4 = smethod_20(IntPtr.Zero, (uint)array11.Length, 4096u, 64u);
		byte[] array12 = array11;
		byte[] array13 = null;
		byte[] array14 = null;
		byte[] array15 = null;
		if (IntPtr.Size == 4)
		{
			array15 = BitConverter.GetBytes(intptr_3.ToInt32());
			array13 = BitConverter.GetBytes(zero2.ToInt32());
			array14 = BitConverter.GetBytes(Convert.ToInt32(num30));
		}
		else
		{
			array15 = BitConverter.GetBytes(intptr_3.ToInt64());
			array13 = BitConverter.GetBytes(zero2.ToInt64());
			array14 = BitConverter.GetBytes(num30);
		}
		if (IntPtr.Size == 4)
		{
			array12[9] = array15[0];
			array12[10] = array15[1];
			array12[11] = array15[2];
			array12[12] = array15[3];
			array12[16] = array14[0];
			array12[17] = array14[1];
			array12[18] = array14[2];
			array12[19] = array14[3];
			array12[23] = array13[0];
			array12[24] = array13[1];
			array12[25] = array13[2];
			array12[26] = array13[3];
		}
		else
		{
			array12[2] = array15[0];
			array12[3] = array15[1];
			array12[4] = array15[2];
			array12[5] = array15[3];
			array12[6] = array15[4];
			array12[7] = array15[5];
			array12[8] = array15[6];
			array12[9] = array15[7];
			array12[18] = array14[0];
			array12[19] = array14[1];
			array12[20] = array14[2];
			array12[21] = array14[3];
			array12[22] = array14[4];
			array12[23] = array14[5];
			array12[24] = array14[6];
			array12[25] = array14[7];
			array12[30] = array13[0];
			array12[31] = array13[1];
			array12[32] = array13[2];
			array12[33] = array13[3];
			array12[34] = array13[4];
			array12[35] = array13[5];
			array12[36] = array13[6];
			array12[37] = array13[7];
		}
		Marshal.Copy(array12, 0, intPtr4, array12.Length);
		bool_1 = false;
		smethod_22(new IntPtr(num29), IntPtr.Size, 64, ref int_2);
		Marshal.WriteIntPtr(new IntPtr(num29), intPtr4);
		smethod_22(new IntPtr(num29), IntPtr.Size, int_2, ref int_2);
	}

	internal static object smethod_18(Assembly assembly_1)
	{
		try
		{
			if (File.Exists(assembly_1.Location))
			{
				return assembly_1.Location;
			}
		}
		catch
		{
		}
		try
		{
			if (File.Exists(assembly_1.GetName().CodeBase.ToString().Replace("file:///", "")))
			{
				return assembly_1.GetName().CodeBase.ToString().Replace("file:///", "");
			}
		}
		catch
		{
		}
		try
		{
			if (File.Exists(assembly_1.GetType().GetProperty("Location").GetValue(assembly_1, new object[0])
				.ToString()))
			{
				return assembly_1.GetType().GetProperty("Location").GetValue(assembly_1, new object[0])
					.ToString();
			}
		}
		catch
		{
		}
		return "";
	}

	[DllImport("kernel32")]
	public static extern IntPtr LoadLibrary(string string_1);

	[DllImport("kernel32", CharSet = CharSet.Ansi)]
	public static extern IntPtr GetProcAddress(IntPtr intptr_4, string string_1);

	private static IntPtr smethod_19(IntPtr intptr_4, string string_1, uint uint_1)
	{
		if (delegate4_0 == null)
		{
			delegate4_0 = (Delegate4)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Find ".Trim() + "ResourceA"), typeof(Delegate4));
		}
		return delegate4_0(intptr_4, string_1, uint_1);
	}

	private static IntPtr smethod_20(IntPtr intptr_4, uint uint_1, uint uint_2, uint uint_3)
	{
		if (delegate5_0 == null)
		{
			delegate5_0 = (Delegate5)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Virtual ".Trim() + "Alloc"), typeof(Delegate5));
		}
		return delegate5_0(intptr_4, uint_1, uint_2, uint_3);
	}

	private static int smethod_21(IntPtr intptr_4, IntPtr intptr_5, [In][Out] byte[] byte_2, uint uint_1, out IntPtr intptr_6)
	{
		if (delegate6_0 == null)
		{
			delegate6_0 = (Delegate6)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Write ".Trim() + "Process ".Trim() + "Memory"), typeof(Delegate6));
		}
		return delegate6_0(intptr_4, intptr_5, byte_2, uint_1, out intptr_6);
	}

	private static int smethod_22(IntPtr intptr_4, int int_6, int int_7, ref int int_8)
	{
		if (delegate7_0 == null)
		{
			delegate7_0 = (Delegate7)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Virtual ".Trim() + "Protect"), typeof(Delegate7));
		}
		return delegate7_0(intptr_4, int_6, int_7, ref int_8);
	}

	private static IntPtr smethod_23(uint uint_1, int int_6, uint uint_2)
	{
		if (delegate8_0 == null)
		{
			delegate8_0 = (Delegate8)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Open ".Trim() + "Process"), typeof(Delegate8));
		}
		return delegate8_0(uint_1, int_6, uint_2);
	}

	private static int smethod_24(IntPtr intptr_4)
	{
		if (delegate9_0 == null)
		{
			delegate9_0 = (Delegate9)Marshal.GetDelegateForFunctionPointer(GetProcAddress(smethod_25(), "Close ".Trim() + "Handle"), typeof(Delegate9));
		}
		return delegate9_0(intptr_4);
	}

	[SpecialName]
	private static IntPtr smethod_25()
	{
		if (intptr_1 == IntPtr.Zero)
		{
			intptr_1 = LoadLibrary("kernel ".Trim() + "32.dll");
		}
		return intptr_1;
	}

	private static byte[] smethod_26(string string_1)
	{
		using FileStream fileStream = new FileStream(string_1, FileMode.Open, FileAccess.Read, FileShare.Read);
		int num = 0;
		int num2 = (int)fileStream.Length;
		byte[] array = new byte[num2];
		while (num2 > 0)
		{
			int num3 = fileStream.Read(array, num, num2);
			num += num3;
			num2 -= num3;
		}
		return array;
	}

	internal static byte[] smethod_27(Stream stream_0)
	{
		return ((MemoryStream)stream_0).ToArray();
	}

	private static byte[] smethod_28(byte[] byte_2)
	{
		Stream stream = new MemoryStream();
		SymmetricAlgorithm symmetricAlgorithm = smethod_6();
		symmetricAlgorithm.Key = new byte[32]
		{
			147, 143, 54, 53, 218, 23, 166, 148, 64, 3,
			156, 72, 45, 198, 76, 97, 214, 130, 186, 162,
			73, 207, 127, 130, 146, 177, 77, 138, 152, 16,
			175, 30
		};
		symmetricAlgorithm.IV = new byte[16]
		{
			56, 14, 218, 187, 92, 84, 171, 150, 40, 205,
			57, 6, 50, 199, 139, 41
		};
		CryptoStream cryptoStream = new CryptoStream(stream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(byte_2, 0, byte_2.Length);
		cryptoStream.Close();
		return smethod_27(stream);
	}

	private byte[] method_2()
	{
		return null;
	}

	private byte[] method_3()
	{
		return null;
	}

	private byte[] method_4()
	{
		return null;
	}

	private byte[] method_5()
	{
		return null;
	}

	private byte[] method_6()
	{
		return null;
	}

	private byte[] method_7()
	{
		return null;
	}

	internal byte[] method_8()
	{
		_ = "{11111-22222-40001-00001}".Length;
		return new byte[2] { 1, 2 };
	}

	internal byte[] method_9()
	{
		_ = "{11111-22222-40001-00002}".Length;
		return new byte[2] { 1, 2 };
	}

	internal byte[] method_10()
	{
		return null;
	}

	internal byte[] method_11()
	{
		return null;
	}

	internal static object smethod_29(Class60 class60_0)
	{
		return class60_0.method_0();
	}

	internal static void smethod_30(Stream stream_0, long long_2)
	{
		stream_0.Position = long_2;
	}

	internal static long smethod_31(Stream stream_0)
	{
		return stream_0.Length;
	}

	internal static object smethod_32(Class60 class60_0, int int_6)
	{
		return class60_0.method_1(int_6);
	}

	internal static void smethod_33(Class60 class60_0)
	{
		class60_0.method_4();
	}

	internal static void smethod_34(Array array_0)
	{
		Array.Reverse(array_0);
	}

	internal static object smethod_35(Assembly assembly_1)
	{
		return assembly_1.GetName();
	}

	internal static object smethod_36(AssemblyName assemblyName_0)
	{
		return assemblyName_0.GetPublicKeyToken();
	}

	internal static object smethod_37()
	{
		return smethod_6();
	}

	internal static void smethod_38(SymmetricAlgorithm symmetricAlgorithm_0, CipherMode cipherMode_0)
	{
		symmetricAlgorithm_0.Mode = cipherMode_0;
	}

	internal static object smethod_39(SymmetricAlgorithm symmetricAlgorithm_0, byte[] byte_2, byte[] byte_3)
	{
		return symmetricAlgorithm_0.CreateDecryptor(byte_2, byte_3);
	}

	internal static object smethod_40()
	{
		return new MemoryStream();
	}

	internal static void smethod_41(Stream stream_0, byte[] byte_2, int int_6, int int_7)
	{
		stream_0.Write(byte_2, int_6, int_7);
	}

	internal static void smethod_42(CryptoStream cryptoStream_0)
	{
		cryptoStream_0.FlushFinalBlock();
	}

	internal static object smethod_43(Stream stream_0)
	{
		return smethod_27(stream_0);
	}

	internal static void smethod_44(Stream stream_0)
	{
		stream_0.Close();
	}

	internal static object smethod_45(Assembly assembly_1)
	{
		return assembly_1.EntryPoint;
	}

	internal static bool smethod_46(MethodInfo methodInfo_0, MethodInfo methodInfo_1)
	{
		return methodInfo_0 == methodInfo_1;
	}

	internal static bool smethod_47()
	{
		return (object)null == null;
	}

	internal static object smethod_48()
	{
		return null;
	}

	internal static bool smethod_49()
	{
		return (object)null == null;
	}

	internal static object smethod_50()
	{
		return null;
	}

	static int smethod_51()
	{
		return 1;
	}

	internal static IntPtr smethod_52(IntPtr intptr_4, int int_6)
	{
		return Marshal.ReadIntPtr(intptr_4, int_6);
	}

	internal static int smethod_53(IntPtr intptr_4, int int_6)
	{
		return Marshal.ReadInt32(intptr_4, int_6);
	}

	internal static long smethod_54(IntPtr intptr_4, int int_6)
	{
		return Marshal.ReadInt64(intptr_4, int_6);
	}

	internal static void smethod_55(IntPtr intptr_4, int int_6, IntPtr intptr_5)
	{
		Marshal.WriteIntPtr(intptr_4, int_6, intptr_5);
	}

	internal static void smethod_56(IntPtr intptr_4, int int_6, int int_7)
	{
		Marshal.WriteInt32(intptr_4, int_6, int_7);
	}

	internal static void smethod_57(IntPtr intptr_4, int int_6, long long_2)
	{
		Marshal.WriteInt64(intptr_4, int_6, long_2);
	}

	internal static IntPtr smethod_58(int int_6)
	{
		return Marshal.AllocCoTaskMem(int_6);
	}

	internal static void smethod_59(byte[] byte_2, int int_6, IntPtr intptr_4, int int_7)
	{
		Marshal.Copy(byte_2, int_6, intptr_4, int_7);
	}

	internal static void smethod_60()
	{
		smethod_15();
	}

	internal static object smethod_61()
	{
		return Process.GetCurrentProcess();
	}

	internal static object smethod_62(Process process_0)
	{
		return process_0.MainModule;
	}

	internal static IntPtr smethod_63(ProcessModule processModule_0)
	{
		return processModule_0.BaseAddress;
	}

	internal static IntPtr smethod_64(IntPtr intptr_4, string string_1, uint uint_1)
	{
		return smethod_19(intptr_4, string_1, uint_1);
	}

	internal static bool smethod_65(IntPtr intptr_4, IntPtr intptr_5)
	{
		return intptr_4 != intptr_5;
	}

	internal static void smethod_66()
	{
	}

	internal static int smethod_67()
	{
		return IntPtr.Size;
	}

	internal static Type smethod_68(string string_1, bool bool_7)
	{
		return Type.GetType(string_1, bool_7);
	}

	internal static bool smethod_69(Type type_0, Type type_1)
	{
		return type_0 != type_1;
	}

	internal static object smethod_70(Process process_0)
	{
		return process_0.Modules;
	}

	internal static object smethod_71(ReadOnlyCollectionBase readOnlyCollectionBase_0)
	{
		return readOnlyCollectionBase_0.GetEnumerator();
	}

	internal static object smethod_72(IEnumerator ienumerator_0)
	{
		return ienumerator_0.Current;
	}

	internal static object smethod_73(ProcessModule processModule_0)
	{
		return processModule_0.ModuleName;
	}

	internal static object smethod_74(string string_1)
	{
		return string_1.ToLower();
	}

	internal static bool smethod_75(string string_1, string string_2)
	{
		return string_1 == string_2;
	}

	internal static object smethod_76(ProcessModule processModule_0)
	{
		return processModule_0.FileVersionInfo;
	}

	internal static int smethod_77(FileVersionInfo fileVersionInfo_0)
	{
		return fileVersionInfo_0.ProductMajorPart;
	}

	internal static int smethod_78(FileVersionInfo fileVersionInfo_0)
	{
		return fileVersionInfo_0.ProductMinorPart;
	}

	internal static int smethod_79(FileVersionInfo fileVersionInfo_0)
	{
		return fileVersionInfo_0.ProductBuildPart;
	}

	internal static int smethod_80(FileVersionInfo fileVersionInfo_0)
	{
		return fileVersionInfo_0.ProductPrivatePart;
	}

	internal static bool smethod_81(Version version_0, Version version_1)
	{
		return version_0 >= version_1;
	}

	internal static bool smethod_82(Version version_0, Version version_1)
	{
		return version_0 < version_1;
	}

	internal static bool smethod_83(IEnumerator ienumerator_0)
	{
		return ienumerator_0.MoveNext();
	}

	internal static void smethod_84(IDisposable idisposable_0)
	{
		idisposable_0.Dispose();
	}

	internal static object smethod_85(Assembly assembly_1, string string_1)
	{
		return assembly_1.GetManifestResourceStream(string_1);
	}

	internal static object smethod_86(Class60 class60_0)
	{
		return class60_0.method_0();
	}

	internal static void smethod_87(Stream stream_0, long long_2)
	{
		stream_0.Position = long_2;
	}

	internal static long smethod_88(Stream stream_0)
	{
		return stream_0.Length;
	}

	internal static object smethod_89(Class60 class60_0, int int_6)
	{
		return class60_0.method_1(int_6);
	}

	internal static void smethod_90(Array array_0)
	{
		Array.Reverse(array_0);
	}

	internal static object smethod_91(Assembly assembly_1)
	{
		return assembly_1.GetName();
	}

	internal static object smethod_92(AssemblyName assemblyName_0)
	{
		return assemblyName_0.GetPublicKeyToken();
	}

	internal static void smethod_93(Array array_0, int int_6, int int_7)
	{
		Array.Clear(array_0, int_6, int_7);
	}

	internal static object smethod_94(Assembly assembly_1)
	{
		return assembly_1.GetModules();
	}

	internal static IntPtr smethod_95(Module module_0)
	{
		return Marshal.GetHINSTANCE(module_0);
	}

	internal static object smethod_96(Assembly assembly_1)
	{
		return assembly_1.Location;
	}

	internal static int smethod_97(string string_1)
	{
		return string_1.Length;
	}

	internal static int smethod_98(Class60 class60_0)
	{
		return class60_0.method_3();
	}

	internal static object smethod_99()
	{
		return smethod_6();
	}

	internal static void smethod_100(SymmetricAlgorithm symmetricAlgorithm_0, CipherMode cipherMode_0)
	{
		symmetricAlgorithm_0.Mode = cipherMode_0;
	}

	internal static object smethod_101(SymmetricAlgorithm symmetricAlgorithm_0, byte[] byte_2, byte[] byte_3)
	{
		return symmetricAlgorithm_0.CreateDecryptor(byte_2, byte_3);
	}

	internal static void smethod_102(Stream stream_0, byte[] byte_2, int int_6, int int_7)
	{
		stream_0.Write(byte_2, int_6, int_7);
	}

	internal static void smethod_103(CryptoStream cryptoStream_0)
	{
		cryptoStream_0.FlushFinalBlock();
	}

	internal static object smethod_104(MemoryStream memoryStream_0)
	{
		return memoryStream_0.ToArray();
	}

	internal static void smethod_105(Stream stream_0)
	{
		stream_0.Close();
	}

	internal static void smethod_106(Class60 class60_0)
	{
		class60_0.method_4();
	}

	internal static int smethod_107(Process process_0)
	{
		return process_0.Id;
	}

	internal static IntPtr smethod_108(uint uint_1, int int_6, uint uint_2)
	{
		return smethod_23(uint_1, int_6, uint_2);
	}

	internal static object smethod_109(int int_6)
	{
		return BitConverter.GetBytes(int_6);
	}

	internal static long smethod_110(Stream stream_0)
	{
		return stream_0.Position;
	}

	internal static void smethod_111(IntPtr intptr_4, int int_6)
	{
		Marshal.WriteInt32(intptr_4, int_6);
	}

	internal static int smethod_112(IntPtr intptr_4)
	{
		return smethod_24(intptr_4);
	}

	internal static void smethod_113(Hashtable hashtable_1, object object_3, object object_4)
	{
		hashtable_1.Add(object_3, object_4);
	}

	internal static Type smethod_114(RuntimeTypeHandle runtimeTypeHandle_0)
	{
		return Type.GetTypeFromHandle(runtimeTypeHandle_0);
	}

	internal static int smethod_115(long long_2)
	{
		return Convert.ToInt32(long_2);
	}

	internal static object smethod_116()
	{
		return Encoding.UTF8;
	}

	internal static object smethod_117(Encoding encoding_0, byte[] byte_2)
	{
		return encoding_0.GetString(byte_2);
	}

	internal static bool smethod_118(IntPtr intptr_4, IntPtr intptr_5)
	{
		return intptr_4 == intptr_5;
	}

	internal static object smethod_119(IntPtr intptr_4, Type type_0)
	{
		return smethod_16(intptr_4, type_0);
	}

	internal static IntPtr smethod_120(Delegate3 delegate3_0)
	{
		return delegate3_0();
	}

	internal static int smethod_121(IntPtr intptr_4)
	{
		return Marshal.ReadInt32(intptr_4);
	}

	internal static long smethod_122(IntPtr intptr_4)
	{
		return Marshal.ReadInt64(intptr_4);
	}

	internal static IntPtr smethod_123(Delegate delegate_0)
	{
		return Marshal.GetFunctionPointerForDelegate(delegate_0);
	}

	internal static int smethod_124(ProcessModule processModule_0)
	{
		return processModule_0.ModuleMemorySize;
	}

	internal static object smethod_125(Assembly assembly_1)
	{
		return assembly_1.EntryPoint;
	}

	internal static bool smethod_126(MethodInfo methodInfo_0, MethodInfo methodInfo_1)
	{
		return methodInfo_0 != methodInfo_1;
	}

	internal static object smethod_127(Delegate delegate_0)
	{
		return delegate_0.Method;
	}

	internal static object smethod_128(Type type_0, MethodInfo methodInfo_0)
	{
		return Delegate.CreateDelegate(type_0, methodInfo_0);
	}

	internal static object smethod_129(MethodBase methodBase_0)
	{
		return methodBase_0.GetParameters();
	}

	internal static object smethod_130(Assembly assembly_1)
	{
		return assembly_1.ManifestModule;
	}

	internal static ModuleHandle smethod_131(Module module_0)
	{
		return module_0.ModuleHandle;
	}

	internal static Type smethod_132(object object_3)
	{
		return object_3.GetType();
	}

	internal static object smethod_133(FieldInfo fieldInfo_0, object object_3)
	{
		return fieldInfo_0.GetValue(object_3);
	}

	internal static object smethod_134(long long_2)
	{
		return BitConverter.GetBytes(long_2);
	}

	internal static void smethod_135(Delegate delegate_0)
	{
		RuntimeHelpers.PrepareDelegate(delegate_0);
	}

	internal static RuntimeMethodHandle smethod_136(MethodBase methodBase_0)
	{
		return methodBase_0.MethodHandle;
	}

	internal static void smethod_137(RuntimeMethodHandle runtimeMethodHandle_0)
	{
		RuntimeHelpers.PrepareMethod(runtimeMethodHandle_0);
	}

	internal static void smethod_138(Array array_0, RuntimeFieldHandle runtimeFieldHandle_0)
	{
		RuntimeHelpers.InitializeArray(array_0, runtimeFieldHandle_0);
	}

	internal static IntPtr smethod_139(IntPtr intptr_4, uint uint_1, uint uint_2, uint uint_3)
	{
		return smethod_20(intptr_4, uint_1, uint_2, uint_3);
	}

	internal static void smethod_140(IntPtr intptr_4, IntPtr intptr_5)
	{
		Marshal.WriteIntPtr(intptr_4, intptr_5);
	}

	internal static bool smethod_141()
	{
		return (object)null == null;
	}

	internal static object smethod_142()
	{
		return null;
	}
}
