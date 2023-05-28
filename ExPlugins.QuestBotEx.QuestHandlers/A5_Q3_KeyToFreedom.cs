using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q3_KeyToFreedom
{
	private static Monster Casticus => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1990 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedCasticusPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["CasticusPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["CasticusPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act5.ControlBlocks.IsCurrentArea)
		{
			Monster casticus = Casticus;
			if ((NetworkObject)(object)casticus != (NetworkObject)null)
			{
				CachedCasticusPos = ((NetworkObject)(object)casticus).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillCasticus()
	{
		if (Helpers.PlayerHasQuestItem("TemplarCourtKey"))
		{
			return false;
		}
		if (World.Act5.ControlBlocks.IsCurrentArea)
		{
			WalkablePosition casticusPos = CachedCasticusPos;
			if (casticusPos != null)
			{
				await Helpers.MoveAndWait(casticusPos);
				return true;
			}
		}
		await Travel.To(World.Act5.OriathSquare);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act5.ControlBlocks.IsCurrentArea)
		{
			await Travel.To(World.Act5.OriathSquare);
			return true;
		}
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Lani, "Casticus Reward", Quests.KeyToFreedom.Id);
	}
}
