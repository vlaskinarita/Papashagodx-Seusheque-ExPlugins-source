using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.Game.Utilities;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.CommonTasks;
using ExPlugins.MapBotEx.Helpers;
using static DreamPoeBot.Loki.Game.LokiPoe.InGameState;
using static DreamPoeBot.Loki.Game.LokiPoe.InGameState.AtlasUi;
using static DreamPoeBot.Loki.Game.LokiPoe.InGameState.StashUi;

namespace ExPlugins.MapBotEx.Tasks;

public class TakeMapTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents {
    private static readonly GeneralSettings Settings;

    private static readonly ExtensionsSettings extensionsSettings_0;

    public static readonly Dictionary<string, bool> AvailableCurrency;

    private static readonly Dictionary<string, int> dictionary_0;

    private static bool bool_0;

    private static bool bool_1;

    private static readonly Interval interval_0;

    public static bool TempIgnoreRavaged;

    public static bool TempIgnoreBlighted;

    private static bool HasMagicOrbs => HasCurrency(CurrencyNames.Alteration) && HasCurrency(CurrencyNames.Augmentation) && HasCurrency(CurrencyNames.Transmutation);

    private static bool HasRareOrbs {
        get {
            if (Settings.RerollMethod == RareReroll.ScourAlch) {
                return HasScourAlchemy;
            }
            return HasCurrency(CurrencyNames.Alchemy) && HasCurrency(CurrencyNames.Chaos);
        }
    }

    private static bool HasMagicToRareOrbs {
        get {
            if (Settings.RerollMethod == RareReroll.ScourAlch) {
                return HasScourAlchemy;
            }
            return HasScourAlchemy && HasCurrency(CurrencyNames.Chaos);
        }
    }

    private static bool HasScourAlchemy => HasCurrency(CurrencyNames.Scouring) && HasCurrency(CurrencyNames.Alchemy);

    private static bool HasScourTransmute => HasCurrency(CurrencyNames.Scouring) && HasCurrency(CurrencyNames.Transmutation);

    public string Name => "TakeMapTask";

    public string Author => "ExVault";

    public string Description => "Task for taking maps from the stash.";

    public string Version => "1.0";

    public async Task<bool> Run() {
        if (!Settings.IsOnRun) {
            DatWorldAreaWrapper area = World.CurrentArea;
            if (SkillsUi.IsOpened || AtlasSkillsUi.IsOpened) {
                return false;
            }
            if (!area.IsTown && !area.IsMyHideoutArea) {
                return false;
            }
            if (Settings.StopRequested) {
                GlobalLog.Warn("Stopping the bot by a user's request (stop after current map)");
                Settings.StopRequested = false;
                BotManager.Stop(new StopReasonData("map_finished_stop", "Stopping the bot by a user's request (stop after current map)", (object)null), false);
                return true;
            }
            if (Settings.UseChargedCompass) {
                await ApplyCompass();
            }
            await AtlasHelper.RollSextantsOnVoidstones();
            Item mavenInvitation = Inventories.InventoryItems.Find((Item i) => i.Name.Contains("Maven's Invitation") && i.Class == "QuestItem");
            Item miniBossInvitation = Inventories.InventoryItems.Find((Item i) => (i.Name.Contains("Writhing Invitation") || i.Name.Contains("Polaric Invitation")) && i.Class == "QuestItem");
            Item bossInvitation = Inventories.InventoryItems.Find((Item i) => (i.Name.Contains("Screaming Invitation") || i.Name.Contains("Incandescent Invitation")) && i.Class == "QuestItem");
            if ((!((RemoteMemoryObject)(object)miniBossInvitation != (RemoteMemoryObject)null) || !Settings.OpenMiniBossQuestInvitations) && (!((RemoteMemoryObject)(object)bossInvitation != (RemoteMemoryObject)null) || !Settings.OpenBossQuestInvitations) && !((RemoteMemoryObject)(object)mavenInvitation != (RemoteMemoryObject)null)) {
                Item invMap = Inventories.InventoryItems.FirstOrDefault((Item i) => i.IsMap());
                CachedMapItem map = null;
                if ((RemoteMemoryObject)(object)invMap == (RemoteMemoryObject)null) {
                    map = await FindProperMap();
                    if (map == null) {
                        if (TempIgnoreBlighted || TempIgnoreRavaged) {
                            GlobalLog.Error($"[TakeMapTask] Seems like bot have only blighted maps in all map tabs but don't have oils to anoint them. Make sure you specified correct stash tab for oils. Stopping bot for now. [anoint option enabled? {Settings.AnointMaps}]");
                            Utility.BroadcastMessage((object)this, "mapbot_outofmaps", new object[1] { "[TakeMapTask] Seems like bot have only blighted maps in all map tabs but don't have oils to anoint them. Make sure you specified correct stash tab for oils." });
                        }
                        else {
                            GlobalLog.Error("[TakeMapTask] Fail to find a proper map in all map tabs. Now stopping the bot because it cannot continue.");
                            Utility.BroadcastMessage((object)this, "mapbot_outofmaps", new object[1] { "[TakeMapTask] Fail to find a proper map in all map tabs. Now stopping the bot because it cannot continue." });
                        }
                        if (extensionsSettings_0.SwitchBotOnNoMaps) {
                            BackgroundWorker bg = new BackgroundWorker();
                            bg.DoWork += delegate {
                                Stopwatch stopwatch = Stopwatch.StartNew();
                                while (BotManager.IsRunning && stopwatch.ElapsedMilliseconds <= 30000L) {
                                }
                                if (!BotManager.IsRunning) {
                                    IBot val2 = BotManager.Bots.FirstOrDefault((IBot b) => ((IAuthored)b).Name == "QuestBotEx");
                                    if (val2 == null) {
                                        GlobalLog.Error("cant switch bot, just stopping");
                                    }
                                    else {
                                        GlobalLog.Debug("switching bot");
                                        BotManager.Current = val2;
                                        BotManager.Start();
                                        Utility.BroadcastMessage((object)this, "botswitch_event", new object[1] { "MapBotEx to QuestBotEx" });
                                    }
                                }
                            };
                            bg.RunWorkerAsync();
                        }
                        CachedMapItem cachedMapItem;
                        map = (cachedMapItem = await FindProperMap(force: true));
                        if (cachedMapItem == null) {
                            BotManager.Stop(new StopReasonData("mapbot_outofmaps", "cant switch bot, just stopping", (object)null), false);
                            return true;
                        }
                    }
                }
                if ((RemoteMemoryObject)(object)invMap == (RemoteMemoryObject)null) {
                    Item val;
                    invMap = (val = await Inventories.TakeMapFromStash(map));
                    if (!((RemoteMemoryObject)(object)val != (RemoteMemoryObject)null)) {
                        return true;
                    }
                    CachedMaps.Instance.MapCache.Maps.Remove(map);
                }
                Vector2i mapPos = invMap.LocationTopLeft;
                Rarity mapRarity = invMap.Rarity;
                StashTask.ProtectedSlot = mapPos;
                if (invMap.Metadata.Contains("MapFragments/CurrencyAfflictionFragment")) {
                    ChooseMap(mapPos);
                    return false;
                }
                if ((int)mapRarity != 3 && invMap.IsIdentified && !invMap.IsMirrored && !invMap.IsCorrupted) {
                    if (!invMap.IsCorrupted && invMap.Stats.ContainsKey((StatTypeGGG)10342) && CanAnoint(invMap)) {
                        await GetOils(invMap);
                        if (!(await TalkWithCassia())) {
                            return true;
                        }
                        UpdateMapReference(mapPos, ref invMap);
                    }
                    if (!invMap.Stats.ContainsKey((StatTypeGGG)14763) || Settings.AlchRavagedMaps || (int)invMap.Rarity != 0) {
                        if (!invMap.Stats.ContainsKey((StatTypeGGG)10342) || Settings.AlchBlightedMaps || (int)invMap.Rarity != 0) {
                            if ((int)mapRarity != 0) {
                                if ((int)mapRarity != 1) {
                                    if ((int)mapRarity != 2) {
                                        GlobalLog.Error($"[TakeMapTask] Unknown map rarity: \"{mapRarity}\".");
                                        ErrorManager.ReportCriticalError();
                                        return true;
                                    }
                                    if (!(await HandleRareMap(invMap))) {
                                        StashTask.ProtectedSlot = new Vector2i(100, 100);
                                        return true;
                                    }
                                    UpdateMapReference(mapPos, ref invMap);
                                }
                                else {
                                    if (!(await HandleMagicMap(invMap))) {
                                        StashTask.ProtectedSlot = new Vector2i(100, 100);
                                        return true;
                                    }
                                    UpdateMapReference(mapPos, ref invMap);
                                }
                            }
                            else {
                                if (!(await HandleNormalMap(invMap))) {
                                    StashTask.ProtectedSlot = new Vector2i(100, 100);
                                    return true;
                                }
                                UpdateMapReference(mapPos, ref invMap);
                            }
                        }
                        else {
                            if (!(await HandleBlightedMap(invMap))) {
                                return true;
                            }
                            UpdateMapReference(mapPos, ref invMap);
                        }
                    }
                    else {
                        if (!(await HandleBlightedMap(invMap))) {
                            return true;
                        }
                        UpdateMapReference(mapPos, ref invMap);
                    }
                    UpdateMapReference(mapPos, ref invMap);
                    if (invMap.ShouldUpgrade(Settings.VaalUpgrade) && HasCurrency(CurrencyNames.Vaal)) {
                        if (!(await CorruptMap(mapPos))) {
                            StashTask.ProtectedSlot = new Vector2i(100, 100);
                            return true;
                        }
                        UpdateMapReference(mapPos, ref invMap);
                    }
                    while (invMap.ShouldUpgrade(Settings.FragmentUpgrade) && bool_0 && !(await GetFragment(invMap))) {
                    }
                    ChooseMap(mapPos);
                    return false;
                }
                while (invMap.ShouldUpgrade(Settings.FragmentUpgrade) && bool_0 && !(await GetFragment(invMap))) {
                }
                ChooseMap(mapPos);
                return false;
            }
            OpenMapTask.Enabled = true;
            return false;
        }
        return false;
    }

