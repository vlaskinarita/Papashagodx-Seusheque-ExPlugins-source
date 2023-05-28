using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A3_Q4_SeverRightHand
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster Gravicius => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)702 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedGraviciusPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["GraviciusPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["GraviciusPosition"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.SeverRightHand) <= 1;
		if (World.Act3.EbonyBarracks.IsCurrentArea)
		{
			Monster gravicius = Gravicius;
			if ((NetworkObject)(object)gravicius != (NetworkObject)null)
			{
				CachedGraviciusPos = ((NetworkObject)(object)gravicius).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillGravicius()
	{
		if (!bool_0)
		{
			if (World.Act3.EbonyBarracks.IsCurrentArea)
			{
				WalkablePosition graviciusPos = CachedGraviciusPos;
				if (!(graviciusPos != null))
				{
					tgtPosition_0.Come();
					return true;
				}
				await Helpers.MoveAndWait(graviciusPos);
				return true;
			}
			await Travel.To(World.Act3.EbonyBarracks);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (!World.Act3.EbonyBarracks.IsCurrentArea)
		{
			return await Helpers.TakeQuestReward(World.Act3.SarnEncampment, TownNpcs.Maramoa, "Gravicius Reward", Quests.SeverRightHand.Id);
		}
		await Travel.To(World.Act3.LunarisTemple1);
		return true;
	}

	static A3_Q4_SeverRightHand()
	{
		tgtPosition_0 = new TgtPosition("General Gravicius location", "temple_carpet_oneside_01_01.tgt");
	}
}
