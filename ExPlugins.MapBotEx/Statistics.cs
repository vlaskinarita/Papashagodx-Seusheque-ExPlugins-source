using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.SimulacrumPluginEx;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ExPlugins.MapBotEx;

public class Statistics : INotifyPropertyChanged
{
	public class MapStatistics : INotifyPropertyChanged
	{
		private string string_0;

		private string string_1;

		private string string_2;

		private string string_3;

		public string Name
		{
			get
			{
				return string_3;
			}
			set
			{
				string_3 = value;
				OnPropertyChanged("Name");
			}
		}

		public string FinishTime
		{
			get
			{
				return string_2;
			}
			set
			{
				string_2 = value;
				OnPropertyChanged("FinishTime");
			}
		}

		public string ChaosValueLooted
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				OnPropertyChanged("ChaosValueLooted");
			}
		}

		public string Date
		{
			get
			{
				return string_1;
			}
			set
			{
				string_1 = value;
				OnPropertyChanged("Date");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static readonly string string_0;

	private static Statistics statistics_0;

	private readonly HashSet<Vector2i> hashSet_0 = new HashSet<Vector2i>();

	private readonly List<int> list_0 = new List<int>();

	private readonly List<int> list_1 = new List<int>();

	private readonly List<int> list_2 = new List<int>();

	private readonly Interval interval_0 = new Interval(500);

	private readonly Stopwatch stopwatch_0 = Stopwatch.StartNew();

	public readonly Stopwatch MapTimer = new Stopwatch();

	private int int_0;

	private int int_1;

	private string string_1 = "0";

	private double double_0;

	private double double_1;

	private int int_2;

	private int int_3;

	private int int_4;

	[CompilerGenerated]
	private ObservableCollection<MapStatistics> observableCollection_0 = new ObservableCollection<MapStatistics>();

	[CompilerGenerated]
	private double double_2;

	public string CurrentMapName = "UNKNOWN";

	public static Statistics Instance => statistics_0 ?? (statistics_0 = new Statistics());

	public ObservableCollection<MapStatistics> MapsStatistics
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_0;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_0 = value;
		}
	}

	public int TotalEntered
	{
		get
		{
			return int_2;
		}
		set
		{
			if (value != int_2)
			{
				int_2 = value;
				OnPropertyChanged("TotalEntered");
				OnPropertyChanged("MapIncome");
			}
		}
	}

	public int TotalFinished
	{
		get
		{
			return int_3;
		}
		set
		{
			if (value != int_3)
			{
				int_3 = value;
				OnPropertyChanged("TotalFinished");
			}
		}
	}

	public int TotalFound
	{
		get
		{
			return int_4;
		}
		set
		{
			if (value != int_4)
			{
				int_4 = value;
				OnPropertyChanged("TotalFound");
				OnPropertyChanged("MapIncome");
			}
		}
	}

	public int AverageTierEntered
	{
		get
		{
			return int_0;
		}
		set
		{
			if (value != int_0)
			{
				int_0 = value;
				OnPropertyChanged("AverageTierEntered");
			}
		}
	}

	public int AverageTierFound
	{
		get
		{
			return int_1;
		}
		set
		{
			if (value != int_1)
			{
				int_1 = value;
				OnPropertyChanged("AverageTierFound");
			}
		}
	}

	public string AverageTimeSpent
	{
		get
		{
			return string_1;
		}
		set
		{
			if (!(value == string_1))
			{
				string_1 = value;
				OnPropertyChanged("AverageTimeSpent");
			}
		}
	}

	public string MapIncome => (TotalFound - TotalEntered).ToString("+#;-#;0");

	public string TotalTimeSpent => stopwatch_0.Elapsed.ToString("hh\\:mm\\:ss");

	public string CurrentMapInfo => $"{CurrentMapName} / {MapTimer.Elapsed:mm\\:ss}";

	public string TotalChaosValue => double_1.ToString("0.00");

	public string ChaosPerHour => ChaosPerHourDouble.ToString("0.00");

	public double ChaosPerHourDouble
	{
		[CompilerGenerated]
		get
		{
			return double_2;
		}
		[CompilerGenerated]
		private set
		{
			double_2 = value;
		}
	}

	public string ChaosLootedThisMap => double_0.ToString("0.00");

	public event PropertyChangedEventHandler PropertyChanged;

	private Statistics()
	{
		Configuration.OnSaveAll += delegate
		{
			Save();
		};
		Events.AreaChanged += OnAreaChanged;
		Events.PricedItemLootedEvent += OnItemLooted;
		LokiPoe.OnGuiTick += delegate
		{
			OnPropertyChanged("TotalTimeSpent");
			OnPropertyChanged("CurrentMapInfo");
			OnPropertyChanged("TotalChaosValue");
			OnPropertyChanged("ChaosPerHour");
		};
	}

	private void OnItemLooted(KeyValuePair<Item, double> pair)
	{
		if (GeneralSettings.Instance.ItemNamesToIgnoreInTotalChaosValueCalc.Any((NameEntry e) => e != null && (e.Name.Equals(pair.Key.Name) || e.Name.Equals(pair.Key.FullName))))
		{
			return;
		}
		string text = "";
		string text2 = "";
		if (pair.Key.StackCount > 1)
		{
			text2 = $"{pair.Key.StackCount}X ";
		}
		if (pair.Key.IsMap())
		{
			foreach (Item item in InventoryUi.InventoryControl_Main.Inventory.Items.Where((Item i) => i.Stats.ContainsKey((StatTypeGGG)10342)))
			{
				if (!(item.FullName + item.Name != pair.Key.FullName + pair.Key.Name))
				{
					text = (item.Stats.ContainsKey((StatTypeGGG)14763) ? "Blight-ravaged  " : "Blighted  ");
				}
			}
		}
		GlobalLog.Debug($"[Statistics] Total chaos value looted on this map: {double_0:0.00}c + {pair.Value:0.00}c [{text2}{text}{pair.Key.Name}]");
		double_0 += pair.Value;
		double_1 += pair.Value;
	}

	private void OnAreaChanged(object sender, AreaChangedArgs args)
	{
		DatWorldAreaWrapper newArea = args.NewArea;
		if (!newArea.IsMap)
		{
			MapTimer.Stop();
		}
		else
		{
			MapTimer.Start();
		}
		if (newArea.IsTown || newArea.IsHideoutArea)
		{
			GlobalLog.Info("[Statistics] Clearing inventory map positions.");
			hashSet_0.Clear();
		}
	}

	public void OnNewMapEnter()
	{
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		int totalEntered = TotalEntered + 1;
		TotalEntered = totalEntered;
		LocalData.MapMods.TryGetValue((StatTypeGGG)10011, out var value);
		if (!LocalData.MapMods.ContainsKey((StatTypeGGG)14763))
		{
			if (LocalData.MapMods.ContainsKey((StatTypeGGG)10342))
			{
				CurrentMapName = $"Blighted {currentArea.Name} (T{value})";
			}
			else if (currentArea.Id.Contains("Affliction"))
			{
				CurrentMapName = $"Simulacrum: {currentArea.Name} [Wave: {0}]";
				value = 0;
			}
			else
			{
				CurrentMapName = $"{currentArea.Name} (T{value})";
			}
		}
		else
		{
			CurrentMapName = $"Blight-Ravaged {currentArea.Name} (T{value})";
		}
		list_0.Add(value);
		AverageTierEntered = Round(list_0.Average());
		MapTimer.Restart();
		double_0 = 0.0;
	}

	public void OnMapFinish(bool failed = false)
	{
		int totalFinished = TotalFinished + 1;
		TotalFinished = totalFinished;
		MapTimer.Stop();
		list_2.Add(Round(MapTimer.Elapsed.TotalSeconds));
		AverageTimeSpent = TimeSpan.FromSeconds(list_2.Average()).ToString("hh\\:mm\\:ss");
		if (CurrentMapName != "UNKNOWN")
		{
			string text = "Successfully finished " + CurrentMapInfo + ".";
			if (failed)
			{
				text = "Ran out of portals for " + CurrentMapInfo + ".";
			}
			if (GeneralSettings.Instance.EnableMapLootStatistics)
			{
				text = text + " Picked " + ChaosLootedThisMap + "c worth of loot ";
			}
			text += $"[{TotalFinished} maps finished in this session]";
			Utility.BroadcastMessage((object)this, "stat_map_finished", new object[1] { text });
			GlobalLog.Info("[Statistics] " + text);
		}
		Application.Current.Dispatcher.Invoke(delegate
		{
			MapStatistics item = new MapStatistics
			{
				Date = DateTime.Now.ToString("dd/MM HH:mm:ss"),
				Name = CurrentMapName,
				FinishTime = CurrentMapInfo,
				ChaosValueLooted = ChaosLootedThisMap
			};
			MapsStatistics.Add(item);
			OnPropertyChanged("MapsStatistics");
		});
	}

	public void Tick()
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		if (!interval_0.Elapsed)
		{
			return;
		}
		if (LokiPoe.IsInGame)
		{
			ChaosPerHourDouble = double_1 / (stopwatch_0.Elapsed.TotalSeconds / 3600.0);
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			if (!currentArea.IsMap)
			{
				return;
			}
			if (currentArea.Id.Contains("Affliction"))
			{
				CurrentMapName = $"Simulacrum: {currentArea.Name} [Wave: {global::ExPlugins.SimulacrumPluginEx.SimulacrumPluginEx.CurrentWave}]";
			}
			List<Item> list = Inventories.InventoryItems.Where((Item i) => i.IsMap() && !hashSet_0.Contains(i.LocationTopLeft)).ToList();
			if (list.Count == 0)
			{
				return;
			}
			foreach (Item item in list)
			{
				GlobalLog.Info("[Statistics] Found \"" + item.Name + "\".");
				hashSet_0.Add(item.LocationTopLeft);
				list_1.Add(item.MapTier);
			}
			TotalFound = list_1.Count;
			AverageTierFound = Round(list_1.Average());
		}
		else
		{
			MapTimer.Stop();
		}
	}

	private static int Round(double d)
	{
		return (int)Math.Round(d);
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void Save()
	{
		ObservableCollection<MapStatistics> mapsStatistics = MapsStatistics;
		string contents = JsonConvert.SerializeObject((object)mapsStatistics, (Formatting)1);
		File.WriteAllText(string_0, contents);
	}

	public void Load()
	{
		if (!File.Exists(string_0))
		{
			return;
		}
		string text = File.ReadAllText(string_0);
		if (!string.IsNullOrWhiteSpace(text))
		{
			ObservableCollection<MapStatistics> observableCollection = JsonConvert.DeserializeObject<ObservableCollection<MapStatistics>>(text);
			if (observableCollection != null)
			{
				MapsStatistics = observableCollection;
			}
			else
			{
				GlobalLog.Error("[MapBotEx] Fail to load \"MapStatistics.json\". Json deserealizer returned null.");
			}
		}
		else
		{
			GlobalLog.Error("[MapBotEx] Fail to load \"MapStatistics.json\". File is empty.");
		}
	}

	static Statistics()
	{
		string_0 = Path.Combine("State/", "MapStatistics.json");
	}
}
