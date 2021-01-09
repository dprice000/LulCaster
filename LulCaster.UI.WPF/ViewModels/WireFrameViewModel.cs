using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.ViewModels
{
  public class WireFrameViewModel : ViewModelBase
  {
    #region "Private Members"

    private readonly IPresetListController _presetController;
    private readonly IRegionListController _regionController;
    private readonly ITriggerController _triggerController;

    private ObservableCollection<PresetViewModel> _presets = new ObservableCollection<PresetViewModel>();
    private PresetViewModel _selectedPreset;
    private ObservableCollection<RegionViewModel> _regions = new ObservableCollection<RegionViewModel>();
    private RegionViewModel _selectedRegion;
    private TriggerViewModel _selectedTrigger;
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

    public ObservableCollection<PresetViewModel> Presets
    {
      get
      {
        return _presets;
      }
      set
      {
        _presets = value ?? new ObservableCollection<PresetViewModel>();
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
        OnPresetSelectionChanged();
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
        _regions = value ?? new ObservableCollection<RegionViewModel>();
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

    #endregion "Properties"

    #region "Commands"

    private ICommand _saveTriggerClicked;

    public ICommand SaveTriggerClicked
    {
      get
      {
        return _saveTriggerClicked ?? (_saveTriggerClicked = new Command((obj) => SaveConfigTriggeredClicked(obj), () => (SelectedTrigger != null)));
      }
    }

    private ICommand _canvasLeftMouseUp;

    public ICommand CanvasLeftMouseUp
    {
      get
      {
        return _canvasLeftMouseUp ?? (_canvasLeftMouseUp = new Command(CanvasMouseLeftButtonUp, () => (SelectedRegion != null)));
      }
    }

    private ICommand _canvasLeftMouseDown;

    public ICommand CanvasLeftMouseDown
    {
      get
      {
        return _canvasLeftMouseDown ?? (_canvasLeftMouseDown = new Command(CanvasMouseLeftButtonDown, () => (SelectedRegion != null)));
      }
    }

    private ICommand _canvasMouseMove;

    public ICommand CanvasMouseMove
    {
      get
      {
        return _canvasMouseMove ?? (_canvasMouseMove = new Command(CanvasMouseMoveOccured, () => (SelectedRegion != null)));
      }
    }

    private ICommand _deleteTriggerClick;

    public ICommand DeleteTriggerClick
    {
      get
      {
        return _deleteTriggerClick ?? (_deleteTriggerClick = new Command(DeleteTriggerClicked, () => (SelectedTrigger != null)));
      }
    }

    private ICommand _addTriggerClick;

    public ICommand AddTriggerClick
    {
      get
      {
        return _addTriggerClick ?? (_addTriggerClick = new Command(AddTriggerClicked, () => (SelectedRegion != null)));
      }
    }

    #endregion "Commands"

    public WireFrameViewModel(IPresetListController presetController
      , IRegionListController regionController
      , ITriggerController triggerController)
    {
      _presetController = presetController;
      _regionController = regionController;
      _triggerController = triggerController;

      InitializePresetList();
    }

    private void InitializePresetList()
    {
      var presetTask = _presetController.GetAllAsync();
      presetTask.Wait();
      Presets = new ObservableCollection<PresetViewModel>(presetTask.Result);
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

    private async void SaveConfigTriggeredClicked(object parameters)
    {
      await _regionController.UpdatAsync(SelectedPreset.Id, SelectedRegion);
    }

    private void OnPresetSelectionChanged()
    {
      Regions = (SelectedPreset != null) ? new ObservableCollection<RegionViewModel>(_regionController.GetRegions(SelectedPreset.Id)) : null;
    }

    #region "Canvas Mouse Events"

    private async void CanvasMouseLeftButtonUp(object args)
    {
      var e = (MouseButtonEventArgs)args;

      if (e.LeftButton == MouseButtonState.Pressed)
        return;

      if (SelectedRegion?.BoundingBox != null)
      {
        await _regionController.UpdatAsync(SelectedPreset.Id, SelectedRegion);
      }
    }

    private void CanvasMouseLeftButtonDown(object args)
    {
      var e = (MouseButtonEventArgs)args;

      if (e.LeftButton == MouseButtonState.Released || SelectedRegion == null)
        return;

      SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseDown(e);
    }

    private void CanvasMouseMoveOccured(object args)
    {
      var e = (MouseButtonEventArgs)args;

      if (e.LeftButton == MouseButtonState.Released || SelectedRegion == null)
        return;

      SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseMove(e);
    }

    #endregion "Canvas Mouse Events"

    #region "Trigger Events"

    private void AddTriggerClicked(object args)
    {
      
      if (InputDialogProvider.Show("New Trigger", "New Trigger Name:", DialogButtons.OkCancel) is InputDialogResult dialogResult && dialogResult.DialogResult == DialogResults.Ok)
      {
        var newTrigger = _triggerController.CreateTrigger(SelectedPreset.Id, SelectedRegion.Id, dialogResult.Input);

        SelectedRegion.Triggers.Add(newTrigger);
        SelectedTrigger = newTrigger;
      }
    }

    private void DeleteTriggerClicked(object args)
    {
      if (MessageBoxProvider.Show("Delete Trigger", "Delete selected trigger?", DialogButtons.YesNo) is LulDialogResult dialogResult
        && dialogResult.DialogResult == DialogResults.Yes)
      {
        _triggerController.DeleteTrigger(SelectedPreset.Id, SelectedRegion.Id, SelectedTrigger);
        SelectedRegion.Triggers.Remove(SelectedTrigger);
        SelectedTrigger = null;
      }
    }

    #endregion "Trigger Events"

    #region "Preset Control Events"
    private async void NewItemClicked(object args)
    {
      var e = (ButtonClickArgs)args;

      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";
      var selectedItem = SelectedPreset;
      var results = CrudDialogProvider.PresetModal(new NestedDialogViewModel<PresetViewModel>(title, message, selectedItem, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newPreset = await _presetController.CreateAsync(results.InnerResults.Name, results.InnerResults.ProcessName);
      Presets.Add(newPreset);
      SelectedPreset = newPreset;
    }

    private async void LstGamePresets_DeleteItemClicked(object sender, ButtonClickArgs e)
    {
      if (MessageBoxProvider.ShowDeleteDialog("Preset")?.DialogResult != DialogResults.Yes)
        return;

      await _presetController.DeleteAsync(SelectedPreset);
      Presets.Remove(SelectedPreset);
      SelectedPreset = null;
    }

    private async void LstGamePresets_EditItemClicked(object sender, ButtonClickArgs e)
    {
      var results = CrudDialogProvider.PresetModal(new NestedDialogViewModel<PresetViewModel>("Editing Preset", "Editing Preset: ", SelectedPreset, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var existingPresetIndex = Presets.IndexOf(SelectedPreset);
      await _presetController.DeleteAsync(SelectedPreset);
      var newPreset = await _presetController.CreateAsync(results.InnerResults.Name, results.InnerResults.ProcessName);
      Presets[existingPresetIndex] = newPreset;
      SelectedPreset = newPreset;
    }
    #endregion

    #region "Region Control Events"
    private void LstScreenRegions_NewItemClicked(object sender, ButtonClickArgs e)
    {
      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";

      var results = CrudDialogProvider.RegionModal(new NestedDialogViewModel<RegionViewModel>(title, message, new RegionViewModel(), DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newRegion = _regionController.Create(SelectedPreset.Id, results.InnerResults.Name);
      Regions.Add(newRegion);
      SelectedRegion = newRegion;
    }

    private void LstScreenRegions_DeleteItemClicked(object sender, ButtonClickArgs e)
    {
      if (MessageBoxProvider.ShowDeleteDialog("Region")?.DialogResult != DialogResults.Yes)
        return;

      _regionController.Delete(SelectedPreset.Id, SelectedRegion.Id);
      Regions.Remove(SelectedRegion);
      SelectedRegion = null;
    }

    private void LstScreenRegions_EditItemClicked(object sender, ButtonClickArgs e)
    {
      var selectedRegion = SelectedRegion;
      var results = CrudDialogProvider.RegionModal(new NestedDialogViewModel<RegionViewModel>("Editing Region", "Editing Region: ", selectedRegion, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Yes)
        return;

      var existingRegionIndex = Regions.IndexOf(SelectedRegion);
      _regionController.Delete(SelectedPreset.Id, SelectedRegion.Id);
      var newRegion = _regionController.Create(SelectedPreset.Id, results.InnerResults.Name);
      Regions[existingRegionIndex] = newRegion;
      SelectedRegion = newRegion;
    }
    #endregion
  }
}