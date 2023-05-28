using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions.Positions;

public class WalkablePosition : WorldPosition
{
	protected int Radius;

	protected int Step;

	[CompilerGenerated]
	private readonly string string_0;

	[CompilerGenerated]
	private bool bool_0;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
	}

	public virtual bool Initialized
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		set
		{
			bool_0 = value;
		}
	}

	public WalkablePosition(string name, Vector2i vector, int step = 10, int radius = 30)
		: base(vector)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		string text = name;
		if (name.Contains("<rgb("))
		{
			text = name.Substring(name.IndexOf(">", StringComparison.Ordinal) + 1).Replace("{", "").Replace("}", "");
		}
		string_0 = text;
		Step = step;
		Radius = radius;
	}

	public WalkablePosition(string name, int x, int y, int step = 10, int radius = 30)
		: base(x, y)
	{
		string_0 = name;
		Step = step;
		Radius = radius;
	}

	public void Come()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (!Initialized)
		{
			HardInitialize();
		}
		Move.TowardsWalkable(Vector, Name);
	}

	public async Task ComeAtOnce(int distance = 20)
	{
		if (!Initialized)
		{
			HardInitialize();
		}
		await Move.AtOnce(Vector, Name, distance);
	}

	public bool TryCome()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!Initialized && !Initialize())
		{
			return false;
		}
		return Move.Towards(Vector, Name);
	}

	public async Task<bool> TryComeAtOnce(int distance = 20)
	{
		if (!Initialized && !Initialize())
		{
			return false;
		}
		await Move.AtOnce(Vector, Name, distance);
		return true;
	}

	public virtual bool Initialize()
	{
		if (FindWalkable())
		{
			Initialized = true;
			return true;
		}
		GlobalLog.Debug($"[WalkablePosition] Fail to find any walkable position for {this}");
		return false;
	}

	protected virtual void HardInitialize()
	{
		if (FindWalkable())
		{
			Initialized = true;
			return;
		}
		GlobalLog.Error($"[WalkablePosition] Fail to find any walkable position for {this}");
		DatWorldAreaWrapper currentArea = World.CurrentArea;
		Travel.RequestNewInstance(currentArea);
	}

	protected bool FindWalkable()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		if (!base.PathExists)
		{
			GlobalLog.Debug($"[WalkablePosition] {this} is unwalkable.");
			WorldPosition worldPosition = WorldPosition.FindPositionForMove(this, Step, Radius);
			if (!(worldPosition == null))
			{
				GlobalLog.Debug($"[WalkablePosition] Walkable position has been found at {worldPosition.AsVector} ({((Vector2i)(ref Vector)).Distance((Vector2i)worldPosition)} away from original position).");
				Vector = worldPosition;
				return true;
			}
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return $"[{Name}] at {Vector} (distance: {base.Distance})";
	}
}
