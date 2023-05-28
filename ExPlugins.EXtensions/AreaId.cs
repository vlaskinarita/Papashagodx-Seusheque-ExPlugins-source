using System;

namespace ExPlugins.EXtensions;

[AttributeUsage(AttributeTargets.Field)]
public class AreaId : Attribute
{
	public readonly string Id;

	public AreaId(string id)
	{
		Id = id;
	}
}
