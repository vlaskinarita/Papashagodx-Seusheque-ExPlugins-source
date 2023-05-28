using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions;

public static class Events
{
	public static class Messages
	{
		public const string IngameBotStart = "ingame_bot_start_event";

		public const string AreaChanged = "area_changed_event";

		public const string CombatAreaChanged = "combat_area_changed_event";

		public const string PlayerDied = "player_died_event";

		public const string PlayerResurrected = "player_resurrected_event";

		public const string PlayerLeveled = "player_leveled_event";

		public const string ItemLootedEvent = "item_looted_event";

		public const string ItemStashedEvent = "item_stashed_event";

		public const string ItemsStashedEvent = "items_stashed_event";

		public const string ItemsSoldEvent = "items_sold_event";

		public const string NewTransitionCachedMessage = "combat_area_new_transition_event";

		public const string PricedItemLootedEvent = "priced_item_looted_event";
	}

	private static bool bool_0;

	private static uint uint_0;

	private static uint uint_1;

	private static DatWorldAreaWrapper datWorldAreaWrapper_0;

	private static DatWorldAreaWrapper meMqgvwhno;

	private static bool bool_1;

	private static string string_0;

	private static int int_0;

	[CompilerGenerated]
	private static Action action_0;

	[CompilerGenerated]
	private static EventHandler<AreaChangedArgs> eventHandler_0;

	[CompilerGenerated]
	private static EventHandler<AreaChangedArgs> eventHandler_1;

	[CompilerGenerated]
	private static Action<int> action_1;

	[CompilerGenerated]
	private static Action action_2;

	[CompilerGenerated]
	private static Action<int> action_3;

	[CompilerGenerated]
	private static Action<CachedItem> action_4;

	[CompilerGenerated]
	private static Action<CachedItem> action_5;

	[CompilerGenerated]
	private static EventHandler<ItemsSoldArgs> eventHandler_2;

	[CompilerGenerated]
	private static Action<CachedTransition> action_6;

	[CompilerGenerated]
	private static Action<KeyValuePair<Item, double>> action_7;

