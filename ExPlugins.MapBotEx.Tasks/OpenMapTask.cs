using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx.Tasks;

public class OpenMapTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents {
    public static class MapDevice {
        public static bool IsOpen => (World.CurrentArea.IsMyHideoutArea || World.CurrentArea.Name == "Karui Shores") ? MasterDeviceUi.IsOpened : MapDeviceUi.IsOpened;

        public static InventoryControlWrapper InventoryControl => (World.CurrentArea.IsMyHideoutArea || World.CurrentArea.Name == "Karui Shores") ? MasterDeviceUi.InventoryControl : MapDeviceUi.InventoryControl;
    }

    internal static bool Enabled;

    public string Name => "OpenMapTask";

    public string Description => "Task for opening maps via Map Device.";

    public string Author => "ExVault";

    public string Version => "1.0";

    public async Task<bool> Run() {
        if (!Enabled) {
            return false;
        }
        if (LokiPoe.IsInGame) {
            if (!World.CurrentArea.IsCombatArea) {
                if (SkillsUi.IsOpened || AtlasSkillsUi.IsOpened) {
                    return false;
                }
                await AtlasHelper.SocketVoidstones();
                Item mavenInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Maven's Invitation") && i.Class == "QuestItem");
                Item writhingInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Writhing Invitation") && i.Class == "QuestItem");
                Item screamingInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Screaming Invitation") && i.Class == "QuestItem");
                Item polaricInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Polaric Invitation") && i.Class == "QuestItem");
                Item incandescentInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Incandescent Invitation") && i.Class == "QuestItem");
                Item simulacrum = Inventories.InventoryItems.Find((Item i) => i.Metadata.Contains("CurrencyAfflictionFragment") && i.Class == "MapFragment");
                if (!((RemoteMemoryObject)(object)simulacrum != (RemoteMemoryObject)null) || !GeneralSettings.SimulacrumsEnabled) {
                    if (!((RemoteMemoryObject)(object)mavenInvitation != (RemoteMemoryObject)null) || !GeneralSettings.Instance.OpenMavenIvitations) {
                        if (!((RemoteMemoryObject)(object)writhingInvitation != (RemoteMemoryObject)null) || !GeneralSettings.Instance.OpenMiniBossQuestInvitations) {
                            if (!((RemoteMemoryObject)(object)screamingInvitation != (RemoteMemoryObject)null) || !GeneralSettings.Instance.OpenBossQuestInvitations) {
                                if (!((RemoteMemoryObject)(object)polaricInvitation != (RemoteMemoryObject)null) || !GeneralSettings.Instance.OpenMiniBossQuestInvitations) {
                                    if (!((RemoteMemoryObject)(object)incandescentInvitation != (RemoteMemoryObject)null) || !GeneralSettings.Instance.OpenBossQuestInvitations) {
                                        Item item_ = Inventories.InventoryItems.Find((Item i) => i.IsMap());
                                        if ((RemoteMemoryObject)(object)item_ == (RemoteMemoryObject)null) {
                                            GlobalLog.Error("[OpenMapTask] There is no map in inventory.");
                                            Enabled = false;
                                            return true;
                                        }
                                        if (!(await PlayerAction.TryTo(OpenDevice, "Open Map Device", 3, 2000))) {
                                            ErrorManager.ReportError();
                                            if (!World.CurrentArea.IsCombatArea) {
                                                return true;
                                            }
                                            GlobalLog.Error("[OpenMapTask] The bot is in a combat area. Return false to continue.");
                                            return false;
                                        }
                                        if (!MasterDeviceUi.IsFiveSlotDevice && GeneralSettings.Instance.UseFiveSlotMapDevice) {
                                            GlobalLog.Error("[OpenMapTask] Device is not five-slot! Setting UseFiveSlotMapDevice to false!");
                                            GeneralSettings.Instance.UseFiveSlotMapDevice = false;
                                        }
                                        if (MasterDeviceUi.IsFiveSlotDevice && !GeneralSettings.Instance.UseFiveSlotMapDevice) {
                                            GlobalLog.Warn("[OpenMapTask] Device is five-slot! Setting UseFiveSlotMapDevice to true!");
                                            GeneralSettings.Instance.UseFiveSlotMapDevice = true;
                                        }
                                        if (AtlasWarningDialog.IsOpened) {
                                            AtlasWarningDialog.ConfirmDialog(true);
                                        }
                                        if (MasterDeviceUi.IsAdditionalModifierPannelOpen) {
                                            MasterDeviceUi.ToggleAdditionalModifierPannel();
                                        }
                                        if (!(await CleanDevice())) {
                                            ErrorManager.ReportError();
                                            return true;
                                        }
                                        if (!(await PlayerAction.TryTo(() => PlaceIntoDevice(item_), "Place map into device", 3))) {
                                            ErrorManager.ReportError();
                                            return true;
                                        }
                                        if (GeneralSettings.Instance.SearingExarchInfluence && MasterDeviceUi.IsTheSearingExarchVisible && !EnableEye(ChooseExplorationInfluence("Exarch"))) {
                                            ErrorManager.ReportError();
                                            return false;
                                        }
                                        if (GeneralSettings.Instance.MavenInfluence && MasterDeviceUi.IsTheMavenInvitationVisible && !EnableEye(ChooseExplorationInfluence("Maven"))) {
                                            ErrorManager.ReportError();
                                            return false;
                                        }
                                        if (GeneralSettings.Instance.EaterOfWorldsInfluence && MasterDeviceUi.IsTheEaterOfWorldsVisible && !EnableEye(ChooseExplorationInfluence("Eater"))) {
                                            ErrorManager.ReportError();
                                            return false;
                                        }
                                        List<Item> nonSortedFragments = Inventories.InventoryItems.Where((Item i) => i.IsSacrificeFragment() || i.Name.Contains("Scarab")).ToList();
                                        List<Item> fragments = nonSortedFragments.OrderByDescending((Item i) => GeneralSettings.Instance.ScarabsToPrioritize.Any((NameEntry scar) => i.FullName.ContainsIgnorecase(scar.Name))).ToList();
                                        foreach (Item item_0 in fragments) {
                                            if ((!GeneralSettings.Instance.ScarabsToIgnore.Any((NameEntry s) => item_0.FullName.ContainsIgnorecase(s.Name)) || GeneralSettings.Instance.ScarabsToPrioritize.Any((NameEntry s) => item_0.FullName.ContainsIgnorecase(s.Name))) && (!MapDevice.InventoryControl.Inventory.Items.Any((Item i) => i.FullName.Contains("Shaper")) || !item_0.FullName.Contains("Elder")) && (!MapDevice.InventoryControl.Inventory.Items.Any((Item i) => i.FullName.Contains("Elder")) || !item_0.FullName.Contains("Shaper")) && (!MapDevice.InventoryControl.Inventory.Items.Any((Item i) => i.Stats.ContainsKey((StatTypeGGG)10055)) || !item_0.FullName.Contains("Blight")) && (!MapDevice.InventoryControl.Inventory.Items.Any((Item i) => i.Stats.ContainsKey((StatTypeGGG)10342) || (int)i.Rarity == 3) || item_0.IsSacrificeFragment()) && MapDevice.InventoryControl.Inventory.Items.All((Item i) => TakeMapTask.ScarabType(i) != TakeMapTask.ScarabType(item_0))) {
                                                await PlayerAction.TryTo(() => PlaceIntoDevice(item_0), "Place " + item_0.FullName + " into device", 3);
                                            }
                                        }
                                    }
                                    else if (await OpenQuestItemInMapDevice(incandescentInvitation)) {
                                        return true;
                                    }
                                }
                                else if (await OpenQuestItemInMapDevice(polaricInvitation)) {
                                    return true;
                                }
                            }
                            else if (await OpenQuestItemInMapDevice(screamingInvitation)) {
                                return true;
                            }
                        }
                        else if (await OpenQuestItemInMapDevice(writhingInvitation)) {
                            return true;
                        }
                    }
                    else if (await OpenQuestItemInMapDevice(mavenInvitation)) {
                        return true;
                    }
                }
                else if (await OpenQuestItemInMapDevice(simulacrum)) {
                    return true;
                }
                NetworkObject networkObject_0 = ObjectManager.MapDevice;
                Portal portal_0 = ((IEnumerable)ObjectManager.Objects.Where(delegate (NetworkObject x) {
                    //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                    //IL_0006: Unknown result type (might be due to invalid IL or missing references)
                    //IL_0010: Unknown result type (might be due to invalid IL or missing references)
                    Vector2i position5 = x.Position;
                    return ((Vector2i)(ref position5)).Distance(networkObject_0.Position) < 20;
                })).Closest<Portal>();
                if (GeneralSettings.Instance.UseMapDeviceMods && !ChoseMapDeviceMod(GeneralSettings.Instance.MapDeviceModName)) {
                    GlobalLog.Error("[OpenMapTask] Error Choosing Map Device Mod");
                }
                if ((NetworkObject)(object)portal_0 == (NetworkObject)null) {
                    GlobalLog.Warn("[OpenMapTask] Starting fresh map. No portals present");
                    await PlayerAction.TryTo(ActivateDevice, "Activate Map Device", 3);
                    if (!(await Wait.For(delegate {
                        Portal val2 = ((IEnumerable)ObjectManager.Objects.Where(delegate (NetworkObject x) {
                            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
                            //IL_0010: Unknown result type (might be due to invalid IL or missing references)
                            Vector2i position4 = x.Position;
                            return ((Vector2i)(ref position4)).Distance(networkObject_0.Position) < 20 && x.IsTargetable;
                        })).Closest<Portal>();
                        return (NetworkObject)(object)val2 != (NetworkObject)null && val2.LeadsTo((DatWorldAreaWrapper a) => a.IsMap || a.Id == "MavenHub" || a.Id.Contains("Affliction"));
                    }, "new map portals spawning", 500, 15000))) {
                        ErrorManager.ReportError();
                        return true;
                    }
                    await Wait.SleepSafe(500);
                    portal_0 = ((IEnumerable)ObjectManager.Objects.Where(delegate (NetworkObject x) {
                        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
                        //IL_0010: Unknown result type (might be due to invalid IL or missing references)
                        Vector2i position3 = x.Position;
                        return ((Vector2i)(ref position3)).Distance(networkObject_0.Position) < 20 && x.IsTargetable;
                    })).Closest<Portal>();
                    if (!(await TakeMapPortal(portal_0))) {
                        ErrorManager.ReportError();
                    }
                    return true;
                }
                bool isTargetable = ((NetworkObject)portal_0).IsTargetable;
                if (await PlayerAction.TryTo(ActivateDevice, "Activate Map Device", 3)) {
                    if (isTargetable && !(await Wait.For(() => !((NetworkObject)portal_0.Fresh<Portal>()).IsTargetable, "old map portals despawning", 200, 10000))) {
                        ErrorManager.ReportError();
                        return true;
                    }
                    if (!(await Wait.For(delegate {
                        Portal val = ((IEnumerable)ObjectManager.Objects.Where(delegate (NetworkObject x) {
                            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
                            //IL_0010: Unknown result type (might be due to invalid IL or missing references)
                            Vector2i position2 = x.Position;
                            return ((Vector2i)(ref position2)).Distance(networkObject_0.Position) < 20 && x.IsTargetable;
                        })).Closest<Portal>();
                        return (NetworkObject)(object)val != (NetworkObject)null && val.LeadsTo((DatWorldAreaWrapper a) => a.IsMap || a.Id == "MavenHub" || a.Id.Contains("Affliction"));
                    }, "new map portals spawning", 500, 15000))) {
                        ErrorManager.ReportError();
                        return true;
                    }
                    await Wait.SleepSafe(500);
                    portal_0 = ((IEnumerable)ObjectManager.Objects.Where(delegate (NetworkObject x) {
                        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
                        //IL_0010: Unknown result type (might be due to invalid IL or missing references)
                        Vector2i position = x.Position;
                        return ((Vector2i)(ref position)).Distance(networkObject_0.Position) < 20 && x.IsTargetable;
                    })).Closest<Portal>();
                    if (await TakeMapPortal(portal_0)) {
                        Utility.BroadcastMessage((object)null, "MB_map_portal_entered_event", Array.Empty<object>());
                    }
                    else {
                        ErrorManager.ReportError();
                    }
                    return true;
                }
                ErrorManager.ReportError();
                return true;
            }
            GlobalLog.Error("[OpenMapTask] The bot is in a combat area. Return false to continue.");
            return false;
        }
        return false;
    }

