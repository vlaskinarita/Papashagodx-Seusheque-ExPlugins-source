using System;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;

namespace ExPlugins.EXtensions;

public class AreaInfo : IEquatable<AreaInfo>
{
	public readonly string Id;

	public readonly string Name;

	public bool IsCurrentArea => LocalData.WorldArea.Id == Id;

	public bool IsWaypointOpened => InstanceInfo.AvailableWaypoints.ContainsKey(Id);

	public AreaInfo(string id, string name)
	{
		Id = id;
		Name = name;
	}

	public bool Equals(AreaInfo other)
	{
		return this == other;
	}

	public static implicit operator AreaInfo(DatWorldAreaWrapper area)
	{
		return new AreaInfo(area.Id, area.Name);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as AreaInfo);
	}

	public static bool operator ==(AreaInfo left, AreaInfo right)
	{
		if ((object)left != right)
		{
			if ((object)left == null || (object)right == null)
			{
				return false;
			}
			return left.Id == right.Id;
		}
		return true;
	}

	public static bool operator !=(AreaInfo left, AreaInfo right)
	{
		return !(left == right);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public override string ToString()
	{
		return "\"" + Name + "\" (" + Id + ")";
	}
}
