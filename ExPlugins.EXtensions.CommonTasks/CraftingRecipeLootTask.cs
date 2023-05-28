using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.EXtensions.CommonTasks;

public class CraftingRecipeLootTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private static readonly Interval interval_0;

	private static readonly Dictionary<int, Stopwatch> dictionary_0;

	private static readonly TimeSpan timeSpan_0;

	private static CachedObject cachedObject_0;

	public string Name => "CraftingRecipeLootTask";

	public string Description => "Task for unlocking recipes.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		CombatAreaCache cache = CombatAreaCache.Current;
		if (cachedObject_0 != null)
		{
			await ProcessRecipe();
			return true;
		}
		CachedObject closestChest = cache.CraftingRecipe.ClosestValid();
		if (closestChest != null && ShouldOpen(closestChest, 150))
		{
			cachedObject_0 = closestChest;
			return true;
		}
		return false;
	}

	public void Tick()
	{
		if (LokiPoe.IsInGame && interval_0.Elapsed && cachedObject_0 != null)
		{
			NetworkObject @object = cachedObject_0.Object;
			CraftingRecipe val = (CraftingRecipe)(object)((@object is CraftingRecipe) ? @object : null);
			if ((NetworkObject)(object)val != (NetworkObject)null && val.IsOpened)
			{
				CombatAreaCache.Current.CraftingRecipe.Remove(cachedObject_0);
				cachedObject_0 = null;
			}
		}
	}

	public MessageResult Message(Message message)
	{
		return (MessageResult)1;
	}

	private static async Task ProcessRecipe()
	{
		WalkablePosition pos = cachedObject_0.Position;
		if (!((float)pos.Distance > 187.5f))
		{
			if (!pos.IsFar && !pos.IsFarByPath)
			{
				CraftingRecipe craftingRecipe_0 = default(CraftingRecipe);
				ref CraftingRecipe reference = ref craftingRecipe_0;
				NetworkObject @object = cachedObject_0.Object;
				reference = (CraftingRecipe)(object)((@object is CraftingRecipe) ? @object : null);
				if (!((NetworkObject)(object)craftingRecipe_0 == (NetworkObject)null) && !craftingRecipe_0.IsOpened)
				{
					if (++cachedObject_0.InteractionAttempts > 3)
					{
						GlobalLog.Error("[CraftingRecipeLootTask] All attempts to open a recipe have been spent. Now ignoring it.");
						cachedObject_0.Ignored = true;
						cachedObject_0 = null;
					}
					else if (await PlayerAction.Interact((NetworkObject)(object)craftingRecipe_0))
					{
						await Wait.LatencySleep();
						if (await Wait.For(() => craftingRecipe_0.IsOpened, "recipe opening", 50, 300))
						{
							CombatAreaCache.Current.CraftingRecipe.Remove(cachedObject_0);
							cachedObject_0 = null;
						}
					}
					else
					{
						await Wait.SleepSafe(300);
					}
				}
				else
				{
					CombatAreaCache.Current.CraftingRecipe.Remove(cachedObject_0);
					cachedObject_0 = null;
				}
			}
			else if (!pos.TryCome())
			{
				GlobalLog.Error($"[CraftingRecipeLootTask] Fail to move to {pos}. Marking this recipe as unwalkable.");
				cachedObject_0.Unwalkable = true;
				cachedObject_0 = null;
			}
		}
		else
		{
			GlobalLog.Debug("[CraftingRecipeLootTask] Abandoning current recipe because its too far away.");
			TemporaryIgnore(cachedObject_0.Id);
			cachedObject_0 = null;
		}
	}

	private static bool ShouldOpen(CachedObject obj, int openRange)
	{
		if (openRange != -1 && obj.Position.Distance > openRange)
		{
			return false;
		}
		return !IsTemporaryIgnored(obj.Id);
	}

	private static void TemporaryIgnore(int id)
	{
		dictionary_0.Add(id, Stopwatch.StartNew());
	}

	private static bool IsTemporaryIgnored(int id)
	{
		if (dictionary_0.TryGetValue(id, out var value))
		{
			if (value.Elapsed >= timeSpan_0)
			{
				dictionary_0.Remove(id);
				return false;
			}
			return true;
		}
		return false;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	static CraftingRecipeLootTask()
	{
		interval_0 = new Interval(100);
		dictionary_0 = new Dictionary<int, Stopwatch>();
		timeSpan_0 = TimeSpan.FromSeconds(15.0);
	}
}
