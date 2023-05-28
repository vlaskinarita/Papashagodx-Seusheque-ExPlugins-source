using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A9_Q2_QueenOfSands
{
	private static bool bool_0;

	private static Monster Shakari => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2307 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.QueenOfSands) <= 2;
	}

	public static async Task<bool> TalkToSin()
	{
		if (World.Act9.Highgate.IsCurrentArea)
		{
			if (!(await TownNpcs.SinA9.Talk()))
			{
				ErrorManager.ReportError();
				return true;
			}
			await Coroutines.CloseBlockingWindows();
			EscapeState.LogoutToCharacterSelection();
			return false;
		}
		await Travel.To(World.Act9.Highgate);
		return true;
	}

	public static async Task<bool> KillShakari()
	{
		if (!bool_0)
		{
			if (World.Act9.Oasis.IsCurrentArea)
			{
				Monster shakari = Shakari;
				if ((NetworkObject)(object)shakari != (NetworkObject)null && ((NetworkObject)(object)shakari).PathExists())
				{
					await Helpers.MoveAndWait((NetworkObject)(object)shakari);
					return true;
				}
				await Helpers.Explore("The Sand Pit");
				return true;
			}
			await Travel.To(World.Act9.Oasis);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeBottledStorm()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.PetarusAndVanjaA9, "Take Bottled Storm");
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act9.Highgate, TownNpcs.Irasha, "Shakari Reward", null, "Book-a9q5");
	}
}
