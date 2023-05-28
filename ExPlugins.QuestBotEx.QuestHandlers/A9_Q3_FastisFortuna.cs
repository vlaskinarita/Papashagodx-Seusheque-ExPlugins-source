using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A9_Q3_FastisFortuna
{
	private static Monster Boulderback => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1645 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedBoulderbackPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["BoulderbackPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["BoulderbackPosition"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act9.Foothills.IsCurrentArea)
		{
			Monster boulderback = Boulderback;
			if ((NetworkObject)(object)boulderback != (NetworkObject)null)
			{
				CachedBoulderbackPos = ((!((Actor)boulderback).IsDead) ? ((NetworkObject)(object)boulderback).WalkablePosition() : null);
			}
		}
	}

	public static async Task<bool> GrabCalendar()
	{
		if (!Helpers.PlayerHasQuestItem("Calendar"))
		{
			if (World.Act9.Foothills.IsCurrentArea)
			{
				WalkablePosition boulderPos = CachedBoulderbackPos;
				if (boulderPos != null)
				{
					boulderPos.Come();
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act9.Foothills);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.PetarusAndVanjaA9, "Maraketh Calendar Reward", null, "Book-a9q4");
	}
}
