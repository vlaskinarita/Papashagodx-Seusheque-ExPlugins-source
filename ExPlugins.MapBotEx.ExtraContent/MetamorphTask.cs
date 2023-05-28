using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Tasks;

namespace ExPlugins.MapBotEx.ExtraContent;

public class MetamorphTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static int int_0;

	private static int int_1;

	private static int GetUniqueCount => ObjectManager.GetObjectsByType<Monster>().Count((Monster x) => (int)x.Rarity == 3 && ((NetworkObject)x).Distance < 60f);

	public string Name => "MetamorphTask";

	public string Description => "Use Metamorph Device.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (World.CurrentArea.IsMap)
		{
			if (MetamorphSummonUi.IsOpened)
			{
				if (MapExplorationTask.MapCompleted)
				{
					if (GeneralSettings.Instance.BuildMetamorph)
					{
						if (MetamorphTask.int_0 >= 2)
						{
							return false;
						}
						if (int_1 >= 4)
						{
							return false;
						}
						NetworkObject device2 = ObjectManager.GetObjectByMetadata("Metadata/MiscellaneousObjects/Metamorphosis/MetamorphosisVat");
						ProcessHookManager.ClearAllKeyStates();
						bool flag = device2 == (NetworkObject)null;
						bool flag2 = flag;
						if (flag2)
						{
							flag2 = !(await SummonDevice());
						}
						if (flag2)
						{
							GlobalLog.Error("[MetamorphTask] Failed to summon Metamorph Device");
							return false;
						}
						device2 = ObjectManager.GetObjectByMetadata("Metadata/MiscellaneousObjects/Metamorphosis/MetamorphosisVat");
						if (!(device2 == (NetworkObject)null))
						{
							WalkablePosition devicePos = new WalkablePosition("Metamorph Device", device2.Position);
							if (devicePos.IsFar && !devicePos.TryCome())
							{
								int_1++;
								return true;
							}
							if (await PlayerAction.Interact(device2))
							{
								if (!(await Wait.For(() => MetamorphUi.IsOpened, "Metamorph Device to open")))
								{
									GlobalLog.Error("[MetamorphTask] Timeout while Opening the Metamorph Device.");
									return false;
								}
								await Coroutines.ReactionWait();
								await Coroutines.ReactionWait();
							}
							MetamorphTask.int_0++;
							if (MetamorphUi.CanCraft)
							{
								GlobalLog.Warn("[MetamorphTask] Crafting Metamorphic Boss.");
								int int_0 = GetUniqueCount;
								CraftBossResult res = MetamorphUi.CraftBoss();
								if ((int)res > 0)
								{
									GlobalLog.Error($"[MetamorphTask] CraftBossResult = {res}.");
									return false;
								}
								if (!(await Wait.For(() => GetUniqueCount > int_0, "Metamorph Boss to spawn", 100, 10000)))
								{
									GlobalLog.Error("[MetamorphTask] No Boss Spawned for 10 Sec.");
									return false;
								}
								return true;
							}
							GlobalLog.Warn("[MetamorphTask] Missing Ingredients to Craft Metamorphic Boss.");
							return true;
						}
						GlobalLog.Error("[MetamorphTask] Metamorph Device not found.");
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

	public MessageResult Message(Message message)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (message.Id == "MB_new_map_entered_event")
		{
			int_0 = 0;
			int_1 = 0;
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	private static async Task<bool> SummonDevice()
	{
		if (!MetamorphSummonUi.IsOpened)
		{
			GlobalLog.Error("[MetamorphTask] The Summon Device Button is not Open.");
			return false;
		}
		if (MetamorphSummonUi.IsActive)
		{
			if ((int)MetamorphSummonUi.SummonDevice() == 0)
			{
				if (await Wait.For(() => ObjectManager.GetObjectByMetadata("Metadata/MiscellaneousObjects/Metamorphosis/MetamorphosisVat") != (NetworkObject)null, "Metamorph Device to Appear", 100, 5000))
				{
					return true;
				}
				GlobalLog.Error("[MetamorphTask] Timeout while waiting for Metamorph Device.");
			}
			return false;
		}
		GlobalLog.Error("[MetamorphTask] The Summon Device Button is not Enable.");
		return false;
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
}
