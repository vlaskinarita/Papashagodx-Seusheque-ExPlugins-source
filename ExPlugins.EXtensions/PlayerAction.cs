using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Controllers;
using DreamPoeBot.Loki.Coroutine;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Positions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.EXtensions;

public static class PlayerAction
{
	public static async Task<bool> OpenWaypoint()
	{
		WalkablePosition wpPos;
		if (World.CurrentArea.IsTown)
		{
			wpPos = StaticPositions.GetWaypointPosByAct();
		}
		else
		{
			NetworkObject wpObj = ObjectManager.Waypoint;
			if (wpObj == (NetworkObject)null)
			{
				GlobalLog.Error("[OpenWaypoint] Fail to find any Waypoint nearby.");
				return false;
			}
			wpPos = wpObj.WalkablePosition();
		}
		if (wpPos.Distance > 20 && ExtensionsSettings.Instance.HumanizerNew && LokiPoe.CurrentWorldArea.IsTown && Wait.WaypointPauseProbability(70))
		{
			WorldPosition pos = WorldPosition.FindRandomPositionForMove(((NetworkObject)LokiPoe.Me).Position);
			if (pos != null)
			{
				await Move.AtOnce(pos, "To Random Location and pause.");
				await Wait.TownMoveRandomDelay();
			}
		}
		await EnableAlwaysHighlight();
		await wpPos.ComeAtOnce();
		await Interact(ObjectManager.Waypoint, () => WorldUi.IsOpened || GlobalWarningDialog.IsBetrayalLeaveZoneWarningOverlayOpen, "wold panel opening");
		if (GlobalWarningDialog.IsBetrayalLeaveZoneWarningOverlayOpen)
		{
			GlobalLog.Info("[CloseBlockingWindows] IsBetrayalLeaveZoneWarningOverlayOpen == true. Click Yes.");
			GlobalWarningDialog.ConfirmDialog(true);
		}
		await Wait.SleepSafe(200);
		return WorldUi.IsOpened;
	}

	public static async Task<bool> MoveAway(int min, int max)
	{
		int offsetX2 = LokiPoe.Random.Next(-max, max);
		int offsetY2 = LokiPoe.Random.Next(-max, max);
		Vector2i vect2 = new Vector2i(LokiPoe.MyPosition.X + offsetX2, LokiPoe.MyPosition.Y + offsetY2);
		WalkablePosition pos = new WalkablePosition("move away position", vect2, 5, 20);
		while (!pos.PathExists || (double)(int)Math.Round(pos.PathDistance) > (double)max * 1.2 || (int)Math.Round(pos.PathDistance) < min)
		{
			offsetX2 = LokiPoe.Random.Next(-max, max);
			offsetY2 = LokiPoe.Random.Next(-max, max);
			vect2 = new Vector2i(LokiPoe.MyPosition.X + offsetX2, LokiPoe.MyPosition.Y + offsetY2);
			pos = new WalkablePosition("move away position", vect2, 5, 20);
		}
		return await pos.TryComeAtOnce(10);
	}

	public static async Task<bool> Interact(NetworkObject obj, Func<bool> success, string desc, int timeout = 3000)
	{
		if (!(obj == (NetworkObject)null))
		{
			WalkablePosition walkPos = obj.WalkablePosition();
			walkPos.Initialize();
			if (obj.Distance > 35f)
			{
				await walkPos.TryComeAtOnce(10);
			}
			await Coroutines.FinishCurrentAction(true);
			await Coroutines.CloseBlockingWindows();
			if (!(await FasterInteract(obj)))
			{
				GlobalLog.Error($"[Interact] Fail to interact with \"{walkPos}\".");
				await Coroutines.CloseBlockingWindows();
				if (World.CurrentArea.IsCombatArea && (NetworkObject)(object)AnyPortalInRangeOf(20) != (NetworkObject)null)
				{
					GlobalLog.Error("[Interact] Seems like portal is covering the object. Let's try to open new one at some distance.");
					await MoveAway(40, 60);
					await CreateTownPortal();
				}
				return false;
			}
			return await Wait.For(success, desc, 20, timeout);
		}
		GlobalLog.Error("[Interact] Object for interaction is null.");
		return false;
	}

