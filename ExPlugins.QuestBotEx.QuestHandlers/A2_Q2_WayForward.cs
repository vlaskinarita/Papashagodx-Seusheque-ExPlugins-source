using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q2_WayForward
{
	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static Monster CaptainArteri => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)709 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static NetworkObject ThaumeticSeal => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/Spikes/SpikeBlockageButton");

	private static WalkablePosition CachedArteriPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["ArteriPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["ArteriPosition"] = value;
		}
	}

	private static CachedObject CachedSeal
	{
		get
		{
			return CombatAreaCache.Current.Storage["ThaumeticSeal"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["ThaumeticSeal"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act2.WesternForest.IsCurrentArea)
		{
			return;
		}
		Monster captainArteri = CaptainArteri;
		if ((NetworkObject)(object)captainArteri != (NetworkObject)null)
		{
			CachedArteriPos = ((NetworkObject)(object)captainArteri).WalkablePosition();
		}
		if (CachedSeal == null)
		{
			NetworkObject thaumeticSeal = ThaumeticSeal;
			if (thaumeticSeal != (NetworkObject)null)
			{
				CachedSeal = new CachedObject(thaumeticSeal);
			}
		}
	}

	public static async Task<bool> KillArteri()
	{
		if (Helpers.PlayerHasQuestItem("SpikeSealKey"))
		{
			return false;
		}
		if (World.Act2.WesternForest.IsCurrentArea)
		{
			WalkablePosition arteriPos = CachedArteriPos;
			if (arteriPos != null)
			{
				await Helpers.MoveAndWait(arteriPos);
			}
			else
			{
				tgtPosition_0.Come();
			}
			return true;
		}
		await Travel.To(World.Act2.WesternForest);
		return true;
	}

	public static async Task<bool> OpenPath()
	{
		if (Helpers.PlayerHasQuestItem("SpikeSealKey"))
		{
			if (World.Act2.WesternForest.IsCurrentArea)
			{
				if (await Helpers.HandleQuestObject(CachedSeal))
				{
					return true;
				}
				tgtPosition_1.Come();
				return true;
			}
			await Travel.To(World.Act2.WesternForest);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act1.LioneyeWatch, TownNpcs.Bestel, "Road Reward", null, "Book-a1q9");
	}

	public static async Task<bool> UseReward()
	{
		return await Helpers.UseQuestReward("Book-a1q9");
	}

	static A2_Q2_WayForward()
	{
		tgtPosition_0 = new TgtPosition("Captain Arteri location", "tent_mid_square_alteri_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Thaumetic Seal location", "forest_mountainpass_v01_01.tgt");
	}
}
