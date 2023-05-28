using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A8_Q1_EssenceOfHag
{
	private static bool bool_0;

	private static Monster Doedre => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2083 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static NetworkObject Valve => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/Monsters/Doedre/DoedreSewer/DoedreCauldronValve");

	public static void Tick()
	{
		bool_0 = World.Act8.DoedreCesspool.IsWaypointOpened;
	}

	public static async Task<bool> KillDoedre()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act8.DoedreCesspool.IsCurrentArea)
		{
			NetworkObject networkObject_0 = Valve;
			if (networkObject_0 != (NetworkObject)null)
			{
				if (!networkObject_0.IsTargetable)
				{
					Monster doedre = Doedre;
					if ((NetworkObject)(object)doedre != (NetworkObject)null && ((NetworkObject)(object)doedre).PathExists())
					{
						await Helpers.MoveAndWait((NetworkObject)(object)doedre);
						return true;
					}
					await Helpers.MoveAndWait(networkObject_0.WalkablePosition(), "Waiting for any Doedre fight object");
					return true;
				}
				await networkObject_0.WalkablePosition().ComeAtOnce();
				if (!(await PlayerAction.Interact(networkObject_0, () => !networkObject_0.Fresh<NetworkObject>().IsTargetable, "Valve interaction")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
		}
		await Travel.To(World.Act8.GrandPromenade);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act8.SarnEncampment, TownNpcs.HarganA8, "Doedre Reward", Quests.EssenceOfHag.Id);
	}
}
