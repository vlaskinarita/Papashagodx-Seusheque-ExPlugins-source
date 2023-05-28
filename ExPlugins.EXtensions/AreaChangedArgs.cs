using System;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;

namespace ExPlugins.EXtensions;

public class AreaChangedArgs : EventArgs
{
	[CompilerGenerated]
	private readonly uint uint_0;

	[CompilerGenerated]
	private readonly uint uint_1;

	[CompilerGenerated]
	private readonly DatWorldAreaWrapper datWorldAreaWrapper_0;

	[CompilerGenerated]
	private readonly DatWorldAreaWrapper datWorldAreaWrapper_1;

	public uint OldHash
	{
		[CompilerGenerated]
		get
		{
			return uint_0;
		}
	}

	public uint NewHash
	{
		[CompilerGenerated]
		get
		{
			return uint_1;
		}
	}

	public DatWorldAreaWrapper OldArea
	{
		[CompilerGenerated]
		get
		{
			return datWorldAreaWrapper_0;
		}
	}

	public DatWorldAreaWrapper NewArea
	{
		[CompilerGenerated]
		get
		{
			return datWorldAreaWrapper_1;
		}
	}

	public AreaChangedArgs(uint oldHash, uint newHash, DatWorldAreaWrapper oldArea, DatWorldAreaWrapper newArea)
	{
		uint_0 = oldHash;
		uint_1 = newHash;
		datWorldAreaWrapper_0 = oldArea;
		datWorldAreaWrapper_1 = newArea;
	}

	public AreaChangedArgs(Message message)
	{
		uint_0 = message.GetInput<uint>(0);
		uint_1 = message.GetInput<uint>(1);
		datWorldAreaWrapper_0 = message.GetInput<DatWorldAreaWrapper>(2);
		datWorldAreaWrapper_1 = message.GetInput<DatWorldAreaWrapper>(3);
	}
}
