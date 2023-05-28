using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.PapashaCore;

public partial class PapashaGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public PapashaGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
