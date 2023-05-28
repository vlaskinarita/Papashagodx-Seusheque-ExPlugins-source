using System.Windows.Media;
using DreamPoeBot.Loki.Game.GameData;

namespace ExPlugins.EXtensions;

public static class RarityColors
{
	public static readonly SolidColorBrush Normal;

	public static readonly SolidColorBrush Magic;

	public static readonly SolidColorBrush Rare;

	public static readonly SolidColorBrush Unique;

	public static readonly SolidColorBrush Currency;

	public static readonly SolidColorBrush Gem;

	public static readonly SolidColorBrush Quest;

	public static readonly SolidColorBrush Card;

	public static readonly SolidColorBrush Prophecy;

	public static SolidColorBrush FromRarity(Rarity rarity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected I4, but got Unknown
		return (int)rarity switch
		{
			0 => Normal, 
			1 => Magic, 
			2 => Rare, 
			3 => Unique, 
			4 => Gem, 
			5 => Currency, 
			6 => Quest, 
			_ => null, 
		};
	}

	static RarityColors()
	{
		Normal = new SolidColorBrush(Color.FromRgb(200, 200, 200));
		Magic = new SolidColorBrush(Color.FromRgb(136, 136, byte.MaxValue));
		Rare = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, 119));
		Unique = new SolidColorBrush(Color.FromRgb(175, 96, 37));
		Currency = new SolidColorBrush(Color.FromRgb(170, 158, 130));
		Gem = new SolidColorBrush(Color.FromRgb(27, 162, 155));
		Quest = new SolidColorBrush(Color.FromRgb(74, 230, 58));
		Card = new SolidColorBrush(Color.FromRgb(170, 230, 230));
		Prophecy = new SolidColorBrush(Color.FromRgb(181, 75, byte.MaxValue));
	}
}
