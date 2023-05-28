using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.AutoPassiveEx;
using ExPlugins.EXtensions;

internal class Class55 : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly AutoPassiveExSettings autoPassiveExSettings_0 = AutoPassiveExSettings.Instance;

	public string Author => "Seusheque";

	public string Description => "A plugin that assigns passives";

	public string Name => "AssignPassivesTask";

	public string Version => "0.0.1";

	public async Task<bool> Run()
	{
		if (AutoPassiveEx.ErrorCount <= 10)
		{
			if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
			{
				if (((Player)LokiPoe.Me).Level >= autoPassiveExSettings_0.DisabledAtLevel)
				{
					return false;
				}
				DatWorldAreaWrapper area = World.CurrentArea;
				if (!autoPassiveExSettings_0.AllocateOnlyInTown || area.IsHideoutArea || area.IsTown)
				{
					if (InstanceInfo.PassiveSkillPointsAvailable > 0 || InstanceInfo.AtlasPassivePointsAvailable > 0)
					{
						List<AutoPassiveExSettings.PassiveEntry> validCharacterPassives = (from p in autoPassiveExSettings_0.CharacterPassives
							where !InstanceInfo.PassiveSkillIds.Contains(p.Id)
							orderby p.Number
							select p).ToList();
						if (InstanceInfo.PassiveSkillPointsAvailable > 0 && validCharacterPassives.Any())
						{
							if (!(await OpenCharacterPassiveUi()))
							{
								return true;
							}
							foreach (AutoPassiveExSettings.PassiveEntry passive2 in validCharacterPassives)
							{
								ChoosePassiveError err2 = SkillsUi.ChoosePassive((int)passive2.Id);
								if ((int)err2 > 0)
								{
									if (AutoPassiveEx.ErrorCount != 0)
									{
										GlobalLog.Error($"[{Name}] ChoosePassive returned {err2} for {passive2.Name}:[{passive2.Id}] №{passive2.Number}.");
									}
									AutoPassiveEx.ErrorCount++;
									break;
								}
								await Wait.SleepSafe(15, 30);
							}
							PassiveAllocationActionError err4 = SkillsUi.ConfirmOperation();
							if ((int)err4 != 0)
							{
								GlobalLog.Error($"[{Name}] ConfirmOperation returned {err4}.");
								AutoPassiveEx.ErrorCount++;
							}
							else
							{
								GlobalLog.Debug("[" + Name + "] All character passives were assigned.");
								AutoPassiveEx.ErrorCount = 0;
								Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
							}
						}
						List<AutoPassiveExSettings.PassiveEntry> validAtlasPassives = (from p in autoPassiveExSettings_0.AtlasPassives
							where !InstanceInfo.AtlasPassiveSkillIds.Contains(p.Id)
							orderby p.Number
							select p).ToList();
						if (InstanceInfo.AtlasPassivePointsAvailable > 0 && validAtlasPassives.Any())
						{
							if (!(await OpenAtlasPassiveUi()))
							{
								return true;
							}
							foreach (AutoPassiveExSettings.PassiveEntry passive in validAtlasPassives)
							{
								ChoosePassiveError err = AtlasSkillsUi.ChoosePassive((int)passive.Id);
								if ((int)err > 0)
								{
									if (AutoPassiveEx.ErrorCount != 0)
									{
										GlobalLog.Error($"[{Name}] ChoosePassive returned {err} for {passive.Name}:[{passive.Id}] №{passive.Number}.");
									}
									AutoPassiveEx.ErrorCount++;
									break;
								}
								await Wait.SleepSafe(15, 30);
							}
							PassiveAllocationActionError err3 = AtlasSkillsUi.ConfirmOperation();
							if ((int)err3 == 0)
							{
								GlobalLog.Debug("[" + Name + "] All atlas passives were assigned.");
								AutoPassiveEx.ErrorCount = 0;
								Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
							}
							else
							{
								GlobalLog.Error($"[{Name}] ConfirmOperation returned {err3}.");
								AutoPassiveEx.ErrorCount++;
							}
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		GlobalLog.Error("[" + Name + "] Error count is more than 10. Something is wrong. Please check if you missed any adjastend passives. Stopping bot now.");
		BotManager.Stop(new StopReasonData("passive_error", "too many errors allocating passives", (object)null), false);
		await Wait.SleepSafe(2000);
		return true;
	}

	private static async Task<bool> OpenCharacterPassiveUi()
	{
		if (!SkillsUi.IsOpened || GlobalWarningDialog.IsPassiveTreeWarningOverlayOpen)
		{
			if (((Actor)LokiPoe.Me).IsDead)
			{
				return false;
			}
			await Coroutines.CloseBlockingWindows();
			ProcessHookManager.ClearAllKeyStates();
			Input.SimulateKeyEvent(Binding.open_passive_skills_panel, true, false, false, Keys.None);
			if (await Wait.For(() => SkillsUi.IsOpened, "passive UI opening", 10, 300))
			{
				if (!GlobalWarningDialog.IsPassiveTreeWarningOverlayOpen)
				{
					return true;
				}
				Input.SimulateKeyEvent(Keys.Escape, true, false, false, Keys.None);
				await Wait.SleepSafe(250);
				return false;
			}
			return false;
		}
		return true;
	}

	private static async Task<bool> OpenAtlasPassiveUi()
	{
		if (!AtlasSkillsUi.IsOpened)
		{
			if (((Actor)LokiPoe.Me).IsDead)
			{
				return false;
			}
			await Coroutines.CloseBlockingWindows();
			ProcessHookManager.ClearAllKeyStates();
			ProcessHookManager.SetKeyState(Keys.ControlKey, short.MinValue, Keys.None);
			await Wait.For(() => ProcessHookManager.IsKeyDown(Keys.ControlKey), "", 10, 300);
			Input.SimulateKeyEvent(Binding.open_atlas_screen, true, false, false, Keys.None);
			return !(await Wait.For(() => AtlasSkillsUi.IsOpened, "atlas passive UI opening", 10, 300));
		}
		return true;
	}

	public void Tick()
	{
	}

	public void Deinitialize()
	{
	}

	public void Initialize()
	{
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
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
}
