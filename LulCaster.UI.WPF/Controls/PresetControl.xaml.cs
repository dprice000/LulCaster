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

    public void LoadPresets()
    {
      PresetList = PresetController.GetAllPresets().ToList();
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

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      if (PresetController.ShowNewPresetDialog() is PresetViewModel newPreset)
      {
        PresetList.Add(newPreset);
        SelectedPreset = newPreset;
      }
    }

    private void Button_BtndeletePreset(object sender, RoutedEventArgs e)
    {
      PresetController.DeletePreset(SelectedPreset.Id);
      PresetList.Remove(SelectedPreset);
      SelectedPreset = null;
    }
  }
}