using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.MapBotEx.ExtraContent.Breach;

public class BreachCache
{
	public static int ValidateDistance;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private readonly int int_0;

	[CompilerGenerated]
	private readonly Vector2i vector2i_0;

	[CompilerGenerated]
	private readonly WalkablePosition walkablePosition_0;

	[CompilerGenerated]
	private bool? nullable_0;

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

	public WalkablePosition WalkablePosition
	{
		[CompilerGenerated]
		get
		{
			return walkablePosition_0;
		}
	}

	public Breach NetworkObject => ObjectManager.GetObjectById<Breach>((long)Id);

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

	public BreachCache(NetworkObject breach)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		int_0 = breach.Id;
		vector2i_0 = breach.Position;
		walkablePosition_0 = breach.WalkablePosition();
		IsValid = true;
		GlobalLog.Info($"[BreachCache] {Id} {WalkablePosition}");
	}

	public void Update(Breach breach)
	{
	}

	public void Validate()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (!IsValid)
		{
			return;
		}
		Vector2i myPosition = LokiPoe.MyPosition;
		if (((Vector2i)(ref myPosition)).Distance(Position) <= ValidateDistance)
		{
			Breach networkObject = NetworkObject;
			if ((NetworkObject)(object)networkObject == (NetworkObject)null)
			{
				GlobalLog.Info($"[BreachCache::Validate] The Breach [{Id}] is no longer valid because it does not exist.");
				IsValid = false;
			}
		}
	}

	static BreachCache()
	{
		ValidateDistance = 75;
	}
}
