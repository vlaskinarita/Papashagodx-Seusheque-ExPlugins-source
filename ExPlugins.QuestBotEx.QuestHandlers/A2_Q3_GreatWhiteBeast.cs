using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q3_GreatWhiteBeast
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster GreatWhiteBeast => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)199 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedWhiteBeastPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["WhiteBeastPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["WhiteBeastPosition"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.GreatWhiteBeast) <= 2;
		if (World.Act2.Den.IsCurrentArea)
		{
			Monster greatWhiteBeast = GreatWhiteBeast;
			if ((NetworkObject)(object)greatWhiteBeast != (NetworkObject)null)
			{
				CachedWhiteBeastPos = ((NetworkObject)(object)greatWhiteBeast).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillWhiteBeast()
	{
		if (!bool_0)
		{
			if (World.Act2.Den.IsCurrentArea)
			{
				WalkablePosition beastPos = CachedWhiteBeastPos;
				if (beastPos != null)
				{
					await Helpers.MoveAndWait(beastPos);
					return true;
				}
				if (!tgtPosition_0.IsFar)
				{
					tgtPosition_0.ProceedToNext();
				}
				else
				{
					tgtPosition_0.Come();
				}
				return true;
			}
			await Travel.To(World.Act2.Den);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (!World.Act2.Den.IsCurrentArea)
		{
			if (World.Act2.OldFields.IsCurrentArea)
			{
				await Travel.To(World.Act2.Crossroads);
				return true;
			}
			return await Helpers.TakeQuestReward(World.Act2.ForestEncampment, TownNpcs.Yeena, "Great White Beast Reward", Quests.GreatWhiteBeast.Id);
		}
		AreaTransition transition = ObjectManager.Objects.FirstOrDefault((AreaTransition a) => a.LeadsTo(World.Act2.OldFields));
		if ((NetworkObject)(object)transition != (NetworkObject)null)
		{
			if (!(await PlayerAction.TakeTransition(transition)))
			{
				ErrorManager.ReportError();
			}
		}
		else if (!(await PlayerAction.TpToTown()))
		{
			ErrorManager.ReportError();
		}
		return true;
	}

	static A2_Q3_GreatWhiteBeast()
	{
		tgtPosition_0 = new TgtPosition("Den exit", "forestcaveup_exit_v01_01.tgt");
	}
}
