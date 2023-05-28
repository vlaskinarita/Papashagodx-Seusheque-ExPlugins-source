using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;

namespace ExPlugins.EXtensions.Positions;

public static class StaticPositions
{
	public static readonly WalkablePosition StashPosAct1;

	public static readonly WalkablePosition StashPosAct2;

	public static readonly WalkablePosition StashPosAct3;

	public static readonly WalkablePosition StashPosAct4;

	public static readonly WalkablePosition StashPosAct5;

	public static readonly WalkablePosition StashPosAct6;

	public static readonly WalkablePosition StashPosAct7;

	public static readonly WalkablePosition StashPosAct8;

	public static readonly WalkablePosition StashPosAct9;

	public static readonly WalkablePosition StashPosAct10;

	public static readonly WalkablePosition StashPosAct11;

	public static readonly WalkablePosition walkablePosition_0;

	public static readonly WalkablePosition WaypointPosAct1;

	public static readonly WalkablePosition WaypointPosAct2;

	public static readonly WalkablePosition WaypointPosAct3;

	public static readonly WalkablePosition WaypointPosAct4;

	public static readonly WalkablePosition WaypointPosAct5;

	public static readonly WalkablePosition WaypointPosAct6;

	public static readonly WalkablePosition WaypointPosAct7;

	public static readonly WalkablePosition WaypointPosAct8;

	public static readonly WalkablePosition WaypointPosAct9;

	public static readonly WalkablePosition WaypointPosAct10;

	public static readonly WalkablePosition WaypointPosAct11;

	public static readonly WalkablePosition WaypointPosAct11Shores;

	public static readonly WalkablePosition CommonPortalSpotAct1;

	public static readonly WalkablePosition CommonPortalSpotAct2;

	public static readonly WalkablePosition CommonPortalSpotAct3;

	public static readonly WalkablePosition CommonPortalSpotAct4;

	public static readonly WalkablePosition CommonPortalSpotAct5;

	public static readonly WalkablePosition CommonPortalSpotAct6;

	public static readonly WalkablePosition CommonPortalSpotAct7;

	public static readonly WalkablePosition CommonPortalSpotAct8;

	public static readonly WalkablePosition CommonPortalSpotAct9;

	public static readonly WalkablePosition CommonPortalSpotAct10;

	public static readonly WalkablePosition CommonPortalSpotAct11;

