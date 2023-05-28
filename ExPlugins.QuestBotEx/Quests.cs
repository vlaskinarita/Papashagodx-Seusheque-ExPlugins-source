using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;
using ExPlugins.EXtensions;

namespace ExPlugins.QuestBotEx;

public static class Quests
{
	[AttributeUsage(AttributeTargets.Field)]
	public class QuestId : Attribute
	{
		public readonly string Id;

		public QuestId(string id)
		{
			Id = id;
		}
	}

	[QuestId("a1q1")]
	public static readonly DatQuestWrapper EnemyAtTheGate;

	[QuestId("a1q5")]
	public static readonly DatQuestWrapper MercyMission;

	[QuestId("a1q8")]
	public static readonly DatQuestWrapper DirtyJob;

	[QuestId("a1q4")]
	public static readonly DatQuestWrapper BreakingSomeEggs;

	[QuestId("a1q7")]
	public static readonly DatQuestWrapper DwellerOfTheDeep;

	[QuestId("a1q2")]
	public static readonly DatQuestWrapper CagedBrute;

	[QuestId("a1q6")]
	public static readonly DatQuestWrapper MaroonedMariner;

	[QuestId("a1q3")]
	public static readonly DatQuestWrapper SirensCadence;

	[QuestId("a2q4")]
	public static readonly DatQuestWrapper SharpAndCruel;

	[QuestId("a1q9")]
	public static readonly DatQuestWrapper WayForward;

	[QuestId("a2q10")]
	public static readonly DatQuestWrapper GreatWhiteBeast;

	[QuestId("a2q6")]
	public static readonly DatQuestWrapper IntrudersInBlack;

	[QuestId("a2q5")]
	public static readonly DatQuestWrapper ThroughSacredGround;

	[QuestId("a2q7")]
	public static readonly DatQuestWrapper DealWithBandits;

	[QuestId("hideout1")]
	public static readonly DatQuestWrapper HelenaHideout;

	[QuestId("a2q8")]
	public static readonly DatQuestWrapper ShadowOfVaal;

	[QuestId("a3q1")]
	public static readonly DatQuestWrapper LostInLove;

	[QuestId("a3q11")]
	public static readonly DatQuestWrapper VictarioSecrets;

	[QuestId("a3q4")]
	public static readonly DatQuestWrapper RibbonSpool;

	[QuestId("a3q5")]
	public static readonly DatQuestWrapper FieryDust;

	[QuestId("a3q8")]
	public static readonly DatQuestWrapper SeverRightHand;

	[QuestId("a3q9")]
	public static readonly DatQuestWrapper PietyPets;

	[QuestId("a3q13")]
	public static readonly DatQuestWrapper SwigOfHope;

	[QuestId("a3q12")]
	public static readonly DatQuestWrapper FixtureOfFate;

	[QuestId("a3q10")]
	public static readonly DatQuestWrapper SceptreOfGod;

	[QuestId("a4q2")]
	public static readonly DatQuestWrapper BreakingSeal;

	[QuestId("a4q6")]
	public static readonly DatQuestWrapper IndomitableSpirit;

	[QuestId("a4q3")]
	public static readonly DatQuestWrapper KingOfFury;

	[QuestId("a4q4")]
	public static readonly DatQuestWrapper KingOfDesire;

	[QuestId("a4q1")]
	public static readonly DatQuestWrapper EternalNightmare;

	[QuestId("a5q1b")]
	public static readonly DatQuestWrapper ReturnToOriath;

	[QuestId("a5q3")]
	public static readonly DatQuestWrapper InServiceToScience;

	[QuestId("a5q2")]
	public static readonly DatQuestWrapper KeyToFreedom;

	[QuestId("a5q4")]
	public static readonly DatQuestWrapper DeathToPurity;

	[QuestId("a5q5")]
	public static readonly DatQuestWrapper KingFeast;

	[QuestId("a5q7")]
	public static readonly DatQuestWrapper KitavaTorments;

	[QuestId("a5q6")]
	public static readonly DatQuestWrapper RavenousGod;

	[QuestId("a6q4")]
	public static readonly DatQuestWrapper FallenFromGrace;

	[QuestId("a6q5")]
	public static readonly DatQuestWrapper BestelEpic;

	[QuestId("a6q3")]
	public static readonly DatQuestWrapper FatherOfWar;

	[QuestId("a6q2")]
	public static readonly DatQuestWrapper EssenceOfUmbra;

