using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Workers.Events;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Windows.Controls;
using System.Windows.Media;

namespace LulCaster.UI.WPF.Pages
{
  /// <summary>
  /// Interaction logic for WireFramePage.xaml
  /// </summary>
  public partial class WireFramePage : Page
  {
    #region "Private Members"
    private readonly ScreenCaptureWorker _screenCaptureWorker;
    private readonly RegionWorkerPool _regionWorkerPool;
    private readonly SoundEffectWorker _soundEffectWorker;

    private readonly IConfigManagerService _configManagerService;

    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();

    #endregion "Private Members"

    #region "Properties"

    private WireFrameViewModel ViewModel { get => (WireFrameViewModel)DataContext; }

    #endregion "Properties"

    #region "Contructors"

    public WireFramePage(WireFrameViewModel viewModel
                          , IScreenCaptureService screenCaptureService
                          , IConfigManagerService configManagerService)
    {
      InitializeComponent();
      DataContext = viewModel;

      // Dialog Services
      _configManagerService = configManagerService;

      //Worker Initialization
      var captureFps = _configManagerService.GetAsInteger("CaptureFps");
      var workerIdleTimeout = _configManagerService.GetAsInteger("WorkIdleTimeout");
      _soundEffectWorker = new SoundEffectWorker(workerIdleTimeout);
      _screenCaptureWorker = new ScreenCaptureWorker(screenCaptureService, canvasScreenFeed.RenderSize, captureFps, workerIdleTimeout);
      _regionWorkerPool = new RegionWorkerPool(_configManagerService.GetAsInteger("MaxRegionThreads"), captureFps, workerIdleTimeout, ViewModel);

      InitializeWorkers();
      InitializeUserControlEvents();
    }

    #endregion "Contructors"

    #region "Initialization Methods"

    private void InitializeUserControlEvents()
    {
      LstGamePresets.SelectionChanged += LstGamePresets_SelectionChanged;
      Controls.RegionConfiguration.SaveConfigTriggered += RegionConfiguration_SaveConfigTriggered;
      _screenCaptureWorker.ScreenCaptureCompleted += ViewModel.screenCaptureWorker_ScreenCaptureCompleted;

      CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private async void RegionConfiguration_SaveConfigTriggered(object sender, RegionViewModel e)
    {
      await ViewModel.RegionControl.SaveTriggerConfigAsnyc();
    }

    private void InitializeWorkers()
    {
      _screenCaptureWorker.Start();
      _screenCaptureWorker.ScreenCaptureCompleted += _regionWorkerPool.screenCaptureWorker_ScreenCaptureCompleted;
      TriggerEmitter.TriggerActivated += _soundEffectWorker.triggerWorkerPool_TriggerActivated;
      _regionWorkerPool.Start();
    }

    #endregion "Initialization Methods"

    #region "User Control Events"

    private void LstGamePresets_SelectionChanged(object sender, Controls.IListItem e)
    {
      if (ViewModel?.PresetControl.SelectedPreset != null)
      {
        try
        {
          var handle = HandleFinder.GetWindowsHandle(ViewModel.PresetControl.SelectedPreset.Name);
          _screenCaptureWorker.SetGameHandle(handle);
          _screenCaptureWorker.Start();
        }
        catch
        {
          //TODO: This should not be empty
        }
      }
      else
      {
        _screenCaptureWorker.Stop();
      }
    }

    #endregion "User Control Events"

    #region "Screen Capture Events"

    private void DrawSelectedRegion()
    {
      canvasScreenFeed.Children.Clear();

      if (ViewModel?.RegionControl.SelectedRegion?.BoundingBox != null)
      {
        var selectedBox = ViewModel.RegionControl.SelectedRegion.BoundingBox;
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(selectedBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, selectedBox.X);
        Canvas.SetTop(windowsBox, selectedBox.Y);
        ViewModel.RegionControl.SelectedRegion.BoundingBox = selectedBox;
      }
    }

    #endregion "Screen Capture Events"

    #region "Special Controls Events"

    /// <summary>
    /// Used to hook into the render event to only draw region boxes when render happens
    /// </summary>
    private void CompositionTarget_Rendering(object sender, System.EventArgs e)
    {
      DrawSelectedRegion();
      _screenCaptureWorker.CanvasBounds = canvasScreenFeed.RenderSize;
    }

    private void canvasScreenFeed_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      ViewModel.RegionControl.CanvasMouseLeftButtonDownOccured(e);
    }

    private void canvasScreenFeed_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      ViewModel.RegionControl.CanvasMouseLeftButtonUpOccured(e);
    }
    private void canvasScreenFeed_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      ViewModel.RegionControl.CanvasMouseMoveOccured(e);
    }

    #endregion "Special Controls Events"

    #region "Preset Control Events"
    private async void LstGamePresets_NewItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      await ViewModel.PresetControl.NewItemClickedAsync(e);
    }

    private async void LstGamePresets_DeleteItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      await ViewModel.PresetControl.DeleteItemClickedAsync(sender, e);
    }

    private async void LstGamePresets_EditItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      await ViewModel.PresetControl.EditItemClickedAsync(sender, e);
    }
    #endregion "Preset Control Events"

    #region "Region Control Events"
    private void LstScreenRegions_NewItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      ViewModel.RegionControl.NewItemClicked(e);
    }

    private void LstScreenRegions_DeleteItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      ViewModel.RegionControl.DeleteItemClicked(sender, e);
    }

    private void LstScreenRegions_EditItemClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      ViewModel.RegionControl.EditItemClicked(sender, e);
    }
    #endregion

    #region "Region Config Control Events"
    private void cntrlRegionConfig_AddTriggerClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      ViewModel.RegionConfigControl.AddTriggerClicked(e);
    }

    private void cntrlRegionConfig_DeleteTriggerClicked(object sender, Controls.EventArgs.ButtonClickArgs e)
    {
      ViewModel.RegionConfigControl.DeleteTriggerClicked(e);
    }
    #endregion
  }
}