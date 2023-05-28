using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.TabSetuper;

public partial class TabSetuperGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public TabSetuperGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
