using System.Collections.Generic;
using System.Linq;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;

namespace ExPlugins.MapBotEx.ExtraContent.Breach;

public class BreachData : AreaData
{
	private readonly Dictionary<int, BreachCache> dictionary_0 = new Dictionary<int, BreachCache>();

	public IEnumerable<BreachCache> Breaches => dictionary_0.Select((KeyValuePair<int, BreachCache> kvp) => kvp.Value).ToList();

	public BreachData(uint hash)
		: base(hash)
	{
	}

	public override void Start(bool isActive)
	{
		foreach (KeyValuePair<int, BreachCache> item in dictionary_0)
		{
			item.Value.Activate = null;
		}
	}

	public override void Tick(bool isActive)
	{
		if (!isActive)
		{
			return;
		}
		List<Breach> list = ObjectManager.GetObjectsByType<Breach>().ToList();
		foreach (Breach item in list)
		{
			if (!dictionary_0.TryGetValue(((NetworkObject)item).Id, out var value))
			{
				value = new BreachCache((NetworkObject)(object)item);
				dictionary_0.Add(((NetworkObject)item).Id, value);
			}
		}
		foreach (KeyValuePair<int, BreachCache> item2 in dictionary_0)
		{
			item2.Value.Validate();
		}
	}

	public override void Stop(bool isActive)
	{
	}
}
