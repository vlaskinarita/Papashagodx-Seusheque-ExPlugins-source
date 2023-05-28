using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.QuestBotEx.QuestHandlers;
using Newtonsoft.Json;

namespace ExPlugins.QuestBotEx;

public static class QuestManager
{
	public class CompletedQuests
	{
		private class Class20
		{
			public readonly HashSet<string> hashSet_0 = new HashSet<string>();

			public CharacterClass characterClass_0;

			public int int_0;
		}

		private static CompletedQuests completedQuests_0;

		private readonly string string_0 = Path.Combine(Configuration.Instance.Path, "CompletedQuests.json");

		private Dictionary<string, Class20> dictionary_0;

		public static CompletedQuests Instance => completedQuests_0 ?? (completedQuests_0 = new CompletedQuests());

		private CompletedQuests()
		{
			if (File.Exists(string_0))
			{
				Load();
			}
			else
			{
				dictionary_0 = new Dictionary<string, Class20>();
			}
		}

		public void Add(DatQuestWrapper quest)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			LocalPlayer me = LokiPoe.Me;
			string name = ((NetworkObject)me).Name;
			if (dictionary_0.TryGetValue(name, out var value))
			{
				value.characterClass_0 = ((Player)me).Class;
				value.int_0 = ((Player)me).Level;
				value.hashSet_0.Add(quest.Id);
			}
			else
			{
				Class20 @class = new Class20
				{
					characterClass_0 = ((Player)me).Class,
					int_0 = ((Player)me).Level
				};
				@class.hashSet_0.Add(quest.Id);
				dictionary_0.Add(name, @class);
			}
			Save();
			GlobalLog.Debug("[QuestManager] Quest \"" + quest.Name + "\"(" + quest.Id + ") has been added to completed quests cache.");
		}

		public bool Contains(DatQuestWrapper quest)
		{
			Class20 value;
			return dictionary_0.TryGetValue(((NetworkObject)LokiPoe.Me).Name, out value) && value.hashSet_0.Contains(quest.Id);
		}

