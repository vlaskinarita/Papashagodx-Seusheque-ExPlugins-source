using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.TangledAltarsEx;

public partial class TangledAltarsGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public TangledAltarsGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
