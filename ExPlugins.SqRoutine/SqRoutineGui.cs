using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using ExPlugins.EXtensions;

namespace ExPlugins.SqRoutine;

public partial class SqRoutineGui : UserControl, IComponentConnector
{
	private bool bool_0;

	public SqRoutineGui()
	{
		InitializeComponent();
		base.DataContext = SqRoutineSettings.Instance;
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	public void RefreshVaalButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (!LokiPoe.IsInGame)
		{
			GlobalLog.Error("[SqRoutine] Not in game! ");
			return;
		}
		List<Skill> source = SkillBarHud.Skills.ToList();
		Skill skill_0 = source.FirstOrDefault((Skill s) => s.Name == "Vaal Discipline" && s.IsCastable);
		Skill skill_1 = source.FirstOrDefault((Skill s) => s.Name == "Vaal Molten Shell" && s.IsCastable);
		Skill skill_2 = source.FirstOrDefault((Skill s) => s.Name == "Vaal Summon Skeletons" && s.IsCastable);
		List<Skill> source2 = source.Where((Skill s) => s.SkillType.Contains("vaal") && !s.SkillTags.Contains("totem") && s.IsCastable && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_0 && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_1 && (RemoteMemoryObject)(object)s != (RemoteMemoryObject)(object)skill_2).ToList();
		foreach (Skill item in source2.Where((Skill s) => !SqRoutineSettings.Instance.VaalSkillsList.ToList().Any((SqRoutineSettings.VaalSkillEntry entry) => entry.Name.Equals(s.Name))))
		{
			SqRoutineSettings.Instance.VaalSkillsList.Add(new SqRoutineSettings.VaalSkillEntry(item.Name, (Rarity)2, soulEater: false));
		}
	}

	public void RefreshTotemButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (LokiPoe.IsInGame)
		{
			List<Skill> source = SkillBarHud.Skills.ToList();
			List<Skill> source2 = source.Where((Skill s) => (s.SkillTags.Contains("totem") || s.Stats.ContainsKey((StatTypeGGG)2330)) && s.IsCastable).ToList();
			{
				foreach (Skill item in source2.Where((Skill s) => !SqRoutineSettings.Instance.VaalSkillsList.ToList().Any((SqRoutineSettings.VaalSkillEntry entry) => entry.Name.Equals(s.Name))))
				{
					SqRoutineSettings.Instance.TotemSkillsList.Add(new SqRoutineSettings.TotemSkillEntry(item.Name, (Rarity)2, SqRoutineSettings.TotemUsageCase.BuffOnly, bool_1: false));
				}
				return;
			}
		}
		GlobalLog.Error("[SqRoutine] Not in game! ");
	}
}