    public MessageResult Message(Message message) {
        //IL_0014: Unknown result type (might be due to invalid IL or missing references)
        //IL_0070: Unknown result type (might be due to invalid IL or missing references)
        //IL_0072: Unknown result type (might be due to invalid IL or missing references)
        if (!(message.Id == "item_stashed_event")) {
            return (MessageResult)1;
        }
        CachedItem input = message.GetInput<CachedItem>(0);
        if (input != null) {
            string @class = input.Class;
            if (@class == "StackableCurrency") {
                UpdateAvailableCurrency(input.Name);
            }
            else if (@class == "MapFragment" && !bool_0) {
                UpdateAvailableFragments();
            }
        }
        return (MessageResult)0;
    }

    public void Start() {
        foreach (string item in AvailableCurrency.Keys.ToList()) {
            AvailableCurrency[item] = true;
        }
        bool_0 = true;
    }

    private static async Task ApplyCompass() {
        if (!bool_1) {
            return;
        }
        List<string> tabs = extensionsSettings_0.GetTabsForCategory("Charged Compass");
        List<Item> allCompasses = new List<Item>();
        List<Voidstone> allVoidstones = new List<Voidstone>();
        HashSet<ModAffix> voidAffixes = new HashSet<ModAffix>();
        await Coroutines.CloseBlockingWindows();
        await AtlasHelper.OpenAtlasUi();
        if ((RemoteMemoryObject)(object)AtlasUi.GraspingVoidstone.Item != (RemoteMemoryObject)null && !AtlasUi.GraspingVoidstone.Item.Affixes.Any()) {
            allVoidstones.Add(AtlasUi.GraspingVoidstone);
            voidAffixes.Add(AtlasUi.GraspingVoidstone.Item.Affixes.FirstOrDefault());
        }
        if ((RemoteMemoryObject)(object)AtlasUi.DecayedVoidstone.Item != (RemoteMemoryObject)null && !AtlasUi.DecayedVoidstone.Item.Affixes.Any()) {
            allVoidstones.Add(AtlasUi.DecayedVoidstone);
            voidAffixes.Add(AtlasUi.DecayedVoidstone.Item.Affixes.FirstOrDefault());
        }
        if ((RemoteMemoryObject)(object)AtlasUi.OmniscientVoidstone.Item != (RemoteMemoryObject)null && !AtlasUi.OmniscientVoidstone.Item.Affixes.Any()) {
            allVoidstones.Add(AtlasUi.OmniscientVoidstone);
            voidAffixes.Add(AtlasUi.OmniscientVoidstone.Item.Affixes.FirstOrDefault());
        }
        if ((RemoteMemoryObject)(object)AtlasUi.CerimonialVoidstone.Item != (RemoteMemoryObject)null && !AtlasUi.CerimonialVoidstone.Item.Affixes.Any()) {
            allVoidstones.Add(AtlasUi.CerimonialVoidstone);
            voidAffixes.Add(AtlasUi.CerimonialVoidstone.Item.Affixes.FirstOrDefault());
        }
        await AtlasHelper.CloseAtlasUi();
        if (allVoidstones.Any()) {
            foreach (string tab in tabs) {
                await Inventories.OpenStashTab(tab, "ApplyCompass");
                List<Item> tabCompasses = (from i in StashUi.InventoryControl.Inventory.Items
                                           where i.Metadata.Contains("CurrencyItemisedSextantModifier")
                                           select i into e
                                           group e by e.Affixes.First().Category into g
                                           select g.First()).ToList();
                if (!tabCompasses.Any()) {
                    GlobalLog.Debug("[ApplyCompass] No compasses found in tab " + tab + "!");
                    continue;
                }
                allCompasses.AddRange(tabCompasses);
                List<Item> toUse = new List<Item>();
                foreach (Item compass3 in tabCompasses) {
                    if (toUse.Count < allVoidstones.Count) {
                        string string_0 = compass3.Affixes.First().Category;
                        if (voidAffixes.Any((ModAffix a) => a != null && a.Category == string_0)) {
                            GlobalLog.Warn("[ApplyCompass] Affix: " + string_0 + " is already present. Skip.");
                        }
                        else {
                            toUse.Add(compass3);
                        }
                        continue;
                    }
                    break;
                }
                if (!toUse.Any()) {
                    continue;
                }
                foreach (Item compass2 in toUse) {
                    await Inventories.FastMoveFromStashTab(compass2.LocationTopLeft);
                }
            }
            if (!allCompasses.Any()) {
                GlobalLog.Error("[ApplyCompass] No compasses found in all specified stash tabs! Skip.");
                bool_1 = false;
                return;
            }
            await Coroutines.CloseBlockingWindows();
            await AtlasHelper.OpenAtlasUi();
            await Inventories.OpenInventory();
            List<Item> invCompasses = InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Metadata.Contains("CurrencyItemisedSextantModifier")).ToList();
            foreach (Item compass in invCompasses) {
                Voidstone voidstone = allVoidstones.FirstOrDefault((Voidstone i) => !i.Item.Affixes.Any());
                if (voidstone != null && (RemoteMemoryObject)(object)voidstone.Item != (RemoteMemoryObject)null) {
                    string affix = compass.Affixes.First().Category;
                    await InventoryUi.InventoryControl_Main.PickItemToCursor(compass.LocationTopLeft, rightClick: true);
                    await Wait.For(() => (RemoteMemoryObject)(object)CursorItemOverlay.Item != (RemoteMemoryObject)null, "applying " + affix + " to " + voidstone.Item.Name, 5, 1000);
                    voidstone.LeftClick();
                    await Inventories.WaitForCursorToBeEmpty();
                    GlobalLog.Debug("[ApplyCompass] Successfully applied " + affix + " to " + voidstone.Item.Name);
                    allVoidstones.Remove(voidstone);
                    continue;
                }
                GlobalLog.Warn("[ApplyCompass] Can't find voidstone!.");
                break;
            }
        }
        else {
            GlobalLog.Error("[ApplyCompass] No usable voidstones found! Skip.");
        }
    }

    private static async Task<bool> TalkWithCassia() {
        if (InventoryUi.InventoryControl_Main.Inventory.Items.Any((Item i) => i.Name.Contains("Oil"))) {
            Item map = Inventories.InventoryItems.FirstOrDefault((Item i) => i.Class == "Map" && !i.IsCorrupted && i.Stats.ContainsKey((StatTypeGGG)10342));
            NetworkObject cassia = ObjectManager.GetObjectByName("Sister Cassia");
            if (!(cassia == (NetworkObject)null) && !((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null)) {
                TownNpc cassiaTownNpc = cassia.AsTownNpc();
                if (await cassiaTownNpc.OpenDefaultCtrlAnoint()) {
                    if (!CleanAnointUi()) {
                        return false;
                    }
                    if (!(await Inventories.FastMoveFromInventory(map.LocationTopLeft))) {
                        return false;
                    }
                    while (true) {
                        if (!AnointingUi.IsOpened) {
                            await cassiaTownNpc.OpenDefaultCtrlAnoint();
                            continue;
                        }
                        List<Item> oils = (from i in InventoryUi.InventoryControl_Main.Inventory.Items
                                           where Settings.AnointOils.Any((OilEntry o) => i.FullName.Equals(o.Name) && (o.UseOnBlighted || o.UseOnRavaged))
                                           orderby i.StackCount descending, i.FullName
                                           select i).ToList();
                        if (!oils.Any()) {
                            break;
                        }
                        Item oil = oils.First();
                        while (true) {
                            if (((RemoteMemoryObject)(object)AnointingUi.Oil1 == (RemoteMemoryObject)null || (RemoteMemoryObject)(object)AnointingUi.Oil2 == (RemoteMemoryObject)null || (RemoteMemoryObject)(object)AnointingUi.Oil3 == (RemoteMemoryObject)null) && string.IsNullOrEmpty(AnointingUi.ModError)) {
                                Item item_0 = InventoryUi.InventoryControl_Main.Inventory.FindItemByFullName(oil.FullName);
                                if ((RemoteMemoryObject)(object)item_0 == (RemoteMemoryObject)null || !(await Inventories.FastMoveFromInventory(item_0.LocationTopLeft, waitForStackDecrease: true))) {
                                    break;
                                }
                                await Wait.SleepSafe(100);
                                if (string.IsNullOrEmpty(AnointingUi.ModError)) {
                                    continue;
                                }
                                string error = AnointingUi.ModError;
                                GlobalLog.Error("[TakeMapTask] " + error);
                                if (error.Contains("more than 3 of the same Blight modifier")) {
                                    oils.RemoveAll((Item i) => i.FullName.Equals(item_0.FullName));
                                    continue;
                                }
                            }
                            while (!string.IsNullOrEmpty(AnointingUi.ModError)) {
                                if ((RemoteMemoryObject)(object)AnointingUi.Oil3 != (RemoteMemoryObject)null) {
                                    AnointingUi.FastRemoveOil(3);
                                    if (string.IsNullOrEmpty(AnointingUi.ModError)) {
                                        break;
                                    }
                                }
                                if ((RemoteMemoryObject)(object)AnointingUi.Oil2 != (RemoteMemoryObject)null) {
                                    AnointingUi.FastRemoveOil(2);
                                    if (string.IsNullOrEmpty(AnointingUi.ModError)) {
                                        break;
                                    }
                                }
                                if ((RemoteMemoryObject)(object)AnointingUi.Oil1 != (RemoteMemoryObject)null) {
                                    AnointingUi.FastRemoveOil(1);
                                    if (string.IsNullOrEmpty(AnointingUi.ModError)) {
                                        break;
                                    }
                                }
                            }
                            AnointResult res = AnointingUi.Anoint();
                            if ((int)res != 0) {
                                GlobalLog.Error($"Failed to anoint because {res}");
                                return false;
                            }
                            await Wait.For(() => GlobalWarningDialog.IsOpened || (RemoteMemoryObject)(object)AnointingUi.Oil1 == (RemoteMemoryObject)null, "Anoint", 10, 1000);
                            if (GlobalWarningDialog.IsOpened) {
                                GlobalWarningDialog.ConfirmDialog(true);
                            }
                            Item cassiaMap = AnointingUi.InventoryControl_Main.Inventory.Items.FirstOrDefault();
                            if ((RemoteMemoryObject)(object)cassiaMap != (RemoteMemoryObject)null) {
                                cassiaMap.Stats.TryGetValue((StatTypeGGG)1023, out var packSize);
                                int anointsAmt = packSize / 5;
                                if ((cassiaMap.Stats.ContainsKey((StatTypeGGG)14763) && anointsAmt < 9) || anointsAmt < 3) {
                                    break;
                                }
                            }
                            return CleanAnointUi();
                        }
                    }
                    GlobalLog.Error("[TakeMapTask] Can't find usable oils in inventory. Weird!");
                    return CleanAnointUi();
                }
                return false;
            }
            GlobalLog.Error($"[TakeMapTask] Something went wrong with anointing. Cassia null? {cassia == (NetworkObject)null}. Map null? {(RemoteMemoryObject)(object)map == (RemoteMemoryObject)null}");
            return true;
        }
        return true;
    }

    private static bool CleanAnointUi() {
        //IL_0079: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c2: Unknown result type (might be due to invalid IL or missing references)
        //IL_0187: Unknown result type (might be due to invalid IL or missing references)
        //IL_018c: Unknown result type (might be due to invalid IL or missing references)
        //IL_018e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0191: Invalid comparison between Unknown and I4
        //IL_01b9: Unknown result type (might be due to invalid IL or missing references)
        if (!AnointingUi.InventoryControl_Main.Inventory.Items.Any() && (RemoteMemoryObject)(object)AnointingUi.Oil1 == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)AnointingUi.Oil2 == (RemoteMemoryObject)null && (RemoteMemoryObject)(object)AnointingUi.Oil3 == (RemoteMemoryObject)null) {
            return true;
        }
        GlobalLog.Warn("[TakeMapTask] Anoint UI is not empty - attempting to clear it");
        while (true) {
            if ((RemoteMemoryObject)(object)AnointingUi.Oil1 != (RemoteMemoryObject)null) {
                GlobalLog.Debug("Removing " + AnointingUi.Oil1.FullName + " in slot 1");
                AnointingUi.FastRemoveOil(1);
            }
            else if (!((RemoteMemoryObject)(object)AnointingUi.Oil2 != (RemoteMemoryObject)null)) {
                if ((RemoteMemoryObject)(object)AnointingUi.Oil3 != (RemoteMemoryObject)null) {
                    GlobalLog.Debug("Removing " + AnointingUi.Oil3.FullName + " in slot 3");
                    AnointingUi.FastRemoveOil(3);
                    continue;
                }
                if (!AnointingUi.InventoryControl_Main.Inventory.Items.Any((Item i) => (RemoteMemoryObject)(object)i != (RemoteMemoryObject)null)) {
                    break;
                }
                Item val = AnointingUi.InventoryControl_Main.Inventory.Items.First((Item i) => (RemoteMemoryObject)(object)i != (RemoteMemoryObject)null);
                GlobalLog.Debug("Removing " + val.FullName + " " + val.Name + " in map slot");
                FastMoveResult val2 = AnointingUi.InventoryControl_Main.FastMove(val.LocalId, true, true);
                if ((int)val2 > 0) {
                    return true;
                }
            }
            else {
                GlobalLog.Debug("Removing " + AnointingUi.Oil2.FullName + " in slot 2");
                AnointingUi.FastRemoveOil(2);
            }
        }
        return false;
    }

    private static bool CanAnoint(CachedMapItem map) {
        if (Settings.AnointMaps) {
            bool flag = map.Stats.ContainsKey((StatTypeGGG)10342);
            bool flag2 = map.Stats.ContainsKey((StatTypeGGG)14763);
            if (flag) {
                int num = map.AffixList.Count((KeyValuePair<string, string> a) => a.Key.Contains("BlightFragmentMushrune"));
                if (flag2) {
                    return num < 9;
                }
                return num < 3;
            }
            return false;
        }
        return false;
    }

    private static bool CanAnoint(Item map) {
        if (Settings.AnointMaps) {
            bool flag = map.Stats.ContainsKey((StatTypeGGG)10342);
            bool flag2 = map.Stats.ContainsKey((StatTypeGGG)14763);
            if (!flag) {
                return false;
            }
            int num = map.Affixes.Count((ModAffix a) => a.Category == "BlightFragmentMushrune");
            GlobalLog.Debug($"[TakeMapTask] {map.CleanName()} anointsAmt: {num}");
            if (flag2) {
                return num < 9;
            }
            return num < 3;
        }
        return false;
    }

    private static async Task GetOils(Item map) {
        bool isRavaged = map.Stats.ContainsKey((StatTypeGGG)14763);
        int oilsNeeded = (isRavaged ? 9 : 3);
        string type = ((oilsNeeded == 9) ? "Blight-ravaged" : "Blighted");
        List<OilEntry> anoOils = Settings.AnointOils.ToList();
        List<OilEntry> usableOils = ((!isRavaged) ? anoOils.Where((OilEntry o) => o.UseOnBlighted && o.AmountToUse > 0).ToList() : anoOils.Where((OilEntry o) => o.UseOnRavaged && o.AmountToUse > 0).ToList());
        foreach (OilEntry oilEntry_0 in usableOils) {
            if (Inventories.InventoryItems.Where((Item i) => Settings.AnointOils.Any((OilEntry o) => i.FullName.Equals(o.Name))).Sum((Item i) => i.StackCount) > oilsNeeded) {
                break;
            }
            if (oilEntry_0.CurrentAmount >= oilEntry_0.AmountToUse) {
                if (await Inventories.FindTabWithCurrency(oilEntry_0.Name)) {
                    if ((int)StashUi.StashTabInfo.TabType != 3) {
                        Item item_0 = StashUi.InventoryControl.Inventory.Items.OrderByDescending((Item i) => i.StackCount).FirstOrDefault((Item i) => i.Name == oilEntry_0.Name);
                        if (!((RemoteMemoryObject)(object)item_0 == (RemoteMemoryObject)null) && item_0.StackCount >= oilEntry_0.AmountToUse && usableOils.Any((OilEntry o) => o.Name.EqualsIgnorecase(item_0.Name))) {
                            await Inventories.SplitAndPlaceItemInMainInventory(StashUi.InventoryControl, item_0, oilEntry_0.AmountToUse);
                        }
                    }
                    else {
                        InventoryControlWrapper inventoryControlWrapper_0 = (from i in Inventories.GetControlsWithCurrency(oilEntry_0.Name)
                                                                             orderby i.CustomTabItem.StackCount descending
                                                                             select i).FirstOrDefault();
                        if (!((RemoteMemoryObject)(object)inventoryControlWrapper_0 == (RemoteMemoryObject)null) && inventoryControlWrapper_0.CustomTabItem.StackCount >= oilEntry_0.AmountToUse && usableOils.Any((OilEntry o) => o.Name.EqualsIgnorecase(inventoryControlWrapper_0.CustomTabItem.Name))) {
                            await Inventories.SplitAndPlaceItemInMainInventory(inventoryControlWrapper_0, inventoryControlWrapper_0.CustomTabItem, oilEntry_0.AmountToUse);
                        }
                    }
                }
                else {
                    GlobalLog.Error("[TakeMapTask:Anoint] Fail to find tab with anoint oils!");
                }
                continue;
            }
            GlobalLog.Error("[TakeMapTask:Anoint] We don't have enough " + oilEntry_0.Name + ". Disabling " + type + " maps until we get enough.");
            if (oilsNeeded != 9 || !oilEntry_0.UseOnRavaged) {
                if (oilEntry_0.UseOnBlighted) {
                    TempIgnoreBlighted = true;
                }
            }
            else {
                TempIgnoreRavaged = true;
            }
            break;
        }
    }

    private static async Task<CachedMapItem> FindProperMap(bool force = false) {
        ((PerCachedValue<List<RegionInfo>>)(object)AtlasHelper.RegionCache).RequestCacheFlush();
        Class34.hashSet_0.Clear();
        HashSet<CachedMapItem> maps = new HashSet<CachedMapItem>();
        if (CachedMaps.Instance.MapCache == null || !CachedMaps.Cached || force) {
            GlobalLog.Debug($"[FindProperMap] Need full reCache {CachedMaps.Instance.MapCache == null}/{!CachedMaps.Cached}/{force}");
            if (CachedMaps.Instance.MapCache != null) {
                CachedMaps.Instance.MapCache.Maps.Clear();
            }
            CachedMaps instance = CachedMaps.Instance;
            instance.MapCache = await Inventories.CacheMapTabs(full: true);
            CachedMaps.Cached = true;
        }
        if (CachedMaps.Instance.MapCache.Maps == null) {
            return null;
        }
        foreach (CachedMapItem map in CachedMaps.Instance.MapCache.Maps) {
            if (GeneralSettings.SimulacrumsEnabled && map.Metadata.Contains("MapFragments/CurrencyAfflictionFragment")) {
                maps.Add(map);
            }
            if (Settings.OnlyRunEnchantedMaps) {
                if (!map.AffixList.Any((KeyValuePair<string, string> a) => Settings.EnchantsToPrioritize.Any((NameEntry e) => a.Value.ContainsIgnorecase(e.Name)))) {
                    continue;
                }
                maps.Add(map);
            }
            if (map.Stats.ContainsKey((StatTypeGGG)6548) || (!Settings.RunConqMaps && map.Stats.ContainsKey((StatTypeGGG)13845)) || ((!Settings.RunBlightedMaps || TempIgnoreBlighted) && map.Stats.ContainsKey((StatTypeGGG)10342) && !map.Stats.ContainsKey((StatTypeGGG)14763)) || ((!Settings.RunBlightRavagedMaps || TempIgnoreRavaged) && map.Stats.ContainsKey((StatTypeGGG)14763)) || (!Settings.AtlasExplorationEnabled && map.Ignored() && !map.Stats.ContainsKey((StatTypeGGG)10342))) {
                continue;
            }
            Rarity rarity = map.Rarity;
            if (((int)rarity == 3 && !MapData.SupportedUniqueMapNames().Contains(map.FullName)) || !map.BelowTierLimit() || ((int)rarity == 2 && Settings.ExistingRares == ExistingRares.NoRun && NoRareUpgrade(map)) || (!Settings.RunUnId && !map.IsIdentified)) {
                continue;
            }
            if (map.GetBannedAffix() != null) {
                if (map.IsCorrupted || map.IsMirrored || map.IsFractured || ((int)rarity == 1 && !HasMagicOrbs)) {
                    continue;
                }
                if ((int)rarity == 2) {
                    if (NoRareUpgrade(map)) {
                        if (Settings.ExistingRares == ExistingRares.NoReroll) {
                            continue;
                        }
                        if (Settings.ExistingRares == ExistingRares.Downgrade) {
                            if (HasScourTransmute) {
                                maps.Add(map);
                            }
                            continue;
                        }
                    }
                    if (!HasRareOrbs) {
                        continue;
                    }
                }
            }
            maps.Add(map);
            if (!Class34.hashSet_0.Contains(map.Name) && !map.IsCorrupted) {
                Class34.hashSet_0.Add(map.Name);
            }
        }
        if (maps.Count != 0) {
            List<CachedMapItem> sortedMaps = (Settings.AtlasExplorationEnabled ? (from m in maps
                                                                                  orderby m.AffixList.Any((KeyValuePair<string, string> a) => Settings.EnchantsToPrioritize.Any((NameEntry e) => a.Value.ContainsIgnorecase(e.Name))) descending, m.Stats.ContainsKey((StatTypeGGG)14763) && !CanAnoint(m) descending, m.Stats.ContainsKey((StatTypeGGG)14763) descending, m.Stats.ContainsKey((StatTypeGGG)10342) && !CanAnoint(m) descending, m.Stats.ContainsKey((StatTypeGGG)10342) descending, (int)m.Rarity == 3 descending, m.Priority() descending, m.Rarity descending, m.MapTier descending
                                                                                  select m).ToList() : (from m in maps
                                                                                                        orderby m.AffixList.Any((KeyValuePair<string, string> a) => Settings.EnchantsToPrioritize.Any((NameEntry e) => a.Value.ContainsIgnorecase(e.Name))) descending, m.Stats.ContainsKey((StatTypeGGG)14763) && !CanAnoint(m) descending, m.Stats.ContainsKey((StatTypeGGG)14763) descending, m.Stats.ContainsKey((StatTypeGGG)10342) && !CanAnoint(m) descending, m.Stats.ContainsKey((StatTypeGGG)10342) descending, m.Priority() descending, m.MapTier descending, m.Rarity descending, m.Quality descending
                                                                                                        select m).ToList());
            CachedMapItem result = sortedMaps.FirstOrDefault();
            if (Settings.RunConqMaps) {
                CachedMapItem conq = sortedMaps.FirstOrDefault((CachedMapItem m) => m.Stats.ContainsKey((StatTypeGGG)13845));
                if (conq != null) {
                    result = conq;
                }
            }
            if (GeneralSettings.SimulacrumsEnabled) {
                CachedMapItem simul = sortedMaps.FirstOrDefault((CachedMapItem m) => m.Metadata.Contains("CurrencyAfflictionFragment"));
                if (simul != null) {
                    result = simul;
                }
            }
            if (result != null) {
                GlobalLog.Info($"[FindProperMap] Found: T{result.MapTier} {result.CleanName()} tab: {result.StashTab}");
            }
            return result;
        }
        return null;
    }

    private static async Task<bool> HandleBlightedMap(Item map) {
        if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
            GlobalLog.Error("[HandleBlightedMap] Fail to find a map.");
            return false;
        }
        Vector2i mapPos = map.LocationTopLeft;
        if (!map.ShouldUpgrade(Settings.ChiselUpgrade) || !HasCurrency(CurrencyNames.Chisel)) {
            return true;
        }
        return await ApplyChisels(mapPos);
    }

    private static async Task<bool> HandleNormalMap(Item map) {
        if (!((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null)) {
            Vector2i mapPos = map.LocationTopLeft;
            if (!map.ShouldUpgrade(Settings.ChiselUpgrade) || !HasCurrency(CurrencyNames.Chisel) || await ApplyChisels(mapPos)) {
                UpdateMapReference(mapPos, ref map);
                if ((MapExtensions.AtlasData.IsCompleted(map) || !Settings.AtlasExplorationEnabled || map.MapTier <= 5 || !HasRareOrbs) && (!map.ShouldUpgrade(Settings.RareUpgrade) || !HasRareOrbs)) {
                    if (!map.ShouldUpgrade(Settings.MagicUpgrade) || !HasMagicOrbs) {
                        return true;
                    }
                    if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Transmutation))) {
                        return false;
                    }
                    return await RerollMagic(mapPos);
                }
                if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy))) {
                    return false;
                }
                return await RerollRare(mapPos);
            }
            return false;
        }
        GlobalLog.Error("[HandleNormalMap] Fail to find a map.");
        return false;
    }

    private static async Task<bool> HandleMagicMap(Item map) {
        if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
            GlobalLog.Error("[HandleMagicMap] Fail to find a map.");
            return false;
        }
        if (map.IsFractured) {
            return true;
        }
        Vector2i mapPos = map.LocationTopLeft;
        bool isBlighted = map.Stats.ContainsKey((StatTypeGGG)10342);
        bool flag = isBlighted;
        bool flag2 = flag;
        if (flag2) {
            flag2 = !(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring));
        }
        if (!flag2) {
            if (map.Quality >= 16 || Settings.ExistingRares != ExistingRares.RerollIfLowQuality || !HasScourTransmute || !HasCurrency(CurrencyNames.Chisel)) {
                if (map.ShouldUpgrade(Settings.MagicRareUpgrade) && HasMagicToRareOrbs && !isBlighted) {
                    if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring))) {
                        return false;
                    }
                    if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy))) {
                        return false;
                    }
                    return await RerollRare(mapPos);
                }
                return await RerollMagic(mapPos);
            }
            if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring))) {
                return false;
            }
            if (!(await ApplyChisels(mapPos))) {
                return false;
            }
            UpdateMapReference(map.LocationTopLeft, ref map);
            if (map.ShouldUpgrade(Settings.RareUpgrade)) {
                if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy)) {
                    UpdateMapReference(map.LocationTopLeft, ref map);
                    return await RerollRare(mapPos);
                }
                return false;
            }
            if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Transmutation)) {
                UpdateMapReference(map.LocationTopLeft, ref map);
                return await RerollMagic(mapPos);
            }
            return false;
        }
        return false;
    }

    private static async Task<bool> HandleRareMap(Item map) {
        if (!((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null)) {
            Vector2i mapPos = map.LocationTopLeft;
            bool flag = map.Stats.ContainsKey((StatTypeGGG)10342) && !Settings.AlchBlightedMaps;
            bool flag2 = flag;
            if (flag2) {
                flag2 = !(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring));
            }
            if (!flag2) {
                if (map.Quality >= 16 || Settings.ExistingRares != ExistingRares.RerollIfLowQuality || !HasScourAlchemy || !HasCurrency(CurrencyNames.Chisel)) {
                    if (Settings.ExistingRares == ExistingRares.Downgrade && HasScourTransmute) {
                        if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring))) {
                            return false;
                        }
                        if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Transmutation))) {
                            return false;
                        }
                        return await RerollMagic(mapPos);
                    }
                    return await RerollRare(mapPos);
                }
                if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring))) {
                    return false;
                }
                if (await ApplyChisels(mapPos)) {
                    UpdateMapReference(map.LocationTopLeft, ref map);
                    if (!map.ShouldUpgrade(Settings.RareUpgrade)) {
                        if ((int)map.Rarity > 0) {
                            return false;
                        }
                        if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Transmutation)) {
                            UpdateMapReference(map.LocationTopLeft, ref map);
                            return await RerollMagic(mapPos);
                        }
                        return false;
                    }
                    if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy)) {
                        UpdateMapReference(map.LocationTopLeft, ref map);
                        return await RerollRare(mapPos);
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        GlobalLog.Error("[HandleRareMap] Fail to find a map.");
        return false;
    }

    public static async Task<bool> RerollMagic(Vector2i mapPos) {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        while (true) {
            Item map = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
            if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
                break;
            }
            Rarity rarity = map.Rarity;
            if ((int)rarity == 1) {
                string affix = map.GetBannedAffix();
                if (affix != null) {
                    GlobalLog.Info("[RerollMagic] Rerolling banned \"" + affix + "\" affix.");
                    if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Alteration))) {
                        return false;
                    }
                    continue;
                }
                if (map.CanAugment() && HasCurrency(CurrencyNames.Augmentation)) {
                    if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Augmentation))) {
                        return false;
                    }
                    continue;
                }
                return true;
            }
            GlobalLog.Error($"[TakeMapTask] RerollMagic is called on {rarity} map.");
            return false;
        }
        GlobalLog.Error($"[RerollMagic] Fail to find a map at {mapPos}.");
        return false;
    }

    public static async Task<bool> RerollRare(Vector2i mapPos) {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        while (true) {
            Item map = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
            if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
                break;
            }
            Rarity rarity = map.Rarity;
            if ((int)rarity == 2) {
                string affix = map.GetBannedAffix();
                if (affix != null) {
                    GlobalLog.Warn("[RerollRare] Rerolling banned \"" + affix + "\" affix.");
                    if (Settings.RerollMethod != RareReroll.Chaos) {
                        if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring))) {
                            return false;
                        }
                        if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy))) {
                            return false;
                        }
                    }
                    else if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Chaos))) {
                        return false;
                    }
                    continue;
                }
                if (map.MapTier >= Settings.MinMapIIQMinTier && map.IIQ() < Settings.MinMapIIQ) {
                    GlobalLog.Warn($"[RerollRare] Map: {map.CleanName()} IIQ: {map.IIQ()}% is less than configured {Settings.MinMapIIQ}%. Rerolling");
                    if (Settings.RerollMethod == RareReroll.Chaos) {
                        if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Chaos)) {
                            continue;
                        }
                        return false;
                    }
                    if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Scouring)) {
                        if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Alchemy)) {
                            continue;
                        }
                        return false;
                    }
                    return false;
                }
                return true;
            }
            GlobalLog.Error($"[TakeMapTask] RerollRare is called on {rarity} map.");
            return false;
        }
        GlobalLog.Error($"[RerollRare] Fail to find a map at {mapPos}.");
        return false;
    }

    public static async Task<bool> ApplyChisels(Vector2i mapPos) {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        while (true) {
            Item map = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
            if ((RemoteMemoryObject)(object)map == (RemoteMemoryObject)null) {
                break;
            }
            if (map.Quality < 20) {
                if (!(await Inventories.ApplyOrb(mapPos, CurrencyNames.Chisel))) {
                    return false;
                }
                continue;
            }
            return true;
        }
        GlobalLog.Error($"[ApplyChisels] Fail to find a map at {mapPos}.");
        return false;
    }

    private static async Task<bool> CorruptMap(Vector2i mapPos) {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        Item map2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
        string originalName = map2.CleanName();
        if (await Inventories.ApplyOrb(mapPos, CurrencyNames.Vaal)) {
            map2 = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
            if (!map2.IsIdentified) {
                GlobalLog.Warn("[CorruptMap] Unidentified corrupted map retains it's original affixes. We are good to go.");
                return true;
            }
            if (!(map2.CleanName() != originalName)) {
                if (map2.Ignored()) {
                    GlobalLog.Warn("[CorruptMap] Map has been changed to another. New one is ignored in settings.");
                    return false;
                }
                if (map2.BelowTierLimit()) {
                    string affix = map2.GetBannedAffix();
                    if (affix != null) {
                        GlobalLog.Warn("[CorruptMap] Banned \"" + affix + "\" has been spawned.");
                        return false;
                    }
                    GlobalLog.Warn("[CorruptMap] Resulting corrupted map fits all requirements. We are good to go.");
                    return true;
                }
                GlobalLog.Warn("[CorruptMap] Map tier has been increased beyond tier limit in settings.");
                return false;
            }
            GlobalLog.Warn("[CorruptMap] Map has been changed to another. Need to re-evaluate.");
            return false;
        }
        return false;
    }

    private static async Task<bool> GetFragment(Item map) {
        List<string> tabs = new List<string>(extensionsSettings_0.GetTabsForCategory("Fragments"));
        int int_0 = (Settings.UseFiveSlotMapDevice ? 4 : 3);
        if (tabs.Count > 1 && StashUi.IsOpened) {
            string currentTab = StashUi.StashTabInfo.DisplayName;
            int index = tabs.IndexOf(currentTab);
            if (index > 0) {
                string tab2 = tabs[index];
                tabs.RemoveAt(index);
                tabs.Insert(0, tab2);
            }
        }
        foreach (string tab3 in tabs.TakeWhile((string tab) => Inventories.InventoryItems.Count((Item i) => i.IsSacrificeFragment() || i.FullName.Contains("Scarab")) < int_0)) {
            if (await Inventories.OpenStashTab(tab3, "GetFragment")) {
                GlobalLog.Debug("[TakeMapTask] Looking for Scarab in \"" + tab3 + "\" tab.");
                if (StashUi.StashTabInfo.IsPremiumSpecial) {
                    InventoryTabType tabType = StashUi.StashTabInfo.TabType;
                    if ((int)tabType != 9) {
                        GlobalLog.Error($"[TakeMapTask] Incorrect tab type ({tabType}) for Scarabs.");
                        BotManager.Stop(new StopReasonData("incorrect_tab_type", $"[TakeMapTask] Incorrect tab type ({tabType}) for Scarabs.", (object)null), false);
                        continue;
                    }
                    if (!map.Stats.ContainsKey((StatTypeGGG)10342) && !map.Stats.ContainsKey((StatTypeGGG)10055) && map.MapTier >= Settings.MinTierToUsePrioritisedScarabs && (int)map.Rarity != 3) {
                        List<InventoryControlWrapper> scarabs = (from c in FragmentTab.AllScarab
                                                                 where (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null
                                                                 where Settings.ScarabsToPrioritize.Any((NameEntry s) => c.CustomTabItem.FullName.ContainsIgnorecase(s.Name)) || PoeNinjaTracker.LookupChaosValue(c.CustomTabItem) < Settings.MaxFragmentCost
                                                                 select c).ToList();
                        scarabs = scarabs.OrderByDescending((InventoryControlWrapper i) => Settings.ScarabsToPrioritize.Any((NameEntry scar) => i.CustomTabItem.FullName.ContainsIgnorecase(scar.Name))).ThenByDescending((InventoryControlWrapper f) => f.CustomTabItem.FullName.Contains("Winged")).ThenByDescending((InventoryControlWrapper f) => f.CustomTabItem.FullName.Contains("Gilded"))
                            .ThenByDescending((InventoryControlWrapper f) => f.CustomTabItem.FullName.Contains("Polished"))
                            .ThenByDescending((InventoryControlWrapper f) => f.CustomTabItem.FullName.Contains("Rusted"))
                            .ToList();
                        foreach (InventoryControlWrapper control in scarabs) {
                            if (Inventories.InventoryItems.Count((Item i) => i.IsSacrificeFragment() || i.FullName.Contains("Scarab")) < int_0) {
                                Item item_2 = control.CustomTabItem;
                                if (!((RemoteMemoryObject)(object)item_2 == (RemoteMemoryObject)null) && (!Settings.ScarabsToIgnore.Any((NameEntry s) => item_2.FullName.ContainsIgnorecase(s.Name)) || Settings.ScarabsToPrioritize.Any((NameEntry s) => item_2.FullName.ContainsIgnorecase(s.Name))) && !Inventories.InventoryItems.Any((Item i) => ScarabType(i) == ScarabType(item_2)) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Shaper")) || !ScarabType(item_2).Contains("Elder")) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Elder")) || !ScarabType(item_2).Contains("Shaper"))) {
                                    GlobalLog.Debug("[TakeMapTask] Found \"" + item_2.Name + "\" in \"" + tab3 + "\" tab.");
                                    await Inventories.SplitAndPlaceItemInMainInventory(control, item_2, 1);
                                }
                                continue;
                            }
                            break;
                        }
                    }
                    List<InventoryControlWrapper> frags2 = (from c in FragmentTab.AllGeneral
                                                            where (RemoteMemoryObject)(object)c.CustomTabItem != (RemoteMemoryObject)null
                                                            select c into i
                                                            where i.CustomTabItem.IsSacrificeFragment()
                                                            select i).ToList();
                    foreach (InventoryControlWrapper control2 in frags2) {
                        if (Inventories.InventoryItems.Count((Item i) => i.IsSacrificeFragment() || i.FullName.Contains("Scarab")) < int_0) {
                            Item item_ = control2.CustomTabItem;
                            if (!((RemoteMemoryObject)(object)item_ == (RemoteMemoryObject)null) && (!Settings.ScarabsToIgnore.Any((NameEntry s) => item_.FullName.ContainsIgnorecase(s.Name)) || Settings.ScarabsToPrioritize.Any((NameEntry s) => item_.FullName.ContainsIgnorecase(s.Name))) && !Inventories.InventoryItems.Any((Item i) => ScarabType(i) == ScarabType(item_)) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Shaper")) || !ScarabType(item_).Contains("Elder")) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Elder")) || !ScarabType(item_).Contains("Shaper"))) {
                                GlobalLog.Debug("[TakeMapTask] Found \"" + item_.Name + "\" in \"" + tab3 + "\" tab.");
                                await Inventories.SplitAndPlaceItemInMainInventory(control2, item_, 1);
                            }
                            continue;
                        }
                        break;
                    }
                    continue;
                }
                List<Item> frags3 = Inventories.StashTabItems.Where((Item i) => i.IsSacrificeFragment() || i.Name.Contains("Scarab")).ToList();
                if (map.Stats.ContainsKey((StatTypeGGG)10342) || map.Stats.ContainsKey((StatTypeGGG)10055) || map.MapTier < Settings.MinTierToUsePrioritisedScarabs || (int)map.Rarity == 3) {
                    frags3 = (from i in frags3
                              where i.IsSacrificeFragment()
                              orderby i.FullName
                              select i).ToList();
                }
                frags3 = (from i in frags3
                          where Settings.ScarabsToPrioritize.Any((NameEntry s) => i.FullName.ContainsIgnorecase(s.Name)) || PoeNinjaTracker.LookupChaosValue(i) < Settings.MaxFragmentCost
                          orderby Settings.ScarabsToPrioritize.Any((NameEntry scar) => i.FullName.ContainsIgnorecase(scar.Name)) descending
                          select i).ThenByDescending((Item f) => f.FullName.Contains("Winged")).ThenByDescending((Item f) => f.FullName.Contains("Gilded")).ThenByDescending((Item f) => f.FullName.Contains("Polished"))
                    .ThenByDescending((Item f) => f.FullName.Contains("Rusted"))
                    .ToList();
                foreach (Item item_0 in frags3) {
                    if (Inventories.InventoryItems.Count((Item i) => i.IsSacrificeFragment() || i.FullName.Contains("Scarab")) < int_0) {
                        if ((!Settings.ScarabsToIgnore.Any((NameEntry s) => item_0.FullName.ContainsIgnorecase(s.Name)) || Settings.ScarabsToPrioritize.Any((NameEntry s) => item_0.FullName.ContainsIgnorecase(s.Name))) && !Inventories.InventoryItems.Any((Item i) => ScarabType(i) == ScarabType(item_0)) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Shaper")) || !ScarabType(item_0).Contains("Elder")) && (!Inventories.InventoryItems.Any((Item i) => ScarabType(i).Contains("Elder")) || !ScarabType(item_0).Contains("Shaper"))) {
                            GlobalLog.Debug("[TakeMapTask] Found \"" + item_0.Name + "\" in \"" + tab3 + "\" tab.");
                            await Inventories.SplitAndPlaceItemInMainInventory(StashUi.InventoryControl, item_0, 1);
                        }
                        continue;
                    }
                    break;
                }
                continue;
            }
            return false;
        }
        if (!Inventories.InventoryItems.Any((Item i) => i.IsSacrificeFragment() || i.FullName.Contains("Scarab"))) {
            GlobalLog.Error("[TakeMapTask] There are no Scarabs in all tabs assigned to them. Now marking them as unavailable.");
            bool_0 = false;
            return true;
        }
        return true;
    }

    private static void ChooseMap(Vector2i mapPos) {
        Item val = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
        OpenMapTask.Enabled = true;
        string arg = val.FullName + " " + val.Name;
        if (val.FullName.Contains(val.Name)) {
            arg = val.FullName;
        }
        StashTask.ProtectedSlot = new Vector2i(100, 100);
        GlobalLog.Warn($"[TakeMapTask] Now going to {arg} [prio: {val.Priority()}].");
    }

    private static void UpdateMapReference(Vector2i mapPos, ref Item map) {
        //IL_000b: Unknown result type (might be due to invalid IL or missing references)
        map = InventoryUi.InventoryControl_Main.Inventory.FindItemByPos(mapPos);
    }

    private static bool NoRareUpgrade(CachedMapItem map) {
        return !map.ShouldUpgrade(Settings.RareUpgrade) && !map.ShouldUpgrade(Settings.MagicRareUpgrade);
    }

    private static bool HasCurrency(string name) {
        if (AvailableCurrency[name]) {
            return true;
        }
        GlobalLog.Debug("[TakeMapTask] HasCurrency is false for " + name + ".");
        return false;
    }

    private static void UpdateAvailableCurrency(string currencyName) {
        if (AvailableCurrency.TryGetValue(currencyName, out var value) && !value) {
            int currencyAmountInStashTab = Inventories.GetCurrencyAmountInStashTab(currencyName);
            if (currencyAmountInStashTab >= dictionary_0[currencyName]) {
                GlobalLog.Info($"[TakeMapTask] There are {currencyAmountInStashTab} \"{currencyName}\" in current stash tab. Now marking this currency as available.");
                AvailableCurrency[currencyName] = true;
            }
        }
    }

    private static void UpdateAvailableFragments() {
        if (!StashUi.StashTabInfo.IsPremiumFragment) {
            if (Inventories.StashTabItems.Any((Item i) => i.IsSacrificeFragment() || i.Name.Contains("Scarab"))) {
                GlobalLog.Info("[TakeMapTask] Fragment has been stashed. Now marking it as available.");
                bool_0 = true;
            }
        }
        else if (!FragmentTab.All.All((InventoryControlWrapper c) => (RemoteMemoryObject)(object)c.CustomTabItem == (RemoteMemoryObject)null)) {
            GlobalLog.Info("[TakeMapTask] Sacrifice Fragment has been stashed. Now marking it as available.");
            bool_0 = true;
        }
    }

    public static string ScarabType(Item item) {
        return item.FullName.Replace("Winged", "").Replace("Gilded", "").Replace("Polished", "")
            .Replace("Rusted", "")
            .Replace("Scarab", "")
            .Trim();
    }

    public async Task<LogicResult> Logic(Logic logic) {
        return (LogicResult)1;
    }

    public void Tick() {
        if (interval_0.Elapsed) {
            CachedMaps.Instance.MapCache = null;
            CachedMaps.Cached = false;
        }
    }

    public void Stop() {
    }

    static TakeMapTask() {
        Settings = GeneralSettings.Instance;
        extensionsSettings_0 = ExtensionsSettings.Instance;
        AvailableCurrency = new Dictionary<string, bool> {
            [CurrencyNames.Transmutation] = true,
            [CurrencyNames.Augmentation] = true,
            [CurrencyNames.Alteration] = true,
            [CurrencyNames.Alchemy] = true,
            [CurrencyNames.Chaos] = true,
            [CurrencyNames.Scouring] = true,
            [CurrencyNames.Chisel] = true,
            [CurrencyNames.Vaal] = true
        };
        dictionary_0 = new Dictionary<string, int> {
            [CurrencyNames.Transmutation] = 1,
            [CurrencyNames.Augmentation] = 5,
            [CurrencyNames.Alteration] = 5,
            [CurrencyNames.Alchemy] = 10,
            [CurrencyNames.Chaos] = 5,
            [CurrencyNames.Scouring] = 5,
            [CurrencyNames.Chisel] = 10,
            [CurrencyNames.Vaal] = 1
        };
        bool_0 = true;
        bool_1 = true;
        interval_0 = new Interval(3600000);
    }
}
