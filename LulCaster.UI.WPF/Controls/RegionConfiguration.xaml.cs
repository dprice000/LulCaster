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
  public partial class RegionConfiguration : UserControl, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

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
      set 
      { 
        SetValue(SelectedRegionProperty, value);
        OnPropertyChanged(nameof(IsControlEnabled));
      }
    }

    public bool IsControlEnabled
    {
      get
      {
        return SelectedRegion != null;
      }
    }

    #endregion


    public RegionConfiguration()
    {
      InitializeComponent();

      this.DataContext = new RegionViewModel();
    }

    private void btnSoundBrowser_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "MP3|*.mp3|WAV (*.wav)|*.wav|WMA|*.wma";

      if (openFileDialog.ShowDialog() == true)
        SelectedRegion.SoundFilePath = openFileDialog.FileName;
    }

    private static void OnSelectedRegionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionConfiguration thisControl)
      {
        thisControl.SelectedRegion = (RegionViewModel)e.NewValue;
        thisControl.DataContext = thisControl.SelectedRegion;
      }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}