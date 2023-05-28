using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.AutoPassiveEx;

public partial class AutoPassiveExGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public AutoPassiveExGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
