using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q7_RavenousGod
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static readonly WalkablePosition walkablePosition_0;

	private static readonly WalkablePosition walkablePosition_1;

	private static NetworkObject networkObject_0;

	private static Monster monster_0;

	private static Monster monster_1;

	private static bool bool_0;

	private static Npc Bannon => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)98 }).FirstOrDefault<Npc>();

	private static Npc LillyRoth => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)100 }).FirstOrDefault<Npc>();

	private static Chest Tomb => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2574 }).FirstOrDefault<Chest>();

	private static CachedObject CachedTomb
	{
		get
		{
			return CombatAreaCache.Current.Storage["FirstTemplarTomb"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["FirstTemplarTomb"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = World.Act5.CathedralRooftop.IsWaypointOpened;
		if (World.Act5.Ossuary.IsCurrentArea && CachedTomb == null)
		{
			Chest tomb = Tomb;
			if ((NetworkObject)(object)tomb != (NetworkObject)null)
			{
				CachedTomb = new CachedObject((NetworkObject)(object)tomb);
			}
		}
	}

	public static async Task<bool> TalkToBannonAndGetToSquare()
	{
		if (World.Act5.RuinedSquare.IsCurrentArea)
		{
			return false;
		}
		if (World.Act5.ChamberOfInnocence.IsCurrentArea)
		{
			Npc bannon = Bannon;
			if ((NetworkObject)(object)bannon != (NetworkObject)null && ((NetworkObject)bannon).HasNpcFloatingIcon)
			{
				WalkablePosition bannonPos = ((NetworkObject)(object)bannon).WalkablePosition();
				if (bannonPos.IsFar)
				{
					bannonPos.Come();
					return true;
				}
				await Helpers.TalkTo((NetworkObject)(object)bannon);
				return true;
			}
		}
		await Travel.To(World.Act5.RuinedSquare);
		return true;
	}

	public static async Task<bool> GrabSignOfPurity()
	{
		if (!Helpers.PlayerHasQuestItem("KitavaKey"))
		{
			if (World.Act5.Ossuary.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedTomb))
				{
					return true;
				}
				tgtPosition_0.Come();
				return true;
			}
			await Travel.To(World.Act5.Ossuary);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillKitava()
	{
		if (!bool_0)
		{
			if (World.Act5.CathedralRooftop.IsCurrentArea)
			{
				UpdateKitavaFightObjects();
				if (networkObject_0 != (NetworkObject)null && networkObject_0.PathExists())
				{
					if (!networkObject_0.IsTargetable)
					{
						if (!((NetworkObject)(object)monster_1 != (NetworkObject)null) || !((NetworkObject)monster_1).IsTargetable)
						{
							if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
							{
								if (monster_0.IsActive)
								{
									walkablePosition_0.Come();
								}
								else
								{
									await Helpers.MoveAndWait(walkablePosition_0, "Waiting for Kitava, the Insatiable");
								}
							}
							return true;
						}
						await Helpers.MoveAndWait(((NetworkObject)(object)monster_1).WalkablePosition());
						return true;
					}
					await networkObject_0.WalkablePosition().ComeAtOnce();
					if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Cradle of Purity interaction")))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				await Helpers.MoveAndTakeLocalTransition(tgtPosition_1);
				return true;
			}
			await Travel.To(World.Act5.CathedralRooftop);
			return true;
		}
		return false;
	}

	public static async Task<bool> SailToWraeclast()
	{
		if (!World.Act6.LioneyeWatch.IsCurrentArea)
		{
			if (World.Act5.CathedralRooftop.IsCurrentArea)
			{
				if (!walkablePosition_1.PathExists)
				{
					GlobalLog.Debug("Waiting for Kitava fight ending");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				await walkablePosition_1.ComeAtOnce();
				Npc lilly = LillyRoth;
				if ((NetworkObject)(object)lilly == (NetworkObject)null)
				{
					GlobalLog.Error("[SailToWraeclast] We are near Lilly Roth position but NPC object is null.");
					await Wait.StuckDetectionSleep(500);
					return true;
				}
				uint hash = LocalData.AreaHash;
				if (!(await ((NetworkObject)(object)lilly).AsTownNpc().Converse("Sail to Wraeclast")))
				{
					ErrorManager.ReportError();
					return true;
				}
				await Coroutines.CloseBlockingWindows();
				await Wait.ForAreaChange(hash);
				return true;
			}
			await Travel.To(World.Act5.CathedralRooftop);
			return true;
		}
		if (World.Act6.LioneyeWatch.IsWaypointOpened)
		{
			return false;
		}
		if (!(await PlayerAction.OpenWaypoint()))
		{
			ErrorManager.ReportError();
		}
		return true;
	}

	private static void UpdateKitavaFightObjects()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		networkObject_0 = null;
		monster_0 = null;
		monster_1 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && (int)val.Rarity == 3)
			{
				string name = ((NetworkObject)val).Name;
				if (!(name == "Kitava, the Insatiable"))
				{
					if (name == "Kitava's Heart")
					{
						monster_1 = val;
					}
				}
				else
				{
					monster_0 = val;
				}
			}
			else if (@object.Metadata == "Metadata/Terrain/Act5/Area8/Objects/ArenaSocket")
			{
				networkObject_0 = @object;
			}
		}
	}

	static A5_Q7_RavenousGod()
	{
		tgtPosition_0 = new TgtPosition("Tomb of the First Templar location", "ossuary_quest_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Kitava room", "cathedralroof_boss_area_v02_01.tgt");
		walkablePosition_0 = new WalkablePosition("Walkable position in front of Kitava", 1730, 3105);
		walkablePosition_1 = new WalkablePosition("Lilly Roth", 704, 2543);
	}
}
