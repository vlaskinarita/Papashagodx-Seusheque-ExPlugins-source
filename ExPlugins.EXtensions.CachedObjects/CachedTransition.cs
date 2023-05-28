using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CachedObjects;

public class CachedTransition : CachedObject
{
	[CompilerGenerated]
	private readonly TransitionType transitionType_0;

	[CompilerGenerated]
	private readonly DatWorldAreaWrapper datWorldAreaWrapper_0;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	public TransitionType Type
	{
		[CompilerGenerated]
		get
		{
			return transitionType_0;
		}
	}

	public DatWorldAreaWrapper Destination
	{
		[CompilerGenerated]
		get
		{
			return datWorldAreaWrapper_0;
		}
	}

	public bool Visited
	{
		[CompilerGenerated]
		get
		{
			return bool_2;
		}
		[CompilerGenerated]
		set
		{
			bool_2 = value;
		}
	}

	public bool LeadsBack
	{
		[CompilerGenerated]
		get
		{
			return bool_3;
		}
		[CompilerGenerated]
		set
		{
			bool_3 = value;
		}
	}

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string Metadata
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public new AreaTransition Object
	{
		get
		{
			NetworkObject @object = GetObject();
			return (AreaTransition)(object)((@object is AreaTransition) ? @object : null);
		}
	}

	public CachedTransition(int id, WalkablePosition position, TransitionType type, DatWorldAreaWrapper destination)
		: base(id, position)
	{
		transitionType_0 = type;
		datWorldAreaWrapper_0 = destination;
		AreaTransition @object = Object;
		string_0 = ((@object == null) ? null : ((NetworkObject)@object).Name);
		AreaTransition object2 = Object;
		string_1 = ((object2 != null) ? ((NetworkObject)object2).Metadata : null);
	}
}
