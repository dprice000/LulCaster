using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Utility;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionConfigViewModel : ViewModelBase
  {
    #region "Private Members"
    private IRegionListController _regionController;
    private ITriggerController _triggerController;
    private TriggerViewModel _selectedTrigger;
    private PresetViewModel _selectedPreset;
    private RegionViewModel _selectedRegion;
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
        OnPropertyChanged(nameof(SelectedPreset));
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
    #endregion

    #region "Commands"
    private DelegateCommand<ButtonClickArgs> _deleteTriggerClick;

    public DelegateCommand<ButtonClickArgs> DeleteTriggerClick
    {
      get
      {
        return _deleteTriggerClick ?? (_deleteTriggerClick = new DelegateCommand<ButtonClickArgs>(DeleteTriggerClicked, (buttonClickArgs) => (SelectedTrigger != null)));
      }
    }

    private DelegateCommand<ButtonClickArgs> _addTriggerClick;

    public DelegateCommand<ButtonClickArgs> AddTriggerClick
    {
      get
      {
        return _addTriggerClick ?? (_addTriggerClick = new DelegateCommand<ButtonClickArgs>(AddTriggerClicked, (buttonClickArgs) => (SelectedRegion != null)));
      }
    }
    #endregion


    public RegionConfigViewModel(IRegionListController regionController, ITriggerController triggerController)
    {
      _regionController = regionController;
      _triggerController = triggerController;
    }

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

    public async void SaveConfigTriggeredClicked(object parameters)
    {
      await _regionController.UpdatAsync(SelectedPreset.Id, SelectedRegion);
    }

    #endregion "Trigger Events"
  }
}