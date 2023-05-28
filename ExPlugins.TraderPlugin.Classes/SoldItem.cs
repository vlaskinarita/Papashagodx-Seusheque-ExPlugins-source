using System.Runtime.CompilerServices;

namespace ExPlugins.TraderPlugin.Classes;

public class SoldItem
{
	[CompilerGenerated]
	private string string_0;

	[CompilerGenerated]
	private string string_1;

	[CompilerGenerated]
	private string string_2;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private string string_3;

	public string Source
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

	public string Type
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

	public int Price
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

	public string Stats
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

	public SoldItem()
	{
	}

	public SoldItem(string source, string type, string name, int price, string stats)
	{
		Source = source;
		Type = type;
		Name = name;
		Price = price;
		Stats = stats;
	}

	public override string ToString()
	{
		return (!string.IsNullOrEmpty(Stats)) ? $"[SoldItem] {Name} ({Type}) sold for {Price}c. Stats: {Stats}" : $"[SoldItem] {Name} ({Type}) sold for {Price}c.";
	}
}