		public void Verify()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			LocalPlayer me = LokiPoe.Me;
			string name = ((NetworkObject)me).Name;
			if (dictionary_0.TryGetValue(name, out var value) && (((Player)me).Class != value.characterClass_0 || ((Player)me).Level < value.int_0))
			{
				GlobalLog.Info("[QuestBotEx] Removing outdated character entry from completed quest cache.");
				dictionary_0.Remove(name);
				Save();
			}
		}

		private void Save()
		{
			string contents = JsonConvert.SerializeObject((object)dictionary_0, (Formatting)1);
			File.WriteAllText(string_0, contents);
		}

		private void Load()
		{
			string text = File.ReadAllText(string_0);
			if (!string.IsNullOrWhiteSpace(text))
			{
				try
				{
					dictionary_0 = JsonConvert.DeserializeObject<Dictionary<string, Class20>>(text);
				}
				catch (Exception)
				{
					GlobalLog.Info("[QuestBotEx] Clearing current completed quest cache. Exception during json deserialization.");
					Clear();
					return;
				}
				if (dictionary_0 == null)
				{
					GlobalLog.Info("[QuestBotEx] Clearing current completed quest cache. Json deserealizer returned null.");
					Clear();
				}
			}
			else
			{
				GlobalLog.Info("[QuestBotEx] Clearing current completed quest cache. Json file is empty.");
				Clear();
			}
		}

		private void Clear()
		{
			dictionary_0 = new Dictionary<string, Class20>();
			File.Delete(string_0);
		}
	}

	private static Dictionary<string, KeyValuePair<DatQuestWrapper, DatQuestStateWrapper>> dictionary_0;

	private static Dictionary<string, DatWorldAreaWrapper> dictionary_1;

	private static int int_0;

	private static readonly QuestBotSettings Config;

	private static int CurrentAct => World.CurrentArea.Act;

	public static QuestHandler GetQuestHandler()
	{
		if (World.Act1.TwilightStrand.IsCurrentArea)
		{
			return CreateQuestHander(Quests.EnemyAtTheGate, "Enter Lioneye's Watch", A1_Q1_EnemyAtTheGate.EnterLioneyeWatch, A1_Q1_EnemyAtTheGate.Tick);
		}
		if (Config.CheckGrindingFirst)
		{
			QuestHandler grindingHandler = GetGrindingHandler(int.MaxValue);
			if (grindingHandler != null)
			{
				return grindingHandler;
			}
		}
		if (!World.CurrentArea.IsHideoutArea)
		{
			UpdateStatesAndWaypoints();
			int num = 1;
			AreaInfo lioneyeWatch = World.Act1.LioneyeWatch;
			DatQuestWrapper enemyAtTheGate = Quests.EnemyAtTheGate;
			if (QuestIsNotCompleted(enemyAtTheGate))
			{
				int_0 = 1000;
				if (CurrentAct == num)
				{
					int state = GetState(enemyAtTheGate);
					if (state != 0)
					{
						if (state != 1 && state != 3)
						{
							GlobalLog.Error($"[EnemyAtTheGate] Unknown quest state and area combination. State: {state}. Area: {World.CurrentArea.Name}.");
							return null;
						}
						return CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q1_EnemyAtTheGate.TakeReward, null);
					}
					CompletedQuests.Instance.Add(enemyAtTheGate);
					return QuestHandler.QuestAddedToCache;
				}
				return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
			}
			enemyAtTheGate = Quests.MercyMission;
			if (Config.IsQuestEnabled(enemyAtTheGate) && ((Player)LokiPoe.Me).Level > 5 && QuestIsNotCompleted(enemyAtTheGate))
			{
				if (CurrentAct == num)
				{
					int state2 = GetState(enemyAtTheGate);
					if (state2 != 0)
					{
						if (state2 <= 2 || Helpers.PlayerHasQuestItem("MedicineSet"))
						{
							return CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q2_MercyMission.TakeReward, null);
						}
						return CreateQuestHander(enemyAtTheGate, "Kill Hailrake", A1_Q2_MercyMission.KillHailrake, A1_Q2_MercyMission.Tick);
					}
					CompletedQuests.Instance.Add(enemyAtTheGate);
					return QuestHandler.QuestAddedToCache;
				}
				return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
			}
			enemyAtTheGate = Quests.BreakingSomeEggs;
			if (!QuestIsNotCompleted(enemyAtTheGate))
			{
				enemyAtTheGate = Quests.DirtyJob;
				if (Config.IsQuestEnabled(enemyAtTheGate) && ((Player)LokiPoe.Me).Level > 25 && QuestIsNotCompleted(enemyAtTheGate))
				{
					if (CurrentAct != num)
					{
						return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
					}
					int state3 = GetState(enemyAtTheGate);
					if (state3 != 0)
					{
						return (state3 >= 4) ? CreateQuestHander(enemyAtTheGate, "Clear Fetid Pool", A1_Q3_DirtyJob.ClearFetidPool, A1_Q3_DirtyJob.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q3_DirtyJob.TakeReward, null);
					}
					CompletedQuests.Instance.Add(enemyAtTheGate);
					return QuestHandler.QuestAddedToCache;
				}
				enemyAtTheGate = Quests.CagedBrute;
				if (!QuestIsNotCompleted(enemyAtTheGate))
				{
					enemyAtTheGate = Quests.DwellerOfTheDeep;
					if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
					{
						if (CurrentAct == num)
						{
							int state4 = GetState(enemyAtTheGate);
							if (state4 != 0)
							{
								return (state4 >= 4) ? CreateQuestHander(enemyAtTheGate, "Kill Deep Dweller", A1_Q5_DwellerOfTheDeep.KillDweller, A1_Q5_DwellerOfTheDeep.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q5_DwellerOfTheDeep.TakeReward, null);
							}
							CompletedQuests.Instance.Add(enemyAtTheGate);
							return QuestHandler.QuestAddedToCache;
						}
						return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
					}
					enemyAtTheGate = Quests.SirensCadence;
					if (QuestIsNotCompleted(enemyAtTheGate))
					{
						int_0 = 1300;
						if (!IsWaypointOpened(World.Act2.SouthernForest) || IsWaypointOpened(World.Act2.ForestEncampment))
						{
							if (CurrentAct == num)
							{
								int state5 = GetState(enemyAtTheGate);
								if (state5 != 6 && state5 != 1)
								{
									if (state5 == 0 || IsWaypointOpened(World.Act2.SouthernForest))
									{
										CompletedQuests.Instance.Add(enemyAtTheGate);
										return QuestHandler.QuestAddedToCache;
									}
									return CreateQuestHander(enemyAtTheGate, "Kill Merveil", A1_Q8_SirensCadence.KillMerveil, A1_Q8_SirensCadence.Tick);
								}
								return CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q8_SirensCadence.TakeReward, null);
							}
							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
						}
						return CreateQuestHander(enemyAtTheGate, "Enter Forest Encampment", A1_Q8_SirensCadence.EnterForestEncampment, A1_Q8_SirensCadence.Tick);
					}
					if (IsWaypointOpened(World.Act2.ForestEncampment))
					{
						enemyAtTheGate = Quests.MaroonedMariner;
						if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
						{
							if (CurrentAct != num)
							{
								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
							}
							int state6 = GetState(enemyAtTheGate);
							if (state6 != 0)
							{
								if (state6 == 4 || state6 == 5 || Helpers.PlayerHasQuestItem("AllFlameLantern"))
								{
									return CreateQuestHander(enemyAtTheGate, "Kill Captain Fairgraves", A1_Q7_MaroonedMariner.KillFairgraves, A1_Q7_MaroonedMariner.Tick);
								}
								if (state6 <= 3)
								{
									return CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q7_MaroonedMariner.TakeReward, null);
								}
								return CreateQuestHander(enemyAtTheGate, "Grab Allflame", A1_Q7_MaroonedMariner.GrabAllflame, A1_Q7_MaroonedMariner.Tick);
							}
							CompletedQuests.Instance.Add(enemyAtTheGate);
							return QuestHandler.QuestAddedToCache;
						}
						num = 2;
						lioneyeWatch = World.Act2.ForestEncampment;
						QuestHandler questHandler = RandomAct2Quests1(2, lioneyeWatch);
						if (questHandler != null)
						{
							return questHandler;
						}
						num = 3;
						lioneyeWatch = World.Act3.SarnEncampment;
						enemyAtTheGate = Quests.LostInLove;
						if (QuestIsNotCompleted(enemyAtTheGate, 1))
						{
							int_0 = 3100;
							if (CurrentAct == num)
							{
								int state7 = GetState(enemyAtTheGate);
								if (state7 <= 1)
								{
									CompletedQuests.Instance.Add(enemyAtTheGate);
									return QuestHandler.QuestAddedToCache;
								}
								if (state7 < 10)
								{
									if (IsWaypointOpened(World.Act3.SarnEncampment))
									{
										if (state7 != 2 && state7 != 5 && !Helpers.PlayerHasQuestItem("TolmanBracelet"))
										{
											if (state7 == 3 || state7 == 4)
											{
												return CreateQuestHander(enemyAtTheGate, "Take Maramoa reward", A3_Q1_LostInLove.TakeMaramoaReward, null);
											}
											return CreateQuestHander(enemyAtTheGate, "Grab Bracelet", A3_Q1_LostInLove.GrabBracelet, A3_Q1_LostInLove.Tick);
										}
										return CreateQuestHander(enemyAtTheGate, "Take Clarissa reward", A3_Q1_LostInLove.TakeClarissaReward, null);
									}
									return CreateQuestHander(enemyAtTheGate, "Enter Sarn Encampment", A3_Q1_LostInLove.EnterSarnEncampment, null);
								}
								return CreateQuestHander(enemyAtTheGate, "Free Clarissa", A3_Q1_LostInLove.FreeClarissa, A3_Q1_LostInLove.Tick);
							}
							AreaInfo areaInfo_0 = ((!IsWaypointOpened(World.Act3.SarnEncampment)) ? World.Act3.CityOfSarn : World.Act3.SarnEncampment);
							return CreateQuestHander(enemyAtTheGate, "Travel to Act 3", () => TravelTo(areaInfo_0), null);
						}
						enemyAtTheGate = Quests.VictarioSecrets;
						if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
						{
							if (Helpers.PlayerHasQuestItemAmount("Busts/Bust", 3))
							{
								return CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q2_VictarioSecrets.TakeReward, null);
							}
							if (CurrentAct != num)
							{
								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
							}
							int state8 = GetState(enemyAtTheGate);
							if (state8 == 0)
							{
								CompletedQuests.Instance.Add(enemyAtTheGate);
								return QuestHandler.QuestAddedToCache;
							}
							if (state8 <= 3 || A3_Q2_VictarioSecrets.HaveAllBustsStates.Contains(state8))
							{
								return CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q2_VictarioSecrets.TakeReward, null);
							}
							return CreateQuestHander(enemyAtTheGate, "Grab busts", A3_Q2_VictarioSecrets.GrabBusts, A3_Q2_VictarioSecrets.Tick);
						}
						enemyAtTheGate = Quests.SwigOfHope;
						if (Config.IsQuestEnabled(enemyAtTheGate) && GetStateInaccurate(enemyAtTheGate) >= 8 && !Helpers.PlayerHasQuestItem("Fairgraves/Decanter"))
						{
							return CreateQuestHander(enemyAtTheGate, "Grab Decanter Spiritus", A3_Q6_SwigOfHope.GrabDecanter, A3_Q6_SwigOfHope.Tick);
						}
						enemyAtTheGate = Quests.RibbonSpool;
						if (QuestIsNotCompleted(enemyAtTheGate))
						{
							int_0 = 3200;
							if (CurrentAct != num)
							{
								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
							}
							switch (GetState(enemyAtTheGate))
							{
							case 0:
								CompletedQuests.Instance.Add(enemyAtTheGate);
								return QuestHandler.QuestAddedToCache;
							default:
								if (!Helpers.PlayerHasQuestItem("RibbonSpool"))
								{
									return CreateQuestHander(enemyAtTheGate, "Grab Ribbon Spool", A3_Q3_GemlingQueen.GrabRibbon, A3_Q3_GemlingQueen.Tick);
								}
								break;
							case 1:
								break;
							}
						}
						enemyAtTheGate = Quests.FieryDust;
						if (!QuestIsNotCompleted(enemyAtTheGate, 3))
						{
							enemyAtTheGate = Quests.SeverRightHand;
							if (!QuestIsNotCompleted(enemyAtTheGate))
							{
								enemyAtTheGate = Quests.PietyPets;
								if (QuestIsNotCompleted(enemyAtTheGate))
								{
									int_0 = 3500;
									if (CurrentAct == num)
									{
										int state9 = GetState(enemyAtTheGate);
										if (state9 != 0)
										{
											return (state9 >= 4) ? CreateQuestHander(enemyAtTheGate, "Kill Piety", A3_Q5_PietyPets.KillPiety, A3_Q5_PietyPets.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q5_PietyPets.TakeReward, null);
										}
										CompletedQuests.Instance.Add(enemyAtTheGate);
										return QuestHandler.QuestAddedToCache;
									}
									return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
								}
								enemyAtTheGate = Quests.SwigOfHope;
								if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
								{
									if (CurrentAct != num)
									{
										return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
									}
									int state10 = GetState(enemyAtTheGate);
									if (state10 == 0)
									{
										CompletedQuests.Instance.Add(enemyAtTheGate);
										return QuestHandler.QuestAddedToCache;
									}
									if (state10 > 6)
									{
										if (state10 != 7 && !Helpers.PlayerHasQuestItem("Fairgraves/Decanter"))
										{
											return CreateQuestHander(enemyAtTheGate, "Grab Decanter Spiritus", A3_Q6_SwigOfHope.GrabDecanter, A3_Q6_SwigOfHope.Tick);
										}
										if (state10 != 9 && !Helpers.PlayerHasQuestItem("Fairgraves/Fruit"))
										{
											return CreateQuestHander(enemyAtTheGate, "Grab Chitus Plum", A3_Q6_SwigOfHope.GrabPlum, A3_Q6_SwigOfHope.Tick);
										}
									}
									return CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q6_SwigOfHope.TakeReward, A3_Q6_SwigOfHope.Tick);
								}
								enemyAtTheGate = Quests.FixtureOfFate;
								if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
								{
									if (!Helpers.PlayerHasQuestItemAmount("GoldenPages/Page", 4))
									{
										if (CurrentAct == num)
										{
											int state11 = GetState(enemyAtTheGate);
											if (state11 == 0)
											{
												CompletedQuests.Instance.Add(enemyAtTheGate);
												return QuestHandler.QuestAddedToCache;
											}
											return (state11 >= 3) ? CreateQuestHander(enemyAtTheGate, "Grab Golden Pages", A3_Q7_FixtureOfFate.GrabPages, A3_Q7_FixtureOfFate.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q7_FixtureOfFate.TakeReward, A3_Q7_FixtureOfFate.Tick);
										}
										return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
									}
									return CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q7_FixtureOfFate.TakeReward, A3_Q7_FixtureOfFate.Tick);
								}
								enemyAtTheGate = Quests.SceptreOfGod;
								if (!IsWaypointOpened(World.Act4.Aqueduct))
								{
									return CreateQuestHander(enemyAtTheGate, "Kill Dominus", A3_Q8_SceptreOfGod.KillDominus, A3_Q8_SceptreOfGod.Tick);
								}
								if (!IsWaypointOpened(World.Act4.Highgate))
								{
									return CreateQuestHander(enemyAtTheGate, "Enter Highgate", A3_Q8_SceptreOfGod.EnterHighgate, null);
								}
								num = 4;
								lioneyeWatch = World.Act4.Highgate;
								enemyAtTheGate = Quests.BreakingSeal;
								if (!QuestIsNotCompleted(enemyAtTheGate))
								{
									enemyAtTheGate = Quests.IndomitableSpirit;
									if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
									{
										if (CurrentAct == num)
										{
											int state12 = GetState(enemyAtTheGate);
											if (state12 != 0)
											{
												return (state12 >= 3) ? CreateQuestHander(enemyAtTheGate, "Free Deshret Spirit", A4_Q2_IndomitableSpirit.FreeDeshret, A4_Q2_IndomitableSpirit.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A4_Q2_IndomitableSpirit.TakeReward, null);
											}
											CompletedQuests.Instance.Add(enemyAtTheGate);
											return QuestHandler.QuestAddedToCache;
										}
										return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
									}
									enemyAtTheGate = Quests.KingOfFury;
									if (QuestIsNotCompleted(enemyAtTheGate))
									{
										if (Helpers.PlayerHasQuestItem("KaomGem"))
										{
											return CreateQuestHander(enemyAtTheGate, "Bring Eye of Fury to Dialla", A4_Q3_KingOfFury.TurnInQuest, A4_Q3_KingOfFury.Tick);
										}
										if (CurrentAct != num)
										{
											return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
										}
										int state13 = GetState(enemyAtTheGate);
										if (state13 == 0)
										{
											CompletedQuests.Instance.Add(enemyAtTheGate);
											return QuestHandler.QuestAddedToCache;
										}
										if (state13 >= 4)
										{
											return CreateQuestHander(enemyAtTheGate, "Kill Kaom", A4_Q3_KingOfFury.KillKaom, A4_Q3_KingOfFury.Tick);
										}
										if (state13 == 1)
										{
											return CreateQuestHander(enemyAtTheGate, "Take reward", A4_Q4_KingOfDesire.TakeReward, null);
										}
									}
									enemyAtTheGate = Quests.KingOfDesire;
									if (QuestIsNotCompleted(enemyAtTheGate))
									{
										if (Helpers.PlayerHasQuestItem("DaressoGem"))
										{
											return CreateQuestHander(enemyAtTheGate, "Bring Eye of Desire to Dialla", A4_Q4_KingOfDesire.TurnInQuest, A4_Q4_KingOfDesire.Tick);
										}
										if (CurrentAct != num)
										{
											return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
										}
										int state14 = GetState(enemyAtTheGate);
										if (state14 == 0)
										{
											CompletedQuests.Instance.Add(enemyAtTheGate);
											return QuestHandler.QuestAddedToCache;
										}
										if (state14 >= 4)
										{
											return CreateQuestHander(enemyAtTheGate, "Kill Daresso", A4_Q4_KingOfDesire.KillDaresso, A4_Q4_KingOfDesire.Tick);
										}
										if (state14 == 1)
										{
											return CreateQuestHander(enemyAtTheGate, "Take reward", A4_Q4_KingOfDesire.TakeReward, null);
										}
									}
									enemyAtTheGate = Quests.EternalNightmare;
									if (!QuestIsNotCompleted(enemyAtTheGate, 1))
									{
										enemyAtTheGate = Quests.ReturnToOriath;
										if (IsWaypointOpened(World.Act5.OverseerTower))
										{
											num = 5;
											lioneyeWatch = World.Act5.OverseerTower;
											if (QuestIsNotCompleted(enemyAtTheGate))
											{
												int_0 = 5100;
												if (CurrentAct != num)
												{
													return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
												}
												if (GetState(enemyAtTheGate) == 0)
												{
													CompletedQuests.Instance.Add(enemyAtTheGate);
													return QuestHandler.QuestAddedToCache;
												}
												return CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q1_ReturnToOriath.TakeReward, null);
											}
											enemyAtTheGate = Quests.InServiceToScience;
											if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
											{
												if (CurrentAct == num)
												{
													int state15 = GetState(enemyAtTheGate);
													if (state15 != 0)
													{
														if (state15 <= 3 || Helpers.PlayerHasQuestItem("Miasmeter"))
														{
															return CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q2_InServiceToScience.TakeReward, null);
														}
														return CreateQuestHander(enemyAtTheGate, "Grab Miasmeter", A5_Q2_InServiceToScience.GrabMiasmeter, A5_Q2_InServiceToScience.Tick);
													}
													CompletedQuests.Instance.Add(enemyAtTheGate);
													return QuestHandler.QuestAddedToCache;
												}
												return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
											}
											enemyAtTheGate = Quests.KeyToFreedom;
											if (QuestIsNotCompleted(enemyAtTheGate))
											{
												int_0 = 5200;
												if (CurrentAct == num)
												{
													int state16 = GetState(enemyAtTheGate);
													if (state16 != 0)
													{
														if (state16 <= 2 || Helpers.PlayerHasQuestItem("TemplarCourtKey"))
														{
															return CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q3_KeyToFreedom.TakeReward, null);
														}
														return CreateQuestHander(enemyAtTheGate, "Kill Justicar Casticus", A5_Q3_KeyToFreedom.KillCasticus, A5_Q3_KeyToFreedom.Tick);
													}
													CompletedQuests.Instance.Add(enemyAtTheGate);
													return QuestHandler.QuestAddedToCache;
												}
												return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
											}
											enemyAtTheGate = Quests.DeathToPurity;
											if (!QuestIsNotCompleted(enemyAtTheGate))
											{
												if (!IsWaypointOpened(World.Act5.RuinedSquare))
												{
													UpdateGuiAndLog(Quests.RavenousGod.Name, "Travel to Ruined Square");
													return new QuestHandler(A5_Q7_RavenousGod.TalkToBannonAndGetToSquare, null);
												}
												enemyAtTheGate = Quests.KingFeast;
												if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
												{
													if (CurrentAct != num)
													{
														return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
													}
													int state17 = GetState(enemyAtTheGate);
													if (state17 != 0)
													{
														return (state17 >= 4) ? CreateQuestHander(enemyAtTheGate, "Kill Utula", A5_Q5_KingFeast.KillUtula, A5_Q5_KingFeast.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q5_KingFeast.TakeReward, null);
													}
													CompletedQuests.Instance.Add(enemyAtTheGate);
													return QuestHandler.QuestAddedToCache;
												}
												enemyAtTheGate = Quests.KitavaTorments;
												if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
												{
													if (!Helpers.PlayerHasQuestItemAmount("Act5/Torment", 3))
													{
														if (CurrentAct == num)
														{
															int state18 = GetState(enemyAtTheGate);
															if (state18 == 0)
															{
																CompletedQuests.Instance.Add(enemyAtTheGate);
																return QuestHandler.QuestAddedToCache;
															}
															return (state18 < 4) ? CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q6_KitavaTorments.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Grab Kitava's Torments", A5_Q6_KitavaTorments.GrabTorments, A5_Q6_KitavaTorments.Tick);
														}
														return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
													}
													return CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q6_KitavaTorments.TakeReward, null);
												}
												enemyAtTheGate = Quests.RavenousGod;
												if (IsWaypointOpened(World.Act6.LioneyeWatch))
												{
													num = 6;
													lioneyeWatch = World.Act6.LioneyeWatch;
													enemyAtTheGate = Quests.FallenFromGrace;
													if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
													{
														if (CurrentAct == num)
														{
															int state19 = GetState(enemyAtTheGate);
															if (state19 != 0)
															{
																return (state19 < 3) ? CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q1_FallenFromGrace.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Clear Twilight Strand", A6_Q1_FallenFromGrace.ClearStrand, A6_Q1_FallenFromGrace.Tick);
															}
															CompletedQuests.Instance.Add(enemyAtTheGate);
															return QuestHandler.QuestAddedToCache;
														}
														return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
													}
													enemyAtTheGate = Quests.BestelEpic;
													if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
													{
														if (CurrentAct == num)
														{
															int state20 = GetState(enemyAtTheGate);
															if (state20 == 0)
															{
																CompletedQuests.Instance.Add(enemyAtTheGate);
																return QuestHandler.QuestAddedToCache;
															}
															if (state20 > 3 && !Helpers.PlayerHasQuestItem("BestelsManuscript"))
															{
																return CreateQuestHander(enemyAtTheGate, "Grab Bestel's Manuscript", A6_Q2_BestelEpic.GrabManuscript, A6_Q2_BestelEpic.Tick);
															}
															return CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q2_BestelEpic.TakeReward, null);
														}
														return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
													}
													enemyAtTheGate = Quests.FatherOfWar;
													if (!QuestIsNotCompleted(enemyAtTheGate))
													{
														enemyAtTheGate = Quests.EssenceOfUmbra;
														if (!QuestIsNotCompleted(enemyAtTheGate))
														{
															enemyAtTheGate = Quests.BrineKing;
															if (IsWaypointOpened(World.Act7.BridgeEncampment))
															{
																if (A6_Q7_BrineKing.OriginalCombatRange.HasValue)
																{
																	A6_Q7_BrineKing.RestoreCombatRange();
																}
																num = 7;
																lioneyeWatch = World.Act7.BridgeEncampment;
																enemyAtTheGate = Quests.SilverLocket;
																if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																{
																	if (CurrentAct == num)
																	{
																		int state21 = GetState(enemyAtTheGate);
																		if (state21 == 0)
																		{
																			CompletedQuests.Instance.Add(enemyAtTheGate);
																			return QuestHandler.QuestAddedToCache;
																		}
																		if (state21 <= 3 || Helpers.PlayerHasQuestItem("SilverLocket"))
																		{
																			return CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q1_SilverLocket.TakeReward, null);
																		}
																		return CreateQuestHander(enemyAtTheGate, "Grab Silver Locket", A7_Q1_SilverLocket.GrabSilverLocket, A7_Q1_SilverLocket.Tick);
																	}
																	return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																}
																enemyAtTheGate = Quests.EssenceOfArtist;
																if (QuestIsNotCompleted(enemyAtTheGate))
																{
																	int_0 = 7100;
																	if (CurrentAct == num)
																	{
																		int state22 = GetState(enemyAtTheGate);
																		if (state22 != 0)
																		{
																			if (state22 < 11)
																			{
																				if (state22 >= 3)
																				{
																					return CreateQuestHander(enemyAtTheGate, "Kill Maligaro", A7_Q2_EssenceOfArtist.KillMaligaro, A7_Q2_EssenceOfArtist.Tick);
																				}
																				return CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q2_EssenceOfArtist.TakeReward, null);
																			}
																			return CreateQuestHander(enemyAtTheGate, "Grab Maligaro's Map", A7_Q2_EssenceOfArtist.GrabMaligaroMap, A7_Q2_EssenceOfArtist.Tick);
																		}
																		CompletedQuests.Instance.Add(enemyAtTheGate);
																		return QuestHandler.QuestAddedToCache;
																	}
																	return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																}
																enemyAtTheGate = Quests.WebOfSecrets;
																if (!QuestIsNotCompleted(enemyAtTheGate, 1))
																{
																	enemyAtTheGate = Quests.MasterOfMillionFaces;
																	if (QuestIsNotCompleted(enemyAtTheGate, 3))
																	{
																		int_0 = 7300;
																		if (IsWaypointOpened(World.Act7.NorthernForest))
																		{
																			if (CurrentAct == num)
																			{
																				int state23 = GetState(enemyAtTheGate);
																				if (state23 <= 3)
																				{
																					CompletedQuests.Instance.Add(enemyAtTheGate);
																					return QuestHandler.QuestAddedToCache;
																				}
																				return CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q4_MasterOfMillionFaces.TakeReward, null);
																			}
																			return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																		}
																		return CreateQuestHander(enemyAtTheGate, "Kill Ralakesh", A7_Q4_MasterOfMillionFaces.KillRalakesh, A7_Q4_MasterOfMillionFaces.Tick);
																	}
																	enemyAtTheGate = Quests.InMemoryOfGreust;
																	if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																	{
																		if (CurrentAct == num)
																		{
																			int state24 = GetState(enemyAtTheGate);
																			if (state24 != 0)
																			{
																				if (state24 >= 5)
																				{
																					return CreateQuestHander(enemyAtTheGate, "Take Greust's Necklace", A7_Q5_InMemoryOfGreust.TakeNecklace, null);
																				}
																				if (state24 < 3)
																				{
																					return CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q5_InMemoryOfGreust.TakeReward, null);
																				}
																				return CreateQuestHander(enemyAtTheGate, "Place Greust's Necklace", A7_Q5_InMemoryOfGreust.PlaceNecklace, A7_Q5_InMemoryOfGreust.Tick);
																			}
																			CompletedQuests.Instance.Add(enemyAtTheGate);
																			return QuestHandler.QuestAddedToCache;
																		}
																		return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																	}
																	enemyAtTheGate = Quests.LightingTheWay;
																	if (!QuestIsNotCompleted(enemyAtTheGate, 3))
																	{
																		enemyAtTheGate = Quests.QueenOfDespair;
																		if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																		{
																			if (CurrentAct == num)
																			{
																				int state25 = GetState(enemyAtTheGate);
																				if (state25 != 0)
																				{
																					return (state25 < 3) ? CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q7_QueenOfDespair.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Kill Gruthkul", A7_Q7_QueenOfDespair.KillGruthkul, A7_Q7_QueenOfDespair.Tick);
																				}
																				CompletedQuests.Instance.Add(enemyAtTheGate);
																				return QuestHandler.QuestAddedToCache;
																			}
																			return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																		}
																		enemyAtTheGate = Quests.KisharaStar;
																		if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																		{
																			if (CurrentAct == num)
																			{
																				int state26 = GetState(enemyAtTheGate);
																				if (state26 == 0)
																				{
																					CompletedQuests.Instance.Add(enemyAtTheGate);
																					return QuestHandler.QuestAddedToCache;
																				}
																				if (state26 <= 3 || Helpers.PlayerHasQuestItem("KisharaStar"))
																				{
																					return CreateQuestHander(enemyAtTheGate, "Take reward", A7_Q8_KisharaStar.TakeReward, null);
																				}
																				return CreateQuestHander(enemyAtTheGate, "Grab Kishara's Star", A7_Q8_KisharaStar.GrabKisharaStar, A7_Q8_KisharaStar.Tick);
																			}
																			return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																		}
																		enemyAtTheGate = Quests.MotherOfSpiders;
																		if (IsWaypointOpened(World.Act8.SarnRamparts))
																		{
																			if (IsWaypointOpened(World.Act8.SarnEncampment))
																			{
																				num = 8;
																				lioneyeWatch = World.Act8.SarnEncampment;
																				enemyAtTheGate = Quests.EssenceOfHag;
																				if (QuestIsNotCompleted(enemyAtTheGate))
																				{
																					int_0 = 8100;
																					if (!IsWaypointOpened(World.Act8.DoedreCesspool))
																					{
																						return CreateQuestHander(enemyAtTheGate, "Kill Doedre", A8_Q1_EssenceOfHag.KillDoedre, A8_Q1_EssenceOfHag.Tick);
																					}
																					if (CurrentAct == num)
																					{
																						if (GetState(enemyAtTheGate) == 0)
																						{
																							CompletedQuests.Instance.Add(enemyAtTheGate);
																							return QuestHandler.QuestAddedToCache;
																						}
																						return CreateQuestHander(enemyAtTheGate, "Take reward", A8_Q1_EssenceOfHag.TakeReward, null);
																					}
																					return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																				}
																				if (!IsWaypointOpened(World.Act8.DoedreCesspool))
																				{
																					return CreateQuestHander(enemyAtTheGate, "Kill Doedre", A8_Q1_EssenceOfHag.KillDoedre, A8_Q1_EssenceOfHag.Tick);
																				}
																				enemyAtTheGate = Quests.LoveIsDead;
																				if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																				{
																					if (CurrentAct != num)
																					{
																						return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																					}
																					int state27 = GetState(enemyAtTheGate);
																					if (state27 != 0)
																					{
																						if (state27 >= 7)
																						{
																							return CreateQuestHander(enemyAtTheGate, "Grab Ankh of Eternity", A8_Q2_LoveIsDead.GrabAnkh, A8_Q2_LoveIsDead.Tick);
																						}
																						if (state27 < 4 && !Helpers.PlayerHasQuestItem("AnkhOfEternity"))
																						{
																							return CreateQuestHander(enemyAtTheGate, "Take reward", A8_Q2_LoveIsDead.TakeReward, null);
																						}
																						return CreateQuestHander(enemyAtTheGate, "Kill Tolman", A8_Q2_LoveIsDead.KillTolman, A8_Q2_LoveIsDead.Tick);
																					}
																					CompletedQuests.Instance.Add(enemyAtTheGate);
																					return QuestHandler.QuestAddedToCache;
																				}
																				enemyAtTheGate = Quests.GemlingLegion;
																				if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																				{
																					if (CurrentAct == num)
																					{
																						int state28 = GetState(enemyAtTheGate);
																						if (state28 != 0)
																						{
																							return (state28 >= 3) ? CreateQuestHander(enemyAtTheGate, "Kill Gemlings", A8_Q3_GemlingLegion.KillGemlings, A8_Q3_GemlingLegion.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A8_Q3_GemlingLegion.TakeReward, null);
																						}
																						CompletedQuests.Instance.Add(enemyAtTheGate);
																						return QuestHandler.QuestAddedToCache;
																					}
																					return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																				}
																				enemyAtTheGate = Quests.WingsOfVastiri;
																				if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																				{
																					if (CurrentAct == num)
																					{
																						int state29 = GetState(enemyAtTheGate);
																						if (state29 != 0)
																						{
																							if (state29 <= 2 || Helpers.PlayerHasQuestItem("WingsOfVastiri"))
																							{
																								return CreateQuestHander(enemyAtTheGate, "Take reward", A8_Q4_WingsOfVastiri.TakeReward, null);
																							}
																							return CreateQuestHander(enemyAtTheGate, "Kill Hector Titucius", A8_Q4_WingsOfVastiri.GrabWings, A8_Q4_WingsOfVastiri.Tick);
																						}
																						CompletedQuests.Instance.Add(enemyAtTheGate);
																						return QuestHandler.QuestAddedToCache;
																					}
																					return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																				}
																				num = 7;
																				lioneyeWatch = World.Act7.BridgeEncampment;
																				enemyAtTheGate = Quests.ClovenOne;
																				if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																				{
																					if (CurrentAct == num)
																					{
																						int state30 = GetState(enemyAtTheGate);
																						if (state30 == 0)
																						{
																							CompletedQuests.Instance.Add(enemyAtTheGate);
																							return QuestHandler.QuestAddedToCache;
																						}
																						return (state30 >= 3) ? CreateQuestHander(enemyAtTheGate, "Kill Abberath", A6_Q5_ClovenOne.KillAbberath, A6_Q5_ClovenOne.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q5_ClovenOne.TakeReward, null);
																					}
																					return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																				}
																				enemyAtTheGate = Quests.PuppetMistress;
																				if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																				{
																					if (CurrentAct == num)
																					{
																						int state31 = GetState(enemyAtTheGate);
																						if (state31 != 0)
																						{
																							return (state31 < 3) ? CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q6_PuppetMistress.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Kill Ryslatha", A6_Q6_PuppetMistress.KillRyslatha, A6_Q6_PuppetMistress.Tick);
																						}
																						CompletedQuests.Instance.Add(enemyAtTheGate);
																						return QuestHandler.QuestAddedToCache;
																					}
																					return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																				}
																				num = 8;
																				lioneyeWatch = World.Act8.SarnEncampment;
																				enemyAtTheGate = Quests.LunarEclipse;
																				if (IsWaypointOpened(World.Act9.BloodAqueduct))
																				{
																					if (IsWaypointOpened(World.Act9.Highgate))
																					{
																						num = 9;
																						lioneyeWatch = World.Act9.Highgate;
																						enemyAtTheGate = Quests.StormBlade;
																						if (Config.IsQuestEnabled(Quests.QueenOfSands) && QuestIsNotCompleted(enemyAtTheGate))
																						{
																							if (CurrentAct != num)
																							{
																								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																							}
																							int state32 = GetState(enemyAtTheGate);
																							if (state32 == 0)
																							{
																								CompletedQuests.Instance.Add(enemyAtTheGate);
																								return QuestHandler.QuestAddedToCache;
																							}
																							if (state32 > 3 && !Helpers.PlayerHasQuestItem("StormSword"))
																							{
																								return CreateQuestHander(enemyAtTheGate, "Grab Storm Blade", A9_Q1_StormBlade.GrabStormBlade, A9_Q1_StormBlade.Tick);
																							}
																							return CreateQuestHander(enemyAtTheGate, "Take reward", A9_Q1_StormBlade.TakeReward, null);
																						}
																						enemyAtTheGate = Quests.QueenOfSands;
																						if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																						{
																							if (CurrentAct == num)
																							{
																								int state33 = GetState(enemyAtTheGate);
																								if (state33 == 0)
																								{
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								if (state33 < 8)
																								{
																									if (state33 != 7)
																									{
																										if (state33 >= 3)
																										{
																											return CreateQuestHander(enemyAtTheGate, "Kill Shakari", A9_Q2_QueenOfSands.KillShakari, A9_Q2_QueenOfSands.Tick);
																										}
																										return CreateQuestHander(enemyAtTheGate, "Take reward", A9_Q2_QueenOfSands.TakeReward, null);
																									}
																									return CreateQuestHander(enemyAtTheGate, "Take Bottled Storm", A9_Q2_QueenOfSands.TakeBottledStorm, A9_Q2_QueenOfSands.Tick);
																								}
																								return CreateQuestHander(enemyAtTheGate, "Talk to Sin", A9_Q2_QueenOfSands.TalkToSin, A9_Q2_QueenOfSands.Tick);
																							}
																							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																						}
																						enemyAtTheGate = Quests.FastisFortuna;
																						if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																						{
																							if (CurrentAct != num)
																							{
																								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																							}
																							int state34 = GetState(enemyAtTheGate);
																							if (state34 == 0)
																							{
																								CompletedQuests.Instance.Add(enemyAtTheGate);
																								return QuestHandler.QuestAddedToCache;
																							}
																							if (state34 > 3 && !Helpers.PlayerHasQuestItem("Calendar"))
																							{
																								return CreateQuestHander(enemyAtTheGate, "Grab Calendar of Fortune", A9_Q3_FastisFortuna.GrabCalendar, A9_Q3_FastisFortuna.Tick);
																							}
																							return CreateQuestHander(enemyAtTheGate, "Take reward", A9_Q3_FastisFortuna.TakeReward, null);
																						}
																						enemyAtTheGate = Quests.RulerOfHighgate;
																						if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate, 1))
																						{
																							if (CurrentAct == num)
																							{
																								int state35 = GetState(enemyAtTheGate);
																								if (state35 <= 1)
																								{
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								if (state35 == 4)
																								{
																									return CreateQuestHander(enemyAtTheGate, "Take reward", A9_Q4_RulerOfHighgate.TakeTasuniReward, null);
																								}
																								if (state35 > 6 && !Helpers.PlayerHasQuestItem("SekhemaFeather"))
																								{
																									return CreateQuestHander(enemyAtTheGate, "Kill Garukhan", A9_Q4_RulerOfHighgate.KillGarukhan, A9_Q4_RulerOfHighgate.Tick);
																								}
																								return CreateQuestHander(enemyAtTheGate, "Take reward", A9_Q4_RulerOfHighgate.TakeIrashaReward, null);
																							}
																							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																						}
																						int_0 = 9100;
																						enemyAtTheGate = Quests.RecurringNightmare;
																						if (IsWaypointOpened(World.Act10.OriathDocks))
																						{
																							num = 10;
																							lioneyeWatch = World.Act10.OriathDocks;
																							enemyAtTheGate = Quests.SafePassage;
																							if (QuestIsNotCompleted(enemyAtTheGate))
																							{
																								if (CurrentAct != num)
																								{
																									return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																								}
																								int state36 = GetState(enemyAtTheGate);
																								if (state36 == 0)
																								{
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								return (state36 >= 5) ? CreateQuestHander(enemyAtTheGate, "Save Bannon", A10_Q1_SafePassage.SaveBannon, A10_Q1_SafePassage.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q1_SafePassage.TakeReward, null);
																							}
																							int_0 = 10100;
																							enemyAtTheGate = Quests.NoLoveForOldGhosts;
																							if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																							{
																								if (CurrentAct == num)
																								{
																									int state37 = GetState(enemyAtTheGate);
																									if (state37 == 0)
																									{
																										CompletedQuests.Instance.Add(enemyAtTheGate);
																										return QuestHandler.QuestAddedToCache;
																									}
																									if (state37 <= 3 || Helpers.PlayerHasQuestItem("Potion"))
																									{
																										return CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q2_NoLoveForOldGhosts.TakeReward, null);
																									}
																									return CreateQuestHander(enemyAtTheGate, "Grab Elixir of Allure", A10_Q2_NoLoveForOldGhosts.GrabElixir, A10_Q2_NoLoveForOldGhosts.Tick);
																								}
																								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																							}
																							enemyAtTheGate = Quests.VilentaVengeance;
																							if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																							{
																								if (CurrentAct != num)
																								{
																									return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																								}
																								int state38 = GetState(enemyAtTheGate);
																								if (state38 != 0)
																								{
																									return (state38 >= 3) ? CreateQuestHander(enemyAtTheGate, "Kill Vilenta", A10_Q3_VilentaVengeance.KillVilenta, A10_Q3_VilentaVengeance.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q3_VilentaVengeance.TakeReward, null);
																								}
																								CompletedQuests.Instance.Add(enemyAtTheGate);
																								return QuestHandler.QuestAddedToCache;
																							}
																							enemyAtTheGate = Quests.MapToTsoatha;
																							if (Config.IsQuestEnabled(enemyAtTheGate) && QuestIsNotCompleted(enemyAtTheGate))
																							{
																								if (CurrentAct == num)
																								{
																									int state39 = GetState(enemyAtTheGate);
																									if (state39 != 0)
																									{
																										if (state39 > 3 && !Helpers.PlayerHasQuestItem("Teardrop"))
																										{
																											return CreateQuestHander(enemyAtTheGate, "Grab Teardrop", A10_Q4_MapToTsoatha.GrabTeardrop, A10_Q4_MapToTsoatha.Tick);
																										}
																										return CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q4_MapToTsoatha.TakeReward, null);
																									}
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																							}
																							num = 8;
																							lioneyeWatch = World.Act8.SarnEncampment;
																							enemyAtTheGate = Quests.ReflectionOfTerror;
																							if (Config.IsQuestEnabled(enemyAtTheGate) && ((Player)LokiPoe.Me).Level > 64 && QuestIsNotCompleted(enemyAtTheGate))
																							{
																								if (CurrentAct == num)
																								{
																									int state40 = GetState(enemyAtTheGate);
																									if (state40 != 0)
																									{
																										return (state40 < 3) ? CreateQuestHander(enemyAtTheGate, "Take reward", A8_Q5_ReflectionOfTerror.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Kill Yugul", A8_Q5_ReflectionOfTerror.KillYugul, A8_Q5_ReflectionOfTerror.Tick);
																									}
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																							}
																							num = 10;
																							lioneyeWatch = World.Act10.OriathDocks;
																							enemyAtTheGate = Quests.DeathAndRebirth;
																							if (QuestIsNotCompleted(enemyAtTheGate))
																							{
																								int_0 = 11000;
																								if (!Helpers.PlayerHasQuestItem("AvariusStaff"))
																								{
																									if (CurrentAct == num)
																									{
																										int state41 = GetState(enemyAtTheGate);
																										if (state41 == 0)
																										{
																											CompletedQuests.Instance.Add(enemyAtTheGate);
																											return QuestHandler.QuestAddedToCache;
																										}
																										return (state41 < 5) ? CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q5_DeathAndRebirth.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Kill Avarius", A10_Q5_DeathAndRebirth.KillAvarius, A10_Q5_DeathAndRebirth.Tick);
																									}
																									return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																								}
																								return CreateQuestHander(enemyAtTheGate, "Turn in Staff of Purity", A10_Q5_DeathAndRebirth.TurnInStaff, null);
																							}
																							enemyAtTheGate = Quests.EndToHunger;
																							if (!QuestIsNotCompleted(enemyAtTheGate))
																							{
																								enemyAtTheGate = Quests.TowardtheFuture;
																								if (QuestIsNotCompleted(enemyAtTheGate))
																								{
																									if (World.Act11.KaruiShores.IsWaypointOpened)
																									{
																										CompletedQuests.Instance.Add(enemyAtTheGate);
																										return QuestHandler.QuestAddedToCache;
																									}
																									if (GetState(enemyAtTheGate) == 1 && CurrentAct != 11)
																									{
																										return CreateQuestHander(enemyAtTheGate, "Sail from Oriath", A10_Q6_EndToHunger.SailFromOriath, null);
																									}
																									if (CurrentAct != num)
																									{
																										return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																									}
																								}
																								QuestHandler grindingHandler2 = GetGrindingHandler(int.MaxValue);
																								if (grindingHandler2 == null)
																								{
																									return QuestHandler.AllQuestsDone;
																								}
																								return grindingHandler2;
																							}
																							if (CurrentAct == num)
																							{
																								if (GetState(enemyAtTheGate) == 0)
																								{
																									CompletedQuests.Instance.Add(enemyAtTheGate);
																									return QuestHandler.QuestAddedToCache;
																								}
																								if (GetState(enemyAtTheGate) == 5 || GetState(enemyAtTheGate) == 2)
																								{
																									return CreateQuestHander(enemyAtTheGate, "Take reward", A10_Q6_EndToHunger.TakeReward, null);
																								}
																								return CreateQuestHander(enemyAtTheGate, "Kill Kitava", A10_Q6_EndToHunger.KillKitava, A10_Q6_EndToHunger.Tick);
																							}
																							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																						}
																						if (A9_Q5_RecurringNightmare.HaveAnyIngredient)
																						{
																							return CreateQuestHander(enemyAtTheGate, "Turn in the ingredient", A9_Q5_RecurringNightmare.TurnInIngredient, null);
																						}
																						if (CurrentAct != num)
																						{
																							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																						}
																						int state42 = GetState(enemyAtTheGate);
																						if (state42 >= 17)
																						{
																							if (state42 != 23 && !Helpers.PlayerHasQuestItem("Ingredient2"))
																							{
																								return CreateQuestHander(enemyAtTheGate, "Grab Basilisk Acid", A9_Q5_RecurringNightmare.GrabAcid, A9_Q5_RecurringNightmare.Tick);
																							}
																							if (state42 != 22 && !Helpers.PlayerHasQuestItem("Ingredient1"))
																							{
																								return CreateQuestHander(enemyAtTheGate, "Grab Trarthan Powder", A9_Q5_RecurringNightmare.GrabPowder, A9_Q5_RecurringNightmare.Tick);
																							}
																						}
																						return CreateQuestHander(enemyAtTheGate, "Kill Depraved Trinity", A9_Q5_RecurringNightmare.KillTrinity, A9_Q5_RecurringNightmare.Tick);
																					}
																					return CreateQuestHander(enemyAtTheGate, "Enter Highgate", A8_Q6_LunarEclipse.EnterHighgate, null);
																				}
																				if (CurrentAct == num)
																				{
																					int state43 = GetState(enemyAtTheGate);
																					if (state43 != 2 && state43 != 3)
																					{
																						if (state43 == 4)
																						{
																							return CreateQuestHander(enemyAtTheGate, "Grab Sun Orb", A8_Q6_LunarEclipse.GrabSunOrb, null);
																						}
																						return CreateQuestHander(enemyAtTheGate, "Grab Moon Orb", A8_Q6_LunarEclipse.GrabMoonOrb, null);
																					}
																					return CreateQuestHander(enemyAtTheGate, "Kill Solaris and Lunaris", A8_Q6_LunarEclipse.KillSolarisLunaris, A8_Q6_LunarEclipse.Tick);
																				}
																				return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																			}
																			return CreateQuestHander(enemyAtTheGate, "Enter Sarn Encampment", A7_Q9_MotherOfSpiders.EnterSarnEncampment, null);
																		}
																		return CreateQuestHander(enemyAtTheGate, "Kill Arakaali", A7_Q9_MotherOfSpiders.KillArakaali, A7_Q9_MotherOfSpiders.Tick);
																	}
																	int_0 = 7400;
																	if (CurrentAct != num)
																	{
																		return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																	}
																	int state44 = GetState(enemyAtTheGate);
																	if (state44 <= 3)
																	{
																		CompletedQuests.Instance.Add(enemyAtTheGate);
																		return QuestHandler.QuestAddedToCache;
																	}
																	return CreateQuestHander(enemyAtTheGate, "Grab Fireflies", A7_Q6_LightingTheWay.GrabFireflies, A7_Q6_LightingTheWay.Tick);
																}
																int_0 = 7200;
																if (CurrentAct != num)
																{
																	return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
																}
																int state45 = GetState(enemyAtTheGate);
																if (state45 <= 1)
																{
																	CompletedQuests.Instance.Add(enemyAtTheGate);
																	return QuestHandler.QuestAddedToCache;
																}
																if (state45 > 3 && !Helpers.PlayerHasQuestItem("BlackVenom"))
																{
																	return CreateQuestHander(enemyAtTheGate, "Kill Maligaro", A7_Q2_EssenceOfArtist.KillMaligaro, A7_Q2_EssenceOfArtist.Tick);
																}
																return CreateQuestHander(enemyAtTheGate, "Take Obsidian Key", A7_Q3_WebOfSecrets.TakeObsidianKey, A7_Q3_WebOfSecrets.Tick);
															}
															if (CurrentAct == num)
															{
																int state46 = GetState(enemyAtTheGate);
																if (state46 < 10 && state46 != 6)
																{
																	if (state46 != 8 && state46 != 9)
																	{
																		if (state46 != 7)
																		{
																			if (!IsWaypointOpened(World.Act6.BrineKingReef))
																			{
																				return CreateQuestHander(enemyAtTheGate, "Sail to Brine King's Reef", A6_Q7_BrineKing.SailToReef, A6_Q7_BrineKing.Tick);
																			}
																			return CreateQuestHander(enemyAtTheGate, "Kill Brine King and Sail to Act 7", A6_Q7_BrineKing.KillBrineKingAndSailToAct7, A6_Q7_BrineKing.Tick);
																		}
																		return CreateQuestHander(enemyAtTheGate, "Light the Beacon", A6_Q7_BrineKing.LightBeacon, A6_Q7_BrineKing.Tick);
																	}
																	return CreateQuestHander(enemyAtTheGate, "Fuel the Beacon", A6_Q7_BrineKing.FuelBeacon, A6_Q7_BrineKing.Tick);
																}
																return CreateQuestHander(enemyAtTheGate, "Grab Black Flag", A6_Q7_BrineKing.GrabBlackFlag, A6_Q7_BrineKing.Tick);
															}
															return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
														}
														int_0 = 6200;
														if (IsWaypointOpened(World.Act6.PrisonerGate))
														{
															if (CurrentAct == num)
															{
																if (GetState(enemyAtTheGate) != 0)
																{
																	return CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q4_EssenceOfUmbra.TakeReward, null);
																}
																CompletedQuests.Instance.Add(enemyAtTheGate);
																return QuestHandler.QuestAddedToCache;
															}
															return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
														}
														return CreateQuestHander(enemyAtTheGate, "Kill Shavronne", A6_Q4_EssenceOfUmbra.KillShavronne, A6_Q4_EssenceOfUmbra.Tick);
													}
													int_0 = 6100;
													if (CurrentAct == num)
													{
														int state47 = GetState(enemyAtTheGate);
														if (state47 == 0)
														{
															CompletedQuests.Instance.Add(enemyAtTheGate);
															return QuestHandler.QuestAddedToCache;
														}
														if (state47 < 8)
														{
															if (state47 < 3)
															{
																return CreateQuestHander(enemyAtTheGate, "Take reward", A6_Q3_FatherOfWar.TakeReward, null);
															}
															return CreateQuestHander(enemyAtTheGate, "Kill Tukohama", A6_Q3_FatherOfWar.KillTukohama, A6_Q3_FatherOfWar.Tick);
														}
														return CreateQuestHander(enemyAtTheGate, "Grab Eye of Conquest", A6_Q3_FatherOfWar.GrabEyeOfConquest, A6_Q3_FatherOfWar.Tick);
													}
													return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
												}
												if (!IsWaypointOpened(World.Act5.CathedralRooftop))
												{
													if (CurrentAct == num)
													{
														int state48 = GetState(enemyAtTheGate);
														if (state48 == 0)
														{
															CompletedQuests.Instance.Add(enemyAtTheGate);
															return QuestHandler.QuestAddedToCache;
														}
														return (state48 >= 7) ? CreateQuestHander(enemyAtTheGate, "Grab Sign of Purity", A5_Q7_RavenousGod.GrabSignOfPurity, A5_Q7_RavenousGod.Tick) : CreateQuestHander(enemyAtTheGate, "Kill Kitava", A5_Q7_RavenousGod.KillKitava, A5_Q7_RavenousGod.Tick);
													}
													return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
												}
												return CreateQuestHander(enemyAtTheGate, "Sail to Wraeclast", A5_Q7_RavenousGod.SailToWraeclast, null);
											}
											int_0 = 5300;
											if (CurrentAct == num)
											{
												int state49 = GetState(enemyAtTheGate);
												if (state49 != 0)
												{
													return (state49 >= 4) ? CreateQuestHander(enemyAtTheGate, "Kill Avarius", A5_Q4_DeathToPurity.KillAvarius, A5_Q4_DeathToPurity.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A5_Q4_DeathToPurity.TakeReward, null);
												}
												CompletedQuests.Instance.Add(enemyAtTheGate);
												return QuestHandler.QuestAddedToCache;
											}
											return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
										}
										return CreateQuestHander(enemyAtTheGate, "Enter Overseer Tower", A5_Q1_ReturnToOriath.EnterOverseerTower, A5_Q1_ReturnToOriath.Tick);
									}
									if (CurrentAct != num)
									{
										return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
									}
									int state50 = GetState(enemyAtTheGate);
									if (state50 > 1)
									{
										if (state50 == 2 || state50 == 3)
										{
											return CreateQuestHander(enemyAtTheGate, "Talk to Tasuni", A4_Q5_EternalNightmare.TalkToTasuni, null);
										}
										return CreateQuestHander(enemyAtTheGate, "Kill Malachai", A4_Q5_EternalNightmare.KillMalachai, A4_Q5_EternalNightmare.Tick);
									}
									CompletedQuests.Instance.Add(enemyAtTheGate);
									return QuestHandler.QuestAddedToCache;
								}
								if (!Helpers.PlayerHasQuestItem("RedBanner"))
								{
									if (CurrentAct == num)
									{
										int state51 = GetState(enemyAtTheGate);
										if (state51 != 0)
										{
											return (state51 >= 3) ? CreateQuestHander(enemyAtTheGate, "Kill Voll", A4_Q1_BreakingSeal.KillVoll, A4_Q1_BreakingSeal.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A4_Q1_BreakingSeal.TakeReward, null);
										}
										CompletedQuests.Instance.Add(enemyAtTheGate);
										return QuestHandler.QuestAddedToCache;
									}
									return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
								}
								return CreateQuestHander(enemyAtTheGate, "Break Deshret Seal", A4_Q1_BreakingSeal.BreakSeal, A4_Q1_BreakingSeal.Tick);
							}
							int_0 = 3400;
							if (CurrentAct == num)
							{
								int state52 = GetState(enemyAtTheGate);
								if (state52 != 0)
								{
									return (state52 < 2) ? CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q4_SeverRightHand.TakeReward, null) : CreateQuestHander(enemyAtTheGate, "Kill General Gravicius", A3_Q4_SeverRightHand.KillGravicius, A3_Q4_SeverRightHand.Tick);
								}
								CompletedQuests.Instance.Add(enemyAtTheGate);
								return QuestHandler.QuestAddedToCache;
							}
							return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
						}
						int_0 = 3300;
						if (CurrentAct == num)
						{
							int state53 = GetState(enemyAtTheGate);
							if (state53 > 3)
							{
								if (state53 != 4 && state53 != 7 && !Helpers.PlayerHasQuestItem("SulphiteFlask"))
								{
									return CreateQuestHander(enemyAtTheGate, "Grab Thaumetic Sulphite", A3_Q3_GemlingQueen.GrabSulphite, A3_Q3_GemlingQueen.Tick);
								}
								if (GetState(Quests.RibbonSpool) == 0)
								{
									return CreateQuestHander(enemyAtTheGate, "Take reward", A3_Q3_GemlingQueen.TakeTalcReward, A3_Q3_GemlingQueen.Tick);
								}
								return CreateQuestHander(Quests.RibbonSpool, "Take reward", A3_Q3_GemlingQueen.TakeRibbonReward, A3_Q3_GemlingQueen.Tick);
							}
							CompletedQuests.Instance.Add(enemyAtTheGate);
							return QuestHandler.QuestAddedToCache;
						}
						return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
					}
					return CreateQuestHander(enemyAtTheGate, "Enter Forest Encampment", A1_Q8_SirensCadence.EnterForestEncampment, A1_Q8_SirensCadence.Tick);
				}
				int_0 = 1200;
				if (CurrentAct == num)
				{
					int state54 = GetState(enemyAtTheGate);
					if (state54 != 0)
					{
						if (state54 <= 2 || IsWaypointOpened(World.Act1.PrisonerGate))
						{
							return CreateQuestHander(enemyAtTheGate, "Take Tarkleigh reward", A1_Q6_CagedBrute.TakeTarkleighReward, null);
						}
						switch (state54)
						{
						case 4:
							return CreateQuestHander(enemyAtTheGate, "Take Nessa reward", A1_Q6_CagedBrute.TakeNessaReward, null);
						default:
							if (state54 != 6)
							{
								return CreateQuestHander(enemyAtTheGate, "Enter Prison", A1_Q6_CagedBrute.EnterPrison, A1_Q6_CagedBrute.Tick);
							}
							goto case 3;
						case 3:
						case 5:
							return CreateQuestHander(enemyAtTheGate, "Kill Brutus", A1_Q6_CagedBrute.KillBrutus, A1_Q6_CagedBrute.Tick);
						}
					}
					CompletedQuests.Instance.Add(enemyAtTheGate);
					return QuestHandler.QuestAddedToCache;
				}
				return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
			}
			int_0 = 1100;
			if (!Helpers.PlayerHasQuestItemAmount("Glyphs/Glyph", 3))
			{
				if (CurrentAct == num)
				{
					int state55 = GetState(enemyAtTheGate);
					if (state55 == 0)
					{
						CompletedQuests.Instance.Add(enemyAtTheGate);
						return QuestHandler.QuestAddedToCache;
					}
					return (state55 >= 3) ? CreateQuestHander(enemyAtTheGate, "Grab glyphs", A1_Q4_BreakingSomeEggs.GrabGlyphs, A1_Q4_BreakingSomeEggs.Tick) : CreateQuestHander(enemyAtTheGate, "Take reward", A1_Q4_BreakingSomeEggs.TakeReward, null);
				}
				return CreateTravelHander(enemyAtTheGate, lioneyeWatch, num);
			}
			return CreateQuestHander(enemyAtTheGate, "Open Submerged Passage", A1_Q4_BreakingSomeEggs.OpenPassage, A1_Q4_BreakingSomeEggs.Tick);
		}
		DatWorldAreaWrapper datWorldAreaWrapper_0 = World.LastOpenedAct;
		UpdateGuiAndLog("Travel", "Go to last opened act: " + datWorldAreaWrapper_0.Name);
		return new QuestHandler(() => TravelTo(datWorldAreaWrapper_0), null);
	}

	private static QuestHandler RandomAct2Quests1(int act, AreaInfo town)
	{
		DatQuestWrapper sharpAndCruel = Quests.SharpAndCruel;
		if (!QuestIsNotCompleted(sharpAndCruel, 1))
		{
			sharpAndCruel = Quests.WayForward;
			if (!Helpers.PlayerHasQuestItem("Book-a1q9"))
			{
				if (Config.IsQuestEnabled(sharpAndCruel) && QuestIsNotCompleted(sharpAndCruel))
				{
					if (!Helpers.PlayerHasQuestItem("SpikeSealKey"))
					{
						int num;
						if (CurrentAct != 1)
						{
							num = GetStateInaccurate(sharpAndCruel);
						}
						else
						{
							num = GetState(sharpAndCruel);
							if (num == 0)
							{
								CompletedQuests.Instance.Add(sharpAndCruel);
								return QuestHandler.QuestAddedToCache;
							}
						}
						return (num < 4) ? CreateQuestHander(sharpAndCruel, "Take reward", A2_Q2_WayForward.TakeReward, null) : CreateQuestHander(sharpAndCruel, "Kill Arteri", A2_Q2_WayForward.KillArteri, A2_Q2_WayForward.Tick);
					}
					return CreateQuestHander(sharpAndCruel, "Open path", A2_Q2_WayForward.OpenPath, A2_Q2_WayForward.Tick);
				}
				sharpAndCruel = Quests.GreatWhiteBeast;
				if (Config.IsQuestEnabled(sharpAndCruel) && QuestIsNotCompleted(sharpAndCruel))
				{
					if (CurrentAct == act)
					{
						int state = GetState(sharpAndCruel);
						if (state != 0)
						{
							return (state >= 3) ? CreateQuestHander(sharpAndCruel, "Kill Great White Beast", A2_Q3_GreatWhiteBeast.KillWhiteBeast, A2_Q3_GreatWhiteBeast.Tick) : CreateQuestHander(sharpAndCruel, "Take reward", A2_Q3_GreatWhiteBeast.TakeReward, null);
						}
						CompletedQuests.Instance.Add(sharpAndCruel);
						return QuestHandler.QuestAddedToCache;
					}
					return CreateTravelHander(sharpAndCruel, town, act);
				}
				sharpAndCruel = Quests.IntrudersInBlack;
				if (QuestIsNotCompleted(sharpAndCruel))
				{
					int_0 = 2100;
					if (CurrentAct == act)
					{
						int state2 = GetState(sharpAndCruel);
						if (state2 != 0)
						{
							if (state2 != 1 && !Helpers.PlayerHasQuestItem("PoisonSkillGem"))
							{
								return CreateQuestHander(sharpAndCruel, "Grab Baleful Gem", A2_Q4_IntrudersInBlack.GrabBalefulGem, A2_Q4_IntrudersInBlack.Tick);
							}
							return CreateQuestHander(sharpAndCruel, "Take reward", A2_Q4_IntrudersInBlack.TakeReward, null);
						}
						CompletedQuests.Instance.Add(sharpAndCruel);
						return QuestHandler.QuestAddedToCache;
					}
					return CreateTravelHander(sharpAndCruel, town, act);
				}
				sharpAndCruel = Quests.ThroughSacredGround;
				if (Config.IsQuestEnabled(sharpAndCruel) && QuestIsNotCompleted(sharpAndCruel))
				{
					if (CurrentAct != act)
					{
						return CreateTravelHander(sharpAndCruel, town, act);
					}
					if (GetState(sharpAndCruel) == 0)
					{
						CompletedQuests.Instance.Add(sharpAndCruel);
						return QuestHandler.QuestAddedToCache;
					}
					if (Helpers.PlayerHasQuestItem("GoldenHand"))
					{
						return CreateQuestHander(sharpAndCruel, "Take reward", A2_Q5_ThroughSacredGround.TakeReward, null);
					}
					return CreateQuestHander(sharpAndCruel, "Grab Golden Hand", A2_Q5_ThroughSacredGround.GrabGoldenHand, A2_Q5_ThroughSacredGround.Tick);
				}
				sharpAndCruel = Quests.DealWithBandits;
				if (QuestIsNotCompleted(sharpAndCruel, 1))
				{
					int_0 = 2500;
					if (CurrentAct != act)
					{
						return CreateTravelHander(sharpAndCruel, town, act);
					}
					int state3 = GetState(sharpAndCruel);
					if (state3 <= 1)
					{
						CompletedQuests.Instance.Add(sharpAndCruel);
						return QuestHandler.QuestAddedToCache;
					}
					QuestHandler questHandler = AnalyzeBandits2();
					if (questHandler != null)
					{
						return questHandler;
					}
					GlobalLog.Error($"[DealWithBandits] Fail to analyze bandit amulets. Quest state: {state3}.");
					return null;
				}
				sharpAndCruel = Quests.HelenaHideout;
				if (QuestIsNotCompleted(sharpAndCruel, 2))
				{
					int_0 = 2600;
					if (CurrentAct == act)
					{
						int state4 = GetState(sharpAndCruel);
						if (state4 <= 2 || ((Player)LokiPoe.Me).HasHideout)
						{
							CompletedQuests.Instance.Add(sharpAndCruel);
							return QuestHandler.QuestAddedToCache;
						}
						if (A2_HelenaHideout.Cleared && !((Player)LokiPoe.Me).HasHideout)
						{
							return CreateQuestHander(sharpAndCruel, "Talk to helena", A2_HelenaHideout.TakeReward, null);
						}
						if (!A2_HelenaHideout.Interacted)
						{
							GlobalLog.Debug("interacted!");
							return CreateQuestHander(sharpAndCruel, "Talk to helena", A2_HelenaHideout.TakeReward, null);
						}
						return CreateQuestHander(sharpAndCruel, "Clear Dread Thicket", A2_HelenaHideout.ClearDreadThicket, A2_HelenaHideout.Tick);
					}
					return CreateTravelHander(sharpAndCruel, town, act);
				}
				sharpAndCruel = Quests.ShadowOfVaal;
				if (!IsWaypointOpened(World.Act3.CityOfSarn))
				{
					return CreateQuestHander(sharpAndCruel, "Kill Vaal Oversoul", A2_Q7_ShadowOfVaal.KillVaal, A2_Q7_ShadowOfVaal.Tick);
				}
				return null;
			}
			return CreateQuestHander(sharpAndCruel, "Use reward", A2_Q2_WayForward.UseReward, null);
		}
		int_0 = 2300;
		if (CurrentAct == act)
		{
			int state5 = GetState(sharpAndCruel);
			if (state5 > 1)
			{
				return (state5 < 6) ? CreateQuestHander(sharpAndCruel, "Take reward", A2_Q1_SharpAndCruel.TakeReward, null) : CreateQuestHander(sharpAndCruel, "Kill Weaver", A2_Q1_SharpAndCruel.KillWeaver, A2_Q1_SharpAndCruel.Tick);
			}
			CompletedQuests.Instance.Add(sharpAndCruel);
			return QuestHandler.QuestAddedToCache;
		}
		return CreateTravelHander(sharpAndCruel, town, act);
	}

	public static int GetState(DatQuestWrapper quest)
	{
		return GetState(quest.Id);
	}

	public static int GetState(string questId)
	{
		DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate(questId);
		if (currentQuestStateAccurate != null)
		{
			return currentQuestStateAccurate.Id;
		}
		return 255;
	}

	public static int GetStateInaccurate(DatQuestWrapper quest)
	{
		DatQuestStateWrapper currentQuestState = InGameState.GetCurrentQuestState(quest.Id);
		if (currentQuestState == null)
		{
			return 255;
		}
		return currentQuestState.Id;
	}

	public static void UpdateGuiAndLog(string questName, string questState)
	{
		Config.CurrentQuestName = questName;
		Config.CurrentQuestState = questState;
		GlobalLog.Warn((int_0 != 25500) ? $"[QuestManager] {questName} {int_0} - {questState}" : ("[QuestManager] " + questName + " - " + questState));
	}

	private static bool QuestIsNotCompleted(DatQuestWrapper quest, int completedState = 0)
	{
		if (!dictionary_0.TryGetValue(quest.Id, out var value))
		{
			GlobalLog.Error("[QuestManager] Quest state dictionary does not contain \"" + quest.Id + "\".");
			ErrorManager.ReportCriticalError();
		}
		else
		{
			DatQuestStateWrapper value2 = value.Value;
			if (value2 != null && value2.Id <= completedState)
			{
				return false;
			}
		}
		if (!CompletedQuests.Instance.Contains(quest))
		{
			return true;
		}
		return false;
	}

	private static bool IsWaypointOpened(AreaInfo area)
	{
		return dictionary_1.ContainsKey(area.Id);
	}

	private static QuestHandler CreateQuestHander(DatQuestWrapper quest, string state, Func<Task<bool>> execute, Action tick)
	{
		ClearStatesAndWaypoints();
		int questIndex = Quests.All.FindIndex((DatQuestWrapper q) => q.Id == quest.Id);
		QuestHandler grindingHandler = GetGrindingHandler(questIndex);
		if (grindingHandler != null)
		{
			return grindingHandler;
		}
		UpdateGuiAndLog(quest.Name, state);
		return new QuestHandler(execute, tick);
	}

	private static QuestHandler GetGrindingHandler(int questIndex)
	{
		int level = ((Player)LokiPoe.Me).Level;
		foreach (QuestBotSettings.GrindingRule grindingRule_0 in Config.GrindingRules)
		{
			if (level < grindingRule_0.LevelCap)
			{
				int num = Quests.All.FindIndex((DatQuestWrapper q) => q.Id == grindingRule_0.Quest.Id);
				if (num < questIndex)
				{
					GrindingHandler.SetGrindingRule(grindingRule_0);
					return new QuestHandler(GrindingHandler.Execute, null);
				}
			}
		}
		return null;
	}

	private static QuestHandler CreateTravelHander(DatQuestWrapper quest, AreaInfo area, int act)
	{
		ClearStatesAndWaypoints();
		UpdateGuiAndLog(quest.Name, $"Travel to Act {act}");
		return new QuestHandler(() => TravelTo(area), null);
	}

	private static void UpdateStatesAndWaypoints()
	{
		dictionary_0 = InGameState.GetCurrentQuestStates();
		dictionary_1 = InstanceInfo.AvailableWaypoints;
	}

	private static void ClearStatesAndWaypoints()
	{
		dictionary_0 = null;
		dictionary_1 = null;
	}

	private static async Task<bool> TravelTo(AreaInfo area)
	{
		if (!area.IsCurrentArea)
		{
			await Travel.To(area);
			return true;
		}
		return false;
	}

	private static QuestHandler AnalyzeBandits2()
	{
		DatQuestWrapper dealWithBandits = Quests.DealWithBandits;
		string rewardForQuest = Config.GetRewardForQuest(dealWithBandits.Id);
		if (!(rewardForQuest != "Kraityn") || Helpers.PlayerHasQuestItem("DexAmulet"))
		{
			if (rewardForQuest != "Alira" && !Helpers.PlayerHasQuestItem("IntAmulet"))
			{
				return CreateQuestHander(dealWithBandits, "Kill Alira", A2_Q6_DealWithBandits.KillAlira, A2_Q6_DealWithBandits.Tick);
			}
			if (!(rewardForQuest != "Oak") || Helpers.PlayerHasQuestItem("StrAmulet"))
			{
				return rewardForQuest switch
				{
					"Kraityn" => CreateQuestHander(dealWithBandits, "Help Kraityn", A2_Q6_DealWithBandits.HelpKraityn, A2_Q6_DealWithBandits.Tick), 
					"Oak" => CreateQuestHander(dealWithBandits, "Help Oak", A2_Q6_DealWithBandits.HelpOak, A2_Q6_DealWithBandits.Tick), 
					"Alira" => CreateQuestHander(dealWithBandits, "Help Alira", A2_Q6_DealWithBandits.HelpAlira, A2_Q6_DealWithBandits.Tick), 
					"Eramir" => CreateQuestHander(dealWithBandits, "Help Eramir", A2_Q6_DealWithBandits.HelpEramir, A2_Q6_DealWithBandits.Tick), 
					_ => null, 
				};
			}
			return CreateQuestHander(dealWithBandits, "Kill Oak", A2_Q6_DealWithBandits.KillOak, A2_Q6_DealWithBandits.Tick);
		}
		return CreateQuestHander(dealWithBandits, "Kill Kraityn", A2_Q6_DealWithBandits.KillKraityn, A2_Q6_DealWithBandits.Tick);
	}

	static QuestManager()
	{
		int_0 = 25500;
		Config = QuestBotSettings.Instance;
	}
}
