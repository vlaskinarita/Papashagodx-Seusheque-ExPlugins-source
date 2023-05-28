using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.MapBotEx;

public partial class Gui : UserControl, IComponentConnector, IStyleConnector
{
	public class EnumToBoolConverter : IValueConverter
	{
		public static readonly EnumToBoolConverter Instance;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value?.Equals(parameter) ?? false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (!value.Equals(true)) ? Binding.DoNothing : parameter;
		}

		static EnumToBoolConverter()
		{
			Instance = new EnumToBoolConverter();
		}
	}

	public class DescriptionConverter : IValueConverter
	{
		public static readonly DescriptionConverter Instance;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				FieldInfo field = value.GetType().GetField(value.ToString());
				object[] customAttributes = field.GetCustomAttributes(inherit: false);
				foreach (object obj in customAttributes)
				{
					if (obj is DescriptionAttribute descriptionAttribute)
					{
						return descriptionAttribute.Description;
					}
				}
				return value.ToString();
			}
			return "null";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		static DescriptionConverter()
		{
			Instance = new DescriptionConverter();
		}
	}

	private readonly ICollectionView icollectionView_0;

	private readonly ICollectionView icollectionView_1;

	private readonly ToolTip toolTip_0 = new ToolTip();

	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		icollectionView_1 = CollectionViewSource.GetDefaultView(MapSettings.Instance.MapList);
		icollectionView_0 = CollectionViewSource.GetDefaultView(AffixSettings.Instance.AffixList);
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private bool MapFilter(object obj)
	{
		string text = MapSearchTextBox.Text;
		MapData mapData = (MapData)obj;
		string[] array = mapData.Name.Split(' ');
		string[] array2 = array;
		int num = 0;
		while (true)
		{
			if (num < array2.Length)
			{
				string text2 = array2[num];
				if (text2.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	private bool AffixFilter(object obj)
	{
		string text = AffixSearchTextBox.Text;
		AffixData affixData = (AffixData)obj;
		return affixData.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase) || affixData.Description.ContainsIgnorecase(text);
	}

	private void MapSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (string.IsNullOrEmpty(MapSearchTextBox.Text))
		{
			icollectionView_1.Filter = null;
		}
		icollectionView_1.Filter = MapFilter;
	}

	private void AffixSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (string.IsNullOrEmpty(AffixSearchTextBox.Text))
		{
			icollectionView_0.Filter = null;
		}
		icollectionView_0.Filter = AffixFilter;
	}

	private async void MapGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
	{
		if (e.Column.Header is string header && !(header != "E%"))
		{
			MapType type = ((MapData)e.Row.DataContext).Type;
			if (type != 0 && type != MapType.Bossroom)
			{
				e.Cancel = true;
				await ShowTooltip("Only for Regular and Bossroom maps");
			}
		}
	}

	private async void OnIbCheckBoxClicked(object sender, RoutedEventArgs e)
	{
		MapData mapData = (MapData)((CheckBox)sender).DataContext;
		if (mapData.Type != MapType.Bossroom)
		{
			await ShowTooltip("Only for Bossroom maps");
		}
	}

	private async void OnFtCheckBoxClicked(object sender, RoutedEventArgs e)
	{
		MapData mapData = (MapData)((CheckBox)sender).DataContext;
		MapType type = mapData.Type;
		if (type != MapType.Multilevel && type != MapType.Complex)
		{
			await ShowTooltip("Only for Multilevel and Complex maps");
		}
	}

	private async Task ShowTooltip(string msg)
	{
		if (!toolTip_0.IsOpen)
		{
			toolTip_0.Content = msg;
			toolTip_0.IsOpen = true;
			await Task.Delay(2000);
			toolTip_0.IsOpen = false;
		}
	}

	private void DataGridUnselectAll(object sender, SelectionChangedEventArgs e)
	{
		((DataGrid)sender).UnselectAll();
	}

	private void ClearHistoryButton_OnClick(object sender, RoutedEventArgs e)
	{
		Statistics.Instance.MapsStatistics.Clear();
		if (!Statistics.Instance.MapsStatistics.Any())
		{
			GlobalLog.Warn("[Statistics] Map statistics cleared!");
		}
	}
}
