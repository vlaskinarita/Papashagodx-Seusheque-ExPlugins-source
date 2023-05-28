using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class ReturnAfterTownrunTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static bool Enabled;

	public string Name => "ReturnAfterTownrunTask";

	public string Description => "Task for returning to overworld area after townrun.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!Enabled || !World.CurrentArea.IsTown)
		{
			return false;
		}
		await StaticPositions.GetCommonPortalSpotByAct().ComeAtOnce();
		PortalObject portalObj = LocalData.TownPortals.Find((PortalObject p) => (RemoteMemoryObject)(object)p != (RemoteMemoryObject)null && p.NetworkObject.IsTargetable && p.OwnerName == ((NetworkObject)LokiPoe.Me).Name);
		if ((RemoteMemoryObject)(object)portalObj == (RemoteMemoryObject)null)
		{
			GlobalLog.Error("[ReturnAfterTownrunTask] There is no portal to enter.");
			Enabled = false;
			return true;
		}
		NetworkObject networkObject = portalObj.NetworkObject;
		Portal portal = (Portal)(object)((networkObject is Portal) ? networkObject : null);
		await ((NetworkObject)(object)portal).WalkablePosition().ComeAtOnce();
		if (await PlayerAction.TakePortal(portal))
		{
			Enabled = false;
			return true;
		}
		ErrorManager.ReportError();
		return true;
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
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
