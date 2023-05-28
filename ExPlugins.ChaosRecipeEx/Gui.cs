using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.ChaosRecipeEx;

public partial class Gui : UserControl, IComponentConnector
{
	private bool msfjiHeIaf;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
