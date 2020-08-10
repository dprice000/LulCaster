using System.Collections.ObjectModel;

namespace LulCaster.UI.WPF.ViewModels
{
  public class WireFrameViewModel : ViewModelBase
  {
    private ObservableCollection<PresetViewModel> _presets;
    private PresetViewModel _selectedPreset;
    private ObservableCollection<RegionViewModel> _regions;
    private RegionViewModel _selectedRegion;
    private TriggerViewModel _selectedTrigger;

    public ObservableCollection<PresetViewModel> Presets 
    {
      get
      {
        return _presets;
      }
      set
      {
        _presets = value;
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
  }
}