using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx;
using ExPlugins.QuestBotEx;

namespace ExPlugins.EXtensions.CommonTasks;

public class FollowerHandlerTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static readonly HashSet<string> hashSet_0;

	private static Stopwatch stopwatch_0;

	private int int_0;

	private int int_1;

	private bool bool_0;

	public string Name => "FollowerHandlerTask";

	public string Description => "Task to handle follower invites/kicks.";

	public string Author => "Seusheque";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (ExtensionsSettings.Instance.UseFollower && !ExtensionsSettings.Instance.Followers.All((FollowerEntry e) => e == null))
			{
				if (((IAuthored)BotManager.Current).Name != "QuestBotEx" && ((IAuthored)BotManager.Current).Name != "MapBotEx")
				{
					GlobalLog.Error("[" + Name + "] This task is only compatible with MapBotEx or QuestBotEx. Removing.");
					RemoveMe();
					return false;
				}
				if (int_0 > 25)
				{
					GlobalLog.Error($"[{Name}] Global Invite Tries: {int_0}. Some of the followers must be offline. Removing task.");
					RemoveMe();
					return false;
				}
				if (!ExtensionsSettings.Instance.UseFollower || !Enumerable.Any(ExtensionsSettings.Instance.Followers, (FollowerEntry e) => e == null))
				{
					if (!((Actor)LokiPoe.Me).IsDead)
					{
						if (ExtensionsSettings.Instance.UseFollower && interval_0.Elapsed)
						{
							if (!(((IAuthored)BotManager.Current).Name != "QuestBotEx") && !(QuestBotSettings.Instance.CurrentQuestName != "Deal with the Bandits"))
							{
								if (InstanceInfo.PartyMembers.Any())
								{
									await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("/kick " + ((NetworkObject)LokiPoe.Me).Name);
									return true;
								}
							}
							else
							{
								foreach (FollowerEntry followerEntry_0 in ExtensionsSettings.Instance.Followers)
								{
									if (Enumerable.Any(InstanceInfo.PartyMembers, (PartyMember m) => m.PlayerEntry.Name.EqualsIgnorecase(followerEntry_0.Name)))
									{
										continue;
									}
									while (int_1 < 5)
									{
										await Coroutines.FinishCurrentAction(true);
										ProcessHookManager.ClearAllKeyStates();
										GlobalLog.Debug("[" + Name + "] Inviting " + followerEntry_0.Name + ".");
										ChatResult res = await global::ExPlugins.MapBotEx.MapBotEx.SendChatMsg("/invite " + followerEntry_0.Name);
										if ((int)res > 0)
										{
											GlobalLog.Error(res);
											continue;
										}
										if (!(await Wait.For(() => Enumerable.Any(InstanceInfo.PartyMembers, (PartyMember m) => m.PlayerEntry.Name.EqualsIgnorecase(followerEntry_0.Name) && (int)m.MemberStatus == 2), followerEntry_0.Name + " to accept party invite", 500, 6000)))
										{
											int_1++;
											int_0++;
											continue;
										}
										GlobalLog.Debug("[" + Name + "] " + followerEntry_0.Name + " sucessfully invited.");
										int_1 = 0;
										break;
									}
								}
							}
						}
						if (!World.CurrentArea.IsCombatArea)
						{
							return false;
						}
						foreach (PartyMember partyMember_0 in InstanceInfo.PartyMembers)
						{
							if ((int)partyMember_0.MemberStatus != 2 || (RemoteMemoryObject)(object)partyMember_0.PlayerEntry == (RemoteMemoryObject)null)
							{
								continue;
							}
							FollowerEntry follower = Enumerable.FirstOrDefault(ExtensionsSettings.Instance.Followers, (FollowerEntry e) => e.Name.EqualsIgnorecase(partyMember_0.PlayerEntry.Name));
							if (follower == null)
							{
								continue;
							}
							if (partyMember_0.PlayerEntry.Area.WorldAreaId != World.CurrentArea.WorldAreaId)
							{
								if (!follower.WaitForSameZone || hashSet_0.Contains(follower.Name))
								{
									continue;
								}
								if (!bool_0)
								{
									if (stopwatch_0 == null || !stopwatch_0.IsRunning)
									{
										stopwatch_0 = Stopwatch.StartNew();
									}
									GlobalLog.Debug("[" + Name + "] Waiting for " + partyMember_0.PlayerEntry.Name + " to come from " + partyMember_0.PlayerEntry.Area.Name + " to " + World.CurrentArea.Name);
									StuckDetection.Reset();
									if (!((Actor)LokiPoe.Me).HasAura("Grace Period"))
									{
										if (stopwatch_0.Elapsed.TotalSeconds > 25.0)
										{
											GlobalLog.Debug("[" + Name + "] We are waiting for " + partyMember_0.PlayerEntry.Name + " to come from " + partyMember_0.PlayerEntry.Area.Name + " for too long. Maybe creating portal will help?");
											await PlayerAction.CreateTownPortal();
											bool_0 = true;
											stopwatch_0.Reset();
										}
										return ((Actor)LokiPoe.Me).HasAura("Grace Period");
									}
									await Wait.SleepSafe(2000);
									return true;
								}
								GlobalLog.Error("[" + Name + "] We tried new portal but " + partyMember_0.PlayerEntry.Name + " still didn't join current zone. Seems like we ran out of map portals. Skip waiting.");
								hashSet_0.Add(follower.Name);
							}
							else
							{
								Player player = Enumerable.FirstOrDefault(ObjectManager.GetObjectsByType<Player>(), (Player x) => ((NetworkObject)x).Name == partyMember_0.PlayerEntry.Name);
								WalkablePosition walkPos = ((NetworkObject)(object)player).WalkablePosition();
								if (walkPos.Distance > follower.MaxDistance)
								{
									GlobalLog.Debug($"[{Name}] Waiting for {partyMember_0.PlayerEntry.Name} to catch up. Distance: {walkPos.Distance}");
									walkPos.TryCome();
									StuckDetection.Reset();
									return true;
								}
							}
						}
						return false;
					}
					return false;
				}
				string error = "[" + Name + "] Follower Names List contains empty entries. Please check.";
				GlobalLog.Error(error);
				BotManager.Stop(new StopReasonData("empty_follower_entry", error, (object)null), false);
				return false;
			}
			GlobalLog.Debug("[" + Name + "] Followers not used. Removing " + Name + ".");
			RemoveMe();
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "combat_area_changed_event"))
		{
			return (MessageResult)1;
		}
		hashSet_0.Clear();
		int_1 = 0;
		bool_0 = false;
		return (MessageResult)0;
	}

	public void RemoveMe()
	{
		ITask taskByName = ((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).GetTaskByName(Name);
		if (taskByName != null)
		{
			if (((TaskManagerBase<ITask>)(object)BotStructure.TaskManager).Remove(Name))
			{
				GlobalLog.Debug("[" + Name + "] Remove " + Name + " success.");
			}
			else
			{
				GlobalLog.Error("[" + Name + "] Remove " + Name + " failed.");
			}
		}
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static FollowerHandlerTask()
	{
		interval_0 = new Interval(10000);
		hashSet_0 = new HashSet<string>();
	}
}
