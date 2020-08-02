using AutoMapper;
using LulCaster.UI.WPF.Config.Models;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public class PresetConfigService : IPresetConfigService
  {
    private readonly IMapper _mapper;

    public PresetConfigService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public PresetViewModel CreatePreset(string name)
    {
      var id = Guid.NewGuid();
      var newPreset = new PresetViewModel()
      {
        Id = id,
        Name = name,
        FilePath = $"{id}.json"
      };

      var presetConfig = _mapper.Map<PresetConfig>(newPreset);
      var presetSection = PresetConfigSection.GetConfig();

      if (presetSection != null)
      {
        presetSection.Presets.Add(presetConfig);
      }

      return newPreset;
    }

    public IEnumerable<PresetViewModel> GetAllPresets()
    {
      var presets = new List<PresetViewModel>();
      var presetSection = PresetConfigSection.GetConfig();

      if (presetSection != null)
      {
        foreach (var preset in presetSection.Presets)
        {
          presets.Add(_mapper.Map<PresetViewModel>(preset));
        }
      }

      return presets;
    }

    public void DeletePreset(PresetViewModel preset)
    {
      var presetSection = PresetConfigSection.GetConfig();
      presetSection.Presets.Remove(preset.Id);
    }
  }
}