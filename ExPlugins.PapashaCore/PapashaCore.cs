using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using ExPlugins.EXtensions;
using ExPlugins.PapashaCore.Core;
using log4net;

namespace ExPlugins.PapashaCore;

public class PapashaCore : IPlugin, IAuthored, IBase, IConfigurable, IEnableable, ILogicProvider, IMessageHandler, IStartStopEvents, IUrlProvider {
    public static ILog Log;

    public static TaskManager BotTaskManager;

    private static global::ExPlugins.PapashaCore.Core.Core.flWX1mkkDGmd flWX1mkkDGmd_0;

    private static readonly Interval interval_0;

    private static readonly List<string> list_0;

    public static int additionalTimeUse;

    private static readonly Stopwatch stopwatch_0;

    private static bool bool_0;

    private static bool bool_1;

    private static bool bool_2;

    private static bool bool_3;

    public static bool ready;

    private Thread thread_0;

    private PapashaGui papashaGui_0;

    public string Url => "https://discord.gg/HeqYtkujWW";

    public string Name => "PapashaCore";

    public string Description => "Д bI О";

    public string Author => "Papashagodx";

    public string Version => "13.37";

    public JsonSettings Settings => (JsonSettings)(object)PapashaCoreSettings.Instance;

    public UserControl Control => papashaGui_0 ?? (papashaGui_0 = new PapashaGui());

    private static string ConvertToTime(int val) {
        TimeSpan timeSpan = TimeSpan.FromMinutes(val);
        string text = string.Format("{0}{1}{2}{3}", (timeSpan.Duration().Days <= 0) ? string.Empty : string.Format("{0:0} day{1}, ", timeSpan.Days, (timeSpan.Days == 1) ? string.Empty : "s"), (timeSpan.Duration().Hours > 0) ? string.Format("{0:0} hour{1}, ", timeSpan.Hours, (timeSpan.Hours == 1) ? string.Empty : "s") : string.Empty, (timeSpan.Duration().Minutes > 0) ? string.Format("{0:0} minute{1}, ", timeSpan.Minutes, (timeSpan.Minutes == 1) ? string.Empty : "s") : string.Empty, (timeSpan.Duration().Seconds > 0) ? string.Format("{0:0} second{1}", timeSpan.Seconds, (timeSpan.Seconds == 1) ? string.Empty : "s") : string.Empty);
        if (text.EndsWith(", ")) {
            text = text.Substring(0, text.Length - 2);
        }
        if (string.IsNullOrEmpty(text)) {
            text = "0 seconds";
        }
        return text;
    }

    public static TaskManager GetCurrentBotTaskManager() {
        //IL_000e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0015: Expected O, but got Unknown
        //IL_0019: Unknown result type (might be due to invalid IL or missing references)
        IBot current = BotManager.Current;
        Message val = new Message("GetTaskManager", (object)null);
        ((IMessageHandler)current).Message(val);
        TaskManager output = val.GetOutput<TaskManager>(0);
        if (output != null) {
            return output;
        }
        return null;
    }

