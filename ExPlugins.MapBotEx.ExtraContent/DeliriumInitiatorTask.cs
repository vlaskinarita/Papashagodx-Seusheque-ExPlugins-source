using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Components;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.ExtraContent;

public class DeliriumInitiatorTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static bool bool_0;

	private static CachedObject cachedObject_0;

	private static uint uint_0;

	private static readonly Interval interval_0;

	private static int int_0;

	public string Name => "DeliriumInitiatorTask";

	public string Description => "Task that start delirium.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!LokiPoe.IsInGame)
		{
			return false;
		}
		if (GeneralSettings.Instance.ActivateDelirium)
		{
			if (!LokiPoe.CurrentWorldArea.IsTown && !LokiPoe.CurrentWorldArea.IsHideoutArea)
			{
				if (bool_0)
				{
					return false;
				}
				if (int_0 <= 3)
				{
					if (DeliriumUi.IsDeliriumActive)
					{
						GlobalLog.Warn("Delirium Active!");
						bool_0 = true;
						cachedObject_0 = null;
						return false;
					}
					if (!(cachedObject_0 == null))
					{
						if (cachedObject_0.Position.Distance > 10)
						{
							TriggerableBlockage door = ((IEnumerable)ObjectManager.Objects).Closest<TriggerableBlockage>((Func<TriggerableBlockage, bool>)IsClosedDoor);
							if ((NetworkObject)(object)door != (NetworkObject)null)
							{
								return false;
							}
							if (!cachedObject_0.Position.TryCome())
							{
								cachedObject_0 = null;
							}
							return true;
						}
						if (!(cachedObject_0.Object == (NetworkObject)null) && cachedObject_0.Object.IsValid)
						{
							Vector2i position = ((NetworkObject)LokiPoe.Me).Position;
							WalkablePosition pos = new WalkablePosition("Delirium Starter", ((Vector2i)(ref position)).GetPointAtDistanceAfterEnd((Vector2i)cachedObject_0.Position, 5f));
							await pos.TryComeAtOnce(4);
							if (!(await Wait.For(() => DeliriumUi.IsDeliriumActive, "delirium activation")))
							{
								GlobalLog.Warn("Delirium Not Active after 3 seconds");
								int_0++;
								return true;
							}
							GlobalLog.Warn("Delirium Active!");
							bool_0 = true;
							cachedObject_0 = null;
							return false;
						}
						bool_0 = true;
						cachedObject_0 = null;
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void Tick()
	{
		if (!LokiPoe.CurrentWorldArea.IsTown && !LokiPoe.CurrentWorldArea.IsHideoutArea && !bool_0 && interval_0.Elapsed)
		{
			DeliriumScan();
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (id == "area_changed_event")
		{
			if (!LokiPoe.CurrentWorldArea.IsTown && !LokiPoe.CurrentWorldArea.IsHideoutArea)
			{
				if (LocalData.AreaHash != uint_0)
				{
					int_0 = 0;
					cachedObject_0 = null;
					bool_0 = false;
					return (MessageResult)0;
				}
				return (MessageResult)0;
			}
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static void DeliriumScan()
	{
		if (LocalData.AreaHash == uint_0 || !(cachedObject_0 == null))
		{
			return;
		}
		NetworkObject val = ObjectManager.Objects.FirstOrDefault((NetworkObject x) => x.Metadata == "Metadata/MiscellaneousObjects/Affliction/AfflictionInitiator" && !x.Components.StateMachineComponent.StageStates.Any((StageState s) => s.Name.ContainsIgnorecase("interacted") && s.IsActive));
		if (!(val == (NetworkObject)null))
		{
			cachedObject_0 = new CachedObject(val);
			uint_0 = LocalData.AreaHash;
		}
	}

	private static bool IsClosedDoor(TriggerableBlockage d)
	{
		return ((NetworkObject)d).IsTargetable && !d.IsOpened && ((NetworkObject)d).Distance <= 25f && (((NetworkObject)d).Name == "Door" || ((NetworkObject)d).Metadata == "Metadata/MiscellaneousObjects/Smashable" || ((NetworkObject)d).Metadata.Contains("LabyrinthSmashableDoor"));
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

	static DeliriumInitiatorTask()
	{
		interval_0 = new Interval(1000);
	}
}