	public static async Task<bool> Interact(NetworkObject obj)
	{
		if (!(obj == (NetworkObject)null))
		{
			WalkablePosition walkPos = obj.WalkablePosition();
			walkPos.Initialize();
			if (obj.Distance > 35f)
			{
				await walkPos.TryComeAtOnce(10);
			}
			if (await FasterInteract(obj))
			{
				return true;
			}
			GlobalLog.Error($"[Interact] Fail to interact with \"{walkPos}\".");
			await Coroutines.CloseBlockingWindows();
			if (World.CurrentArea.IsCombatArea && (NetworkObject)(object)AnyPortalInRangeOf(20) != (NetworkObject)null)
			{
				GlobalLog.Error("[Interact] Seems like portal is covering the object. Let's try to open new one at some distance.");
				await MoveAway(40, 60);
				await CreateTownPortal();
			}
			return false;
		}
		GlobalLog.Error("[Interact] Object for interaction is null.");
		return false;
	}

	public static async Task<bool> Interact(NetworkObject obj, int attempts)
	{
		if (obj == (NetworkObject)null)
		{
			GlobalLog.Error("[Interact] Object for interaction is null.");
			return false;
		}
		for (int i = 1; i <= attempts; i++)
		{
			if (!LokiPoe.IsInGame)
			{
				break;
			}
			if (((Actor)LokiPoe.Me).IsDead)
			{
				break;
			}
			await Coroutines.FinishCurrentAction(true);
			WalkablePosition walkPos = obj.WalkablePosition();
			walkPos.Initialize();
			if (obj.Distance > 35f)
			{
				await walkPos.TryComeAtOnce(10);
			}
			if (!(await FasterInteract(obj)))
			{
				GlobalLog.Error($"[Interact] Fail to interact with \"{walkPos}\". Attempt: {i}/{attempts}.");
				await Coroutines.CloseBlockingWindows();
				if (World.CurrentArea.IsCombatArea && (NetworkObject)(object)AnyPortalInRangeOf(20) != (NetworkObject)null)
				{
					GlobalLog.Error("[Interact] Seems like portal is covering the object. Let's try to open new one at some distance.");
					await MoveAway(40, 60);
					await CreateTownPortal();
				}
				continue;
			}
			return true;
		}
		return false;
	}

	public static async Task<bool> Logout(bool toTitle = true)
	{
		GlobalLog.Debug("[Logout] Now going to log out.");
		bool isElevated = false;
		SecurityIdentifier securityIdentifier = WindowsIdentity.GetCurrent().Owner;
		if (securityIdentifier != null)
		{
			isElevated = securityIdentifier.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
		}
		if (isElevated)
		{
			GlobalLog.Debug("[Logout] DPB is elevated. Now going to kill TCP connection");
			Class41.KillTcpConnectionForProcess(LokiPoe.Memory.Process.Id);
			await Wait.SleepSafe(15);
		}
		if (!LokiPoe.IsInLoginScreen)
		{
			LogoutError err = EscapeState.LogoutToTitleScreen();
			if ((int)err > 0)
			{
				GlobalLog.Error($"[Logout] Fail to log out. Error: \"{err}\".");
				return false;
			}
		}
		return await Wait.For(() => LokiPoe.IsInLoginScreen, "log out", 500, 5000);
	}

	public static async Task<bool> TryTo(Func<Task<bool>> action, string desc, int attempts, int interval = 1000)
	{
		int i = 1;
		while (i <= attempts && LokiPoe.IsInGame && !((Actor)LokiPoe.Me).IsDead)
		{
			if (desc != null)
			{
				GlobalLog.Debug($"[TryTo] {desc} attempt: {i}/{attempts}");
			}
			if (await action())
			{
				return true;
			}
			await Wait.SleepSafe(interval);
			int num = i + 1;
			i = num;
		}
		return false;
	}

