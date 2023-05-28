using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Components;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.Game.Utilities;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using JetBrains.Annotations;

namespace ExPlugins.EXtensions;

public static class ClassExtensions
{
	public static float ScreenDistance(this Vector3 thisPos, Vector2 targetPos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		int num = default(int);
		int num2 = default(int);
		ClientFunctions.WorldToScreen(thisPos, ref num, ref num2);
		return ((Vector2)(ref targetPos)).Distance(new Vector2((float)num, (float)num2)) / 10f;
	}

	public static bool EqualsIgnorecase(this string thisStr, string str)
	{
		return thisStr.Equals(str, StringComparison.OrdinalIgnoreCase);
	}

	public static bool ContainsIgnorecase(this string thisStr, string str)
	{
		return thisStr.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	public static void LogProperties(this object obj)
	{
		Type type = obj.GetType();
		string name = type.Name;
		PropertyInfo[] properties = obj.GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			GlobalLog.Info(string.Format("[{0}] {1}: {2}", name, propertyInfo.Name, propertyInfo.GetValue(obj) ?? "null"));
		}
	}

	public static Item FindItemByPos(this Inventory inventory, Vector2i pos)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return inventory.Items.Find((Item i) => i.LocationTopLeft == pos);
	}

	public static Item FindItemById(this Inventory inventory, int id)
	{
		return inventory.Items.Find((Item i) => i.LocalId == id);
	}

	public static WorldPosition WorldPosition(this NetworkObject obj)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return new WorldPosition(obj.Position);
	}

	public static WalkablePosition WalkablePosition(this NetworkObject obj, int step = 10, int radius = 30)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new WalkablePosition(obj.Name, obj.Position, step, radius);
	}

	public static bool PathExists(this NetworkObject obj)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ExilePather.PathExistsBetween(LokiPoe.MyPosition, obj.Position, false);
	}

	public static float PathDistance(this NetworkObject obj)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ExilePather.PathDistance(LokiPoe.MyPosition, obj.Position, false, false);
	}

	public static TownNpc AsTownNpc(this NetworkObject npc)
	{
		return new TownNpc(npc);
	}

	public static bool IsPlayerPortal(this Portal p)
	{
		return ((NetworkObject)p).IsTargetable && p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsTown);
	}

	public static bool LeadsTo(this Portal p, AreaInfo area)
	{
		Portal portalComponent = ((NetworkObject)p).Components.PortalComponent;
		DatWorldAreaWrapper val = ((portalComponent == null) ? null : portalComponent.Area);
		return val != null && val == area;
	}

	public static bool LeadsTo(this Portal p, Func<DatWorldAreaWrapper, bool> match)
	{
		object obj;
		if (p == null)
		{
			obj = null;
		}
		else
		{
			EntityComponentInformation components = ((NetworkObject)p).Components;
			if (components == null)
			{
				obj = null;
			}
			else
			{
				Portal portalComponent = components.PortalComponent;
				obj = ((portalComponent != null) ? portalComponent.Destination : null);
			}
		}
		DatWorldAreaWrapper val = (DatWorldAreaWrapper)obj;
		return val != null && match(val);
	}

	public static bool LeadsTo(this AreaTransition a, AreaInfo area)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)a.TransitionType == 1)
		{
			return false;
		}
		AreaTransition areaTransitionComponent = ((NetworkObject)a).Components.AreaTransitionComponent;
		DatWorldAreaWrapper val = ((areaTransitionComponent == null) ? null : areaTransitionComponent.Destination);
		return val != null && val == area;
	}

	public static bool LeadsTo(this AreaTransition a, Func<DatWorldAreaWrapper, bool> match)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)a.TransitionType == 1)
		{
			return false;
		}
		AreaTransition areaTransitionComponent = ((NetworkObject)a).Components.AreaTransitionComponent;
		DatWorldAreaWrapper val = ((areaTransitionComponent != null) ? areaTransitionComponent.Destination : null);
		return val != null && match(val);
	}

	public static T Fresh<T>(this T obj) where T : NetworkObject
	{
		NetworkObject obj2 = ObjectManager.Objects.Find((NetworkObject o) => o.Id == ((NetworkObject)obj).Id);
		return (T)(object)((obj2 is T) ? obj2 : null);
	}

	[CanBeNull]
	public static T Closest<T>(this IEnumerable<T> collection) where T : NetworkObject
	{
		T val = default(T);
		foreach (T item in collection)
		{
			if ((NetworkObject)(object)val == (NetworkObject)null || ((NetworkObject)item).Distance < ((NetworkObject)val).Distance)
			{
				val = item;
			}
		}
		return val;
	}

	[CanBeNull]
	public static T Closest<T>(this IEnumerable collection) where T : NetworkObject
	{
		T val = default(T);
		foreach (object item in collection)
		{
			T val2 = (T)((item is T) ? item : null);
			if (!((NetworkObject)(object)val2 == (NetworkObject)null) && ((NetworkObject)(object)val == (NetworkObject)null || ((NetworkObject)val2).Distance < ((NetworkObject)val).Distance))
			{
				val = val2;
			}
		}
		return val;
	}

	public static List<T> All<T>(this IEnumerable collection) where T : NetworkObject
	{
		List<T> list = new List<T>();
		foreach (object item in collection)
		{
			T val = (T)((item is T) ? item : null);
			if ((NetworkObject)(object)val != (NetworkObject)null)
			{
				list.Add(val);
			}
		}
		return list;
	}

	[CanBeNull]
	public static T Closest<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : NetworkObject
	{
		T val = default(T);
		foreach (T item in collection)
		{
			if (match(item) && ((NetworkObject)(object)val == (NetworkObject)null || ((NetworkObject)item).Distance < ((NetworkObject)val).Distance))
			{
				val = item;
			}
		}
		return val;
	}

	[CanBeNull]
	public static T Closest<T>(this IEnumerable collection, Func<T, bool> match) where T : NetworkObject
	{
		T val = default(T);
		foreach (object item in collection)
		{
			T val2 = (T)((item is T) ? item : null);
			if ((NetworkObject)(object)val2 != (NetworkObject)null && match(val2) && ((NetworkObject)(object)val == (NetworkObject)null || ((NetworkObject)val2).Distance < ((NetworkObject)val).Distance))
			{
				val = val2;
			}
		}
		return val;
	}

	[CanBeNull]
	public static T Random<T>(this IEnumerable collection, Func<T, bool> match) where T : NetworkObject
	{
		List<T> list = new List<T>();
		foreach (object item in collection)
		{
			T val = (T)((item is T) ? item : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && match(val))
			{
				list.Add(val);
			}
		}
		return (list.Count == 0) ? default(T) : list[LokiPoe.Random.Next(list.Count)];
	}

	[CanBeNull]
	public static T FirstOrDefault<T>(this IEnumerable collection) where T : class
	{
		foreach (object item in collection)
		{
			if (item is T result)
			{
				return result;
			}
		}
		return null;
	}

	[CanBeNull]
	public static T FirstOrDefault<T>(this IEnumerable collection, Func<T, bool> match) where T : class
	{
		foreach (object item in collection)
		{
			if (item is T val && match(val))
			{
				return val;
			}
		}
		return null;
	}

	public static bool Any<T>(this IEnumerable collection, Func<T, bool> match) where T : class
	{
		foreach (object item in collection)
		{
			if (item is T arg && match(arg))
			{
				return true;
			}
		}
		return false;
	}

	public static IEnumerable<T> Where<T>(this IEnumerable collection, Func<T, bool> match) where T : class
	{
		foreach (object element in collection)
		{
			T t = element as T;
			if (t != null && match(t))
			{
				yield return t;
			}
		}
	}

	public static IEnumerable<T> Valid<T>(this IEnumerable<T> collection) where T : CachedObject
	{
		return Enumerable.Where(collection, (T e) => !e.Ignored && !e.Unwalkable);
	}

	public static IEnumerable<T> Valid<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : CachedObject
	{
		return Enumerable.Where(collection, (T e) => !e.Ignored && !e.Unwalkable && match(e));
	}

	[CanBeNull]
	public static T ClosestValid<T>(this IEnumerable<T> collection) where T : CachedObject
	{
		T val = null;
		foreach (T item in collection)
		{
			if (!item.Ignored && !item.Unwalkable && !(val?.Position.Distance <= item.Position.Distance))
			{
				val = item;
			}
		}
		return val;
	}

	[CanBeNull]
	public static T ClosestValid<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : CachedObject
	{
		T val = null;
		foreach (T item in collection)
		{
			if (!item.Ignored && !item.Unwalkable && match(item) && !(val?.Position.Distance <= item.Position.Distance))
			{
				val = item;
			}
		}
		return val;
	}
}
