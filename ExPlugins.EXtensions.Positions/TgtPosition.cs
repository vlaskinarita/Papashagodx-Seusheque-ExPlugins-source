using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions.Global;

namespace ExPlugins.EXtensions.Positions;

public class TgtPosition : WalkablePosition
{
	private static readonly Vector2i vector2i_0;

	private readonly bool bool_1;

	private readonly string string_1;

	private uint uint_0;

	private List<WorldPosition> list_0 = new List<WorldPosition>();

	public override bool Initialized => uint_0 == LocalData.AreaHash;

	public TgtPosition(string name, string tgtName, bool closest = false, int step = 10, int radius = 45)
		: base(name, vector2i_0, step, radius)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		string_1 = tgtName;
		bool_1 = closest;
	}

	public void ResetCurrentPosition()
	{
		if (Initialized)
		{
			if (!SetCurrentPosition())
			{
				GlobalLog.Error("[TgtPosition] No walkable position can be found.");
				DatWorldAreaWrapper currentArea = World.CurrentArea;
				Travel.RequestNewInstance(currentArea);
			}
		}
		else
		{
			HardInitialize();
		}
	}

	public void ProceedToNext()
	{
		if (list_0.Count <= 1)
		{
			GlobalLog.Error("[TgtPosition] Cannot proceed to next, current one is the last.");
			ErrorManager.ReportCriticalError();
			return;
		}
		WorldPosition worldPosition = list_0.OrderBy((WorldPosition p) => p.Distance).First();
		list_0.Remove(worldPosition);
		GlobalLog.Debug($"[TgtPosition] {worldPosition} has been removed.");
		if (!SetCurrentPosition())
		{
			GlobalLog.Error("[TgtPosition] No walkable position can be found.");
			DatWorldAreaWrapper currentArea = World.CurrentArea;
			Travel.RequestNewInstance(currentArea);
		}
	}

	public override bool Initialize()
	{
		if (!FindTgtPositions())
		{
			GlobalLog.Debug("[TgtPosition] Fail to find any \"" + string_1 + "\" tgt.");
			return false;
		}
		if (SetCurrentPosition())
		{
			uint_0 = LocalData.AreaHash;
			return true;
		}
		GlobalLog.Debug("[TgtPosition] No walkable position can be found.");
		return false;
	}

	protected override void HardInitialize()
	{
		if (FindTgtPositions())
		{
			if (!SetCurrentPosition())
			{
				GlobalLog.Error("[TgtPosition] No walkable position can be found.");
				DatWorldAreaWrapper currentArea = World.CurrentArea;
				Travel.RequestNewInstance(currentArea);
			}
			else
			{
				uint_0 = LocalData.AreaHash;
			}
		}
		else
		{
			GlobalLog.Error("[TgtPosition] Fail to find any \"" + string_1 + "\" tgt.");
			ErrorManager.ReportCriticalError();
		}
	}

	private bool FindTgtPositions()
	{
		list_0.Clear();
		if (!string_1.Contains('|'))
		{
			list_0.AddRange(Tgt.FindAll(string_1));
		}
		else
		{
			List<string> list = (from tgt in string_1.Split('|')
				select tgt.Trim()).ToList();
			foreach (string item in list)
			{
				list_0.AddRange(Tgt.FindAll(item));
			}
		}
		return list_0.Count > 0;
	}

	private bool SetCurrentPosition()
	{
		if ((!bool_1) ? FindDistantPosition() : FindClosestPosition())
		{
			GlobalLog.Warn($"[TgtPosition] Registering {this}");
			return true;
		}
		return false;
	}

	private bool FindDistantPosition()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		list_0 = list_0.OrderByDescending((WorldPosition p) => p.Distance).ToList();
		foreach (WorldPosition item in list_0)
		{
			Vector = item;
			if (FindWalkable())
			{
				return true;
			}
		}
		return false;
	}

	private bool FindClosestPosition()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		list_0 = list_0.OrderBy((WorldPosition p) => p.Distance).ToList();
		foreach (WorldPosition item in list_0)
		{
			Vector = item;
			if (FindWalkable())
			{
				return true;
			}
		}
		return false;
	}

	static TgtPosition()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		vector2i_0 = new Vector2i(int.MaxValue, int.MaxValue);
	}
}
