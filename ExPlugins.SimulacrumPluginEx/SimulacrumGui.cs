using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using ExPlugins.EXtensions;

namespace ExPlugins.SimulacrumPluginEx;

public partial class SimulacrumGui : UserControl, IComponentConnector
{
	public class VectorToStringConverter : IValueConverter
	{
		public static readonly VectorToStringConverter Instance;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (value != null)
			{
				Vector2i val = (Vector2i)value;
				int x = val.X;
				int y = val.Y;
				return $"[{x}, {y}]";
			}
			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		static VectorToStringConverter()
		{
			Instance = new VectorToStringConverter();
		}
	}

	private bool bool_0;

	public SimulacrumGui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void DumpCoords(object sender, RoutedEventArgs e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (!LokiPoe.IsInGame)
		{
			return;
		}
		foreach (SimulSett.SimulSettNotify anchorPoint in SimulSett.Instance.AnchorPoints)
		{
			string name = World.CurrentArea.Name;
			if (anchorPoint.Layout.Equals(name))
			{
				anchorPoint.Coords = LokiPoe.MyPosition;
				GlobalLog.Warn($"[SimulacrumPluginEx] {anchorPoint.Layout} anchor point had been set to {LokiPoe.MyPosition}.");
			}
		}
	}
}
