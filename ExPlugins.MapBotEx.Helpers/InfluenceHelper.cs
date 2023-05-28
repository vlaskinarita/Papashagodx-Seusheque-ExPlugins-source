using System;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;

namespace ExPlugins.MapBotEx.Helpers;

public class InfluenceHelper
{
	public enum InfluenceType
	{
		None,
		Enslaver,
		Eradicator,
		Constrictor,
		Purifier,
		Baran,
		Veritania,
		AlHezmin,
		Drox,
		Chimera,
		Hydra,
		Phoenix,
		Minotaur
	}

	public static string GetInfluenceName(CachedMapItem map)
	{
		InfluenceType? influence = GetInfluence(map);
		string result = map.Name;
		switch (influence)
		{
		default:
			throw new ArgumentOutOfRangeException();
		case InfluenceType.Enslaver:
			result = "The Enslaver";
			break;
		case InfluenceType.Eradicator:
			result = "The Eradicator";
			break;
		case InfluenceType.Constrictor:
			result = "The Constrictor";
			break;
		case InfluenceType.Purifier:
			result = "The Purifier";
			break;
		case InfluenceType.Baran:
			result = "Baran, The Crusader";
			break;
		case InfluenceType.Veritania:
			result = "Veritania, The Redeemer";
			break;
		case InfluenceType.AlHezmin:
			result = "Al-Hezmin, The Hunter";
			break;
		case InfluenceType.Drox:
			result = "Drox, The Warlord";
			break;
		case null:
		case InfluenceType.None:
		case InfluenceType.Chimera:
		case InfluenceType.Hydra:
		case InfluenceType.Phoenix:
		case InfluenceType.Minotaur:
			break;
		}
		return result;
	}

	public static string GetInfluenceName(Item map)
	{
		InfluenceType? influence = GetInfluence(map);
		string result = map.Name;
		switch (influence)
		{
		default:
			throw new ArgumentOutOfRangeException();
		case InfluenceType.Enslaver:
			result = "The Enslaver";
			break;
		case InfluenceType.Eradicator:
			result = "The Eradicator";
			break;
		case InfluenceType.Constrictor:
			result = "The Constrictor";
			break;
		case InfluenceType.Purifier:
			result = "The Purifier";
			break;
		case InfluenceType.Baran:
			result = "Baran, The Crusader";
			break;
		case InfluenceType.Veritania:
			result = "Veritania, The Redeemer";
			break;
		case InfluenceType.AlHezmin:
			result = "Al-Hezmin, The Hunter";
			break;
		case InfluenceType.Drox:
			result = "Drox, The Warlord";
			break;
		case null:
		case InfluenceType.None:
		case InfluenceType.Chimera:
		case InfluenceType.Hydra:
		case InfluenceType.Phoenix:
		case InfluenceType.Minotaur:
			break;
		}
		return result;
	}

	public static InfluenceType? GetInfluence(CachedMapItem map)
	{
		map.Stats.TryGetValue((StatTypeGGG)13845, out var value);
		map.Stats.TryGetValue((StatTypeGGG)6548, out var value2);
		if (map.Stats.ContainsKey((StatTypeGGG)13845))
		{
			switch (value)
			{
			case 1:
				return InfluenceType.Baran;
			case 2:
				return InfluenceType.Veritania;
			case 3:
				return InfluenceType.AlHezmin;
			case 4:
				return InfluenceType.Drox;
			}
		}
		if (map.Stats.ContainsKey((StatTypeGGG)6548))
		{
			switch (value2)
			{
			case 1:
				return InfluenceType.Enslaver;
			case 2:
				return InfluenceType.Eradicator;
			case 3:
				return InfluenceType.Constrictor;
			case 4:
				return InfluenceType.Purifier;
			}
		}
		if (map.Stats.ContainsKey((StatTypeGGG)6827) && map.Stats[(StatTypeGGG)6827] == 1)
		{
			if (map.Name.Contains("Chimera"))
			{
				return InfluenceType.Chimera;
			}
			if (map.Name.Contains("Hydra"))
			{
				return InfluenceType.Hydra;
			}
			if (map.Name.Contains("Phoenix"))
			{
				return InfluenceType.Phoenix;
			}
			if (map.Name.Contains("Minotaur"))
			{
				return InfluenceType.Minotaur;
			}
		}
		return InfluenceType.None;
	}

	public static InfluenceType? GetInfluence(Item map)
	{
		map.Stats.TryGetValue((StatTypeGGG)13845, out var value);
		map.Stats.TryGetValue((StatTypeGGG)6548, out var value2);
		if (map.Stats.ContainsKey((StatTypeGGG)13845))
		{
			switch (value)
			{
			case 1:
				return InfluenceType.Baran;
			case 2:
				return InfluenceType.Veritania;
			case 3:
				return InfluenceType.AlHezmin;
			case 4:
				return InfluenceType.Drox;
			}
		}
		if (map.Stats.ContainsKey((StatTypeGGG)6548))
		{
			switch (value2)
			{
			case 1:
				return InfluenceType.Enslaver;
			case 2:
				return InfluenceType.Eradicator;
			case 3:
				return InfluenceType.Constrictor;
			case 4:
				return InfluenceType.Purifier;
			}
		}
		if (map.Stats.ContainsKey((StatTypeGGG)6827) && map.Stats[(StatTypeGGG)6827] == 1)
		{
			if (map.Name.Contains("Chimera"))
			{
				return InfluenceType.Chimera;
			}
			if (map.Name.Contains("Hydra"))
			{
				return InfluenceType.Hydra;
			}
			if (map.Name.Contains("Phoenix"))
			{
				return InfluenceType.Phoenix;
			}
			if (map.Name.Contains("Minotaur"))
			{
				return InfluenceType.Minotaur;
			}
		}
		return InfluenceType.None;
	}
}
