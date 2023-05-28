using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;

namespace ExPlugins.EssencePluginEx;

public class MonolithData : AreaData
{
	private readonly Dictionary<int, MonolithCache> dictionary_0 = new Dictionary<int, MonolithCache>();

	private static readonly Interval interval_0;

	public IEnumerable<MonolithCache> Monoliths => dictionary_0.Select((KeyValuePair<int, MonolithCache> kvp) => kvp.Value).ToList();

	public MonolithData(uint hash)
		: base(hash)
	{
	}

	public override void Start(bool isActive)
	{
		foreach (KeyValuePair<int, MonolithCache> item in dictionary_0)
		{
			item.Value.Activate = null;
		}
	}

	public override void Tick(bool isActive)
	{
		if (!isActive || !interval_0.Elapsed)
		{
			return;
		}
		List<Monolith> list = (from m in ObjectManager.GetObjectsByType<Monolith>()
			where !m.IsMini
			select m).ToList();
		foreach (Monolith item in list)
		{
			if (!dictionary_0.TryGetValue(((NetworkObject)item).Id, out var value))
			{
				value = new MonolithCache(item);
				dictionary_0.Add(((NetworkObject)item).Id, value);
			}
			value.Update(item);
		}
		foreach (KeyValuePair<int, MonolithCache> item2 in dictionary_0)
		{
			item2.Value.Validate();
		}
	}

	public override void Stop(bool isActive)
	{
	}

	static MonolithData()
	{
		interval_0 = new Interval(250);
	}
}
