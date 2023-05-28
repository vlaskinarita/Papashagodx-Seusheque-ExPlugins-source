using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using ExPlugins.TangledAltarsEx.Classes;
using JetBrains.Annotations;

namespace ExPlugins.TangledAltarsEx;

public class TangledAltarsExSettings : JsonSettings
{
	public class NameEntry : INotifyPropertyChanged
	{
		private string string_0;

		public string Name
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				OnPropertyChanged("Name");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public NameEntry()
		{
		}

		public NameEntry(string name)
		{
			Name = name;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static TangledAltarsExSettings tangledAltarsExSettings_0;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private ObservableCollection<ShrineMod> observableCollection_0 = new ObservableCollection<ShrineMod>();

	[CompilerGenerated]
	private ObservableCollection<NameEntry> observableCollection_1 = new ObservableCollection<NameEntry>();

	public static TangledAltarsExSettings Instance => tangledAltarsExSettings_0 ?? (tangledAltarsExSettings_0 = new TangledAltarsExSettings());

	[DefaultValue(false)]
	public bool DebugMode
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

	public ObservableCollection<ShrineMod> ModList
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

	public ObservableCollection<NameEntry> ModsToIgnoreList
	{
		[CompilerGenerated]
		get
		{
			return observableCollection_1;
		}
		[CompilerGenerated]
		set
		{
			observableCollection_1 = value;
		}
	}

	private TangledAltarsExSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"TangledAltarsEx.json"
		}))
	{
		ModsToIgnoreList = new ObservableCollection<NameEntry>(from s in ModsToIgnoreList
			where !string.IsNullOrWhiteSpace(s.Name)
			select s into e
			group e by e.Name into g
			select g.First());
		if (ModList == null || ModList.Count < 1)
		{
			ModList = new ObservableCollection<ShrineMod>
			{
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Grand Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Grand Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Greater Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Greater Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Lesser Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.2–2.4)% chance to drop an additional Lesser Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Awakened Sextant"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Blessed Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Cartographer's Chisel"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Chaos Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Chromatic Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Corrupted Item"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Corrupted Unique Item"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Map"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Unique Armour"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Unique Item"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Unique Map"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards a Unique Weapon"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Basic Currency"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Currency"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Gems"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards League Currency"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Levelled Gems"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards other Divination Cards"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Quality Gems"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divination Card which rewards Unique Jewellery"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Divine Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Eldritch Chaos Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Eldritch Exalted Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Eldritch Orb of Annulment"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Enkindling Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Exalted Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gemcutter's Prism"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Abyss Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Ambush Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Bestiary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Blight Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Breach Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Cartography Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Divination Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Elder Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Expedition Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Harbinger Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Legion Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Metamorph Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Reliquary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Shaper Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Sulphite Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Gilded Torment Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Glassblower's Bauble"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Grand Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Grand Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Greater Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Greater Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Instilling Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Jeweller's Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Lesser Eldritch Ember"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Lesser Eldritch Ichor"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Alteration"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Annulment"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Binding"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Fusing"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Horizons"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Regret"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Scouring"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Orb of Unmaking"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Abyss Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Ambush Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Bestiary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Blight Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Breach Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Cartography Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Divination Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Elder Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Expedition Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Harbinger Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Legion Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Metamorph Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Reliquary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Shaper Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Sulphite Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Polished Torment Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Regal Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Abyss Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Ambush Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Bestiary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Blight Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Breach Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Cartography Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Divination Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Elder Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Expedition Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Harbinger Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Legion Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Metamorph Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Reliquary Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Shaper Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Sulphite Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Rusted Torment Scarab"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Vaal Orb"),
				new ShrineMod("Eldritch Minions gain:", "(1.6–3.2)% chance to drop an additional Veiled Chaos Orb"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Awakened Sextants"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Blessed Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Cartographer's Chisels"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Chaos Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Chromatic Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Corrupted Item"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Corrupted Unique Item"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Map"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Unique Armour"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Unique Item"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Unique Map"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward a Unique Weapon"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Basic Currency"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Currency"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Gems"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward League Currency"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Levelled Gems"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward other Divination Cards"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Quality Gems"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divination Cards which reward Unique Jewellery"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Divine Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Eldritch Chaos Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Eldritch Exalted Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Eldritch Orbs of Annulment"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Enkindling Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Exalted Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gemcutter's Prisms"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Abyss Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Ambush Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Bestiary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Blight Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Breach Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Cartography Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Divination Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Elder Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Expedition Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Harbinger Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Legion Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Metamorph Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Reliquary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Shaper Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Sulphite Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Gilded Torment Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Glassblower's Baubles"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Grand Eldritch Embers"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Grand Eldritch Ichors"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Greater Eldritch Embers"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Greater Eldritch Ichors"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Instilling Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Jeweller's Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Lesser Eldritch Embers"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Lesser Eldritch Ichors"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Alteration"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Annulment"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Binding"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Fusing"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Horizons"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Regret"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Scouring"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Orbs of Unmaking"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Abyss Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Ambush Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Bestiary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Blight Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Breach Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Cartography Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Divination Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Elder Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Expedition Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Harbinger Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Legion Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Metamorph Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Reliquary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Shaper Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Sulphite Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Polished Torment Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Regal Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Abyss Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Ambush Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Bestiary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Blight Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Breach Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Cartography Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Divination Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Elder Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Expedition Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Harbinger Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Legion Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Metamorph Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Reliquary Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Shaper Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Sulphite Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Rusted Torment Scarabs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Vaal Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops (2–4) additional Veiled Chaos Orbs"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Grand Eldritch Ember"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Grand Eldritch Ichor"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Greater Eldritch Ember"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Greater Eldritch Ichor"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Lesser Eldritch Ember"),
				new ShrineMod("Map boss gains:", "Final Boss drops 1 additional Lesser Eldritch Ichor"),
				new ShrineMod("Player gains:", "(10–15)% increased Quantity of Items found in this Area"),
				new ShrineMod("Player gains:", "(5–10)% increased Rarity of Items found in this Area"),
				new ShrineMod("Player gains:", "(8–12)% increased Experience gain"),
				new ShrineMod("Player gains:", "Basic Currency Items dropped by slain Enemies have (10–15)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Basic Currency Items dropped by slain Enemies have (15–30)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Divination Cards dropped by slain Enemies have (15–30)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Gems dropped by slain Enemies have (10–15)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Gems dropped by slain Enemies have (15–30)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Maps dropped by slain Enemies have (10–15)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Maps dropped by slain Enemies have (15–30)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Scarabs dropped by slain Enemies have (15–30)% chance to be Duplicated"),
				new ShrineMod("Player gains:", "Unique Items dropped by slain Enemies have (15–30)% chance to be Duplicated")
			};
		}
	}
}
