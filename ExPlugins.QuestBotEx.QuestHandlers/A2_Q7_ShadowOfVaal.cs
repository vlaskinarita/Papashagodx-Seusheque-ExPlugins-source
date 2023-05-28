using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q7_ShadowOfVaal
{
	private static bool bool_0;

	private static TriggerableBlockage DarkAltar => ObjectManager.Objects.FirstOrDefault((TriggerableBlockage t) => ((NetworkObject)t).Metadata == "Metadata/Monsters/IncaShadowBoss/IncaBossSpawner");

	private static Monster VaalOversoul => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)217 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	public static void Tick()
	{
		bool_0 = World.Act3.CityOfSarn.IsWaypointOpened;
	}

	public static async Task<bool> KillVaal()
	{
		if (bool_0)
		{
			if (World.Act2.ForestEncampment.IsCurrentArea)
			{
				return false;
			}
			await Travel.To(World.Act2.ForestEncampment);
			return true;
		}
		if (World.Act2.AncientPyramid.IsCurrentArea)
		{
			TriggerableBlockage triggerableBlockage_0 = DarkAltar;
			if ((NetworkObject)(object)triggerableBlockage_0 != (NetworkObject)null)
			{
				if (triggerableBlockage_0.IsOpened)
				{
					Monster vaal = VaalOversoul;
					if ((NetworkObject)(object)vaal != (NetworkObject)null)
					{
						await Helpers.MoveToBossOrAnyMob(vaal);
						return true;
					}
					await Helpers.MoveAndWait(((NetworkObject)(object)triggerableBlockage_0).WalkablePosition(), "Waiting for Vaal Oversoul");
					return true;
				}
				await ((NetworkObject)(object)triggerableBlockage_0).WalkablePosition().ComeAtOnce();
				if (!(await PlayerAction.Interact((NetworkObject)(object)triggerableBlockage_0, () => triggerableBlockage_0.Fresh<TriggerableBlockage>().IsOpened, "Dark Altar opening")))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
		}
		await Travel.To(World.Act3.CityOfSarn);
		return true;
	}
}
