using System;
using System.Collections;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A5_Q5_KingFeast
{
	private static bool bool_0;

	private static readonly WalkablePosition walkablePosition_0;

	private static Monster Utula => ((IEnumerable)ObjectManager.GetObjects(Array.Empty<PoeObjectEnum>())).Closest<Monster>((Func<Monster, bool>)((Monster m) => ((NetworkObject)m).Name.Equals("Utula, Stone and Steel") && (int)m.Rarity == 3 && m.IsAliveHostile));

	private static WalkablePosition CachedUtulaPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["UtulaPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["UtulaPosition"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.KingFeast) <= 3;
		if (World.Act5.RuinedSquare.IsCurrentArea)
		{
			Monster utula = Utula;
			if ((NetworkObject)(object)utula != (NetworkObject)null)
			{
				CachedUtulaPos = ((NetworkObject)(object)utula).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillUtula()
	{
		if (!bool_0)
		{
			if (World.Act5.RuinedSquare.IsCurrentArea)
			{
				WalkablePosition utulaPos = CachedUtulaPos;
				if (utulaPos != null)
				{
					await Helpers.MoveAndWait(utulaPos);
					return true;
				}
				await Helpers.MoveAndWait(walkablePosition_0);
				return true;
			}
			await Travel.To(World.Act5.RuinedSquare);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act5.OverseerTower, TownNpcs.Bannon, "Utula Reward", Quests.KingFeast.Id);
	}

	static A5_Q5_KingFeast()
	{
		walkablePosition_0 = new WalkablePosition("Utula location", 1369, 311);
	}
}
