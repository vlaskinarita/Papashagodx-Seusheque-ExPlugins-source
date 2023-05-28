using System;
using System.Diagnostics;

namespace ExPlugins.EXtensions;

public class Interval
{
	private readonly int int_0;

	private readonly Stopwatch stopwatch_0;

	public bool Elapsed
	{
		get
		{
			if (stopwatch_0.ElapsedMilliseconds >= int_0)
			{
				stopwatch_0.Restart();
				return true;
			}
			return false;
		}
	}

	public Interval(int milliseconds)
	{
		stopwatch_0 = Stopwatch.StartNew();
		int_0 = milliseconds;
	}

	public Interval(TimeSpan timespan)
		: this((int)timespan.TotalMilliseconds)
	{
	}
}
