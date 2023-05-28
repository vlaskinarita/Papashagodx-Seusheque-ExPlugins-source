using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A4_Q5_EternalNightmare
{
	private class Class22
	{
		[CompilerGenerated]
		private readonly string string_0;

		[CompilerGenerated]
		private readonly TgtPosition tgtPosition_0;

		[CompilerGenerated]
		private WalkablePosition walkablePosition_0;

		[CompilerGenerated]
		private bool bool_0;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
		}

		public TgtPosition TgtPosition
		{
			[CompilerGenerated]
			get
			{
				return tgtPosition_0;
			}
		}

		public WalkablePosition Position
		{
			[CompilerGenerated]
			get
			{
				return walkablePosition_0;
			}
			[CompilerGenerated]
			set
			{
				walkablePosition_0 = value;
			}
		}

		public bool Killed
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}

		public Class22(string name, TgtPosition tgtPosition, WalkablePosition position, bool killed)
		{
			string_0 = name;
			tgtPosition_0 = tgtPosition;
			Position = position;
			Killed = killed;
		}
	}

	private static readonly TgtPosition tgtPosition_0;

	private static readonly TgtPosition tgtPosition_1;

	private static readonly TgtPosition mfsuwvJrmr;

	private static Class22 srMuFxSaBq;

	private static bool bool_0;

	private static Monster monster_0;

	private static Npc npc_0;

	private static Npc npc_1;

	private static NetworkObject networkObject_0;

	private static NetworkObject networkObject_1;

	private static AreaTransition areaTransition_0;

	private static AreaTransition areaTransition_1;

	private static AreaTransition areaTransition_2;

	private static Monster monster_1;

	private static Monster monster_2;

	private static Monster monster_3;

	private static int int_0;

	private static string string_0;

	private static Monster Shavronne => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1673 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster Doedre => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1668 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static Monster Maligaro => ObjectManager.GetObjects((PoeObjectEnum[])(object)new PoeObjectEnum[1] { (PoeObjectEnum)1664 }).FirstOrDefault((Monster m) => (int)m.Rarity == 3);

	private static List<Class22> Minibosses
	{
		get
		{
			List<Class22> list = CombatAreaCache.Current.Storage["Minibosses"] as List<Class22>;
			if (list == null)
			{
				tgtPosition_0.Initialize();
				tgtPosition_1.Initialize();
				mfsuwvJrmr.Initialize();
				list = new List<Class22>
				{
					new Class22("Shavronne of Umbra", tgtPosition_0, null, killed: false),
					new Class22("Doedre Darktongue", tgtPosition_1, null, killed: false),
					new Class22("Maligaro, The Inquisitor", mfsuwvJrmr, null, killed: false)
				};
				CombatAreaCache.Current.Storage["Minibosses"] = list;
			}
			return list;
		}
	}

	public static void Tick()
	{
		if (!World.Act4.Harvest.IsCurrentArea || srMuFxSaBq == null)
		{
			return;
		}
		string name = srMuFxSaBq.Name;
		Monster val = null;
		switch (name)
		{
		case "Shavronne of Umbra":
			val = Shavronne;
			break;
		case "Maligaro, The Inquisitor":
			val = Maligaro;
			break;
		case "Doedre Darktongue":
			val = Doedre;
			break;
		}
		if ((NetworkObject)(object)val == (NetworkObject)null)
		{
			return;
		}
		if (((Actor)val).IsDead)
		{
			srMuFxSaBq.Killed = true;
			srMuFxSaBq = null;
			if (Minibosses.All((Class22 m) => m.Killed))
			{
				bool_0 = true;
			}
		}
		else
		{
			WalkablePosition walkablePosition = ((NetworkObject)(object)val).WalkablePosition();
			if (srMuFxSaBq.Position == null)
			{
				GlobalLog.Warn($"[EternalNightmare] Registering {walkablePosition}");
			}
			srMuFxSaBq.Position = walkablePosition;
		}
	}

	private static async Task KillMinibosses()
	{
		if (srMuFxSaBq == null)
		{
			srMuFxSaBq = (from b in Minibosses
				where !b.Killed
				orderby b.TgtPosition.Distance
				select b).FirstOrDefault();
		}
		WalkablePosition pos = srMuFxSaBq.Position;
		if (pos != null)
		{
			await Helpers.MoveAndWait(pos);
		}
		else
		{
			srMuFxSaBq.TgtPosition.Come();
		}
	}

	public static async Task<bool> KillMalachai()
	{
		if (World.Act4.Harvest.IsCurrentArea)
		{
			UpdateMalachaiFightObjects();
			if (networkObject_0 != (NetworkObject)null)
			{
				if (!((NetworkObject)(object)areaTransition_2 != (NetworkObject)null))
				{
					if (!((NetworkObject)(object)areaTransition_1 != (NetworkObject)null) || !((NetworkObject)areaTransition_1).IsTargetable || !(((NetworkObject)areaTransition_1).Distance < 80f))
					{
						if (!((NetworkObject)(object)monster_3 != (NetworkObject)null) || !((NetworkObject)monster_3).IsTargetable || ((Actor)monster_3).IsDead)
						{
							if (!((NetworkObject)(object)monster_1 != (NetworkObject)null) || (int)((NetworkObject)monster_1).Reaction != 1)
							{
								if (!((NetworkObject)(object)monster_2 != (NetworkObject)null))
								{
									if ((NetworkObject)(object)monster_1 != (NetworkObject)null && (int)((NetworkObject)monster_1).Reaction == 0 && ((NetworkObject)monster_1).Distance > 15f)
									{
										((NetworkObject)(object)monster_1).WalkablePosition().Come();
										return true;
									}
									GlobalLog.Debug("Waiting for any Malachai fight object");
									await Wait.StuckDetectionSleep(500);
									return true;
								}
								await Helpers.MoveAndWait(((NetworkObject)(object)monster_2).WalkablePosition());
								return true;
							}
							WalkablePosition pos = ((NetworkObject)(object)monster_1).WalkablePosition();
							if (pos.IsFar)
							{
								pos.Come();
							}
							return true;
						}
						((NetworkObject)(object)monster_3).WalkablePosition().Come();
						return true;
					}
					if (!(await PlayerAction.TakeTransition(areaTransition_1)))
					{
						ErrorManager.ReportError();
					}
					return true;
				}
				return false;
			}
			if (Minibosses.Any((Class22 b) => !b.Killed))
			{
				await KillMinibosses();
				return true;
			}
			if (bool_0)
			{
				await PlayerAction.Logout();
				bool_0 = false;
				return true;
			}
			if ((NetworkObject)(object)npc_1 != (NetworkObject)null && ((NetworkObject)npc_1).IsTargetable && ((NetworkObject)npc_1).HasNpcFloatingIcon)
			{
				WalkablePosition pietyPos2 = ((NetworkObject)(object)npc_1).WalkablePosition();
				if (pietyPos2.IsFar)
				{
					pietyPos2.Come();
				}
				else
				{
					await Helpers.TalkTo((NetworkObject)(object)npc_1);
				}
				return true;
			}
			if (networkObject_1 != (NetworkObject)null && networkObject_1.IsTargetable && networkObject_1.Distance < 80f)
			{
				await networkObject_1.WalkablePosition().ComeAtOnce();
				if (!(await PlayerAction.Interact(networkObject_1, () => !networkObject_1.IsTargetable, "Black Core Mouth opening")))
				{
					ErrorManager.ReportError();
				}
				if (ErrorManager.ErrorCount >= 8)
				{
					ErrorManager.Reset();
					Travel.RequestNewInstance(World.Act4.Harvest);
					await PlayerAction.TpToTown();
				}
				return true;
			}
			if ((NetworkObject)(object)areaTransition_0 != (NetworkObject)null && ((NetworkObject)areaTransition_0).IsTargetable && ((NetworkObject)areaTransition_0).Distance < 80f)
			{
				await ((NetworkObject)(object)areaTransition_0).WalkablePosition().ComeAtOnce();
				if (!(await PlayerAction.TakeTransition(areaTransition_0)))
				{
					ErrorManager.ReportError();
				}
				return true;
			}
		}
		if (World.Act4.BellyOfBeast2.IsCurrentArea)
		{
			UpdateBellyObjects();
			if ((NetworkObject)(object)monster_0 != (NetworkObject)null)
			{
				await Helpers.MoveAndWait(((NetworkObject)(object)monster_0).WalkablePosition());
				return true;
			}
			if ((NetworkObject)(object)npc_0 != (NetworkObject)null)
			{
				if (!((NetworkObject)npc_0).IsTargetable)
				{
					await Helpers.MoveAndWait(((NetworkObject)(object)npc_0).WalkablePosition());
					return true;
				}
				if (((NetworkObject)npc_0).HasNpcFloatingIcon)
				{
					WalkablePosition pietyPos = ((NetworkObject)(object)npc_0).WalkablePosition();
					if (pietyPos.IsFar)
					{
						pietyPos.Come();
					}
					else
					{
						await Helpers.TalkTo((NetworkObject)(object)npc_0);
					}
					return true;
				}
				if (string_0 != ((NetworkObject)LokiPoe.Me).Name)
				{
					int_0 = 0;
					string_0 = ((NetworkObject)LokiPoe.Me).Name;
				}
				if (int_0 < 5)
				{
					int_0++;
					await Helpers.TalkTo((NetworkObject)(object)npc_0);
					await Wait.StuckDetectionSleep(1000);
					return true;
				}
			}
		}
		if (!World.Act4.Harvest.IsCurrentArea)
		{
			await Travel.To(World.Act4.Harvest);
		}
		return true;
	}

	public static async Task<bool> TalkToTasuni()
	{
		if (!World.Act4.Highgate.IsCurrentArea)
		{
			if (!World.Act4.Harvest.IsCurrentArea)
			{
				if (!World.Act4.Harvest.IsCurrentArea)
				{
					await Travel.To(World.Act4.Highgate);
				}
				return true;
			}
			UpdateMalachaiFightObjects();
			if ((NetworkObject)(object)areaTransition_2 != (NetworkObject)null && ((NetworkObject)(object)areaTransition_2).PathExists())
			{
				if (!(await PlayerAction.TakeTransition(areaTransition_2)))
				{
					ErrorManager.ReportError();
				}
			}
			else
			{
				await Travel.To(World.Act4.Highgate);
			}
			return true;
		}
		await TownNpcs.Tasuni.Position.ComeAtOnce();
		NetworkObject tasuniObj = TownNpcs.Tasuni.NpcObject;
		if (!tasuniObj.HasNpcFloatingIcon)
		{
			EscapeState.LogoutToCharacterSelection();
			return false;
		}
		await Helpers.TalkTo(tasuniObj);
		return true;
	}

	private static void UpdateMalachaiFightObjects()
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		npc_1 = null;
		networkObject_0 = null;
		networkObject_1 = null;
		areaTransition_0 = null;
		areaTransition_1 = null;
		areaTransition_2 = null;
		monster_1 = null;
		monster_2 = null;
		monster_3 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			string metadata = @object.Metadata;
			AreaTransition val = (AreaTransition)(object)((@object is AreaTransition) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null)
			{
				if (!metadata.Contains("Act4/CoreTransition"))
				{
					if (metadata.Contains("Act4/MalachaiDeathPortal"))
					{
						areaTransition_2 = val;
					}
					else if (((NetworkObject)val).Name == "The Black Heart")
					{
						areaTransition_1 = val;
					}
				}
				else
				{
					areaTransition_0 = val;
				}
				continue;
			}
			Monster val2 = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val2 != (NetworkObject)null) || (int)val2.Rarity != 3)
			{
				Npc val3 = (Npc)(object)((@object is Npc) ? @object : null);
				if ((NetworkObject)(object)val3 != (NetworkObject)null)
				{
					if (@object.Metadata.Contains("Act4/PietyHarvest"))
					{
						npc_1 = val3;
					}
				}
				else if (!(metadata == "Metadata/Monsters/Malachai/ArenaMiddle"))
				{
					if (metadata.Contains("Act4/CoreMouth"))
					{
						networkObject_1 = @object;
					}
				}
				else
				{
					networkObject_0 = @object;
				}
			}
			else if (!metadata.Contains("Malachai/MalachaiBoss"))
			{
				if (metadata.Contains("Malachai/BeastHeart"))
				{
					monster_3 = val2;
				}
				else if (metadata.Contains("Axis/Piety"))
				{
					monster_1 = val2;
				}
			}
			else
			{
				monster_2 = val2;
			}
		}
	}

	private static void UpdateBellyObjects()
	{
		monster_0 = null;
		npc_0 = null;
		foreach (NetworkObject @object in ObjectManager.Objects)
		{
			Monster val = (Monster)(object)((@object is Monster) ? @object : null);
			if (!((NetworkObject)(object)val != (NetworkObject)null))
			{
				Npc val2 = (Npc)(object)((@object is Npc) ? @object : null);
				if ((NetworkObject)(object)val2 != (NetworkObject)null && ((NetworkObject)val2).Metadata.Contains("Act4/PietyBelly"))
				{
					npc_0 = val2;
				}
			}
			else if (((NetworkObject)val).Metadata.Contains("Axis/PietyBeastBoss"))
			{
				monster_0 = val;
			}
		}
	}

	static A4_Q5_EternalNightmare()
	{
		tgtPosition_0 = new TgtPosition("Shavronne of Umbra", "shavronn_Area_v01_01.tgt");
		tgtPosition_1 = new TgtPosition("Doedre Darktongue", "DoedreStones_Area_v01_01.tgt");
		mfsuwvJrmr = new TgtPosition("Maligaro, The Inquisitor", "Maligaro_Area_v01_01.tgt");
	}
}
