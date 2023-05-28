using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A7_Q3_WebOfSecrets
{
	private static Npc Silk => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)12 }).FirstOrDefault<Npc>();

	private static CachedObject CachedSilk
	{
		get
		{
			return CombatAreaCache.Current.Storage["Silk"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["Silk"] = value;
		}
	}

	public static void Tick()
	{
		if (World.Act7.ChamberOfSins1.IsCurrentArea && CachedSilk == null)
		{
			Npc silk = Silk;
			if ((NetworkObject)(object)silk != (NetworkObject)null)
			{
				CachedSilk = new CachedObject((NetworkObject)(object)silk);
			}
		}
	}

	public static async Task<bool> TakeObsidianKey()
	{
		if (World.Act7.ChamberOfSins1.IsCurrentArea)
		{
			if (CachedSilk != null)
			{
				WalkablePosition pos = CachedSilk.Position;
				if (pos.IsFar)
				{
					pos.Come();
					return true;
				}
				if (!(await CachedSilk.Object.AsTownNpc().TakeReward(null, "Black Death Reward")))
				{
					if ((RemoteMemoryObject)(object)Inventories.InventoryItems.FirstOrDefault((Item d) => d.Metadata == "Metadata/Items/QuestItems/Act7/ObsidianKey") != (RemoteMemoryObject)null)
					{
						GlobalLog.Debug("Obsidian key detected! need relog to reset quest state.");
						EscapeState.LogoutToCharacterSelection();
					}
					else
					{
						GlobalLog.Debug("No key in inventory");
						ErrorManager.ReportError();
					}
				}
				return false;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act7.ChamberOfSins1);
		return true;
	}
}
