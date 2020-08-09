using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
  public partial class WireFramePage : Page
  {
    #region "Private Members"
    private readonly ScreenCaptureTimer _screenCaptureTimer;
    private const double CAPTURE_TIMER_INTERVAL = 3000;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private readonly Dictionary<string, Rectangle> _boundingBoxCollection = new Dictionary<string, Rectangle>(); //TODO: This will live in the region configuration tool
    private Rectangle _currentBoundingBox; //TODO: This will live in the region configuration tool
    private readonly IPresetListController _presetListController;
    private readonly IRegionListController _regionListController;
    private readonly IDialogService<InputDialog, InputDialogResult> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, LulDialogResult> _messageBoxService;

    #endregion "Private Members"

    #region "Properties"

    private WireFrameViewModel ViewModel { get => (WireFrameViewModel)DataContext; }

    #endregion "Properties"

    public WireFramePage(IPresetListController presetListController
                          , IRegionListController regionListController
                          , IScreenCaptureService screenCaptureService
                          , IDialogService<InputDialog, InputDialogResult> inputDialog
                          , IDialogService<MessageBoxDialog, LulDialogResult> messageBoxService)
    {
      InitializeComponent();
      DataContext = new WireFrameViewModel();
      // Dialog Services
      _inputDialog = inputDialog;
      _messageBoxService = messageBoxService;
      InitializeDialogs();

      //Controllers
      _presetListController = presetListController;
      _regionListController = regionListController;

      ViewModel.Presets = new ObservableCollection<PresetViewModel>(_presetListController.GetAllPresets());

      _screenCaptureTimer = new ScreenCaptureTimer(screenCaptureService, CAPTURE_TIMER_INTERVAL);
      _screenCaptureTimer.ScreenCaptureCompleted += _screenCaptureTimer_ScreenCaptureCompleted;
      _screenCaptureTimer.Start();

      //User Control Events
      LstGamePresets.SelectionChanged += LstGamePresets_SelectionChanged;
      Controls.RegionConfiguration.SaveConfigTriggered += RegionConfiguration_SaveConfigTriggered;
    }

    private void InitializeDialogs()
    {
      LstGamePresets.InputDialog = _inputDialog;
      LstGamePresets.MessageBoxService = _messageBoxService;
      LstScreenRegions.InputDialog = _inputDialog;
      LstScreenRegions.MessageBoxService = _messageBoxService;
    }

    private void LstGamePresets_SelectionChanged(object sender, Controls.IListItem e)
    {
      ViewModel.Regions = (ViewModel?.SelectedPreset != null)
        ? new ObservableCollection<RegionViewModel>(_regionListController.GetRegions(ViewModel.SelectedPreset.Id))
        : null;
    }

    private async void RegionConfiguration_SaveConfigTriggered(object sender, RegionViewModel e)
    {
      await _regionListController.UpdateRegionAsync(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion);
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

    #region "Dialog Events"
    private void LstGamePresets_NewPresetDialogExecuted(object sender, InputDialogResult e)
    {
      if (e.DialogResult == Dialogs.Models.DialogResults.Ok)
      {
        var newPreset = _presetListController.CreatePreset(e.Input);
        ViewModel.Presets.Add(newPreset);
        ViewModel.SelectedPreset = newPreset;
      }
    }

    private void LstGamePresets_DeletePresetDialogExecuted(object sender, LulDialogResult e)
    {
      if (e.DialogResult == Dialogs.Models.DialogResults.Yes)
      {
        _presetListController.DeletePreset(ViewModel.SelectedPreset);
        ViewModel.Presets.Remove(ViewModel.SelectedPreset);
        ViewModel.SelectedPreset = null;
      }
    }

    private void LstScreenRegions_NewRegionDialogExecuted(object sender, InputDialogResult e)
    {
      if (e.DialogResult == Dialogs.Models.DialogResults.Ok)
      {
        var newRegion = _regionListController.CreateRegion(ViewModel.SelectedPreset.Id, e.Input);
        ViewModel.Regions.Add(newRegion);
        ViewModel.SelectedRegion = newRegion;
      }
    }

    private void LstScreenRegions_DeleteRegionDialogExecuted(object sender, LulDialogResult e)
    {
      if (e.DialogResult == Dialogs.Models.DialogResults.Yes)
      {
        _regionListController.DeleteRegion(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id);
        ViewModel.Regions.Remove(ViewModel.SelectedRegion);
        ViewModel.SelectedRegion = null;
      }
    }
    #endregion
  }
}