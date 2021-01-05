using AutoMapper;
using LulCaster.UI.WPF.Config.UserSettings.Models;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Config.UserSettings
{
  public class PresetConfigService : IPresetConfigService
  {
    private readonly IMapper _mapper;

    public PresetConfigService(IMapper mapper)
    {
      _mapper = mapper;
    }

    private async Task<IEnumerable<PresetConfig>> GetAllConfigsAsync()
    {
      var fileContents = await File.ReadAllTextAsync(PresetFile.ListingFilePath);
      return JsonConvert.DeserializeObject<IEnumerable<PresetConfig>>(fileContents).ToList();
    }

    public async Task<IEnumerable<PresetViewModel>> GetAllAsync()
    {
      var presets = new List<PresetViewModel>();
      var fileContents = await File.ReadAllTextAsync(PresetFile.ListingFilePath);
      var presetSection = JsonConvert.DeserializeObject<IEnumerable<PresetConfig>>(fileContents).ToList();

      if (presetSection != null)
      {
        foreach (var preset in presetSection)
        {
          presets.Add(_mapper.Map<PresetViewModel>(preset));
        }
      }

      return presets;
    }

    public async Task<PresetViewModel> CreateAsync(string name, string processName)
    {
      var id = Guid.NewGuid();
      var newPreset = new PresetViewModel()
      {
        Id = id,
        Name = name,
        FilePath = $"{id}.json",
        ProcessName = processName
      };

      var presetSection = (await GetAllConfigsAsync()).ToList();
      presetSection.Add(_mapper.Map<PresetConfig>(newPreset));
      await File.WriteAllTextAsync(PresetFile.ListingFilePath, JsonConvert.SerializeObject(presetSection));

      return newPreset;
    }

    public async Task UpdateAsync(PresetViewModel preset)
    {
      var presetConfig = _mapper.Map<PresetConfig>(preset);
      var presetSection = (await GetAllConfigsAsync()).ToList();
      var presetIndex = presetSection.FindIndex(x => x.Id == presetConfig.Id);
      presetSection[presetIndex] = presetConfig;
      await File.WriteAllTextAsync(PresetFile.ListingFilePath, JsonConvert.SerializeObject(presetSection));
    }

    public async Task DeleteAsync(PresetViewModel preset)
    {
      var presetSection = (await GetAllConfigsAsync()).ToList();
      presetSection.RemoveAll(x => x.Id == preset.Id);
      await File.WriteAllTextAsync(PresetFile.ListingFilePath, JsonConvert.SerializeObject(presetSection));
    }
  }
}