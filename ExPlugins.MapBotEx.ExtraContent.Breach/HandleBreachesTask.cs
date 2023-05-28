using System;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.MapBotEx.ExtraContent.Breach;

public class HandleBreachesTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	internal static readonly AreaDataManager<BreachData> areaDataManager_0;

	private BreachCache breachCache_0;

	private int int_0;

	private bool bool_0;

	public string Name => "HandleBreachesTask";

	public string Description => "This task will handle interacting with Breaches.";

	public string Author => "Bossland GmbH";

	public string Version => "0.0.1.1";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			if (!bool_0)
			{
				if (!((Actor)LokiPoe.Me).IsDead && !LokiPoe.Me.IsInHideout && !LokiPoe.Me.IsInTown && !LokiPoe.Me.IsInMapRoom)
				{
					if (GeneralSettings.Instance.OpenBreaches)
					{
						Vector2i vector2i_0 = LokiPoe.MyPosition;
						BreachData active = areaDataManager_0.Active;
						if (active != null)
						{
							if (breachCache_0 != null && (!breachCache_0.IsValid || Blacklist.Contains(breachCache_0.Id)))
							{
								breachCache_0 = null;
							}
							if (breachCache_0 == null)
							{
								breachCache_0 = active.Breaches.Where((BreachCache m) => m.IsValid && !Blacklist.Contains(m.Id) && ShouldActivate(m)).OrderBy(delegate(BreachCache m)
								{
									//IL_0001: Unknown result type (might be due to invalid IL or missing references)
									//IL_0006: Unknown result type (might be due to invalid IL or missing references)
									//IL_000b: Unknown result type (might be due to invalid IL or missing references)
									Vector2i position = m.Position;
									return ((Vector2i)(ref position)).Distance(vector2i_0);
								}).FirstOrDefault();
								int_0 = 0;
							}
							if (breachCache_0 != null)
							{
								if (int_0 > 5)
								{
									Blacklist.Add(breachCache_0.Id, TimeSpan.FromHours(1.0), "[HandleBreachesTask::Logic] Unable to move to the Breach.");
									breachCache_0 = null;
									return true;
								}
								if (((Vector2i)(ref vector2i_0)).Distance((Vector2i)breachCache_0.WalkablePosition) > 50)
								{
									await Coroutines.CloseBlockingWindows();
									if (!breachCache_0.WalkablePosition.TryCome())
									{
										GlobalLog.Error($"[HandleBreachesTask::Logic] PlayerMoverManager.MoveTowards({breachCache_0.WalkablePosition}) failed for Breach [{breachCache_0.Id}].");
										int_0++;
										return true;
									}
									int_0 = 0;
									return true;
								}
								await Coroutines.FinishCurrentAction(true);
								Breach breach = breachCache_0.NetworkObject;
								if (!((NetworkObject)(object)breach == (NetworkObject)null))
								{
									if (PlayerMoverManager.MoveTowards((Vector2i)breachCache_0.WalkablePosition, (object)null))
									{
										return true;
									}
									GlobalLog.Error($"[HandleBreachesTask::Logic] PlayerMoverManager.MoveTowards({breachCache_0.WalkablePosition}) failed for Breach [{breachCache_0.Id}].");
									int_0++;
									return true;
								}
								breachCache_0.Activate = false;
								GlobalLog.Error($"[HandleBreachesTask::Logic] The NetworkObject does not exist for the Breach [{breachCache_0.Id}] yet.");
								breachCache_0 = null;
								return true;
							}
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
		return false;
	}

	public void Start()
	{
		bool_0 = false;
		breachCache_0 = null;
		areaDataManager_0.Start();
	}

	public void Tick()
	{
		areaDataManager_0.Tick();
	}

	public void Stop()
	{
		areaDataManager_0.Stop();
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	private bool ShouldActivate(BreachCache breach)
	{
		if (breach.Activate.HasValue)
		{
			return breach.Activate.Value;
		}
		GlobalLog.Debug($"[HandleBreachesTask] The Breach [{breach.Id}] will be activated.");
		breach.Activate = true;
		return false;
	}

	static HandleBreachesTask()
	{
		areaDataManager_0 = new AreaDataManager<BreachData>((Func<uint, BreachData>)((uint hash) => new BreachData(hash)))
		{
			DebugLogging = true
		};
	}
}
