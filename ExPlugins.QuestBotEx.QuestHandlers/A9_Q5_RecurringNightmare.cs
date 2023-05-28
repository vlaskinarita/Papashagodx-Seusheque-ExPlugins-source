using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A9_Q5_RecurringNightmare
{
	private static Npc npc_0;

	private static Npc npc_1;

	private static Monster monster_0;

	private static AreaTransition areaTransition_0;

	private static bool bool_0;

	private static Monster Basilisk => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2277 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster Adus => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2291 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Chest Theurgic => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2585 }).FirstOrDefault<Chest>();

	private static bool IsFightEnded
	{
		get
		{
			Npc val = ObjectManager.Objects.FirstOrDefault((Npc n) => ((NetworkObject)n).Metadata == "Metadata/NPC/Act9/Lilly");
			return (NetworkObject)(object)val != (NetworkObject)null && ((NetworkObject)(object)val).PathExists();
		}
	}

	private static WalkablePosition CachedBasiliskPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["BasiliskPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["BasiliskPosition"] = value;
		}
	}

	private static WalkablePosition CachedAdusPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["AdusPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["AdusPosition"] = value;
		}
	}

	private static CachedObject CachedTheurgic
	{
		get
		{
			return CombatAreaCache.Current.Storage["Theurgic"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Theurgic"] = value;
		}
	}

	public static bool HaveAnyIngredient => Inventories.InventoryItems.Exists((Item i) => i.Class == "QuestItem" && i.Metadata.Contains("Ingredient"));

	public static void Tick()
	{
		if (World.Act9.BoilingLake.IsCurrentArea)
		{
			Monster basilisk = Basilisk;
			if ((NetworkObject)(object)basilisk != (NetworkObject)null)
			{
				CachedBasiliskPos = (((Actor)basilisk).IsDead ? null : ((NetworkObject)(object)basilisk).WalkablePosition());
			}
		}
		if (!World.Act9.Refinery.IsCurrentArea)
		{
			return;
		}
		if (CachedTheurgic == null)
		{
			Chest theurgic = Theurgic;
			if ((NetworkObject)(object)theurgic != (NetworkObject)null)
			{
				CachedTheurgic = new CachedObject((NetworkObject)(object)theurgic);
			}
		}
		Monster adus = Adus;
		if ((NetworkObject)(object)adus != (NetworkObject)null)
		{
			CachedAdusPos = ((!((Actor)adus).IsDead) ? ((NetworkObject)(object)adus).WalkablePosition() : null);
		}
	}

	public static async Task<bool> GrabAcid()
	{
		if (Helpers.PlayerHasQuestItem("Ingredient2"))
		{
			return false;
		}
		if (World.Act9.BoilingLake.IsCurrentArea)
		{
			WalkablePosition basiliskPos = CachedBasiliskPos;
			if (basiliskPos != null)
			{
				basiliskPos.Come();
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act9.BoilingLake);
		return true;
	}

	public static async Task<bool> GrabPowder()
	{
		if (Helpers.PlayerHasQuestItem("Ingredient1"))
		{
			return false;
		}
		if (World.Act9.Refinery.IsCurrentArea)
		{
			WalkablePosition adusPos = CachedAdusPos;
			if (adusPos != null)
			{
				adusPos.Come();
				return true;
			}
			if (await Helpers.OpenQuestChest(CachedTheurgic))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act9.Refinery);
		return true;
	}

	public static async Task<bool> TurnInIngredient()
	{
		if (HaveAnyIngredient)
		{
			if (World.Act9.Highgate.IsCurrentArea)
			{
				if (!(await TownNpcs.SinA9.Talk()))
				{
					ErrorManager.ReportError();
				}
				else
				{
					await Coroutines.CloseBlockingWindows();
				}
				return true;
			}
			await Travel.To(World.Act9.Highgate);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillTrinity()
	{
		if (!World.Act9.RottingCore.IsCurrentArea)
		{
			if (World.Act10.OriathDocks.IsCurrentArea)
			{
				if (World.Act10.OriathDocks.IsWaypointOpened)
				{
					return false;
				}
				if (!(await PlayerAction.OpenWaypoint()))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			await Travel.To(World.Act9.RottingCore);
			return true;
		}
		UpdateTrinityObjects();
		if ((NetworkObject)(object)npc_1 != (NetworkObject)null && ((NetworkObject)(object)npc_1).PathExists())
		{
			uint hash = LocalData.AreaHash;
			if (!(await ((NetworkObject)(object)npc_1).AsTownNpc().Converse("Sail to Oriath")))
			{
				ErrorManager.ReportError();
				return true;
			}
			await Coroutines.CloseBlockingWindows();
			await Wait.ForAreaChange(hash);
			return true;
		}
		if (!bool_0)
		{
			if (!((NetworkObject)(object)areaTransition_0 != (NetworkObject)null) || !((NetworkObject)areaTransition_0).IsTargetable)
			{
				if (!((NetworkObject)(object)npc_0 != (NetworkObject)null) || !((NetworkObject)npc_0).HasNpcFloatingIcon)
				{
					if ((NetworkObject)(object)monster_0 != (NetworkObject)null && ((NetworkObject)(object)monster_0).PathExists())
					{
						await Helpers.MoveAndWait((NetworkObject)(object)monster_0);
						return true;
					}
					await Helpers.Explore();
					return true;
				}
				if (!(await ((NetworkObject)(object)npc_0).AsTownNpc().Talk()))
				{
					ErrorManager.ReportError();
					return true;
				}
				await Coroutines.CloseBlockingWindows();
				await Wait.SleepSafe(500);
				AreaTransition t = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => ((NetworkObject)a).Metadata.Contains("UnholyTrioSubBossPortal")));
				if ((NetworkObject)(object)t != (NetworkObject)null && !((NetworkObject)t).IsTargetable)
				{
					await Wait.For(() => ((NetworkObject)npc_0.Fresh<Npc>()).HasNpcFloatingIcon, "Next Sin's dialogue", 500, 7000);
				}
				else
				{
					int state = QuestManager.GetStateInaccurate(Quests.RecurringNightmare);
					if (state == 4 || state == 5)
					{
						await Wait.For(() => ((NetworkObject)areaTransition_0.Fresh<AreaTransition>()).IsTargetable, "Black Heart activation", 500, 10000);
					}
				}
				return true;
			}
			if (!(await PlayerAction.TakeTransition(areaTransition_0)))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		if (!(await Wait.For(() => IsFightEnded, "Waiting for Depraved Trinity fight ending", 500, 7000)))
		{
			ErrorManager.ReportError();
			if (World.Act10.OriathDocks.IsWaypointOpened)
			{
				return false;
			}
		}
		return true;
	}

	private static void UpdateTrinityObjects()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		npc_0 = null;
		npc_1 = null;
		monster_0 = null;
		areaTransition_0 = null;
		bool_0 = false;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			string metadata = @object.Metadata;
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && (int)val.Rarity == 3 && (int)((NetworkObject)val).Reaction > 0)
			{
				if (!((Actor)val).IsDead)
				{
					if ((!val.IsHidden || !metadata.Contains("Doedre/DoedreSoul")) && !((NetworkObject)val).Metadata.Contains("LegionLeague") && !((NetworkObject)val).Metadata.Contains("LeagueBetrayal") && !((NetworkObject)val).Metadata.Contains("Metadata/Monsters/Masters/BlightBuilderWild") && !((NetworkObject)val).Metadata.Contains("LeagueExpedition"))
					{
						monster_0 = val;
					}
				}
				else if (metadata == "Metadata/Monsters/UnholyTrio/UnholyTrio")
				{
					bool_0 = true;
				}
				continue;
			}
			AreaTransition val2 = (AreaTransition)(object)((@object is AreaTransition) ? @object : null);
			if (!((NetworkObject)(object)val2 != (NetworkObject)null) || !(metadata == "Metadata/QuestObjects/Act9/HarvestFinalBossTransition"))
			{
				Npc val3 = (Npc)(object)((@object is Npc) ? @object : null);
				if (!((NetworkObject)(object)val3 != (NetworkObject)null))
				{
					continue;
				}
				if (!(metadata == "Metadata/NPC/Act9/SinCore"))
				{
					if (metadata == "Metadata/NPC/Act9/Lilly")
					{
						npc_1 = val3;
					}
				}
				else
				{
					npc_0 = val3;
				}
			}
			else
			{
				areaTransition_0 = val2;
			}
		}
	}
}
