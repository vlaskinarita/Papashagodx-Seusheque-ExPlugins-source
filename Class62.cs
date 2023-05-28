using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

internal class Class62
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Struct7
	{
		[FieldOffset(0)]
		public byte byte_0;

		[FieldOffset(0)]
		public sbyte sbyte_0;

		[FieldOffset(0)]
		public ushort ushort_0;

		[FieldOffset(0)]
		public short short_0;

		[FieldOffset(0)]
		public uint uint_0;

		[FieldOffset(0)]
		public int int_0;
	}

	private class Class75 : Class74
	{
		public Struct7 struct7_0;

		public Enum3 enum3_0;

		internal override void vmethod_9(Class73 class73_0)
		{
			struct7_0 = ((Class75)class73_0).struct7_0;
			enum3_0 = ((Class75)class73_0).enum3_0;
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_9(class73_0);
		}

		public Class75(bool bool_0)
		{
			enum6_0 = (Enum6)1;
			if (bool_0)
			{
				struct7_0.int_0 = 1;
			}
			else
			{
				struct7_0.int_0 = 0;
			}
			enum3_0 = (Enum3)11;
		}

		public Class75(Class75 class75_0)
		{
			enum6_0 = class75_0.enum6_0;
			struct7_0.int_0 = class75_0.struct7_0.int_0;
			enum3_0 = class75_0.enum3_0;
		}

		public override Class74 vmethod_71()
		{
			return new Class75(this);
		}

		public Class75(int int_0)
		{
			enum6_0 = (Enum6)1;
			struct7_0.int_0 = int_0;
			enum3_0 = (Enum3)5;
		}

		public Class75(uint uint_0)
		{
			enum6_0 = (Enum6)1;
			struct7_0.uint_0 = uint_0;
			enum3_0 = (Enum3)6;
		}

		public Class75(int int_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)1;
			struct7_0.int_0 = int_0;
			enum3_0 = enum3_1;
		}

		public Class75(uint uint_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)1;
			struct7_0.uint_0 = uint_0;
			enum3_0 = enum3_1;
		}

		public override bool vmethod_10()
		{
			switch (enum3_0)
			{
			default:
				return struct7_0.uint_0 == 0;
			case (Enum3)1:
			case (Enum3)3:
			case (Enum3)5:
			case (Enum3)7:
			case (Enum3)11:
			case (Enum3)15:
				return struct7_0.int_0 == 0;
			}
		}

		public override bool vmethod_11()
		{
			return !vmethod_10();
		}

		public override Class73 vmethod_12(Enum3 enum3_1)
		{
			return enum3_1 switch
			{
				(Enum3)1 => vmethod_14(), 
				(Enum3)2 => vmethod_15(), 
				(Enum3)3 => vmethod_16(), 
				(Enum3)4 => vmethod_17(), 
				(Enum3)5 => vmethod_18(), 
				(Enum3)6 => vmethod_19(), 
				(Enum3)11 => vmethod_13(), 
				(Enum3)15 => method_6(), 
				(Enum3)16 => vmethod_71(), 
				_ => throw new Exception(((Enum7)4).ToString()), 
			};
		}

		internal override object vmethod_4(Type type_0)
		{
			if (type_0 != null && type_0.IsByRef)
			{
				type_0 = type_0.GetElementType();
			}
			if (type_0 != null && Nullable.GetUnderlyingType(type_0) != null)
			{
				type_0 = Nullable.GetUnderlyingType(type_0);
			}
			if (!(type_0 == null) && !(type_0 == typeof(object)))
			{
				if (!(type_0 == typeof(int)))
				{
					if (!(type_0 == typeof(uint)))
					{
						if (type_0 == typeof(short))
						{
							return struct7_0.short_0;
						}
						if (!(type_0 == typeof(ushort)))
						{
							if (type_0 == typeof(byte))
							{
								return struct7_0.byte_0;
							}
							if (type_0 == typeof(sbyte))
							{
								return struct7_0.sbyte_0;
							}
							if (type_0 == typeof(bool))
							{
								return !vmethod_10();
							}
							if (type_0 == typeof(long))
							{
								return (long)struct7_0.int_0;
							}
							if (!(type_0 == typeof(ulong)))
							{
								if (type_0 == typeof(char))
								{
									return (char)struct7_0.int_0;
								}
								if (!(type_0 == typeof(IntPtr)))
								{
									if (type_0 == typeof(UIntPtr))
									{
										return new UIntPtr(struct7_0.uint_0);
									}
									if (type_0.IsEnum)
									{
										return method_5(type_0);
									}
									throw new Exception1();
								}
								return new IntPtr(struct7_0.int_0);
							}
							return (ulong)struct7_0.uint_0;
						}
						return struct7_0.ushort_0;
					}
					return struct7_0.uint_0;
				}
				return struct7_0.int_0;
			}
			return enum3_0 switch
			{
				(Enum3)1 => struct7_0.sbyte_0, 
				(Enum3)2 => struct7_0.byte_0, 
				(Enum3)3 => struct7_0.short_0, 
				(Enum3)4 => struct7_0.ushort_0, 
				(Enum3)5 => struct7_0.int_0, 
				(Enum3)6 => struct7_0.uint_0, 
				(Enum3)7 => (long)struct7_0.int_0, 
				(Enum3)8 => (ulong)struct7_0.uint_0, 
				(Enum3)11 => vmethod_11(), 
				(Enum3)15 => (char)struct7_0.int_0, 
				_ => struct7_0.int_0, 
			};
		}

		internal object method_5(Type type_0)
		{
			Type underlyingType = Enum.GetUnderlyingType(type_0);
			if (underlyingType == typeof(int))
			{
				return Enum.ToObject(type_0, struct7_0.int_0);
			}
			if (!(underlyingType == typeof(uint)))
			{
				if (underlyingType == typeof(short))
				{
					return Enum.ToObject(type_0, struct7_0.short_0);
				}
				if (!(underlyingType == typeof(ushort)))
				{
					if (!(underlyingType == typeof(byte)))
					{
						if (underlyingType == typeof(sbyte))
						{
							return Enum.ToObject(type_0, struct7_0.sbyte_0);
						}
						if (underlyingType == typeof(long))
						{
							return Enum.ToObject(type_0, (long)struct7_0.int_0);
						}
						if (underlyingType == typeof(ulong))
						{
							return Enum.ToObject(type_0, (ulong)struct7_0.uint_0);
						}
						if (underlyingType == typeof(char))
						{
							return Enum.ToObject(type_0, (ushort)struct7_0.int_0);
						}
						return Enum.ToObject(type_0, struct7_0.int_0);
					}
					return Enum.ToObject(type_0, struct7_0.byte_0);
				}
				return Enum.ToObject(type_0, struct7_0.ushort_0);
			}
			return Enum.ToObject(type_0, struct7_0.uint_0);
		}

		public override Class75 vmethod_13()
		{
			return new Class75((!vmethod_10()) ? 1 : 0);
		}

		internal override bool vmethod_6()
		{
			return vmethod_11();
		}

		public override Class75 vmethod_14()
		{
			return new Class75(struct7_0.sbyte_0, (Enum3)1);
		}

		public Class75 method_6()
		{
			return new Class75(struct7_0.int_0, (Enum3)15);
		}

		public override Class75 vmethod_15()
		{
			return new Class75((uint)struct7_0.byte_0, (Enum3)2);
		}

		public override Class75 vmethod_16()
		{
			return new Class75(struct7_0.short_0, (Enum3)3);
		}

		public override Class75 vmethod_17()
		{
			return new Class75((uint)struct7_0.ushort_0, (Enum3)4);
		}

		public override Class75 vmethod_18()
		{
			return new Class75(struct7_0.int_0, (Enum3)5);
		}

		public override Class75 vmethod_19()
		{
			return new Class75(struct7_0.uint_0, (Enum3)6);
		}

		public override Class76 vmethod_20()
		{
			return new Class76(struct7_0.int_0, (Enum3)7);
		}

		public override Class76 vmethod_21()
		{
			return new Class76((ulong)struct7_0.uint_0, (Enum3)8);
		}

		public override Class75 vmethod_22()
		{
			return vmethod_14();
		}

		public override Class75 vmethod_23()
		{
			return vmethod_16();
		}

		public override Class75 vmethod_24()
		{
			return vmethod_18();
		}

		public override Class76 vmethod_25()
		{
			return vmethod_20();
		}

		public override Class75 vmethod_26()
		{
			return vmethod_15();
		}

		public override Class75 vmethod_27()
		{
			return vmethod_17();
		}

		public override Class75 vmethod_28()
		{
			return vmethod_19();
		}

		public override Class76 vmethod_29()
		{
			return vmethod_21();
		}

		public override Class75 vmethod_30()
		{
			return new Class75(checked((sbyte)struct7_0.int_0), (Enum3)1);
		}

		public override Class75 vmethod_31()
		{
			return new Class75(checked((sbyte)struct7_0.uint_0), (Enum3)1);
		}

		public override Class75 vmethod_32()
		{
			return new Class75(checked((short)struct7_0.int_0), (Enum3)3);
		}

		public override Class75 usfdqHavse()
		{
			return new Class75(checked((short)struct7_0.uint_0), (Enum3)3);
		}

		public override Class75 vmethod_33()
		{
			return new Class75(struct7_0.int_0, (Enum3)5);
		}

		public override Class75 vmethod_34()
		{
			return new Class75(checked((int)struct7_0.uint_0), (Enum3)5);
		}

		public override Class76 vmethod_35()
		{
			return new Class76(struct7_0.int_0, (Enum3)7);
		}

		public override Class76 vmethod_36()
		{
			return new Class76(struct7_0.uint_0, (Enum3)7);
		}

		public override Class75 vmethod_37()
		{
			return new Class75(checked((byte)struct7_0.int_0), (Enum3)2);
		}

		public override Class75 vmethod_38()
		{
			return new Class75(checked((byte)struct7_0.uint_0), (Enum3)2);
		}

		public override Class75 vmethod_39()
		{
			return new Class75(checked((ushort)struct7_0.int_0), (Enum3)4);
		}

		public override Class75 vmethod_40()
		{
			return new Class75(checked((ushort)struct7_0.uint_0), (Enum3)4);
		}

		public override Class75 vmethod_41()
		{
			return new Class75(checked((uint)struct7_0.int_0), (Enum3)6);
		}

		public override Class75 vmethod_42()
		{
			return new Class75(struct7_0.uint_0, (Enum3)6);
		}

		public override Class76 vmethod_43()
		{
			return new Class76(checked((ulong)struct7_0.int_0), (Enum3)8);
		}

		public override Class76 vmethod_44()
		{
			return new Class76((ulong)struct7_0.uint_0, (Enum3)8);
		}

		public override Class78 vmethod_45()
		{
			return new Class78(struct7_0.int_0);
		}

		public override Class78 vmethod_46()
		{
			return new Class78((double)struct7_0.int_0);
		}

		public override Class78 vmethod_47()
		{
			return new Class78((double)struct7_0.uint_0);
		}

		public override Class77 vmethod_48()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_25().struct8_0.long_0);
			}
			return new Class77(vmethod_24().struct7_0.int_0);
		}

		public override Class77 vmethod_49()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_29().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_28().struct7_0.uint_0);
		}

		public override Class77 vmethod_50()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_35().struct8_0.long_0);
			}
			return new Class77(vmethod_33().struct7_0.int_0);
		}

		public override Class77 vmethod_51()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_43().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_41().struct7_0.uint_0);
		}

		public override Class77 vmethod_52()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_36().struct8_0.long_0);
			}
			return new Class77(vmethod_34().struct7_0.int_0);
		}

		public override Class77 vmethod_53()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_44().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_42().struct7_0.uint_0);
		}

		public override Class73 vmethod_54()
		{
			switch (enum3_0)
			{
			default:
				return new Class75((int)(0L - (long)struct7_0.uint_0));
			case (Enum3)1:
			case (Enum3)3:
			case (Enum3)5:
			case (Enum3)11:
			case (Enum3)15:
				return new Class75(-struct7_0.int_0);
			}
		}

		public override Class73 Add(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.int_0 + ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).Add(this);
		}

		public override Class73 vmethod_55(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(checked(struct7_0.int_0 + ((Class75)class73_0).struct7_0.int_0));
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_55(this);
		}

		public override Class73 vmethod_56(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(checked(struct7_0.uint_0 + ((Class75)class73_0).struct7_0.uint_0));
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_56(this);
		}

		public override Class73 vmethod_57(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.int_0 - ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_7(this);
		}

		public override Class73 vmethod_58(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).method_8(this);
				}
				throw new Exception1();
			}
			return new Class75(checked(struct7_0.int_0 - ((Class75)class73_0).struct7_0.int_0));
		}

		public override Class73 vmethod_59(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(checked(struct7_0.uint_0 - ((Class75)class73_0).struct7_0.uint_0));
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_9(this);
		}

		public override Class73 vmethod_60(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).vmethod_60(this);
				}
				throw new Exception1();
			}
			return new Class75(struct7_0.int_0 * ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_61(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(checked(struct7_0.int_0 * ((Class75)class73_0).struct7_0.int_0));
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_61(this);
		}

		public override Class73 vmethod_62(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				return ((Class77)class73_0).vmethod_62(this);
			}
			return new Class75(checked(struct7_0.uint_0 * ((Class75)class73_0).struct7_0.uint_0));
		}

		public override Class73 vmethod_63(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.int_0 / ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_10(this);
		}

		public override Class73 vmethod_64(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.uint_0 / ((Class75)class73_0).struct7_0.uint_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_11(this);
		}

		public override Class73 vmethod_65(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).method_12(this);
				}
				throw new Exception1();
			}
			return new Class75(struct7_0.int_0 % ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_66(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).yIwByEsukh(this);
				}
				throw new Exception1();
			}
			return new Class75(struct7_0.uint_0 % ((Class75)class73_0).struct7_0.uint_0);
		}

		public override Class73 vmethod_67(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).vmethod_67(this);
				}
				throw new Exception1();
			}
			return new Class75(struct7_0.int_0 & ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_68(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				return ((Class77)class73_0).vmethod_68(this);
			}
			return new Class75(struct7_0.int_0 | ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_69()
		{
			return new Class75(~struct7_0.int_0);
		}

		public override Class73 vmethod_70(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.int_0 ^ ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_70(this);
		}

		public override Class73 vmethod_72(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				return ((Class77)class73_0).method_15(this);
			}
			return new Class75(struct7_0.int_0 << ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_73(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.int_0 >> ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_14(this);
		}

		public override Class73 vmethod_74(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return new Class75(struct7_0.uint_0 >> ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).method_13(this);
		}

		public override string ToString()
		{
			switch (enum3_0)
			{
			default:
				return struct7_0.uint_0.ToString();
			case (Enum3)1:
			case (Enum3)3:
			case (Enum3)5:
			case (Enum3)11:
				return struct7_0.int_0.ToString();
			}
		}

		internal override Class73 vmethod_7()
		{
			return this;
		}

		internal override bool vmethod_8()
		{
			return true;
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.method_0())
			{
				if (class73_0.vmethod_0())
				{
					return ((Class79)class73_0).vmethod_5(this);
				}
				Class73 @class = class73_0.vmethod_7();
				if (@class.vmethod_8())
				{
					if (@class.method_3())
					{
						return false;
					}
					if (@class.method_1())
					{
						return struct7_0.int_0 == ((Class75)@class).struct7_0.int_0;
					}
					return ((Class77)@class).vmethod_5(this);
				}
				return false;
			}
			return ((Class85)class73_0).vmethod_5(this);
		}

		private static Class74 smethod_4(Class73 class73_0)
		{
			Class74 @class = class73_0 as Class74;
			if (@class == null && class73_0.vmethod_0())
			{
				@class = class73_0.vmethod_7() as Class74;
			}
			return @class;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (!class73_0.method_0())
			{
				if (class73_0.vmethod_0())
				{
					return ((Class79)class73_0).BeouTiljCp(this);
				}
				Class73 @class = class73_0.vmethod_7();
				if (@class.vmethod_8())
				{
					if (@class.method_3())
					{
						return false;
					}
					if (!@class.method_1())
					{
						return ((Class77)@class).BeouTiljCp(this);
					}
					return struct7_0.uint_0 != ((Class75)@class).struct7_0.uint_0;
				}
				return false;
			}
			return false;
		}

		public override bool vmethod_75(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).vmethod_78(this);
				}
				throw new Exception1();
			}
			return struct7_0.int_0 >= ((Class75)class73_0).struct7_0.int_0;
		}

		public override bool vmethod_76(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return struct7_0.uint_0 >= ((Class75)class73_0).struct7_0.uint_0;
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_79(this);
		}

		public override bool vmethod_77(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				return ((Class77)class73_0).vmethod_80(this);
			}
			return struct7_0.int_0 > ((Class75)class73_0).struct7_0.int_0;
		}

		public override bool lwlumgaheq(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return struct7_0.uint_0 > ((Class75)class73_0).struct7_0.uint_0;
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_81(this);
		}

		public override bool vmethod_78(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				return struct7_0.int_0 <= ((Class75)class73_0).struct7_0.int_0;
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			return ((Class77)class73_0).vmethod_75(this);
		}

		public override bool vmethod_79(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				return ((Class77)class73_0).vmethod_76(this);
			}
			return struct7_0.uint_0 <= ((Class75)class73_0).struct7_0.uint_0;
		}

		public override bool vmethod_80(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).vmethod_77(this);
				}
				throw new Exception1();
			}
			return struct7_0.int_0 < ((Class75)class73_0).struct7_0.int_0;
		}

		public override bool vmethod_81(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					return ((Class77)class73_0).lwlumgaheq(this);
				}
				throw new Exception1();
			}
			return struct7_0.uint_0 < ((Class75)class73_0).struct7_0.uint_0;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct Struct8
	{
		[FieldOffset(0)]
		public byte byte_0;

		[FieldOffset(0)]
		public sbyte sbyte_0;

		[FieldOffset(0)]
		public ushort ushort_0;

		[FieldOffset(0)]
		public short short_0;

		[FieldOffset(0)]
		public uint uint_0;

		[FieldOffset(0)]
		public int int_0;

		[FieldOffset(0)]
		public ulong ulong_0;

		[FieldOffset(0)]
		public long long_0;
	}

	private class Class76 : Class74
	{
		public Struct8 struct8_0;

		public Enum3 enum3_0;

		internal override void vmethod_9(Class73 class73_0)
		{
			struct8_0 = ((Class76)class73_0).struct8_0;
			enum3_0 = ((Class76)class73_0).enum3_0;
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_9(class73_0);
		}

		public Class76(long long_0)
		{
			enum6_0 = (Enum6)2;
			struct8_0.long_0 = long_0;
			enum3_0 = (Enum3)7;
		}

		public Class76(Class76 class76_0)
		{
			enum6_0 = class76_0.enum6_0;
			struct8_0.long_0 = class76_0.struct8_0.long_0;
			enum3_0 = class76_0.enum3_0;
		}

		public override Class74 vmethod_71()
		{
			return new Class76(this);
		}

		public Class76(long long_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)2;
			struct8_0.long_0 = long_0;
			enum3_0 = enum3_1;
		}

		public Class76(ulong ulong_0)
		{
			enum6_0 = (Enum6)2;
			struct8_0.ulong_0 = ulong_0;
			enum3_0 = (Enum3)8;
		}

		public Class76(ulong ulong_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)2;
			struct8_0.ulong_0 = ulong_0;
			enum3_0 = enum3_1;
		}

		public override bool vmethod_10()
		{
			if (enum3_0 == (Enum3)7)
			{
				return struct8_0.long_0 == 0L;
			}
			return struct8_0.ulong_0 == 0L;
		}

		public override bool vmethod_11()
		{
			return !vmethod_10();
		}

		public override Class73 vmethod_12(Enum3 enum3_1)
		{
			return enum3_1 switch
			{
				(Enum3)1 => vmethod_14(), 
				(Enum3)2 => vmethod_15(), 
				(Enum3)3 => vmethod_16(), 
				(Enum3)4 => vmethod_17(), 
				(Enum3)5 => vmethod_18(), 
				(Enum3)6 => vmethod_19(), 
				(Enum3)7 => vmethod_20(), 
				(Enum3)8 => vmethod_21(), 
				(Enum3)11 => vmethod_13(), 
				(Enum3)15 => method_6(), 
				(Enum3)16 => vmethod_71(), 
				_ => throw new Exception(((Enum7)4).ToString()), 
			};
		}

		internal override object vmethod_4(Type type_0)
		{
			if (type_0 != null && type_0.IsByRef)
			{
				type_0 = type_0.GetElementType();
			}
			if (!(type_0 == null) && !(type_0 == typeof(object)))
			{
				if (type_0 == typeof(int))
				{
					return struct8_0.int_0;
				}
				if (!(type_0 == typeof(uint)))
				{
					if (type_0 == typeof(short))
					{
						return struct8_0.short_0;
					}
					if (!(type_0 == typeof(ushort)))
					{
						if (!(type_0 == typeof(byte)))
						{
							if (type_0 == typeof(sbyte))
							{
								return struct8_0.sbyte_0;
							}
							if (type_0 == typeof(bool))
							{
								return !vmethod_10();
							}
							if (!(type_0 == typeof(long)))
							{
								if (!(type_0 == typeof(ulong)))
								{
									if (type_0 == typeof(char))
									{
										return (char)struct8_0.long_0;
									}
									if (type_0.IsEnum)
									{
										return method_5(type_0);
									}
									throw new Exception1();
								}
								return struct8_0.ulong_0;
							}
							return struct8_0.long_0;
						}
						return struct8_0.byte_0;
					}
					return struct8_0.ushort_0;
				}
				return struct8_0.uint_0;
			}
			return enum3_0 switch
			{
				(Enum3)1 => struct8_0.sbyte_0, 
				(Enum3)2 => struct8_0.byte_0, 
				(Enum3)3 => struct8_0.short_0, 
				(Enum3)4 => struct8_0.ushort_0, 
				(Enum3)5 => struct8_0.int_0, 
				(Enum3)6 => struct8_0.uint_0, 
				(Enum3)7 => struct8_0.long_0, 
				(Enum3)8 => struct8_0.ulong_0, 
				(Enum3)11 => vmethod_11(), 
				(Enum3)15 => (char)struct8_0.int_0, 
				_ => struct8_0.long_0, 
			};
		}

		internal object method_5(Type type_0)
		{
			Type underlyingType = Enum.GetUnderlyingType(type_0);
			if (underlyingType == typeof(int))
			{
				return Enum.ToObject(type_0, struct8_0.int_0);
			}
			if (!(underlyingType == typeof(uint)))
			{
				if (underlyingType == typeof(short))
				{
					return Enum.ToObject(type_0, struct8_0.short_0);
				}
				if (underlyingType == typeof(ushort))
				{
					return Enum.ToObject(type_0, struct8_0.ushort_0);
				}
				if (!(underlyingType == typeof(byte)))
				{
					if (underlyingType == typeof(sbyte))
					{
						return Enum.ToObject(type_0, struct8_0.sbyte_0);
					}
					if (underlyingType == typeof(long))
					{
						return Enum.ToObject(type_0, struct8_0.long_0);
					}
					if (!(underlyingType == typeof(ulong)))
					{
						if (underlyingType == typeof(char))
						{
							return Enum.ToObject(type_0, (ushort)struct8_0.int_0);
						}
						return Enum.ToObject(type_0, struct8_0.long_0);
					}
					return Enum.ToObject(type_0, struct8_0.ulong_0);
				}
				return Enum.ToObject(type_0, struct8_0.byte_0);
			}
			return Enum.ToObject(type_0, struct8_0.uint_0);
		}

		public override Class75 vmethod_13()
		{
			return new Class75((!vmethod_10()) ? 1 : 0);
		}

		internal override bool vmethod_6()
		{
			return vmethod_11();
		}

		public Class75 method_6()
		{
			return new Class75(struct8_0.sbyte_0, (Enum3)15);
		}

		public override Class75 vmethod_14()
		{
			return new Class75(struct8_0.sbyte_0, (Enum3)1);
		}

		public override Class75 vmethod_15()
		{
			return new Class75((uint)struct8_0.byte_0, (Enum3)2);
		}

		public override Class75 vmethod_16()
		{
			return new Class75(struct8_0.short_0, (Enum3)3);
		}

		public override Class75 vmethod_17()
		{
			return new Class75((uint)struct8_0.ushort_0, (Enum3)4);
		}

		public override Class75 vmethod_18()
		{
			return new Class75(struct8_0.int_0, (Enum3)5);
		}

		public override Class75 vmethod_19()
		{
			return new Class75(struct8_0.uint_0, (Enum3)6);
		}

		public override Class76 vmethod_20()
		{
			return new Class76(struct8_0.long_0, (Enum3)7);
		}

		public override Class76 vmethod_21()
		{
			return new Class76(struct8_0.ulong_0, (Enum3)8);
		}

		public override Class75 vmethod_22()
		{
			return vmethod_14();
		}

		public override Class75 vmethod_23()
		{
			return vmethod_16();
		}

		public override Class75 vmethod_24()
		{
			return vmethod_18();
		}

		public override Class76 vmethod_25()
		{
			return vmethod_20();
		}

		public override Class75 vmethod_26()
		{
			return vmethod_15();
		}

		public override Class75 vmethod_27()
		{
			return vmethod_17();
		}

		public override Class75 vmethod_28()
		{
			return vmethod_19();
		}

		public override Class76 vmethod_29()
		{
			return vmethod_21();
		}

		public override Class75 vmethod_30()
		{
			return new Class75(checked((sbyte)struct8_0.long_0), (Enum3)1);
		}

		public override Class75 vmethod_31()
		{
			return new Class75(checked((sbyte)struct8_0.ulong_0), (Enum3)1);
		}

		public override Class75 vmethod_32()
		{
			return new Class75(checked((short)struct8_0.long_0), (Enum3)3);
		}

		public override Class75 usfdqHavse()
		{
			return new Class75(checked((short)struct8_0.ulong_0), (Enum3)3);
		}

		public override Class75 vmethod_33()
		{
			return new Class75(checked((int)struct8_0.long_0), (Enum3)5);
		}

		public override Class75 vmethod_34()
		{
			return new Class75(checked((int)struct8_0.ulong_0), (Enum3)5);
		}

		public override Class76 vmethod_35()
		{
			return new Class76(struct8_0.long_0, (Enum3)7);
		}

		public override Class76 vmethod_36()
		{
			return new Class76(checked((long)struct8_0.ulong_0), (Enum3)7);
		}

		public override Class75 vmethod_37()
		{
			return new Class75(checked((byte)struct8_0.long_0), (Enum3)2);
		}

		public override Class75 vmethod_38()
		{
			return new Class75(checked((byte)struct8_0.ulong_0), (Enum3)2);
		}

		public override Class75 vmethod_39()
		{
			return new Class75(checked((ushort)struct8_0.long_0), (Enum3)4);
		}

		public override Class75 vmethod_40()
		{
			return new Class75(checked((ushort)struct8_0.ulong_0), (Enum3)4);
		}

		public override Class75 vmethod_41()
		{
			return new Class75(checked((uint)struct8_0.long_0), (Enum3)6);
		}

		public override Class75 vmethod_42()
		{
			return new Class75(checked((uint)struct8_0.ulong_0), (Enum3)6);
		}

		public override Class76 vmethod_43()
		{
			return new Class76(checked((ulong)struct8_0.long_0), (Enum3)8);
		}

		public override Class76 vmethod_44()
		{
			return new Class76(struct8_0.ulong_0, (Enum3)8);
		}

		public override Class78 vmethod_45()
		{
			return new Class78(struct8_0.long_0);
		}

		public override Class78 vmethod_46()
		{
			return new Class78((double)struct8_0.long_0);
		}

		public override Class78 vmethod_47()
		{
			return new Class78((double)struct8_0.ulong_0);
		}

		public override Class77 vmethod_48()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_25().struct8_0.long_0);
			}
			return new Class77(vmethod_24().struct7_0.int_0);
		}

		public override Class77 vmethod_49()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_29().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_28().struct7_0.uint_0);
		}

		public override Class77 vmethod_50()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_35().struct8_0.long_0);
			}
			return new Class77(vmethod_33().struct7_0.int_0);
		}

		public override Class77 vmethod_51()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_43().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_41().struct7_0.uint_0);
		}

		public override Class77 vmethod_52()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_36().struct8_0.long_0);
			}
			return new Class77(vmethod_34().struct7_0.int_0);
		}

		public override Class77 vmethod_53()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(struct8_0.ulong_0);
			}
			return new Class77((ulong)checked((uint)struct8_0.ulong_0));
		}

		public override Class73 vmethod_54()
		{
			return new Class76(-struct8_0.long_0);
		}

		public override Class73 Add(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 + ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_55(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.long_0 + ((Class76)class73_0).struct8_0.long_0));
		}

		public override Class73 vmethod_56(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.ulong_0 + ((Class76)class73_0).struct8_0.ulong_0));
		}

		public override Class73 vmethod_57(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 - ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_58(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.long_0 - ((Class76)class73_0).struct8_0.long_0));
		}

		public override Class73 vmethod_59(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.ulong_0 - ((Class76)class73_0).struct8_0.ulong_0));
		}

		public override Class73 vmethod_60(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 * ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_61(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.long_0 * ((Class76)class73_0).struct8_0.long_0));
		}

		public override Class73 vmethod_62(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(checked(struct8_0.ulong_0 * ((Class76)class73_0).struct8_0.ulong_0));
		}

		public override Class73 vmethod_63(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 / ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_64(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.ulong_0 / ((Class76)class73_0).struct8_0.ulong_0);
		}

		public override Class73 vmethod_65(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 % ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_66(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.ulong_0 % ((Class76)class73_0).struct8_0.ulong_0);
		}

		public override Class73 vmethod_67(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 & ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_68(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 | ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_69()
		{
			return new Class76(~struct8_0.long_0);
		}

		public override Class73 vmethod_70(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 ^ ((Class76)class73_0).struct8_0.long_0);
		}

		public override Class73 vmethod_72(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				if (!class73_0.vmethod_3())
				{
					throw new Exception1();
				}
				return new Class76(struct8_0.long_0 << ((Class74)class73_0).vmethod_18().struct7_0.int_0);
			}
			return new Class76(struct8_0.long_0 << ((Class76)class73_0).struct8_0.int_0);
		}

		public override Class73 vmethod_73(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				if (class73_0.vmethod_3())
				{
					return new Class76(struct8_0.long_0 >> ((Class74)class73_0).vmethod_18().struct7_0.int_0);
				}
				throw new Exception1();
			}
			return new Class76(struct8_0.long_0 >> ((Class76)class73_0).struct8_0.int_0);
		}

		public override Class73 vmethod_74(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				if (!class73_0.vmethod_3())
				{
					throw new Exception1();
				}
				return new Class76(struct8_0.ulong_0 >> ((Class74)class73_0).vmethod_18().struct7_0.int_0);
			}
			return new Class76(struct8_0.ulong_0 >> ((Class76)class73_0).struct8_0.int_0);
		}

		public override string ToString()
		{
			if (enum3_0 == (Enum3)7)
			{
				return struct8_0.long_0.ToString();
			}
			return struct8_0.ulong_0.ToString();
		}

		internal override Class73 vmethod_7()
		{
			return this;
		}

		internal override bool vmethod_8()
		{
			return true;
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (class73_0.method_0())
			{
				return ((Class85)class73_0).vmethod_5(this);
			}
			if (class73_0.vmethod_0())
			{
				return ((Class79)class73_0).vmethod_5(this);
			}
			Class73 @class = class73_0.vmethod_7();
			if (@class.method_3())
			{
				return struct8_0.long_0 == ((Class76)@class).struct8_0.long_0;
			}
			return false;
		}

		private static Class74 smethod_4(Class73 class73_0)
		{
			Class74 @class = class73_0 as Class74;
			if (@class == null && class73_0.vmethod_0())
			{
				@class = class73_0.vmethod_7() as Class74;
			}
			return @class;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (!class73_0.method_0())
			{
				if (!class73_0.vmethod_0())
				{
					Class73 @class = class73_0.vmethod_7();
					if (@class.method_3())
					{
						return struct8_0.ulong_0 != ((Class76)@class).struct8_0.ulong_0;
					}
					return false;
				}
				return ((Class79)class73_0).BeouTiljCp(this);
			}
			return false;
		}

		public override bool vmethod_75(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.long_0 >= ((Class76)class73_0).struct8_0.long_0;
		}

		public override bool vmethod_76(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.ulong_0 >= ((Class76)class73_0).struct8_0.ulong_0;
		}

		public override bool vmethod_77(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.long_0 > ((Class76)class73_0).struct8_0.long_0;
		}

		public override bool lwlumgaheq(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.ulong_0 > ((Class76)class73_0).struct8_0.ulong_0;
		}

		public override bool vmethod_78(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.long_0 <= ((Class76)class73_0).struct8_0.long_0;
		}

		public override bool vmethod_79(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.ulong_0 <= ((Class76)class73_0).struct8_0.ulong_0;
		}

		public override bool vmethod_80(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.long_0 < ((Class76)class73_0).struct8_0.long_0;
		}

		public override bool vmethod_81(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_3())
			{
				throw new Exception1();
			}
			return struct8_0.ulong_0 < ((Class76)class73_0).struct8_0.ulong_0;
		}
	}

	private class Class77 : Class74
	{
		public object object_0;

		public Enum3 enum3_0;

		internal void method_5(Class73 class73_0)
		{
			if (class73_0.method_2())
			{
				object_0 = ((Class77)class73_0).object_0;
				enum3_0 = ((Class77)class73_0).enum3_0;
			}
			else
			{
				vmethod_9(class73_0);
			}
		}

		internal unsafe override void vmethod_9(Class73 class73_0)
		{
			if (!class73_0.method_2())
			{
				object obj = class73_0.vmethod_4(null);
				if (obj == null)
				{
					return;
				}
				IntPtr intPtr = ((IntPtr.Size != 8) ? new IntPtr(((Class75)object_0).struct7_0.int_0) : new IntPtr(((Class76)object_0).struct8_0.long_0));
				Type type = obj.GetType();
				if (type == typeof(string))
				{
					return;
				}
				if (!(type == typeof(byte)))
				{
					if (type == typeof(sbyte))
					{
						*(sbyte*)(void*)intPtr = (sbyte)obj;
					}
					else if (type == typeof(short))
					{
						*(short*)(void*)intPtr = (short)obj;
					}
					else if (type == typeof(ushort))
					{
						*(ushort*)(void*)intPtr = (ushort)obj;
					}
					else if (type == typeof(int))
					{
						*(int*)(void*)intPtr = (int)obj;
					}
					else if (!(type == typeof(uint)))
					{
						if (!(type == typeof(long)))
						{
							if (type == typeof(ulong))
							{
								*(ulong*)(void*)intPtr = (ulong)obj;
							}
							else if (!(type == typeof(float)))
							{
								if (!(type == typeof(double)))
								{
									if (!(type == typeof(bool)))
									{
										if (type == typeof(IntPtr))
										{
											*(IntPtr*)(void*)intPtr = (IntPtr)obj;
										}
										else if (!(type == typeof(UIntPtr)))
										{
											if (!(type == typeof(char)))
											{
												throw new Exception1();
											}
											*(char*)(void*)intPtr = (char)obj;
										}
										else
										{
											*(UIntPtr*)(void*)intPtr = (UIntPtr)obj;
										}
									}
									else
									{
										*(bool*)(void*)intPtr = (bool)obj;
									}
								}
								else
								{
									*(double*)(void*)intPtr = (double)obj;
								}
							}
							else
							{
								*(float*)(void*)intPtr = (float)obj;
							}
						}
						else
						{
							*(long*)(void*)intPtr = (long)obj;
						}
					}
					else
					{
						*(uint*)(void*)intPtr = (uint)obj;
					}
				}
				else
				{
					*(byte*)(void*)intPtr = (byte)obj;
				}
			}
			else if (IntPtr.Size == 8)
			{
				IntPtr intPtr2 = new IntPtr(((Class76)object_0).struct8_0.long_0);
				IntPtr intPtr3 = new IntPtr(((Class76)((Class77)class73_0).object_0).struct8_0.long_0);
				*(long*)(void*)intPtr2 = intPtr3.ToInt64();
			}
			else
			{
				IntPtr intPtr4 = new IntPtr(((Class75)object_0).struct7_0.int_0);
				IntPtr intPtr5 = new IntPtr(((Class75)((Class77)class73_0).object_0).struct7_0.int_0);
				*(int*)(void*)intPtr4 = intPtr5.ToInt32();
			}
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_9(class73_0);
		}

		public Class77(IntPtr intptr_0)
		{
			enum6_0 = (Enum6)3;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(intptr_0.ToInt64());
				enum3_0 = (Enum3)12;
			}
			else
			{
				object_0 = new Class75(intptr_0.ToInt32());
				enum3_0 = (Enum3)12;
			}
		}

		public Class77(UIntPtr uintptr_0)
		{
			enum6_0 = (Enum6)3;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(uintptr_0.ToUInt64());
				enum3_0 = (Enum3)12;
			}
			else
			{
				object_0 = new Class75(uintptr_0.ToUInt32());
				enum3_0 = (Enum3)12;
			}
		}

		public Class77()
		{
			enum6_0 = (Enum6)3;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(0L);
				enum3_0 = (Enum3)12;
			}
			else
			{
				object_0 = new Class75(0);
				enum3_0 = (Enum3)12;
			}
		}

		public override Class74 vmethod_71()
		{
			return new Class77
			{
				object_0 = ((Class74)object_0).vmethod_71(),
				enum3_0 = enum3_0
			};
		}

		public Class77(long long_0)
		{
			enum6_0 = (Enum6)3;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(long_0);
				enum3_0 = (Enum3)12;
			}
			else
			{
				object_0 = new Class75((int)long_0);
				enum3_0 = (Enum3)12;
			}
		}

		public Class77(long long_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)3;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(long_0);
				enum3_0 = enum3_1;
			}
			else
			{
				object_0 = new Class75((int)long_0);
				enum3_0 = enum3_1;
			}
		}

		public Class77(ulong ulong_0)
		{
			enum6_0 = (Enum6)4;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(ulong_0);
				enum3_0 = (Enum3)13;
			}
			else
			{
				object_0 = new Class75((uint)ulong_0);
				enum3_0 = (Enum3)13;
			}
		}

		public Class77(ulong ulong_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)4;
			if (IntPtr.Size == 8)
			{
				object_0 = new Class76(ulong_0);
				enum3_0 = enum3_1;
			}
			else
			{
				object_0 = new Class75((uint)ulong_0);
				enum3_0 = enum3_1;
			}
		}

		public override bool vmethod_10()
		{
			return ((Class74)object_0).vmethod_10();
		}

		public override bool vmethod_11()
		{
			return !vmethod_10();
		}

		internal override bool vmethod_6()
		{
			return vmethod_11();
		}

		internal override bool vmethod_1()
		{
			return true;
		}

		public override Class73 vmethod_12(Enum3 enum3_1)
		{
			return enum3_1 switch
			{
				(Enum3)1 => vmethod_14(), 
				(Enum3)2 => vmethod_15(), 
				(Enum3)3 => vmethod_16(), 
				(Enum3)4 => vmethod_17(), 
				(Enum3)5 => vmethod_18(), 
				(Enum3)6 => vmethod_19(), 
				(Enum3)7 => vmethod_20(), 
				(Enum3)8 => vmethod_21(), 
				(Enum3)11 => vmethod_13(), 
				(Enum3)12 => this, 
				(Enum3)13 => this, 
				(Enum3)16 => vmethod_71(), 
				_ => throw new Exception(((Enum7)4).ToString()), 
			};
		}

		internal IntPtr method_6()
		{
			if (IntPtr.Size == 8)
			{
				return new IntPtr(((Class76)object_0).struct8_0.long_0);
			}
			return new IntPtr(((Class75)object_0).struct7_0.int_0);
		}

		internal override object vmethod_4(Type type_0)
		{
			if (type_0 != null && type_0.IsByRef)
			{
				type_0 = type_0.GetElementType();
			}
			if (!(type_0 == typeof(IntPtr)))
			{
				if (!(type_0 == typeof(UIntPtr)))
				{
					if (!(type_0 == null) && !(type_0 == typeof(object)))
					{
						throw new Exception1();
					}
					if (IntPtr.Size == 8)
					{
						if (enum3_0 == (Enum3)12)
						{
							return new IntPtr(((Class76)object_0).struct8_0.long_0);
						}
						return new UIntPtr(((Class76)object_0).struct8_0.ulong_0);
					}
					if (enum3_0 == (Enum3)12)
					{
						return new IntPtr(((Class76)object_0).struct8_0.int_0);
					}
					return new UIntPtr(((Class75)object_0).struct7_0.uint_0);
				}
				if (IntPtr.Size == 8)
				{
					return new UIntPtr(((Class76)object_0).struct8_0.ulong_0);
				}
				return new UIntPtr(((Class75)object_0).struct7_0.uint_0);
			}
			if (IntPtr.Size == 8)
			{
				return new IntPtr(((Class76)object_0).struct8_0.long_0);
			}
			return new IntPtr(((Class75)object_0).struct7_0.int_0);
		}

		public override Class75 vmethod_13()
		{
			return ((Class74)object_0).vmethod_13();
		}

		public override Class75 vmethod_14()
		{
			return ((Class74)object_0).vmethod_14();
		}

		public override Class75 vmethod_15()
		{
			return ((Class74)object_0).vmethod_15();
		}

		public override Class75 vmethod_16()
		{
			return ((Class74)object_0).vmethod_16();
		}

		public override Class75 vmethod_17()
		{
			return ((Class74)object_0).vmethod_17();
		}

		public override Class75 vmethod_18()
		{
			return ((Class74)object_0).vmethod_18();
		}

		public override Class75 vmethod_19()
		{
			return ((Class74)object_0).vmethod_19();
		}

		public override Class76 vmethod_20()
		{
			return ((Class74)object_0).vmethod_20();
		}

		public override Class76 vmethod_21()
		{
			return ((Class74)object_0).vmethod_21();
		}

		public override Class75 vmethod_22()
		{
			return vmethod_14();
		}

		public override Class75 vmethod_23()
		{
			return vmethod_16();
		}

		public override Class75 vmethod_24()
		{
			return vmethod_18();
		}

		public override Class76 vmethod_25()
		{
			return vmethod_20();
		}

		public override Class75 vmethod_26()
		{
			return vmethod_15();
		}

		public override Class75 vmethod_27()
		{
			return vmethod_17();
		}

		public override Class75 vmethod_28()
		{
			return vmethod_19();
		}

		public override Class76 vmethod_29()
		{
			return vmethod_21();
		}

		public override Class75 vmethod_30()
		{
			return ((Class74)object_0).vmethod_30();
		}

		public override Class75 vmethod_31()
		{
			return ((Class74)object_0).vmethod_31();
		}

		public override Class75 vmethod_32()
		{
			return ((Class74)object_0).vmethod_32();
		}

		public override Class75 usfdqHavse()
		{
			return ((Class74)object_0).usfdqHavse();
		}

		public override Class75 vmethod_33()
		{
			return ((Class74)object_0).vmethod_33();
		}

		public override Class75 vmethod_34()
		{
			return ((Class74)object_0).vmethod_34();
		}

		public override Class76 vmethod_35()
		{
			return ((Class74)object_0).vmethod_35();
		}

		public override Class76 vmethod_36()
		{
			return ((Class74)object_0).vmethod_36();
		}

		public override Class75 vmethod_37()
		{
			return ((Class74)object_0).vmethod_37();
		}

		public override Class75 vmethod_38()
		{
			return ((Class74)object_0).vmethod_38();
		}

		public override Class75 vmethod_39()
		{
			return ((Class74)object_0).vmethod_39();
		}

		public override Class75 vmethod_40()
		{
			return ((Class74)object_0).vmethod_40();
		}

		public override Class75 vmethod_41()
		{
			return ((Class74)object_0).vmethod_41();
		}

		public override Class75 vmethod_42()
		{
			return ((Class74)object_0).vmethod_42();
		}

		public override Class76 vmethod_43()
		{
			return ((Class74)object_0).vmethod_43();
		}

		public override Class76 vmethod_44()
		{
			return ((Class74)object_0).vmethod_44();
		}

		public override Class78 vmethod_45()
		{
			return ((Class74)object_0).vmethod_45();
		}

		public override Class78 vmethod_46()
		{
			return ((Class74)object_0).vmethod_46();
		}

		public override Class78 vmethod_47()
		{
			return ((Class74)object_0).vmethod_47();
		}

		public override Class77 vmethod_48()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_25().struct8_0.long_0);
			}
			return new Class77(vmethod_24().struct7_0.int_0);
		}

		public override Class77 vmethod_49()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_29().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_28().struct7_0.uint_0);
		}

		public override Class77 vmethod_50()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_35().struct8_0.long_0);
			}
			return new Class77(vmethod_33().struct7_0.int_0);
		}

		public override Class77 vmethod_51()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_43().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_41().struct7_0.uint_0);
		}

		public override Class77 vmethod_52()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_36().struct8_0.long_0);
			}
			return new Class77(vmethod_34().struct7_0.int_0);
		}

		public override Class77 vmethod_53()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_44().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_42().struct7_0.uint_0);
		}

		public override Class73 vmethod_54()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(-((Class76)object_0).struct8_0.long_0);
			}
			return new Class77(-((Class75)object_0).struct7_0.int_0);
		}

		public override Class73 Add(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 + ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 + ((Class75)class73_0).struct7_0.int_0);
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 + ((Class77)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 + ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			throw new Exception1();
		}

		public override Class73 vmethod_55(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			checked
			{
				if (!class73_0.method_1())
				{
					if (class73_0.method_2())
					{
						if (IntPtr.Size == 8)
						{
							return new Class77(vmethod_20().struct8_0.long_0 + ((Class77)class73_0).vmethod_20().struct8_0.long_0);
						}
						return new Class77(vmethod_18().struct7_0.int_0 + ((Class77)class73_0).vmethod_18().struct7_0.int_0);
					}
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 + ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 + ((Class75)class73_0).struct7_0.int_0);
			}
		}

		public override Class73 vmethod_56(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return new Class77(checked(vmethod_20().struct8_0.ulong_0 + ((Class77)class73_0).vmethod_20().struct8_0.ulong_0));
					}
					return new Class77((ulong)checked(vmethod_18().struct7_0.uint_0 + ((Class77)class73_0).vmethod_18().struct7_0.uint_0));
				}
				throw new Exception1();
			}
			checked
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 + ((Class75)class73_0).struct7_0.uint_0);
				}
				return new Class77(vmethod_18().struct7_0.uint_0 + ((Class75)class73_0).struct7_0.uint_0);
			}
		}

		public override Class73 vmethod_57(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 - ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 - ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 - ((Class77)class73_0).vmethod_20().struct8_0.long_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 - ((Class77)class73_0).vmethod_18().struct7_0.int_0);
		}

		public Class73 method_7(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class75)class73_0).vmethod_20().struct8_0.long_0 - vmethod_20().struct8_0.long_0);
				}
				return new Class77(((Class75)class73_0).struct7_0.int_0 - vmethod_18().struct7_0.int_0);
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class77)class73_0).vmethod_20().struct8_0.long_0 - vmethod_20().struct8_0.long_0);
				}
				return new Class77(((Class77)class73_0).vmethod_18().struct7_0.int_0 - vmethod_18().struct7_0.int_0);
			}
			throw new Exception1();
		}

		public override Class73 vmethod_58(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			checked
			{
				if (!class73_0.method_1())
				{
					if (class73_0.method_2())
					{
						if (IntPtr.Size == 8)
						{
							return new Class77(vmethod_20().struct8_0.long_0 - ((Class77)class73_0).vmethod_20().struct8_0.long_0);
						}
						return new Class77(vmethod_18().struct7_0.int_0 - ((Class77)class73_0).vmethod_18().struct7_0.int_0);
					}
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 - ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 - ((Class75)class73_0).struct7_0.int_0);
			}
		}

		public Class73 method_8(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			checked
			{
				if (class73_0.method_1())
				{
					if (IntPtr.Size == 8)
					{
						return new Class77(((Class75)class73_0).vmethod_20().struct8_0.long_0 - vmethod_20().struct8_0.long_0);
					}
					return new Class77(((Class75)class73_0).struct7_0.int_0 - vmethod_18().struct7_0.int_0);
				}
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return new Class77(((Class77)class73_0).vmethod_20().struct8_0.long_0 - vmethod_20().struct8_0.long_0);
					}
					return new Class77(((Class77)class73_0).vmethod_18().struct7_0.int_0 - vmethod_18().struct7_0.int_0);
				}
				throw new Exception1();
			}
		}

		public override Class73 vmethod_59(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(checked(vmethod_20().struct8_0.ulong_0 - ((Class77)class73_0).vmethod_20().struct8_0.ulong_0));
				}
				return new Class77((ulong)checked(vmethod_18().struct7_0.uint_0 - ((Class77)class73_0).vmethod_18().struct7_0.uint_0));
			}
			checked
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 - ((Class75)class73_0).struct7_0.uint_0);
				}
				return new Class77(vmethod_18().struct7_0.uint_0 - ((Class75)class73_0).struct7_0.uint_0);
			}
		}

		public Class73 method_9(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(checked(((Class77)class73_0).vmethod_20().struct8_0.ulong_0 - vmethod_20().struct8_0.ulong_0));
				}
				return new Class77((ulong)checked(((Class77)class73_0).vmethod_18().struct7_0.uint_0 - vmethod_18().struct7_0.uint_0));
			}
			checked
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class75)class73_0).struct7_0.uint_0 - vmethod_20().struct8_0.ulong_0);
				}
				return new Class77(((Class75)class73_0).struct7_0.uint_0 - vmethod_18().struct7_0.uint_0);
			}
		}

		public override Class73 vmethod_60(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 * ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 * ((Class75)class73_0).struct7_0.int_0);
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 * ((Class77)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 * ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			throw new Exception1();
		}

		public override Class73 vmethod_61(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			checked
			{
				if (!class73_0.method_1())
				{
					if (!class73_0.method_2())
					{
						throw new Exception1();
					}
					if (IntPtr.Size == 8)
					{
						return new Class77(vmethod_20().struct8_0.long_0 * ((Class77)class73_0).vmethod_20().struct8_0.long_0);
					}
					return new Class77(vmethod_18().struct7_0.int_0 * ((Class77)class73_0).vmethod_18().struct7_0.int_0);
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 * ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 * ((Class75)class73_0).struct7_0.int_0);
			}
		}

		public override Class73 vmethod_62(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(checked(vmethod_20().struct8_0.ulong_0 * ((Class77)class73_0).vmethod_20().struct8_0.ulong_0));
				}
				return new Class77((ulong)checked(vmethod_18().struct7_0.uint_0 * ((Class77)class73_0).vmethod_18().struct7_0.uint_0));
			}
			checked
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 * ((Class75)class73_0).struct7_0.uint_0);
				}
				return new Class77(vmethod_18().struct7_0.uint_0 * ((Class75)class73_0).struct7_0.uint_0);
			}
		}

		public override Class73 vmethod_63(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 / ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 / ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 / ((Class77)class73_0).vmethod_20().struct8_0.long_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 / ((Class77)class73_0).vmethod_18().struct7_0.int_0);
		}

		public Class73 method_10(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class75)class73_0).vmethod_20().struct8_0.long_0 / vmethod_20().struct8_0.long_0);
				}
				return new Class77(((Class75)class73_0).struct7_0.int_0 / vmethod_18().struct7_0.int_0);
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class77)class73_0).vmethod_20().struct8_0.long_0 / vmethod_20().struct8_0.long_0);
				}
				return new Class77(((Class77)class73_0).vmethod_18().struct7_0.int_0 / vmethod_18().struct7_0.int_0);
			}
			throw new Exception1();
		}

		public override Class73 vmethod_64(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 / ((Class77)class73_0).vmethod_20().struct8_0.ulong_0);
				}
				return new Class77((ulong)(vmethod_18().struct7_0.uint_0 / ((Class77)class73_0).vmethod_18().struct7_0.uint_0));
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.ulong_0 / ((Class75)class73_0).vmethod_20().struct8_0.ulong_0);
			}
			return new Class77(vmethod_18().struct7_0.uint_0 / ((Class75)class73_0).struct7_0.uint_0);
		}

		public Class73 method_11(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class75)class73_0).vmethod_20().struct8_0.ulong_0 / vmethod_20().struct8_0.ulong_0);
				}
				return new Class77(((Class75)class73_0).struct7_0.uint_0 / vmethod_18().struct7_0.uint_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(((Class77)class73_0).vmethod_20().struct8_0.ulong_0 / vmethod_20().struct8_0.ulong_0);
			}
			return new Class77((ulong)(((Class77)class73_0).vmethod_18().struct7_0.uint_0 / vmethod_18().struct7_0.uint_0));
		}

		public override Class73 vmethod_65(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 % ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 % ((Class75)class73_0).struct7_0.int_0);
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 % ((Class77)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 % ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			throw new Exception1();
		}

		public Class73 method_12(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(((Class75)class73_0).vmethod_20().struct8_0.long_0 % vmethod_20().struct8_0.long_0);
				}
				return new Class77(((Class75)class73_0).struct7_0.int_0 % vmethod_18().struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(((Class77)class73_0).vmethod_20().struct8_0.long_0 % vmethod_20().struct8_0.long_0);
			}
			return new Class77(((Class77)class73_0).vmethod_18().struct7_0.int_0 % vmethod_18().struct7_0.int_0);
		}

		public override Class73 vmethod_66(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 % ((Class75)class73_0).vmethod_20().struct8_0.ulong_0);
				}
				return new Class77(vmethod_18().struct7_0.uint_0 % ((Class75)class73_0).struct7_0.uint_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.ulong_0 % ((Class77)class73_0).vmethod_20().struct8_0.ulong_0);
			}
			return new Class77((ulong)(vmethod_18().struct7_0.uint_0 % ((Class77)class73_0).vmethod_18().struct7_0.uint_0));
		}

		public Class73 yIwByEsukh(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return new Class77(((Class77)class73_0).vmethod_20().struct8_0.ulong_0 % vmethod_20().struct8_0.ulong_0);
					}
					return new Class77((ulong)(((Class77)class73_0).vmethod_18().struct7_0.uint_0 % vmethod_18().struct7_0.uint_0));
				}
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(((Class75)class73_0).vmethod_20().struct8_0.ulong_0 % vmethod_20().struct8_0.ulong_0);
			}
			return new Class77(((Class75)class73_0).struct7_0.uint_0 % vmethod_18().struct7_0.uint_0);
		}

		public override Class73 vmethod_67(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 & ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 & ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 & ((Class77)class73_0).vmethod_20().struct8_0.long_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 & ((Class77)class73_0).vmethod_18().struct7_0.int_0);
		}

		public override Class73 vmethod_68(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 | ((Class75)class73_0).vmethod_20().struct8_0.long_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 | ((Class75)class73_0).struct7_0.int_0);
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 | ((Class77)class73_0).vmethod_20().struct8_0.long_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 | ((Class77)class73_0).vmethod_18().struct7_0.int_0);
		}

		public override Class73 vmethod_69()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(~vmethod_20().struct8_0.long_0);
			}
			return new Class77(~vmethod_18().struct7_0.int_0);
		}

		public override Class73 vmethod_70(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return new Class77(vmethod_20().struct8_0.long_0 ^ ((Class77)class73_0).vmethod_20().struct8_0.long_0);
					}
					return new Class77(vmethod_18().struct7_0.int_0 ^ ((Class77)class73_0).vmethod_18().struct7_0.int_0);
				}
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 ^ ((Class75)class73_0).vmethod_20().struct8_0.long_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 ^ ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_72(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 << ((Class77)class73_0).vmethod_20().struct8_0.int_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 << ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 << ((Class75)class73_0).struct7_0.int_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 << ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_73(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.long_0 >> ((Class77)class73_0).vmethod_20().struct8_0.int_0);
				}
				return new Class77(vmethod_18().struct7_0.int_0 >> ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.long_0 >> ((Class75)class73_0).struct7_0.int_0);
			}
			return new Class77(vmethod_18().struct7_0.int_0 >> ((Class75)class73_0).struct7_0.int_0);
		}

		public override Class73 vmethod_74(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return new Class77(vmethod_20().struct8_0.ulong_0 >> ((Class77)class73_0).vmethod_20().struct8_0.int_0);
				}
				return new Class77(vmethod_18().struct7_0.uint_0 >> ((Class77)class73_0).vmethod_18().struct7_0.int_0);
			}
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_20().struct8_0.ulong_0 >> ((Class75)class73_0).struct7_0.int_0);
			}
			return new Class77(vmethod_18().struct7_0.uint_0 >> ((Class75)class73_0).struct7_0.int_0);
		}

		public Class73 method_13(Class75 class75_0)
		{
			return new Class77(class75_0.struct7_0.uint_0 >> vmethod_18().struct7_0.int_0);
		}

		public Class73 method_14(Class75 class75_0)
		{
			return new Class77(class75_0.struct7_0.int_0 >> vmethod_20().struct8_0.int_0);
		}

		public Class73 method_15(Class75 class75_0)
		{
			return new Class77(class75_0.struct7_0.int_0 << vmethod_20().struct8_0.int_0);
		}

		public override string ToString()
		{
			return object_0.ToString();
		}

		internal override Class73 vmethod_7()
		{
			return this;
		}

		internal override bool vmethod_8()
		{
			return true;
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.method_0())
			{
				if (!class73_0.vmethod_0())
				{
					Class73 @class = class73_0.vmethod_7();
					if (!@class.vmethod_8())
					{
						return false;
					}
					if (!@class.method_1())
					{
						if (!@class.method_2())
						{
							return false;
						}
						_ = IntPtr.Size;
						return vmethod_20().struct8_0.long_0 == ((Class77)class73_0).vmethod_20().struct8_0.long_0;
					}
					if (IntPtr.Size == 8)
					{
						return vmethod_20().struct8_0.long_0 == ((Class75)class73_0).vmethod_20().struct8_0.long_0;
					}
					return vmethod_18().struct7_0.int_0 == ((Class75)class73_0).struct7_0.int_0;
				}
				return ((Class79)class73_0).vmethod_5(this);
			}
			return false;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (!class73_0.method_0())
			{
				if (class73_0.vmethod_0())
				{
					return ((Class79)class73_0).BeouTiljCp(this);
				}
				Class73 @class = class73_0.vmethod_7();
				if (!@class.vmethod_8())
				{
					return false;
				}
				if (!@class.method_1())
				{
					if (@class.method_2())
					{
						_ = IntPtr.Size;
						return vmethod_20().struct8_0.ulong_0 != ((Class77)class73_0).vmethod_20().struct8_0.ulong_0;
					}
					return false;
				}
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.ulong_0 != ((Class75)class73_0).vmethod_20().struct8_0.ulong_0;
				}
				return vmethod_18().struct7_0.uint_0 != ((Class75)class73_0).struct7_0.uint_0;
			}
			return false;
		}

		public override bool vmethod_75(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.long_0 >= ((Class75)class73_0).vmethod_20().struct8_0.long_0;
				}
				return vmethod_18().struct7_0.int_0 >= ((Class75)class73_0).struct7_0.int_0;
			}
			if (!class73_0.method_2())
			{
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return vmethod_20().struct8_0.long_0 >= ((Class77)class73_0).vmethod_20().struct8_0.long_0;
			}
			return vmethod_18().struct7_0.int_0 >= ((Class77)class73_0).vmethod_18().struct7_0.int_0;
		}

		public override bool vmethod_76(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return vmethod_20().struct8_0.ulong_0 >= ((Class77)class73_0).vmethod_20().struct8_0.ulong_0;
					}
					return vmethod_18().struct7_0.uint_0 >= ((Class77)class73_0).vmethod_18().struct7_0.uint_0;
				}
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return vmethod_20().struct8_0.ulong_0 >= ((Class75)class73_0).vmethod_20().struct8_0.ulong_0;
			}
			return vmethod_18().struct7_0.uint_0 >= ((Class75)class73_0).struct7_0.uint_0;
		}

		public override bool vmethod_77(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.long_0 > ((Class75)class73_0).vmethod_20().struct8_0.long_0;
				}
				return vmethod_18().struct7_0.int_0 > ((Class75)class73_0).struct7_0.int_0;
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.long_0 > ((Class77)class73_0).vmethod_20().struct8_0.long_0;
				}
				return vmethod_18().struct7_0.int_0 > ((Class77)class73_0).vmethod_18().struct7_0.int_0;
			}
			throw new Exception1();
		}

		public override bool lwlumgaheq(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.ulong_0 > ((Class75)class73_0).vmethod_20().struct8_0.ulong_0;
				}
				return vmethod_18().struct7_0.uint_0 > ((Class75)class73_0).struct7_0.uint_0;
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.ulong_0 > ((Class77)class73_0).vmethod_20().struct8_0.ulong_0;
				}
				return vmethod_18().struct7_0.uint_0 > ((Class77)class73_0).vmethod_18().struct7_0.uint_0;
			}
			throw new Exception1();
		}

		public override bool vmethod_78(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (!class73_0.method_2())
				{
					throw new Exception1();
				}
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.long_0 <= ((Class77)class73_0).vmethod_20().struct8_0.long_0;
				}
				return vmethod_18().struct7_0.int_0 <= ((Class77)class73_0).vmethod_18().struct7_0.int_0;
			}
			if (IntPtr.Size == 8)
			{
				return vmethod_20().struct8_0.long_0 <= ((Class75)class73_0).vmethod_20().struct8_0.long_0;
			}
			return vmethod_18().struct7_0.int_0 <= ((Class75)class73_0).struct7_0.int_0;
		}

		public override bool vmethod_79(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return vmethod_20().struct8_0.ulong_0 <= ((Class77)class73_0).vmethod_20().struct8_0.ulong_0;
					}
					return vmethod_18().struct7_0.uint_0 <= ((Class77)class73_0).vmethod_18().struct7_0.uint_0;
				}
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return vmethod_20().struct8_0.ulong_0 <= ((Class75)class73_0).vmethod_20().struct8_0.ulong_0;
			}
			return vmethod_18().struct7_0.uint_0 <= ((Class75)class73_0).struct7_0.uint_0;
		}

		public override bool vmethod_80(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_1())
			{
				if (class73_0.method_2())
				{
					if (IntPtr.Size == 8)
					{
						return vmethod_20().struct8_0.long_0 < ((Class77)class73_0).vmethod_20().struct8_0.long_0;
					}
					return vmethod_18().struct7_0.int_0 < ((Class77)class73_0).vmethod_18().struct7_0.int_0;
				}
				throw new Exception1();
			}
			if (IntPtr.Size == 8)
			{
				return vmethod_20().struct8_0.long_0 < ((Class75)class73_0).vmethod_20().struct8_0.long_0;
			}
			return vmethod_18().struct7_0.int_0 < ((Class75)class73_0).struct7_0.int_0;
		}

		public override bool vmethod_81(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (class73_0.method_1())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.ulong_0 < ((Class75)class73_0).vmethod_20().struct8_0.ulong_0;
				}
				return vmethod_18().struct7_0.uint_0 < ((Class75)class73_0).struct7_0.uint_0;
			}
			if (class73_0.method_2())
			{
				if (IntPtr.Size == 8)
				{
					return vmethod_20().struct8_0.ulong_0 < ((Class77)class73_0).vmethod_20().struct8_0.ulong_0;
				}
				return vmethod_18().struct7_0.uint_0 < ((Class77)class73_0).vmethod_18().struct7_0.uint_0;
			}
			throw new Exception1();
		}
	}

	private abstract class Class74 : Class73
	{
		public abstract bool vmethod_10();

		public abstract bool vmethod_11();

		public abstract Class73 vmethod_12(Enum3 enum3_0);

		public abstract Class75 vmethod_13();

		public abstract Class75 vmethod_14();

		public abstract Class75 vmethod_15();

		public abstract Class75 vmethod_16();

		public abstract Class75 vmethod_17();

		public abstract Class75 vmethod_18();

		public abstract Class75 vmethod_19();

		public abstract Class76 vmethod_20();

		public abstract Class76 vmethod_21();

		public abstract Class75 vmethod_22();

		public abstract Class75 vmethod_23();

		public abstract Class75 vmethod_24();

		public abstract Class76 vmethod_25();

		public abstract Class75 vmethod_26();

		public abstract Class75 vmethod_27();

		public abstract Class75 vmethod_28();

		public abstract Class76 vmethod_29();

		public abstract Class75 vmethod_30();

		public abstract Class75 vmethod_31();

		public abstract Class75 vmethod_32();

		public abstract Class75 usfdqHavse();

		public abstract Class75 vmethod_33();

		public abstract Class75 vmethod_34();

		public abstract Class76 vmethod_35();

		public abstract Class76 vmethod_36();

		public abstract Class75 vmethod_37();

		public abstract Class75 vmethod_38();

		public abstract Class75 vmethod_39();

		public abstract Class75 vmethod_40();

		public abstract Class75 vmethod_41();

		public abstract Class75 vmethod_42();

		public abstract Class76 vmethod_43();

		public abstract Class76 vmethod_44();

		public abstract Class78 vmethod_45();

		public abstract Class78 vmethod_46();

		public abstract Class78 vmethod_47();

		public abstract Class77 vmethod_48();

		public abstract Class77 vmethod_49();

		public abstract Class77 vmethod_50();

		public abstract Class77 vmethod_51();

		public abstract Class77 vmethod_52();

		public abstract Class77 vmethod_53();

		public abstract Class73 vmethod_54();

		public abstract Class73 Add(Class73 class73_0);

		public abstract Class73 vmethod_55(Class73 class73_0);

		public abstract Class73 vmethod_56(Class73 class73_0);

		public abstract Class73 vmethod_57(Class73 class73_0);

		public abstract Class73 vmethod_58(Class73 class73_0);

		public abstract Class73 vmethod_59(Class73 class73_0);

		public abstract Class73 vmethod_60(Class73 class73_0);

		public abstract Class73 vmethod_61(Class73 class73_0);

		public abstract Class73 vmethod_62(Class73 class73_0);

		public abstract Class73 vmethod_63(Class73 class73_0);

		public abstract Class73 vmethod_64(Class73 class73_0);

		public abstract Class73 vmethod_65(Class73 class73_0);

		public abstract Class73 vmethod_66(Class73 class73_0);

		public abstract Class73 vmethod_67(Class73 class73_0);

		public abstract Class73 vmethod_68(Class73 class73_0);

		public abstract Class73 vmethod_69();

		public abstract Class73 vmethod_70(Class73 class73_0);

		public abstract Class74 vmethod_71();

		public abstract Class73 vmethod_72(Class73 class73_0);

		public abstract Class73 vmethod_73(Class73 class73_0);

		public abstract Class73 vmethod_74(Class73 class73_0);

		public abstract bool vmethod_75(Class73 class73_0);

		public abstract bool vmethod_76(Class73 class73_0);

		public abstract bool vmethod_77(Class73 class73_0);

		public abstract bool lwlumgaheq(Class73 class73_0);

		public abstract bool vmethod_78(Class73 class73_0);

		public abstract bool vmethod_79(Class73 class73_0);

		public abstract bool vmethod_80(Class73 class73_0);

		public abstract bool vmethod_81(Class73 class73_0);

		internal override bool vmethod_3()
		{
			return true;
		}
	}

	private class Class78 : Class74
	{
		public double double_0;

		public Enum3 enum3_0;

		internal override void vmethod_9(Class73 class73_0)
		{
			double_0 = ((Class78)class73_0).double_0;
			enum3_0 = ((Class78)class73_0).enum3_0;
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_9(class73_0);
		}

		public Class78(double double_1)
		{
			enum6_0 = (Enum6)5;
			enum3_0 = (Enum3)10;
			double_0 = double_1;
		}

		public Class78(Class78 class78_0)
		{
			enum6_0 = class78_0.enum6_0;
			enum3_0 = class78_0.enum3_0;
			double_0 = class78_0.double_0;
		}

		public override Class74 vmethod_71()
		{
			return new Class78(this);
		}

		public Class78(double double_1, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)5;
			double_0 = double_1;
			enum3_0 = enum3_1;
		}

		public Class78(float float_0)
		{
			enum6_0 = (Enum6)5;
			double_0 = float_0;
			enum3_0 = (Enum3)9;
		}

		public Class78(float float_0, Enum3 enum3_1)
		{
			enum6_0 = (Enum6)5;
			double_0 = float_0;
			enum3_0 = enum3_1;
		}

		public override bool vmethod_10()
		{
			return double_0 == 0.0;
		}

		public override bool vmethod_11()
		{
			return !vmethod_10();
		}

		public override string ToString()
		{
			return double_0.ToString();
		}

		public override Class73 vmethod_12(Enum3 enum3_1)
		{
			return enum3_1 switch
			{
				(Enum3)1 => vmethod_14(), 
				(Enum3)2 => vmethod_15(), 
				(Enum3)3 => vmethod_16(), 
				(Enum3)4 => vmethod_17(), 
				(Enum3)5 => vmethod_18(), 
				(Enum3)6 => vmethod_19(), 
				(Enum3)7 => vmethod_20(), 
				(Enum3)8 => vmethod_21(), 
				(Enum3)9 => vmethod_45(), 
				(Enum3)10 => vmethod_46(), 
				(Enum3)11 => vmethod_13(), 
				_ => throw new Exception(((Enum7)4).ToString()), 
			};
		}

		internal override object vmethod_4(Type type_0)
		{
			if (type_0 != null && type_0.IsByRef)
			{
				type_0 = type_0.GetElementType();
			}
			if (!(type_0 == typeof(float)))
			{
				if (!(type_0 == typeof(double)))
				{
					if ((type_0 == null || type_0 == typeof(object)) && enum3_0 == (Enum3)9)
					{
						return (float)double_0;
					}
					return double_0;
				}
				return double_0;
			}
			return (float)double_0;
		}

		public override Class75 vmethod_13()
		{
			return new Class75(vmethod_10() ? 1 : 0);
		}

		internal override bool vmethod_6()
		{
			return vmethod_11();
		}

		public override Class75 vmethod_14()
		{
			return new Class75((sbyte)double_0, (Enum3)1);
		}

		public override Class75 vmethod_15()
		{
			return new Class75((uint)(byte)double_0, (Enum3)2);
		}

		public override Class75 vmethod_16()
		{
			return new Class75((short)double_0, (Enum3)3);
		}

		public override Class75 vmethod_17()
		{
			return new Class75((uint)(ushort)double_0, (Enum3)4);
		}

		public override Class75 vmethod_18()
		{
			return new Class75((int)double_0, (Enum3)5);
		}

		public override Class75 vmethod_19()
		{
			return new Class75((uint)double_0, (Enum3)6);
		}

		public override Class76 vmethod_20()
		{
			return new Class76((long)double_0, (Enum3)7);
		}

		public override Class76 vmethod_21()
		{
			return new Class76((ulong)double_0, (Enum3)8);
		}

		public override Class75 vmethod_22()
		{
			return vmethod_14();
		}

		public override Class75 vmethod_23()
		{
			return vmethod_16();
		}

		public override Class75 vmethod_24()
		{
			return vmethod_18();
		}

		public override Class76 vmethod_25()
		{
			return vmethod_20();
		}

		public override Class75 vmethod_26()
		{
			return vmethod_15();
		}

		public override Class75 vmethod_27()
		{
			return vmethod_17();
		}

		public override Class75 vmethod_28()
		{
			return vmethod_19();
		}

		public override Class76 vmethod_29()
		{
			return vmethod_21();
		}

		public override Class75 vmethod_30()
		{
			return new Class75(checked((sbyte)double_0), (Enum3)1);
		}

		public override Class75 vmethod_31()
		{
			return new Class75(checked((sbyte)double_0), (Enum3)1);
		}

		public override Class75 vmethod_32()
		{
			return new Class75(checked((short)double_0), (Enum3)3);
		}

		public override Class75 usfdqHavse()
		{
			return new Class75(checked((short)double_0), (Enum3)3);
		}

		public override Class75 vmethod_33()
		{
			return new Class75(checked((int)double_0), (Enum3)5);
		}

		public override Class75 vmethod_34()
		{
			return new Class75(checked((int)double_0), (Enum3)5);
		}

		public override Class76 vmethod_35()
		{
			return new Class76(checked((long)double_0), (Enum3)7);
		}

		public override Class76 vmethod_36()
		{
			return new Class76(checked((long)double_0), (Enum3)7);
		}

		public override Class75 vmethod_37()
		{
			return new Class75(checked((byte)double_0), (Enum3)2);
		}

		public override Class75 vmethod_38()
		{
			return new Class75(checked((byte)double_0), (Enum3)2);
		}

		public override Class75 vmethod_39()
		{
			return new Class75(checked((ushort)double_0), (Enum3)4);
		}

		public override Class75 vmethod_40()
		{
			return new Class75(checked((ushort)double_0), (Enum3)4);
		}

		public override Class75 vmethod_41()
		{
			return new Class75(checked((uint)double_0), (Enum3)6);
		}

		public override Class75 vmethod_42()
		{
			return new Class75(checked((uint)double_0), (Enum3)6);
		}

		public override Class76 vmethod_43()
		{
			return new Class76(checked((ulong)double_0), (Enum3)8);
		}

		public override Class76 vmethod_44()
		{
			return new Class76(checked((ulong)double_0), (Enum3)8);
		}

		public override Class78 vmethod_45()
		{
			return new Class78((float)double_0, (Enum3)9);
		}

		public override Class78 vmethod_46()
		{
			return new Class78(double_0, (Enum3)10);
		}

		public override Class78 vmethod_47()
		{
			return new Class78(double_0);
		}

		public override Class77 vmethod_48()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_25().struct8_0.long_0);
			}
			return new Class77(vmethod_24().struct7_0.int_0);
		}

		public override Class77 vmethod_49()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_29().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_28().struct7_0.uint_0);
		}

		public override Class77 vmethod_50()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_35().struct8_0.long_0);
			}
			return new Class77(vmethod_33().struct7_0.int_0);
		}

		public override Class77 vmethod_51()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_43().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_41().struct7_0.uint_0);
		}

		public override Class77 vmethod_52()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_36().struct8_0.long_0);
			}
			return new Class77(vmethod_34().struct7_0.int_0);
		}

		public override Class77 vmethod_53()
		{
			if (IntPtr.Size == 8)
			{
				return new Class77(vmethod_44().struct8_0.ulong_0);
			}
			return new Class77((ulong)vmethod_42().struct7_0.uint_0);
		}

		public override Class73 vmethod_54()
		{
			if (enum3_0 == (Enum3)9)
			{
				return new Class78((float)(0.0 - double_0));
			}
			return new Class78(0.0 - double_0);
		}

		public override Class73 Add(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 + ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_55(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 + ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_56(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 + ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_57(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 - ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_58(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 - ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_59(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 - ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_60(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4() || !class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 * ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_61(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 * ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_62(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 * ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_63(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 / ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_64(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 / ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_65(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 % ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_66(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return new Class78(double_0 % ((Class78)class73_0).double_0);
		}

		public override Class73 vmethod_67(Class73 class73_0)
		{
			throw new Exception1();
		}

		public override Class73 vmethod_68(Class73 class73_0)
		{
			throw new Exception1();
		}

		public override Class73 vmethod_69()
		{
			throw new Exception1();
		}

		public override Class73 vmethod_70(Class73 class73_0)
		{
			throw new Exception1();
		}

		public override Class73 vmethod_72(Class73 class73_0)
		{
			throw new Exception1();
		}

		public override Class73 vmethod_73(Class73 class73_0)
		{
			throw new Exception1();
		}

		public override Class73 vmethod_74(Class73 class73_0)
		{
			throw new Exception1();
		}

		internal override Class73 vmethod_7()
		{
			return this;
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (class73_0.method_0())
			{
				return false;
			}
			if (class73_0.vmethod_0())
			{
				return ((Class79)class73_0).vmethod_5(this);
			}
			Class73 @class = class73_0.vmethod_7();
			if (!@class.method_4())
			{
				return false;
			}
			return double_0 == ((Class78)@class).double_0;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (class73_0.method_0())
			{
				return false;
			}
			if (!class73_0.vmethod_0())
			{
				Class73 @class = class73_0.vmethod_7();
				if (@class.method_4())
				{
					return double_0 != ((Class78)@class).double_0;
				}
				return false;
			}
			return ((Class79)class73_0).BeouTiljCp(this);
		}

		public override bool vmethod_75(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 >= ((Class78)class73_0).double_0;
		}

		public override bool vmethod_76(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 >= ((Class78)class73_0).double_0;
		}

		public override bool vmethod_77(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 > ((Class78)class73_0).double_0;
		}

		public override bool lwlumgaheq(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 > ((Class78)class73_0).double_0;
		}

		public override bool vmethod_78(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 <= ((Class78)class73_0).double_0;
		}

		public override bool vmethod_79(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 <= ((Class78)class73_0).double_0;
		}

		public override bool vmethod_80(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 < ((Class78)class73_0).double_0;
		}

		public override bool vmethod_81(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				class73_0 = class73_0.vmethod_7();
			}
			if (!class73_0.method_4())
			{
				throw new Exception1();
			}
			return double_0 < ((Class78)class73_0).double_0;
		}
	}

	internal enum Enum3 : byte
	{

	}

	internal enum Enum4 : byte
	{

	}

	private class Exception0 : Exception
	{
		public Exception0(string string_0)
			: base(string_0)
		{
		}
	}

	private class Exception1 : Exception
	{
		public Exception1()
		{
		}

		public Exception1(string string_0)
			: base(string_0)
		{
		}
	}

	internal class Class63
	{
		internal Enum5 enum5_0 = (Enum5)126;

		internal object object_0;

		public override string ToString()
		{
			object obj = enum5_0;
			if (object_0 == null)
			{
				return obj.ToString();
			}
			return obj.ToString() + 'H' + object_0.ToString();
		}
	}

	internal abstract class Class79 : Class73
	{
		public Class79()
		{
		}

		internal override bool vmethod_0()
		{
			return true;
		}

		internal abstract IntPtr vmethod_10();

		internal abstract void vmethod_11(Class73 class73_0);

		internal override bool vmethod_1()
		{
			return true;
		}
	}

	internal class Class80 : Class79
	{
		private Class71 class71_0;

		internal int int_0;

		public Class80(int int_1, Class71 class71_1)
		{
			class71_0 = class71_1;
			int_0 = int_1;
			enum6_0 = (Enum6)7;
		}

		internal override void vmethod_9(Class73 class73_0)
		{
			if (class73_0 is Class80)
			{
				class71_0 = ((Class80)class73_0).class71_0;
				int_0 = ((Class80)class73_0).int_0;
				return;
			}
			Class65 @class = class71_0.class68_0.list_1[int_0];
			if (class73_0 is Class79 && (int)(@class.enum3_0 & (Enum3)226) > 0)
			{
				Class73 class73_ = (class73_0 as Class79).vmethod_7();
				vmethod_11(class73_);
			}
			else
			{
				vmethod_11(class73_0);
			}
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_11(class73_0);
		}

		internal override IntPtr vmethod_10()
		{
			throw new NotImplementedException();
		}

		internal override void vmethod_11(Class73 class73_0)
		{
			class71_0.class73_1[int_0] = class73_0;
		}

		internal override object vmethod_4(Type type_0)
		{
			if (class71_0.class73_1[int_0] == null)
			{
				return null;
			}
			return vmethod_7().vmethod_4(type_0);
		}

		internal override Class73 vmethod_7()
		{
			if (class71_0.class73_1[int_0] != null)
			{
				return class71_0.class73_1[int_0].vmethod_7();
			}
			return new Class85(null);
		}

		internal override bool vmethod_8()
		{
			return vmethod_7().vmethod_8();
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.vmethod_0())
			{
				return false;
			}
			if (class73_0 is Class80)
			{
				if (((Class80)class73_0).int_0 == int_0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				if (class73_0 is Class80)
				{
					if (((Class80)class73_0).int_0 != int_0)
					{
						return true;
					}
					return false;
				}
				return true;
			}
			return true;
		}

		internal override bool vmethod_6()
		{
			return vmethod_7().vmethod_6();
		}
	}

	internal class Class81 : Class79
	{
		private Array array_0;

		internal int int_0;

		public Class81(int int_1, Array array_1)
		{
			array_0 = array_1;
			int_0 = int_1;
			enum6_0 = (Enum6)7;
		}

		internal override IntPtr vmethod_10()
		{
			throw new NotImplementedException();
		}

		internal override void vmethod_9(Class73 class73_0)
		{
			if (class73_0 is Class81)
			{
				array_0 = ((Class81)class73_0).array_0;
				int_0 = ((Class81)class73_0).int_0;
			}
			else
			{
				vmethod_11(class73_0);
			}
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_11(class73_0);
		}

		internal override void vmethod_11(Class73 class73_0)
		{
			array_0.SetValue(class73_0.vmethod_4(null), int_0);
		}

		internal override object vmethod_4(Type type_0)
		{
			return vmethod_7().vmethod_4(type_0);
		}

		internal override Class73 vmethod_7()
		{
			return Class73.smethod_1(array_0.GetType().GetElementType(), array_0.GetValue(int_0));
		}

		internal override bool vmethod_8()
		{
			return vmethod_7().vmethod_8();
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.vmethod_0())
			{
				return false;
			}
			if (!(class73_0 is Class81))
			{
				return false;
			}
			Class81 @class = (Class81)class73_0;
			if (@class.int_0 != int_0)
			{
				return false;
			}
			if (@class.array_0 != array_0)
			{
				return false;
			}
			return true;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (!class73_0.vmethod_0())
			{
				return true;
			}
			if (!(class73_0 is Class81))
			{
				return true;
			}
			Class81 @class = (Class81)class73_0;
			if (@class.int_0 != int_0)
			{
				return true;
			}
			if (@class.array_0 != array_0)
			{
				return true;
			}
			return false;
		}

		internal override bool vmethod_6()
		{
			return vmethod_7().vmethod_6();
		}
	}

	internal class Class82 : Class79
	{
		internal FieldInfo fieldInfo_0;

		internal object object_0;

		public Class82(FieldInfo fieldInfo_1, object object_1)
		{
			fieldInfo_0 = fieldInfo_1;
			object_0 = object_1;
			enum6_0 = (Enum6)7;
		}

		internal override IntPtr vmethod_10()
		{
			throw new NotImplementedException();
		}

		internal override void vmethod_11(Class73 class73_0)
		{
			if (object_0 != null && object_0 is Class73)
			{
				fieldInfo_0.SetValue(((Class73)object_0).vmethod_4(null), class73_0.vmethod_4(null));
			}
			else
			{
				fieldInfo_0.SetValue(object_0, class73_0.vmethod_4(null));
			}
		}

		internal override void vmethod_9(Class73 class73_0)
		{
			if (!(class73_0 is Class82))
			{
				vmethod_11(class73_0);
				return;
			}
			fieldInfo_0 = ((Class82)class73_0).fieldInfo_0;
			object_0 = ((Class82)class73_0).object_0;
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_11(class73_0);
		}

		internal override object vmethod_4(Type type_0)
		{
			return vmethod_7().vmethod_4(type_0);
		}

		internal override Class73 vmethod_7()
		{
			if (object_0 != null && object_0 is Class73)
			{
				return Class73.smethod_1(fieldInfo_0.FieldType, fieldInfo_0.GetValue(((Class73)object_0).vmethod_4(null)));
			}
			return Class73.smethod_1(fieldInfo_0.FieldType, fieldInfo_0.GetValue(object_0));
		}

		internal override bool vmethod_8()
		{
			return vmethod_7().vmethod_8();
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.vmethod_0())
			{
				return false;
			}
			if (!(class73_0 is Class82))
			{
				return false;
			}
			Class82 @class = (Class82)class73_0;
			if (@class.fieldInfo_0 != fieldInfo_0)
			{
				return false;
			}
			if (@class.object_0 != object_0)
			{
				return false;
			}
			return true;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				if (class73_0 is Class82)
				{
					Class82 @class = (Class82)class73_0;
					if (!(@class.fieldInfo_0 != fieldInfo_0))
					{
						if (@class.object_0 != object_0)
						{
							return true;
						}
						return false;
					}
					return true;
				}
				return true;
			}
			return true;
		}

		internal override bool vmethod_6()
		{
			return vmethod_7().vmethod_6();
		}
	}

	internal class Class83 : Class79
	{
		private Class71 class71_0;

		internal int int_0;

		public Class83(int int_1, Class71 class71_1)
		{
			class71_0 = class71_1;
			int_0 = int_1;
			enum6_0 = (Enum6)7;
		}

		internal override IntPtr vmethod_10()
		{
			throw new NotImplementedException();
		}

		internal override void vmethod_9(Class73 class73_0)
		{
			if (class73_0 is Class83)
			{
				class71_0 = ((Class83)class73_0).class71_0;
				int_0 = ((Class83)class73_0).int_0;
			}
			else
			{
				vmethod_11(class73_0);
			}
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_11(class73_0);
		}

		internal override void vmethod_11(Class73 class73_0)
		{
			class71_0.class73_0[int_0] = class73_0;
		}

		internal override object vmethod_4(Type type_0)
		{
			if (class71_0.class73_0[int_0] != null)
			{
				return vmethod_7().vmethod_4(type_0);
			}
			return null;
		}

		internal override Class73 vmethod_7()
		{
			if (class71_0.class73_0[int_0] == null)
			{
				return new Class85(null);
			}
			return class71_0.class73_0[int_0].vmethod_7();
		}

		internal override bool vmethod_8()
		{
			return vmethod_7().vmethod_8();
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (!class73_0.vmethod_0())
			{
				return false;
			}
			if (class73_0 is Class83)
			{
				return ((Class83)class73_0).int_0 == int_0;
			}
			return false;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				if (class73_0 is Class83)
				{
					return ((Class83)class73_0).int_0 != int_0;
				}
				return true;
			}
			return true;
		}

		internal override bool vmethod_6()
		{
			return vmethod_7().vmethod_6();
		}
	}

	internal class Class84 : Class79
	{
		private Class73 class73_0;

		private Type type_0;

		public Class84(Class73 class73_1, Type type_1)
		{
			class73_0 = class73_1;
			type_0 = type_1;
			enum6_0 = (Enum6)7;
		}

		internal override IntPtr vmethod_10()
		{
			throw new NotImplementedException();
		}

		internal override void vmethod_9(Class73 class73_1)
		{
			if (!(class73_1 is Class84))
			{
				class73_0.vmethod_9(class73_1);
				return;
			}
			type_0 = ((Class84)class73_1).type_0;
			class73_0 = ((Class84)class73_1).class73_0;
		}

		internal override void vmethod_2(Class73 class73_1)
		{
			vmethod_11(class73_1);
		}

		internal override void vmethod_11(Class73 class73_1)
		{
			class73_0 = class73_1;
		}

		internal override object vmethod_4(Type type_1)
		{
			if (class73_0 != null)
			{
				if (!(type_1 == null) && !(type_1 == typeof(object)))
				{
					return class73_0.vmethod_4(type_1);
				}
				return class73_0.vmethod_4(type_0);
			}
			return new Class85(null);
		}

		internal override Class73 vmethod_7()
		{
			if (class73_0 != null)
			{
				return class73_0.vmethod_7();
			}
			return new Class85(null);
		}

		internal override bool vmethod_8()
		{
			return vmethod_7().vmethod_8();
		}

		internal override bool vmethod_5(Class73 class73_1)
		{
			if (class73_1.vmethod_0())
			{
				if (class73_1 is Class84)
				{
					Class84 @class = (Class84)class73_1;
					if (!(@class.type_0 != type_0))
					{
						if (class73_0 == null)
						{
							if (@class.class73_0 == null)
							{
								return true;
							}
							return false;
						}
						return class73_0.vmethod_5(@class.class73_0);
					}
					return false;
				}
				return false;
			}
			return false;
		}

		internal override bool BeouTiljCp(Class73 class73_1)
		{
			if (class73_1.vmethod_0())
			{
				if (!(class73_1 is Class84))
				{
					return true;
				}
				Class84 @class = (Class84)class73_1;
				if (!(@class.type_0 != type_0))
				{
					if (class73_0 != null)
					{
						return class73_0.BeouTiljCp(@class.class73_0);
					}
					if (@class.class73_0 != null)
					{
						return true;
					}
					return false;
				}
				return true;
			}
			return true;
		}

		internal override bool vmethod_6()
		{
			return vmethod_7().vmethod_6();
		}
	}

	internal class Class64
	{
		public int int_0;

		public bool bool_0;

		public Enum3 enum3_0;
	}

	internal class Class65
	{
		public int int_0;

		public Enum3 enum3_0;

		public bool bool_0;

		public Type type_0 = typeof(object);
	}

	internal class Class66
	{
		public int int_0;

		public int int_1;

		public Class67 class67_0;
	}

	internal class Class67
	{
		public int int_0;

		public int int_1;

		public byte byte_0;

		public Type type_0;

		public int int_2;

		public int int_3;
	}

	internal class Class68
	{
		internal object object_0;

		internal List<Class63> list_0;

		internal Class64[] class64_0;

		internal List<Class65> list_1;

		internal List<Class66> list_2;
	}

	private class Class69
	{
		internal object object_0;

		internal int int_0;

		public Class69(FieldInfo fieldInfo_0, int int_1)
		{
			object_0 = fieldInfo_0;
			int_0 = int_1;
		}
	}

	private class Class70
	{
		private List<Class69> list_0 = new List<Class69>();

		private MethodBase methodBase_0;

		public Class70(MethodBase methodBase_1, List<Class69> list_1)
		{
			list_0 = list_1;
			methodBase_0 = methodBase_1;
		}

		public Class70(MethodBase methodBase_1, Class69[] class69_0)
		{
			list_0.AddRange(class69_0);
		}

		public override bool Equals(object obj)
		{
			Class70 @class = obj as Class70;
			if (obj == null)
			{
				return false;
			}
			if (!(methodBase_0 != @class.methodBase_0))
			{
				if (list_0.Count != @class.list_0.Count)
				{
					return false;
				}
				int num = 0;
				while (true)
				{
					if (num < list_0.Count)
					{
						if ((FieldInfo)list_0[num].object_0 != (FieldInfo)@class.list_0[num].object_0)
						{
							break;
						}
						if (list_0[num].int_0 == @class.list_0[num].int_0)
						{
							num++;
							continue;
						}
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = methodBase_0.GetHashCode();
			foreach (Class69 item in list_0)
			{
				int num2 = item.object_0.GetHashCode() + item.int_0;
				num = (num ^ num2) + num2;
			}
			return num;
		}

		public Class69 method_0(int int_0)
		{
			foreach (Class69 item in list_0)
			{
				if (item.int_0 == int_0)
				{
					return item;
				}
			}
			return null;
		}

		public bool method_1(int int_0)
		{
			foreach (Class69 item in list_0)
			{
				if (item.int_0 == int_0)
				{
					return true;
				}
			}
			return false;
		}
	}

	private delegate object Delegate10(object target, object[] paramters);

	private delegate object Delegate11(object target);

	private delegate void Delegate12(IntPtr a, byte b, int c);

	private delegate void Delegate13(IntPtr s, IntPtr t, uint c);

	internal class Class71
	{
		[Serializable]
		[CompilerGenerated]
		private sealed class Class72
		{
			public static readonly Class72 _003C_003E9;

			public static Comparison<Class66> _003C_003E9__12_0;

			static Class72()
			{
				_003C_003E9 = new Class72();
			}

			internal int method_0(Class66 x, Class66 y)
			{
				return x.class67_0.int_0.CompareTo(y.class67_0.int_0);
			}
		}

		internal Class68 class68_0;

		internal Class73[] class73_0 = new Class73[0];

		internal Class73[] class73_1 = new Class73[0];

		internal Class87 class87_0 = new Class87();

		internal Class73 class73_2;

		internal Exception exception_0;

		internal List<IntPtr> list_0;

		private int int_0;

		private int int_1;

		private int int_2 = -1;

		private object object_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private bool bool_3;

		private static Dictionary<Type, int> dictionary_0;

		private static object object_1;

		private static Dictionary<object, Class73> dictionary_1;

		private static object object_2;

		private static Dictionary<MethodBase, Delegate10> dictionary_2;

		private static Dictionary<MethodBase, Delegate10> dictionary_3;

		private static object object_3;

		private static Dictionary<Class70, Delegate10> dictionary_4;

		private static Dictionary<Class70, Delegate10> dictionary_5;

		private static object object_4;

		private static Dictionary<Class70, Delegate10> dictionary_6;

		private static object object_5;

		private static Dictionary<Type, Delegate11> dictionary_7;

		private static object object_6;

		private static Delegate12 delegate12_0;

		private static Delegate13 delegate13_0;

		internal void method_0()
		{
			bool bool_ = false;
			method_2(ref bool_);
		}

		internal void method_1()
		{
			class87_0.method_1();
			class73_1 = null;
			if (list_0 == null)
			{
				return;
			}
			foreach (IntPtr item in list_0)
			{
				try
				{
					Marshal.FreeHGlobal(item);
				}
				catch
				{
				}
			}
			list_0.Clear();
			list_0 = null;
		}

		internal void method_2(ref bool bool_4)
		{
			while (true)
			{
				if (int_0 > -2)
				{
					if (bool_0)
					{
						bool_0 = false;
						int num = int_1;
						int num2 = int_0;
						method_4(int_1, int_0);
						int_0 = num2;
						int_1 = num;
					}
					if (bool_2)
					{
						break;
					}
					if (!bool_1)
					{
						int_1 = int_0;
						Class63 @class = class68_0.list_0[int_0];
						object_0 = @class.object_0;
						try
						{
							method_7(@class);
						}
						catch (Exception innerException)
						{
							if (innerException is TargetInvocationException)
							{
								TargetInvocationException ex = (TargetInvocationException)innerException;
								if (ex.InnerException != null)
								{
									innerException = ex.InnerException;
								}
							}
							exception_0 = innerException;
							bool_4 = true;
							class87_0.method_1();
							int int_ = int_1;
							Class66 class2 = method_5(int_, innerException);
							List<Class66> list = method_6(int_, bool_4: false);
							List<Class66> list2 = new List<Class66>();
							if (class2 != null)
							{
								list2.Add(class2);
							}
							if (list != null && list.Count > 0)
							{
								list2.AddRange(list);
							}
							list2.Sort((Class66 x, Class66 y) => x.class67_0.int_0.CompareTo(y.class67_0.int_0));
							Class66 class3 = null;
							foreach (Class66 item in list2)
							{
								if (item.class67_0.int_3 != 0)
								{
									class87_0.method_2(new Class85(innerException));
									int_1 = item.class67_0.int_2;
									int_0 = int_1;
									method_0();
									if (bool_3)
									{
										bool_3 = false;
										class3 = item;
										break;
									}
									continue;
								}
								class3 = item;
								break;
							}
							if (class3 != null)
							{
								int_2 = class3.class67_0.int_0;
								method_3(int_, class3.class67_0.int_0);
								if (int_2 >= 0)
								{
									class87_0.method_2(new Class85(innerException));
									int_1 = int_2;
									int_0 = int_1;
									int_2 = -1;
									method_0();
								}
								return;
							}
							throw innerException;
						}
						int_0++;
						continue;
					}
					bool_1 = false;
					return;
				}
				class87_0.method_1();
				return;
			}
			bool_2 = false;
		}

		internal void method_3(int int_3, int int_4)
		{
			if (class68_0.list_2 == null)
			{
				return;
			}
			foreach (Class66 item in class68_0.list_2)
			{
				if ((item.class67_0.int_3 == 4 || item.class67_0.int_3 == 2) && item.class67_0.int_0 >= int_3 && item.class67_0.int_1 <= int_4)
				{
					int_1 = item.class67_0.int_0;
					int_0 = int_1;
					bool bool_ = false;
					method_2(ref bool_);
					if (bool_)
					{
						break;
					}
				}
			}
		}

		internal void method_4(int int_3, int int_4)
		{
			if (class68_0.list_2 == null)
			{
				return;
			}
			foreach (Class66 item in class68_0.list_2)
			{
				if (item.class67_0.int_3 == 2 && item.class67_0.int_0 >= int_3 && item.class67_0.int_1 <= int_4)
				{
					int_1 = item.class67_0.int_0;
					int_0 = int_1;
					bool bool_ = false;
					method_2(ref bool_);
					if (bool_)
					{
						break;
					}
				}
			}
		}

		internal Class66 method_5(int int_3, Exception exception_1)
		{
			Class66 @class = null;
			if (class68_0.list_2 != null)
			{
				foreach (Class66 item in class68_0.list_2)
				{
					if (item.class67_0 != null && item.class67_0.int_3 == 0 && (item.class67_0.type_0 == exception_1.GetType() || (item.class67_0.type_0 != null && (item.class67_0.type_0.FullName == exception_1.GetType().FullName || item.class67_0.type_0.FullName == typeof(object).FullName || item.class67_0.type_0.FullName == typeof(Exception).FullName))) && int_3 >= item.int_0 && int_3 <= item.int_1)
					{
						if (@class == null)
						{
							@class = item;
						}
						else if (item.class67_0.int_0 < @class.class67_0.int_0)
						{
							@class = item;
						}
					}
				}
				return @class;
			}
			return @class;
		}

		internal List<Class66> method_6(int int_3, bool bool_4)
		{
			if (class68_0.list_2 == null)
			{
				return null;
			}
			List<Class66> list = new List<Class66>();
			foreach (Class66 item in class68_0.list_2)
			{
				if ((item.class67_0.int_3 & 1) == 1 && int_3 >= item.int_0 && int_3 <= item.int_1)
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list;
		}

		private unsafe void method_7(Class63 class63_0)
		{
			switch (class63_0.enum5_0)
			{
			case (Enum5)0:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_51());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)1:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				object value = class87_0.method_4().vmethod_4(fieldInfo_.FieldType);
				fieldInfo_.SetValue(null, value);
				break;
			}
			case (Enum5)2:
				class87_0.method_2(new Class83((int)object_0, this));
				break;
			case (Enum5)3:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				Class73 class11 = class87_0.method_4();
				class11.vmethod_7();
				object value = class11.vmethod_4(null);
				class87_0.method_2(new Class82(fieldInfo_, value));
				break;
			}
			case (Enum5)4:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_48();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)5:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_23();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)6:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_24());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)7:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_26());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)8:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				if (class87_0.method_4() is Class79 class4)
				{
					if (!elementType.IsValueType)
					{
						class4.vmethod_11(new Class85(null));
						break;
					}
					object value = Activator.CreateInstance(elementType);
					class4.vmethod_11(Class73.smethod_1(elementType, value));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)9:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_35());
				break;
			}
			case (Enum5)10:
				if ((smethod_1(class87_0.method_3()) ?? throw new ArithmeticException(((Enum7)0).ToString())) is Class78 class9)
				{
					if (double.IsNaN(class9.double_0))
					{
						throw new OverflowException(((Enum7)2).ToString());
					}
					if (double.IsInfinity(class9.double_0))
					{
						throw new OverflowException(((Enum7)1).ToString());
					}
				}
				break;
			case (Enum5)11:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.Add(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)12:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_43());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)13:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_64(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)14:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_47());
				break;
			}
			case (Enum5)15:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_48());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					if (IntPtr.Size == 8)
					{
						long long_ = *(long*)(void*)intPtr;
						class87_0.method_2(new Class77(long_, (Enum3)12));
					}
					else
					{
						int metadataToken = *(int*)(void*)intPtr;
						class87_0.method_2(new Class77(metadataToken, (Enum3)12));
					}
					break;
				}
				throw new Exception1();
			}
			case (Enum5)16:
			{
				bool flag = false;
				Class73 @class = class87_0.method_4();
				if (@class == null || !@class.vmethod_6())
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)17:
				throw (Exception)class87_0.method_4().vmethod_4(null);
			case (Enum5)18:
				class87_0.method_2(((Class74)class87_0.method_4()).vmethod_54());
				break;
			case (Enum5)20:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Array obj6 = (Array)class87_0.method_4().vmethod_4(null);
				object value = obj6.GetValue(class2.vmethod_18().struct7_0.int_0);
				Type elementType = obj6.GetType().GetElementType();
				class87_0.method_2(Class73.smethod_1(elementType, value));
				break;
			}
			case (Enum5)21:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_22());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)24:
				int_0 = (int)object_0 - 1;
				break;
			case (Enum5)25:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(class87_0.method_4());
				((Array)class87_0.method_4().vmethod_4(null)).SetValue(@class.vmethod_4(elementType), class2.vmethod_18().struct7_0.int_0);
				break;
			}
			case (Enum5)26:
				if (class87_0.method_4().BeouTiljCp(class87_0.method_4()))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			case (Enum5)27:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_53());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)28:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_27());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)29:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_45();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)30:
			{
				int metadataToken = (int)object_0;
				class73_1[metadataToken] = method_8(class87_0.method_4(), class68_0.list_1[metadataToken].enum3_0, class68_0.list_1[metadataToken].bool_0);
				break;
			}
			case (Enum5)31:
				class87_0.method_2(class73_0[(int)object_0]);
				break;
			case (Enum5)32:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				class87_0.method_2(new Class82(fieldInfo_, null));
				break;
			}
			case (Enum5)33:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_55(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)34:
				class87_0.method_2(class87_0.method_4().vmethod_7());
				break;
			case (Enum5)35:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_46();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)36:
				class87_0.method_2(new Class75((int)object_0));
				break;
			case (Enum5)37:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(short), value));
				break;
			}
			case (Enum5)38:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_76(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)39:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_50());
				break;
			}
			case (Enum5)40:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_65(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)41:
			{
				int metadataToken = (int)object_0;
				ConstructorInfo constructorInfo = (ConstructorInfo)typeof(Class62).Module.ResolveMethod(metadataToken);
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				object[] array3 = new object[parameters.Length];
				Class73[] array4 = new Class73[parameters.Length];
				List<Class69> list = null;
				Class70 class6 = null;
				for (int i = 0; i < parameters.Length; i++)
				{
					Class73 @class = class87_0.method_4();
					Type elementType = parameters[parameters.Length - 1 - i].ParameterType;
					object obj2 = null;
					bool flag = false;
					if (elementType.IsByRef && @class is Class82 class7)
					{
						if (list == null)
						{
							list = new List<Class69>();
						}
						list.Add(new Class69(class7.fieldInfo_0, parameters.Length - 1 - i));
						obj2 = class7.object_0;
						if (obj2 is Class73)
						{
							@class = obj2 as Class73;
						}
						else
						{
							flag = true;
						}
					}
					if (!flag)
					{
						if (@class != null)
						{
							obj2 = @class.vmethod_4(elementType);
						}
						if (obj2 == null)
						{
							if (elementType.IsByRef)
							{
								elementType = elementType.GetElementType();
							}
							if (elementType.IsValueType)
							{
								obj2 = Activator.CreateInstance(elementType);
								if (@class is Class80)
								{
									((Class79)@class).vmethod_11(Class73.smethod_1(elementType, obj2));
								}
							}
						}
					}
					array4[array3.Length - 1 - i] = @class;
					array3[array3.Length - 1 - i] = obj2;
				}
				Delegate10 @delegate = null;
				if (list != null)
				{
					class6 = new Class70(constructorInfo, list);
					@delegate = smethod_4(constructorInfo, bool_4: true, class6);
				}
				object value = null;
				value = ((@delegate != null) ? @delegate(null, array3) : constructorInfo.Invoke(array3));
				for (int j = 0; j < parameters.Length; j++)
				{
					if (parameters[j].ParameterType.IsByRef && (class6 == null || !class6.method_1(j)))
					{
						if (array4[j].method_2())
						{
							((Class77)array4[j]).method_5(Class73.smethod_1(parameters[j].ParameterType, array3[j]));
						}
						else if (array4[j] is Class80)
						{
							array4[j].vmethod_9(Class73.smethod_1(parameters[j].ParameterType.GetElementType(), array3[j]));
						}
						else
						{
							array4[j].vmethod_9(Class73.smethod_1(parameters[j].ParameterType, array3[j]));
						}
					}
				}
				class87_0.method_2(Class73.smethod_1(constructorInfo.DeclaringType, value));
				break;
			}
			case (Enum5)42:
			{
				int metadataToken = (int)object_0;
				typeof(Class62).Module.ResolveType(metadataToken);
				Class74 class2 = smethod_1(class87_0.method_4());
				Array array = (Array)class87_0.method_4().vmethod_4(null);
				class87_0.method_2(new Class81(class2.vmethod_18().struct7_0.int_0, array));
				break;
			}
			case (Enum5)43:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_41());
				break;
			}
			case (Enum5)44:
			{
				int metadataToken = (int)object_0;
				Type elementType2 = typeof(Class62).Module.ResolveType(metadataToken);
				Class74 class2 = smethod_1(class87_0.method_4());
				Array array = Array.CreateInstance(elementType2, class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(new Class85(array));
				break;
			}
			case (Enum5)45:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(byte), value));
				break;
			}
			case (Enum5)47:
			{
				Class74 class2 = class87_0.method_4() as Class74;
				IntPtr intPtr = bltXeOrHnB(class87_0.method_4());
				IntPtr intPtr2 = bltXeOrHnB(class87_0.method_4());
				if (intPtr != IntPtr.Zero && intPtr2 != IntPtr.Zero)
				{
					uint uint_ = class2.vmethod_19().struct7_0.uint_0;
					smethod_10(intPtr2, intPtr, uint_);
				}
				break;
			}
			case (Enum5)48:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_37());
				break;
			}
			case (Enum5)49:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_29());
				break;
			}
			case (Enum5)50:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				Class73 class73_ = class87_0.method_4();
				Class74 class3 = smethod_1(class73_);
				if (class3 != null && class2 != null)
				{
					if (class3.lwlumgaheq(@class))
					{
						int_0 = (int)object_0 - 1;
					}
				}
				else if (@class.BeouTiljCp(class73_))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)51:
				throw exception_0;
			case (Enum5)52:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(uint), value));
				break;
			}
			case (Enum5)53:
			{
				Class73 @class = class73_1[(int)object_0];
				class87_0.method_2(@class);
				break;
			}
			case (Enum5)54:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_73(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)55:
				class87_0.method_2(new Class78((double)object_0));
				break;
			case (Enum5)56:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				Class73 @class = class87_0.method_4();
				object value = @class.vmethod_4(null);
				if (value != null)
				{
					if (!elementType.IsAssignableFrom(value.GetType()))
					{
						class87_0.method_2(new Class85(null));
					}
					else
					{
						class87_0.method_2(@class);
					}
				}
				else
				{
					class87_0.method_2(new Class85(null));
				}
				break;
			}
			case (Enum5)57:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_45());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class78(*(float*)(void*)intPtr, (Enum3)9));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)58:
			{
				Class73 class10 = class87_0.method_4();
				Class73 @class = class87_0.method_4();
				if (class10.vmethod_5(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)59:
			{
				int metadataToken = (int)object_0;
				Module module = typeof(Class62).Module;
				object value = null;
				try
				{
					value = module.ResolveType(metadataToken);
				}
				catch
				{
					try
					{
						value = module.ResolveMethod(metadataToken);
					}
					catch
					{
						try
						{
							value = module.ResolveField(metadataToken);
							goto end_IL_1363;
						}
						catch
						{
							value = module.ResolveMember(metadataToken);
							goto end_IL_1363;
						}
						end_IL_1363:;
					}
				}
				class87_0.method_2(new Class85(value));
				break;
			}
			case (Enum5)60:
				class87_0.method_2(new Class76((long)object_0));
				break;
			case (Enum5)61:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_25();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)62:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_49());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)63:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_42());
				break;
			}
			case (Enum5)64:
			{
				IntPtr intPtr = Marshal.AllocHGlobal((class87_0.method_4() as Class74).vmethod_18().struct7_0.int_0);
				if (list_0 == null)
				{
					list_0 = new List<IntPtr>();
				}
				list_0.Add(intPtr);
				class87_0.method_2(new Class77(intPtr));
				break;
			}
			case (Enum5)65:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_45());
				break;
			}
			case (Enum5)66:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_80(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)67:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_59(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)68:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				object value = class87_0.method_4().vmethod_7().vmethod_4(elementType);
				Class73 @class = Class73.smethod_1(elementType, value);
				class87_0.method_2(@class);
				break;
			}
			case (Enum5)69:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_63(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)70:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_28());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(uint*)(void*)intPtr, (Enum3)6));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)71:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_72(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)72:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_38());
				break;
			}
			case (Enum5)73:
			{
				Class73 @class = class87_0.method_4();
				if (!@class.vmethod_0())
				{
					throw new Exception1();
				}
				object value = @class.vmethod_4(null);
				@class = ((value != null) ? Class73.smethod_1(value.GetType(), value) : new Class85(null));
				class87_0.method_2(@class);
				break;
			}
			case (Enum5)74:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_62(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)75:
				class87_0.method_2(class87_0.method_3());
				break;
			case (Enum5)76:
				method_12(bool_4: true);
				break;
			case (Enum5)77:
			{
				Type elementType = typeof(Class62).Module.ResolveType((int)object_0);
				object value = class87_0.method_4().vmethod_4(elementType);
				if (value == null)
				{
					value = Activator.CreateInstance(elementType);
				}
				Class85 class12 = new Class85(Class73.smethod_1(elementType, smethod_8(value)));
				class87_0.method_2(class12);
				break;
			}
			case (Enum5)78:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_22());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(sbyte*)(void*)intPtr, (Enum3)1));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)79:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(elementType, value));
				break;
			}
			case (Enum5)80:
				lock (object_2)
				{
					object obj2 = class87_0.method_4().vmethod_4(null);
					Class73 @class = null;
					if (dictionary_1.TryGetValue(obj2, out @class))
					{
						class87_0.method_2(@class);
					}
					else
					{
						class87_0.method_2(new Class85(null));
					}
					break;
				}
			case (Enum5)82:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_34());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)83:
				class87_0.method_2(new Class85(null));
				break;
			case (Enum5)84:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class2 != null && class3 != null)
				{
					class87_0.method_2(class2.vmethod_68(class3));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)85:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(IntPtr), value));
				break;
			}
			case (Enum5)86:
			{
				int metadataToken = (int)object_0;
				MethodBase methodBase = typeof(Class62).Module.ResolveMethod(metadataToken);
				Type elementType = class87_0.method_4().vmethod_4(null).GetType();
				List<Type> list2 = new List<Type>();
				do
				{
					list2.Add(elementType);
					elementType = elementType.BaseType;
				}
				while (elementType != null && elementType != methodBase.DeclaringType);
				list2.Reverse();
				MethodBase methodBase2 = methodBase;
				foreach (Type item in list2)
				{
					MethodInfo[] methods = item.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (MethodInfo methodInfo in methods)
					{
						if (methodInfo.GetBaseDefinition() == methodBase2)
						{
							methodBase2 = methodInfo;
							break;
						}
					}
				}
				class87_0.method_2(new Class77(methodBase2.MethodHandle.GetFunctionPointer()));
				break;
			}
			case (Enum5)87:
				class87_0.method_2(new Class80((int)object_0, this));
				break;
			case (Enum5)88:
			{
				Class73 @class = class87_0.method_4();
				if (@class != null && @class.vmethod_6())
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)89:
				bool_2 = true;
				break;
			case (Enum5)90:
				class87_0.method_2(new Class78((float)object_0));
				break;
			case (Enum5)91:
			{
				Class74 class2 = class87_0.method_4() as Class74;
				Class74 class3 = class87_0.method_4() as Class74;
				IntPtr intPtr = bltXeOrHnB(class87_0.method_4());
				if (intPtr != IntPtr.Zero)
				{
					byte byte_ = class3.vmethod_15().struct7_0.byte_0;
					uint uint_ = class2.vmethod_19().struct7_0.uint_0;
					smethod_9(intPtr, byte_, (int)uint_);
				}
				break;
			}
			case (Enum5)92:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_40());
				break;
			}
			case (Enum5)93:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_79(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)95:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_56(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)96:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_75(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)97:
				if (Class62.list_0.Count == 0)
				{
					Module module = typeof(Class62).Module;
					class87_0.method_2(new Class86(module.ResolveString((int)object_0 | 0x70000000)));
				}
				else
				{
					class87_0.method_2(new Class86(Class62.list_0[(int)object_0]));
				}
				break;
			case (Enum5)98:
			{
				Class73 class8 = smethod_6(class87_0.method_4());
				Class73 @class = smethod_6(class87_0.method_4());
				if (!class8.vmethod_5(@class))
				{
					class87_0.method_2(new Class75(0));
				}
				else
				{
					class87_0.method_2(new Class75(1));
				}
				break;
			}
			case (Enum5)99:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class2 != null && class3 != null)
				{
					class87_0.method_2(class2.vmethod_70(class3));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)100:
			{
				int metadataToken = (int)object_0;
				Module module = typeof(Class62).Module;
				class87_0.method_2(new Class77(module.ResolveMethod(metadataToken).MethodHandle.GetFunctionPointer()));
				break;
			}
			case (Enum5)101:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_81(@class))
				{
					class87_0.method_2(new Class75(1));
				}
				else
				{
					class87_0.method_2(new Class75(0));
				}
				break;
			}
			case (Enum5)102:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_31());
				break;
			}
			case (Enum5)103:
				lock (object_2)
				{
					Class73 @class = class87_0.method_4();
					object obj2 = class87_0.method_4().vmethod_4(null);
					dictionary_1[obj2] = @class;
					break;
				}
			case (Enum5)104:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = (Class74)class87_0.method_4();
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_58(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)105:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_32());
				break;
			}
			case (Enum5)106:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(long), value));
				break;
			}
			case (Enum5)107:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_24();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)109:
			{
				int metadataToken = (int)object_0;
				if (((MethodBase)class68_0.object_0).IsStatic)
				{
					class73_0[metadataToken] = method_8(class87_0.method_4(), class68_0.class64_0[metadataToken].enum3_0);
				}
				else
				{
					class73_0[metadataToken] = method_8(class87_0.method_4(), class68_0.class64_0[metadataToken - 1].enum3_0);
				}
				break;
			}
			case (Enum5)110:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_57(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)111:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_61(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)112:
				int_0 = -3;
				if (class87_0.method_0() > 0)
				{
					class73_2 = class87_0.method_4();
				}
				break;
			case (Enum5)113:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(ushort), value));
				break;
			}
			case (Enum5)114:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_23());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(short*)(void*)intPtr, (Enum3)3));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)115:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_39());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)117:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_52());
				break;
			}
			case (Enum5)118:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(sbyte), value));
				break;
			}
			case (Enum5)119:
				class87_0.method_4();
				break;
			case (Enum5)122:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_44());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)123:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				object value = ((class87_0.method_4() as Class79) ?? throw new Exception1()).vmethod_4(elementType);
				Class73 @class;
				if (value != null)
				{
					if (elementType.IsValueType)
					{
						value = smethod_8(value);
					}
					@class = Class73.smethod_1(elementType, value);
				}
				else if (!elementType.IsValueType)
				{
					@class = new Class85(null);
				}
				else
				{
					value = Activator.CreateInstance(elementType);
					@class = Class73.smethod_1(elementType, value);
				}
				class87_0.method_2(@class);
				break;
			}
			case (Enum5)125:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_25());
				break;
			}
			case (Enum5)126:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_77(@class))
				{
					class87_0.method_2(new Class75(1));
				}
				else
				{
					class87_0.method_2(new Class75(0));
				}
				break;
			}
			case (Enum5)127:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_80(@class))
				{
					class87_0.method_2(new Class75(1));
				}
				else
				{
					class87_0.method_2(new Class75(0));
				}
				break;
			}
			case (Enum5)128:
			{
				Class73 @class = class87_0.method_4();
				bool num = smethod_1(class87_0.method_4()).vmethod_81(@class);
				if (!num)
				{
					class87_0.method_2(new Class75(0));
				}
				else
				{
					class87_0.method_2(new Class75(1));
				}
				if (num)
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)129:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_36());
				break;
			}
			case (Enum5)130:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.usfdqHavse());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)133:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_30());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)23:
			case (Enum5)134:
			{
				int metadataToken = (int)object_0;
				Type elementType = typeof(Class62).Module.ResolveType(metadataToken);
				Class73 @class = class87_0.method_4();
				object value = @class.vmethod_4(elementType);
				if (value != null)
				{
					if (elementType.IsValueType)
					{
						value = smethod_8(value);
					}
					@class = Class73.smethod_1(elementType, value);
				}
				else if (!elementType.IsValueType)
				{
					@class = new Class85(null);
				}
				else
				{
					value = Activator.CreateInstance(elementType);
					@class = Class73.smethod_1(elementType, value);
				}
				((class87_0.method_4() as Class79) ?? throw new Exception1()).vmethod_9(@class);
				break;
			}
			case (Enum5)135:
				int_0 = (int)object_0 - 1;
				bool_0 = true;
				break;
			case (Enum5)136:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_60(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)137:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_25());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class76(*(long*)(void*)intPtr, (Enum3)7));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)138:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_69());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)139:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_66(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)140:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				object value = class87_0.method_4().vmethod_4(null);
				class87_0.method_2(Class73.smethod_1(fieldInfo_.FieldType, fieldInfo_.GetValue(value)));
				break;
			}
			case (Enum5)141:
			{
				Class73 @class = class87_0.method_4();
				if (@class.vmethod_3())
				{
					@class = ((Class74)@class).vmethod_22();
				}
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)142:
			{
				Array array = (Array)class87_0.method_4().vmethod_4(null);
				class87_0.method_2(new Class75(array.Length, (Enum3)5));
				break;
			}
			case (Enum5)144:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_48());
				break;
			}
			case (Enum5)145:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_28());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)146:
			{
				int[] array2 = (int[])object_0;
				Class74 class2 = smethod_1(class87_0.method_4());
				long long_ = class2.vmethod_20().struct8_0.long_0;
				if ((long_ < 0L || class2.method_4()) && IntPtr.Size == 4)
				{
					long_ = (int)long_;
				}
				if (class2.method_1())
				{
					Class75 class5 = (Class75)class2;
					if (class5.enum3_0 == (Enum3)6)
					{
						long_ = class5.struct7_0.uint_0;
					}
				}
				if (long_ < array2.Length && long_ >= 0L)
				{
					int_0 = array2[long_] - 1;
				}
				break;
			}
			case (Enum5)147:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 == null)
				{
					throw new Exception1();
				}
				class87_0.method_2(class2.vmethod_46());
				break;
			}
			case (Enum5)148:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				Class73 class73_ = class87_0.method_4();
				Class74 class3 = smethod_1(class73_);
				if (class3 != null && class2 != null)
				{
					if (class3.lwlumgaheq(@class))
					{
						class87_0.method_2(new Class75(1));
					}
					else
					{
						class87_0.method_2(new Class75(0));
					}
				}
				else if (@class.BeouTiljCp(class73_))
				{
					class87_0.method_2(new Class75(1));
				}
				else
				{
					class87_0.method_2(new Class75(0));
				}
				break;
			}
			case (Enum5)149:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_24());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(int*)(void*)intPtr, (Enum3)5));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)150:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				class87_0.method_2(Class73.smethod_1(fieldInfo_.FieldType, fieldInfo_.GetValue(null)));
				break;
			}
			case (Enum5)151:
			{
				int metadataToken = (int)object_0;
				FieldInfo fieldInfo_ = typeof(Class62).Module.ResolveField(metadataToken);
				object value = class87_0.method_4().vmethod_4(fieldInfo_.FieldType);
				Class73 @class = class87_0.method_4();
				object obj2 = @class.vmethod_4(null);
				if (obj2 == null)
				{
					Type elementType = fieldInfo_.DeclaringType;
					if (elementType.IsByRef)
					{
						elementType = elementType.GetElementType();
					}
					if (!elementType.IsValueType)
					{
						throw new NullReferenceException();
					}
					obj2 = Activator.CreateInstance(elementType);
					if (@class is Class80)
					{
						((Class79)@class).vmethod_11(Class73.smethod_1(elementType, obj2));
					}
				}
				fieldInfo_.SetValue(obj2, value);
				break;
			}
			case (Enum5)152:
				method_12(bool_4: false);
				break;
			case (Enum5)153:
				bool_3 = (bool)class87_0.method_4().vmethod_4(typeof(bool));
				bool_1 = true;
				break;
			case (Enum5)154:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_33());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)155:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_27());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(ushort*)(void*)intPtr, (Enum3)4));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)156:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_26());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class75(*(byte*)(void*)intPtr, (Enum3)2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)157:
			{
				Class73 @class = class87_0.method_4();
				class87_0.method_4().vmethod_2(@class);
				break;
			}
			case (Enum5)158:
				break;
			case (Enum5)160:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(float), value));
				break;
			}
			case (Enum5)161:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(double), value));
				break;
			}
			case (Enum5)162:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class2 != null && class3 != null)
				{
					class87_0.method_2(class2.vmethod_67(class3));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)165:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				if (class2 != null)
				{
					class87_0.method_2(class2.vmethod_23());
					break;
				}
				throw new Exception1();
			}
			case (Enum5)166:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				Class74 class3 = smethod_1(class87_0.method_4());
				if (class3 != null && class2 != null)
				{
					class87_0.method_2(class3.vmethod_74(class2));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)167:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_77(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)19:
			case (Enum5)46:
			case (Enum5)108:
			case (Enum5)131:
			case (Enum5)163:
			case (Enum5)168:
				break;
			case (Enum5)169:
			{
				Class73 @class = class87_0.method_4();
				if (smethod_1(class87_0.method_4()).vmethod_78(@class))
				{
					int_0 = (int)object_0 - 1;
				}
				break;
			}
			case (Enum5)170:
			{
				Class74 class2 = smethod_1(class87_0.method_4());
				object value = ((Array)class87_0.method_4().vmethod_4(null)).GetValue(class2.vmethod_18().struct7_0.int_0);
				class87_0.method_2(Class73.smethod_1(typeof(int), value));
				break;
			}
			case (Enum5)171:
			{
				int metadataToken = (int)object_0;
				uint uint_ = (uint)smethod_0(typeof(Class62).Module.ResolveType(metadataToken));
				class87_0.method_2(new Class75(uint_, (Enum3)6));
				break;
			}
			case (Enum5)172:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(@class);
				if (@class != null && @class.vmethod_0() && class2 != null)
				{
					class87_0.method_2(class2.vmethod_46());
					break;
				}
				if (class2 != null && class2.method_2())
				{
					IntPtr intPtr = ((Class77)class2).method_6();
					class87_0.method_2(new Class78(*(double*)(void*)intPtr, (Enum3)10));
					break;
				}
				throw new Exception1();
			}
			case (Enum5)81:
			case (Enum5)116:
			case (Enum5)124:
			case (Enum5)143:
			case (Enum5)159:
			case (Enum5)175:
				throw new Exception1();
			case (Enum5)22:
			case (Enum5)94:
			case (Enum5)120:
			case (Enum5)121:
			case (Enum5)132:
			case (Enum5)164:
			case (Enum5)173:
			case (Enum5)174:
			{
				Class73 @class = class87_0.method_4();
				Class74 class2 = smethod_1(class87_0.method_4());
				Array obj = (Array)class87_0.method_4().vmethod_4(null);
				Type elementType = obj.GetType().GetElementType();
				obj.SetValue(@class.vmethod_4(elementType), class2.vmethod_18().struct7_0.int_0);
				break;
			}
			}
		}

		private Class73 method_8(Class73 class73_3, Enum3 enum3_0, bool bool_4 = false)
		{
			if (!bool_4 && class73_3.vmethod_0())
			{
				class73_3 = class73_3.vmethod_7();
			}
			if (class73_3.method_1())
			{
				return ((Class75)class73_3).vmethod_12(enum3_0);
			}
			if (class73_3.method_3())
			{
				return ((Class76)class73_3).vmethod_12(enum3_0);
			}
			if (!class73_3.method_4())
			{
				if (!class73_3.method_2())
				{
					return class73_3;
				}
				return ((Class77)class73_3).vmethod_12(enum3_0);
			}
			return ((Class78)class73_3).vmethod_12(enum3_0);
		}

		private Class73 method_9(int int_3)
		{
			return class73_1[int_3];
		}

		private void method_10(int int_3)
		{
			method_11(int_3, class87_0.method_4());
		}

		private static int smethod_0(Type type_0)
		{
			lock (object_1)
			{
				if (dictionary_0 == null)
				{
					dictionary_0 = new Dictionary<Type, int>();
				}
				try
				{
					int value = 0;
					if (dictionary_0.TryGetValue(type_0, out value))
					{
						return value;
					}
					DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(int), Type.EmptyTypes, restrictedSkipVisibility: true);
					ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
					iLGenerator.Emit(OpCodes.Sizeof, type_0);
					iLGenerator.Emit(OpCodes.Ret);
					value = (int)dynamicMethod.Invoke(null, null);
					dictionary_0[type_0] = value;
					return value;
				}
				catch
				{
					return 0;
				}
			}
		}

		private void method_11(int int_3, Class73 class73_3)
		{
			class73_1[int_3] = method_8(class73_3, class68_0.list_1[int_3].enum3_0, class68_0.list_1[int_3].bool_0);
		}

		private static Class74 smethod_1(Class73 class73_3)
		{
			Class74 @class = class73_3 as Class74;
			if (@class == null && class73_3.vmethod_0())
			{
				@class = class73_3.vmethod_7() as Class74;
			}
			return @class;
		}

		private void method_12(bool bool_4)
		{
			int metadataToken = (int)object_0;
			MethodBase methodBase = typeof(Class62).Module.ResolveMethod(metadataToken);
			MethodInfo methodInfo = methodBase as MethodInfo;
			ParameterInfo[] parameters = methodBase.GetParameters();
			object[] array = new object[parameters.Length];
			Class73[] array2 = new Class73[parameters.Length];
			List<Class69> list = null;
			Class70 @class = null;
			for (int i = 0; i < parameters.Length; i++)
			{
				Class73 class2 = class87_0.method_4();
				Type type = parameters[parameters.Length - 1 - i].ParameterType;
				object obj = null;
				bool flag = false;
				if (type.IsByRef && class2 is Class82 class3)
				{
					if (list == null)
					{
						list = new List<Class69>();
					}
					list.Add(new Class69(class3.fieldInfo_0, parameters.Length - 1 - i));
					obj = class3.object_0;
					if (!(obj is Class73))
					{
						flag = true;
						if (obj == null)
						{
							if (type.IsByRef)
							{
								type = type.GetElementType();
							}
							if (type.IsValueType)
							{
								obj = (class3.fieldInfo_0.IsStatic ? class3.fieldInfo_0.GetValue(null) : Activator.CreateInstance(type));
								if (class2 is Class80)
								{
									((Class79)class2).vmethod_11(Class73.smethod_1(type, obj));
								}
							}
						}
					}
					else
					{
						class2 = obj as Class73;
					}
				}
				if (!flag)
				{
					if (class2 != null)
					{
						obj = class2.vmethod_4(type);
					}
					if (obj == null)
					{
						if (type.IsByRef)
						{
							type = type.GetElementType();
						}
						if (type.IsValueType)
						{
							obj = Activator.CreateInstance(type);
							if (class2 is Class80)
							{
								((Class79)class2).vmethod_11(Class73.smethod_1(type, obj));
							}
						}
					}
				}
				array2[array.Length - 1 - i] = class2;
				array[array.Length - 1 - i] = obj;
			}
			Delegate10 @delegate = null;
			if (list == null)
			{
				if (methodInfo != null && methodInfo.ReturnType.IsByRef)
				{
					@delegate = smethod_2(methodBase, bool_4);
				}
			}
			else
			{
				@class = new Class70(methodBase, list);
				@delegate = smethod_3(methodBase, bool_4, @class);
			}
			object obj2 = null;
			Class73 class4 = null;
			if (!methodBase.IsStatic)
			{
				class4 = class87_0.method_4();
				if (class4 != null)
				{
					obj2 = class4.vmethod_4(methodBase.DeclaringType);
				}
				if (obj2 == null)
				{
					Type type2 = methodBase.DeclaringType;
					if (type2.IsByRef)
					{
						type2 = type2.GetElementType();
					}
					if (!type2.IsValueType)
					{
						throw new NullReferenceException();
					}
					obj2 = Activator.CreateInstance(type2);
					if (obj2 == null && Nullable.GetUnderlyingType(type2) != null)
					{
						obj2 = FormatterServices.GetUninitializedObject(type2);
					}
					if (class4 is Class80)
					{
						((Class79)class4).vmethod_11(Class73.smethod_1(type2, obj2));
					}
				}
			}
			object obj3 = null;
			if (!(methodBase is ConstructorInfo) || !(Nullable.GetUnderlyingType(methodBase.DeclaringType) != null))
			{
				obj3 = ((@delegate != null) ? @delegate(obj2, array) : methodBase.Invoke(obj2, array));
			}
			else
			{
				obj3 = array[0];
				if (class4 != null && class4 is Class80)
				{
					((Class79)class4).vmethod_11(Class73.smethod_1(Nullable.GetUnderlyingType(methodBase.DeclaringType), obj3));
				}
			}
			for (int j = 0; j < parameters.Length; j++)
			{
				if (parameters[j].ParameterType.IsByRef && (@class == null || !@class.method_1(j)))
				{
					if (array2[j].method_2())
					{
						((Class77)array2[j]).method_5(Class73.smethod_1(parameters[j].ParameterType, array[j]));
					}
					else if (!(array2[j] is Class80))
					{
						array2[j].vmethod_9(Class73.smethod_1(parameters[j].ParameterType, array[j]));
					}
					else
					{
						array2[j].vmethod_9(Class73.smethod_1(parameters[j].ParameterType.GetElementType(), array[j]));
					}
				}
			}
			if (methodInfo != null && methodInfo.ReturnType != typeof(void))
			{
				class87_0.method_2(Class73.smethod_1(methodInfo.ReturnType, obj3));
			}
		}

		private static Delegate10 smethod_2(MethodBase methodBase_0, bool bool_4)
		{
			lock (object_3)
			{
				Delegate10 value = null;
				if (bool_4)
				{
					if (dictionary_2.TryGetValue(methodBase_0, out value))
					{
						return value;
					}
				}
				else if (dictionary_3.TryGetValue(methodBase_0, out value))
				{
					return value;
				}
				MethodInfo methodInfo = methodBase_0 as MethodInfo;
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[2]
				{
					typeof(object),
					typeof(object[])
				}, restrictedSkipVisibility: true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				ParameterInfo[] parameters = methodBase_0.GetParameters();
				Type[] array = new Type[parameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (parameters[i].ParameterType.IsByRef)
					{
						array[i] = parameters[i].ParameterType.GetElementType();
					}
					else
					{
						array[i] = parameters[i].ParameterType;
					}
				}
				int num = array.Length;
				if (methodBase_0.DeclaringType.IsValueType)
				{
					num++;
				}
				LocalBuilder[] array2 = new LocalBuilder[num];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = iLGenerator.DeclareLocal(array[j]);
				}
				if (methodBase_0.DeclaringType.IsValueType)
				{
					array2[array2.Length - 1] = iLGenerator.DeclareLocal(methodBase_0.DeclaringType.MakeByRefType());
				}
				for (int k = 0; k < array.Length; k++)
				{
					iLGenerator.Emit(OpCodes.Ldarg_1);
					smethod_5(iLGenerator, k);
					iLGenerator.Emit(OpCodes.Ldelem_Ref);
					if (array[k].IsValueType)
					{
						iLGenerator.Emit(OpCodes.Unbox_Any, array[k]);
					}
					else if (array[k] != typeof(object))
					{
						iLGenerator.Emit(OpCodes.Castclass, array[k]);
					}
					iLGenerator.Emit(OpCodes.Stloc, array2[k]);
				}
				if (!methodBase_0.IsStatic)
				{
					iLGenerator.Emit(OpCodes.Ldarg_0);
					if (!methodBase_0.DeclaringType.IsValueType)
					{
						iLGenerator.Emit(OpCodes.Castclass, methodBase_0.DeclaringType);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Unbox, methodBase_0.DeclaringType);
						iLGenerator.Emit(OpCodes.Stloc, array2[array2.Length - 1]);
						iLGenerator.Emit(OpCodes.Ldloc_S, array2[array2.Length - 1]);
					}
				}
				for (int l = 0; l < array.Length; l++)
				{
					if (!parameters[l].ParameterType.IsByRef)
					{
						iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldloca_S, array2[l]);
					}
				}
				if (bool_4)
				{
					if (methodInfo != null)
					{
						iLGenerator.EmitCall(OpCodes.Call, methodInfo, null);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Call, methodBase_0 as ConstructorInfo);
					}
				}
				else if (methodInfo != null)
				{
					iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
				}
				else
				{
					iLGenerator.Emit(OpCodes.Callvirt, methodBase_0 as ConstructorInfo);
				}
				if (!(methodInfo == null) && !(methodInfo.ReturnType == typeof(void)))
				{
					if (!methodInfo.ReturnType.IsByRef)
					{
						if (methodInfo.ReturnType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
						}
					}
					else
					{
						Type elementType = methodInfo.ReturnType.GetElementType();
						if (elementType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Ldobj, elementType);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldind_Ref, elementType);
						}
						if (elementType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, elementType);
						}
					}
				}
				else
				{
					iLGenerator.Emit(OpCodes.Ldnull);
				}
				for (int m = 0; m < array.Length; m++)
				{
					if (parameters[m].ParameterType.IsByRef)
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						smethod_5(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
						if (array2[m].LocalType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
					}
				}
				iLGenerator.Emit(OpCodes.Ret);
				Delegate10 @delegate = (Delegate10)dynamicMethod.CreateDelegate(typeof(Delegate10));
				if (bool_4)
				{
					dictionary_2.Add(methodBase_0, @delegate);
				}
				else
				{
					dictionary_3.Add(methodBase_0, @delegate);
				}
				return @delegate;
			}
		}

		private static Delegate10 smethod_3(MethodBase methodBase_0, bool bool_4, Class70 class70_0)
		{
			lock (object_4)
			{
				Delegate10 value = null;
				if (!bool_4)
				{
					if (dictionary_5.TryGetValue(class70_0, out value))
					{
						return value;
					}
				}
				else if (dictionary_4.TryGetValue(class70_0, out value))
				{
					return value;
				}
				MethodInfo methodInfo = methodBase_0 as MethodInfo;
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[2]
				{
					typeof(object),
					typeof(object[])
				}, typeof(Class62), skipVisibility: true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				ParameterInfo[] parameters = methodBase_0.GetParameters();
				Type[] array = new Type[parameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (parameters[i].ParameterType.IsByRef)
					{
						array[i] = parameters[i].ParameterType.GetElementType();
					}
					else
					{
						array[i] = parameters[i].ParameterType;
					}
				}
				int num = array.Length;
				if (methodBase_0.DeclaringType.IsValueType)
				{
					num++;
				}
				LocalBuilder[] array2 = new LocalBuilder[num];
				for (int j = 0; j < array.Length; j++)
				{
					if (class70_0.method_1(j))
					{
						array2[j] = iLGenerator.DeclareLocal(typeof(object));
					}
					else
					{
						array2[j] = iLGenerator.DeclareLocal(array[j]);
					}
				}
				if (methodBase_0.DeclaringType.IsValueType)
				{
					array2[array2.Length - 1] = iLGenerator.DeclareLocal(methodBase_0.DeclaringType.MakeByRefType());
				}
				for (int k = 0; k < array.Length; k++)
				{
					iLGenerator.Emit(OpCodes.Ldarg_1);
					smethod_5(iLGenerator, k);
					iLGenerator.Emit(OpCodes.Ldelem_Ref);
					if (!class70_0.method_1(k))
					{
						if (array[k].IsValueType)
						{
							iLGenerator.Emit(OpCodes.Unbox_Any, array[k]);
						}
						else if (array[k] != typeof(object))
						{
							iLGenerator.Emit(OpCodes.Castclass, array[k]);
						}
					}
					iLGenerator.Emit(OpCodes.Stloc, array2[k]);
				}
				if (!methodBase_0.IsStatic)
				{
					iLGenerator.Emit(OpCodes.Ldarg_0);
					if (methodBase_0.DeclaringType.IsValueType)
					{
						iLGenerator.Emit(OpCodes.Unbox, methodBase_0.DeclaringType);
						iLGenerator.Emit(OpCodes.Stloc, array2[array2.Length - 1]);
						iLGenerator.Emit(OpCodes.Ldloc_S, array2[array2.Length - 1]);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Castclass, methodBase_0.DeclaringType);
					}
				}
				for (int l = 0; l < array.Length; l++)
				{
					if (!class70_0.method_1(l))
					{
						if (!parameters[l].ParameterType.IsByRef)
						{
							iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldloca_S, array2[l]);
						}
						continue;
					}
					Class69 @class = class70_0.method_0(l);
					if (!((FieldInfo)@class.object_0).IsStatic)
					{
						if (!((MemberInfo)@class.object_0).DeclaringType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
							iLGenerator.Emit(OpCodes.Castclass, ((MemberInfo)@class.object_0).DeclaringType);
							iLGenerator.Emit(OpCodes.Ldflda, (FieldInfo)@class.object_0);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
							iLGenerator.Emit(OpCodes.Unbox, ((MemberInfo)@class.object_0).DeclaringType);
							iLGenerator.Emit(OpCodes.Ldflda, (FieldInfo)@class.object_0);
						}
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldsflda, (FieldInfo)@class.object_0);
					}
				}
				if (!bool_4)
				{
					if (methodInfo != null)
					{
						iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Callvirt, methodBase_0 as ConstructorInfo);
					}
				}
				else if (methodInfo != null)
				{
					iLGenerator.EmitCall(OpCodes.Call, methodInfo, null);
				}
				else
				{
					iLGenerator.Emit(OpCodes.Call, methodBase_0 as ConstructorInfo);
				}
				if (!(methodInfo == null) && !(methodInfo.ReturnType == typeof(void)))
				{
					if (methodInfo.ReturnType.IsByRef)
					{
						Type elementType = methodInfo.ReturnType.GetElementType();
						if (elementType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Ldobj, elementType);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldind_Ref, elementType);
						}
						if (elementType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, elementType);
						}
					}
					else if (methodInfo.ReturnType.IsValueType)
					{
						iLGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
					}
				}
				else
				{
					iLGenerator.Emit(OpCodes.Ldnull);
				}
				for (int m = 0; m < array.Length; m++)
				{
					if (!parameters[m].ParameterType.IsByRef)
					{
						continue;
					}
					if (!class70_0.method_1(m))
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						smethod_5(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
						if (array2[m].LocalType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
						continue;
					}
					Class69 class2 = class70_0.method_0(m);
					if (((FieldInfo)class2.object_0).IsStatic)
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						smethod_5(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldsfld, (FieldInfo)class2.object_0);
						if (((FieldInfo)class2.object_0).FieldType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, ((FieldInfo)class2.object_0).FieldType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						smethod_5(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
						if (array2[m].LocalType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, ((FieldInfo)class2.object_0).FieldType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
					}
				}
				iLGenerator.Emit(OpCodes.Ret);
				Delegate10 @delegate = (Delegate10)dynamicMethod.CreateDelegate(typeof(Delegate10));
				if (bool_4)
				{
					dictionary_4.Add(class70_0, @delegate);
				}
				else
				{
					dictionary_5.Add(class70_0, @delegate);
				}
				return @delegate;
			}
		}

		private static Delegate10 smethod_4(MethodBase methodBase_0, bool bool_4, Class70 class70_0)
		{
			lock (object_5)
			{
				Delegate10 value = null;
				if (dictionary_6.TryGetValue(class70_0, out value))
				{
					return value;
				}
				ConstructorInfo constructorInfo = methodBase_0 as ConstructorInfo;
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[2]
				{
					typeof(object),
					typeof(object[])
				}, typeof(Class62), skipVisibility: true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				ParameterInfo[] parameters = methodBase_0.GetParameters();
				Type[] array = new Type[parameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (parameters[i].ParameterType.IsByRef)
					{
						array[i] = parameters[i].ParameterType.GetElementType();
					}
					else
					{
						array[i] = parameters[i].ParameterType;
					}
				}
				int num = array.Length;
				if (methodBase_0.DeclaringType.IsValueType)
				{
					num++;
				}
				LocalBuilder[] array2 = new LocalBuilder[num];
				for (int j = 0; j < array.Length; j++)
				{
					if (class70_0.method_1(j))
					{
						array2[j] = iLGenerator.DeclareLocal(typeof(object));
					}
					else
					{
						array2[j] = iLGenerator.DeclareLocal(array[j]);
					}
				}
				if (methodBase_0.DeclaringType.IsValueType)
				{
					array2[array2.Length - 1] = iLGenerator.DeclareLocal(methodBase_0.DeclaringType.MakeByRefType());
				}
				for (int k = 0; k < array.Length; k++)
				{
					iLGenerator.Emit(OpCodes.Ldarg_1);
					smethod_5(iLGenerator, k);
					iLGenerator.Emit(OpCodes.Ldelem_Ref);
					if (!class70_0.method_1(k))
					{
						if (!array[k].IsValueType)
						{
							if (array[k] != typeof(object))
							{
								iLGenerator.Emit(OpCodes.Castclass, array[k]);
							}
						}
						else
						{
							iLGenerator.Emit(OpCodes.Unbox_Any, array[k]);
						}
					}
					iLGenerator.Emit(OpCodes.Stloc, array2[k]);
				}
				for (int l = 0; l < array.Length; l++)
				{
					if (class70_0.method_1(l))
					{
						Class69 @class = class70_0.method_0(l);
						if (((FieldInfo)@class.object_0).IsStatic)
						{
							iLGenerator.Emit(OpCodes.Ldsflda, (FieldInfo)@class.object_0);
						}
						else if (((MemberInfo)@class.object_0).DeclaringType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
							iLGenerator.Emit(OpCodes.Unbox, ((MemberInfo)@class.object_0).DeclaringType);
							iLGenerator.Emit(OpCodes.Ldflda, (FieldInfo)@class.object_0);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
							iLGenerator.Emit(OpCodes.Castclass, ((MemberInfo)@class.object_0).DeclaringType);
							iLGenerator.Emit(OpCodes.Ldflda, (FieldInfo)@class.object_0);
						}
					}
					else if (parameters[l].ParameterType.IsByRef)
					{
						iLGenerator.Emit(OpCodes.Ldloca_S, array2[l]);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
					}
				}
				iLGenerator.Emit(OpCodes.Newobj, methodBase_0 as ConstructorInfo);
				if (constructorInfo.DeclaringType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Box, constructorInfo.DeclaringType);
				}
				for (int m = 0; m < array.Length; m++)
				{
					if (!parameters[m].ParameterType.IsByRef)
					{
						continue;
					}
					if (class70_0.method_1(m))
					{
						Class69 class2 = class70_0.method_0(m);
						if (((FieldInfo)class2.object_0).IsStatic)
						{
							iLGenerator.Emit(OpCodes.Ldarg_1);
							smethod_5(iLGenerator, m);
							iLGenerator.Emit(OpCodes.Ldsfld, (FieldInfo)class2.object_0);
							if (((FieldInfo)class2.object_0).FieldType.IsValueType)
							{
								iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
							}
							iLGenerator.Emit(OpCodes.Stelem_Ref);
						}
						else
						{
							iLGenerator.Emit(OpCodes.Ldarg_1);
							smethod_5(iLGenerator, m);
							iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
							if (array2[m].LocalType.IsValueType)
							{
								iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
							}
							iLGenerator.Emit(OpCodes.Stelem_Ref);
						}
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						smethod_5(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
						if (array2[m].LocalType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
					}
				}
				iLGenerator.Emit(OpCodes.Ret);
				Delegate10 @delegate = (Delegate10)dynamicMethod.CreateDelegate(typeof(Delegate10));
				dictionary_6.Add(class70_0, @delegate);
				return @delegate;
			}
		}

		private static void smethod_5(ILGenerator ilgenerator_0, int int_3)
		{
			switch (int_3)
			{
			case -1:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_M1);
				return;
			case 0:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_0);
				return;
			case 1:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_1);
				return;
			case 2:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_2);
				return;
			case 3:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_3);
				return;
			case 4:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_4);
				return;
			case 5:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_5);
				return;
			case 6:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_6);
				return;
			case 7:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_7);
				return;
			case 8:
				ilgenerator_0.Emit(OpCodes.Ldc_I4_8);
				return;
			}
			if (int_3 > -129 && int_3 < 128)
			{
				ilgenerator_0.Emit(OpCodes.Ldc_I4_S, (sbyte)int_3);
			}
			else
			{
				ilgenerator_0.Emit(OpCodes.Ldc_I4, int_3);
			}
		}

		private static Class73 smethod_6(Class73 class73_3)
		{
			if (class73_3.vmethod_7().method_0())
			{
				object obj = class73_3.vmethod_4(null);
				if (obj != null && obj.GetType().IsEnum)
				{
					Type underlyingType = Enum.GetUnderlyingType(obj.GetType());
					object obj2 = Convert.ChangeType(obj, underlyingType);
					Class73 @class = smethod_7(Class73.smethod_1(underlyingType, obj2));
					if (@class != null)
					{
						return @class as Class74;
					}
				}
			}
			return class73_3;
		}

		private static Class74 smethod_7(Class73 class73_3)
		{
			Class74 @class = class73_3 as Class74;
			if (@class == null && class73_3.vmethod_0())
			{
				@class = class73_3.vmethod_7() as Class74;
			}
			return @class;
		}

		private static IntPtr bltXeOrHnB(Class73 class73_3)
		{
			if (class73_3 != null)
			{
				if (!class73_3.method_2())
				{
					if (class73_3.vmethod_0())
					{
						Class79 @class = (Class79)class73_3;
						try
						{
							return @class.vmethod_10();
						}
						catch
						{
						}
					}
					object obj2 = class73_3.vmethod_4(typeof(IntPtr));
					if (obj2 != null && obj2.GetType() == typeof(IntPtr))
					{
						return (IntPtr)obj2;
					}
					throw new Exception1();
				}
				return ((Class77)class73_3).method_6();
			}
			return IntPtr.Zero;
		}

		private static object smethod_8(object object_7)
		{
			lock (object_6)
			{
				if (dictionary_7 == null)
				{
					dictionary_7 = new Dictionary<Type, Delegate11>();
				}
				if (object_7 != null)
				{
					try
					{
						Type type = object_7.GetType();
						if (dictionary_7.TryGetValue(type, out var value))
						{
							return value(object_7);
						}
						DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[1] { typeof(object) }, restrictedSkipVisibility: true);
						ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
						iLGenerator.Emit(OpCodes.Ldarg_0);
						iLGenerator.Emit(OpCodes.Unbox_Any, type);
						iLGenerator.Emit(OpCodes.Box, type);
						iLGenerator.Emit(OpCodes.Ret);
						Delegate11 @delegate = (Delegate11)dynamicMethod.CreateDelegate(typeof(Delegate11));
						dictionary_7.Add(type, @delegate);
						return @delegate(object_7);
					}
					catch
					{
						return null;
					}
				}
				return null;
			}
		}

		private static void smethod_9(IntPtr intptr_0, byte byte_0, int int_3)
		{
			lock (object_6)
			{
				if (delegate12_0 == null)
				{
					DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new Type[3]
					{
						typeof(IntPtr),
						typeof(byte),
						typeof(int)
					}, typeof(Class62), skipVisibility: true);
					ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
					iLGenerator.Emit(OpCodes.Ldarg_0);
					iLGenerator.Emit(OpCodes.Ldarg_1);
					iLGenerator.Emit(OpCodes.Ldarg_2);
					iLGenerator.Emit(OpCodes.Initblk);
					iLGenerator.Emit(OpCodes.Ret);
					delegate12_0 = (Delegate12)dynamicMethod.CreateDelegate(typeof(Delegate12));
				}
				delegate12_0(intptr_0, byte_0, int_3);
			}
		}

		private static void smethod_10(IntPtr intptr_0, IntPtr intptr_1, uint uint_0)
		{
			if (delegate13_0 == null)
			{
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new Type[3]
				{
					typeof(IntPtr),
					typeof(IntPtr),
					typeof(uint)
				}, typeof(Class62), skipVisibility: true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldarg_1);
				iLGenerator.Emit(OpCodes.Ldarg_2);
				iLGenerator.Emit(OpCodes.Cpblk);
				iLGenerator.Emit(OpCodes.Ret);
				delegate13_0 = (Delegate13)dynamicMethod.CreateDelegate(typeof(Delegate13));
			}
			delegate13_0(intptr_0, intptr_1, uint_0);
		}

		static Class71()
		{
			object_1 = new object();
			dictionary_1 = new Dictionary<object, Class73>();
			object_2 = new object();
			dictionary_2 = new Dictionary<MethodBase, Delegate10>();
			dictionary_3 = new Dictionary<MethodBase, Delegate10>();
			object_3 = new object();
			dictionary_4 = new Dictionary<Class70, Delegate10>();
			dictionary_5 = new Dictionary<Class70, Delegate10>();
			object_4 = new object();
			dictionary_6 = new Dictionary<Class70, Delegate10>();
			object_5 = new object();
			object_6 = new object();
		}
	}

	internal enum Enum5 : byte
	{

	}

	internal enum Enum6 : byte
	{

	}

	internal abstract class Class73
	{
		internal Enum6 enum6_0;

		public Class73()
		{
		}

		internal bool method_0()
		{
			return enum6_0 == (Enum6)0;
		}

		internal bool method_1()
		{
			return enum6_0 == (Enum6)1;
		}

		internal bool method_2()
		{
			if (enum6_0 != (Enum6)3)
			{
				return enum6_0 == (Enum6)4;
			}
			return true;
		}

		internal bool method_3()
		{
			return enum6_0 == (Enum6)2;
		}

		internal bool method_4()
		{
			return enum6_0 == (Enum6)5;
		}

		internal bool nGhvlaVjva()
		{
			return enum6_0 == (Enum6)6;
		}

		internal virtual bool vmethod_0()
		{
			return false;
		}

		internal virtual bool vmethod_1()
		{
			return false;
		}

		internal abstract void vmethod_2(Class73 class73_0);

		internal virtual bool vmethod_3()
		{
			return false;
		}

		internal Class73(Enum6 enum6_1)
		{
			enum6_0 = enum6_1;
		}

		internal abstract object vmethod_4(Type type_0);

		internal abstract bool vmethod_5(Class73 class73_0);

		internal abstract bool BeouTiljCp(Class73 class73_0);

		internal abstract bool vmethod_6();

		internal abstract Class73 vmethod_7();

		internal virtual bool vmethod_8()
		{
			return false;
		}

		internal abstract void vmethod_9(Class73 class73_0);

		internal static Enum4 smethod_0(Type type_0)
		{
			Type type = type_0;
			if (type != null)
			{
				if (type.IsByRef)
				{
					type = type.GetElementType();
				}
				if (type != null && Nullable.GetUnderlyingType(type) != null)
				{
					type = Nullable.GetUnderlyingType(type);
				}
				if (!(type == typeof(string)))
				{
					if (type == typeof(byte))
					{
						return (Enum4)2;
					}
					if (type == typeof(sbyte))
					{
						return (Enum4)1;
					}
					if (type == typeof(short))
					{
						return (Enum4)3;
					}
					if (!(type == typeof(ushort)))
					{
						if (!(type == typeof(int)))
						{
							if (type == typeof(uint))
							{
								return (Enum4)6;
							}
							if (!(type == typeof(long)))
							{
								if (type == typeof(ulong))
								{
									return (Enum4)8;
								}
								if (!(type == typeof(float)))
								{
									if (!(type == typeof(double)))
									{
										if (!(type == typeof(bool)))
										{
											if (!(type == typeof(IntPtr)))
											{
												if (type == typeof(UIntPtr))
												{
													return (Enum4)13;
												}
												if (type == typeof(char))
												{
													return (Enum4)15;
												}
												if (!(type == typeof(object)))
												{
													if (type.IsEnum)
													{
														return (Enum4)16;
													}
													return (Enum4)17;
												}
												return (Enum4)0;
											}
											return (Enum4)12;
										}
										return (Enum4)11;
									}
									return (Enum4)10;
								}
								return (Enum4)9;
							}
							return (Enum4)7;
						}
						return (Enum4)5;
					}
					return (Enum4)4;
				}
				return (Enum4)14;
			}
			return (Enum4)18;
		}

		internal static Class73 smethod_1(Type type_0, object object_0)
		{
			Enum4 @enum = smethod_0(type_0);
			Enum4 enum2 = (Enum4)18;
			if (object_0 != null)
			{
				enum2 = smethod_0(object_0.GetType());
			}
			Class73 @class = null;
			switch (@enum)
			{
			case (Enum4)0:
				@class = ((enum2 != (Enum4)15) ? smethod_2(object_0) : new Class85(object_0));
				goto default;
			case (Enum4)1:
				@class = enum2 switch
				{
					(Enum4)2 => new Class75((sbyte)(byte)object_0, (Enum3)1), 
					(Enum4)1 => new Class75((sbyte)object_0, (Enum3)1), 
					(Enum4)15 => new Class75((sbyte)(char)object_0, (Enum3)1), 
					(Enum4)11 => ((bool)object_0) ? new Class75(1, (Enum3)1) : new Class75(0, (Enum3)1), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)2:
				@class = enum2 switch
				{
					(Enum4)2 => new Class75((byte)object_0, (Enum3)2), 
					(Enum4)1 => new Class75((byte)(sbyte)object_0, (Enum3)2), 
					(Enum4)15 => new Class75((byte)(char)object_0, (Enum3)2), 
					(Enum4)11 => (!(bool)object_0) ? new Class75(0, (Enum3)2) : new Class75(1, (Enum3)2), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)3:
				@class = enum2 switch
				{
					(Enum4)15 => new Class75((short)(char)object_0, (Enum3)3), 
					(Enum4)11 => ((bool)object_0) ? new Class75(1, (Enum3)3) : new Class75(0, (Enum3)3), 
					(Enum4)3 => new Class75((short)object_0, (Enum3)3), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)4:
				@class = enum2 switch
				{
					(Enum4)15 => new Class75((char)object_0, (Enum3)4), 
					(Enum4)11 => ((bool)object_0) ? new Class75(1, (Enum3)4) : new Class75(0, (Enum3)4), 
					(Enum4)4 => new Class75((ushort)object_0, (Enum3)4), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)5:
				@class = enum2 switch
				{
					(Enum4)15 => new Class75((char)object_0, (Enum3)5), 
					(Enum4)11 => (!(bool)object_0) ? new Class75(0, (Enum3)5) : new Class75(1, (Enum3)5), 
					(Enum4)5 => new Class75((int)object_0, (Enum3)5), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)6:
				@class = enum2 switch
				{
					(Enum4)15 => new Class75((uint)(char)object_0, (Enum3)6), 
					(Enum4)11 => (!(bool)object_0) ? new Class75(0u, (Enum3)6) : new Class75(1u, (Enum3)6), 
					(Enum4)6 => new Class75((uint)object_0, (Enum3)6), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)7:
				@class = enum2 switch
				{
					(Enum4)15 => new Class76((char)object_0, (Enum3)7), 
					(Enum4)11 => ((bool)object_0) ? new Class76(1L, (Enum3)7) : new Class76(0L, (Enum3)7), 
					(Enum4)7 => new Class76((long)object_0, (Enum3)7), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)8:
				@class = enum2 switch
				{
					(Enum4)15 => new Class76((ulong)(char)object_0, (Enum3)8), 
					(Enum4)11 => ((bool)object_0) ? new Class76(1uL, (Enum3)8) : new Class76(0uL, (Enum3)8), 
					(Enum4)8 => new Class76((ulong)object_0, (Enum3)8), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)9:
				if (enum2 == (Enum4)9)
				{
					@class = new Class78((float)object_0);
					goto default;
				}
				throw new InvalidCastException();
			case (Enum4)10:
				if (enum2 == (Enum4)10)
				{
					@class = new Class78((double)object_0);
					goto default;
				}
				throw new InvalidCastException();
			case (Enum4)11:
				switch (enum2)
				{
				case (Enum4)1:
					@class = new Class75((sbyte)object_0 != 0);
					break;
				case (Enum4)2:
					@class = new Class75((byte)object_0 != 0);
					break;
				case (Enum4)3:
					@class = new Class75((short)object_0 != 0);
					break;
				case (Enum4)4:
					@class = new Class75((ushort)object_0 != 0);
					break;
				case (Enum4)5:
					@class = new Class75((int)object_0 != 0);
					break;
				case (Enum4)6:
					@class = new Class75((uint)object_0 != 0);
					break;
				case (Enum4)7:
					@class = new Class75((ulong)(long)object_0 > 0uL);
					break;
				case (Enum4)8:
					@class = new Class75((ulong)object_0 > 0L);
					break;
				case (Enum4)11:
					@class = new Class75((bool)object_0);
					break;
				case (Enum4)9:
				case (Enum4)10:
				case (Enum4)12:
				case (Enum4)13:
				case (Enum4)14:
				case (Enum4)15:
				case (Enum4)16:
					throw new InvalidCastException();
				default:
					@class = new Class75(object_0 != null);
					break;
				case (Enum4)18:
					@class = new Class75(bool_0: false);
					break;
				}
				goto default;
			case (Enum4)12:
				if (enum2 == (Enum4)12)
				{
					@class = new Class77((IntPtr)object_0);
					goto default;
				}
				throw new InvalidCastException();
			case (Enum4)13:
				if (enum2 == (Enum4)13)
				{
					@class = new Class77((UIntPtr)object_0);
					goto default;
				}
				throw new InvalidCastException();
			case (Enum4)14:
				@class = new Class86(object_0 as string);
				goto default;
			case (Enum4)15:
				@class = enum2 switch
				{
					(Enum4)15 => new Class75((char)object_0, (Enum3)15), 
					(Enum4)1 => new Class75((sbyte)object_0, (Enum3)15), 
					(Enum4)2 => new Class75((byte)object_0, (Enum3)15), 
					(Enum4)3 => new Class75((short)object_0, (Enum3)15), 
					(Enum4)4 => new Class75((ushort)object_0, (Enum3)15), 
					(Enum4)5 => new Class75((int)object_0, (Enum3)15), 
					(Enum4)6 => new Class75((int)(uint)object_0, (Enum3)15), 
					_ => throw new InvalidCastException(), 
				};
				goto default;
			case (Enum4)16:
			case (Enum4)17:
				@class = smethod_2(object_0);
				goto default;
			default:
				if (type_0.IsByRef)
				{
					@class = new Class84(@class, type_0.GetElementType());
				}
				return @class;
			case (Enum4)18:
				throw new InvalidCastException();
			}
		}

		private static Class73 smethod_2(object object_0)
		{
			if (object_0 != null && object_0.GetType().IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(object_0.GetType());
				object object_ = Convert.ChangeType(object_0, underlyingType);
				Class73 @class = smethod_3(smethod_1(underlyingType, object_));
				if (@class != null)
				{
					return @class as Class74;
				}
			}
			return new Class85(object_0);
		}

		private static Class74 smethod_3(Class73 class73_0)
		{
			Class74 @class = class73_0 as Class74;
			if (@class == null && class73_0.vmethod_0())
			{
				@class = class73_0.vmethod_7() as Class74;
			}
			return @class;
		}
	}

	private class Class85 : Class73
	{
		public Class73 class73_0;

		public Type type_0;

		public Class85()
			: this(null)
		{
		}

		internal override void vmethod_9(Class73 class73_1)
		{
			if (!(class73_1 is Class85))
			{
				class73_0 = class73_1.vmethod_7();
				return;
			}
			class73_0 = ((Class85)class73_1).class73_0;
			type_0 = ((Class85)class73_1).type_0;
		}

		internal override void vmethod_2(Class73 class73_1)
		{
			vmethod_9(class73_1);
		}

		public Class85(object object_0)
			: base((Enum6)0)
		{
			class73_0 = (Class73)object_0;
			type_0 = null;
		}

		public Class85(object object_0, Type type_1)
			: base((Enum6)0)
		{
			class73_0 = (Class73)object_0;
			type_0 = type_1;
		}

		public override string ToString()
		{
			if (class73_0 == null)
			{
				return ((Enum7)5).ToString();
			}
			return class73_0.ToString();
		}

		internal override object vmethod_4(Type type_1)
		{
			if (class73_0 != null)
			{
				if (type_1 != null && type_1.IsByRef)
				{
					type_1 = type_1.GetElementType();
				}
				if (class73_0 is Class73)
				{
					if (!(type_0 != null))
					{
						object obj = class73_0.vmethod_4(type_1);
						if (obj != null && type_1 != null && obj.GetType() != type_1)
						{
							if (type_1 == typeof(RuntimeFieldHandle) && obj is FieldInfo)
							{
								obj = ((FieldInfo)obj).FieldHandle;
							}
							else if (type_1 == typeof(RuntimeTypeHandle) && obj is Type)
							{
								obj = ((Type)obj).TypeHandle;
							}
							else if (type_1 == typeof(RuntimeMethodHandle) && obj is MethodBase)
							{
								obj = ((MethodBase)obj).MethodHandle;
							}
						}
						return obj;
					}
					return class73_0.vmethod_4(type_0);
				}
				object obj2 = class73_0;
				if (obj2 != null && type_1 != null && obj2.GetType() != type_1)
				{
					if (type_1 == typeof(RuntimeFieldHandle) && obj2 is FieldInfo)
					{
						obj2 = ((FieldInfo)obj2).FieldHandle;
					}
					else if (type_1 == typeof(RuntimeTypeHandle) && obj2 is Type)
					{
						obj2 = ((Type)obj2).TypeHandle;
					}
					else if (type_1 == typeof(RuntimeMethodHandle) && obj2 is MethodBase)
					{
						obj2 = ((MethodBase)obj2).MethodHandle;
					}
				}
				return obj2;
			}
			return null;
		}

		internal override bool vmethod_5(Class73 class73_1)
		{
			if (class73_1.vmethod_0())
			{
				return ((Class79)class73_1).vmethod_5(this);
			}
			object obj = vmethod_4(null);
			object obj2 = class73_1.vmethod_4(null);
			return obj == obj2;
		}

		internal override bool BeouTiljCp(Class73 class73_1)
		{
			if (!class73_1.vmethod_0())
			{
				object obj = vmethod_4(null);
				object obj2 = class73_1.vmethod_4(null);
				return obj != obj2;
			}
			return ((Class79)class73_1).BeouTiljCp(this);
		}

		internal override Class73 vmethod_7()
		{
			if (!(class73_0 is Class73 @class))
			{
				return this;
			}
			return @class.vmethod_7();
		}

		internal override bool vmethod_6()
		{
			if (class73_0 == null)
			{
				return false;
			}
			if (class73_0 is Class73 @class)
			{
				if (@class.vmethod_4(null) == null)
				{
					return false;
				}
				return true;
			}
			return true;
		}
	}

	private class Class86 : Class73
	{
		public string string_0;

		public Class86(string string_1)
			: base((Enum6)6)
		{
			string_0 = string_1;
		}

		internal override void vmethod_9(Class73 class73_0)
		{
			string_0 = ((Class86)class73_0).string_0;
		}

		internal override void vmethod_2(Class73 class73_0)
		{
			vmethod_9(class73_0);
		}

		public override string ToString()
		{
			if (string_0 != null)
			{
				return '*' + string_0 + '*';
			}
			return ((Enum7)5).ToString();
		}

		internal override bool vmethod_6()
		{
			return string_0 != null;
		}

		internal override object vmethod_4(Type type_0)
		{
			return string_0;
		}

		internal override bool vmethod_5(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				return ((Class79)class73_0).vmethod_5(this);
			}
			string text = string_0;
			object obj = class73_0.vmethod_4(null);
			return text == obj;
		}

		internal override bool BeouTiljCp(Class73 class73_0)
		{
			if (class73_0.vmethod_0())
			{
				return ((Class79)class73_0).BeouTiljCp(this);
			}
			string text = string_0;
			object obj = class73_0.vmethod_4(null);
			return text != obj;
		}

		internal override Class73 vmethod_7()
		{
			return this;
		}
	}

	internal class Class87
	{
		private List<Class73> list_0 = new List<Class73>();

		[SpecialName]
		public int method_0()
		{
			return list_0.Count;
		}

		public void method_1()
		{
			list_0.Clear();
		}

		public void method_2(Class73 class73_0)
		{
			list_0.Add(class73_0);
		}

		public Class73 method_3()
		{
			return list_0[list_0.Count - 1];
		}

		public Class73 method_4()
		{
			Class73 result = method_3();
			if (list_0.Count != 0)
			{
				list_0.RemoveAt(list_0.Count - 1);
			}
			return result;
		}
	}

	private struct Struct9
	{
		private StringBuilder stringBuilder_0;

		public Struct9(int int_0, int int_1)
		{
			stringBuilder_0 = new StringBuilder();
		}

		public Struct9(int int_0, int int_1, IFormatProvider iformatProvider_0)
		{
			stringBuilder_0 = new StringBuilder();
		}

		public void AppendLiteral(string string_0)
		{
			if (string_0 != null)
			{
				stringBuilder_0.Append(string_0);
			}
		}

		public void AppendFormatted<T>(T value)
		{
			if (value != null)
			{
				stringBuilder_0.Append(value);
			}
		}

		public void AppendFormatted<T>(T value, string string_0)
		{
			if (string_0 != null)
			{
				stringBuilder_0.AppendFormat(string_0, value);
			}
			else
			{
				stringBuilder_0.Append(value);
			}
		}

		public void AppendFormatted<T>(T value, int int_0)
		{
			if (value != null)
			{
				stringBuilder_0.Append(value);
			}
		}

		public void AppendFormatted<T>(T value, int int_0, string string_0)
		{
			if (string_0 != null)
			{
				stringBuilder_0.AppendFormat(string_0, value);
			}
			else
			{
				stringBuilder_0.Append(value);
			}
		}

		public void AppendFormatted(string string_0)
		{
			if (string_0 != null)
			{
				stringBuilder_0.Append(string_0);
			}
		}

		public void AppendFormatted(string string_0, int int_0 = 0, string string_1 = null)
		{
			if (string_1 == null)
			{
				stringBuilder_0.Append(string_0);
			}
			else
			{
				stringBuilder_0.AppendFormat(string_1, string_0);
			}
		}

		public void AppendFormatted(object object_0, int int_0 = 0, string string_0 = null)
		{
			if (string_0 == null)
			{
				stringBuilder_0.Append(object_0);
			}
			else
			{
				stringBuilder_0.AppendFormat(string_0, object_0);
			}
		}

		public string ToStringAndClear()
		{
			string result = stringBuilder_0.ToString();
			stringBuilder_0.Clear();
			return result;
		}
	}

	internal enum Enum7
	{

	}

	[Serializable]
	[CompilerGenerated]
	private sealed class Class88<T>
	{
		public static readonly Class88<T> _003C_003E9;

		public static Comparison<Class66> _003C_003E9__46_0;

		internal static object object_0;

		static Class88()
		{
			_003C_003E9 = new Class88<T>();
		}

		internal int method_0(Class66 x, Class66 y)
		{
			return x.class67_0.int_0.CompareTo(y.class67_0.int_0);
		}

		internal static bool smethod_0()
		{
			return object_0 == null;
		}

		internal static object smethod_1()
		{
			return object_0;
		}
	}

	internal static Class68[] class68_0;

	internal static int[] int_0;

	internal static List<string> list_0;

	private static BinaryReader binaryReader_0;

	private static byte[] byte_0;

	private static bool bool_0;

	private static object object_0;

	internal static object[] smethod_0()
	{
		return new object[1];
	}

	internal static object[] smethod_1<T>(int int_1, object[] object_1, object object_2, ref T gparam_0)
	{
		Class68 @class = null;
		lock (object_0)
		{
			if (!bool_0)
			{
				bool_0 = true;
				smethod_4();
			}
			if (class68_0[int_1] != null)
			{
				@class = class68_0[int_1];
			}
			else
			{
				binaryReader_0.BaseStream.Position = int_0[int_1];
				@class = new Class68();
				Module module = typeof(Class62).Module;
				int metadataToken = smethod_6(binaryReader_0);
				int num = smethod_6(binaryReader_0);
				int num2 = smethod_6(binaryReader_0);
				int num3 = smethod_6(binaryReader_0);
				@class.object_0 = module.ResolveMethod(metadataToken);
				ParameterInfo[] parameters = ((MethodBase)@class.object_0).GetParameters();
				@class.class64_0 = new Class64[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					Type type = parameters[i].ParameterType;
					Class64 class2 = new Class64();
					class2.bool_0 = type.IsByRef;
					class2.int_0 = i;
					@class.class64_0[i] = class2;
					if (type.IsByRef)
					{
						type = type.GetElementType();
					}
					Enum3 @enum = (Enum3)0;
					@enum = ((!(type == typeof(string))) ? ((!(type == typeof(byte))) ? ((type == typeof(sbyte)) ? ((Enum3)1) : ((!(type == typeof(short))) ? ((!(type == typeof(ushort))) ? ((!(type == typeof(int))) ? ((!(type == typeof(uint))) ? ((!(type == typeof(long))) ? ((!(type == typeof(ulong))) ? ((!(type == typeof(float))) ? ((!(type == typeof(double))) ? ((!(type == typeof(bool))) ? ((!(type == typeof(IntPtr))) ? ((!(type == typeof(UIntPtr))) ? ((type == typeof(char)) ? ((Enum3)15) : ((Enum3)0)) : ((Enum3)13)) : ((Enum3)12)) : ((Enum3)11)) : ((Enum3)10)) : ((Enum3)9)) : ((Enum3)8)) : ((Enum3)7)) : ((Enum3)6)) : ((Enum3)5)) : ((Enum3)4)) : ((Enum3)3))) : ((Enum3)2)) : ((Enum3)14));
					class2.enum3_0 = @enum;
				}
				@class.list_1 = new List<Class65>(num);
				for (int j = 0; j < num; j++)
				{
					int num4 = smethod_6(binaryReader_0);
					Class65 class3 = new Class65();
					class3.type_0 = null;
					if (num4 >= 0 && num4 < 50)
					{
						class3.enum3_0 = (Enum3)((uint)num4 & 0x1Fu);
						class3.bool_0 = (num4 & 0x20) > 0;
					}
					class3.int_0 = j;
					@class.list_1.Add(class3);
				}
				@class.list_2 = new List<Class66>(num2);
				for (int k = 0; k < num2; k++)
				{
					int num5 = smethod_6(binaryReader_0);
					int int_2 = smethod_6(binaryReader_0);
					Class66 class4 = new Class66();
					class4.int_0 = num5;
					class4.int_1 = int_2;
					Class67 class5 = (class4.class67_0 = new Class67());
					num5 = smethod_6(binaryReader_0);
					int_2 = smethod_6(binaryReader_0);
					int num6 = smethod_6(binaryReader_0);
					class5.int_0 = num5;
					class5.int_1 = int_2;
					class5.int_3 = num6;
					switch (num6)
					{
					case 0:
						class5.type_0 = module.ResolveType(smethod_6(binaryReader_0));
						break;
					case 1:
						class5.int_2 = smethod_6(binaryReader_0);
						break;
					default:
						smethod_6(binaryReader_0);
						break;
					}
					@class.list_2.Add(class4);
				}
				@class.list_2.Sort((Class66 x, Class66 y) => x.class67_0.int_0.CompareTo(y.class67_0.int_0));
				@class.list_0 = new List<Class63>(num3);
				for (int l = 0; l < num3; l++)
				{
					Class63 class6 = new Class63();
					byte b = (byte)(class6.enum5_0 = (Enum5)binaryReader_0.ReadByte());
					if (b < 176)
					{
						int num7 = byte_0[b];
						if (num7 == 0)
						{
							class6.object_0 = null;
						}
						else
						{
							object obj = null;
							switch (num7)
							{
							case 1:
								obj = smethod_6(binaryReader_0);
								goto IL_0557;
							case 2:
								obj = binaryReader_0.ReadInt64();
								goto IL_0557;
							case 3:
								obj = binaryReader_0.ReadSingle();
								goto IL_0557;
							case 4:
								obj = binaryReader_0.ReadDouble();
								goto IL_0557;
							case 5:
							{
								int num8 = smethod_6(binaryReader_0);
								int[] array = new int[num8];
								for (int m = 0; m < num8; m++)
								{
									array[m] = smethod_6(binaryReader_0);
								}
								obj = array;
								goto IL_0557;
							}
							default:
								{
									throw new Exception();
								}
								IL_0557:
								class6.object_0 = obj;
								break;
							}
						}
						@class.list_0.Add(class6);
						continue;
					}
					throw new Exception();
				}
				class68_0[int_1] = @class;
			}
		}
		Class71 class7 = new Class71();
		class7.class68_0 = @class;
		ParameterInfo[] parameters2 = ((MethodBase)@class.object_0).GetParameters();
		bool flag = false;
		int num9 = 0;
		if (@class.object_0 is MethodInfo && ((MethodInfo)@class.object_0).ReturnType != typeof(void))
		{
			flag = true;
		}
		if (((MethodBase)@class.object_0).IsStatic)
		{
			class7.class73_0 = new Class73[parameters2.Length];
			for (int n = 0; n < parameters2.Length; n++)
			{
				Type parameterType = parameters2[n].ParameterType;
				class7.class73_0[n] = Class73.smethod_1(parameterType, object_1[n]);
				if (parameterType.IsByRef)
				{
					num9++;
				}
			}
		}
		else
		{
			class7.class73_0 = new Class73[parameters2.Length + 1];
			if (((MemberInfo)@class.object_0).DeclaringType.IsValueType)
			{
				class7.class73_0[0] = new Class84(new Class85(object_2), ((MemberInfo)@class.object_0).DeclaringType);
			}
			else
			{
				class7.class73_0[0] = new Class85(object_2);
			}
			for (int num10 = 0; num10 < parameters2.Length; num10++)
			{
				Type parameterType2 = parameters2[num10].ParameterType;
				if (parameterType2.IsByRef)
				{
					class7.class73_0[num10 + 1] = Class73.smethod_1(parameterType2, object_1[num10]);
					num9++;
				}
				else
				{
					class7.class73_0[num10 + 1] = Class73.smethod_1(parameterType2, object_1[num10]);
				}
			}
		}
		class7.class73_1 = new Class73[@class.list_1.Count];
		for (int num11 = 0; num11 < @class.list_1.Count; num11++)
		{
			Class65 class8 = @class.list_1[num11];
			switch (class8.enum3_0)
			{
			case (Enum3)0:
				class7.class73_1[num11] = null;
				break;
			case (Enum3)7:
			case (Enum3)8:
				class7.class73_1[num11] = new Class76(0L, class8.enum3_0);
				break;
			case (Enum3)9:
			case (Enum3)10:
				class7.class73_1[num11] = new Class78(0.0, class8.enum3_0);
				break;
			case (Enum3)12:
				class7.class73_1[num11] = new Class77(IntPtr.Zero);
				break;
			case (Enum3)13:
				class7.class73_1[num11] = new Class77(UIntPtr.Zero);
				break;
			case (Enum3)14:
				class7.class73_1[num11] = null;
				break;
			case (Enum3)1:
			case (Enum3)2:
			case (Enum3)3:
			case (Enum3)4:
			case (Enum3)5:
			case (Enum3)6:
			case (Enum3)11:
			case (Enum3)15:
				class7.class73_1[num11] = new Class75(0, class8.enum3_0);
				break;
			case (Enum3)16:
				class7.class73_1[num11] = new Class85(null);
				break;
			}
		}
		try
		{
			class7.method_0();
		}
		finally
		{
			class7.method_1();
		}
		int num12 = 0;
		if (flag)
		{
			num12 = 1;
		}
		num12 += num9;
		object[] array2 = new object[num12];
		if (flag)
		{
			array2[0] = null;
		}
		if (@class.object_0 is MethodInfo)
		{
			MethodInfo methodInfo = (MethodInfo)@class.object_0;
			if (methodInfo.ReturnType != typeof(void) && class7.class73_2 != null)
			{
				array2[0] = class7.class73_2.vmethod_4(methodInfo.ReturnType);
			}
		}
		if (num9 > 0)
		{
			int num13 = 0;
			if (flag)
			{
				num13++;
			}
			for (int num14 = 0; num14 < parameters2.Length; num14++)
			{
				Type parameterType3 = parameters2[num14].ParameterType;
				if (!parameterType3.IsByRef)
				{
					continue;
				}
				parameterType3 = parameterType3.GetElementType();
				if (class7.class73_0[num14] != null)
				{
					if (((MethodBase)@class.object_0).IsStatic)
					{
						array2[num13] = class7.class73_0[num14].vmethod_4(parameterType3);
					}
					else
					{
						array2[num13] = class7.class73_0[num14 + 1].vmethod_4(parameterType3);
					}
				}
				else
				{
					array2[num13] = null;
				}
				num13++;
			}
		}
		if (!((MethodBase)@class.object_0).IsStatic && ((MemberInfo)@class.object_0).DeclaringType.IsValueType)
		{
			gparam_0 = (T)class7.class73_0[0].vmethod_4(((MemberInfo)@class.object_0).DeclaringType);
		}
		return array2;
	}

	internal static object[] smethod_2(int int_1, object[] object_1, object object_2)
	{
		int gparam_ = 0;
		return smethod_1(int_1, object_1, object_2, ref gparam_);
	}

	internal static object[] smethod_3<T>(int int_1, object[] object_1, ref T gparam_0)
	{
		return smethod_1(int_1, object_1, gparam_0, ref gparam_0);
	}

	internal static void smethod_4()
	{
		if (int_0 == null)
		{
			BinaryReader binaryReader = new BinaryReader(typeof(Class62).Assembly.GetManifestResourceStream("yLUPAWu5sI6amrZjZN.TyOqZjKd8OubV3dwjU"));
			binaryReader.BaseStream.Position = 0L;
			byte[] byte_ = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
			binaryReader.Close();
			smethod_5(byte_);
		}
	}

	internal static void smethod_5(byte[] byte_1)
	{
		binaryReader_0 = new BinaryReader(new MemoryStream(byte_1));
		byte_0 = new byte[255];
		int num = smethod_6(binaryReader_0);
		for (int i = 0; i < num; i++)
		{
			int num2 = binaryReader_0.ReadByte();
			byte_0[num2] = binaryReader_0.ReadByte();
		}
		num = smethod_6(binaryReader_0);
		list_0 = new List<string>(num);
		for (int j = 0; j < num; j++)
		{
			list_0.Add(Encoding.Unicode.GetString(binaryReader_0.ReadBytes(smethod_6(binaryReader_0))));
		}
		num = smethod_6(binaryReader_0);
		class68_0 = new Class68[num];
		int_0 = new int[num];
		for (int k = 0; k < num; k++)
		{
			class68_0[k] = null;
			int_0[k] = smethod_6(binaryReader_0);
		}
		int num3 = (int)binaryReader_0.BaseStream.Position;
		for (int l = 0; l < num; l++)
		{
			int num4 = int_0[l];
			int_0[l] = num3;
			num3 += num4;
		}
	}

	internal static int smethod_6(BinaryReader binaryReader_1)
	{
		bool flag = false;
		uint num = 0u;
		uint num2 = binaryReader_1.ReadByte();
		num = 0u | (num2 & 0x3Fu);
		if ((num2 & 0x40u) != 0)
		{
			flag = true;
		}
		if (num2 >= 128)
		{
			int num3 = 0;
			while (true)
			{
				uint num4 = binaryReader_1.ReadByte();
				num |= (num4 & 0x7F) << 7 * num3 + 6;
				if (num4 < 128)
				{
					break;
				}
				num3++;
			}
			if (!flag)
			{
				return (int)num;
			}
			return (int)(~num);
		}
		if (!flag)
		{
			return (int)num;
		}
		return (int)(~num);
	}

	static Class62()
	{
		class68_0 = null;
		int_0 = null;
		bool_0 = false;
		object_0 = new object();
	}
}
