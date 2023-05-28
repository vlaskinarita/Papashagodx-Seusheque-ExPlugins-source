using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using DreamPoeBot.Common;
using ExPlugins.EXtensions;
using ExPlugins.MapBotEx.Helpers;
using ExPlugins.TraderPlugin;
using Newtonsoft.Json;

internal class Class9
{
	public class Class10
	{
		[CompilerGenerated]
		private Vector2i vector2i_0;

		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private DateTime? nullable_0;

		[CompilerGenerated]
		private InfluenceHelper.InfluenceType? nullable_1;

		public Vector2i ItemPos
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return vector2i_0;
			}
			[CompilerGenerated]
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				vector2i_0 = value;
			}
		}

		public string ItemName
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string ItemPrice
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public int StackCount
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public DateTime? ListDate
		{
			[CompilerGenerated]
			get
			{
				return nullable_0;
			}
			[CompilerGenerated]
			set
			{
				nullable_0 = value;
			}
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public InfluenceHelper.InfluenceType? InfluenceType
		{
			[CompilerGenerated]
			get
			{
				return nullable_1;
			}
			[CompilerGenerated]
			set
			{
				nullable_1 = value;
			}
		}

		public Class10(Vector2i itemPos, string itemName, string itemPrice, int stackCount, DateTime? listDate, InfluenceHelper.InfluenceType? influenceType)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ItemPos = itemPos;
			ItemName = itemName;
			ItemPrice = itemPrice;
			StackCount = stackCount;
			ListDate = listDate;
			InfluenceType = influenceType;
		}
	}

	private static Class9 class9_0;

	[CompilerGenerated]
	private List<Class10> list_0 = new List<Class10>();

	public static Class9 EvaluatedItemsInstance => class9_0 ?? (class9_0 = new Class9());

	public List<Class10> ListedItems
	{
		[CompilerGenerated]
		get
		{
			return list_0;
		}
		[CompilerGenerated]
		set
		{
			list_0 = value;
		}
	}

	public void Load()
	{
		if (!File.Exists(TraderPlugin.FullFileName))
		{
			File.Create(TraderPlugin.FullFileName);
			return;
		}
		string text = File.ReadAllText(TraderPlugin.FullFileName);
		if (!string.IsNullOrWhiteSpace(text))
		{
			List<Class10> list = JsonConvert.DeserializeObject<List<Class10>>(text);
			if (list == null)
			{
				GlobalLog.Error("[TraderPlugin] Fail to load " + TraderPlugin.FullFileName + ". Json deserealizer returned null.");
			}
			else
			{
				ListedItems = list;
			}
		}
		else
		{
			GlobalLog.Error("[TraderPlugin] Fail to load " + TraderPlugin.FullFileName + ". File is empty.");
		}
	}
}
