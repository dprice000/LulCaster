using LulCaster.UI.WPF.Workers.Events.Arguments;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.ViewModels
{
  public class WireFrameViewModel : ViewModelBase
  {
    private ObservableCollection<PresetViewModel> _presets;
    private PresetViewModel _selectedPreset;
    private ObservableCollection<RegionViewModel> _regions;
    private RegionViewModel _selectedRegion;
    private TriggerViewModel _selectedTrigger;
    private Brush _screenCaptureBrush;

    public Brush ScreenCaptureBrush
    {
      get
      {
        return _screenCaptureBrush;
      }
      set
      {
        _screenCaptureBrush = value;
        OnPropertyChanged(nameof(ScreenCaptureBrush));
      }
    }

    internal void _screenCaptureWorker_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      var imageStream = new MemoryStream(captureArgs.ScreenImageStream);
      var screenCaptureImage = new BitmapImage();
      screenCaptureImage.BeginInit();
      screenCaptureImage.StreamSource = imageStream;
      screenCaptureImage.CacheOption = BitmapCacheOption.OnLoad;
      screenCaptureImage.EndInit();
      screenCaptureImage.Freeze();

      Application.Current.Dispatcher.Invoke(() =>
      {
        ScreenCaptureBrush = new ImageBrush(screenCaptureImage);
      });
    }

    public ObservableCollection<PresetViewModel> Presets
    {
      get
      {
        return _presets;
      }
      set
      {
        _presets = value;
        OnPropertyChanged(nameof(Presets));
      }
    }

    public PresetViewModel SelectedPreset
    {
      get
      {
        return _selectedPreset;
      }
      set
      {
        _selectedPreset = value;
        OnPropertyChanged(nameof(SelectedPreset));
      }
    }

    public ObservableCollection<RegionViewModel> Regions
    {
      get
      {
        return _regions;
      }
      set
      {
        _regions = value;
        OnPropertyChanged(nameof(Regions));
      }
    }

    public RegionViewModel SelectedRegion
    {
      get
      {
        return _selectedRegion;
      }
      set
      {
        _selectedRegion = value;
        OnPropertyChanged(nameof(SelectedRegion));
      }
    }

    public TriggerViewModel SelectedTrigger
    {
      get
      {
        return _selectedTrigger;
      }
      set
      {
        _selectedTrigger = value;
        OnPropertyChanged(nameof(SelectedTrigger));
      }
    }
  }
}