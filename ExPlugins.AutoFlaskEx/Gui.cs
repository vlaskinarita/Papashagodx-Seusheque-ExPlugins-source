using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExPlugins.AutoFlaskEx;

public partial class Gui : UserControl, IComponentConnector, IStyleConnector
{
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

	public class VisibilityConverter : IValueConverter
	{
		public static readonly VisibilityConverter Instance;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TriggerType triggerType = (TriggerType)value;
			string text = (string)parameter;
			return triggerType switch
			{
				TriggerType.Es => (!(text == "Es")) ? Visibility.Collapsed : Visibility.Visible, 
				TriggerType.Mobs => (!(text == "Mobs") && !(text == "MobsOrAttack")) ? Visibility.Collapsed : Visibility.Visible, 
				TriggerType.Attack => (!(text == "Attack") && !(text == "MobsOrAttack")) ? Visibility.Collapsed : Visibility.Visible, 
				TriggerType.Hp => (!(text == "Hp")) ? Visibility.Collapsed : Visibility.Visible, 
				_ => Visibility.Collapsed, 
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		static VisibilityConverter()
		{
			Instance = new VisibilityConverter();
		}
	}

	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void AddFlaskTrigger(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		ObservableCollection<FlaskTrigger> observableCollection = (ObservableCollection<FlaskTrigger>)button.Tag;
		observableCollection.Add(new FlaskTrigger());
	}

	private void RemoveFlaskTrigger(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		FlaskTrigger item = (FlaskTrigger)button.DataContext;
		Settings.FlaskEntry flaskEntry = (Settings.FlaskEntry)button.Tag;
		flaskEntry.Triggers.Remove(item);
	}
}
