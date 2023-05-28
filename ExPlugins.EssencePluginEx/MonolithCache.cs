using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.EssencePluginEx;

public class MonolithCache
{
	public static int ValidateDistance;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private readonly int int_0;

	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly Vector2i vector2i_1;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private readonly string string_1;

	[CompilerGenerated]
	private readonly List<DatBaseItemTypeWrapper> list_0;

	[CompilerGenerated]
	private bool? nullable_0;

	[CompilerGenerated]
	private bool bool_1 = false;

	public bool IsValid
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		private set
		{
			bool_0 = value;
		}
	}

	public int Id
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
	}

	public Vector2i Position
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
	}

	public Vector2i WalkablePosition
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_1;
		}
	}

	public string MonsterName
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public string MonsterMetadata
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
	}

	public List<DatBaseItemTypeWrapper> Essences
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
	}

	public Monolith NetworkObject => ObjectManager.GetObjectById<Monolith>((long)Id);

	public bool? Activate
	{
		[CompilerGenerated]
		get
		{
			return nullable_0;
		}
		[CompilerGenerated]
		set
		{
			nullable_0 = value;
		}
	}

	public bool Corrupt
	{
		[CompilerGenerated]
		get
		{
			return bool_1;
		}
		[CompilerGenerated]
		set
		{
			bool_1 = value;
		}
	}

	public MonolithCache(Monolith monolith)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		int_0 = ((NetworkObject)monolith).Id;
		vector2i_0 = ((NetworkObject)monolith).Position;
		vector2i_1 = ExilePather.FastWalkablePositionFor((NetworkObject)(object)monolith, 30, true);
		string_0 = ((NetworkObject)monolith).Name;
		string_1 = monolith.MonsterTypeMetadata;
		list_0 = monolith.EssenceBaseItemTypes;
		IsValid = true;
		GlobalLog.Info(string.Format("[MonolithCache] {0} {1} {2} {3} {4}", Id, WalkablePosition, MonsterName, MonsterMetadata, string.Join(", ", Essences.Select((DatBaseItemTypeWrapper e) => e.Metadata))));
	}

	public void Update(Monolith monolith)
	{
	}

	public void Validate()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!IsValid)
		{
			return;
		}
		Vector2i myPosition = LokiPoe.MyPosition;
		if (((Vector2i)(ref myPosition)).Distance(Position) <= ValidateDistance)
		{
			Monolith networkObject = NetworkObject;
			if ((NetworkObject)(object)networkObject == (NetworkObject)null)
			{
				GlobalLog.Info($"[MonolithCache::Validate] The Monolith [{Id}] is no longer valid because it does not exist.");
				IsValid = false;
			}
			else if (!((NetworkObject)networkObject).IsTargetable)
			{
				GlobalLog.Info($"[MonolithCache::Validate] The Monolith [{Id}] is no longer valid because it is not targetable.");
				IsValid = false;
			}
		}
	}

	static MonolithCache()
	{
		ValidateDistance = 75;
	}
}
