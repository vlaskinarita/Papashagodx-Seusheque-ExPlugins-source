using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Common.MVVM;
using JetBrains.Annotations;
using Newtonsoft.Json;

internal class SimulSett : JsonSettings {
    public class SimulSettNotify : INotifyPropertyChanged {
        private Vector2i vector2i_0;

        private string string_0;

        private Vector2i vector2i_1;

        public string Layout {
            get {
                return string_0;
            }
            set {
                string_0 = value;
                OnPropertyChanged("Layout");
            }
        }

        public Vector2i Coords {
            get {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                return vector2i_0;
            }
            set {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                //IL_0002: Unknown result type (might be due to invalid IL or missing references)
                vector2i_0 = value;
                OnPropertyChanged("Coords");
            }
        }

        public Vector2i StashCoords {
            get {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                return vector2i_1;
            }
            set {
                //IL_0001: Unknown result type (might be due to invalid IL or missing references)
                //IL_0002: Unknown result type (might be due to invalid IL or missing references)
                vector2i_1 = value;
                OnPropertyChanged("StashCoords");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SimulSettNotify(string layout, Vector2i coords, Vector2i stashCoords) {
            //IL_0010: Unknown result type (might be due to invalid IL or missing references)
            //IL_0017: Unknown result type (might be due to invalid IL or missing references)
            Layout = layout;
            Coords = coords;
            StashCoords = stashCoords;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private static SimulSett class18_0;

    private bool bool_0;

    private bool bool_1;

    private int int_0;

    private bool bool_2;

    [CompilerGenerated]
    private ObservableCollection<SimulSettNotify> observableCollection_0 = new ObservableCollection<SimulSettNotify>();

    public static SimulSett Instance => class18_0 ?? (class18_0 = new SimulSett());

    [DefaultValue(30)]
    public int MaxWave {
        get {
            return int_0;
        }
        set {
            int_0 = value;
            ((NotificationObject)this).NotifyPropertyChanged<int>((Expression<Func<int>>)(() => MaxWave));
        }
    }

    [DefaultValue(false)]
    public bool KillBossAtTheEnd {
        get {
            return bool_0;
        }
        set {
            bool_0 = value;
            ((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => KillBossAtTheEnd));
        }
    }

    [DefaultValue(true)]
    public bool EnableAnchorPoints {
        get {
            return bool_1;
        }
        set {
            bool_1 = value;
            ((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => EnableAnchorPoints));
        }
    }

    public ObservableCollection<SimulSettNotify> AnchorPoints {
        [CompilerGenerated]
        get {
            return observableCollection_0;
        }
        [CompilerGenerated]
        set {
            observableCollection_0 = value;
        }
    }

    [JsonIgnore]
    public bool StopRequest {
        get {
            return bool_2;
        }
        set {
            bool_2 = value;
            ((NotificationObject)this).NotifyPropertyChanged<bool>((Expression<Func<bool>>)(() => StopRequest));
        }
    }

    private SimulSett() : base(JsonSettings.GetSettingsFilePath(new string[] { Configuration.Instance.Name,"SimulacrumPluginEx.json" })) {
        if (AnchorPoints == null || !AnchorPoints.Any()) {
            AnchorPoints = new ObservableCollection<SimulSettNotify>
            {
                new SimulSettNotify("Oriath Delusion", new Vector2i(561, 287), new Vector2i(368, 283)),
                new SimulSettNotify("Lunacy's Watch", new Vector2i(316, 657), new Vector2i(386, 630)),
                new SimulSettNotify("The Bridge Enraptured", new Vector2i(549, 590), new Vector2i(592, 622)),
                new SimulSettNotify("Hysteriagate", new Vector2i(156, 261), new Vector2i(279, 364)),
                new SimulSettNotify("The Syndrome Encampment", new Vector2i(335, 412), new Vector2i(322, 437))
            };
        }
    }
}
