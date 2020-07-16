using AutoMapper;
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
      var section = _config.GetSection($"{PRESETS_KEY}");
      throw new NotImplementedException();
    }

    public void DeletePreset(PresetViewModel preset)
    {
      var presetList = JsonConvert.DeserializeObject<List<PresetViewModel>>(_config[$"{PRESETS_KEY}"]);
      presetList.Remove(preset);
      _config[$"{PRESETS_KEY}"] = JsonConvert.SerializeObject(presetList);
    }
  }
}