	public static event Action IngameBotStart
	{
		[CompilerGenerated]
		add
		{
			Action action = action_0;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = action_0;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event EventHandler<AreaChangedArgs> AreaChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler<AreaChangedArgs> eventHandler = eventHandler_0;
			EventHandler<AreaChangedArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<AreaChangedArgs> value2 = (EventHandler<AreaChangedArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<AreaChangedArgs> eventHandler = eventHandler_0;
			EventHandler<AreaChangedArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<AreaChangedArgs> value2 = (EventHandler<AreaChangedArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public static event EventHandler<AreaChangedArgs> CombatAreaChanged
	{
		[CompilerGenerated]
		add
		{
			EventHandler<AreaChangedArgs> eventHandler = eventHandler_1;
			EventHandler<AreaChangedArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<AreaChangedArgs> value2 = (EventHandler<AreaChangedArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<AreaChangedArgs> eventHandler = eventHandler_1;
			EventHandler<AreaChangedArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<AreaChangedArgs> value2 = (EventHandler<AreaChangedArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public static event Action<int> PlayerDied
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = action_1;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_1, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> action = action_1;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_1, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action PlayerResurrected
	{
		[CompilerGenerated]
		add
		{
			Action action = action_2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_2, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = action_2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_2, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<int> PlayerLeveled
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = action_3;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_3, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> action = action_3;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_3, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<CachedItem> ItemLootedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<CachedItem> action = action_4;
			Action<CachedItem> action2;
			do
			{
				action2 = action;
				Action<CachedItem> value2 = (Action<CachedItem>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_4, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CachedItem> action = action_4;
			Action<CachedItem> action2;
			do
			{
				action2 = action;
				Action<CachedItem> value2 = (Action<CachedItem>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_4, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<CachedItem> ItemStashedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<CachedItem> action = action_5;
			Action<CachedItem> action2;
			do
			{
				action2 = action;
				Action<CachedItem> value2 = (Action<CachedItem>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_5, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CachedItem> action = action_5;
			Action<CachedItem> action2;
			do
			{
				action2 = action;
				Action<CachedItem> value2 = (Action<CachedItem>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_5, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event EventHandler<ItemsSoldArgs> ItemsSoldEvent
	{
		[CompilerGenerated]
		add
		{
			EventHandler<ItemsSoldArgs> eventHandler = eventHandler_2;
			EventHandler<ItemsSoldArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ItemsSoldArgs> value2 = (EventHandler<ItemsSoldArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler<ItemsSoldArgs> eventHandler = eventHandler_2;
			EventHandler<ItemsSoldArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<ItemsSoldArgs> value2 = (EventHandler<ItemsSoldArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public static event Action<CachedTransition> NewTransitionCached
	{
		[CompilerGenerated]
		add
		{
			Action<CachedTransition> action = action_6;
			Action<CachedTransition> action2;
			do
			{
				action2 = action;
				Action<CachedTransition> value2 = (Action<CachedTransition>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_6, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<CachedTransition> action = action_6;
			Action<CachedTransition> action2;
			do
			{
				action2 = action;
				Action<CachedTransition> value2 = (Action<CachedTransition>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_6, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<KeyValuePair<Item, double>> PricedItemLootedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<KeyValuePair<Item, double>> action = action_7;
			Action<KeyValuePair<Item, double>> action2;
			do
			{
				action2 = action;
				Action<KeyValuePair<Item, double>> value2 = (Action<KeyValuePair<Item, double>>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_7, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<KeyValuePair<Item, double>> action = action_7;
			Action<KeyValuePair<Item, double>> action2;
			do
			{
				action2 = action;
				Action<KeyValuePair<Item, double>> value2 = (Action<KeyValuePair<Item, double>>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_7, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static void Start()
	{
		bool_0 = true;
	}

	public static void Tick()
	{
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		if (bool_0)
		{
			bool_0 = false;
			GlobalLog.Info("[Events] Ingame bot start.");
			Utility.BroadcastMessage((object)null, "ingame_bot_start_event", Array.Empty<object>());
		}
		uint areaHash = LocalData.AreaHash;
		DatWorldAreaWrapper val;
		uint num;
		string name;
		string[] obj;
		object obj2;
		if (areaHash != uint_0)
		{
			val = datWorldAreaWrapper_0;
			num = uint_0;
			datWorldAreaWrapper_0 = World.CurrentArea;
			uint_0 = areaHash;
			name = datWorldAreaWrapper_0.Name;
			obj = new string[5] { "[Events] Area changed (", null, null, null, null };
			if (val == null)
			{
				obj2 = null;
			}
			else
			{
				obj2 = val.Name;
				if (obj2 != null)
				{
					goto IL_00a3;
				}
			}
			obj2 = "null";
			goto IL_00a3;
		}
		goto IL_01df;
		IL_00a3:
		obj[1] = (string)obj2;
		obj[2] = " -> ";
		obj[3] = name;
		obj[4] = ")";
		GlobalLog.Info(string.Concat(obj));
		Utility.BroadcastMessage((object)null, "area_changed_event", new object[4] { num, areaHash, val, datWorldAreaWrapper_0 });
		if (GlobalSettings.Instance.IsBackgroundFpsActive)
		{
			ClientFunctions.SetBackgroundFps(datWorldAreaWrapper_0.IsHideoutArea ? 60 : GlobalSettings.Instance.BackgroundFps, false);
		}
		DatWorldAreaWrapper val2;
		uint num2;
		string[] obj3;
		object obj4;
		if (areaHash != uint_1 && datWorldAreaWrapper_0.IsCombatArea)
		{
			val2 = meMqgvwhno;
			num2 = uint_1;
			meMqgvwhno = datWorldAreaWrapper_0;
			uint_1 = areaHash;
			obj3 = new string[5] { "[Events] Combat area changed (", null, null, null, null };
			if (val2 != null)
			{
				obj4 = val2.Name;
				if (obj4 != null)
				{
					goto IL_0189;
				}
			}
			else
			{
				obj4 = null;
			}
			obj4 = "null";
			goto IL_0189;
		}
		goto IL_01df;
		IL_0189:
		obj3[1] = (string)obj4;
		obj3[2] = " -> ";
		obj3[3] = name;
		obj3[4] = ")";
		GlobalLog.Info(string.Concat(obj3));
		Utility.BroadcastMessage((object)null, "combat_area_changed_event", new object[4] { num2, areaHash, val2, datWorldAreaWrapper_0 });
		goto IL_01df;
		IL_01df:
		LocalPlayer me = LokiPoe.Me;
		if (((Actor)me).IsDead)
		{
			if (!bool_1)
			{
				bool_1 = true;
				CombatAreaCache current = CombatAreaCache.Current;
				int deathCount = current.DeathCount + 1;
				current.DeathCount = deathCount;
				GlobalLog.Info($"[Events] Player died ({current.DeathCount})");
				Utility.BroadcastMessage((object)null, "player_died_event", new object[1] { current.DeathCount });
			}
		}
		else
		{
			bool_1 = false;
		}
		string name2 = ((NetworkObject)me).Name;
		int level = ((Player)me).Level;
		if (!(name2 != string_0))
		{
			if (level > int_0)
			{
				int_0 = level;
				GlobalLog.Info($"[Events] Player leveled ({level})");
				Utility.BroadcastMessage((object)null, "player_leveled_event", new object[1] { level });
			}
		}
		else
		{
			string_0 = name2;
			int_0 = level;
		}
	}

	public static void FireEventsFromMessage(Message message)
	{
		switch (message.Id)
		{
		case "ingame_bot_start_event":
			action_0?.Invoke();
			break;
		case "priced_item_looted_event":
			action_7?.Invoke(message.GetInput<KeyValuePair<Item, double>>(0));
			break;
		case "item_stashed_event":
			action_5?.Invoke(message.GetInput<CachedItem>(0));
			break;
		case "player_leveled_event":
			action_3?.Invoke(message.GetInput<int>(0));
			break;
		case "player_died_event":
			action_1?.Invoke(message.GetInput<int>(0));
			break;
		case "item_looted_event":
			action_4?.Invoke(message.GetInput<CachedItem>(0));
			break;
		case "items_sold_event":
			eventHandler_2?.Invoke(null, new ItemsSoldArgs(message));
			break;
		case "combat_area_changed_event":
			eventHandler_1?.Invoke(null, new AreaChangedArgs(message));
			break;
		case "combat_area_new_transition_event":
			action_6?.Invoke(message.GetInput<CachedTransition>(0));
			break;
		case "player_resurrected_event":
			action_2?.Invoke();
			break;
		case "area_changed_event":
			eventHandler_0?.Invoke(null, new AreaChangedArgs(message));
			break;
		}
	}
}
