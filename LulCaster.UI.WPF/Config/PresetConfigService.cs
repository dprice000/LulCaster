using AutoMapper;
using LulCaster.UI.WPF.Config.Models;
using LulCaster.UI.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public class PresetConfigService : IPresetConfigService
  {
    private const string PRESETS_KEY = "presets";

    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PresetConfigService(IConfiguration config, IMapper mapper)
    {
      _config = config;
      _mapper = mapper;
    }

    public PresetViewModel CreatePreset(string name)
    {
      var presetList = JsonConvert.DeserializeObject<List<PresetViewModel>>(_config[$"{PRESETS_KEY}"]);
      var id = Guid.NewGuid();
      var newPreset = new PresetViewModel()
      {
        Id = id,
        Name = name,
        FilePath = $"{id}.json"
      };
      presetList.Add(newPreset);
      _config[$"{PRESETS_KEY}"] = JsonConvert.SerializeObject(presetList);

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
