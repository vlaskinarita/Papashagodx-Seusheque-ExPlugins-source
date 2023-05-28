using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A8_Q4_WingsOfVastiri
{
	private static Monster Hector => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2257 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Chest HectorTomb => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2584 }).FirstOrDefault<Chest>();

	private static WalkablePosition CachedHectorPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["HectorPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["HectorPosition"] = value;
		}
	}

	private static CachedObject CachedHectorTomb
	{
		get
		{
			return CombatAreaCache.Current.Storage["HectorTomb"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["HectorTomb"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act8.BathHouse.IsCurrentArea)
		{
			return;
		}
		if (CachedHectorTomb == null)
		{
			Chest hectorTomb = HectorTomb;
			if ((NetworkObject)(object)hectorTomb != (NetworkObject)null)
			{
				CachedHectorTomb = new CachedObject((NetworkObject)(object)hectorTomb);
			}
		}
		Monster hector = Hector;
		if ((NetworkObject)(object)hector != (NetworkObject)null)
		{
			CachedHectorPos = ((!((Actor)hector).IsDead) ? ((NetworkObject)(object)hector).WalkablePosition() : null);
		}
	}

	public static async Task<bool> GrabWings()
	{
		if (!Helpers.PlayerHasQuestItem("WingsOfVastiri"))
		{
			if (World.Act8.BathHouse.IsCurrentArea)
			{
				WalkablePosition hectorPos = CachedHectorPos;
				if (hectorPos != null)
				{
					hectorPos.Come();
					return true;
				}
				if (await Helpers.OpenQuestChest(CachedHectorTomb))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act8.BathHouse);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act8.SarnEncampment, TownNpcs.HarganA8, "Wings of Vastiri Reward", Quests.WingsOfVastiri.Id);
	}
}
