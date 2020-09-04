using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Pages
{
  /// <summary>
  /// Interaction logic for WireFramePage.xaml
  /// </summary>
  public partial class WireFramePage : Page
  {
    #region "Private Members"
    private readonly ScreenCaptureWorker _screenCaptureTimer;
    private const int CAPTURE_TIMER_INTERVAL = 1000;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private readonly IPresetListController _presetListController;
    private readonly IRegionListController _regionListController;
    private readonly ITriggerController _triggerController;
    private readonly IDialogService<InputDialog, InputDialogResult> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, LulDialogResult> _messageBoxService;

    #endregion "Private Members"

    #region "Properties"

    private WireFrameViewModel ViewModel { get => (WireFrameViewModel)DataContext; }

    #endregion "Properties"

    public WireFramePage(IPresetListController presetListController
                          , IRegionListController regionListController
                          , ITriggerController triggerController
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
      InitializeRegionConfigEvents();

      //Controllers
      _presetListController = presetListController;
      _regionListController = regionListController;
      _triggerController = triggerController;
      ViewModel.Presets = new ObservableCollection<PresetViewModel>(_presetListController.GetAllPresets());

      _screenCaptureTimer = new ScreenCaptureWorker(screenCaptureService, CAPTURE_TIMER_INTERVAL);
      _screenCaptureTimer.ScreenCaptureCompleted += _screenCaptureTimer_ScreenCaptureCompleted;
      _screenCaptureTimer.Start();

      //User Control Events
      LstGamePresets.SelectionChanged += LstGamePresets_SelectionChanged;
      Controls.RegionConfiguration.SaveConfigTriggered += RegionConfiguration_SaveConfigTriggered;
      ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(ViewModel.SelectedRegion))
      {
        DrawSelectedRegion();
      }
    }

    private void InitializeRegionConfigEvents()
    {
      cntrlRegionConfig.BtnAddTrigger.Click += BtnAddTrigger_Click;
      cntrlRegionConfig.BtnDeleteTrigger.Click += BtnDeleteTrigger_Click;
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

        DrawSelectedRegion();
      }, System.Windows.Threading.DispatcherPriority.Background);
    }

    private void CanvasScreenFeed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        canvasScreenFeed.Children.Clear();

        if (e.LeftButton == MouseButtonState.Released || ViewModel.SelectedRegion == null) return;

        ViewModel.SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseDown(e);
      });
    }

    private void CanvasScreenFeed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(async () =>
      {
        if (e.LeftButton == MouseButtonState.Pressed) return;

        if (ViewModel.SelectedRegion?.BoundingBox != null)
        {
          await _regionListController.UpdateRegionAsync(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion);
        }
      });
    }

    private void CanvasScreenFeed_MouseMove(object sender, MouseEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        canvasScreenFeed.Children.Clear();

        if (e.LeftButton == MouseButtonState.Released || ViewModel.SelectedRegion == null) return;

        ViewModel.SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseMove(e);
      });
    }

    private void DrawSelectedRegion()
    {
      canvasScreenFeed.Children.Clear();

      if (ViewModel?.SelectedRegion?.BoundingBox != null)
      {
        var selectedBox = ViewModel.SelectedRegion.BoundingBox;
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(selectedBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, selectedBox.X);
        Canvas.SetTop(windowsBox, selectedBox.Y);
        ViewModel.SelectedRegion.BoundingBox = selectedBox;
      }
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
      if (e.DialogResult == DialogResults.Yes)
      {
        _presetListController.DeletePreset(ViewModel.SelectedPreset);
        ViewModel.Presets.Remove(ViewModel.SelectedPreset);
        ViewModel.SelectedPreset = null;
      }
    }

    private void LstScreenRegions_NewRegionDialogExecuted(object sender, InputDialogResult e)
    {
      if (e.DialogResult == DialogResults.Ok)
      {
        var newRegion = _regionListController.CreateRegion(ViewModel.SelectedPreset.Id, e.Input);
        ViewModel.Regions.Add(newRegion);
        ViewModel.SelectedRegion = newRegion;
      }
    }

    private void LstScreenRegions_DeleteRegionDialogExecuted(object sender, LulDialogResult e)
    {
      if (e.DialogResult == DialogResults.Yes)
      {
        _regionListController.DeleteRegion(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id);
        ViewModel.Regions.Remove(ViewModel.SelectedRegion);
        ViewModel.SelectedRegion = null;
      }
    }
    #endregion

    #region "Region Config Events"
    private void BtnAddTrigger_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (_inputDialog.Show("New Trigger", "New Trigger Name:", DialogButtons.OkCancel) is InputDialogResult dialogResult && dialogResult.DialogResult == DialogResults.Ok)
      {
        var newTrigger = _triggerController.CreateTrigger(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id, dialogResult.Input);

        ViewModel.SelectedRegion.Triggers.Add(newTrigger);
        ViewModel.SelectedTrigger = newTrigger;
      }
    }

    private void BtnDeleteTrigger_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (_messageBoxService.Show("Delete Trigger","Delete selected trigger?", DialogButtons.YesNo) is LulDialogResult dialogResult 
        && dialogResult.DialogResult == DialogResults.Yes)
      {
        _triggerController.DeleteTrigger(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id, ViewModel.SelectedTrigger);
        ViewModel.SelectedRegion.Triggers.Remove(ViewModel.SelectedTrigger);
        ViewModel.SelectedTrigger = null;
      }
    }
    #endregion
  }
}