using System;
using DreamPoeBot.Loki.Game;

namespace ExPlugins.EXtensions;

public class InputDelayOverride : IDisposable
{
	private readonly int int_0;

	public InputDelayOverride(int newDelay)
	{
		int_0 = Input.InputEventDelayMs;
		GlobalLog.Info($"[InputDelayOverride] Setting input delay to {newDelay} ms.");
		Input.InputEventDelayMs = newDelay;
	}

	public void Dispose()
	{
		GlobalLog.Info($"[InputDelayOverride] Restoring original input delay {int_0} ms.");
		Input.InputEventDelayMs = int_0;
	}
}