	public static async Task<bool> GoToHideout()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsMyHideoutArea)
		{
			return true;
		}
		if (area.IsMap)
		{
			await TpToTown();
			return false;
		}
		bool res = ((!ExtensionsSettings.Instance.UseChatForHideout || ExtensionsSettings.Instance.ForceWaypointHideout || (!area.IsTown && !area.IsMenagerieArea && (area.IsMyHideoutArea || !area.IsHideoutArea))) ? (await GoToHideoutViaWaypoint()) : (await GoToHideoutViaCommand()));
		if (res)
		{
			await LoadHideoutTemplate();
		}
		return res;
	}

	private static async Task<bool> GoToHideoutViaCommand()
	{
		GlobalLog.Debug("[GoToHideoutViaCommand] Now going to hideout via chat command.");
		uint areaHash = LocalData.AreaHash;
		GlobalLog.Debug($"[PlayerAction] areHash: {areaHash}");
		ChatResult err = Commands.hideout(true);
		if ((int)err > 0)
		{
			GlobalLog.Error($"[GoToHideoutViaCommand] Fail to use /hideout command. Fail-safe activated. Error: \"{err}\".");
			GlobalLog.Warn("[GoToHideoutViaCommand] Using waypoints for this session.");
			ExtensionsSettings.Instance.ForceWaypointHideout = true;
			return false;
		}
		bool changed = await Wait.ForHideoutChange();
		if (changed)
		{
			GlobalLog.Debug("[PlayerAction] changed: true");
		}
		else
		{
			GlobalLog.Error("[GoToHideoutViaCommand] Wait.ForAreaChange failed. Fail-safe activated.");
		}
		return changed;
	}

	private static async Task<bool> GoToHideoutViaWaypoint()
	{
		bool flag = !WorldUi.IsOpened;
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = !(await OpenWaypoint());
		}
		if (!flag2)
		{
			GlobalLog.Debug("[GoToHideoutViaWaypoint] Now going to take a waypoint to hideout.");
			TakeWaypointResult err = WorldUi.GoToHideout();
			if ((int)err <= 0)
			{
				return await Wait.ForHideoutChange();
			}
			GlobalLog.Error($"[GoToHideoutViaWaypoint] Fail to take a waypoint to hideout. Error: \"{err}\".");
			return false;
		}
		GlobalLog.Error("[GoToHideoutViaWaypoint] Fail to open a waypoint.");
		return false;
	}

	public static async Task<bool> TpToTown(bool forceNewPortal = false, bool repeatUntilInTown = true)
	{
		if (ErrorManager.GetErrorCount("TpToTown") > 5)
		{
			GlobalLog.Debug("[TpToTown] We failed to take a portal to town more than 5 times. Now going to log out.");
			return await Logout();
		}
		GlobalLog.Debug("[TpToTown] Now going to open and take a portal to town.");
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea)
		{
			GlobalLog.Error("[TpToTown] We are already in town/hideout.");
			return false;
		}
		bool conqFightingArena = AtlasHelper.IsAtlasBossPresent && ObjectManager.GetObjectsByMetadata("Metadata/Monsters/AtlasExiles/ArenaMechanics/AtlasExileArenaBlocker").Any();
		Portal portal2 = default(Portal);
		if ((!area.IsOverworldArea && !area.IsMap && !area.IsCorruptedArea && !area.IsMapRoom && !area.IsTempleOfAtzoatl && area.Name != "Syndicate Hideout") || area.Id == "MavenHub" || area.Name == "Absence of Patience and Wisdom" || area.Name == "Absence of Symmetry and Harmony" || area.Name == "Polaric Void" || area.Name == "Seething Chyme" || conqFightingArena)
		{
			Portal val;
			portal2 = (val = AnyPortalInRangeOf(200));
			if ((NetworkObject)(object)val != (NetworkObject)null)
			{
				GlobalLog.Debug($"[TpToTown] There is a ready-to-use portal at a distance of {((NetworkObject)portal2).Distance}. Now going to take it.");
				if (!(await TakePortal(portal2)))
				{
					GlobalLog.Debug($"[TpToTown] {area.Name} Taking {((NetworkObject)(object)portal2).WalkablePosition()} failed now going to relog.");
					return await Logout();
				}
				return true;
			}
			GlobalLog.Debug("[TpToTown] Cannot create portals in this area (" + area.Name + "). Now going to log out.");
			return await Logout();
		}
		int num;
		if (forceNewPortal)
		{
			num = 1;
		}
		else
		{
			Portal val;
			portal2 = (val = PlayerPortalInRangeOf(200));
			num = (((NetworkObject)(object)val == (NetworkObject)null) ? 1 : 0);
		}
		if (num == 0)
		{
			GlobalLog.Debug($"[TpToTown] There is a ready-to-use portal at a distance of {((NetworkObject)portal2).Distance}. Now going to take it.");
		}
		else
		{
			portal2 = await CreateTownPortal();
			if ((NetworkObject)(object)portal2 == (NetworkObject)null)
			{
				GlobalLog.Error("[TpToTown] Fail to create a new town portal. Now going to log out to character selection.");
				EscapeState.LogoutToCharacterSelection();
			}
		}
		if (await TakePortal(portal2))
		{
			DatWorldAreaWrapper newArea = World.CurrentArea;
			if (!repeatUntilInTown || !newArea.IsCombatArea)
			{
				GlobalLog.Debug("[TpToTown] We have been successfully teleported from \"" + area.Name + "\" to \"" + newArea.Name + "\".");
				return true;
			}
			GlobalLog.Debug("[TpToTown] After taking a portal we appeared in another combat area (" + newArea.Name + "). Now calling TpToTown again.");
			return await TpToTown(forceNewPortal);
		}
		ErrorManager.ReportError("TpToTown");
		return false;
	}

	public static async Task<bool> TakeTransition(AreaTransition transition, bool newInstance = false)
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (!((NetworkObject)(object)transition == (NetworkObject)null))
		{
			WalkablePosition pos;
			switch (((NetworkObject)transition).Name)
			{
			case "Shrine of the Winds":
				pos = ((NetworkObject)(object)transition).WalkablePosition(7, 10);
				break;
			default:
				pos = ((NetworkObject)(object)transition).WalkablePosition();
				break;
			case "The Chamber of Sins Level 2":
			case "Tukohama's Keep":
				pos = ((NetworkObject)(object)transition).WalkablePosition(2, 5);
				break;
			case "The Crypt Level 1":
				pos = ((NetworkObject)(object)transition).WalkablePosition(1, 5);
				break;
			}
			TransitionTypes type = transition.TransitionType;
			GlobalLog.Debug("[TakeTransition] Now going to enter \"" + pos.Name + "\".");
			if (((NetworkObject)transition).Name == "Shrine of the Winds")
			{
				await pos.ComeAtOnce(13);
			}
			else if (((NetworkObject)transition).Name == "Altar of Hunger")
			{
				await new WalkablePosition("Unstuck Position", new Vector2i(1834, 3001), 2, 5).ComeAtOnce(6);
			}
			else if (((NetworkObject)transition).Name == "The Chamber of Sins Level 2")
			{
				await pos.ComeAtOnce(9);
			}
			else if (!(((NetworkObject)transition).Name == "The Crypt Level 1") && (!(((NetworkObject)transition).Name == "The Crypt") || !(area.Id == "2_7_3")))
			{
				if (!(area.Name == "The Sceptre of God") && !(area.Name == "The Upper Sceptre of God"))
				{
					if (((NetworkObject)transition).Name == "Tukohama's Keep")
					{
						if (!(((NetworkObject)transition).Position == new Vector2i(1022, 350)))
						{
							await new WalkablePosition("Unstuck Position", new Vector2i(1232, 323), 2, 5).ComeAtOnce(8);
						}
						else
						{
							await new WalkablePosition("Unstuck Position", new Vector2i(1024, 315), 2, 5).ComeAtOnce(8);
						}
					}
					else if (!(((NetworkObject)transition).Name == "The Quay"))
					{
						await pos.ComeAtOnce();
					}
					else
					{
						await new WalkablePosition("Unstuck Position", new Vector2i(((NetworkObject)transition).Position.X + 10, ((NetworkObject)transition).Position.Y - 10), 2, 6).ComeAtOnce(6);
					}
				}
				else if (((NetworkObject)transition).Name == "Tower Rooftop")
				{
					await new WalkablePosition("Unstuck Position", new Vector2i(2691, 423), 2, 5).ComeAtOnce(6);
				}
				else
				{
					await pos.ComeAtOnce(13);
				}
			}
			else
			{
				await new WalkablePosition("Unstuck Position", new Vector2i(((NetworkObject)transition).Position.X - 40, ((NetworkObject)transition).Position.Y), 2, 5).ComeAtOnce(5);
			}
			await Coroutines.FinishCurrentAction(true);
			await Wait.SleepSafe(100);
			uint hash = LocalData.AreaHash;
			Vector2i vector2i_0 = LokiPoe.MyPosition;
			if ((!newInstance) ? (await Interact((NetworkObject)(object)transition)) : (await CreateNewInstance((NetworkObject)(object)transition)))
			{
				if ((int)type == 1)
				{
					if (!(await Wait.For(() => ((Actor)LokiPoe.Me).HasAura("Grace Period") || ((Vector2i)(ref vector2i_0)).Distance(LokiPoe.MyPosition) > 15, "local transition", 500, 5000)))
					{
						GlobalLog.Error("Grace: " + string.Format("{0}, ", ((Actor)LokiPoe.Me).HasAura("Grace Period")) + $"LastPos: {vector2i_0}, CurrentPos: {((NetworkObject)LokiPoe.Me).Position}, " + $"Distance: {((Vector2i)(ref vector2i_0)).Distance(LokiPoe.MyPosition)}");
						return false;
					}
					Utility.BroadcastMessage((object)null, "local_transition_entered", Array.Empty<object>());
					await Wait.SleepSafe(100);
				}
				else if (!(await Wait.ForAreaChange(hash)))
				{
					return false;
				}
				GlobalLog.Debug("[TakeTransition] \"" + pos.Name + "\" has been successfully entered.");
			}
			return true;
		}
		GlobalLog.Error("[TakeTransition] Transition object is null.");
		return false;
	}

	public static async Task<bool> TakeTransitionByName(string name, bool newInstance = false)
	{
		AreaTransition transition = ((IEnumerable)ObjectManager.Objects).Closest<AreaTransition>((Func<AreaTransition, bool>)((AreaTransition a) => ((NetworkObject)a).Name == name));
		return await TakeTransition(transition, newInstance);
	}

	public static async Task<bool> TakeWaypoint(AreaInfo area, bool newInstance = false)
	{
		if (!WorldUi.IsOpened && !(await OpenWaypoint()))
		{
			GlobalLog.Error("[TakeWaypoint] Fail to open a waypoint.");
			return false;
		}
		GlobalLog.Debug($"[TakeWaypoint] Now going to take a waypoint to {area}");
		uint areaHash = LocalData.AreaHash;
		TakeWaypointResult err = WorldUi.TakeWaypoint(area.Id, newInstance, -1, true);
		if ((int)err <= 0)
		{
			return await Wait.ForAreaChange(areaHash);
		}
		GlobalLog.Error($"[TakeWaypoint] Fail to take a waypoint to {area}. Error: \"{err}\".");
		return false;
	}

	private static async Task<bool> CreateNewInstance(NetworkObject transition)
	{
		string name = transition.Name;
		if (!(await FasterInteract(transition, holdCtrl: true)))
		{
			GlobalLog.Error("[CreateNewInstance] Fail to interact with \"" + name + "\" transition.");
			return false;
		}
		if (await Wait.For(() => InstanceManagerUi.IsOpened, "instance manager opening"))
		{
			GlobalLog.Debug("[CreateNewInstance] Creating new instance for \"" + name + "\".");
			JoinInstanceResult err = InstanceManagerUi.JoinNewInstance(true);
			if ((int)err <= 0)
			{
				GlobalLog.Debug("[CreateNewInstance] New instance for \"" + name + "\" has been successfully created.");
				return true;
			}
			GlobalLog.Error($"[CreateNewInstance] Fail to create a new instance. Error: \"{err}\".");
			return false;
		}
		return false;
	}

	private static Portal PlayerPortalInRangeOf(int range)
	{
		return ObjectManager.Portals.Closest<Portal>((Func<Portal, bool>)((Portal p) => p.IsPlayerPortal() && ((NetworkObject)p).Distance <= (float)range && p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsTown)));
	}

	public static Portal AnyPortalInRangeOf(int range)
	{
		return ObjectManager.Portals.Closest<Portal>((Func<Portal, bool>)((Portal p) => ((NetworkObject)p).Distance <= (float)range && p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsTown)));
	}

	public static Portal AnyPortalInRangeOf(int range, Vector2i pos)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return ObjectManager.Portals.Closest<Portal>((Func<Portal, bool>)delegate(Portal p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Vector2i position = ((NetworkObject)p).Position;
			return ((Vector2i)(ref position)).Distance(pos) <= range && p.LeadsTo((DatWorldAreaWrapper a) => a.IsHideoutArea || a.IsTown);
		});
	}

	public static async Task<Portal> CreateTownPortal()
	{
		Vector2i myPos = LokiPoe.MyPosition;
		int rand = LokiPoe.Random.Next(-25, 25);
		WalkablePosition walkable = new WalkablePosition("random pos", new Vector2i(myPos.X + rand, myPos.Y + rand), 5);
		if (((Actor)LokiPoe.Me).HasAura("Grace Period") && walkable.PathExists)
		{
			if (!(await walkable.TryComeAtOnce()))
			{
				GlobalLog.Error($"Move towards {walkable} failed!");
			}
			GlobalLog.Debug($"Move towards {walkable} success!");
		}
		Item portalScroll = (from i in Enumerable.Where(Inventories.InventoryItems, (Item i) => i.Name == CurrencyNames.Portal)
			orderby i.StackCount
			select i).FirstOrDefault();
		string areaName = World.CurrentArea.Name;
		if (areaName == "The Maven's Crucible")
		{
			GlobalLog.Error("[CreateTownPortal] Can't create portal in " + areaName + ". Skipping!");
			return null;
		}
		if (!((RemoteMemoryObject)(object)portalScroll == (RemoteMemoryObject)null))
		{
			int itemId = portalScroll.LocalId;
			ProcessHookManager.ClearAllKeyStates();
			if (await Inventories.OpenInventory())
			{
				UseItemResult err = InventoryUi.InventoryControl_Main.UseItem(itemId, true);
				if ((int)err > 0)
				{
					GlobalLog.Error($"[CreateTownPortal] Fail to use a Portal Scroll. Error: \"{err}\".");
					return null;
				}
				await Coroutines.CloseBlockingWindows();
				Portal portal_0 = null;
				walkable.TryCome();
				await Wait.For(() => (NetworkObject)(object)(portal_0 = PlayerPortalInRangeOf(40)) != (NetworkObject)null, "portal spawning", 50, 2000, log: true);
				return portal_0;
			}
			return null;
		}
		GlobalLog.Error("[CreateTownPortal] Out of portal scrolls.");
		return null;
	}

	public static async Task<bool> EnableAlwaysHighlight()
	{
		if (!ConfigManager.IsAlwaysHighlightEnabled)
		{
			GlobalLog.Info("[EnableAlwaysHighlight] Now enabling always highlight.");
			Input.SimulateKeyEvent(Binding.highlight_toggle, true, false, false, Keys.None);
			await Wait.For(() => ConfigManager.IsAlwaysHighlightEnabled, "enable always highlight", 5, 55);
			await Wait.SleepSafe(10);
			return ConfigManager.IsAlwaysHighlightEnabled;
		}
		return true;
	}

	public static async Task<bool> DisableAlwaysHighlight()
	{
		if (ConfigManager.IsAlwaysHighlightEnabled)
		{
			GlobalLog.Info("[DisableAlwaysHighlight] Now disabling always highlight.");
			Input.SimulateKeyEvent(Binding.highlight_toggle, true, false, false, Keys.None);
			await Wait.For(() => !ConfigManager.IsAlwaysHighlightEnabled, "disable always highlight", 5, 55);
			await Wait.SleepSafe(10);
			return !ConfigManager.IsAlwaysHighlightEnabled;
		}
		return true;
	}

	public static async Task<bool> TakePortal(Portal portal)
	{
		if (!((NetworkObject)(object)portal == (NetworkObject)null))
		{
			WalkablePosition pos = ((NetworkObject)(object)portal).WalkablePosition();
			await DisableAlwaysHighlight();
			await pos.ComeAtOnce();
			await Wait.SleepSafe(200);
			GlobalLog.Debug("[TakePortal] Now going to take portal to \"" + pos.Name + "\".");
			uint hash = LocalData.AreaHash;
			if (!(await Interact((NetworkObject)(object)portal, () => !LokiPoe.IsInGame, "loading screen")))
			{
				return false;
			}
			if (await Wait.ForAreaChange(hash))
			{
				GlobalLog.Debug("[TakePortal] Portal to \"" + pos.Name + "\" has been successfully taken.");
				return true;
			}
			return false;
		}
		GlobalLog.Error("[TakePortal] Portal object is null.");
		return false;
	}

	public static async Task<bool> LoadHideoutTemplate()
	{
		if (!string.IsNullOrEmpty(ExtensionsSettings.Instance.HideoutTemplateDir))
		{
			if (ExtensionsSettings.Instance.HideoutTemplateAmount == 0)
			{
				string filename2 = LokiPoe.CurrentWorldArea.Name.Replace('\'', ' ').Replace(' ', '_') + ".hideout";
				string dir2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExtensionsSettings.Instance.HideoutTemplateDir);
				string fileFullPath2 = Path.Combine(dir2, filename2);
				string statename2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "State", "who.txt");
				if (!File.Exists(fileFullPath2) || File.Exists(statename2))
				{
					GlobalLog.Error("[OpenMapTask] the file " + fileFullPath2 + " do not exist.");
					return false;
				}
				ImportLayoutError ret2 = HideoutEditorUi.ImportLayout(dir2, filename2);
				if ((int)ret2 != 0)
				{
					GlobalLog.Error($"[OpenMapTask] ImportLayout returned {ret2}.");
					return false;
				}
				File.WriteAllText(statename2, "ti pidor");
				await Wait.SleepSafe(LokiPoe.Random.Next(500, 1000));
			}
			else
			{
				string filename = string.Format(arg1: LokiPoe.Random.Next(1, ExtensionsSettings.Instance.HideoutTemplateAmount), format: "{0}{1}.hideout", arg0: LokiPoe.CurrentWorldArea.Name.Replace('\'', ' ').Replace(' ', '_'));
				string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExtensionsSettings.Instance.HideoutTemplateDir);
				string fileFullPath = Path.Combine(dir, filename);
				string statename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "State", "who.txt");
				if (!File.Exists(fileFullPath) || File.Exists(statename))
				{
					GlobalLog.Error("[OpenMapTask] the file " + fileFullPath + " do not exist.");
					return false;
				}
				ImportLayoutError ret = HideoutEditorUi.ImportLayout(dir, filename);
				if ((int)ret != 0)
				{
					GlobalLog.Error($"[OpenMapTask] ImportLayout returned {ret}.");
					return false;
				}
				File.WriteAllText(statename, "ti pidor");
				await Wait.SleepSafe(LokiPoe.Random.Next(500, 1000));
			}
			return true;
		}
		GlobalLog.Error("[OpenMapTask] No directory configured in MapBot settings.");
		return false;
	}

	public static async Task<bool> FastTravelTo(string characterName)
	{
		await Coroutines.CloseBlockingWindows();
		FastGoToZoneResult res = PartyHud.FastGoToZone(characterName);
		if ((int)res != 0)
		{
			GlobalLog.Error($"[PlayerAction] FastGoToZone error: {res}");
			return false;
		}
		if (GlobalWarningDialog.IsOpened)
		{
			GlobalWarningDialog.ConfirmDialog(true);
		}
		if (!(await Wait.For(() => StateManager.IsAreaLoadingStateActive, "loading screen", 25, 2500)))
		{
			return false;
		}
		return await Wait.ForAreaChange(ExilePather.AreaHash, 20000);
	}

	public static async Task<bool> FasterInteract(NetworkObject obj, bool holdCtrl = false)
	{
		if (!(obj == (NetworkObject)null))
		{
			bool isItem = obj is WorldItem;
			string name = obj.Name;
			CachedObject cachedObject_0 = new CachedObject(obj);
			if (isItem)
			{
				await DisableAlwaysHighlight();
			}
			else if (await Coroutines.InteractWith(obj, holdCtrl))
			{
				return true;
			}
			ProcessHookManager.ClearAllKeyStates();
			await Coroutines.FinishCurrentAction(true);
			await Coroutines.CloseBlockingWindows();
			bool useHighlight = (int)Binding.KeyPickup == 2 && isItem;
			bool useBound = (int)Binding.KeyPickup == 3 && isItem;
			Vector2i point = ((Entity)obj.Entity).Position;
			bool found = false;
			if (holdCtrl)
			{
				ProcessHookManager.SetKeyState(Keys.ControlKey, short.MinValue, Keys.None);
			}
			if (useBound)
			{
				ProcessHookManager.SetKeyState(Binding.enable_key_pickup_combo.Key, short.MinValue, Binding.enable_key_pickup_combo.Modifier);
			}
			if (useHighlight)
			{
				ProcessHookManager.SetKeyState(Binding.highlight_combo.Key, short.MinValue, Binding.highlight_combo.Modifier);
			}
			await Coroutine.Sleep(5);
			MouseManager.SetMousePos((string)null, point, true);
			await Coroutine.Sleep(5);
			if (GameController.Instance.Game.IngameState.FrameUnderCursor == ((RemoteMemoryObject)obj.Entity).Address)
			{
				found = true;
			}
			MouseManager.ClickLMB(0, 0);
			if (!found)
			{
				await Wait.LatencySleep();
				if (!isItem || !(cachedObject_0.Object != (NetworkObject)null))
				{
					return await Coroutines.InteractWith(obj, holdCtrl);
				}
				await EnableAlwaysHighlight();
				await Coroutines.InteractWith(obj, holdCtrl);
			}
			if (holdCtrl)
			{
				ProcessHookManager.SetKeyState(Keys.ControlKey, (short)0, Keys.None);
			}
			if (useBound)
			{
				ProcessHookManager.SetKeyState(Binding.enable_key_pickup_combo.Key, (short)0, Binding.enable_key_pickup_combo.Modifier);
			}
			if (useHighlight)
			{
				ProcessHookManager.SetKeyState(Binding.highlight_combo.Key, (short)0, Binding.highlight_combo.Modifier);
			}
			if (!isItem)
			{
				return true;
			}
			return await Wait.For(() => cachedObject_0.Object == (NetworkObject)null, name + " pick up", 10, 1000);
		}
		return false;
	}

	public static async Task<bool> FastPickup(WorldItem item)
	{
		if ((NetworkObject)(object)item == (NetworkObject)null)
		{
			return false;
		}
		if (!(await EnableAlwaysHighlight()))
		{
			GlobalLog.Error("[FastPickup] Couldn't enable AlwaysHighlight");
			return false;
		}
		await Coroutines.CloseBlockingWindows();
		WorldItemLabelClass label = item.WorldItemLabel;
		bool useHighlight = false;
		bool useBound = false;
		string name = ((NetworkObject)item).Name;
		CachedObject cachedObject_0 = new CachedObject((NetworkObject)(object)item);
		if (label != null)
		{
			ProcessHookManager.ClearAllKeyStates();
			await Coroutines.FinishCurrentAction(true);
			if ((int)Binding.KeyPickup == 2)
			{
				ProcessHookManager.SetKeyState(Binding.highlight_combo.Key, short.MinValue, Binding.highlight_combo.Modifier);
				useHighlight = true;
			}
			if ((int)Binding.KeyPickup == 3)
			{
				ProcessHookManager.SetKeyState(Binding.enable_key_pickup_combo.Key, short.MinValue, Binding.enable_key_pickup_combo.Modifier);
				useBound = true;
			}
			for (int i = 1; i < 6; i++)
			{
				Vector2i point = new Vector2i((int)(label.Coordinate.X + label.Size.X / 7f * (float)i), (int)(label.Coordinate.Y + label.Size.Y / 2f));
				MouseManager.SetMousePosition(point, false);
				await Coroutine.Sleep(15);
				if (GameController.Instance.Game.IngameState.FrameUnderCursor == ((RemoteMemoryObject)((NetworkObject)item).Entity).Address)
				{
					break;
				}
			}
			MouseManager.ClickLMB(0, 0);
			if (useHighlight)
			{
				ProcessHookManager.SetKeyState(Binding.highlight_combo.Key, (short)0, Binding.highlight_combo.Modifier);
			}
			if (useBound)
			{
				ProcessHookManager.SetKeyState(Binding.enable_key_pickup_combo.Key, (short)0, Binding.enable_key_pickup_combo.Modifier);
			}
			if (await Wait.For(() => cachedObject_0.Object == (NetworkObject)null, name + " pick up", 10, 1000))
			{
				return true;
			}
			return await FasterInteract((NetworkObject)(object)item);
		}
		return true;
	}
}
