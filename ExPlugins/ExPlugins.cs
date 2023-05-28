using DreamPoeBot.Loki.Bot;

namespace ExPlugins;

public class ExPlugins : IAuthored, IUrlProvider
{
	public string Name => "ExPlugins";

	public string Description => "ExPlugins plugin container";

	public string Author => "Seusheque/Papashagodx";

	public string Version => "1.0.0.6a";

	public string Url => "https://discord.gg/HeqYtkujWW";
}
