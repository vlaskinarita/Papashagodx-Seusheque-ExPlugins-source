using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ExPlugins.AutoFlaskEx;

public class FlaskInfo
{
	public class TriggerFlask
	{
		public readonly string Name;

		public readonly int Slot;

		public readonly Stopwatch PostUseDelay;

		public readonly List<FlaskTrigger> Triggers;

		public TriggerFlask(string name, List<FlaskTrigger> triggers, int slot)
		{
			Name = name;
			Triggers = triggers;
			PostUseDelay = new Stopwatch();
			if (triggers.Exists((FlaskTrigger t) => t.Type == TriggerType.Attack))
			{
				PostUseDelay.Start();
			}
			Slot = slot;
		}
	}

	public readonly List<TriggerFlask> TriggerFlasks = new List<TriggerFlask>();

	public bool HasAntiBleed;

	public bool HasAntiCurse;

	public bool HasAntiFreeze;

	public bool HasAntiIgnite;

	public bool HasAntiPoison;

	public bool HasAntiShock;

	public bool HasAtzirisFlask;

	public bool HasDivinationDistillateFlask;

	public bool HasLifeFlask;

	public bool HasManaFlask;

	public bool HasQsilverFlask;

	public bool HasRumiConcoctionFlask;

	public bool HasSilverFlask;

	public bool HasSoulCatcherFlask;

	public bool bool_0;

	public bool HasTriggerFlask;

	public void AddTriggerFlask(int slot, string name, List<FlaskTrigger> triggers)
	{
		if (!TriggerFlasks.Exists((TriggerFlask f) => f.Name == name))
		{
			TriggerFlasks.Add(new TriggerFlask(name, triggers, slot));
		}
	}

	public void Log()
	{
		StringBuilder stringBuilder = new StringBuilder("[FlaskInfo] Flags: ");
		FieldInfo[] fields = GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(bool)) && (bool)fieldInfo.GetValue(this))
			{
				stringBuilder.Append(fieldInfo.Name + ", ");
			}
		}
		stringBuilder.Length -= 2;
		stringBuilder.Append('.');
	}
}
