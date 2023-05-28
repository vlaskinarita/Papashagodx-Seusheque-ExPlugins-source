using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A8_Q2_LoveIsDead
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Chest SealedCasket => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2583 }).FirstOrDefault<Chest>();

	private static Npc Clarissa => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)20 }).FirstOrDefault<Npc>();

	private static Monster Tolman => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)105 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static CachedObject CachedSealedCasket
	{
		get
		{
			return CombatAreaCache.Current.Storage["SealedCasket"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["SealedCasket"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.LoveIsDead) <= 3;
		if (World.Act8.Quay.IsCurrentArea && CachedSealedCasket == null)
		{
			Chest sealedCasket = SealedCasket;
			if ((NetworkObject)(object)sealedCasket != (NetworkObject)null)
			{
				CachedSealedCasket = new CachedObject((NetworkObject)(object)sealedCasket);
			}
		}
	}

	public static async Task<bool> GrabAnkh()
	{
		if (!Helpers.PlayerHasQuestItem("AnkhOfEternity"))
		{
			if (World.Act8.Quay.IsCurrentArea)
			{
				if (await Helpers.OpenQuestChest(CachedSealedCasket))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act8.Quay);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillTolman()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act8.Quay.IsCurrentArea)
		{
			Npc clarissa = Clarissa;
			if ((NetworkObject)(object)clarissa != (NetworkObject)null)
			{
				if (!((NetworkObject)clarissa).IsTargetable || !((NetworkObject)clarissa).HasNpcFloatingIcon)
				{
					Monster tolman = Tolman;
					if ((NetworkObject)(object)tolman != (NetworkObject)null)
					{
						await Helpers.MoveToBossOrAnyMob(Tolman);
						return true;
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)clarissa).WalkablePosition(), "Waiting for any Tolman fight object");
					return true;
				}
				await Helpers.TalkTo((NetworkObject)(object)clarissa);
				return true;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
			return true;
		}
		await Travel.To(World.Act8.Quay);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act8.SarnEncampment, TownNpcs.townNpc_1, "Tolman Reward", null, "Book-a8q6");
	}

	static A8_Q2_LoveIsDead()
	{
		tgtPosition_0 = new TgtPosition("Tolman room", "market_transition_warehouse_v01_01.tgt", closest: true);
	}
}
