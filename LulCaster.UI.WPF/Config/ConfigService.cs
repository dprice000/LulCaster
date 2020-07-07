using AutoMapper;
using LulCaster.UI.WPF.Extensions;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.UI.WPF.Config
{
  public class ConfigService : IConfigService
  {
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private const string REGIONS_KEY = "regions";
    private const string PRESETS_KEY = "presets";

    public ConfigService(IConfiguration config, IMapper mapper)
    {
      _config = config;
      _mapper = mapper;
    }

    public void CreateRegionConfig(string preset, RegionConfig regionConfig)
    {
      var regions = GetAllRegions(preset).ToList();
      regions.Add(regionConfig);
      _config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"] = JsonConvert.SerializeObject(regions);
    }

    public IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(string preset)
    {
      var regionConfigs = JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(_config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"]);

      return _mapper.Map<IEnumerable<RegionViewModel>>(regionConfigs);
    }

    public IEnumerable<RegionConfig> GetAllRegions(string preset)
    {
      var regionConfigs = JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(_config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"]);

      return regionConfigs;
    }

    public RegionViewModel GetRegion(string preset, Guid id)
    {
      var regionConfig = JsonConvert.DeserializeObject<List<RegionConfig>>(_config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"]).FirstOrDefault(x => x.Id == id);

      return _mapper.Map<RegionViewModel>(regionConfig);
    }

    public void UpdateRegion(string preset, RegionViewModel regionViewModel)
    {
      var regionConfig = _mapper.Map<RegionConfig>(regionViewModel);
      regionConfig.BoundingBoxDimensions = regionViewModel.BoundingBoxToString();

      var regionList = GetAllRegions(preset).ToList();
      var configItem = regionList.First(x => x.Id == regionViewModel.Id);
      configItem = regionConfig;

      _config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"] = JsonConvert.SerializeObject(regionList);
    }

    public void DeleteRegion(string preset, Guid regionId)
    {
      var regions = GetAllRegions(preset).ToList();
      regions.RemoveAll(x => x.Id == regionId);
      _config[$"{PRESETS_KEY}:{preset}:{REGIONS_KEY}"] = JsonConvert.SerializeObject(regions);
    }

    public void CreatePreset(string preset)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> GetAllPresets()
    {
      return new List<string>
      {
        "One",
        "Two",
        "Three"
      };
    }

    public void DeletePreset(string preset)
    {
      throw new NotImplementedException();
    }
  }
}