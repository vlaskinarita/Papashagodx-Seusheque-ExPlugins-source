using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.Global;

public static class StuckDetection
{
	public const string TriggeredMessage = "stuck_detection_triggered_event";

	private static readonly Interval interval_0;

	private static int int_0;

	private static int int_1;

	private static readonly Dictionary<int, int> dictionary_0;

	private static Vector2i vector2i_0;

	private static int int_2;

	[CompilerGenerated]
	private static int int_3;

	[CompilerGenerated]
	private static bool bool_0;

	[CompilerGenerated]
	private static Action<int> action_0;

	public static int SmallStuckCount
	{
		[CompilerGenerated]
		get
		{
			return int_3;
		}
		[CompilerGenerated]
		private set
		{
			int_3 = value;
		}
	}

	public static bool StopBotOnStuck
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		set
		{
			bool_0 = value;
		}
	}

	public static event Action<int> Triggered
	{
		[CompilerGenerated]
		add
		{
			Action<int> action = action_0;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int> action = action_0;
			Action<int> action2;
			do
			{
				action2 = action;
				Action<int> value2 = (Action<int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref action_0, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	static StuckDetection()
	{
		interval_0 = new Interval(2500);
		dictionary_0 = new Dictionary<int, int>();
		Events.AreaChanged += OnAreaChanged;
	}

	private static void OnAreaChanged(object sender, AreaChangedArgs args)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Reset();
		int_2 = 0;
		vector2i_0 = Vector2i.Zero;
		dictionary_0.Clear();
	}

	public static void Reset()
	{
		SmallStuckCount = 0;
		int_0 = 0;
		int_1 = 0;
	}

	public static void Tick()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		if (!ExtensionsSettings.Instance.StuckDetectionEnabled || !interval_0.Elapsed || !LokiPoe.IsInGame || ((Actor)LokiPoe.Me).IsDead || !World.CurrentArea.IsCombatArea)
		{
			return;
		}
		FillMobData();
		if (CheckMobHpDecrease() || CheckWorldItemDecrease())
		{
			return;
		}
		Vector2i myPosition = LokiPoe.MyPosition;
		if (((Vector2i)(ref myPosition)).Distance(vector2i_0) >= 30)
		{
			SmallStuckCount = 0;
		}
		else
		{
			SmallStuckCount = int_3 + 1;
			if (int_3 >= 1)
			{
				HandleBlockingChestsTask.Enabled = true;
			}
			if (int_3 >= 3)
			{
				GlobalLog.Error($"[StuckDetection] Small range stuck count: {int_3}");
				int num = LokiPoe.Random.Next(-25, 25);
				WalkablePosition walkablePosition = new WalkablePosition("unstuck pos", new Vector2i(myPosition.X + num, myPosition.Y + num), 5);
				walkablePosition.Initialize();
				walkablePosition.TryCome();
			}
			if (int_3 >= ExtensionsSettings.Instance.MaxStuckCountSmall)
			{
				GlobalLog.Error($"[StuckDetection] Small range stuck count: {int_3}. Now logging out.");
				HandleStuck();
				return;
			}
		}
		if (((Vector2i)(ref myPosition)).Distance(vector2i_0) >= 50)
		{
			int_0 = 0;
		}
		else
		{
			int_0++;
			if (int_0 >= ExtensionsSettings.Instance.MaxStuckCountSmall)
			{
				GlobalLog.Debug($"[StuckDetection] Medium range stuck count: {int_0}");
			}
			if (int_0 >= ExtensionsSettings.Instance.MaxStuckCountMedium)
			{
				GlobalLog.Error($"[StuckDetection] Medium range stuck count: {int_0}. Now logging out.");
				HandleStuck();
				return;
			}
		}
		if (((Vector2i)(ref myPosition)).Distance(vector2i_0) < 70)
		{
			int_1++;
			if (int_1 >= ExtensionsSettings.Instance.MaxStuckCountMedium)
			{
				GlobalLog.Debug($"[StuckDetection] Long range stuck count: {int_1}");
			}
			if (int_1 >= ExtensionsSettings.Instance.MaxStuckCountLong)
			{
				GlobalLog.Error($"[StuckDetection] Long range stuck count: {int_1}. Now logging out.");
				HandleStuck();
			}
		}
		else
		{
			int_1 = 0;
		}
		vector2i_0 = myPosition;
	}

	private static void FillMobData()
	{
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val == (NetworkObject)null) && IsMonsterForScan(val))
			{
				int id = ((NetworkObject)val).Id;
				if (!dictionary_0.ContainsKey(id))
				{
					dictionary_0.Add(id, ((Actor)val).Health);
				}
			}
		}
	}

	private static bool CheckMobHpDecrease()
	{
		bool result = false;
		List<int> list = new List<int>();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> keyValuePair_0 in dictionary_0)
		{
			Monster val = ObjectManager.Objects.FirstOrDefault((Monster m) => ((NetworkObject)m).Id == keyValuePair_0.Key);
			if (!((NetworkObject)(object)val == (NetworkObject)null))
			{
				if (((Actor)val).IsDead)
				{
					list.Add(keyValuePair_0.Key);
					result = true;
				}
				else if (((Actor)val).Health < keyValuePair_0.Value)
				{
					result = true;
					dictionary.Add(keyValuePair_0.Key, ((Actor)val).Health);
				}
			}
			else
			{
				list.Add(keyValuePair_0.Key);
			}
		}
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			dictionary_0[item.Key] = item.Value;
		}
		foreach (int item2 in list)
		{
			dictionary_0.Remove(item2);
		}
		return result;
	}

	private static bool CheckWorldItemDecrease()
	{
		int count = CombatAreaCache.Current.Items.Count;
		bool result = count < int_2;
		int_2 = count;
		return result;
	}

	private static void HandleStuck()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		Utility.BroadcastMessage((object)null, "stuck_detection_triggered_event", Array.Empty<object>());
		if (bool_0)
		{
			GlobalLog.Warn("[StuckDetection] StopBotOnStuck is true. Now reseting stuck counters and stopping the bot.");
			Reset();
			BotManager.Stop(new StopReasonData("stop_on_stuck", "StopBotOnStuck is true. Now reseting stuck counters and stopping the bot.", (object)null), false);
			return;
		}
		CombatAreaCache current = CombatAreaCache.Current;
		int stuckCount = current.StuckCount + 1;
		current.StuckCount = stuckCount;
		GlobalLog.Error($"[StuckDetection] Stuck incidents in current area: {current.StuckCount}");
		if (current.StuckCount >= ExtensionsSettings.Instance.MaxStucksPerInstance)
		{
			GlobalLog.Error("[StuckDetection] Too many stuck incidents in current area (" + World.CurrentArea.Name + "). Now requesting a new instance.");
			EXtensions.AbandonCurrentArea();
		}
		if ((int)EscapeState.LogoutToCharacterSelection() <= 0)
		{
			GlobalLog.Info($"[StuckDetection] Triggered event ({current.StuckCount})");
			action_0?.Invoke(current.StuckCount);
			Reset();
		}
		else
		{
			GlobalLog.Error("[StuckDetection] Logout error.");
			stuckCount = current.StuckCount - 1;
			current.StuckCount = stuckCount;
		}
	}

	private static bool IsMonsterForScan(Monster m)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		return !((Actor)m).IsDead && (int)((NetworkObject)m).Reaction == 1 && ((NetworkObject)m).Distance <= 100f;
	}
}
