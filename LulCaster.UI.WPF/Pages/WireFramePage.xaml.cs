using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LulCaster.UI.WPF.Pages
{
  /// <summary>
  /// Interaction logic for WireFramePage.xaml
  /// </summary>
  public partial class WireFramePage : Page, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly ScreenCaptureTimer _screenCaptureTimer;
    private const double CAPTURE_TIMER_INTERVAL = 3000;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private readonly Dictionary<string, Rectangle> _boundingBoxCollection = new Dictionary<string, Rectangle>(); //TODO: This will live in the region configuration tool
    private Rectangle _currentBoundingBox; //TODO: This will live in the region configuration tool
    private IRegionConfigService _configService;
    private PresetViewModel _selectedPreset;
    private IList<RegionViewModel> _regions;
    private RegionViewModel _selectedRegion;

    #region "Properties"
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

    public IList<RegionViewModel> Regions
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
    #endregion

    public WireFramePage(IRegionConfigService configService, IPresetListController presetListController, IRegionListController regionListController, IScreenCaptureService screenCaptureService)
    {

      InitializeComponent();
      cntrlRegionList.RegionListController = regionListController;
      cntrlPresetList.PresetController = presetListController;
      cntrlPresetList.LoadPresets();

      _screenCaptureTimer = new ScreenCaptureTimer(screenCaptureService, CAPTURE_TIMER_INTERVAL);
      _screenCaptureTimer.ScreenCaptureCompleted += _screenCaptureTimer_ScreenCaptureCompleted;
      _configService = configService;

      _screenCaptureTimer.Start();
    }

    private void _screenCaptureTimer_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      this.Dispatcher?.Invoke(() =>
      {
        var imageStream = new MemoryStream(captureArgs.ScreenImageStream);
        var screenCaptureImage = new BitmapImage();
        screenCaptureImage.BeginInit();
        screenCaptureImage.StreamSource = imageStream;
        screenCaptureImage.CacheOption = BitmapCacheOption.OnLoad;
        screenCaptureImage.EndInit();
        screenCaptureImage.Freeze();

        var imageBrush = new ImageBrush(screenCaptureImage);

        canvasScreenFeed.Background = imageBrush;
      }, System.Windows.Threading.DispatcherPriority.Background);
    }

    private void CanvasScreenFeed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released) return;

        canvasScreenFeed.Children.Clear();
        var newBox = _boundingBoxBrush.OnMouseDown(e);
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(newBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, newBox.X);
        Canvas.SetTop(windowsBox, newBox.Y);
        _boundingBoxCollection[""] = windowsBox; //TODO: The name needs to be taken from the segment configuration control
        _currentBoundingBox = windowsBox;
      });
    }

    private void CanvasScreenFeed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Pressed) return;
        //TODO: Update the configuration of the selected bounding box in the segment configuration control
      });
    }

    private void CanvasScreenFeed_MouseMove(object sender, MouseEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released) return;

        canvasScreenFeed.Children.Clear();

        var newBox = _boundingBoxBrush.OnMouseMove(e);
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(newBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, newBox.X);
        Canvas.SetTop(windowsBox, newBox.Y);
        _boundingBoxCollection[""] = windowsBox;
        _currentBoundingBox = windowsBox;
      });
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}