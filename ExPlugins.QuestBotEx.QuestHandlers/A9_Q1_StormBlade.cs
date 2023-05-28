using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A9_Q1_StormBlade
{
	private static NetworkObject StormChest => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act9/MummyEventChest");

	private static CachedObject CachedStormChest
	{
		get
		{
			return CombatAreaCache.Current.Storage["StormChest"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["StormChest"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act9.VastiriDesert.IsCurrentArea && CachedStormChest == null)
		{
			NetworkObject stormChest = StormChest;
			if (stormChest != (NetworkObject)null)
			{
				CachedStormChest = new CachedObject(stormChest);
			}
		}
	}

	public static async Task<bool> GrabStormBlade()
	{
		if (!Helpers.PlayerHasQuestItem("StormSword"))
		{
			if (World.Act9.VastiriDesert.IsCurrentArea)
			{
				CachedObject chest = CachedStormChest;
				if (chest != null)
				{
					WalkablePosition pos = chest.Position;
					if (pos.IsFar || pos.IsFarByPath)
					{
						pos.Come();
						return true;
					}
					NetworkObject networkObject_0 = chest.Object;
					if (!networkObject_0.IsTargetable)
					{
						Monster mob = Helpers.ClosestActiveMob;
						if (!((NetworkObject)(object)mob != (NetworkObject)null) || !((NetworkObject)(object)mob).PathExists())
						{
							GlobalLog.Debug("Waiting for any active monster");
							await Wait.StuckDetectionSleep(500);
							return true;
						}
						PlayerMoverManager.MoveTowards(((NetworkObject)mob).Position, (object)null);
						return true;
					}
					if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Storm Chest interaction")))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act9.VastiriDesert);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.PetarusAndVanjaA9, "Storm Blade Reward", Quests.StormBlade.Id);
	}
}
