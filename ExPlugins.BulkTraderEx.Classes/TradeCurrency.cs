using System.Runtime.CompilerServices;
using ExPlugins.EXtensions;

namespace ExPlugins.BulkTraderEx.Classes;

public class TradeCurrency
{
	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private string string_3;

	[CompilerGenerated]
	private string string_4;

	[CompilerGenerated]
	private double double_0;

	[CompilerGenerated]
	private string string_5;

	[CompilerGenerated]
	private double double_1;

	[CompilerGenerated]
	private int gEyRquMkov;

	[CompilerGenerated]
	private WebHelper.BulkPrice bulkPrice_0;

	public string SellerIgn
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

	public string SellerAccount
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

	public string Sellcurrency
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

	public string Whisper
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

	public string WhisperToken
	{
		[CompilerGenerated]
		get
		{
			return string_4;
		}
		[CompilerGenerated]
		set
		{
			string_4 = value;
		}
	}

	public double MinSell
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

	public string Buycurrency
	{
		[CompilerGenerated]
		get
		{
			return string_5;
		}
		[CompilerGenerated]
		set
		{
			string_5 = value;
		}
	}

	public double MinBuy
	{
		[CompilerGenerated]
		get
		{
			return double_1;
		}
		[CompilerGenerated]
		set
		{
			double_1 = value;
		}
	}

	public int Stock
	{
		[CompilerGenerated]
		get
		{
			return gEyRquMkov;
		}
		[CompilerGenerated]
		set
		{
			gEyRquMkov = value;
		}
	}

	public WebHelper.BulkPrice Bulk
	{
		[CompilerGenerated]
		get
		{
			return bulkPrice_0;
		}
		[CompilerGenerated]
		set
		{
			bulkPrice_0 = value;
		}
	}

	public TradeCurrency()
	{
	}

	public TradeCurrency(string ign, string account, int stock, string sellcurrency, int minSell, string buycurrency, int minBuy, string whisper, string whisperToken, WebHelper.BulkPrice bulk)
	{
		SellerIgn = ign;
		SellerAccount = account;
		Stock = stock;
		Sellcurrency = sellcurrency;
		MinSell = minSell;
		Buycurrency = buycurrency;
		MinBuy = minBuy;
		Whisper = whisper;
		WhisperToken = whisperToken;
		Bulk = bulk;
	}
}
