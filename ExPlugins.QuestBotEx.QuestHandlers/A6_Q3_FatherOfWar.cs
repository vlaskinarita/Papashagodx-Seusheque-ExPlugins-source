using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A6_Q3_FatherOfWar
{
	private static readonly TgtPosition tgtPosition_0;

	private static bool bool_0;

	private static Monster DishonouredQueen => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2218 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster ClosestTukohamaTotem => ((IEnumerable)ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2165 })).Closest<Monster>((Func<Monster, bool>)((Monster m) => ((NetworkObject)m).IsTargetable && !((Actor)m).IsDead));

	private static NetworkObject TukohamaRoomObj => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/MiscellaneousObjects/ArenaMiddle");

	private static AreaTransition TukohamaRoomNorthernExit => (from t in ObjectManager.Objects
		where ((NetworkObject)t).Name == World.Act6.KaruiFortress.Name
		orderby ((NetworkObject)t).Position.Y descending
		select t).FirstOrDefault();

	private static WalkablePosition CachedQueenPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["DishonouredQueenPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["DishonouredQueenPosition"] = value;
		}
	}

	public static void Tick()
	{
		bool_0 = QuestManager.GetStateInaccurate(Quests.FatherOfWar) <= 2;
		if (World.Act6.MudFlats.IsCurrentArea)
		{
			Monster dishonouredQueen = DishonouredQueen;
			if ((NetworkObject)(object)dishonouredQueen != (NetworkObject)null)
			{
				CachedQueenPos = (((Actor)dishonouredQueen).IsDead ? null : ((NetworkObject)(object)dishonouredQueen).WalkablePosition());
			}
		}
	}

	public static async Task<bool> GrabEyeOfConquest()
	{
		if (Helpers.PlayerHasQuestItem("KaruiEye"))
		{
			return false;
		}
		if (World.Act6.MudFlats.IsCurrentArea)
		{
			WalkablePosition queenPos = CachedQueenPos;
			if (queenPos != null)
			{
				await Helpers.MoveAndWait(queenPos);
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act6.MudFlats);
		return true;
	}

	public static async Task<bool> KillTukohama()
	{
		if (bool_0)
		{
			return false;
		}
		if (World.Act6.KaruiFortress.IsCurrentArea)
		{
			NetworkObject roomObj = TukohamaRoomObj;
			if (roomObj != (NetworkObject)null)
			{
				Monster totem = ClosestTukohamaTotem;
				if ((NetworkObject)(object)totem != (NetworkObject)null && ((NetworkObject)totem).Distance > 50f)
				{
					((NetworkObject)(object)totem).WalkablePosition().Come();
					return true;
				}
				return true;
			}
			await Helpers.MoveAndTakeLocalTransition(tgtPosition_0);
			return true;
		}
		await Travel.To(World.Act6.KaruiFortress);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		if (World.Act6.KaruiFortress.IsCurrentArea)
		{
			NetworkObject roomObj = TukohamaRoomObj;
			if (roomObj != (NetworkObject)null)
			{
				if (!(await PlayerAction.TakeTransition(TukohamaRoomNorthernExit)))
				{
					ErrorManager.ReportError();
				}
			}
			else
			{
				await Travel.To(World.Act6.Ridge);
			}
			return true;
		}
		return await Helpers.TakeQuestReward(World.Act6.LioneyeWatch, TownNpcs.townNpc_0, "Tukohama Reward", null, "Book-a6q3");
	}

	static A6_Q3_FatherOfWar()
	{
		tgtPosition_0 = new TgtPosition("Tukohama's Keep", "swamp_longbridge_v01_01.tgt", closest: true);
	}
}
