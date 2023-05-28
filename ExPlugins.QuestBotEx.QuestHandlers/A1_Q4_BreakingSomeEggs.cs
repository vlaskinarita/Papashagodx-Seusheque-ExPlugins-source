using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;
using ExPlugins.EXtensions.Global;
using ExPlugins.EXtensions.Positions;

namespace ExPlugins.QuestBotEx.QuestHandlers;

public static class A1_Q4_BreakingSomeEggs
{
	private static IEnumerable<Chest> RhoaNests => ObjectManager.Objects.Where((Chest c) => ((NetworkObject)c).Metadata.Contains("QuestChests/RhoaChest"));

	private static NetworkObject GlyphWall => ObjectManager.Objects.Find((NetworkObject o) => o.Metadata == "Metadata/QuestObjects/WaterCave/GlyphWall");

	private static List<CachedObject> CachedRhoaNests
	{
		get
		{
			List<CachedObject> list = CombatAreaCache.Current.Storage["RhoaNests"] as List<CachedObject>;
			if (list == null)
			{
				list = new List<CachedObject>(3);
				CombatAreaCache.Current.Storage["RhoaNests"] = list;
			}
			return list;
		}
	}

	private static CachedObject CachedGlyphWall
	{
		get
		{
			return CombatAreaCache.Current.Storage["GlyphWall"] as CachedObject;
		}
		set
		{
			CombatAreaCache.Current.Storage["GlyphWall"] = value;
		}
	}

	public static void Tick()
	{
		if (!World.Act1.MudFlats.IsCurrentArea)
		{
			return;
		}
		if (CachedGlyphWall == null)
		{
			NetworkObject glyphWall = GlyphWall;
			if (glyphWall != (NetworkObject)null)
			{
				CachedGlyphWall = new CachedObject(glyphWall);
			}
		}
		foreach (Chest chest_0 in RhoaNests)
		{
			bool isOpened = chest_0.IsOpened;
			int id = ((NetworkObject)chest_0).Id;
			List<CachedObject> cachedRhoaNests = CachedRhoaNests;
			int num = cachedRhoaNests.FindIndex((CachedObject c) => c.Id == ((NetworkObject)chest_0).Id);
			if (num >= 0)
			{
				if (isOpened)
				{
					GlobalLog.Warn($"[BreakingSomeEggs] Removing opened {((NetworkObject)(object)chest_0).WalkablePosition()}");
					cachedRhoaNests.RemoveAt(num);
				}
			}
			else if (!isOpened)
			{
				WalkablePosition walkablePosition = ((NetworkObject)(object)chest_0).WalkablePosition();
				GlobalLog.Warn($"[BreakingSomeEggs] Registering {walkablePosition}");
				cachedRhoaNests.Add(new CachedObject(id, walkablePosition));
			}
		}
	}

	public static async Task<bool> GrabGlyphs()
	{
		if (Helpers.PlayerHasQuestItemAmount("Glyphs/Glyph", 3))
		{
			return false;
		}
		if (World.Act1.MudFlats.IsCurrentArea)
		{
			if (await Helpers.OpenQuestChest(CachedRhoaNests.FirstOrDefault()))
			{
				return true;
			}
			await Helpers.Explore();
			return true;
		}
		await Travel.To(World.Act1.MudFlats);
		return true;
	}

	public static async Task<bool> OpenPassage()
	{
		if (Helpers.PlayerHasQuestItemAmount("Glyphs/Glyph", 3))
		{
			if (World.Act1.MudFlats.IsCurrentArea)
			{
				if (await Helpers.HandleQuestObject(CachedGlyphWall))
				{
					return true;
				}
				await Travel.To(World.Act1.SubmergedPassage);
				return true;
			}
			await Travel.To(World.Act1.MudFlats);
			return true;
		}
		return false;
	}

	public static async Task<bool> TakeReward()
	{
		if (!World.Act1.MudFlats.IsCurrentArea)
		{
			if (World.Act1.LioneyeWatch.IsCurrentArea)
			{
				if (QuestManager.GetState(Quests.BreakingSomeEggs) == 0)
				{
					return false;
				}
				if (await TownNpcs.Tarkleigh.OpenDialogPanel())
				{
					if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Glyph Reward 2")))
					{
						string reward2 = "Any";
						if (!(await TownNpcs.Tarkleigh.TakeReward(reward2, "Glyph Reward 2")))
						{
							ErrorManager.ReportError();
						}
						return true;
					}
					if (NpcDialogUi.DialogEntries.Any((NpcDialogEntryWrapper d) => d.Text.EqualsIgnorecase("Glyph Reward")))
					{
						string reward = QuestBotSettings.Instance.GetRewardForQuest(Quests.BreakingSomeEggs.Id);
						if (!(await TownNpcs.Tarkleigh.TakeReward(reward, "Glyph Reward")))
						{
							ErrorManager.ReportError();
						}
					}
					return true;
				}
				ErrorManager.ReportError();
				return true;
			}
			await Travel.To(World.Act1.LioneyeWatch);
			return true;
		}
		await Travel.To(World.Act1.SubmergedPassage);
		return true;
	}
}
