using System;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.EssencePluginEx;

public class HandleEssenceTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	public static readonly AreaDataManager<MonolithData> MonolithDataManager;

	private static MonolithCache monolithCache_0;

	private int int_0;

	private int int_1;

	private int int_2;

	private bool bool_0;

	private static readonly EssencePluginExSettings Config;

	public string Name => "HandleEssenceTask";

	public string Description => "This task will handle interacting with EssencePluginEx.";

	public string Author => "Bossland GmbH";

	public string Version => "0.0.1.1";

	public async Task<bool> Run()
	{
		if (!bool_0)
		{
			if (LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead && !LokiPoe.Me.IsInHideout && !LokiPoe.Me.IsInTown)
			{
				Vector2i ExQxYmkaPZ = LokiPoe.MyPosition;
				MonolithData active = MonolithDataManager.Active;
				if (monolithCache_0 != null)
				{
					monolithCache_0.Validate();
					if (!monolithCache_0.IsValid || Blacklist.Contains(monolithCache_0.Id))
					{
						monolithCache_0 = null;
					}
				}
				if (monolithCache_0 == null)
				{
					monolithCache_0 = active.Monoliths.Where((MonolithCache m) => m.IsValid && !Blacklist.Contains(m.Id) && ShouldActivate(m)).OrderBy(delegate(MonolithCache m)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						//IL_0006: Unknown result type (might be due to invalid IL or missing references)
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						Vector2i position = m.Position;
						return ((Vector2i)(ref position)).Distance(ExQxYmkaPZ);
					}).FirstOrDefault();
					int_2 = 0;
					int_1 = 0;
					int_0 = 0;
				}
				if (monolithCache_0 != null)
				{
					if (int_2 <= 5)
					{
						if (((Vector2i)(ref ExQxYmkaPZ)).Distance(monolithCache_0.WalkablePosition) > 30)
						{
							await Coroutines.CloseBlockingWindows();
							if (!PlayerMoverManager.MoveTowards(monolithCache_0.WalkablePosition, (object)null))
							{
								GlobalLog.Error($"[HandleEssenceTask::Logic] PlayerMoverManager.MoveTowards({monolithCache_0.WalkablePosition}) failed for Monolith [{monolithCache_0.Id}].");
								int_2++;
								return true;
							}
							int_2 = 0;
							return true;
						}
						await Coroutines.FinishCurrentAction(true);
						if (int_0 <= 10)
						{
							if (int_1 <= 5)
							{
								Monolith monolith = monolithCache_0.NetworkObject;
								if (!((NetworkObject)(object)monolith == (NetworkObject)null))
								{
									bool flag = !monolith.IsCorrupted && monolithCache_0.Corrupt;
									bool flag2 = flag;
									if (flag2)
									{
										flag2 = !(await CorruptMonolith(monolithCache_0));
									}
									if (flag2)
									{
										return true;
									}
									GlobalLog.Info($"[HandleEssenceTask::Logic] The Monolith [{((NetworkObject)monolith).Id}] is at open phase [{monolith.OpenPhase}].");
									int_0++;
									await Coroutines.CloseBlockingWindows();
									if (!((NetworkObject)monolith).IsTargetable || !monolithCache_0.IsValid)
									{
										monolithCache_0 = null;
										return true;
									}
									if (!(await Coroutines.InteractWith((NetworkObject)(object)monolith, false)))
									{
										GlobalLog.Error($"[HandleEssenceTask::Logic] Interact failed for the Monolith [{((NetworkObject)monolith).Id}].");
										int_1++;
										return true;
									}
									int_1 = 0;
									return true;
								}
								GlobalLog.Error($"[HandleEssenceTask::Logic] The NetworkObject does not exist for the Monolith [{monolithCache_0.Id}] yet.");
								int_1++;
								return true;
							}
							Blacklist.Add(monolithCache_0.Id, TimeSpan.FromHours(1.0), "[HandleEssenceTask::Logic] Unable to interact with the Monolith.");
							monolithCache_0 = null;
							return true;
						}
						Blacklist.Add(monolithCache_0.Id, TimeSpan.FromHours(1.0), "[HandleEssenceTask::Logic] Unexpected Monolith interaction result.");
						monolithCache_0 = null;
						return true;
					}
					Blacklist.Add(monolithCache_0.Id, TimeSpan.FromHours(1.0), "[HandleEssenceTask::Logic] Unable to move to the Monolith.");
					monolithCache_0 = null;
					return true;
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
		monolithCache_0 = null;
		MonolithDataManager.Start();
	}

	public void Tick()
	{
		MonolithDataManager.Tick();
	}

	public void Stop()
	{
		MonolithDataManager.Stop();
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	private static bool ShouldActivate(MonolithCache monolith)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		if (!monolith.Activate.HasValue)
		{
			int count = monolith.Essences.Count;
			if (count < Config.MinEssence)
			{
				GlobalLog.Error($"[HandleEssenceTask::ShouldActivate] [{monolith.WalkablePosition}] will not be activated because it's essence count [{count}] is too small [{Config.MinEssence}].");
				monolith.Activate = false;
			}
			else
			{
				GlobalLog.Warn($"[HandleEssenceTask::ShouldActivate] [{monolith.WalkablePosition}] will be activated because it's essence count [{count}] is more than [{Config.MinEssence}].");
				monolith.Activate = true;
			}
			foreach (DatBaseItemTypeWrapper datBaseItemTypeWrapper_0 in monolith.Essences)
			{
				if (Config.SpecificEssenceRules.Any((EssencePluginExSettings.EssenceEntry e) => datBaseItemTypeWrapper_0.Name.Contains(e.Tier) && datBaseItemTypeWrapper_0.Name.Contains(e.Type) && e.Open))
				{
					GlobalLog.Warn($"[HandleEssenceTask::ShouldActivate] [{monolith.WalkablePosition}] will not be activated because it contains whitelisted essence: {datBaseItemTypeWrapper_0.Name}");
					monolith.Activate = true;
				}
				if (Config.SpecificEssenceRules.Any((EssencePluginExSettings.EssenceEntry e) => datBaseItemTypeWrapper_0.Name.Contains(e.Tier) && datBaseItemTypeWrapper_0.Name.Contains(e.Type) && e.Corrupt))
				{
					GlobalLog.Warn($"[HandleEssenceTask::ShouldCorrupt] [{monolith.WalkablePosition}] will not be corrupted because it contains whitelisted essence: {datBaseItemTypeWrapper_0.Name}");
					monolith.Corrupt = true;
				}
			}
			if (Config.EssencesToUseRemnant != -1 && count >= Config.EssencesToUseRemnant)
			{
				GlobalLog.Warn($"[HandleEssenceTask::ShouldCorrupt] [{monolith.WalkablePosition}] will be corrupted because it has {count} essences [{Config.EssencesToUseRemnant}].");
				monolith.Corrupt = true;
			}
			if (Config.MaxEssence != -1 && count > Config.MaxEssence)
			{
				GlobalLog.Error($"[HandleEssenceTask::ShouldActivate] [{monolith.WalkablePosition}] will not be activated because it's essence count [{count}] is too large [{Config.MaxEssence}].");
				monolith.Activate = false;
				return false;
			}
			return monolith.Activate.Value;
		}
		return monolith.Activate.Value;
	}

	private static async Task<bool> CorruptMonolith(MonolithCache mono)
	{
		if (!mono.NetworkObject.IsCorrupted)
		{
			Item remnant = Inventories.InventoryItems.FirstOrDefault((Item i) => i.Metadata.Contains("CurrencyCorruptMonolith"));
			if ((RemoteMemoryObject)(object)remnant == (RemoteMemoryObject)null)
			{
				GlobalLog.Error($"[HandleEssenceTask] We need to corrupt {mono.WalkablePosition} but no Remnants found in inventory. Skipping the task");
				monolithCache_0.Corrupt = false;
				return true;
			}
			if (!(await Inventories.OpenInventory()))
			{
				return false;
			}
			await InventoryUi.InventoryControl_Main.PickItemToCursor(remnant.LocationTopLeft, rightClick: true);
			await Wait.SleepSafe(15);
			MouseManager.SetMousePos((string)null, mono.WalkablePosition, true);
			await Wait.SleepSafe(15);
			MouseManager.ClickLMB(0, 0);
			return true;
		}
		return true;
	}

	static HandleEssenceTask()
	{
		MonolithDataManager = new AreaDataManager<MonolithData>((Func<uint, MonolithData>)((uint hash) => new MonolithData(hash)))
		{
			DebugLogging = true
		};
		Config = EssencePluginExSettings.Instance;
	}
}
