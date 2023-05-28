using System;
using System.Linq;
using System.Runtime.InteropServices;

internal class Class41
{
	public struct Struct0
	{
		public uint uint_0;

		public uint uint_1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] byte_0;

		public uint uint_2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] byte_1;

		public uint uint_3;
	}

	public struct Struct1
	{
		public uint uint_0;

		private readonly Struct0 struct0_0;
	}

	private enum Enum0
	{

	}

	public static void KillTcpConnectionForProcess(int processId)
	{
		int dwOutBufLen = 0;
		IntPtr intPtr = Marshal.AllocHGlobal(0);
		Struct0[] array;
		try
		{
			if (GetExtendedTcpTable(intPtr, ref dwOutBufLen, sort: true, 2, (Enum0)5) != 0)
			{
				return;
			}
			Struct1 @struct = (Struct1)Marshal.PtrToStructure(intPtr, typeof(Struct1));
			IntPtr intPtr2 = (IntPtr)((long)intPtr + Marshal.SizeOf(@struct.uint_0));
			array = new Struct0[@struct.uint_0];
			for (int i = 0; i < @struct.uint_0; i++)
			{
				Struct0 structure = (array[i] = (Struct0)Marshal.PtrToStructure(intPtr2, typeof(Struct0)));
				intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(structure));
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		Struct0 structure2 = array.FirstOrDefault((Struct0 t) => t.uint_3 == processId);
		structure2.uint_0 = 12u;
		IntPtr intPtr3 = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure2));
		Marshal.StructureToPtr(structure2, intPtr3, fDeleteOld: false);
		SetTcpEntry(intPtr3);
	}

	[DllImport("iphlpapi.dll", SetLastError = true)]
	private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion, Enum0 tblClass, uint reserved = 0u);

	[DllImport("iphlpapi.dll")]
	private static extern int SetTcpEntry(IntPtr pTcprow);
}
