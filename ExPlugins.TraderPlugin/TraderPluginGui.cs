using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.TraderPlugin;

public partial class TraderPluginGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public TraderPluginGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
