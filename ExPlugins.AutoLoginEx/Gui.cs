using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DreamPoeBot.Loki.Elements;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.NativeWrappers;

namespace ExPlugins.AutoLoginEx;

public partial class Gui : UserControl, IComponentConnector
{
	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		GatewayComboBox.ItemsSource = Enum.GetValues(typeof(GatewayEnum)).Cast<GatewayEnum>();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void LoadCharactersButton_OnClick(object sender, RoutedEventArgs e)
	{
		ObservableCollection<string> observableCollection = new ObservableCollection<string>();
		if (StateManager.IsSelectCharacterStateActive && !StateManager.IsCreateCharacterStateActive && SelectCharacterState.IsCharacterListLoaded)
		{
			foreach (CharacterEntry item in SelectCharacterState.Characters.OrderBy((CharacterEntry ce) => ce.Name))
			{
				observableCollection.Add(item.Name);
			}
		}
		AutoLoginSettings.Instance.Characters = observableCollection;
	}
}
