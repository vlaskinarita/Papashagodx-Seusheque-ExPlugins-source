using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.QuestBotEx;

public partial class Gui : UserControl, IComponentConnector, IStyleConnector
{
	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ResetIfEmpty(sender as ComboBox);
	}

	private void Combobox_OnLoaded(object sender, RoutedEventArgs e)
	{
		ResetIfEmpty(sender as ComboBox);
	}

	private static void ResetIfEmpty(ComboBox box)
	{
		if (box != null && box.SelectedIndex < 0)
		{
			box.SelectedIndex = 0;
		}
	}

	private void AddRuleButton_OnClick(object sender, RoutedEventArgs e)
	{
		QuestBotSettings.GrindingRule item = new QuestBotSettings.GrindingRule
		{
			GrindArea = new QuestBotSettings.Area()
		};
		QuestBotSettings.Instance.GrindingRules.Add(item);
	}

	private void DeleteRuleButton_OnClick(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		QuestBotSettings.GrindingRule item = (QuestBotSettings.GrindingRule)button.DataContext;
		QuestBotSettings.Instance.GrindingRules.Remove(item);
	}
}
