using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.BlightPluginEx.Classes;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;
using ExPlugins.SqRoutine;

namespace ExPlugins.BlightPluginEx.Tasks;

public class HandleEventTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	internal enum Enum1
	{

	}

	public static int FinishedOnThisMap;

	public static Stopwatch Comeback;

	public static Stopwatch EncounterSw;

	public static Stopwatch EncounterTimeoutSwSmall;

	public static Stopwatch EncounterTimeoutSwBig;

	public static bool SkipEncounter;

	public static int Failcount;

	private static int int_0;

	private static readonly Interval interval_0;

	public static List<WalkablePosition> SpawnerList;

	public string Name => "HandleEventTask";

	public string Description => "Task that start the Blight encounter.";

	public string Author => "Seusheque";

	public string Version => "2.0";

	public async Task<bool> Run()
	{
		if (LokiPoe.IsInGame)
		{
			DatWorldAreaWrapper area = World.CurrentArea;
			if (!area.IsMap)
			{
				return false;
			}
			if (UpgradeTowerTask.stopwatch_1.ElapsedMilliseconds < 3000L)
			{
				return true;
			}
			if (SkipEncounter)
			{
				EncounterTimeoutSwBig.Reset();
				EncounterTimeoutSwSmall.Reset();
			}
			if (Blight.IsEncounterRunning && BlightUi.IsOpened && !SkipEncounter && BlightPluginEx.CachedBlightCore != null)
			{
				if (Comeback == null)
				{
					Comeback = Stopwatch.StartNew();
				}
				if (EncounterSw == null)
				{
					EncounterSw = Stopwatch.StartNew();
				}
				if (EncounterTimeoutSwSmall == null)
				{
					EncounterTimeoutSwSmall = Stopwatch.StartNew();
				}
				if (EncounterTimeoutSwBig == null)
				{
					EncounterTimeoutSwBig = Stopwatch.StartNew();
				}
				if (EncounterTimeoutSwBig.ElapsedMilliseconds > 180000L && !LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
				{
					GlobalLog.Error(string.Format("[{0}] Timeout! {1} > 180", "HandleEventTask", EncounterTimeoutSwBig.Elapsed.TotalSeconds));
					SkipEncounter = true;
					return true;
				}
				if (EncounterTimeoutSwBig.ElapsedMilliseconds > 80000L && LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
				{
					GlobalLog.Error(string.Format("[{0}] Timeout! {1} > 80", "HandleEventTask", EncounterTimeoutSwBig.Elapsed.TotalSeconds));
					SkipEncounter = true;
					return true;
				}
				if (EncounterTimeoutSwSmall.ElapsedMilliseconds > 40000L)
				{
					GlobalLog.Error(string.Format("[{0}] Timeout! {1} > 30", "HandleEventTask", EncounterTimeoutSwSmall.Elapsed.TotalSeconds));
					SkipEncounter = true;
					return true;
				}
				StuckDetection.Reset();
				if (!BlightUi.IsStartBlightVisible)
				{
					List<MinimapIconWrapper> spawnerList = InstanceInfo.MinimapIcons.Where((MinimapIconWrapper p) => p.MinimapIcon != null && p.MinimapIcon.Name.Contains("BlightPortal")).ToList();
					foreach (MinimapIconWrapper spawner in spawnerList)
					{
						WalkablePosition walkable = new WalkablePosition(spawner.MinimapIcon.Name, spawner.LastSeenPosition);
						if (!SpawnerList.Contains(walkable))
						{
							SpawnerList.Add(walkable);
						}
					}
					return true;
				}
				if (Blight.Resources <= 500 || !ObjectManager.BlightDefensiveTowers.Any((BlightDefensiveTower t) => t.Tier == 0 && ((NetworkObject)t).Distance < (float)Class53.Instance.TowerUpgradeDistance))
				{
					if (Class53.Instance.DebugMode)
					{
						GlobalLog.Debug("[HandleEventTask] Start Blight Visible, Starting.");
					}
					ProcessHookManager.ClearAllKeyStates();
					StartBlightEncounterResult err = BlightUi.StartBlightEncounter();
					if ((int)err == 0)
					{
						await Wait.For(() => Blight.IsEncounterRunning && !BlightUi.IsStartBlightVisible, "encounter to actually start", 50, 2000);
						await BlightPluginEx.CachedBlightCore.Position.TryComeAtOnce(10);
						Comeback.Restart();
						EncounterTimeoutSwSmall.Restart();
						await Wait.SleepSafe(1000, 2500);
					}
					return true;
				}
				return true;
			}
			if (!Blight.IsEncounterFinishedLootAvailable && SkipEncounter)
			{
				if (Blight.IsEncounterFinishedLootAvailable || Blight.IsEncounterCompletedPumpDestroied || !BlightUi.IsOpened)
				{
					return false;
				}
				if (!LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
				{
					if (SpawnerList.Any())
					{
						foreach (WalkablePosition pos in SpawnerList)
						{
							if (!(pos.PathDistance < 20f))
							{
								if (await pos.TryComeAtOnce())
								{
									continue;
								}
								SpawnerList.Remove(pos);
								return true;
							}
							SpawnerList.Remove(pos);
							return true;
						}
					}
				}
				else
				{
					if (await TrackMobLogic.Execute())
					{
						return true;
					}
					if (await CombatAreaCache.Current.Explorer.Execute())
					{
						return true;
					}
				}
			}
			if ((!Blight.IsEncounterRunning || SkipEncounter) && Comeback != null && Comeback.IsRunning)
			{
				string blightString = "";
				if (!LocalData.MapMods.ContainsKey((StatTypeGGG)14763))
				{
					if (LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
					{
						blightString = "Blighted  ";
					}
				}
				else
				{
					blightString = "Blight-ravaged  ";
				}
				FinishedOnThisMap++;
				string report;
				if (BlightUi.RewardsList.All((Reward r) => r.LaneCleared))
				{
					report = $"Blight event succesfully finished in {TimeSpan.FromSeconds(EncounterSw.Elapsed.TotalSeconds):hh\\:mm\\:ss}. [{FinishedOnThisMap}] on {blightString}{area.Name} map. Reward chests: {BlightUi.RewardsList.Sum((Reward r) => r.Count)}:";
					GlobalLog.Info("===========================");
					int counter = 0;
					foreach (Reward reward in from l in BlightUi.RewardsList
						where l.LaneCleared
						select l into r
						orderby r.RewardType
						select r)
					{
						counter++;
						GlobalLog.Info($"Reward{counter}: {reward.RewardType}:{reward.Count}X");
						report += $" {reward.RewardType}:{reward.Count}X;";
					}
					GlobalLog.Info("===========================");
				}
				else
				{
					report = $"Blight event failed after {TimeSpan.FromSeconds(EncounterSw.Elapsed.TotalSeconds):hh\\:mm\\:ss} on {blightString}{area.Name} map.";
				}
				Utility.BroadcastMessage((object)null, "blight_finished", new object[1] { report });
				GlobalLog.Warn("[HandleEventTask] " + report);
				EncounterSw.Stop();
				Comeback.Reset();
			}
			return false;
		}
		return false;
	}

	public static bool IsClosedDoor(TriggerableBlockage d)
	{
		return ((NetworkObject)d).IsTargetable && !d.IsOpened && ((NetworkObject)d).Distance <= 25f && (((NetworkObject)d).Name == "Door" || ((NetworkObject)d).Metadata == "Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch_Once" || ((NetworkObject)d).Metadata == "Metadata/MiscellaneousObjects/Smashable" || ((NetworkObject)d).Metadata.Contains("LabyrinthSmashableDoor"));
	}

	internal static WeightedTower FindTowerToUpgrade(bool findClosest = false)
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		if (ObjectManager.BlightDefensiveTowers.Any())
		{
			double num = (LocalData.MapMods.ContainsKey((StatTypeGGG)10342) ? 1.0 : 2.5);
			ObservableCollection<WeightedTower> observableCollection = new ObservableCollection<WeightedTower>();
			foreach (BlightDefensiveTower item2 in ObjectManager.BlightDefensiveTowers.Where((BlightDefensiveTower t) => !Blacklist.Contains(((NetworkObject)t).Id)))
			{
				if (item2.Tier == 4 || ((((NetworkObject)item2).Metadata.Contains("Buff") || ((NetworkObject)item2).Metadata.Contains("Chill") || ((NetworkObject)item2).Metadata.Contains("Stun")) && item2.Tier == 3))
				{
					continue;
				}
				Vector2i position = ((NetworkObject)item2).Position;
				if (((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) <= Class53.Instance.TowerUpgradeDistance)
				{
					int num2 = 0;
					WalkablePosition position2 = ((NetworkObject)(object)item2).WalkablePosition();
					CachedBlightTower towerCached = new CachedBlightTower(((NetworkObject)item2).Id, position2, item2.Tier, ((NetworkObject)item2).Name, ((NetworkObject)item2).Metadata);
					if (PortalNear((NetworkObject)(object)item2))
					{
						num2 -= 1000;
					}
					int num3 = ((((NetworkObject)item2).Metadata.Contains("Chill") || ((NetworkObject)item2).Metadata.Contains("Stun")) ? 20 : 0);
					int num4 = (((NetworkObject)item2).Metadata.Contains("Buff") ? 50 : 0);
					int num5 = 50 - (int)Math.Round((double)NumberOfFullyUpgradedNonBuffTowersAround(ClosestEmpowerTower((NetworkObject)(object)item2), 52) * 20.0);
					int num6 = item2.Tier * 250;
					position = ((NetworkObject)item2).Position;
					int num7 = (int)Math.Round(1000.0 - (double)((Vector2i)(ref position)).Distance((Vector2i)BlightPluginEx.CachedBlightCore.Position) * 5.0);
					int num8;
					if (BlightUi.IsStartBlightVisible && ClosestSpawnerTo((NetworkObject)(object)item2) != null)
					{
						position = ((NetworkObject)item2).Position;
						num8 = (int)Math.Round((double)(50 - ((Vector2i)(ref position)).Distance((Vector2i)ClosestSpawnerTo((NetworkObject)(object)item2))) / num);
					}
					else
					{
						num8 = 0;
					}
					int num9 = num8;
					int num10 = (int)Math.Round((double)NumberOfDotsNear((NetworkObject)(object)item2, 30f) * 6.0);
					num2 += num3 + num4;
					num2 += num5;
					num2 += num6;
					num2 += num7;
					num2 += num9;
					num2 += num10;
					if (item2.Tier == 1 && NumberOfNonBuffTowersAround((NetworkObject)(object)item2, 80) < 3)
					{
						num2 -= 150;
					}
					if (IsEmpowered(item2) && NumberOfTowersAround((NetworkObject)(object)item2, 60, 1, "Chill", "Stun") >= 1 && NumberOfDpsTowersAround((NetworkObject)(object)item2, 80) == 0)
					{
						num2 += 50;
					}
					WeightedTower item = new WeightedTower(((NetworkObject)item2).Id, item2, towerCached, position2, num2);
					observableCollection.Add(item);
				}
			}
			if (observableCollection.Any())
			{
				observableCollection = new ObservableCollection<WeightedTower>(observableCollection.OrderByDescending((WeightedTower i) => i.TotalWeight));
				WeightedTower weightedTower = observableCollection.FirstOrDefault();
				if (findClosest)
				{
					weightedTower = observableCollection.OrderBy((WeightedTower t) => t.Position.Distance).FirstOrDefault();
				}
				if (weightedTower == null)
				{
					return null;
				}
				float pathDistance = weightedTower.Position.PathDistance;
				if (pathDistance > (float)(Class53.Instance.TowerUpgradeDistance + 25))
				{
					Blacklist.Add(weightedTower.Id, TimeSpan.FromMinutes(1.0), $"path distance {pathDistance} is greater than {Class53.Instance.TowerUpgradeDistance + 25}");
					return null;
				}
				return weightedTower;
			}
			return null;
		}
		return null;
	}

	internal static async Task<Enum1> UpgradeTower(WeightedTower weightedTower)
	{
		if (weightedTower == null)
		{
			return (Enum1)2;
		}
		if (interval_0.Elapsed)
		{
			int_0 = LokiPoe.Random.Next(1, 100);
		}
		List<string> upgradeDps = Class53.Instance.UpgradeOpt;
		List<string> upgradeEmpower = new List<string> { "BuffTower1", "BuffTower2", "BuffTower3" };
		List<string> upgradeChill = new List<string> { "ChillingTower1", "ChillingTower2", "ChillingTower3" };
		List<string> upgradeStun = new List<string> { "StunningTower1", "StunningTower2", "StunningTower3" };
		List<string> randArc = new List<string> { "ShockingTower1", "ShockingTower2", "ShockingTower3", "ArcingTower" };
		CachedBlightTower cachedBlightTower_0 = weightedTower.TowerCached;
		if (cachedBlightTower_0 == null)
		{
			return (Enum1)2;
		}
		BlightDefensiveTower twrObj = weightedTower.TowerCached.Object;
		if (IsUpgradable(cachedBlightTower_0) && !((NetworkObject)(object)twrObj == (NetworkObject)null))
		{
			WalkablePosition pos = weightedTower.Position;
			bool mobsNearPump = NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 60f) != 0;
			bool bossNearPump = NumberOfMobsAround(BlightPluginEx.CachedBlightCore, 80f, boss: true) != 0;
			ProcessHookManager.ClearAllKeyStates();
			if (pos.Distance <= 35)
			{
				List<string> list_0;
				if (LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
				{
					if (!Class53.Instance.BuildEmpowerTowersOnBlightedMaps || IsEmpowered(cachedBlightTower_0) || NumberOfTowersAround(cachedBlightTower_0, 51, 1, excludeEmpowered: true) < 1)
					{
						if (!Class53.Instance.BuildChillTowersBlighted || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 50, 1, "Chill", "hehe haha") != 0 || NumberOfDotsNear((CachedObject)cachedBlightTower_0, 25f) < 5)
						{
							if (!Class53.Instance.BuildStunTowersBlighted || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 50, 1, "Stun", "hehe haha") != 0 || NumberOfDotsNear((CachedObject)cachedBlightTower_0, 15f) < 2)
							{
								if (!Class53.Instance.BuildChillTowersBlighted || !Class53.Instance.BuildStunTowersBlighted || !IsEmpowered(cachedBlightTower_0) || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 35, 1, "Stun", "hehe haha") == 0 || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 65, 1, "Chill", "hehe haha") != 0 || NumberOfDpsTowersAround((CachedObject)ClosestEmpowerTower(cachedBlightTower_0), 49, 1) == 0)
								{
									if (!Class53.Instance.BuildChillTowersBlighted || !Class53.Instance.BuildStunTowersBlighted || !IsEmpowered(cachedBlightTower_0) || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 35, 1, "Chill", "hehe haha") == 0 || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 65, 1, "Stun", "hehe haha") != 0 || NumberOfDpsTowersAround((CachedObject)ClosestEmpowerTower(cachedBlightTower_0), 49, 1) == 0)
									{
										if (int_0 <= 95 || NumberOfDotsNear((CachedObject)cachedBlightTower_0, 25f) < 8)
										{
											list_0 = upgradeDps;
											if (upgradeDps.Contains("Pidor") || BlightPluginEx.CachedBlightCore.Position.Distance(pos) < 55)
											{
												if (Class53.Instance.BuildStunTowersBlighted)
												{
													list_0 = upgradeStun;
												}
												if (Class53.Instance.BuildChillTowersBlighted)
												{
													list_0 = upgradeChill;
												}
												if (Class53.Instance.BuildChillTowersBlighted && Class53.Instance.BuildStunTowersBlighted)
												{
													list_0 = ((int_0 > 50) ? upgradeChill : upgradeStun);
												}
											}
										}
										else
										{
											if (Class53.Instance.DebugMode)
											{
												GlobalLog.Debug(string.Format("[{0}] Rand: {1}. PrefOpt: {2}", "HandleEventTask", int_0, randArc.FirstOrDefault()));
											}
											list_0 = randArc;
										}
									}
									else
									{
										list_0 = upgradeStun;
									}
								}
								else
								{
									list_0 = upgradeChill;
								}
							}
							else
							{
								list_0 = upgradeStun;
							}
						}
						else
						{
							list_0 = upgradeChill;
						}
					}
					else
					{
						list_0 = upgradeEmpower;
					}
				}
				else if (NumberOfDpsTowersAround((CachedObject)cachedBlightTower_0, 90, 1) != 0)
				{
					if (!Class53.Instance.BuildChillTowersRegular || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 125, 1, "Chill", "hehe haha") != 0 || NumberOfDotsNear((CachedObject)cachedBlightTower_0, 20f) < 4)
					{
						if (!Class53.Instance.BuildStunTowersRegular || NumberOfTowersAround((CachedObject)cachedBlightTower_0, 125, 1, "Stun", "hehe haha") != 0 || NumberOfDotsNear((CachedObject)cachedBlightTower_0, 10f) < 2)
						{
							if (NumberOfTowersAround((CachedObject)cachedBlightTower_0, 74, 1, "Buff", "hehe haha") != 0 || NumberOfTowersAround(cachedBlightTower_0, 51, 1, excludeEmpowered: false) < 1 || !Class53.Instance.BuildEmpowerTowersOnRegularMaps)
							{
								list_0 = upgradeDps;
								if (upgradeDps.Contains("Pidor"))
								{
									list_0 = ((int_0 > 50) ? upgradeChill : upgradeStun);
								}
							}
							else
							{
								list_0 = upgradeEmpower;
							}
						}
						else
						{
							list_0 = upgradeStun;
						}
					}
					else
					{
						list_0 = upgradeChill;
					}
				}
				else
				{
					list_0 = upgradeDps;
				}
				await PlayerAction.DisableAlwaysHighlight();
				await Coroutines.CloseBlockingWindows();
				if (((Actor)LokiPoe.Me).HasCurrentAction && ((Actor)LokiPoe.Me).CurrentAction.Skill.Name == "Move")
				{
					ProcessHookManager.ClearAllKeyStates();
					await MethodExtensions.SqUseAt(SkillBarHud.LastBoundMoveSkill, new Vector2i(LokiPoe.MyPosition.X + 2, LokiPoe.MyPosition.Y + 2));
				}
				if (twrObj.Tier != 0 || !((RemoteMemoryObject)(object)twrObj.Ui != (RemoteMemoryObject)null) || twrObj.Ui.Menu == null || twrObj.Ui.Menu.Count != 1)
				{
					if (!((RemoteMemoryObject)(object)twrObj.Ui == (RemoteMemoryObject)null) && twrObj.Ui.Menu != null)
					{
						if (Class53.Instance.DebugMode && twrObj.Tier == 0)
						{
							GlobalLog.Debug("[HandleEventTask] prefOptions type: " + list_0.FirstOrDefault()?.Replace("1", ""));
						}
						int retry2 = 0;
						while (true)
						{
							retry2++;
							if (!twrObj.Ui.Menu.Any((BlightTowerOption x) => x.Id.Contains("MeteorTower")))
							{
								if (twrObj.Ui.Menu.Any((BlightTowerOption x) => x.Id.Contains("ArcingTower")))
								{
									BlightTowerOption option3 = twrObj.Ui.Menu.FirstOrDefault((BlightTowerOption x) => x.Id.Contains("ArcingTower"));
									if ((RemoteMemoryObject)(object)option3 != (RemoteMemoryObject)null)
									{
										if (Class53.Instance.DebugMode)
										{
											GlobalLog.Debug("[HandleEventTask] Using " + option3.Id + " to upgrade");
										}
										twrObj.Upgrade(option3.Id);
									}
								}
								else if (!twrObj.Ui.Menu.Any((BlightTowerOption x) => list_0.Contains(x.Id)))
								{
									if (twrObj.Tier > 0)
									{
										twrObj.Upgrade("");
									}
								}
								else
								{
									BlightTowerOption option2 = twrObj.Ui.Menu.FirstOrDefault((BlightTowerOption x) => list_0.Contains(x.Id));
									if ((RemoteMemoryObject)(object)option2 != (RemoteMemoryObject)null)
									{
										if (Class53.Instance.DebugMode)
										{
											GlobalLog.Debug("[HandleEventTask] Using " + option2.Id + " to upgrade");
										}
										twrObj.Upgrade(option2.Id);
									}
								}
							}
							else
							{
								BlightTowerOption option = twrObj.Ui.Menu.FirstOrDefault((BlightTowerOption x) => x.Id.Contains("MeteorTower"));
								if ((RemoteMemoryObject)(object)option != (RemoteMemoryObject)null)
								{
									if (Class53.Instance.DebugMode)
									{
										GlobalLog.Debug("[HandleEventTask] Using " + option.Id + " to upgrade");
									}
									twrObj.Upgrade(option.Id);
								}
							}
							if (await Wait.For(() => (NetworkObject)(object)cachedBlightTower_0.Object == (NetworkObject)null, "tower to upgrade", 50, 400))
							{
								break;
							}
							if (pos.Distance > 20)
							{
								pos.TryCome();
							}
							if (retry2 < 3)
							{
								continue;
							}
							if (Failcount < 4)
							{
								if (Class53.Instance.DebugMode)
								{
									GlobalLog.Error(string.Format("[{0}] Failed To Upgrade Tower [{1}]", "HandleEventTask", weightedTower.Position));
								}
								if (pos.Distance > 23)
								{
									pos.TryCome();
								}
								Failcount++;
								return (Enum1)0;
							}
							Blacklist.Add(weightedTower.TowerCached.Id, TimeSpan.FromSeconds(15.0), "can't upgrade");
							Failcount = 0;
							Class53.weightedTower_0 = null;
							return (Enum1)0;
						}
						if (!mobsNearPump && !bossNearPump && BlightPluginEx.CachedBlightCore.Position.Distance < 60)
						{
							Comeback.Restart();
						}
						Class53.weightedTower_0 = null;
						return (Enum1)1;
					}
					return (Enum1)2;
				}
				int retry = 0;
				do
				{
					retry++;
					twrObj.Upgrade("");
					if (twrObj.Ui.Menu.Count == 6)
					{
						return (Enum1)1;
					}
				}
				while (retry < 3);
				return (Enum1)0;
			}
			if (!pos.TryCome())
			{
				GlobalLog.Error(string.Format("[{0}] Can't come to {1}!", "HandleEventTask", pos));
				Blacklist.Add(weightedTower.TowerCached.Id, TimeSpan.FromMinutes(1.0), "cant pathfind");
				return (Enum1)2;
			}
			return (Enum1)3;
		}
		return (Enum1)2;
	}

	private static bool PortalNear(NetworkObject obj)
	{
		if (!(obj == (NetworkObject)null))
		{
			return (NetworkObject)(object)((IEnumerable)ObjectManager.Objects).Closest<Portal>((Func<Portal, bool>)delegate(Portal p)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				int result;
				if (p.IsPlayerPortal())
				{
					Vector2i position = ((NetworkObject)p).Position;
					if (((Vector2i)(ref position)).Distance(obj.Position) <= 15)
					{
						result = (p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsTown) ? 1 : 0);
						goto IL_004e;
					}
				}
				result = 0;
				goto IL_004e;
				IL_004e:
				return (byte)result != 0;
			}) != (NetworkObject)null;
		}
		return false;
	}

	private static bool IsUpgradable(CachedBlightTower tower)
	{
		LocalData.MapMods.TryGetValue((StatTypeGGG)10108, out var value);
		value = (100 + value) / 100;
		int resources = Blight.Resources;
		BlightDefensiveTower @object = tower.Object;
		int tier = tower.Tier;
		if (tier == 0 && resources >= 100 * value)
		{
			return true;
		}
		if (!((NetworkObject)(object)@object == (NetworkObject)null) && tower.Position.Distance < 45)
		{
			return tower.Tier < 4 && (RemoteMemoryObject)(object)@object.Ui != (RemoteMemoryObject)null && @object.Ui.Menu != null && @object.Ui.Menu.Any((BlightTowerOption e) => !string.IsNullOrEmpty(e.Name));
		}
		switch (tier)
		{
		case 1:
			if (resources >= 150 * value)
			{
				break;
			}
			goto default;
		case 2:
			if (resources >= 300 * value)
			{
				break;
			}
			goto default;
		case 3:
			if (resources >= 500 * value)
			{
				break;
			}
			goto default;
		default:
			return false;
		}
		return true;
	}

	public static bool IsEmpowered(BlightDefensiveTower obj)
	{
		if (Class53.Instance.BuildEmpowerTowersOnBlightedMaps || !LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
		{
			if (Class53.Instance.BuildEmpowerTowersOnRegularMaps || LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
			{
				if (((Actor)obj).Auras.Any((Aura a) => a.InternalName.Equals("blight_tower_aura_buff_towers")))
				{
					return true;
				}
				return NumberOfTowersAround((NetworkObject)(object)obj, 49, 1, "Buff") != 0;
			}
			return true;
		}
		return true;
	}

	public static bool IsEmpowered(CachedBlightTower obj)
	{
		if (Class53.Instance.BuildEmpowerTowersOnBlightedMaps || !LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
		{
			if (Class53.Instance.BuildEmpowerTowersOnRegularMaps || LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
			{
				if (!((NetworkObject)(object)obj.Object != (NetworkObject)null) || !((Actor)obj.Object).Auras.Any((Aura a) => a.InternalName.Equals("blight_tower_aura_buff_towers")))
				{
					return NumberOfTowersAround((CachedObject)obj, 49, 1, "Buff", "hehe haha") != 0;
				}
				return true;
			}
			return true;
		}
		return true;
	}

	public static int NumberOfTowersAround(NetworkObject obj, int range, int minTier, string metadata, string altMetadata = "hehe haha")
	{
		int num = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && x.Metadata.Contains(metadata));
		int num2 = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && x.Metadata.Contains(altMetadata));
		return num + num2;
	}

	public static int NumberOfTowersAround(CachedObject obj, int range, int minTier, string metadata, string altMetadata = "hehe haha")
	{
		int num = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && x.Metadata.Contains(metadata));
		int num2 = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && x.Metadata.Contains(altMetadata));
		return num + num2;
	}

	public static int NumberOfTowersAround(NetworkObject obj, int range, int minTier)
	{
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && !x.Metadata.Contains("Buff"));
	}

	public static int NumberOfTowersAround(CachedObject obj, int range, int minTier, bool excludeEmpowered = false)
	{
		IEnumerable<CachedBlightTower> source = CombatAreaCache.Current.BlightTowers.Where((CachedBlightTower x) => x.Tier >= minTier && x.Position.Distance(obj.Position) < range && !x.Metadata.Contains("Buff"));
		if (excludeEmpowered)
		{
			source = source.Where((CachedBlightTower t) => !IsEmpowered(t)).ToList();
		}
		return source.Count();
	}

	public static int NumberOfFoundationsAround(NetworkObject obj, int range)
	{
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier == 0 && x.Position.Distance(obj.Position) < range);
	}

	public static int NumberOfFoundationsAround(CachedObject obj, int range)
	{
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier == 0 && x.Position.Distance(obj.Position) < range);
	}

	public static int NumberOfFullyUpgradedNonBuffTowersAround(CachedObject obj, int range)
	{
		int num = CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x != null && x.Tier == 3 && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Chill") || x.Metadata.Contains("Stun")));
		return num + CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x != null && x.Tier == 4 && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Meteor") || x.Metadata.Contains("Arc") || x.Metadata.Contains("Minion")));
	}

	public static int NumberOfNonBuffTowersAround(NetworkObject obj, int range, int tier = 1)
	{
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= tier && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Chill") || x.Metadata.Contains("Stun") || x.Metadata.Contains("Flame") || x.Metadata.Contains("Shock") || x.Metadata.Contains("Meteor") || x.Metadata.Contains("Arc") || x.Metadata.Contains("Minion")));
	}

	public static int NumberOfNonBuffTowersAround(CachedObject obj, int range, int tier = 1)
	{
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= tier && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Chill") || x.Metadata.Contains("Stun") || x.Metadata.Contains("Flame") || x.Metadata.Contains("Shock") || x.Metadata.Contains("Meteor") || x.Metadata.Contains("Arc") || x.Metadata.Contains("Minion")));
	}

	public static int NumberOfDpsTowersAround(NetworkObject obj, int range, int tier = 1)
	{
		if (!(obj == (NetworkObject)null))
		{
			return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= tier && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Flame") || x.Metadata.Contains("Shock") || x.Metadata.Contains("Meteor") || x.Metadata.Contains("Arc") || x.Metadata.Contains("Minion")));
		}
		return 0;
	}

	public static int NumberOfDpsTowersAround(CachedObject obj, int range, int tier = 1)
	{
		if (obj == null)
		{
			return 0;
		}
		return CombatAreaCache.Current.BlightTowers.Count((CachedBlightTower x) => x.Tier >= tier && x.Position.Distance(obj.Position) < range && (x.Metadata.Contains("Flame") || x.Metadata.Contains("Shock") || x.Metadata.Contains("Meteor") || x.Metadata.Contains("Arc") || x.Metadata.Contains("Minion")));
	}

	public static CachedBlightTower ClosestEmpowerTower(CachedBlightTower obj)
	{
		CachedBlightTower cachedBlightTower = CombatAreaCache.Current.BlightTowers.OrderBy((CachedBlightTower t) => t.Position.Distance(obj.Position)).FirstOrDefault((CachedBlightTower t) => t.Metadata.Contains("Buff"));
		return (!(cachedBlightTower == null)) ? cachedBlightTower : obj;
	}

	public static CachedBlightTower ClosestEmpowerTower(NetworkObject obj)
	{
		CachedBlightTower cachedBlightTower = CombatAreaCache.Current.BlightTowers.OrderBy((CachedBlightTower t) => t.Position.Distance(obj.Position)).FirstOrDefault((CachedBlightTower t) => t.Metadata.Contains("Buff"));
		return (cachedBlightTower == null) ? CombatAreaCache.Current.BlightTowers.FirstOrDefault((CachedBlightTower t) => t.Id == obj.Id) : cachedBlightTower;
	}

	public static int NumberOfMobsAround(CachedObject target, float distance, bool boss = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Vector2i vector2i_0 = target.Position.AsVector;
		List<CachedMonster> source = CombatAreaCache.Current.Monsters.ToList();
		int result = source.Count(delegate(CachedMonster m)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (!(m != null) || !IsBlightMob(m))
			{
				goto IL_005e;
			}
			if (!(ClosestSpawnerTo((CachedObject)m) == null))
			{
				Vector2i asVector2 = ClosestSpawnerTo((CachedObject)m).AsVector;
				if (((Vector2i)(ref asVector2)).Distance((Vector2i)m.Position) <= 30)
				{
					goto IL_005e;
				}
			}
			int result3 = (((float)m.Position.Distance(vector2i_0) < distance) ? 1 : 0);
			goto IL_005f;
			IL_005f:
			return (byte)result3 != 0;
			IL_005e:
			result3 = 0;
			goto IL_005f;
		});
		if (boss)
		{
			List<CachedMonster> list = source.Where(delegate(CachedMonster m)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Invalid comparison between Unknown and I4
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				if (!(m != null) || !IsBlightMob(m) || (int)m.Rarity < 3)
				{
					goto IL_0067;
				}
				if (!(ClosestSpawnerTo((CachedObject)m) == null))
				{
					Vector2i asVector = ClosestSpawnerTo((CachedObject)m).AsVector;
					if (((Vector2i)(ref asVector)).Distance((Vector2i)m.Position) <= 30)
					{
						goto IL_0067;
					}
				}
				int result2 = (((float)m.Position.Distance(vector2i_0) < distance) ? 1 : 0);
				goto IL_0068;
				IL_0067:
				result2 = 0;
				goto IL_0068;
				IL_0068:
				return (byte)result2 != 0;
			}).ToList();
			return list.Count;
		}
		return result;
	}

	public static int NumberOfMobsAround(NetworkObject target, float distance, bool boss = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Vector2i vector2i_0 = target.Position;
		List<Monster> source = ObjectManager.GetObjectsByType<Monster>().ToList();
		int result = source.Count(delegate(Monster m)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (!((NetworkObject)(object)m != (NetworkObject)null) || !IsBlightMob((Actor)(object)m))
			{
				goto IL_005d;
			}
			Vector2i val2;
			if (!(ClosestSpawnerTo((NetworkObject)(object)m) == null))
			{
				val2 = ClosestSpawnerTo((NetworkObject)(object)m).AsVector;
				if (((Vector2i)(ref val2)).Distance(((NetworkObject)m).Position) <= 30)
				{
					goto IL_005d;
				}
			}
			val2 = ((NetworkObject)m).Position;
			int result3 = (((float)((Vector2i)(ref val2)).Distance(vector2i_0) < distance) ? 1 : 0);
			goto IL_005e;
			IL_005e:
			return (byte)result3 != 0;
			IL_005d:
			result3 = 0;
			goto IL_005e;
		});
		if (!boss)
		{
			return result;
		}
		List<Monster> list = source.Where(delegate(Monster m)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			if (!((NetworkObject)(object)m != (NetworkObject)null) || !IsBlightMob((Actor)(object)m) || (int)m.Rarity < 3)
			{
				goto IL_0066;
			}
			Vector2i val;
			if (!(ClosestSpawnerTo((NetworkObject)(object)m) == null))
			{
				val = ClosestSpawnerTo((NetworkObject)(object)m).AsVector;
				if (((Vector2i)(ref val)).Distance(((NetworkObject)m).Position) <= 30)
				{
					goto IL_0066;
				}
			}
			val = ((NetworkObject)m).Position;
			int result2 = (((float)((Vector2i)(ref val)).Distance(vector2i_0) < distance) ? 1 : 0);
			goto IL_0067;
			IL_0066:
			result2 = 0;
			goto IL_0067;
			IL_0067:
			return (byte)result2 != 0;
		}).ToList();
		return list.Count;
	}

	private static bool IsBlightMob(Actor x)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (!((NetworkObject)(object)x == (NetworkObject)null))
		{
			if ((int)((NetworkObject)x).Reaction != -1)
			{
				if (((NetworkObject)x).Metadata == null)
				{
					return false;
				}
				if (!x.IsDead)
				{
					if (((NetworkObject)x).Name == "Blighted Spore")
					{
						return false;
					}
					return ((NetworkObject)x).Metadata.Contains("Blight") && !((NetworkObject)x).Metadata.Contains("BlightTower") && !((NetworkObject)x).Metadata.Contains("BlightFoundation") && !((NetworkObject)x).Metadata.Contains("BlightBuilder");
				}
				return false;
			}
			return false;
		}
		return false;
	}

	private static bool IsBlightMob(CachedMonster x)
	{
		if (!(x == null))
		{
			if (x.Metadata == null)
			{
				return false;
			}
			return x.Name != "Blighted Spore" && x.Metadata.Contains("Blight") && !x.Metadata.Contains("BlightTower") && !x.Metadata.Contains("BlightFoundation") && !x.Metadata.Contains("BlightBuilder");
		}
		return false;
	}

	public static int NumberOfDotsNear(CachedObject target, float distance)
	{
		WalkablePosition walkablePosition_0 = target.Position;
		return ObjectManager.Objects.Count(delegate(NetworkObject m)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			int result;
			if (m != (NetworkObject)null && m.Metadata.Contains("BlightPathway"))
			{
				Vector2i position = m.Position;
				result = (((float)((Vector2i)(ref position)).Distance((Vector2i)walkablePosition_0) < distance) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		});
	}

	public static int NumberOfDotsNear(NetworkObject target, float distance)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Vector2i vector2i_0 = target.Position;
		return ObjectManager.Objects.Count(delegate(NetworkObject m)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			int result;
			if (m != (NetworkObject)null && m.Metadata.Contains("BlightPathway"))
			{
				Vector2i position = m.Position;
				result = (((float)((Vector2i)(ref position)).Distance(vector2i_0) < distance) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		});
	}

	public static WalkablePosition ClosestSpawnerTo(CachedObject target)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		WalkablePosition walkablePosition_0 = target.Position;
		List<MinimapIconWrapper> source = InstanceInfo.MinimapIcons.Where((MinimapIconWrapper p) => p.MinimapIcon != null && p.MinimapIcon.Name.Contains("BlightPortal")).ToList();
		MinimapIconWrapper val = source.OrderBy(delegate(MinimapIconWrapper p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Vector2i lastSeenPosition = p.LastSeenPosition;
			return ((Vector2i)(ref lastSeenPosition)).Distance((Vector2i)walkablePosition_0);
		}).FirstOrDefault();
		return (val != null) ? new WalkablePosition(val.MinimapIcon.Name, val.LastSeenPosition) : null;
	}

	public static WalkablePosition ClosestSpawnerTo(NetworkObject target)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		Vector2i vector2i_0 = target.Position;
		List<MinimapIconWrapper> source = InstanceInfo.MinimapIcons.Where((MinimapIconWrapper p) => p.MinimapIcon != null && p.MinimapIcon.Name.Contains("BlightPortal")).ToList();
		MinimapIconWrapper val = source.OrderBy(delegate(MinimapIconWrapper p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i lastSeenPosition = p.LastSeenPosition;
			return ((Vector2i)(ref lastSeenPosition)).Distance(vector2i_0);
		}).FirstOrDefault();
		return (val != null) ? new WalkablePosition(val.MinimapIcon.Name, val.LastSeenPosition) : null;
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

	static HandleEventTask()
	{
		interval_0 = new Interval(5000);
		SpawnerList = new List<WalkablePosition>();
	}
}
