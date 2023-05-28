using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.MapBotEx.Tasks;

public class DeviceAreaTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static Portal ClosestActiveMapPortal
	{
		get
		{
			Portal result = ((IEnumerable)ObjectManager.Objects).Closest<Portal>((Func<Portal, bool>)((Portal p) => ((NetworkObject)p).IsTargetable && p.LeadsTo((DatWorldAreaWrapper a) => a.IsMap || a.Name == "The Maven's Crucible")));
			if (GeneralSettings.SimulacrumsEnabled)
			{
				GlobalLog.Warn("Simulacrums enabled!");
				result = ((IEnumerable)ObjectManager.Objects).Closest<Portal>((Func<Portal, bool>)((Portal p) => ((NetworkObject)p).IsTargetable && p.LeadsTo((DatWorldAreaWrapper a) => a.IsMap || a.Name == "The Maven's Crucible" || a.Id.Contains("Affliction"))));
			}
			return result;
		}
	}

	public string Name => "DeviceAreaTask";

	public string Description => "Task for traveling through area containing Map Device.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsMap || area.IsMapTrialArea)
		{
			return false;
		}
		if (!SkillsUi.IsOpened && !AtlasSkillsUi.IsOpened)
		{
			DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("a11q1");
			int? mapDeviceQuestState = ((currentQuestStateAccurate == null) ? null : new int?(currentQuestStateAccurate.Id));
			int num;
			if (area.IsMyHideoutArea)
			{
				num = 0;
			}
			else
			{
				int? num2 = mapDeviceQuestState;
				num = (((num2.GetValueOrDefault() == 0) & num2.HasValue) ? 1 : 0);
			}
			if (num == 0)
			{
				int num3;
				if (area.Name != "Karui Shores")
				{
					int? num2 = mapDeviceQuestState;
					num3 = ((!((num2.GetValueOrDefault() == 0) & num2.HasValue)) ? 1 : 0);
				}
				else
				{
					num3 = 0;
				}
				if (num3 == 0)
				{
					if (!ExtensionsSettings.Instance.UseFollower)
					{
						if (InstanceInfo.PartyMembers.Any())
						{
							await MapBotEx.SendChatMsg("/kick " + ((NetworkObject)LokiPoe.Me).Name);
						}
					}
					else
					{
						List<PartyMember> nonFollowerMembers = InstanceInfo.PartyMembers.Where((PartyMember m) => ExtensionsSettings.Instance.Followers.All((FollowerEntry e) => !e.Name.EqualsIgnorecase(m.PlayerEntry.Name)) && m.PlayerEntry.Name != ((NetworkObject)LokiPoe.Me).Name).ToList();
						if (nonFollowerMembers.Any() && (int)InstanceInfo.PartyStatus == 0)
						{
							foreach (PartyMember partyMember_0 in nonFollowerMembers)
							{
								await MapBotEx.SendChatMsg("/kick " + partyMember_0.PlayerEntry.Name);
								await Wait.For(() => InstanceInfo.PartyMembers.Any((PartyMember a) => a.PlayerEntry.Name == partyMember_0.PlayerEntry.Name), "kick party member: " + partyMember_0.PlayerEntry.Name, 2000);
							}
						}
					}
					await EnterMapPortal();
					return true;
				}
				GlobalLog.Debug("[DeviceAreaTask] We need to open map in Karui Shores");
				await Travel.To(World.Act11.KaruiShores);
				return true;
			}
			await PlayerAction.GoToHideout();
			return false;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (!(id == "player_resurrected_event"))
		{
			if (id == "area_changed_event")
			{
				DatWorldAreaWrapper input = message.GetInput<DatWorldAreaWrapper>(3);
				if (input.IsMap)
				{
					return (MessageResult)0;
				}
				DatWorldAreaWrapper input2 = message.GetInput<DatWorldAreaWrapper>(2);
				if (input2 != null && !input2.IsMap && input.IsMapRoom)
				{
					return (MessageResult)0;
				}
			}
			return (MessageResult)1;
		}
		return (MessageResult)0;
	}

	private static async Task EnterMapPortal()
	{
		Portal portal = ClosestActiveMapPortal;
		if ((NetworkObject)(object)portal == (NetworkObject)null)
		{
			GlobalLog.Error("[DeviceAreaTask] Fail to find any active map portal.");
			if (GeneralSettings.Instance.IsOnRun)
			{
				GeneralSettings.Instance.IsOnRun = false;
				Statistics.Instance.OnMapFinish(failed: true);
			}
		}
		else if (!(await PlayerAction.TakePortal(portal)))
		{
			ErrorManager.ReportError();
		}
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
