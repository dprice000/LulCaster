using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for PresetControl.xaml
  /// </summary>
  public partial class PresetControl : UserControl
  {
    #region "Dependency Properties"

    public static readonly DependencyProperty PresetListProperty =
    DependencyProperty.Register
    (
        "PresetList",
        typeof(IList<PresetViewModel>),
        typeof(PresetControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPresetListChanged))
    );

    public static readonly DependencyProperty SelectedPresetProperty =
    DependencyProperty.Register
    (
        "SelectedPreset",
        typeof(PresetViewModel),
        typeof(PresetControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedPresetChanged))
    );

    #endregion "Dependency Properties"

    #region "Properties"

    public IList<PresetViewModel> PresetList
    {
      get { return (IList<PresetViewModel>)GetValue(PresetListProperty); }
      set { SetValue(PresetListProperty, value); }
    }

    public PresetViewModel SelectedPreset
    {
      get { return (PresetViewModel)GetValue(SelectedPresetProperty); }
      set { SetValue(SelectedPresetProperty, value); }
    }

    public IPresetListController PresetController { get; set; }

    #endregion "Properties"

    #region "Constructors"

    public PresetControl()
    {
      InitializeComponent();
    }

    #endregion "Constructors"

    #region "OnChanged Events"

    private static void OnSelectedPresetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is PresetControl thisControl)
      {
        thisControl.SelectedPreset = (PresetViewModel)e.NewValue;
      }
    }

    private static void OnPresetListChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is PresetControl thisControl)
      {
        thisControl.PresetList = (IList<PresetViewModel>)e.NewValue;
      }
    }

    #endregion "OnChanged Events"

    public void LoadPresets()
    {
      PresetList = PresetController.GetAllPresets().ToList();
    }

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      if (PresetController.ShowNewPresetDialog() is string presetName && !string.IsNullOrWhiteSpace(presetName))
      {
        var newPreset = PresetController.CreatePreset(presetName);
        PresetList.Add(newPreset);
        SelectedPreset = newPreset;
      }
      else
      {
        PresetController.ShowMessageBox("Empty Preset Name", "No Preset Name Provided!!", Dialogs.DialogButtons.Ok);
      }
    }

    private void Button_BtnDeletePreset(object sender, RoutedEventArgs e)
    {
      if (PresetController.ShowMessageBox("Delete Preset", $"Do you want to delete preset {SelectedPreset.Name}?", Dialogs.DialogButtons.YesNo) != Dialogs.DialogResults.Yes)
      {
        return;
      }

      if (PresetController.DeletePreset(SelectedPreset))
      {
        PresetList.Remove(SelectedPreset);
        SelectedPreset = null;
      }
    }
  }
}