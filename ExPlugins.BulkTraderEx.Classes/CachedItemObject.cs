using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.BulkTraderEx.Classes;

public class CachedItemObject
{
	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private Vector2i VvqQoyXcoR;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private Vector2i vector2i_0;

	[CompilerGenerated]
	private int int_2;

	public int ItemId
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
		[CompilerGenerated]
		set
		{
			int_0 = value;
		}
	}

	public string TabName
	{
		[CompilerGenerated]
		get
		{
			return string_0;
		}
		[CompilerGenerated]
		set
		{
			string_0 = value;
		}
	}

	public int MaxStackCount
	{
		[CompilerGenerated]
		get
		{
			return int_1;
		}
		[CompilerGenerated]
		set
		{
			int_1 = value;
		}
	}

	public bool IsInPremiumTab
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

	public string FullName
	{
		[CompilerGenerated]
		get
		{
			return string_1;
		}
		[CompilerGenerated]
		set
		{
			string_1 = value;
		}
	}

	public Vector2i LocationTopLeft
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return VvqQoyXcoR;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			VvqQoyXcoR = value;
		}
	}

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return string_2;
		}
		[CompilerGenerated]
		set
		{
			string_2 = value;
		}
	}

	public Vector2i Size
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vector2i_0;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			vector2i_0 = value;
		}
	}

	public int StackCount
	{
		[CompilerGenerated]
		get
		{
			return int_2;
		}
		[CompilerGenerated]
		set
		{
			int_2 = value;
		}
	}

	private InventoryControlWrapper Wrapper
	{
		get
		{
			if (!string.IsNullOrEmpty(TabName))
			{
				if (IsInPremiumTab)
				{
					return Inventories.GetControlWithCurrency(FullName);
				}
				return StashUi.InventoryControl;
			}
			return InventoryUi.InventoryControl_Main;
		}
	}

	private Item Item
	{
		get
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (!((RemoteMemoryObject)(object)Wrapper == (RemoteMemoryObject)null))
			{
				if (Wrapper.HasCurrencyTabOverride)
				{
					return Wrapper.CustomTabItem;
				}
				Item val = Wrapper.Inventory.GetItemAtLocation(LocationTopLeft) ?? Wrapper.Inventory.GetItemById(ItemId);
				if (!((RemoteMemoryObject)(object)val == (RemoteMemoryObject)null))
				{
					if (!val.FullName.Equals(FullName))
					{
						GlobalLog.Debug("[CachedItem] returned null on ret.FullName.Equals");
						return null;
					}
					return val;
				}
				GlobalLog.Debug("[CachedItem] returned null on Wrapper.Inventory.GetIte(...)");
				return null;
			}
			GlobalLog.Debug("[CachedItem] returned null on Wrapper == null");
			return null;
		}
	}

	public CachedItemObject(InventoryControlWrapper wrp, Item item, string tabName = "")
	{
		TabName = tabName;
		IsInPremiumTab = wrp.HasCustomTabOverride;
		Update(item);
	}

	public async Task<bool> GoTo()
	{
		if (!string.IsNullOrEmpty(TabName))
		{
			return await Inventories.OpenStashTab(TabName, "CachedItemObject");
		}
		if (InventoryUi.IsOpened)
		{
			return true;
		}
		return await Inventories.OpenInventory();
	}

	public async Task<InventoryControlWrapper> GetWrapper()
	{
		if (!(await GoTo()))
		{
			return null;
		}
		return Wrapper;
	}

	public async Task<Item> GetItem()
	{
		if (!(await GoTo()))
		{
			return null;
		}
		return Item;
	}

	private void Update(Item item)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		ItemId = item.LocalId;
		FullName = item.FullName;
		LocationTopLeft = item.LocationTopLeft;
		MaxStackCount = item.MaxStackCount;
		Name = item.Name;
		Size = item.Size;
		StackCount = item.StackCount;
	}
}
