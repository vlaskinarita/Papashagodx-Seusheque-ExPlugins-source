using System.ComponentModel;
using System.Runtime.CompilerServices;
using DreamPoeBot.Loki.Game.GameData;
using JetBrains.Annotations;

namespace ExPlugins.AutoLoginEx;

public class CharacterEntry : INotifyPropertyChanged
{
	private CharacterClass characterClass_0;

	private bool bool_0;

	private int int_0;

	public CharacterClass CharacterClass
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return characterClass_0;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			characterClass_0 = value;
			OnPropertyChanged("CharacterClass");
		}
	}

	public int TargetLevel
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
			OnPropertyChanged("TargetLevel");
		}
	}

	public bool IsCompleted
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
			OnPropertyChanged("IsCompleted");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public CharacterEntry()
	{
	}

	public CharacterEntry(CharacterClass characterClass, int targetLevel)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		CharacterClass = characterClass;
		TargetLevel = targetLevel;
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
