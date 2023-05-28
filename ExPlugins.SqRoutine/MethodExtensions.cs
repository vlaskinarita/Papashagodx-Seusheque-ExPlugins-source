using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.FilesInMemory;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;

namespace ExPlugins.SqRoutine;

public static class MethodExtensions
{
	private static readonly SqRoutineSettings Config;

	public static bool HasCurseFromEx(this Actor target, Skill skill)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!skill.Stats.ContainsKey((StatTypeGGG)1614))
		{
			return target.HasCurseFrom(skill.Name);
		}
		List<Skill> source = SkillBarHud.Skills.ToList();
		InventorySlot inventorySlot_0 = skill.InventorySlot;
		IEnumerable<string> source2 = from s in source
			where s.InventorySlot == inventorySlot_0 && s.SkillTags.Contains("curse") && s.Stats.ContainsKey((StatTypeGGG)17331)
			select s.Name;
		return source2.All((Func<string, bool>)target.HasCurseFrom);
	}

	public static bool SqCanUse(this Skill skill, bool ignoreSkillbar = false)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (!((RemoteMemoryObject)(object)skill == (RemoteMemoryObject)null))
		{
			if (skill.IsOnSkillBar || ignoreSkillbar)
			{
				if (skill.InternalId.Equals("flicker_strike"))
				{
					bool flag = InventoryUi.AllInventoryControls.Any((InventoryControlWrapper x) => (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_Main && (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryMainHand && (RemoteMemoryObject)(object)x != (RemoteMemoryObject)(object)InventoryUi.InventoryControl_SecondaryOffHand && x.Inventory.Items.Any((Item i) => i.FullName.Equals("Soul Taker"))) || ((int)skill.CostType == 0 && skill.Cost < ((Actor)LokiPoe.Me).Health) || skill.Cost < ((Actor)LokiPoe.Me).Mana;
					if (skill.CanUse(false, false, true))
					{
						return true;
					}
					if (((Actor)LokiPoe.Me).FrenzyCharges > 0 && flag)
					{
						return true;
					}
				}
				return skill.CanUse(false, ignoreSkillbar, true);
			}
			return false;
		}
		return false;
	}

	public static UseResult SqBeginUseAt(Skill skill, Vector2i pos)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		if (Config.DebugMode)
		{
			GlobalLog.Info($"[SqBeginUseAt] [{skill.Name}][{skill.BoundKey}]");
		}
		MouseManager.SetMousePos((string)null, pos, true);
		if (ProcessHookManager.GetKeyState(skill.BoundKey) == short.MinValue && ((Actor)LokiPoe.Me).HasCurrentAction && (RemoteMemoryObject)(object)((Actor)LokiPoe.Me).CurrentAction.Skill == (RemoteMemoryObject)(object)skill)
		{
			return (UseResult)0;
		}
		ProcessHookManager.ClearAllKeyStates();
		return SkillBarHud.BeginUseAt(skill.Slot, false, pos);
	}

	public static async Task<UseResult> SqUse(Skill skill)
	{
		if (Config.DebugMode)
		{
			GlobalLog.Info($"[SqUse] [{skill.Name}][{skill.BoundKey}]");
		}
		int slot = skill.Slot;
		Vector2i randomPos = new Vector2i(LokiPoe.MyPosition.X + LokiPoe.Random.Next(-5, 5), LokiPoe.MyPosition.Y + LokiPoe.Random.Next(-5, 5));
		UseResult use = SkillBarHud.UseAt(slot, false, randomPos, skill.HasCastTime());
		if ((int)use == 0)
		{
			await Wait.SleepSafe(15);
			if (((Actor)LokiPoe.Me).HasCurrentAction && !CheckCurrentAction(skill))
			{
				PlayerMoverManager.MoveTowards(randomPos, (object)null);
				use = SkillBarHud.UseAt(slot, false, randomPos, skill.HasCastTime());
			}
		}
		return use;
	}

	public static async Task<UseResult> SqUseAt(Skill skill, Vector2i pos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (Config.DebugMode)
		{
			GlobalLog.Info($"[SqUseAt] [{skill.Name}][{skill.BoundKey}]");
		}
		Vector2i randomPos = new Vector2i(pos.X + LokiPoe.Random.Next(-5, 5), pos.Y + LokiPoe.Random.Next(-5, 5));
		Vector2i findPos = ((!skill.SkillTags.Any((string t) => t.ContainsIgnorecase("melee"))) ? randomPos : pos);
		int slot = skill.Slot;
		UseResult use = SkillBarHud.UseAt(slot, false, findPos, skill.HasCastTime());
		if ((int)use == 0)
		{
			await Wait.SleepSafe(15);
			if (((Actor)LokiPoe.Me).HasCurrentAction && !CheckCurrentAction(skill))
			{
				PlayerMoverManager.MoveTowards(randomPos, (object)null);
				use = SkillBarHud.UseAt(slot, false, findPos, skill.HasCastTime());
			}
		}
		return use;
	}

	private static bool CheckCurrentAction(Skill skill)
	{
		if (skill.HasCastTime())
		{
			bool result;
			if (!(result = ((Actor)LokiPoe.Me).CurrentAction.Skill.InternalId == skill.InternalId) && Config.DebugMode)
			{
				GlobalLog.Error(((Actor)LokiPoe.Me).CurrentAction.Skill.InternalId + " is current action. Need: " + skill.InternalId);
			}
			return result;
		}
		return true;
	}

	public static UseResult ExUseOn(Skill skill, NetworkObject obj)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		int slot = skill.Slot;
		return SkillBarHud.UseOn(slot, skill.HasCastTime(), obj, skill.HasCastTime());
	}

	public static bool HasCastTime(this Skill skill)
	{
		return skill.CastTime.TotalMilliseconds > 50.0;
	}

	public static bool IsSyndicateMember(this NetworkObject m)
	{
		return m.Metadata.StartsWith("Metadata/Monsters/LeagueBetrayal/Betrayal") && !m.Metadata.EndsWith("MasterNinjaCop");
	}

	public static bool HasProxShieldEx(this Actor x)
	{
		if (!Blight.IsEncounterRunning)
		{
			List<Aura> auras = x.Auras;
			return x.HasProximityShield || auras.Any((Aura a) => a.InternalName.Equals("archnemesis_time_bubble"));
		}
		return false;
	}

	public static bool ManaBurnDonut(this Actor x)
	{
		if (Blight.IsEncounterRunning)
		{
			return false;
		}
		return x.Auras.Any((Aura a) => a.InternalName.Equals("archnemesis_mana_donut"));
	}

	public static bool AffixToFocus(this Monster x)
	{
		List<ModRecord> list = x.Affixes.ToList();
		bool result = false;
		foreach (ModRecord item in list)
		{
			switch (item.DisplayName)
			{
			case "Diluting Touch":
			case "Eroding Touch":
			case "Paralysing Touch":
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool IsBlightMob(this Actor x)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (!((NetworkObject)(object)x == (NetworkObject)null))
		{
			if ((int)((NetworkObject)x).Reaction != -1)
			{
				if (((NetworkObject)x).Metadata != null)
				{
					if (!x.IsDead)
					{
						if (!(((NetworkObject)x).Name == "Blighted Spore"))
						{
							if (((NetworkObject)x).Metadata.Contains("Blight") && !((NetworkObject)x).Metadata.Contains("BlightTower") && !((NetworkObject)x).Metadata.Contains("BlightFoundation") && !((NetworkObject)x).Metadata.Contains("BlightBuilder"))
							{
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	static MethodExtensions()
	{
		Config = SqRoutineSettings.Instance;
	}
}
