using System;
using System.IO;
using System.Text;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.TraderPlugin.Classes;

internal class Class8
{
	internal static void WriteToLog(SoldItem soldItem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format("[{0}] {1}", DateTime.Now, soldItem.ToString().Replace("[SoldItem] ", "")) + "\n");
		File.AppendAllText("State/" + ((NetworkObject)LokiPoe.Me).Name + "-TraderPlugin-SoldItems.txt", stringBuilder.ToString());
	}
}
