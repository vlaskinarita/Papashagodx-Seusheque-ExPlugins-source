using System.Windows;
using JetBrains.Annotations;

namespace ExPlugins.EXtensions;

public static class MessageBoxes
{
	public static void Error(string message)
	{
		MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
	}

	[StringFormatMethod("message")]
	public static void Error(string message, params object[] args)
	{
		MessageBox.Show(string.Format(message, args), "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
	}

	public static void Warning(string message)
	{
		MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
	}

	[StringFormatMethod("message")]
	public static void Warning(string message, params object[] args)
	{
		MessageBox.Show(string.Format(message, args), "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
	}
}
