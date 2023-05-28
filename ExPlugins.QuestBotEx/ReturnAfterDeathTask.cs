using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx;

public class ReturnAfterDeathTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private CachedObject cachedObject_0;

	public string Name => "ReturnAfterDeathTask";

	public string Description => "Task for taking closest local transition after death.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!(cachedObject_0 == null))
		{
			WalkablePosition pos = cachedObject_0.Position;
			if (pos.IsFar)
			{
				if (!pos.TryCome())
				{
					GlobalLog.Debug("[ReturnAfterDeathTask] Transition is unwalkable. Skipping this task.");
					cachedObject_0 = null;
				}
				return true;
			}
			AreaTransition transitionObj = (AreaTransition)cachedObject_0.Object;
			if (!(await PlayerAction.TakeTransition(transitionObj)))
			{
				ErrorManager.ReportError();
				return true;
			}
			cachedObject_0 = null;
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "player_resurrected_event")
		{
			cachedObject_0 = null;
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			string string_0 = currentArea.Id;
			if (currentArea.IsTown || SkipThisArea(string_0))
			{
				GlobalLog.Debug("[ReturnAfterDeathTask] Skipping this task because area is " + currentArea.Name + ".");
				return (MessageResult)0;
			}
			AreaTransition val = ((!(string_0 == World.Act9.RottingCore.Id)) ? ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => (int)a.TransitionType == 1 && !((NetworkObject)a).Metadata.Contains("IncursionPortal") && !((NetworkObject)a).Metadata.Contains("SynthesisPortal") && ((NetworkObject)a).Distance <= (float)MaxDistanceByArea(string_0))) : (from a in ObjectManager.Objects.Where<AreaTransition>(ValidRottingCoreTransition)
				orderby ((NetworkObject)a).Metadata == "Metadata/QuestObjects/Act9/HarvestFinalBossTransition" descending, ((NetworkObject)a).Distance
				select a).FirstOrDefault());
			if (!((NetworkObject)(object)val == (NetworkObject)null))
			{
				cachedObject_0 = new CachedObject((NetworkObject)(object)val);
				GlobalLog.Debug($"[ReturnAfterDeathTask] Detected local transition {cachedObject_0.Position}");
				return (MessageResult)0;
			}
			GlobalLog.Debug("[ReturnAfterDeathTask] There is no local area transition nearby. Skipping this task.");
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static bool SkipThisArea(string id)
	{
		return id == World.Act4.DaressoDream.Id || id == World.Act4.BellyOfBeast2.Id || id == World.Act4.Harvest.Id || id == World.Act9.Refinery.Id;
	}

	private static bool ValidRottingCoreTransition(AreaTransition t)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return (int)t.TransitionType == 1 && ((NetworkObject)t).IsTargetable && ((NetworkObject)t).Distance <= 50f && !((NetworkObject)t).Metadata.Contains("BellyArenaTransition") && !((NetworkObject)t).Metadata.Contains("IncursionPortal");
	}

	private static int MaxDistanceByArea(string areaId)
	{
		if (areaId == World.Act9.Descent.Id)
		{
			return 100;
		}
		return 50;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void Tick()
	{
	}
}
