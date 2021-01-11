using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionConfigViewModel : ViewModelBase
  {
    private IRegionListController _regionController;
    private ITriggerController _triggerController;
    private TriggerViewModel _selectedTrigger;

    #region "Properties"
    public PresetViewModel SelectedPreset { get; set; }
    public RegionViewModel SelectedRegion { get; set; }

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