	[QuestId("a6q7")]
	public static readonly DatQuestWrapper ClovenOne;

	[QuestId("a6q6")]
	public static readonly DatQuestWrapper PuppetMistress;

	[QuestId("a6q1")]
	public static readonly DatQuestWrapper BrineKing;

	[QuestId("a7q5")]
	public static readonly DatQuestWrapper SilverLocket;

	[QuestId("a7q2")]
	public static readonly DatQuestWrapper EssenceOfArtist;

	[QuestId("a7q3")]
	public static readonly DatQuestWrapper WebOfSecrets;

	[QuestId("a7q1")]
	public static readonly DatQuestWrapper MasterOfMillionFaces;

	[QuestId("a7q8")]
	public static readonly DatQuestWrapper InMemoryOfGreust;

	[QuestId("a7q7")]
	public static readonly DatQuestWrapper LightingTheWay;

	[QuestId("a7q9")]
	public static readonly DatQuestWrapper QueenOfDespair;

	[QuestId("a7q6")]
	public static readonly DatQuestWrapper KisharaStar;

	[QuestId("a7q4")]
	public static readonly DatQuestWrapper MotherOfSpiders;

	[QuestId("a8q1")]
	public static readonly DatQuestWrapper EssenceOfHag;

	[QuestId("a8q6")]
	public static readonly DatQuestWrapper LoveIsDead;

	[QuestId("a8q7")]
	public static readonly DatQuestWrapper GemlingLegion;

	[QuestId("a8q5")]
	public static readonly DatQuestWrapper WingsOfVastiri;

	[QuestId("a8q4")]
	public static readonly DatQuestWrapper ReflectionOfTerror;

	[QuestId("a8q2")]
	public static readonly DatQuestWrapper LunarEclipse;

	[QuestId("a9q3")]
	public static readonly DatQuestWrapper StormBlade;

	[QuestId("a9q5")]
	public static readonly DatQuestWrapper QueenOfSands;

	[QuestId("a9q4")]
	public static readonly DatQuestWrapper FastisFortuna;

	[QuestId("a9q2")]
	public static readonly DatQuestWrapper RulerOfHighgate;

	[QuestId("a9q1")]
	public static readonly DatQuestWrapper RecurringNightmare;

	[QuestId("a10q1")]
	public static readonly DatQuestWrapper SafePassage;

	[QuestId("a10q4")]
	public static readonly DatQuestWrapper NoLoveForOldGhosts;

	[QuestId("a10q6")]
	public static readonly DatQuestWrapper VilentaVengeance;

	[QuestId("a10q5")]
	public static readonly DatQuestWrapper MapToTsoatha;

	[QuestId("a10q2")]
	public static readonly DatQuestWrapper DeathAndRebirth;

	[QuestId("a10q3")]
	public static readonly DatQuestWrapper EndToHunger;

	[QuestId("a10q3a11")]
	public static readonly DatQuestWrapper EndToHungerEpilogue;

	[QuestId("A11q0")]
	public static readonly DatQuestWrapper TowardtheFuture;

	[QuestId("tangle")]
	public static readonly DatQuestWrapper EaterOfWorlds;

	[QuestId("cleansing_fire")]
	public static readonly DatQuestWrapper SearingExarch;

	[QuestId("uberelder")]
	public static readonly DatQuestWrapper UberElder;

	[QuestId("maven_boss")]
	public static readonly DatQuestWrapper MavenBoss;

	public static readonly List<DatQuestWrapper> All;

	static Quests()
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		All = new List<DatQuestWrapper>();
		Dictionary<string, DatQuestWrapper> dictionary = Dat.Quests.ToDictionary((DatQuestWrapper q) => q.Id);
		string text = "";
		FieldInfo[] fields = typeof(Quests).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			QuestId customAttribute = fieldInfo.GetCustomAttribute<QuestId>();
			if (customAttribute != null)
			{
				if (!dictionary.TryGetValue(customAttribute.Id, out var value))
				{
					GlobalLog.Error("[Quests] Cannot initialize \"" + fieldInfo.Name + "\" field. DatQuests does not contain quest with \"" + customAttribute.Id + "\" id.");
					text = "[Quests] Cannot initialize \"" + fieldInfo.Name + "\" field. DatQuests does not contain quest with \"" + customAttribute.Id + "\" id.";
				}
				else
				{
					fieldInfo.SetValue(null, value);
					All.Add(value);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			BotManager.Stop(new StopReasonData("quest_init_error", text, (object)null), false);
		}
	}
}
