using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for SegmentList.xaml
  /// </summary>
  public partial class RegionListControl : UserControl
  {
    #region "Dependency Properties"

    public static readonly DependencyProperty SelectedPresetProperty =
DependencyProperty.Register
(
    "SelectedPreset",
    typeof(PresetViewModel),
    typeof(RegionListControl),
    new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedPresetChanged))
);

    public static readonly DependencyProperty RegionListProperty =
DependencyProperty.Register
(
    "RegionList",
    typeof(IList<RegionViewModel>),
    typeof(RegionListControl),
    new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRegionListChanged))
);

    public static readonly DependencyProperty SelectedRegionProperty =
    DependencyProperty.Register
    (
        "SelectedRegion",
        typeof(PresetViewModel),
        typeof(RegionListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedRegionChanged))
    );

    #endregion "Dependency Properties"

    #region "Properties"

    public IList<RegionViewModel> RegionList
    {
      get { return (IList<RegionViewModel>)GetValue(RegionListProperty); }
      set { SetValue(RegionListProperty, value); }
    }

    public RegionViewModel SelectedRegion
    {
      get { return (RegionViewModel)GetValue(SelectedRegionProperty); }
      set { SetValue(SelectedRegionProperty, value); }
    }

    public PresetViewModel SelectedPreset
    {
      get { return (PresetViewModel)GetValue(SelectedPresetProperty); }
      set { SetValue(SelectedPresetProperty, value); }
    }

    public IRegionListController RegionListController { get; set; }

    #endregion "Properties"

    public RegionListControl()
    {
      InitializeComponent();
    }

    private void BtnAddRegion_Click(object sender, RoutedEventArgs e)
    {
    }
    #region "OnChanged Events"

    private static void OnSelectedPresetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionListControl thisControl)
      {
        thisControl.SelectedPreset = (PresetViewModel)e.NewValue;
        thisControl.RegionList = thisControl.RegionListController.GetRegions(thisControl.SelectedPreset.FilePath).ToList();
      }
    }

    private static void OnSelectedRegionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionListControl thisControl)
      {
        thisControl.SelectedRegion = (RegionViewModel)e.NewValue;
      }
    }

    private static void OnRegionListChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionListControl thisControl)
      {
        thisControl.RegionList = (IList<RegionViewModel>)e.NewValue;
      }
    }

    #endregion "OnChanged Events"
  }
}