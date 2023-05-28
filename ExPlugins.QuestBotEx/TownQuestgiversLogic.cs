using System.Collections.Generic;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public static class TownQuestgiversLogic
{
	private static readonly List<CachedObject> list_0;

	private static CachedObject cachedObject_0;

	public static bool ShouldExecute
	{
		get
		{
			string currentQuestState = QuestBotSettings.Instance.CurrentQuestState;
			if (string.IsNullOrEmpty(currentQuestState))
			{
				return true;
			}
			if (!currentQuestState.StartsWith("Talk to") && !currentQuestState.StartsWith("Turn in"))
			{
				if (!currentQuestState.EndsWith("reward"))
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public static async Task<bool> Execute()
	{
		if (cachedObject_0 == null && (cachedObject_0 = list_0.ClosestValid()) == null)
		{
			return false;
		}
		WalkablePosition pos = cachedObject_0.Position;
		string name = pos.Name;
		if (!(name == "Eramir") && !(name == "Helena") && !(name == "Lilly Roth"))
		{
			GlobalLog.Debug($"[TalkToQuestgivers] Now going to talk to {pos}");
			if (!(await pos.TryComeAtOnce()))
			{
				GlobalLog.Error("[TalkToQuestgivers] Unexpected error. \"" + name + "\" position is unwalkable.");
				cachedObject_0.Unwalkable = true;
				cachedObject_0 = null;
				return true;
			}
			if (!(cachedObject_0 == null))
			{
				NetworkObject obj = cachedObject_0.Object;
				if (obj == (NetworkObject)null)
				{
					GlobalLog.Error("[TalkToQuestgivers] Unexpected error. \"" + name + "\" object is null.");
					cachedObject_0.Ignored = true;
					cachedObject_0 = null;
					return true;
				}
				if (obj.IsTargetable)
				{
					if (!obj.HasNpcFloatingIcon)
					{
						GlobalLog.Debug("[TalkToQuestgivers] \"" + name + "\" no longer has NpcFloatingIcon.");
						list_0.Remove(cachedObject_0);
						cachedObject_0 = null;
						return true;
					}
					if (++cachedObject_0.InteractionAttempts > 5)
					{
						GlobalLog.Error("[TalkToQuestgivers] All attempts to interact with \"" + name + "\" have been spent. Now ignoring it.");
						cachedObject_0.Ignored = true;
						cachedObject_0 = null;
						return true;
					}
					if (!(await obj.AsTownNpc().Talk()))
					{
						await Wait.SleepSafe(1000);
						return true;
					}
					await Coroutines.CloseBlockingWindows();
					await Wait.SleepSafe(200);
					if (!obj.Fresh<NetworkObject>().HasNpcFloatingIcon)
					{
						list_0.Remove(cachedObject_0);
						cachedObject_0 = null;
					}
					return true;
				}
				GlobalLog.Error("[TalkToQuestgivers] Unexpected error. \"" + name + "\" is untargetable.");
				cachedObject_0.Ignored = true;
				cachedObject_0 = null;
				return true;
			}
			return false;
		}
		GlobalLog.Info("[TalkToQuestgivers] We don't talk to \"" + name + "\" because that will fck up quest logic.");
		cachedObject_0.Ignored = true;
		cachedObject_0 = null;
		return true;
	}

	public static void Tick()
	{
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Npc val = (Npc)(object)((@object is Npc) ? @object : null);
			if (val == null || ((NetworkObject)val).Name == "Navali")
			{
				continue;
			}
			int int_0 = ((NetworkObject)val).Id;
			CachedObject cachedObject = list_0.Find((CachedObject n) => n.Id == int_0);
			if (!((NetworkObject)val).HasNpcFloatingIcon)
			{
				if (cachedObject != null)
				{
					GlobalLog.Debug("[TalkToQuestgivers] Removing \"" + ((NetworkObject)val).Name + "\" from the talk list.");
					if (cachedObject == cachedObject_0)
					{
						cachedObject_0 = null;
					}
					list_0.Remove(cachedObject);
				}
			}
			else if (cachedObject == null)
			{
				WalkablePosition position = ((NetworkObject)(object)val).WalkablePosition(5, 20);
				list_0.Add(new CachedObject(int_0, position));
				GlobalLog.Debug("[TalkToQuestgivers] Adding \"" + ((NetworkObject)val).Name + "\" to the talk list.");
			}
		}
	}

	public static void Reset()
	{
		cachedObject_0 = null;
		list_0.Clear();
	}

	static TownQuestgiversLogic()
	{
		list_0 = new List<CachedObject>();
	}
}
