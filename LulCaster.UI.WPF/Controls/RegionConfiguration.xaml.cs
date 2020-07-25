using LulCaster.UI.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for SegementConfiguration.xaml
  /// </summary>
  public partial class RegionConfiguration : UserControl
  {

    #region "Dependency Properties"

    public static readonly DependencyProperty SelectedRegionProperty =
    DependencyProperty.Register
    (
        "SelectedRegion",
        typeof(RegionViewModel),
        typeof(RegionConfiguration),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedRegionChanged))
    );

    #endregion

    #region "Properties"

    public RegionViewModel SelectedRegion
    {
      get { return (RegionViewModel)GetValue(SelectedRegionProperty); }
      set { SetValue(SelectedRegionProperty, value); }
    }

    #endregion


    public RegionConfiguration()
    {
      InitializeComponent();

      this.DataContext = new RegionViewModel();
    }

    private void btnSoundBrowser_Click(object sender, RoutedEventArgs e)
    {
    }

    private static void OnSelectedRegionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionConfiguration thisControl)
      {
        thisControl.SelectedRegion = (RegionViewModel)e.NewValue;
        thisControl.DataContext = thisControl.SelectedRegion;
      }
    }
  }
}