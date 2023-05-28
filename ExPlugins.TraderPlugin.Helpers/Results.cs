namespace ExPlugins.TraderPlugin.Helpers;

public static class Results
{
	public enum ClearCursorResults
	{
		None,
		InventoryNotOpened,
		NoSpaceInInventory,
		MaxTriesReached
	}

	public enum FastGoToHideoutResult
	{
		None,
		NotInTown,
		NotInGame,
		NoHideout,
		TimeOut
	}

	public enum FindItemInTabResult
	{
		None,
		GuiNotOpened,
		SwitchToTabFailed,
		GoToFirstTabFailed,
		GoToLastTabFailed,
		ItemFoundInTab,
		ItemNotFoundInTab
	}

	public enum NotificationResult
	{
		None,
		ApiKeyError,
		TokenError,
		CredentialsError,
		WebRequestError,
		Bullshit
	}

	public enum OpenStashError
	{
		None,
		CouldNotMoveToStash,
		NoStash,
		InteractFailed,
		StashPanelDidNotOpen
	}

	public enum OpenWaypointError
	{
		None,
		CouldNotMoveToWaypoint,
		NoWaypoint,
		InteractFailed,
		WorldPanelDidNotOpen
	}

	public enum TakeAreaTransitionError
	{
		None,
		NoAreaTransitions,
		InteractFailed,
		InstanceManagerDidNotOpen,
		JoinNewFailed,
		WaitForAreaChangeFailed,
		TooManyInstances
	}

	public enum TalkToNpcError
	{
		None,
		CouldNotMoveToNpc,
		NoNpc,
		InteractFailed,
		NpcDialogPanelDidNotOpen
	}
}
