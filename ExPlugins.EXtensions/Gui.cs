using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DreamPoeBot.Loki.Bot;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions;

public partial class Gui : UserControl, IComponentConnector
{
	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void UpdateReport_Button_OnClick(object sender, RoutedEventArgs e)
	{
		PauseReportText.Text = ExtensionsSettings.Instance.PauseReport();
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		DiscordNotifier.AddNotification("test!", ping: false);
		Utility.BroadcastMessage((object)null, "notifier_test", new object[1] { "CUSTOM_TEST" });
	}
}
