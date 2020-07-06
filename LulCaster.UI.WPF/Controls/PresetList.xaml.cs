using LulCaster.UI.WPF.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for PresetList.xaml
  /// </summary>
  public partial class PresetList : UserControl
  {
    private readonly IPresetListController _presetListController;

    public PresetList() { }

    public PresetList(IPresetListController presetListController)
    {
      InitializeComponent();
      _presetListController = presetListController;
      this.DataContext = _presetListController;
    }

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      _presetListController.ShowNewPresetDialog();
    }

    private void Button_BtndeletePreset(object sender, RoutedEventArgs e)
    {
      _presetListController.DeletePreset((string)lstBoxPresets.SelectedItem);
    }
  }
}