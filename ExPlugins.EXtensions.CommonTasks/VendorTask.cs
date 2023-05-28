using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions.CachedObjects;

namespace ExPlugins.EXtensions.CommonTasks;

public class VendorTask : ITask, IAuthored, ILogicProvider, IMessageHandler, IStartStopEvents, ITickEvents
{
	private readonly Class36[] class36_0 = new Class36[3]
	{
		new Class37(),
		new Class38(),
		new Class39()
	};

	public string Name => "VendorTask";

	public string Description => "Task for exchanging various items with vendors.";

	public string Author => "Alcor75";

	public string Version => "1.0";

	public async Task<bool> Run()
	{
		DatWorldAreaWrapper area = World.CurrentArea;
		if (area.IsTown || area.IsHideoutArea)
		{
			Class36 module = Enumerable.FirstOrDefault(class36_0, (Class36 m) => m.Enabled && m.ShouldExecute);
			if (module == null)
			{
				GlobalLog.Info("[VendorTask] No items to vendor.");
				return false;
			}
			await module.Execute();
			return true;
		}
		return false;
	}

	public MessageResult Message(Message message)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		if (!(message.Id == "item_stashed_event"))
		{
			if (message.Id == "combat_area_changed_event")
			{
				foreach (Class36 item in Enumerable.Where(class36_0, (Class36 m) => m.Enabled))
				{
					item.ResetErrors();
				}
				return (MessageResult)0;
			}
			return (MessageResult)1;
		}
		CachedItem input = message.GetInput<CachedItem>(0);
		foreach (Class36 item2 in Enumerable.Where(class36_0, (Class36 m) => m.Enabled))
		{
			item2.OnStashing(input);
		}
		return (MessageResult)0;
	}

	public async Task<LogicResult> Logic(Logic logic)
	{
		return (LogicResult)1;
	}

	public void Tick()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}
}