    private static void FillRemainingTimes() {
        PapashaCoreSettings.Instance.Plugin1Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[0]);
        PapashaCoreSettings.Instance.Plugin2Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[1]);
        PapashaCoreSettings.Instance.Plugin3Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[2]);
        PapashaCoreSettings.Instance.Plugin4Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[3]);
        PapashaCoreSettings.Instance.Plugin5Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[4]);
        PapashaCoreSettings.Instance.Plugin6Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[5]);
        PapashaCoreSettings.Instance.Plugin7Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[6]);
        PapashaCoreSettings.Instance.Plugin8Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[7]);
        PapashaCoreSettings.Instance.Plugin9Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[8]);
        PapashaCoreSettings.Instance.Plugin10Time = ConvertToTime(flWX1mkkDGmd_0.sdbmewknwekb[9]);
    }

    public static string Fork() {
        Random random = new Random();
        string text = "";
        for (int i = 0; i < 5; i++) {
            int value = ((random.Next(1, 3) == 1) ? random.Next(65, 91) : random.Next(97, 123));
            text += Convert.ToChar(value);
        }
        int num = random.Next(0, 10);
        int num2 = 9 - num;
        text = text.Insert(random.Next(0, 5), num.ToString());
        text = text.Insert(random.Next(0, 6), num2.ToString());
        for (int j = 0; j < 4; j++) {
            int value = ((random.Next(1, 3) == 1) ? random.Next(65, 91) : random.Next(97, 123));
            text += Convert.ToChar(value);
        }
        text = text.Insert(random.Next(7, 11), "y");
        text = text.Insert(random.Next(7, 12), random.Next(0, 10).ToString());
        for (int k = 0; k < 10; k++) {
            int value = ((random.Next(1, 3) == 1) ? random.Next(65, 91) : random.Next(97, 123));
            text += Convert.ToChar(value);
        }
        num = random.Next(1, 10);
        num2 = 9 - num;
        int num3 = 15 - num2 - num;
        int num4 = 23 - num3 - num2 - num;
        text = text.Insert(random.Next(13, 23), num.ToString());
        text = text.Insert(random.Next(13, 24), num2.ToString());
        text = text.Insert(random.Next(13, 25), num3.ToString());
        return text.Insert(random.Next(13, 26), num4.ToString());
    }

    public void Initialize() {
        Log.DebugFormat("[PapashaCore] Initialize", Array.Empty<object>());
    }

    public void Deinitialize() {
        Log.DebugFormat("[PapashaCore] Deinitialize", Array.Empty<object>());
    }

    public void Enable() {
        ready = true;
    }

    public void Disable() {
        ready = false;
        if (BotManager.IsRunning) {
            BotManager.Stop(false);
        }
    }

    public async Task<LogicResult> Logic(Logic logic) {
        return (LogicResult)1;
    }

    public MessageResult Message(Message message) {
        return (MessageResult)1;
    }

    public void Start() {
        Log.DebugFormat("[PapashaCore] Start.", Array.Empty<object>());
        bool_1 = true;
        bool_3 = false;
        stopwatch_0.Start();
        flWX1mkkDGmd_0 = global::ExPlugins.PapashaCore.Core.Core.smethod_5();
        FillRemainingTimes();
        if (!bool_2) {
            thread_0 = new Thread(Module) {
                IsBackground = true
            };
            thread_0.Start();
            bool_2 = true;
        }
    }

    public void Tick() {
        if (interval_0.Elapsed && !ready && BotManager.IsRunning) {
            BotManager.Stop(false);
        }
    }

    public void Stop() {
        stopwatch_0.Stop();
        bool_3 = true;
    }

    private void Module() {
        while (bool_1) {
            if ((stopwatch_0.ElapsedMilliseconds >= 60000L && !bool_3) || bool_0) {
                stopwatch_0.Restart();
                list_0.Clear();
                bool_0 = false;
                if (((IAuthored)BotManager.Current).Name == "MapBotEx" || ((IAuthored)BotManager.Current).Name == "QuestBotEx" || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "AbyssEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "AutoFlaskEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "AutoLoginEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "ChaosRecipeEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "IncursionEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "BreachesEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "BasicGemLeveler") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "DeliriumPluginEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "EssencePluginEx") != null || PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "AutoPassiveEx") != null) {
                    list_0.Add("ExPack");
                    flWX1mkkDGmd_0.sdbmewknwekb[0]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "TraderPlugin") != null) {
                    list_0.Add("TraderPlugin");
                    flWX1mkkDGmd_0.sdbmewknwekb[1]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "StashManager") != null) {
                    list_0.Add("StashManager");
                    flWX1mkkDGmd_0.sdbmewknwekb[2]--;
                }
                if (((IAuthored)RoutineManager.Current).Name == "SqRoutine") {
                    list_0.Add("SqRoutine");
                    flWX1mkkDGmd_0.sdbmewknwekb[3]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "BlightPluginEx") != null) {
                    list_0.Add("BlightPluginEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[4]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "TangledAltarsEx") != null) {
                    list_0.Add("TangledAltarsEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[5]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "ItemFilterEx") != null) {
                    list_0.Add("ItemFilterEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[6]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "EquipPluginEx") != null) {
                    list_0.Add("EquipPluginEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[7]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "SimulacrumPluginEx") != null) {
                    list_0.Add("SimulacrumPluginEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[8]--;
                }
                if (PluginManager.EnabledPlugins.FirstOrDefault((IPlugin n) => ((IAuthored)n).Name == "BulkTraderEx") != null) {
                    list_0.Add("BulkTraderEx");
                    flWX1mkkDGmd_0.sdbmewknwekb[9]--;
                }
                FillRemainingTimes();
                string saGewqgwe = global::ExPlugins.PapashaCore.Core.Core.f8f756b6b3def(list_0.Contains("ExPack"), list_0.Contains("TraderPlugin"), list_0.Contains("StashManager"), list_0.Contains("SqRoutine"), list_0.Contains("BlightPluginEx"), list_0.Contains("TangledAltarsEx"), list_0.Contains("ItemFilterEx"), list_0.Contains("EquipPluginEx"), list_0.Contains("SimulacrumPluginEx"), list_0.Contains("BulkTraderEx"));
                global::ExPlugins.PapashaCore.Core.Core.smethod_6(saGewqgwe, 1 + additionalTimeUse);
            }
            else {
                Thread.Sleep(50);
            }
        }
    }

    static PapashaCore() {
        Log = Logger.GetLoggerInstanceForType();
        interval_0 = new Interval(500);
        list_0 = new List<string>();
        additionalTimeUse = 0;
        stopwatch_0 = new Stopwatch();
        bool_0 = true;
    }
}
