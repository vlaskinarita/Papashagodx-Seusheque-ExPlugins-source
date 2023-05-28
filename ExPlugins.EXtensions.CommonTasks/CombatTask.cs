using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.SimulacrumPluginEx;
using ExPlugins.SqRoutine;

namespace ExPlugins.EXtensions.CommonTasks;

public class CombatTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly int int_0;

	public string Name => "CombatTask (Leash " + int_0 + ")";

	public string Description => "This task executes routine logic for combat.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public CombatTask(int leashRange)
	{
		int_0 = leashRange;
	}

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (area.IsCombatArea)
			{
				if (InstanceInfo.WeaponSet != 1)
				{
					int count = 0;
					while (InstanceInfo.WeaponSet != 1)
					{
						GlobalLog.Warn("[CombatTask] Now switching weaponset to primary");
						if (((Actor)LokiPoe.Me).IsDead || count >= 10)
						{
							break;
						}
						ProcessHookManager.ClearAllKeyStates();
						Input.SimulateKeyEvent(Keys.X, true, false, true, Keys.None);
						await Coroutine.Sleep(15);
						count++;
					}
					ProcessHookManager.ClearAllKeyStates();
					return true;
				}
				if (((Actor)LokiPoe.Me).HasAura("Grace Period"))
				{
					GlobalLog.Debug("[CancelGracePeriod] Grace period detected! Moving a bit.");
					Vector2i myPos = LokiPoe.MyPosition;
					int rand = LokiPoe.Random.Next(-25, 25);
					WalkablePosition walkable = new WalkablePosition("random pos", new Vector2i(myPos.X + rand, myPos.Y + rand), 5);
					if (area.Id.Contains("Affliction"))
					{
						walkable = global::ExPlugins.SimulacrumPluginEx.SimulacrumPluginEx.GetCoords(area.Name, findStash: true);
					}
					bool secondarySetExists = InventoryUi.InventoryControl_SecondaryMainHand.Inventory.Items.Any() || InventoryUi.InventoryControl_SecondaryOffHand.Inventory.Items.Any();
					if (walkable.PathExists)
					{
						bool res1 = walkable.TryCome();
						if (area.Id.Contains("Affliction") && global::ExPlugins.SimulacrumPluginEx.SimulacrumPluginEx.CurrentWave < 1)
						{
							res1 = await walkable.TryComeAtOnce();
						}
						if (!res1)
						{
							GlobalLog.Error($"Move towards {walkable} failed!");
							return false;
						}
						GlobalLog.Debug($"Move towards {walkable} success!");
					}
					if (SqRoutineSettings.Instance.SwitchWeapons && secondarySetExists)
					{
						GlobalLog.Warn("[CombatTask] Switching weaponset to secondary");
						ProcessHookManager.ClearAllKeyStates();
						Input.SimulateKeyEvent(Keys.X, true, false, true, Keys.None);
						await Coroutine.Sleep(15);
					}
					return true;
				}
				if (area.Name == "Seething Chyme")
				{
					Aura hungerCorrosion = Enumerable.FirstOrDefault(((Actor)LokiPoe.Me).Auras, (Aura a) => a.InternalName == "infinite_hunger_corrosion");
					if (hungerCorrosion != null && hungerCorrosion.Stacks > 50)
					{
						GlobalLog.Error($"[{Name}] Seething Chyme: Corrosion stacks: {hungerCorrosion.Stacks}. Relogging.");
						EscapeState.LogoutToCharacterSelection();
						return true;
					}
				}
				IRoutine routine = RoutineManager.Current;
				((IMessageHandler)routine).Message(new Message("SetLeash", (object)this, new object[1] { int_0 }));
				return (int)(await ((ILogicProvider)routine).Logic(new Logic("hook_combat", (object)this))) == 0;
			}
			return false;
		}
		return false;
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

	public void Tick()
	{
	}

	public void Stop()
	{
	}
}
