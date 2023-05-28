using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.StashManager;

public partial class StashManagerGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public StashManagerGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
