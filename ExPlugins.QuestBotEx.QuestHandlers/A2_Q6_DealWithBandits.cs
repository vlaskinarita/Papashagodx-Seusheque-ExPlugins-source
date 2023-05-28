using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A2_Q6_DealWithBandits
{
	public static readonly TgtPosition AliraTgt;

	public static readonly TgtPosition KraitynTgt;

	public static readonly TgtPosition OakTgt;

	private static int int_0;

	private static int int_1;

	private static Monster Alira => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)154 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster Kraityn => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)155 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster Oak => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)156 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static WalkablePosition CachedAliraPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["AliraPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["AliraPosition"] = value;
		}
	}

	private static WalkablePosition CachedKraitynPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["KraitynPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["KraitynPosition"] = value;
		}
	}

	private static WalkablePosition CachedOakPos
	{
		get
		{
			return CombatAreaCache.Current.Storage["OakPosition"] as WalkablePosition;
		}
		set
		{
			CombatAreaCache.Current.Storage["OakPosition"] = value;
		}
	}

	public static void Tick()
	{
		string id = World.CurrentArea.Id;
		if (id == World.Act2.WesternForest.Id)
		{
			Monster alira = Alira;
			if ((NetworkObject)(object)alira != (NetworkObject)null)
			{
				CachedAliraPos = ((NetworkObject)(object)alira).WalkablePosition();
			}
		}
		else if (!(id == World.Act2.BrokenBridge.Id))
		{
			if (id == World.Act2.Wetlands.Id)
			{
				Monster oak = Oak;
				if ((NetworkObject)(object)oak != (NetworkObject)null)
				{
					CachedOakPos = ((NetworkObject)(object)oak).WalkablePosition();
				}
			}
		}
		else
		{
			Monster kraityn = Kraityn;
			if ((NetworkObject)(object)kraityn != (NetworkObject)null)
			{
				CachedKraitynPos = ((NetworkObject)(object)kraityn).WalkablePosition();
			}
		}
	}

	public static async Task<bool> KillAlira()
	{
		if (!Helpers.PlayerHasQuestItem("IntAmulet"))
		{
			if (World.Act2.WesternForest.IsCurrentArea)
			{
				WalkablePosition aliraPos = CachedAliraPos;
				if (!(aliraPos != null))
				{
					AliraTgt.Come();
					return true;
				}
				if (!aliraPos.IsFar)
				{
					Monster monster_0 = Alira;
					if ((NetworkObject)(object)monster_0 != (NetworkObject)null && (int)((NetworkObject)monster_0).Reaction == -1)
					{
						if (!(await BanditHelper.Kill((NetworkObject)(object)monster_0)))
						{
							ErrorManager.ReportError();
							return true;
						}
						if (!(await Wait.For(() => (int)((NetworkObject)monster_0.Fresh<Monster>()).Reaction == 1, "Alira becomes hostile")))
						{
							ErrorManager.ReportError();
						}
					}
					return true;
				}
				aliraPos.Come();
				return true;
			}
			await Travel.To(World.Act2.WesternForest);
			return true;
		}
		return false;
	}

	public static async Task<bool> HelpAlira()
	{
		if (!Helpers.PlayerHasQuestItem("CombinedAmulet"))
		{
			if (World.Act2.WesternForest.IsCurrentArea)
			{
				WalkablePosition aliraPos = CachedAliraPos;
				if (!(aliraPos != null))
				{
					AliraTgt.Come();
					return true;
				}
				if (!aliraPos.IsFar)
				{
					Monster alira = Alira;
					if ((NetworkObject)(object)alira != (NetworkObject)null && (int)((NetworkObject)alira).Reaction == -1)
					{
						if (!(await BanditHelper.Help((NetworkObject)(object)alira)))
						{
							ErrorManager.ReportError();
						}
						else
						{
							await PlayerAction.Logout();
						}
					}
					return true;
				}
				aliraPos.Come();
				return true;
			}
			await Travel.To(World.Act2.WesternForest);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillKraityn()
	{
		if (Helpers.PlayerHasQuestItem("DexAmulet"))
		{
			return false;
		}
		if (World.Act2.BrokenBridge.IsCurrentArea)
		{
			WalkablePosition kraitynPos = CachedKraitynPos;
			if (kraitynPos != null)
			{
				if (!kraitynPos.IsFar)
				{
					Monster monster_0 = Kraityn;
					if ((NetworkObject)(object)monster_0 != (NetworkObject)null && (int)((NetworkObject)monster_0).Reaction == -1)
					{
						if (!(await BanditHelper.Kill((NetworkObject)(object)monster_0)))
						{
							int_0++;
							ErrorManager.ReportError();
							if (int_0 >= 3)
							{
								await LeaveArea();
							}
							return true;
						}
						if (!(await Wait.For(() => (int)((NetworkObject)monster_0.Fresh<Monster>()).Reaction == 1, "Kraityn becomes hostile")))
						{
							ErrorManager.ReportError();
						}
					}
					return true;
				}
				kraitynPos.Come();
				return true;
			}
			KraitynTgt.Come();
			return true;
		}
		await Travel.To(World.Act2.BrokenBridge);
		return true;
	}

	public static async Task<bool> HelpKraityn()
	{
		if (!Helpers.PlayerHasQuestItem("CombinedAmulet"))
		{
			if (World.Act2.BrokenBridge.IsCurrentArea)
			{
				WalkablePosition kraitynPos = CachedKraitynPos;
				if (kraitynPos != null)
				{
					if (!kraitynPos.IsFar)
					{
						Monster kraityn = Kraityn;
						if ((NetworkObject)(object)kraityn != (NetworkObject)null && (int)((NetworkObject)kraityn).Reaction == -1)
						{
							if (!(await BanditHelper.Help((NetworkObject)(object)kraityn)))
							{
								ErrorManager.ReportError();
							}
							else
							{
								await PlayerAction.Logout();
							}
						}
						return true;
					}
					kraitynPos.Come();
					return true;
				}
				KraitynTgt.Come();
				return true;
			}
			await Travel.To(World.Act2.BrokenBridge);
			return true;
		}
		return false;
	}

	public static async Task<bool> KillOak()
	{
		if (Helpers.PlayerHasQuestItem("StrAmulet"))
		{
			return false;
		}
		if (World.Act2.Wetlands.IsCurrentArea)
		{
			WalkablePosition oakPos = CachedOakPos;
			if (oakPos != null)
			{
				if (!oakPos.IsFar)
				{
					Monster monster_0 = Oak;
					if ((NetworkObject)(object)monster_0 != (NetworkObject)null && (int)((NetworkObject)monster_0).Reaction == -1)
					{
						if (!(await BanditHelper.Kill((NetworkObject)(object)monster_0)))
						{
							int_1++;
							ErrorManager.ReportError();
							if (int_1 >= 3)
							{
								await LeaveArea();
							}
							return true;
						}
						if (!(await Wait.For(() => (int)((NetworkObject)monster_0.Fresh<Monster>()).Reaction == 1, "Oak becomes hostile")))
						{
							ErrorManager.ReportError();
						}
					}
					return true;
				}
				oakPos.Come();
				return true;
			}
			OakTgt.Come();
			return true;
		}
		await Travel.To(World.Act2.Wetlands);
		return true;
	}

	public static async Task<bool> HelpOak()
	{
		if (Helpers.PlayerHasQuestItem("CombinedAmulet"))
		{
			return false;
		}
		if (World.Act2.Wetlands.IsCurrentArea)
		{
			WalkablePosition oakPos = CachedOakPos;
			if (oakPos != null)
			{
				if (!oakPos.IsFar)
				{
					Monster oak = Oak;
					if ((NetworkObject)(object)oak != (NetworkObject)null && (int)((NetworkObject)oak).Reaction == -1)
					{
						if (!(await BanditHelper.Help((NetworkObject)(object)oak)))
						{
							ErrorManager.ReportError();
						}
						else
						{
							await PlayerAction.Logout();
						}
					}
					return true;
				}
				oakPos.Come();
				return true;
			}
			OakTgt.Come();
			return true;
		}
		await Travel.To(World.Act2.Wetlands);
		return true;
	}

	public static async Task<bool> HelpEramir()
	{
		if (!Helpers.PlayerHasQuestItem("CombinedAmulet"))
		{
			if (!World.Act2.ForestEncampment.IsCurrentArea)
			{
				await Travel.To(World.Act2.ForestEncampment);
				return true;
			}
			if (!(await TownNpcs.Eramir.TakeReward(null, "Take the Apex")))
			{
				ErrorManager.ReportError();
			}
			return true;
		}
		return false;
	}

	private static async Task LeaveArea()
	{
		GlobalLog.Error("[A2_Q6_DealWithBandits] Leaving area");
		global::ExPlugins.EXtensions.EXtensions.AbandonCurrentArea();
		if (!(await PlayerAction.TpToTown()))
		{
			ErrorManager.ReportError();
		}
		int_1 = 0;
		int_0 = 0;
	}

	static A2_Q6_DealWithBandits()
	{
		AliraTgt = new TgtPosition("Alira camp", "treewitch_camp_v01_01.tgt");
		KraitynTgt = new TgtPosition("Kraityn camp", "bridge_large_v01_01.tgt | bridge_large_v01_01.tgt");
		OakTgt = new TgtPosition("Oak camp", "cliffpathconnection_gate_v01_01.tgt");
	}
}
