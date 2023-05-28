using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A4_Q2_IndomitableSpirit
{
	private static bool bool_0;

	private static NetworkObject DeshretSpirit => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Act4/DeshretSpirit");

	private static CachedObject CachedDeshretSpirit
	{
		get
		{
			return CombatAreaCache.Current.Storage["DeshretSpirit"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["DeshretSpirit"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.IndomitableSpirit) <= 2;
		if (World.Act4.Mines2.IsCurrentArea && CachedDeshretSpirit == null)
		{
			NetworkObject deshretSpirit = DeshretSpirit;
			if (deshretSpirit != (NetworkObject)null)
			{
				CachedDeshretSpirit = new CachedObject(deshretSpirit);
			}
		}
	}

	public static async Task<bool> FreeDeshret()
	{
		if (!bool_0)
		{
			if (World.Act4.Mines2.IsCurrentArea)
			{
				if (await Helpers.HandleQuestObject(CachedDeshretSpirit))
				{
					return true;
				}
				await Helpers.Explore();
				return true;
			}
			await Travel.To(World.Act4.Mines2);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (!World.Act4.Mines2.IsCurrentArea)
		{
			return await Helpers.TakeQuestReward(World.Act4.Highgate, TownNpcs.Tasuni, "Deshret Reward", null, "Book-a4q6");
		}
		await Travel.To(World.Act4.CrystalVeins);
		return true;
	}
}
