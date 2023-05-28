using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.BlightPluginEx;

public partial class BlightGui : UserControl, IComponentConnector
{
	public class WeightColorSelector : IValueConverter
	{
		public static readonly WeightColorSelector Instance;

		private readonly Color sDkCiEvgJyk = Colors.GreenYellow;

		private readonly Color color_0 = Colors.DarkRed;

		private readonly Color color_1 = Colors.GhostWhite;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int num)
			{
				return (num > 50) ? new SolidColorBrush(sDkCiEvgJyk) : ((num > 0) ? new SolidColorBrush(color_1) : new SolidColorBrush(color_0));
			}
			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		static WeightColorSelector()
		{
			Instance = new WeightColorSelector();
		}
	}

	private bool bool_0;

	public BlightGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}
}
