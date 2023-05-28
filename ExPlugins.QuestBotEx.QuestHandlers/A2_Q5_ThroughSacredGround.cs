using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q5_ThroughSacredGround
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Geofri => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)404 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Chest Altar => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2566 }).FirstOrDefault<Chest>();

	private static WalkablePosition CachedGeofriPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["GeofriPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["GeofriPosition"] = value;
		}
	}

	private static CachedObject CachedAltar
	{
		get
		{
			return CombatAreaCache.Current.Storage["Altar"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Altar"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act2.Crypt2.IsCurrentArea)
		{
			return;
		}
		Monster geofri = Geofri;
		if ((NetworkObject)(object)geofri != (NetworkObject)null)
		{
			CachedGeofriPos = (((Actor)geofri).IsDead ? null : ((NetworkObject)(object)geofri).WalkablePosition());
		}
		if (CachedAltar == null)
		{
			Chest altar = Altar;
			if ((NetworkObject)(object)altar != (NetworkObject)null)
			{
				CachedAltar = new CachedObject((NetworkObject)(object)altar);
			}
		}
	}

	public static async Task<bool> GrabGoldenHand()
	{
		if (Helpers.PlayerHasQuestItem("GoldenHand"))
		{
			return false;
		}
		if (World.Act2.Crypt2.IsCurrentArea)
		{
			WalkablePosition geofriPos = CachedGeofriPos;
			if (geofriPos != null)
			{
				await Helpers.MoveAndWait(geofriPos);
				return true;
			}
			if (!(await Helpers.OpenQuestChest(CachedAltar)))
			{
				tgtPosition_0.Come();
				return true;
			}
			return true;
		}
		await Travel.To(World.Act2.Crypt2);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act2.ForestEncampment.IsCurrentArea)
		{
			if (QuestManager.GetState(Quests.ThroughSacredGround) == 0)
			{
				return false;
			}
			if (Helpers.PlayerHasQuestItem("Book-a2q5"))
			{
				if (!(await Helpers.UseQuestItem("Book-a2q5")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			if (!(await TownNpcs.Yeena.OpenDialogPanel()))
			{
				ErrorManager.ReportError();
				return true;
			}
			if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Jewel Reward")))
			{
				if (!(await TownNpcs.Yeena.TakeReward(null, "Jewel Reward")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
			if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Fellshrine Reward")))
			{
				if (!(await TownNpcs.Yeena.TakeReward(null, "Fellshrine Reward")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
		}
		await Travel.To(World.Act2.ForestEncampment);
		return true;
	}

	static A2_Q5_ThroughSacredGround()
	{
		tgtPosition_0 = new TgtPosition("Altar location", "dungeon_church_relic_altar_v01_01.tgt");
	}
}
