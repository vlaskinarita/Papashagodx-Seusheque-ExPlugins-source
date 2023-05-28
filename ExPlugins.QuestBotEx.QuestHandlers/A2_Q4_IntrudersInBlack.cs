using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q4_IntrudersInBlack
{
	private static readonly TgtPosition tgtPosition_0;

	private static Monster Fidelitas => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)215 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Chest StrangeDevice => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)2565 }).FirstOrDefault<Chest>();

	private static WalkablePosition CachedFidelitasPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["FidelitasPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["FidelitasPosition"] = value;
		}
	}

	private static CachedObject CachedStrangeDevice
	{
		get
		{
			return CombatAreaCache.Current.Storage["StrangeDevice"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["StrangeDevice"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act2.ChamberOfSins2.IsCurrentArea)
		{
			return;
		}
		Monster fidelitas = Fidelitas;
		if ((NetworkObject)(object)fidelitas != (NetworkObject)null)
		{
			CachedFidelitasPos = ((!((Actor)fidelitas).IsDead) ? ((NetworkObject)(object)fidelitas).WalkablePosition() : null);
		}
		if (CachedStrangeDevice == null)
		{
			Chest strangeDevice = StrangeDevice;
			if ((NetworkObject)(object)strangeDevice != (NetworkObject)null)
			{
				CachedStrangeDevice = new CachedObject((NetworkObject)(object)strangeDevice);
			}
		}
	}

	public static async Task<bool> GrabBalefulGem()
	{
		if (Helpers.PlayerHasQuestItem("PoisonSkillGem"))
		{
			return false;
		}
		if (World.Act2.ChamberOfSins2.IsCurrentArea)
		{
			WalkablePosition fidelitasPos = CachedFidelitasPos;
			if (fidelitasPos != null)
			{
				await Helpers.MoveAndWait(fidelitasPos);
				return true;
			}
			if (!(await Helpers.OpenQuestChest(CachedStrangeDevice)))
			{
				tgtPosition_0.Come();
				return true;
			}
			return true;
		}
		await Travel.To(World.Act2.ChamberOfSins2);
		return true;
	}

	public static async Task<bool> TakeReward()
	{
		return await Helpers.TakeQuestReward(World.Act2.ForestEncampment, TownNpcs.Greust, "Blackguard Reward", Quests.IntrudersInBlack.Id);
	}

	static A2_Q4_IntrudersInBlack()
	{
		tgtPosition_0 = new TgtPosition("Strange Device location", "templeruinforest_questcart.tgt");
	}
}
