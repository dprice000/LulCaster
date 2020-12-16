using AutoMapper;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Config.UserSettings
{
  // TODO: This class is super inefficient but I just wanted to get it working.
  public class RegionConfigService : IRegionConfigService
  {
    private readonly IMapper _mapper;

    public RegionConfigService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public RegionViewModel CreateRegion(Guid presetId, string regionName)
    {
      var newRegion = new RegionConfig
      {
        Id = Guid.NewGuid(),
        Name = regionName
      };

      var regions = GetAllRegions(presetId).ToList();
      regions.Add(newRegion);
      File.WriteAllText(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));

      return _mapper.Map<RegionViewModel>(newRegion);
    }

    public void WriteAllRegions(string filePath, IEnumerable<RegionViewModel> regions)
    {
      File.WriteAllText(filePath, JsonConvert.SerializeObject(_mapper.Map<IEnumerable<RegionConfig>>(regions)));
    }

    public IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(Guid presetId)
    {
      return _mapper.Map<IEnumerable<RegionViewModel>>(GetAllRegions(presetId));
    }

    public IEnumerable<RegionConfig> GetAllRegions(Guid presetId)
    {
      return JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(File.ReadAllText(PresetFile.ResolvePresetFilePath(presetId)));
    }

    public IEnumerable<RegionViewModel> GetAllRegions(string importFilePath)
    {
      var regionConfigs = JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(File.ReadAllText(importFilePath));
      return _mapper.Map<IEnumerable<RegionViewModel>>(regionConfigs);
    }

    public async Task<IEnumerable<RegionConfig>> GetAllRegionsAsync(Guid presetId)
    {
      var contents = await File.ReadAllTextAsync(PresetFile.ResolvePresetFilePath(presetId));
      return JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(contents);
    }

    public RegionViewModel GetRegion(Guid presetId, Guid id)
    {
      var regions = GetAllRegions(presetId);

      return _mapper.Map<RegionViewModel>(regions.FirstOrDefault(x => x.Id == id));
    }

    public void UpdateRegion(Guid presetId, RegionViewModel regionViewModel)
    {
      var regions = GetAllRegions(presetId).ToList();
      var region = regions.FirstOrDefault(x => x.Id == regionViewModel.Id);
      region = _mapper.Map<RegionConfig>(regionViewModel);

      File.WriteAllText(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));
    }

    public async Task UpdateRegionAsync(Guid presetId, RegionViewModel regionViewModel)
    {
      var regions = (await GetAllRegionsAsync(presetId)).ToList();
      var index = regions.IndexOf(regions.First(x => x.Id == regionViewModel.Id));
      regions[index] = _mapper.Map<RegionConfig>(regionViewModel);

      await File.WriteAllTextAsync(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));
    }

    public void DeleteRegion(Guid presetId, Guid regionId)
    {
      var regions = GetAllRegions(presetId).ToList();
      var region = regions.RemoveAll(x => x.Id == regionId);

      File.WriteAllText(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));
    }
  }
}