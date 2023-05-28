using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.IncursionEx;

public class ItemizeTempleTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public string Name => "ItemizeTempleTask";

	public string Description => "Task that enters The Temple of Atzoatl.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsHideoutArea)
		{
			Npc hoAlva2 = IncursionEx.Alva;
			if ((NetworkObject)(object)hoAlva2 != (NetworkObject)null && (((NetworkObject)hoAlva2).HasNpcFloatingIcon || !IncursionSettings.Instance.AlvaChecked))
			{
				await Coroutines.CloseBlockingWindows();
				if (await PlayerAction.Interact((NetworkObject)(object)hoAlva2))
				{
					await Wait.Sleep(500);
					IncursionSettings.Instance.AlvaChecked = true;
					await Coroutines.CloseBlockingWindows();
					return true;
				}
			}
		}
		if (Incursion.IncursionsRemaining == 0)
		{
			GlobalLog.Warn("[ItemizeTempleTask] Incursion Temple aviable, now going to take chronicle.");
			if (area.IsHideoutArea)
			{
				Npc hoAlva = IncursionEx.Alva;
				if ((NetworkObject)(object)hoAlva == (NetworkObject)null)
				{
					GlobalLog.Error("We need to itemize Temple of Altzoatl, but there is no Alva in hideout. Stopping the bot.");
					BotManager.Stop(new StopReasonData("no_alva_in_hideout", "We need to itemize Temple of Altzoatl, but there is no Alva in hideout. Stopping the bot.", (object)null), false);
					return true;
				}
				IncursionEx.CachedAlva = new CachedObject((NetworkObject)(object)hoAlva);
				CachedObject hoCachedAlva = IncursionEx.CachedAlva;
				WalkablePosition hoPos = hoCachedAlva.Position;
				if (hoPos.Distance <= 20 && !(hoPos.PathDistance > 20f))
				{
					if (((NetworkObject)hoAlva).HasNpcFloatingIcon)
					{
						await Coroutines.CloseBlockingWindows();
						if (await PlayerAction.Interact((NetworkObject)(object)hoAlva))
						{
							await Wait.Sleep(500);
							return true;
						}
					}
					await Coroutines.CloseBlockingWindows();
					if (await ((NetworkObject)(object)hoAlva).AsTownNpc().Converse("Take Temple Chronicle"))
					{
						await Wait.Sleep(500);
						await Coroutines.CloseBlockingWindows();
					}
					return true;
				}
				if (!hoPos.TryCome())
				{
					GlobalLog.Error($"[ItemizeTempleTask] Fail to move to {hoPos}. Alva in Hideout is unwalkable.");
					BotManager.Stop(new StopReasonData("alva_unwalkable", $"Fail to move to {hoPos}. Alva in Hideout is unwalkable.", (object)null), false);
					return false;
				}
				return true;
			}
			await PlayerAction.GoToHideout();
			return true;
		}
		return false;
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

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}
}
