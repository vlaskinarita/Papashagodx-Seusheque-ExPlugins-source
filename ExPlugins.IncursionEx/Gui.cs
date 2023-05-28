using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.IncursionEx;

public partial class Gui : UserControl, IComponentConnector
{
	public class InvertBoolConverter : IValueConverter
	{
		public static readonly InvertBoolConverter Instance;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(bool)value;
		}

		static InvertBoolConverter()
		{
			Instance = new InvertBoolConverter();
		}
	}

	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void DataGridUnselectAll(object sender, SelectionChangedEventArgs e)
	{
		((DataGrid)sender).UnselectAll();
	}
}
