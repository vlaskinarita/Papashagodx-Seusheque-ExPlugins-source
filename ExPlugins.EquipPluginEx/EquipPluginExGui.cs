using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.EquipPluginEx;

public partial class EquipPluginExGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public EquipPluginExGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
