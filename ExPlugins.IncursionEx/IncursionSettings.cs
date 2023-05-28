using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using Newtonsoft.Json;

namespace ExPlugins.IncursionEx;

public class IncursionSettings : JsonSettings
{
	private static IncursionSettings incursionSettings_0;

	[JsonIgnore]
	public static readonly PriorityAction[] PriorityActions;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private List<RoomEntry> list_0 = new List<RoomEntry>();

	public static IncursionSettings Instance => incursionSettings_0 ?? (incursionSettings_0 = new IncursionSettings());

	public bool PortalBeforeIncursion
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

	public bool LeaveAfterIncursion
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

	[JsonIgnore]
	public bool AlvaChecked
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

	public List<RoomEntry> IncursionRooms
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
		[CompilerGenerated]
		set
		{
			list_0 = value;
		}
	}

	private IncursionSettings()
		: base(JsonSettings.GetSettingsFilePath(new string[2]
		{
			Configuration.Instance.Name,
			"IncursionEx.json"
		}))
	{
		InitRoomList();
		IncursionRooms.Sort((RoomEntry r1, RoomEntry r2) => string.CompareOrdinal(r1.Name, r2.Name));
	}

	private static List<RoomEntry> GetDefaultRoomList()
	{
		return new List<RoomEntry>
		{
			new RoomEntry("Poison Garden", 56998, 35549, 7899),
			new RoomEntry("Sacrificial Chamber", 50849, 53673, 63046),
			new RoomEntry("Tempest Generator", 37180, 6913, 5133),
			new RoomEntry("Trap Workshop", 4946, 14070, 11681),
			new RoomEntry("Surveyor's Study", 54001, 61696, 54094),
			new RoomEntry("Royal Meeting Room", 42889, 12574, 63579),
			new RoomEntry("Storage Room", 25231, 63562, 49758),
			new RoomEntry("Corruption Chamber", 7940, 961, 57960),
			new RoomEntry("Explosives Room", 51256, 12263, 62364),
			new RoomEntry("Armourer's Workshop", 47840, 33086, 51778),
			new RoomEntry("Sparring Room", 20517, 46313, 57246),
			new RoomEntry("Guardhouse", 47693, 23727, 39495),
			new RoomEntry("Splinter Research Lab", 36698, 48392, 10187),
			new RoomEntry("Gemcutter's Workshop", 29190, 33628, 45452),
			new RoomEntry("Vault", 36945, 32923, 1833),
			new RoomEntry("Jeweller's Workshop", 56310, 57786, 6047),
			new RoomEntry("Workshop", 27815, 11869, 55471),
			new RoomEntry("Shrine of Empowerment", 1623, 47887, 36762),
			new RoomEntry("Pools of Restoration", 29045, 46208, 16697),
			new RoomEntry("Hatchery", 28140, 27848, 59890),
			new RoomEntry("Flame Workshop", 22115, 565, 19827),
			new RoomEntry("Lightning Workshop", 62908, 42207, 65226)
		};
	}

	private void InitRoomList()
	{
		if (IncursionRooms.Count != 0)
		{
			List<RoomEntry> defaultRoomList = GetDefaultRoomList();
			foreach (RoomEntry roomEntry_0 in defaultRoomList)
			{
				RoomEntry roomEntry = IncursionRooms.Find((RoomEntry r) => r.Name == roomEntry_0.Name);
				if (roomEntry != null)
				{
					roomEntry_0.PriorityAction = roomEntry.PriorityAction;
					roomEntry_0.NoChange = roomEntry.NoChange;
					roomEntry_0.NoUpgrade = roomEntry.NoUpgrade;
				}
			}
			IncursionRooms = defaultRoomList;
		}
		else
		{
			IncursionRooms = GetDefaultRoomList();
		}
	}

	static IncursionSettings()
	{
		PriorityActions = new PriorityAction[3]
		{
			PriorityAction.Doors,
			PriorityAction.Changing,
			PriorityAction.Upgrading
		};
	}
}
