using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.Tasks;

public class SpecialObjectTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static readonly List<CachedObject> list_0;

	private static bool bool_0;

	private static CachedObject cachedObject_0;

	private static Func<Task> func_0;

	public static int LabCounter;

	public static string LastInteractedName;

	public static string LastInteractedMetadata;

	public static bool RitualStarted;

	private static readonly HashSet<string> hashSet_0;

	public string Name => "SpecialObjectTask";

	public string Description => "Task that handles objects specific to certain maps.";

	public string Author => "ExVault";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		if (!bool_0 || MapExplorationTask.MapCompleted || !World.CurrentArea.IsMap)
		{
			return false;
		}
		if (SpecialObjectTask.cachedObject_0 == null && (SpecialObjectTask.cachedObject_0 = list_0.ClosestValid()) == null)
		{
			return false;
		}
		WalkablePosition pos = SpecialObjectTask.cachedObject_0.Position;
		if (pos.IsFar || pos.IsFarByPath)
		{
			if (!pos.TryCome())
			{
				GlobalLog.Error($"[SpecialObjectTask] Fail to move to {pos}. Marking this special object as unwalkable.");
				SpecialObjectTask.cachedObject_0.Unwalkable = true;
				SpecialObjectTask.cachedObject_0 = null;
			}
			return true;
		}
		NetworkObject networkObject_0 = SpecialObjectTask.cachedObject_0.Object;
		if (networkObject_0 == (NetworkObject)null || !networkObject_0.IsTargetable)
		{
			list_0.Remove(SpecialObjectTask.cachedObject_0);
			SpecialObjectTask.cachedObject_0 = null;
			return true;
		}
		string name = SpecialObjectTask.cachedObject_0.Position.Name;
		if (++SpecialObjectTask.cachedObject_0.InteractionAttempts <= 5)
		{
			bool shouldResetCombatTargeting = networkObject_0.Metadata.Contains("RitualRuneInteractable");
			await PlayerAction.DisableAlwaysHighlight();
			CachedObject cachedObject_0 = new CachedObject(networkObject_0);
			string objName = networkObject_0.Name;
			string objMetadata = networkObject_0.Metadata;
			if (!(await PlayerAction.Interact(networkObject_0, () => cachedObject_0.Object == (NetworkObject)null || !networkObject_0.Fresh<NetworkObject>().IsTargetable, "\"" + name + "\" interaction", 1000)))
			{
				await Wait.SleepSafe(500);
			}
			else
			{
				LastInteractedName = objName;
				LastInteractedMetadata = objMetadata;
				if (func_0 != null)
				{
					await func_0();
				}
				if (shouldResetCombatTargeting)
				{
					await Wait.SleepSafe(500);
					RitualStarted = true;
					ResetCombatTargeting();
				}
			}
			if (name.Contains("LegionInitiator") || name.Contains("RitualRuneInteractable"))
			{
				await Wait.Sleep(800);
			}
			return true;
		}
		GlobalLog.Error("[SpecialObjectTask] All attempts to interact with \"" + name + "\" have been spent. Now ignoring it.");
		SpecialObjectTask.cachedObject_0.Ignored = true;
		SpecialObjectTask.cachedObject_0 = null;
		return true;
	}

	public void Tick()
	{
		if (!bool_0 || MapExplorationTask.MapCompleted || !interval_0.Elapsed || !LokiPoe.IsInGame || !World.CurrentArea.IsMap)
		{
			return;
		}
		foreach (NetworkObject networkObject_0 in ObjectManager.Objects)
		{
			if (!hashSet_0.Any((string i) => networkObject_0.Metadata.Contains(i)))
			{
				continue;
			}
			int int_0 = networkObject_0.Id;
			CachedObject cachedObject = list_0.Find((CachedObject s) => s.Id == int_0);
			if (networkObject_0.IsTargetable)
			{
				if (cachedObject == null)
				{
					WalkablePosition walkablePosition = networkObject_0.WalkablePosition(5, 20);
					list_0.Add(new CachedObject(networkObject_0.Id, walkablePosition));
					GlobalLog.Debug($"[SpecialObjectTask] Registering {walkablePosition}");
				}
			}
			else if (cachedObject != null)
			{
				if (cachedObject == cachedObject_0)
				{
					cachedObject_0 = null;
				}
				list_0.Remove(cachedObject);
			}
		}
	}

	public MessageResult Message(Message message)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		string id = message.Id;
		if (id == "MB_new_map_entered_event")
		{
			GlobalLog.Info("[SpecialObjectTask] Reset.");
			Reset(message.GetInput<string>(0));
			LabCounter = 0;
			if (bool_0)
			{
				GlobalLog.Info("[SpecialObjectTask] Enabled.");
			}
			return (MessageResult)0;
		}
		if (id == "explorer_local_transition_entered_event")
		{
			GlobalLog.Info("[SpecialObjectTask] Resetting unwalkable flags.");
			foreach (CachedObject item in list_0)
			{
				item.Unwalkable = false;
			}
			return (MessageResult)0;
		}
		return (MessageResult)1;
	}

	public void Start()
	{
		if (GeneralSettings.Instance.StartLegions)
		{
			hashSet_0.Add("Metadata/Terrain/Leagues/Legion/Objects/LegionInitiator");
		}
		else
		{
			hashSet_0.Remove("Metadata/Terrain/Leagues/Legion/Objects/LegionInitiator");
		}
		if (GeneralSettings.Instance.PickSulphite)
		{
			hashSet_0.Add("Metadata/Terrain/Leagues/Delve/Objects/DelveMineral");
		}
		else
		{
			hashSet_0.Remove("Metadata/Terrain/Leagues/Delve/Objects/DelveMineral");
		}
	}

	private void ResetCombatTargeting()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		IRoutine current = RoutineManager.Current;
		Message val = new Message("ResetCombatTargeting", (object)this);
		((IMessageHandler)current).Message(val);
	}

	private static void Reset(string areaName)
	{
		bool_0 = false;
		cachedObject_0 = null;
		func_0 = null;
		list_0.Clear();
		if (areaName == MapNames.OlmecSanctum)
		{
			bool_0 = true;
			func_0 = OlmecPostInteraction;
		}
		else if (!(areaName == MapNames.MaoKun))
		{
			if (!(areaName == MapNames.Laboratory))
			{
				if (World.CurrentArea.IsMap)
				{
					GlobalLog.Info("[SpecialObjectTask] Enabled.");
					bool_0 = true;
				}
			}
			else
			{
				GlobalLog.Info("[SpecialObjectTask] Special handling for Laboratory.");
				bool_0 = true;
				func_0 = LaboratoryPostInteraction;
			}
		}
		else
		{
			bool_0 = true;
			func_0 = MaoKunPostInteraction;
		}
	}

	private static Task LaboratoryPostInteraction()
	{
		if (LastInteractedMetadata.Contains("Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory") && !LastInteractedMetadata.Contains("Once_Laboratory_Counter"))
		{
			LabCounter++;
			if (LabCounter >= 4)
			{
				hashSet_0.Add("Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_Counter");
				GlobalLog.Warn($"[SpecialObjectTask] Interacted with {LabCounter} levers so far. Procceed boss lever.");
			}
			else
			{
				GlobalLog.Warn($"[SpecialObjectTask] Interacted with {LabCounter} {LastInteractedName}s so far. Procceed to next.");
			}
		}
		return Task.CompletedTask;
	}

	private static async Task OlmecPostInteraction()
	{
		await Wait.For(() => CombatAreaCache.Current.AreaTransitions.Any((CachedTransition a) => a.Position.Name == MapNames.OlmecSanctum && a.Position.Distance < 200), "area transition activation");
	}

	private static Task MaoKunPostInteraction()
	{
		GlobalLog.Warn("[SpecialObjectTask] Fairgraves has been interacted. Now resetting the Explorer.");
		CombatAreaCache.Current.Explorer.BasicExplorer.Reset();
		return Task.CompletedTask;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Stop()
	{
	}

	static SpecialObjectTask()
	{
		interval_0 = new Interval(200);
		list_0 = new List<CachedObject>();
		hashSet_0 = new HashSet<string>
		{
			"Metadata/Effects/Environment/artifacts/Gaius/ObjectiveTablet", "Metadata/Effects/Environment/artifacts/Gaius/TimeTablet", "Metadata/Terrain/EndGame/MapDesert/Objects/MummyEventChest", "Metadata/Monsters/Doedre/DoedreSewer/DoedreCauldronValve", "Metadata/Terrain/EndGame/MapShipGraveyardCagan/Objects/IncaReleaseBall", "Metadata/Terrain/EndGame/MapIncaUniqueLegends/Objects/LegendsGlyph1", "Metadata/Terrain/EndGame/MapIncaUniqueLegends/Objects/LegendsGlyph2", "Metadata/Terrain/EndGame/MapIncaUniqueLegends/Objects/LegendsGlyph3", "Metadata/Terrain/EndGame/MapIncaUniqueLegends/Objects/LegendsGlyph4", "Metadata/Terrain/EndGame/MapIncaUniqueLegends/Objects/LegendsGlyphMain",
			"Metadata/Terrain/EndGame/MapTreasureIsland/Objects/FairgravesTreasureIsland", "Metadata/Terrain/Missions/CraftingUnlocks/RecipeUnlockMap", "Metadata/NPC/Missions/Wild/StrDexIntQuest", "Metadata/Terrain/EndGame/MapGraveyard/Objects/GraveyardAltar", "Metadata/Terrain/EndGame/MapInnerTemple/Objects/BossFightInitiator", "Metadata/Terrain/EndGame/MapBelfry/Objects/ArenaSocket", "LegionChests", "Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch_Once", "Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_0", "Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_1",
			"Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_2", "Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_3", "Metadata/Terrain/EndGame/MapLaboratory/Objects/Switch_Once_Laboratory_4", "Metadata/MiscellaneousObjects/Metamorphosis/MetamorphosisMonsterMarker", "Metadata/Terrain/EndGame/MapSwampFetid/Objects/AngeredBird", "Metadata/Terrain/EndGame/MapSaltFlats/Objects/AngeredBird", "Metadata/Terrain/EndGame/MapAtlasMaven/Objects/MavenBossRushObject", "Metadata/Terrain/EndGame/MapShipGraveyardCagan/Objects/Teleport"
		};
	}
}
