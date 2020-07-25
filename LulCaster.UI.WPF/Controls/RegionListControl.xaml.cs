﻿using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for SegmentList.xaml
  /// </summary>
  public partial class RegionListControl : UserControl, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

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
        typeof(ObservableCollection<RegionViewModel>),
        typeof(RegionListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRegionListChanged))
    );

    public static readonly DependencyProperty SelectedRegionProperty =
    DependencyProperty.Register
    (
        "SelectedRegion",
        typeof(RegionViewModel),
        typeof(RegionListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedRegionChanged))
    );

    #endregion "Dependency Properties"

    #region "Properties"

    public ObservableCollection<RegionViewModel> RegionList
    {
      get { return (ObservableCollection<RegionViewModel>)GetValue(RegionListProperty); }
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

    private void LoadRegions(string presetFilePath)
    {
      RegionList = new ObservableCollection<RegionViewModel>(RegionListController.GetRegions(presetFilePath));
    }

    #region "OnChanged Events"

    private static void OnSelectedPresetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionListControl thisControl)
      {
        thisControl.SelectedPreset = (PresetViewModel)e.NewValue;
        thisControl.LoadRegions(thisControl.SelectedPreset.FilePath);
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
        thisControl.RegionList = (ObservableCollection<RegionViewModel>)e.NewValue;
      }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion "OnChanged Events"
  }
}