using LulCaster.UI.WPF.ViewModels;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        typeof(RegionConfiguration)
    );

    #endregion

    #region "Properties"

    public RegionViewModel SelectedRegion
    {
      get { return (RegionViewModel)GetValue(SelectedRegionProperty); }
      set 
      { 
        SetValue(SelectedRegionProperty, value);
      }
    }
    #endregion


    public RegionConfiguration()
    {
      InitializeComponent();
    }

    private void btnSoundBrowser_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "MP3|*.mp3|WAV (*.wav)|*.wav|WMA|*.wma";

      if (openFileDialog.ShowDialog() == true)
        SelectedRegion.SoundFilePath = openFileDialog.FileName;
    }
  }
}