	public static WalkablePosition GetStashPosByAct()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		switch (World.CurrentArea.Act)
		{
		default:
			GlobalLog.Error($"[GetStashPosByAct] Unknown act: {World.CurrentArea.Act}.");
			BotManager.Stop(new StopReasonData("unknown_act", $"Unknown act: {World.CurrentArea.Act}.", (object)null), false);
			return null;
		case 1:
			return StashPosAct1;
		case 2:
			return StashPosAct2;
		case 3:
			return StashPosAct3;
		case 4:
			return StashPosAct4;
		case 5:
			return StashPosAct5;
		case 6:
			return StashPosAct6;
		case 7:
			return StashPosAct7;
		case 8:
			return StashPosAct8;
		case 9:
			return StashPosAct9;
		case 10:
			return StashPosAct10;
		case 11:
			if (World.CurrentArea.Name != "Karui Shores")
			{
				return StashPosAct11;
			}
			return walkablePosition_0;
		}
	}

	public static WalkablePosition GetWaypointPosByAct()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		switch (World.CurrentArea.Act)
		{
		default:
			GlobalLog.Error($"[GetWaypointPosByAct] Unknown act: {World.CurrentArea.Act}.");
			BotManager.Stop(new StopReasonData("unknown_act", $"Unknown act: {World.CurrentArea.Act}.", (object)null), false);
			return null;
		case 1:
			return WaypointPosAct1;
		case 2:
			return WaypointPosAct2;
		case 3:
			return WaypointPosAct3;
		case 4:
			return WaypointPosAct4;
		case 5:
			return WaypointPosAct5;
		case 6:
			return WaypointPosAct6;
		case 7:
			return WaypointPosAct7;
		case 8:
			return WaypointPosAct8;
		case 9:
			return WaypointPosAct9;
		case 10:
			return WaypointPosAct10;
		case 11:
			if (!(World.CurrentArea.Name != "Karui Shores"))
			{
				return WaypointPosAct11Shores;
			}
			return WaypointPosAct11;
		}
	}

	public static WalkablePosition GetCommonPortalSpotByAct()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		switch (World.CurrentArea.Act)
		{
		default:
			GlobalLog.Error($"[GetCommonPortalSpotByAct] Unknown act: {World.CurrentArea.Act}.");
			BotManager.Stop(new StopReasonData("unknown_act", $"Unknown act: {World.CurrentArea.Act}.", (object)null), false);
			return null;
		case 1:
			return CommonPortalSpotAct1;
		case 2:
			return CommonPortalSpotAct2;
		case 3:
			return CommonPortalSpotAct3;
		case 4:
			return CommonPortalSpotAct4;
		case 5:
			return CommonPortalSpotAct5;
		case 6:
			return CommonPortalSpotAct6;
		case 7:
			return CommonPortalSpotAct7;
		case 8:
			return CommonPortalSpotAct8;
		case 9:
			return CommonPortalSpotAct9;
		case 10:
			return CommonPortalSpotAct10;
		case 11:
			return CommonPortalSpotAct11;
		}
	}

	static StaticPositions()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		StashPosAct1 = new WalkablePosition("Stash", new Vector2i(313, 270));
		StashPosAct2 = new WalkablePosition("Stash", new Vector2i(194, 286));
		StashPosAct3 = new WalkablePosition("Stash", new Vector2i(297, 417));
		StashPosAct4 = new WalkablePosition("Stash", new Vector2i(203, 515));
		StashPosAct5 = new WalkablePosition("Stash", new Vector2i(326, 341));
		StashPosAct6 = new WalkablePosition("Stash", new Vector2i(313, 404));
		StashPosAct7 = new WalkablePosition("Stash", new Vector2i(588, 615));
		StashPosAct8 = StashPosAct3;
		StashPosAct9 = StashPosAct4;
		StashPosAct10 = new WalkablePosition("Stash", new Vector2i(525, 281));
		StashPosAct11 = new WalkablePosition("Stash", new Vector2i(593, 657));
		walkablePosition_0 = new WalkablePosition("Stash", new Vector2i(827, 829));
		WaypointPosAct1 = new WalkablePosition("Waypoint", new Vector2i(256, 169));
		WaypointPosAct2 = new WalkablePosition("Waypoint", new Vector2i(188, 216));
		WaypointPosAct3 = new WalkablePosition("Waypoint", new Vector2i(308, 340));
		WaypointPosAct4 = new WalkablePosition("Waypoint", new Vector2i(274, 482));
		WaypointPosAct5 = new WalkablePosition("Waypoint", new Vector2i(313, 363));
		WaypointPosAct6 = new WalkablePosition("Waypoint", new Vector2i(272, 320));
		WaypointPosAct7 = new WalkablePosition("Waypoint", new Vector2i(544, 529));
		WaypointPosAct8 = WaypointPosAct3;
		WaypointPosAct9 = WaypointPosAct4;
		WaypointPosAct10 = new WalkablePosition("Waypoint", new Vector2i(586, 313));
		WaypointPosAct11 = new WalkablePosition("Waypoint", new Vector2i(591, 689));
		WaypointPosAct11Shores = new WalkablePosition("Waypoint", new Vector2i(823, 831));
		CommonPortalSpotAct1 = new WalkablePosition("common portal spot", new Vector2i(200, 235));
		CommonPortalSpotAct2 = new WalkablePosition("common portal spot", new Vector2i(220, 268));
		CommonPortalSpotAct3 = new WalkablePosition("common portal spot", new Vector2i(331, 329));
		CommonPortalSpotAct4 = new WalkablePosition("common portal spot", new Vector2i(272, 510));
		CommonPortalSpotAct5 = new WalkablePosition("common portal spot", new Vector2i(358, 284));
		CommonPortalSpotAct6 = new WalkablePosition("common portal spot", new Vector2i(210, 385));
		CommonPortalSpotAct7 = new WalkablePosition("common portal spot", new Vector2i(533, 520));
		CommonPortalSpotAct8 = CommonPortalSpotAct3;
		CommonPortalSpotAct9 = CommonPortalSpotAct4;
		CommonPortalSpotAct10 = new WalkablePosition("common portal spot", new Vector2i(400, 285));
		CommonPortalSpotAct11 = new WalkablePosition("common portal spot", new Vector2i(560, 790));
	}
}
