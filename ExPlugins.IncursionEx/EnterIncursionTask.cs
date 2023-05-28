using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.IncursionEx;

public class EnterIncursionTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Stopwatch stopwatch_0;

	private static AreaTransition TimePortal => (from a in ObjectManager.Objects
		where ((NetworkObject)a).Metadata.Contains("IncursionPortal") && ((NetworkObject)a).Components.TransitionableComponent.Flag1 < 3 && ((NetworkObject)a).Distance > -1f && ((NetworkObject)a).Distance < 1000f
		select a into x
		orderby ((NetworkObject)x).Distance
		select x).FirstOrDefault();

	private static AreaTransition InactiveTimePortal => (from a in ObjectManager.Objects
		where ((NetworkObject)a).Metadata.Contains("IncursionPortal") && ((NetworkObject)a).Distance < 70f && ((NetworkObject)a).Components.TransitionableComponent.Flag1 == 3
		select a into x
		orderby ((NetworkObject)x).Distance
		select x).FirstOrDefault();

	private static bool AnyPortalsNearby => ObjectManager.Objects.Any((Portal p) => ((NetworkObject)p).IsTargetable && ((NetworkObject)p).Distance <= 70f && p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsMapRoom));

	public string Name => "EnterIncursionTask";

	public string Description => "Task that enters incursions.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsMap || area.IsOverworldArea)
		{
			if (!CombatAreaCache.IsInIncursion)
			{
				if (Incursion.IncursionsRemaining == 0)
				{
					return false;
				}
				CachedObject portals = IncursionEx.CachedPortal;
				if (!(portals == null))
				{
					WalkablePosition pos = portals.Position;
					if (!pos.IsFar && !pos.IsFarByPath)
					{
						AreaTransition timePortal2 = TimePortal;
						if ((NetworkObject)(object)timePortal2 == (NetworkObject)null)
						{
							GlobalLog.Error("[EnterIncursionTask] Incursion Portal object is null.");
							IncursionEx.CachedPortal = null;
							return true;
						}
						Npc realalva = IncursionEx.Alva;
						if ((NetworkObject)(object)realalva == (NetworkObject)null)
						{
							GlobalLog.Error("[EnterIncursionTask] failed to get Alva near this timePortal.");
							IncursionEx.CachedPortal = null;
							return true;
						}
						IncursionEx.CachedAlva = new CachedObject((NetworkObject)(object)realalva);
						CachedObject alva = IncursionEx.CachedAlva;
						NetworkObject alvaObj = alva.Object;
						if (((NetworkObject)timePortal2).IsTargetable)
						{
							if (IncursionSettings.Instance.PortalBeforeIncursion && area.IsMap && !AnyPortalsNearby)
							{
								WorldPosition distantPos2 = WorldPosition.FindPathablePositionAtDistance(30, 35, 5);
								if (!(distantPos2 != null) || !distantPos2.PathExists)
								{
									await PlayerAction.CreateTownPortal();
								}
								else
								{
									await Move.AtOnce(distantPos2, "away from Time Portal", 10);
									await PlayerAction.CreateTownPortal();
								}
							}
							if (ErrorManager.GetErrorCount("EnterIncursion") < 5)
							{
								timePortal2 = TimePortal;
								if ((NetworkObject)(object)timePortal2 == (NetworkObject)null)
								{
									GlobalLog.Error("[EnterIncursionTask] Incursion Portal object is null2.");
									IncursionEx.CachedPortal = null;
									return true;
								}
								if (await PlayerAction.TakeTransition(timePortal2))
								{
									CombatAreaCache.IsInIncursion = true;
									SetRoomSettings();
								}
								else
								{
									ErrorManager.ReportError("EnterIncursion");
									await Wait.SleepSafe(500);
									WorldPosition distantPos = WorldPosition.FindPathablePositionAtDistance(10, 14, 3);
									if (distantPos != null && distantPos.PathExists)
									{
										await Move.AtOnce(distantPos, "away from Time Portal2", 8);
									}
								}
								return true;
							}
							GlobalLog.Error("[EnterIncursionTask] Failed to enter Time Portal 5 times.");
							IncursionEx.CachedPortal = null;
							IncursionEx.BlacklistedTimeportalPositions.Add(((NetworkObject)timePortal2).Position);
							return false;
						}
						if (!alvaObj.HasNpcFloatingIcon)
						{
							if (IncursionSettings.Instance.AlvaChecked || !((IAuthored)BotManager.Current).Name.Equals("MapBotEx"))
							{
								if (stopwatch_0.ElapsedMilliseconds > 2500L)
								{
									if (await alvaObj.AsTownNpc().Converse("Enter Incursion"))
									{
										stopwatch_0.Restart();
										await Coroutines.CloseBlockingWindows();
										return true;
									}
									GlobalLog.Error("[EnterIncursionTask] failed to request Enter Incursion to Alva.");
									IncursionEx.CachedPortal = null;
									IncursionEx.BlacklistedTimeportalPositions.Add(((NetworkObject)timePortal2).Position);
								}
								if (((NetworkObject)timePortal2.Fresh<AreaTransition>()).IsTargetable)
								{
									return true;
								}
								return true;
							}
							return await alvaObj.AsTownNpc().Converse("Invite to Hideout");
						}
						if (await PlayerAction.Interact(alvaObj))
						{
							await Wait.Sleep(200);
							await Coroutines.CloseBlockingWindows();
						}
						return true;
					}
					if (!pos.TryCome())
					{
						GlobalLog.Error($"[EnterIncursionTask] Fail to move to {pos}. Incursion Portal is unwalkable.");
						IncursionEx.CachedPortal = null;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (!CombatAreaCache.IsInIncursion || !LokiPoe.IsInGame)
		{
			return;
		}
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		if ((currentArea.IsMap || currentArea.IsOverworldArea) && (NetworkObject)(object)InactiveTimePortal != (NetworkObject)null)
		{
			CombatAreaCache.IsInIncursion = false;
			IncursionEx.CachedPortal = null;
			CombatAreaCache.Current.Storage["IncursionData"] = null;
			if (IncursionSettings.Instance.LeaveAfterIncursion)
			{
				FinishGridning();
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (!(id == "area_changed_event") && !(id == "player_resurrected_event"))
		{
			return (MessageResult)1;
		}
		CombatAreaCache.IsInIncursion = false;
		IncursionEx.CachedPortal = null;
		CombatAreaCache.Current.Storage["IncursionData"] = null;
		IncursionEx.BlacklistedTimeportalPositions.Clear();
		return (MessageResult)0;
	}

	private static void SetRoomSettings()
	{
		DatIncursionRoomsWrapper datIncursionRoomsWrapper_0 = Incursion.CurrentIncursionRoom;
		GlobalLog.Info($"[IncursionEx] Current room: {datIncursionRoomsWrapper_0.Name} (Tier {datIncursionRoomsWrapper_0.Tier})");
		RoomEntry roomEntry = IncursionSettings.Instance.IncursionRooms.Find((RoomEntry r) => r.IdEquals(datIncursionRoomsWrapper_0.AreaId));
		if (roomEntry == null)
		{
			IncursionEx.CurrentRoomSettings = null;
			return;
		}
		IncursionEx.CurrentRoomSettings = roomEntry;
		GlobalLog.Info($"[IncursionEx] Prioritize: {roomEntry.PriorityAction}");
		GlobalLog.Info($"[IncursionEx] Never change: {roomEntry.NoChange}");
		GlobalLog.Info($"[IncursionEx] Never upgrade: {roomEntry.NoUpgrade}");
	}

	private static void FinishGridning()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		IBot current = BotManager.Current;
		if (!((IAuthored)current).Name.Contains("QuestBotEx"))
		{
			return;
		}
		Message val = new Message("QB_get_current_quest", (object)null);
		if ((int)((IMessageHandler)current).Message(val) > 0)
		{
			GlobalLog.Debug("[EnterIncursionTask] \"QB_get_current_quest\" message was not processed.");
			return;
		}
		string output = val.GetOutput<string>(0);
		if (!(output != "Grinding"))
		{
			val = new Message("QB_finish_grinding", (object)null);
			if ((int)((IMessageHandler)current).Message(val) > 0)
			{
				GlobalLog.Debug("[EnterIncursionTask] \"QB_finish_grinding\" message was not processed.");
			}
		}
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	static EnterIncursionTask()
	{
		stopwatch_0 = Stopwatch.StartNew();
	}
}
