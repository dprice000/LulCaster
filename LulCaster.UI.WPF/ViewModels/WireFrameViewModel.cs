using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.ViewModels
{
  public class WireFrameViewModel : ViewModelBase
  {
    #region "Private Members"

    private PresetControlViewModel _presetControlViewModel;
    private RegionControlViewModel _regionControlViewModel;
    private RegionConfigViewModel _regionConfigViewModel;

    private Brush _screenCaptureBrush;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();

    #endregion "Private Members"

    #region "Properties"

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

    public PresetControlViewModel PresetControl
    {
      get
      {
        return _presetControlViewModel;
      }
      private set
      {
        _presetControlViewModel = value;
        OnPropertyChanged(nameof(PresetControl));
      }
    }

    public RegionControlViewModel RegionControl
    {
      get
      {
        return _regionControlViewModel;
      }
      private set
      {
        _regionControlViewModel = value;
        OnPropertyChanged(nameof(RegionControl));
      }
    }

    public RegionConfigViewModel RegionConfigControl
    {
      get
      {
        return _regionConfigViewModel;
      }
      set
      {
        _regionConfigViewModel = value;
        OnPropertyChanged(nameof(RegionConfigControl));
      }
    }

    #endregion "Properties"

    #region "Constructor"

    public WireFrameViewModel(PresetControlViewModel presetControlViewModel
      , RegionControlViewModel regionControlViewModel
      , RegionConfigViewModel regionConfigViewModel)
    {
      _presetControlViewModel = presetControlViewModel;
      _regionControlViewModel = regionControlViewModel;
      _regionConfigViewModel = regionConfigViewModel;

      PresetControl.SelectionChanged += PresetControl_SelectionChanged;
      RegionControl.SelectionChanged += RegionControl_SelectionChanged;
    }

    private void RegionControl_SelectionChanged(object sender, RegionViewModel e)
    {
      RegionConfigControl.SelectedRegion = e;
    }

    #endregion "Constructor"

    private void PresetControl_SelectionChanged(object sender, PresetViewModel e)
    {
      RegionControl.SelectedPreset = e;
      RegionConfigControl.SelectedPreset = e;
    }

    internal void screenCaptureWorker_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
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
  }
}