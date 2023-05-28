using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ExPlugins.EXtensions;

namespace ExPlugins.BasicGemLeveler;

public partial class Gui : UserControl, IComponentConnector
{
	private bool bool_0;

	public Gui()
	{
		InitializeComponent();
		base.FontFamily = new FontFamily("Bahnschrift Light");
	}

	private void RefreshSkillsButton_OnClick(object sender, RoutedEventArgs e)
	{
		BasicGemLevelerSettings.Instance.RefreshSkillGemsList();
	}

	private void AddGlobalNameIgnoreButton_OnClick(object sender, RoutedEventArgs e)
	{
		string text = GlobalNameIgnoreTextBox.Text;
		if (!string.IsNullOrEmpty(text))
		{
			if (!BasicGemLevelerSettings.Instance.GlobalNameIgnoreList.Contains(text))
			{
				BasicGemLevelerSettings.Instance.GlobalNameIgnoreList.Add(text);
				BasicGemLevelerSettings.Instance.UpdateGlobalNameIgnoreList();
				GlobalNameIgnoreTextBox.Text = "";
			}
			else
			{
				GlobalLog.Error("[AddGlobalNameIgnoreButtonOnClick] The skillgem " + text + " is already in the GlobalNameIgnoreList.");
			}
		}
	}

	private void RemoveGlobalNameIgnoreButton_OnClick(object sender, RoutedEventArgs e)
	{
		string text = GlobalNameIgnoreTextBox.Text;
		if (!string.IsNullOrEmpty(text))
		{
			if (!BasicGemLevelerSettings.Instance.GlobalNameIgnoreList.Contains(text))
			{
				GlobalLog.Error("[RemoveGlobalNameIgnoreButtonOnClick] The skillgem " + text + " is not in the GlobalNameIgnoreList.");
				return;
			}
			BasicGemLevelerSettings.Instance.GlobalNameIgnoreList.Remove(text);
			BasicGemLevelerSettings.Instance.UpdateGlobalNameIgnoreList();
			GlobalNameIgnoreTextBox.Text = "";
		}
	}

	private void AddSkillGemButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (AllSkillGemsComboBox.SelectedIndex != -1)
		{
			string text = AllSkillGemsComboBox.SelectedValue.ToString();
			if (!string.IsNullOrEmpty(text) && !BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Contains(text))
			{
				BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Add(text);
			}
		}
	}

	private void RemoveSkillGemButton_OnClick(object sender, RoutedEventArgs e)
	{
		if (AllSkillGemsComboBox.SelectedIndex != -1)
		{
			string text = AllSkillGemsComboBox.SelectedValue.ToString();
			if (!string.IsNullOrEmpty(text) && BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Contains(text))
			{
				BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Remove(text);
			}
		}
	}

	private void AddAllSkillGemButton_OnClick(object sender, RoutedEventArgs e)
	{
		foreach (object item in (IEnumerable)AllSkillGemsComboBox.Items)
		{
			string text = item.ToString();
			if (!string.IsNullOrEmpty(text) && !BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Contains(text))
			{
				BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Add(text);
			}
		}
	}

	private void RemoveAllSkillGemButton_OnClick(object sender, RoutedEventArgs e)
	{
		foreach (object item in (IEnumerable)AllSkillGemsComboBox.Items)
		{
			string text = item.ToString();
			if (!string.IsNullOrEmpty(text) && BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Contains(text))
			{
				BasicGemLevelerSettings.Instance.SkillGemsToLevelList.Remove(text);
			}
		}
	}

	private void GlobalNameIgnoreListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e != null && e.AddedItems.Count > 0)
		{
			GlobalNameIgnoreTextBox.Text = e.AddedItems[0].ToString();
		}
	}
}