    public MessageResult Message(Message message) {
        //IL_001a: Unknown result type (might be due to invalid IL or missing references)
        //IL_001f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        if (message.Id == "MB_new_map_entered_event") {
            Enabled = false;
            return (MessageResult)0;
        }
        return (MessageResult)1;
    }

    private static async Task<bool> OpenDevice() {
        if (MasterDeviceUi.IsOpened) {
            return true;
        }
        int failCounter = 0;
        NetworkObject device;
        while (true) {
            device = ObjectManager.MapDevice;
            if (!(device == (NetworkObject)null)) {
                break;
            }
            if (!World.CurrentArea.IsCombatArea) {
                if (World.CurrentArea.IsMyHideoutArea || World.CurrentArea.Name == "Karui Shores") {
                    if (failCounter < 3) {
                        failCounter++;
                        GlobalLog.Error($"[OpenMapTask] Fail to find Map Device in hideout, trying to load hideout template [{failCounter}/3].");
                        await PlayerAction.LoadHideoutTemplate();
                        continue;
                    }
                    GlobalLog.Error("[OpenMapTask] Fail to find Map Device in hideout.");
                }
                else {
                    GlobalLog.Error("[OpenMapTask] Unknown error. Fail to find Map Device in Templar Laboratory.");
                }
                if (World.CurrentArea.IsMyHideoutArea || World.CurrentArea.Name == "Karui Shores" || World.CurrentArea.IsMapRoom) {
                    GlobalLog.Error("[OpenMapTask] Now stopping the bot because it cannot continue.");
                    BotManager.Stop(new StopReasonData("no_map_device", "Unable to find map device in " + World.CurrentArea.Name, (object)null), false);
                    return false;
                }
                return true;
            }
            GlobalLog.Error("[OpenMapTask] The bot probably clicked on a open portal and is now in a combat area. Return false to continue.");
            return false;
        }
        GlobalLog.Debug("[OpenMapTask] Now going to open Map Device.");
        if (device.Distance > 20f) {
            await device.WalkablePosition().ComeAtOnce();
        }
        if (!(await PlayerAction.Interact(device, () => MasterDeviceUi.IsOpened, "Map Device opening", 6000))) {
            GlobalLog.Debug("[OpenMapTask] Fail to open Map Device.");
            return false;
        }
        GlobalLog.Debug("[OpenMapTask] Map Device has been successfully opened.");
        return true;
    }

    private static async Task<bool> OpenQuestItemInMapDevice(Item item) {
        if (await PlayerAction.TryTo(OpenDevice, "Open Map Device", 3, 2000)) {
            if (AtlasWarningDialog.IsOpened) {
                AtlasWarningDialog.ConfirmDialog(true);
            }
            if (!(await CleanDevice())) {
                ErrorManager.ReportError();
                return true;
            }
            if (!(await PlayerAction.TryTo(() => PlaceIntoDevice(item), "Place " + item.FullName + " into device", 3))) {
                ErrorManager.ReportError();
                return true;
            }
            return false;
        }
        ErrorManager.ReportError();
        return true;
    }

    private static async Task<bool> CleanDevice() {
        if (!ui.sett.IsFiveSlotDevice) {
            List<Item> items = MapDevice.InventoryControl.Inventory.Items.ToList();
            if (items.Count == 0) {
                return true;
            }
            GlobalLog.Error("[OpenMapTask] Map Device is not empty. Now going to clean it.");
            foreach (Item item_0 in items) {
                GlobalLog.Debug("[OpenMapTask] Taking out " + item_0.FullName + " from device");
                if (!(await PlayerAction.TryTo(() => FastMoveFromDevice(item_0.LocationTopLeft), null, 2))) {
                    GlobalLog.Error("[OpenMapTask] Error cleaning map device!");
                    return false;
                }
            }
            GlobalLog.Debug("[OpenMapTask] Map Device has been successfully cleaned.");
            return true;
        }
        if (MasterDeviceUi.FiveSlotInventoryControl.All((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem == (RemoteMemoryObject)null)) {
            return true;
        }
        GlobalLog.Error("[OpenMapTask] Map Device is not empty. Now going to clean it.");
        foreach (InventoryControlWrapper control in MasterDeviceUi.FiveSlotInventoryControl.Where((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null)) {
            GlobalLog.Debug("[OpenMapTask] Taking out " + control.CustomTabItem.FullName + " from device");
            await Wait.LatencySleep();
            await Inventories.FastMoveCustomTabItem(control);
        }
        GlobalLog.Debug("[OpenMapTask] Map Device has been successfully cleaned.");
        return true;
    }

    private static async Task<bool> PlaceIntoDevice(Item item) {
        int int_0 = (MasterDeviceUi.IsFiveSlotDevice ? MasterDeviceUi.FiveSlotInventoryControl.Count((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null) : MapDevice.InventoryControl.Inventory.Items.Count);
        if ((int_0 != 4 || MasterDeviceUi.IsFiveSlotDevice) && int_0 != 5) {
            Vector2i itemPos = item.LocationTopLeft;
            if (item.StackCount > 1) {
                await Inventories.SplitAndPlaceItemInMainInventory(InventoryUi.InventoryControl_Main, item, 1);
                Item newItem = (from i in Inventories.InventoryItems
                                where i.FullName == item.FullName
                                orderby i.StackCount
                                select i).FirstOrDefault();
                if ((RemoteMemoryObject)(object)newItem != (RemoteMemoryObject)null) {
                    itemPos = newItem.LocationTopLeft;
                }
            }
            if (await Inventories.FastMoveFromInventory(itemPos)) {
                if (!MasterDeviceUi.IsFiveSlotDevice) {
                    if (!(await Wait.For(() => MapDevice.InventoryControl.Inventory.Items.Count == int_0 + 1, "item amount change in Map Device"))) {
                        return false;
                    }
                }
                else if (!(await Wait.For(() => MasterDeviceUi.FiveSlotInventoryControl.Count((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null) == int_0 + 1, "item amount change in Map Device"))) {
                    return false;
                }
                return true;
            }
            return false;
        }
        return true;
    }

    private static MasterMissionEnum GetMasterMission(Item map) {
        //IL_00c0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c6: Invalid comparison between Unknown and I4
        //IL_0167: Unknown result type (might be due to invalid IL or missing references)
        //IL_0188: Unknown result type (might be due to invalid IL or missing references)
        //IL_0190: Unknown result type (might be due to invalid IL or missing references)
        //IL_0198: Unknown result type (might be due to invalid IL or missing references)
        //IL_01cf: Unknown result type (might be due to invalid IL or missing references)
        //IL_0222: Unknown result type (might be due to invalid IL or missing references)
        //IL_022a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0232: Unknown result type (might be due to invalid IL or missing references)
        //IL_0290: Unknown result type (might be due to invalid IL or missing references)
        //IL_02ae: Unknown result type (might be due to invalid IL or missing references)
        //IL_02b3: Unknown result type (might be due to invalid IL or missing references)
        //IL_02b8: Unknown result type (might be due to invalid IL or missing references)
        //IL_02bd: Unknown result type (might be due to invalid IL or missing references)
        //IL_02c2: Unknown result type (might be due to invalid IL or missing references)
        //IL_02c7: Unknown result type (might be due to invalid IL or missing references)
        //IL_02c9: Unknown result type (might be due to invalid IL or missing references)
        bool flag = map.Metadata.Contains("CurrencyAfflictionFragment") || map.Name.Contains(" Invitation");
        bool flag2 = map.ImplicitStats.ContainsKey((StatTypeGGG)10342);
        int mapTier = map.MapTier;
        bool flag3 = false;
        if (!MasterDeviceUi.IsFiveSlotDevice) {
            if (MapDevice.InventoryControl.Inventory.Items.Any((Item i) => i.FullName.Contains("Bestiary") || i.FullName.Contains("Sulphite"))) {
                flag3 = true;
            }
        }
        else if (MasterDeviceUi.FiveSlotInventoryControl.Any((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null && (c.CustomTabItem.FullName.Contains("Bestiary") || c.CustomTabItem.FullName.Contains("Sulphite")))) {
            flag3 = true;
        }
        if (!(flag || flag2 || (int)map.Rarity == 3 || flag3)) {
            if (!GeneralSettings.Instance.RunKiracMissions || !World.CurrentArea.IsMyHideoutArea || (Atlas.KiracNormalMissionsLeft <= 0 && Atlas.KiracYellowMissionsLeft <= 0 && Atlas.KiracRedMissionsLeft <= 0)) {
                if (mapTier <= 5) {
                    if (GeneralSettings.Instance.RunAlvaMissions && Atlas.AlvaNormalMissionsLeft > 0) {
                        return (MasterMissionEnum)2;
                    }
                    if (GeneralSettings.Instance.RunEinhardMissions && Atlas.EinharNormalMissionsLeft > 0) {
                        return (MasterMissionEnum)1;
                    }
                    if (GeneralSettings.Instance.RunNikoMissions && Atlas.NikoNormalMissionsLeft > 0) {
                        return (MasterMissionEnum)3;
                    }
                    if (GeneralSettings.Instance.RunJunMissions && Atlas.JunNormalMissionsLeft > 0) {
                        return (MasterMissionEnum)4;
                    }
                }
                if (mapTier >= 6 && mapTier <= 10) {
                    if (GeneralSettings.Instance.RunAlvaMissions && Atlas.AlvaYellowMissionsLeft > 0) {
                        return (MasterMissionEnum)2;
                    }
                    if (GeneralSettings.Instance.RunEinhardMissions && Atlas.EinharYellowMissionsLeft > 0) {
                        return (MasterMissionEnum)1;
                    }
                    if (GeneralSettings.Instance.RunNikoMissions && Atlas.NikoYellowMissionsLeft > 0) {
                        return (MasterMissionEnum)3;
                    }
                    if (GeneralSettings.Instance.RunJunMissions && Atlas.JunYellowMissionsLeft > 0) {
                        return (MasterMissionEnum)4;
                    }
                }
                if (mapTier >= 11) {
                    if (GeneralSettings.Instance.RunAlvaMissions && Atlas.AlvaRedMissionsLeft > 0) {
                        return (MasterMissionEnum)2;
                    }
                    if (GeneralSettings.Instance.RunEinhardMissions && Atlas.EinharRedMissionsLeft > 0) {
                        return (MasterMissionEnum)1;
                    }
                    if (GeneralSettings.Instance.RunNikoMissions && Atlas.NikoRedMissionsLeft > 0) {
                        return (MasterMissionEnum)3;
                    }
                    if (GeneralSettings.Instance.RunJunMissions && Atlas.JunRedMissionsLeft > 0) {
                        return (MasterMissionEnum)4;
                    }
                }
                return (MasterMissionEnum)0;
            }
            return (MasterMissionEnum)6;
        }
        return (MasterMissionEnum)0;
    }

    private static async Task<bool> ActivateDevice() {
        GlobalLog.Debug("[OpenMapTask] Now going to activate the Map Device.");
        await Wait.SleepSafe(500);
        Item map = MapDevice.InventoryControl.Inventory.Items.Find((Item i) => i.Class == "Map" || i.Metadata.Contains("CurrencyAfflictionFragment") || i.Name.Contains("Maven's Invitation") || i.Name.Contains("Writhing Invitation") || i.Name.Contains("Screaming Invitation") || i.Name.Contains("Polaric Invitation") || i.Name.Contains("Incandescent Invitation"));
        if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
            GlobalLog.Error("[OpenMapTask] Unexpected error. There is no map in the Map Device.");
            return false;
        }
        if (MasterDeviceUi.IsAdditionalModifierPannelOpen) {
            GlobalLog.Debug("[OpenMapTask] Closing additional modifier panel");
            if (!MasterDeviceUi.ToggleAdditionalModifierPannel()) {
                return false;
            }
        }
        if (World.CurrentArea.IsMyHideoutArea || !(World.CurrentArea.Name != "Karui Shores")) {
            if (map.Name.Contains("Maven's Invitation") || map.Name.Contains("Writhing Invitation") || map.Name.Contains("Screaming Invitation") || map.Name.Contains("Polaric Invitation") || map.Name.Contains("Incandescent Invitation") || map.Name.Contains("Simulacrum")) {
                GlobalLog.Debug("[OpenMapTask] " + map.Name + " is in the device. No additional actions needed. Activating device.");
                return await PressButtonAndWait();
            }
            MasterMissionEnum masterMissionEnum_0 = GetMasterMission(map);
            if ((int)masterMissionEnum_0 > 0) {
                if ((int)masterMissionEnum_0 == 6) {
                    while (true) {
                        if (!KiracMissionsUi.IsOpened) {
                            OpenKiracMissionsResult err = MasterDeviceUi.OpenKiracMissions(true);
                            if ((int)err == 0) {
                                await Coroutines.LatencyWait();
                                await Wait.For(() => KiracMissionsUi.IsOpened, "Kirac mission UI to open", 10, 1000);
                                GlobalLog.Warn("[OpenMapTask] Kirac mission UI opened.");
                            }
                            else {
                                GlobalLog.Error($"[OpenMapTask] Kirac mission UI failed to open {err}.");
                            }
                            continue;
                        }
                        List<Item> noBanned = KiracMissionsUi.AvailableMaps.Where((Item i) => i.GetBannedAffix() == null).ToList();
                        Item item_0 = (from m in noBanned
                                       orderby !m.Ignored() descending, m.Priority() descending
                                       select m).FirstOrDefault();
                        if ((RemoteMemoryObject)(object)item_0 != (RemoteMemoryObject)null) {
                            GlobalLog.Warn($"[OpenMapTask] Map of choice: [{item_0.FullName} {item_0.Name} prio: {item_0.Priority()}] tier: {item_0.MapTier} rarity {item_0.Rarity}");
                            Item currentSelected = KiracMissionsUi.SelectedMap;
                            if ((RemoteMemoryObject)(object)currentSelected != (RemoteMemoryObject)null) {
                                GlobalLog.Debug("[OpenMapTask] Selected map: " + currentSelected.FullName);
                            }
                            if ((RemoteMemoryObject)(object)currentSelected == (RemoteMemoryObject)null || (RemoteMemoryObject)(object)currentSelected != (RemoteMemoryObject)(object)item_0 || string.IsNullOrEmpty(KiracMissionsUi.SelectedMapDescription)) {
                                if ((RemoteMemoryObject)(object)currentSelected != (RemoteMemoryObject)null) {
                                    GlobalLog.Warn("[OpenMapTask] Need to switch " + currentSelected.FullName + " to " + item_0.FullName);
                                }
                                SelectKiracMissionMapResult err3 = KiracMissionsUi.SelectKiracMissionMap(item_0);
                                if ((int)err3 == 5) {
                                    await Coroutines.LatencyWait();
                                    await Wait.For(() => (RemoteMemoryObject)(object)KiracMissionsUi.SelectedMap == (RemoteMemoryObject)(object)item_0, "Select map", 10, 1000);
                                    GlobalLog.Debug("[OpenMapTask] Successfully switched map!");
                                }
                                else {
                                    GlobalLog.Error($"[OpenMapTask] Error switching map! {err3}");
                                }
                                continue;
                            }
                            ActivateKiracMissionResult err2 = KiracMissionsUi.ActivateKiracMission();
                            if ((int)err2 == 2) {
                                await Coroutines.LatencyWait();
                                if (await Wait.For(() => !KiracMissionsUi.IsOpened, "Kirac mission UI closing")) {
                                    break;
                                }
                                GlobalLog.Error("[OpenMapTask] Fail to activate Kirac mission map (device ui not closed).");
                            }
                            GlobalLog.Error($"[OpenMapTask] Fail to activate Kirac mission map {err2}.");
                            continue;
                        }
                        GlobalLog.Error("[OpenMapTask] Something is wrong. Map of choice is null. Kirac missions disabled until bot restart.");
                        GeneralSettings.Instance.RunKiracMissions = false;
                        return false;
                    }
                    GlobalLog.Debug("[OpenMapTask] Successfully activated Kirac mission map!");
                    Utility.BroadcastMessage((object)null, "MB_kirac_mission_opened_event", Array.Empty<object>());
                    return true;
                }
                if (MasterDeviceUi.SelectedMasterMission != masterMissionEnum_0) {
                    GlobalLog.Debug($"[OpenMapTask] Need to switch {MasterDeviceUi.SelectedMasterMission} to {masterMissionEnum_0}");
                    SelectMasterMissionResult selectMasterMissionResult = MasterDeviceUi.SelectMasterMission(masterMissionEnum_0);
                    if (!(await Wait.For(() => MasterDeviceUi.SelectedMasterMission == masterMissionEnum_0, "Select Master Mission"))) {
                        GlobalLog.Error($"[OpenMapTask] SelectMasterMission error: {selectMasterMissionResult}");
                        ErrorManager.ReportError();
                        return false;
                    }
                }
            }
            return await PressButtonAndWait();
        }
        return await PressButtonAndWait(isMaster: false);
    }

    private static async Task<bool> FastMoveFromDevice(Vector2i itemPos) {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        Item item = MasterDeviceUi.InventoryControl.Inventory.FindItemByPos(itemPos);
        if (!((RemoteMemoryObject)(object)item == (RemoteMemoryObject)null)) {
            string itemName = item.FullName;
            GlobalLog.Debug($"[FastMoveFromDevice] Fast moving \"{itemName}\" at {itemPos} from Map Device.");
            FastMoveResult moved = MasterDeviceUi.InventoryControl.FastMove(item.LocalId, true, true);
            if ((int)moved > 0) {
                GlobalLog.Error($"[FastMoveFromDevice] Fast move error: \"{moved}\".");
                return false;
            }
            if (await Wait.For(() => (RemoteMemoryObject)(object)MasterDeviceUi.InventoryControl.Inventory.FindItemByPos(itemPos) == (RemoteMemoryObject)null, "fast move")) {
                GlobalLog.Debug($"[FastMoveFromDevice] \"{itemName}\" at {itemPos} has been successfully fast moved from Map Device.");
                return true;
            }
            GlobalLog.Error($"[FastMoveFromDevice] Fast move timeout for \"{itemName}\" at {itemPos} in Map Device.");
            return false;
        }
        GlobalLog.Error($"[FastMoveFromDevice] Fail to find item at {itemPos} in Map Device.");
        return false;
    }

    public static async Task<bool> TakeMapPortal(Portal portal, int attempts = 3) {
        int i = 1;
        while (i <= attempts) {
            if (!LokiPoe.IsInGame || World.CurrentArea.IsMap) {
                return true;
            }
            await Coroutines.CloseBlockingWindows();
            GlobalLog.Debug($"[OpenMapTask] Take portal to map attempt: {i}/{attempts}");
            if (await PlayerAction.TakePortal(portal)) {
                return true;
            }
            await Wait.SleepSafe(1000);
            int num = i + 1;
            i = num;
        }
        return false;
    }

    private static bool ChoseMapDeviceMod(string zanaModName) {
        //IL_0148: Unknown result type (might be due to invalid IL or missing references)
        //IL_014d: Unknown result type (might be due to invalid IL or missing references)
        //IL_014f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0152: Invalid comparison between Unknown and I4
        //IL_015c: Unknown result type (might be due to invalid IL or missing references)
        //IL_016d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0174: Unknown result type (might be due to invalid IL or missing references)
        //IL_0177: Invalid comparison between Unknown and I4
        if (!MasterDeviceUi.ZanaMods.Any((ZanaMod m) => m.Name.EqualsIgnorecase(zanaModName)) && !zanaModName.EqualsIgnorecase("free")) {
            GlobalLog.Error("[OpenMapTask] Selected Zana mod is not yet unlocked. Skip");
            return true;
        }
        ZanaMod val = MasterDeviceUi.ZanaMods.OrderBy((ZanaMod _) => Guid.NewGuid()).FirstOrDefault((ZanaMod m) => m.ChaosCost == 0);
        if (zanaModName.Equals("free") && val == null) {
            GlobalLog.Debug("[OpenMapTask] No free mods found.");
            return true;
        }
        ZanaMod val2 = ((!zanaModName.Equals("free") || val == null) ? MasterDeviceUi.ZanaMods.First((ZanaMod m) => m.Name.EqualsIgnorecase(zanaModName)) : val);
        GlobalLog.Warn((val2.ChaosCost != 0) ? ("[OpenMapTask] Now going to chose mod " + val2.Name) : ("[OpenMapTask] Now going to chose free mod " + val2.Name));
        SelectZanaModResult val3 = MasterDeviceUi.SelectZanaMod(val2);
        if ((int)val3 > 0) {
            GlobalLog.Error($"[OpenMapTask] Mod chosing error: {val3}");
        }
        return (int)val3 == 0 || (int)val3 == 2;
    }

    public static bool EnableEye(string eye) {
        //IL_003b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0078: Unknown result type (might be due to invalid IL or missing references)
        //IL_007d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0088: Unknown result type (might be due to invalid IL or missing references)
        //IL_008d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0098: Unknown result type (might be due to invalid IL or missing references)
        //IL_009d: Unknown result type (might be due to invalid IL or missing references)
        //IL_009f: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a2: Invalid comparison between Unknown and I4
        //IL_00f5: Unknown result type (might be due to invalid IL or missing references)
        //IL_00fa: Unknown result type (might be due to invalid IL or missing references)
        //IL_00fc: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ff: Invalid comparison between Unknown and I4
        //IL_010d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0112: Unknown result type (might be due to invalid IL or missing references)
        //IL_0114: Unknown result type (might be due to invalid IL or missing references)
        //IL_0117: Invalid comparison between Unknown and I4
        //IL_0193: Unknown result type (might be due to invalid IL or missing references)
        //IL_0198: Unknown result type (might be due to invalid IL or missing references)
        //IL_019a: Unknown result type (might be due to invalid IL or missing references)
        //IL_019d: Invalid comparison between Unknown and I4
        //IL_01c7: Unknown result type (might be due to invalid IL or missing references)
        //IL_01cc: Unknown result type (might be due to invalid IL or missing references)
        //IL_01ce: Unknown result type (might be due to invalid IL or missing references)
        //IL_01d1: Invalid comparison between Unknown and I4
        //IL_0219: Unknown result type (might be due to invalid IL or missing references)
        //IL_021e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0220: Unknown result type (might be due to invalid IL or missing references)
        //IL_0223: Invalid comparison between Unknown and I4
        //IL_0247: Unknown result type (might be due to invalid IL or missing references)
        //IL_024c: Unknown result type (might be due to invalid IL or missing references)
        //IL_024e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0251: Invalid comparison between Unknown and I4
        //IL_027f: Unknown result type (might be due to invalid IL or missing references)
        if (!MasterDeviceUi.IsTheMavenInvitationVisible && !MasterDeviceUi.IsTheSearingExarchVisible && !MasterDeviceUi.IsTheEaterOfWorldsVisible) {
            return true;
        }
        GlobalLog.Warn("[OpenMapTask] Trying to enable " + eye + "'s Eye.");
        EnableTheMavenInvitationResult val = (EnableTheMavenInvitationResult)0;
        switch (eye) {
            case "Eater":
                if (MasterDeviceUi.IsTheEaterOfWorldsVisible) {
                    val = MasterDeviceUi.EnableTheEaterOfWorlds();
                }
                break;
            case "Exarch":
                if (MasterDeviceUi.IsTheSearingExarchVisible) {
                    val = MasterDeviceUi.EnableTheSearingExarch();
                }
                break;
            case "Maven":
                if (MasterDeviceUi.IsTheMavenInvitationVisible) {
                    val = MasterDeviceUi.EnableTheMavenInvitation();
                }
                break;
        }
        if ((int)val <= 0) {
            switch (eye) {
                case "Exarch": {
                        if (MasterDeviceUi.IsTheSearingExarchClicked) {
                            break;
                        }
                        GlobalLog.Warn("[OpenMapTask] " + eye + "'s Eye not clickable. Using another one");
                        EnableTheMavenInvitationResult val6 = MasterDeviceUi.EnableTheEaterOfWorlds();
                        if ((int)val6 != 0) {
                            break;
                        }
                        if (!MasterDeviceUi.IsTheEaterOfWorldsClicked) {
                            EnableTheMavenInvitationResult val7 = MasterDeviceUi.EnableTheMavenInvitation();
                            if ((int)val7 == 0 && MasterDeviceUi.IsTheMavenInvitationClicked) {
                                GlobalLog.Warn("[OpenMapTask] Used Maven's Eye instead of " + eye + "'s");
                            }
                        }
                        else {
                            GlobalLog.Warn("[OpenMapTask] Used Eater's Eye instead of " + eye + "'s");
                        }
                        break;
                    }
                case "Eater": {
                        if (MasterDeviceUi.IsTheEaterOfWorldsClicked) {
                            break;
                        }
                        GlobalLog.Warn("[OpenMapTask] " + eye + "'s Eye not clickable. Using another one");
                        EnableTheMavenInvitationResult val4 = MasterDeviceUi.EnableTheSearingExarch();
                        if ((int)val4 != 0) {
                            break;
                        }
                        if (MasterDeviceUi.IsTheSearingExarchClicked) {
                            GlobalLog.Warn("[OpenMapTask] Used Exarch's Eye instead of " + eye + "'s");
                            break;
                        }
                        EnableTheMavenInvitationResult val5 = MasterDeviceUi.EnableTheMavenInvitation();
                        if ((int)val5 == 0 && MasterDeviceUi.IsTheMavenInvitationClicked) {
                            GlobalLog.Warn("[OpenMapTask] Used Maven's Eye instead of " + eye + "'s");
                        }
                        break;
                    }
                case "Maven": {
                        if (MasterDeviceUi.IsTheMavenInvitationClicked) {
                            break;
                        }
                        GlobalLog.Warn("[OpenMapTask] " + eye + "'s Eye not clickable. Using another one");
                        EnableTheMavenInvitationResult val2 = MasterDeviceUi.EnableTheSearingExarch();
                        if ((int)val2 != 0) {
                            break;
                        }
                        if (MasterDeviceUi.IsTheSearingExarchClicked) {
                            GlobalLog.Warn("[OpenMapTask] Used Exarch's Eye instead of " + eye + "'s");
                            break;
                        }
                        EnableTheMavenInvitationResult val3 = MasterDeviceUi.EnableTheEaterOfWorlds();
                        if ((int)val3 == 0 && MasterDeviceUi.IsTheEaterOfWorldsClicked) {
                            GlobalLog.Warn("[OpenMapTask] Used Eater's Eye instead of " + eye + "'s");
                        }
                        break;
                    }
            }
            return true;
        }
        GlobalLog.Error($"[OpenMapTask] Error enabling {eye}'s Eye. Error: {val}");
        return false;
    }

    private static async Task<bool> PressButtonAndWait(bool isMaster = true) {
        ActivateResult res = (isMaster ? MasterDeviceUi.Activate(true) : MapDeviceUi.Activate(true));
        await Coroutines.LatencyWait();
        if ((int)res > 0) {
            GlobalLog.Error($"[OpenMapTask] Fail to activate the Map Device {res}");
            return false;
        }
        if (!(await Wait.For(() => !MapDeviceUi.IsOpened && !MasterDeviceUi.IsOpened, "Map Device closing"))) {
            GlobalLog.Error("[OpenMapTask] Fail to activate the Map Device.");
            return false;
        }
        GlobalLog.Debug("[OpenMapTask] Map Device has been successfully activated.");
        return true;
    }

    private static string ChooseExplorationInfluence(string prio) {
        if (GeneralSettings.Instance.AtlasExplorationEnabled) {
            DatQuestStateWrapper currentQuestStateAccurate = InGameState.GetCurrentQuestStateAccurate("tangle");
            bool flag = currentQuestStateAccurate != null && currentQuestStateAccurate.Id == 0;
            DatQuestStateWrapper currentQuestStateAccurate2 = InGameState.GetCurrentQuestStateAccurate("cleansing_fire");
            bool flag2 = currentQuestStateAccurate2 != null && currentQuestStateAccurate2.Id == 0;
            DatQuestStateWrapper currentQuestStateAccurate3 = InGameState.GetCurrentQuestStateAccurate("maven_atlas");
            int? num = ((currentQuestStateAccurate3 == null) ? null : new int?(currentQuestStateAccurate3.Id));
            bool flag3 = num >= 5 || num == 1;
            if (!(flag2 && flag)) {
                if (flag3) {
                    if (!flag) {
                        if (!flag2) {
                            return prio;
                        }
                        return "Eater";
                    }
                    return "Exarch";
                }
                return "Maven";
            }
            return prio;
        }
        return prio;
    }

    public async Task<LogicResult> Logic(Logic logic) {
        return (LogicResult)1;
    }

    public void Tick() {
    }

    public void Start() {
    }

    public void Stop() {
    }
}
