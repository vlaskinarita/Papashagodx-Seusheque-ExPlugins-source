using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Bot;
using ExPlugins.EXtensions.CachedObjects;

namespace ExPlugins.EXtensions;

public class ItemsSoldArgs : EventArgs
{
	[CompilerGenerated]
	private readonly List<CachedItem> list_0;

	[CompilerGenerated]
	private readonly List<CachedItem> list_1;

	public List<CachedItem> SoldItems
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
	}

	public List<CachedItem> GainedItems
	{
		[CompilerGenerated]
		get
		{
			return list_1;
		}
	}

	public ItemsSoldArgs(List<CachedItem> soldItems, List<CachedItem> gainedItems)
	{
		list_0 = soldItems;
		list_1 = gainedItems;
	}

	public ItemsSoldArgs(Message message)
	{
		list_0 = message.GetInput<List<CachedItem>>(0);
		list_1 = message.GetInput<List<CachedItem>>(1);
	}
}
