using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.NativeWrappers;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedHarvestIrrigator : CachedObject
{
	public string ColorString;

	public bool Inactive;

	public bool Visited;

	public List<Tuple<int, string>> MobList;

	public string Name;

	public new HarvestIrrigator Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (HarvestIrrigator)(object)((@object is HarvestIrrigator) ? @object : null);
		}
	}

	public CachedHarvestIrrigator(int id, WalkablePosition position, List<Tuple<int, string>> mobList)
		: base(id, position)
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		Name = ((NetworkObject)Object).Name;
		MinimapIconWrapper obj = InstanceInfo.MinimapIcons.OrderBy(delegate(MinimapIconWrapper i)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Vector2i lastSeenPosition = i.LastSeenPosition;
			return ((Vector2i)(ref lastSeenPosition)).Distance(((NetworkObject)Object).Position);
		}).FirstOrDefault();
		DatMinimapIconWrapper val = ((obj != null) ? obj.MinimapIcon : null);
		if (val != null)
		{
			Regex regex = new Regex("\r\n                (?<=[A-Z])(?=[A-Z][a-z]) |\r\n                 (?<=[^A-Z])(?=[A-Z]) |\r\n                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
			string name = regex.Replace(val.Name, " ");
			Name = name;
			if (val.Name.ContainsIgnorecase("yellow"))
			{
				ColorString = "Yellow";
			}
			else if (val.Name.ContainsIgnorecase("purple"))
			{
				ColorString = "Purple";
			}
			else if (val.Name.ContainsIgnorecase("blue"))
			{
				ColorString = "Blue";
			}
			else if (val.Name.ContainsIgnorecase("neutral"))
			{
				ColorString = "Neutral";
			}
		}
		MobList = mobList;
		base.Position = new WalkablePosition(Name, position, 4, 10);
	}
}
