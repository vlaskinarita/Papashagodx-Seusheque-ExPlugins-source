using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.NullBotEx;

public partial class NullBotExGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public NullBotExGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
