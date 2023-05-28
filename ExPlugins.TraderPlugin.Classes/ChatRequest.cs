using System.Diagnostics;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using ExPlugins.MapBotEx.Helpers;

namespace ExPlugins.TraderPlugin.Classes;

public class ChatRequest
{
	public enum TradePhase : ushort
	{
		RequestProcessed,
		ItemsFound,
		WasInvited,
		ItemTaken,
		TradeFinished
	}

	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private Vector2i vector2i_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private decimal decimal_0;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private string string_3;

	[CompilerGenerated]
	private int int_1;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private int int_2;

	[CompilerGenerated]
	private bool bool_0;

	[CompilerGenerated]
	private bool bool_1;

	[CompilerGenerated]
	private bool bool_2;

	[CompilerGenerated]
	private bool bool_3;

	[CompilerGenerated]
	private bool bool_4;

	[CompilerGenerated]
	private Stopwatch stopwatch_0 = new Stopwatch();

	[CompilerGenerated]
	private TradePhase tradePhase_0;

	[CompilerGenerated]
	private InfluenceHelper.InfluenceType? nullable_0;

	public string Name
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

	public Vector2i ItemPos
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

	public string ItemName
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

	public decimal Price
	{
		[CompilerGenerated]
		get
		{
			return decimal_0;
		}
		[CompilerGenerated]
		set
		{
			decimal_0 = value;
		}
	}

	public int SellAmount
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

	public string Currency
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

	public string TabName
	{
		[CompilerGenerated]
		get
		{
			return string_3;
		}
		[CompilerGenerated]
		set
		{
			string_3 = value;
		}
	}

	public int InviteCount
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

	public double LastInviteTime
	{
		[CompilerGenerated]
		get
		{
			return double_0;
		}
		[CompilerGenerated]
		set
		{
			double_0 = value;
		}
	}

	public int TradeCount
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

	public bool WasServed
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

	public bool FoundItem
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

	public bool WasJsoned
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

	public bool WasInvitedInHo
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

	public bool IsSingleItemPrice
	{
		[CompilerGenerated]
		get
		{
			return bool_4;
		}
		[CompilerGenerated]
		set
		{
			bool_4 = value;
		}
	}

	public Stopwatch responseTimer
	{
		[CompilerGenerated]
		get
		{
			return stopwatch_0;
		}
		[CompilerGenerated]
		set
		{
			stopwatch_0 = value;
		}
	}

	public TradePhase CurrentTradePhase
	{
		[CompilerGenerated]
		get
		{
			return tradePhase_0;
		}
		[CompilerGenerated]
		set
		{
			tradePhase_0 = value;
		}
	}

	public InfluenceHelper.InfluenceType? InfluenceType
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

	public static bool operator ==(ChatRequest a, ChatRequest b)
	{
		return a?.Name == b?.Name;
	}

	public static bool operator !=(ChatRequest a, ChatRequest b)
	{
		return a?.Name != b?.Name;
	}
}
