using System;
using System.Reflection;

internal class Class56
{
	internal delegate void Delegate0(object o);

	internal static Module module_0;

	internal static void smethod_0(int typemdt)
	{
		Type type = module_0.ResolveType(33554432 + typemdt);
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			MethodInfo method = (MethodInfo)module_0.ResolveMethod(fieldInfo.MetadataToken + 100663296);
			fieldInfo.SetValue(null, (MulticastDelegate)Delegate.CreateDelegate(type, method));
		}
	}

	static Class56()
	{
		module_0 = typeof(Class56).Assembly.ManifestModule;
	}
}
