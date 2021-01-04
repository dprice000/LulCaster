using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Workers.Events;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
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
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private readonly IPresetListController _presetListController;
    private readonly IRegionListController _regionListController;
    private readonly ITriggerController _triggerController;
    private readonly IConfigManagerService _configManagerService;
    private readonly IDialogService<InputDialog, InputDialogResult> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, LulDialogResult> _messageBoxService;
    private readonly PresetInputDialog _presetInputDialog;
    private readonly RegionDialog _regionInputDialog;
    private Stopwatch _stopWatch = new Stopwatch();

    #endregion "Private Members"

    #region "Properties"

    private WireFrameViewModel ViewModel { get => (WireFrameViewModel)DataContext; }

    #endregion "Properties"

    #region "Contructors"

    public WireFramePage(IPresetListController presetListController
                          , IRegionListController regionListController
                          , ITriggerController triggerController
                          , IScreenCaptureService screenCaptureService
                          , IConfigManagerService configManagerService
                          , IDialogService<InputDialog, InputDialogResult> inputDialog
                          , IDialogService<MessageBoxDialog, LulDialogResult> messageBoxService
                          , PresetInputDialog presetInputDialog
                          , RegionDialog regionDialog)
    {
      InitializeComponent();
      DataContext = new WireFrameViewModel();

      // Dialog Services
      _inputDialog = inputDialog;
      _messageBoxService = messageBoxService;
      _configManagerService = configManagerService;
      _presetInputDialog = presetInputDialog;
      _regionInputDialog = regionDialog;
      InitializeRegionConfigEvents();

      //Controllers
      _presetListController = presetListController;
      _regionListController = regionListController;
      _triggerController = triggerController;
      ViewModel.Presets = new ObservableCollection<PresetViewModel>(_presetListController.GetAllPresets());

      //Worker Initialization
      var captureFps = _configManagerService.GetAsInteger("CaptureFps");
      var workerIdleTimeout = _configManagerService.GetAsInteger("WorkIdleTimeout");
      _soundEffectWorker = new SoundEffectWorker(workerIdleTimeout);
      _screenCaptureWorker = new ScreenCaptureWorker(screenCaptureService, captureFps, workerIdleTimeout);
      _regionWorkerPool = new RegionWorkerPool(_configManagerService.GetAsInteger("MaxRegionThreads"), captureFps, workerIdleTimeout, ViewModel, canvasScreenFeed.RenderSize);

      InitializeWorkers();
      InitializeUserControlEvents();
    }

    #endregion "Contructors"

    #region "Initialization Methods"

    private void InitializeRegionConfigEvents()
    {
      cntrlRegionConfig.BtnAddTrigger.Click += BtnAddTrigger_Click;
      cntrlRegionConfig.BtnDeleteTrigger.Click += BtnDeleteTrigger_Click;
    }

    private void InitializeUserControlEvents()
    {
      LstGamePresets.SelectionChanged += LstGamePresets_SelectionChanged;
      Controls.RegionConfiguration.SaveConfigTriggered += RegionConfiguration_SaveConfigTriggered;
      _screenCaptureWorker.ScreenCaptureCompleted += ViewModel.screenCaptureWorker_ScreenCaptureCompleted;
      CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void InitializeWorkers()
    {
      _screenCaptureWorker.Start();

      TriggerEmitter.TriggerActivated += _soundEffectWorker.triggerWorkerPool_TriggerActivated;
      _regionWorkerPool.Start();
    }

    #endregion "Initialization Methods"

    #region "User Control Events"

    private void LstGamePresets_SelectionChanged(object sender, Controls.IListItem e)
    {
      if (ViewModel?.SelectedPreset != null)
      {
        _screenCaptureWorker.SetGameHandle(HandleFinder.GetWindowsHandle(ViewModel.SelectedPreset.Name));
        _screenCaptureWorker.Start();
        ViewModel.Regions = new ObservableCollection<RegionViewModel>(_regionListController.GetRegions(ViewModel.SelectedPreset.Id));
      }
      else
      {
        _screenCaptureWorker.Stop();
        ViewModel.Regions = null;
      }
    }

    private async void RegionConfiguration_SaveConfigTriggered(object sender, RegionViewModel e)
    {
      await _regionListController.UpdateRegionAsync(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion);
    }

    #endregion "User Control Events"

    #region "Screen Capture Events"

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

    #endregion "Screen Capture Events"

    #region "Mouse Events"

    private void CanvasScreenFeed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released || ViewModel.SelectedRegion == null) return;

        ViewModel.SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseDown(e);
      });
    }

    private void CanvasScreenFeed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(async () =>
      {
        if (e.LeftButton == MouseButtonState.Pressed) return;

        if (ViewModel?.SelectedRegion?.BoundingBox != null)
        {
          await _regionListController.UpdateRegionAsync(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion);
        }
      });
    }

    private void CanvasScreenFeed_MouseMove(object sender, MouseEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released || ViewModel.SelectedRegion == null) return;

        ViewModel.SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseMove(e);
      });
    }

    #endregion "Mouse Events"

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
      if (_messageBoxService.Show("Delete Trigger", "Delete selected trigger?", DialogButtons.YesNo) is LulDialogResult dialogResult
        && dialogResult.DialogResult == DialogResults.Yes)
      {
        _triggerController.DeleteTrigger(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id, ViewModel.SelectedTrigger);
        ViewModel.SelectedRegion.Triggers.Remove(ViewModel.SelectedTrigger);
        ViewModel.SelectedTrigger = null;
      }
    }

    #endregion "Region Config Events"

    #region "Special Controls Events"

    /// <summary>
    /// Used to hook into the render event to only draw region boxes when render happens
    /// </summary>
    private void CompositionTarget_Rendering(object sender, System.EventArgs e)
    {
      DrawSelectedRegion();
    }

    private void LstGamePresets_NewItemClicked(object sender, ButtonClickArgs e)
    {
      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";
      var selectedItem = (PresetViewModel)LstGamePresets.SelectedItem;
      var results = _presetInputDialog.Show(new NestedDialogViewModel<PresetViewModel>(title, message, selectedItem, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newPreset = _presetListController.CreatePreset(results.InnerResults.Name, results.InnerResults.ProcessName);
      ViewModel.Presets.Add(newPreset);
      ViewModel.SelectedPreset = newPreset;
    }

    private void LstGamePresets_DeleteItemClicked(object sender, ButtonClickArgs e)
    {
      if (ShowDeleteCheck(LstGamePresets.ItemDescriptor)?.DialogResult != DialogResults.Yes)
        return;

      _presetListController.DeletePreset(ViewModel.SelectedPreset);
      ViewModel.Presets.Remove(ViewModel.SelectedPreset);
      ViewModel.SelectedPreset = null;
    }

    private void LstGamePresets_EditItemClicked(object sender, ButtonClickArgs e)
    {
      var selectedPreset = ViewModel.SelectedPreset;
      var results = _presetInputDialog.Show(new NestedDialogViewModel<PresetViewModel>("Editing Preset", "Editing Preset: ", selectedPreset, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var existingPresetIndex = ViewModel.Presets.IndexOf(ViewModel.SelectedPreset);
      _presetListController.DeletePreset(ViewModel.SelectedPreset);
      var newPreset = _presetListController.CreatePreset(results.InnerResults.Name, results.InnerResults.ProcessName);
      ViewModel.Presets[existingPresetIndex] = newPreset;
      ViewModel.SelectedPreset = newPreset;
    }

    private void LstScreenRegions_NewItemClicked(object sender, ButtonClickArgs e)
    {
      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";

      var results = _regionInputDialog.Show(new NestedDialogViewModel<RegionViewModel>(title, message, new RegionViewModel(), DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newRegion = _regionListController.CreateRegion(ViewModel.SelectedPreset.Id, results.InnerResults.Name);
      ViewModel.Regions.Add(newRegion);
      ViewModel.SelectedRegion = newRegion;
    }

    private void LstScreenRegions_DeleteItemClicked(object sender, ButtonClickArgs e)
    {
      if (ShowDeleteCheck(LstScreenRegions.ItemDescriptor)?.DialogResult != DialogResults.Yes)
        return;

      _regionListController.DeleteRegion(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id);
      ViewModel.Regions.Remove(ViewModel.SelectedRegion);
      ViewModel.SelectedRegion = null;
    }

    private LulDialogResult ShowDeleteCheck(string itemDescriptor)
    {
      return _messageBoxService.Show($"Delete {itemDescriptor}", $"Do you want to delete selected {itemDescriptor}(s)?", DialogButtons.YesNo);
    }

    private void LstScreenRegions_EditItemClicked(object sender, ButtonClickArgs e)
    {
      var selectedRegion = ViewModel.SelectedRegion;
      var results = _regionInputDialog.Show(new NestedDialogViewModel<RegionViewModel>("Editing Region", "Editing Region: ", selectedRegion, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Yes)
        return;

      var existingRegionIndex = ViewModel.Regions.IndexOf(ViewModel.SelectedRegion);
      _regionListController.DeleteRegion(ViewModel.SelectedPreset.Id, ViewModel.SelectedRegion.Id);
      var newRegion = _regionListController.CreateRegion(ViewModel.SelectedPreset.Id, results.InnerResults.Name);
      ViewModel.Regions[existingRegionIndex] = newRegion;
      ViewModel.SelectedRegion = newRegion;
    }

    #endregion "Special Controls Events"
  }
}