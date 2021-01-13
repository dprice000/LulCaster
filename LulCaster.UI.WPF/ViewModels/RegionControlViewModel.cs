using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.Utility;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionControlViewModel : ViewModelBase
  {
    #region "Private Members"
    private readonly IRegionListController _regionController;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private PresetViewModel _selectedPreset;

    private RegionViewModel _selectedRegion;
    private ObservableCollection<RegionViewModel> _regions = new ObservableCollection<RegionViewModel>();
    #endregion "Private Members"

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
        Regions = (_selectedPreset != null) ? new ObservableCollection<RegionViewModel>(_regionController.GetRegions(_selectedPreset.Id)) : null;
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
    #endregion "Properties"

    #region "Commands"
    private DelegateCommand<ButtonClickArgs> _canvasLeftMouseUp;

    public DelegateCommand<ButtonClickArgs> CanvasLeftMouseUp
    {
      get
      {
        return _canvasLeftMouseUp ?? (_canvasLeftMouseUp = new DelegateCommand<ButtonClickArgs>(CanvasMouseLeftButtonUpOccured, (buttonClickArgs) => (SelectedRegion != null)));
      }
    }

    private DelegateCommand<ButtonClickArgs> _canvasLeftMouseDown;

    public DelegateCommand<ButtonClickArgs> CanvasLeftMouseDown
    {
      get
      {
        return _canvasLeftMouseDown ?? (_canvasLeftMouseDown = new DelegateCommand<ButtonClickArgs>(CanvasMouseLeftButtonDownOccured, (buttonClickArgs) => (SelectedRegion != null)));
      }
    }

    private DelegateCommand<ButtonClickArgs> _canvasMouseMove;

    public DelegateCommand<ButtonClickArgs> CanvasMouseMove
    {
      get
      {
        return _canvasMouseMove ?? (_canvasMouseMove = new DelegateCommand<ButtonClickArgs>(CanvasMouseMoveOccured, (buttonClickArgs) => (SelectedRegion != null)));
      }
    }
    #endregion

    public RegionControlViewModel(IRegionListController regionController)
    {
      _regionController = regionController;
    }

    #region "Control Events"

    public void NewItemClicked(ButtonClickArgs e)
    {
      var title = $"{e.Action} {e.ItemDescriptor}";
      var message = $"{e.Action} {e.ItemDescriptor}: ";

      var results = CrudDialogProvider.Show<RegionViewModel>(new NestedViewModel<RegionViewModel>(title, message, new RegionViewModel(), DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Ok)
        return;

      var newRegion = _regionController.Create(SelectedPreset.Id, results.InnerResults.Name);
      Regions.Add(newRegion);
      SelectedRegion = newRegion;
    }

    public void DeleteItemClicked(object sender, ButtonClickArgs e)
    {
      if (MessageBoxProvider.ShowDeleteDialog("Region")?.DialogResult != DialogResults.Yes)
        return;

      _regionController.Delete(SelectedPreset.Id, SelectedRegion.Id);
      Regions.Remove(SelectedRegion);
      SelectedRegion = null;
    }

    public void EditItemClicked(object sender, ButtonClickArgs e)
    {
      var selectedRegion = SelectedRegion;
      var results = CrudDialogProvider.Show(new NestedViewModel<RegionViewModel>("Editing Region", "Editing Region: ", selectedRegion, DialogButtons.OkCancel));

      if (results.DialogResult != DialogResults.Yes)
        return;

      var existingRegionIndex = Regions.IndexOf(SelectedRegion);
      _regionController.Delete(SelectedPreset.Id, SelectedRegion.Id);
      var newRegion = _regionController.Create(SelectedPreset.Id, results.InnerResults.Name);
      Regions[existingRegionIndex] = newRegion;
      SelectedRegion = newRegion;
    }

    public async Task SaveTriggerConfigAsnyc()
    {
      await _regionController.UpdatAsync(SelectedPreset.Id, SelectedRegion);
    }
    #endregion

    #region "Canvas Events"
    public async void CanvasMouseLeftButtonUpOccured(object e)
    {
      var args = (MouseButtonEventArgs)e;

      if (args.LeftButton == MouseButtonState.Pressed)
        return;

      if (SelectedRegion?.BoundingBox != null)
      {
        await _regionController.UpdatAsync(SelectedPreset.Id, SelectedRegion);
      }
    }

    public void CanvasMouseLeftButtonDownOccured(object e)
    {
      var args = (MouseButtonEventArgs)e;

      if (args.LeftButton == MouseButtonState.Released || SelectedRegion == null)
        return;

      SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseDown(args);
    }

    public void CanvasMouseMoveOccured(object e)
    {
      var args = (MouseEventArgs)e;

      if (args.LeftButton == MouseButtonState.Released || SelectedRegion == null)
        return;

      SelectedRegion.BoundingBox = _boundingBoxBrush.OnMouseMove(args);
    }
    #endregion
  }
}
