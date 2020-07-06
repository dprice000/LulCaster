using LulCaster.UI.WPF.Controllers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for PresetList.xaml
  /// </summary>
  public partial class PresetList : UserControl
  {
    private readonly IPresetListController presetListController;

    public List<string> PresetNames = new List<string>();

    public PresetList(IPresetListController presetListController)
    {
      InitializeComponent();
      this.presetListController = presetListController;
    }

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      PresetNames.Add(presetListController.ShowNewPresetDialog());     
    }

    private void Button_BtndeletePreset(object sender, RoutedEventArgs e)
    {
      var selectedPreset = (string)lstBoxPresets.SelectedItem;
      presetListController.DeletePreset(selectedPreset);
      PresetNames.Remove(selectedPreset);
    }
  }
}