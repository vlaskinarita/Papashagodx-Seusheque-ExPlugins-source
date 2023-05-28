using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.BulkTraderEx;

public partial class BulkTraderExGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public BulkTraderExGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void ForceTradeOnClick(object sender, RoutedEventArgs e)
	{
		BulkTraderExSettings.Instance.ShouldTrade = true;
	}
}
