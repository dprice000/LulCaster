using AutoMapper;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LulCaster.UI.WPF.Config
{
  // TODO: This class is super inefficient but I just wanted to get it working.
  public class RegionConfigService : IRegionConfigService
  {
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private const string REGIONS_KEY = "regions";
    private const string PRESETS_KEY = "presets";

    public RegionConfigService(IConfiguration config, IMapper mapper)
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

    public IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(string presetFilePath)
    {
      return _mapper.Map<IEnumerable<RegionViewModel>>(GetAllRegions(presetFilePath));
    }

    public IEnumerable<RegionConfig> GetAllRegions(string presetFilePath)
    {
      return JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(File.ReadAllText(presetFilePath));
    }

    public RegionViewModel GetRegion(string presetFilePath, Guid id)
    {
      var regions = GetAllRegions(presetFilePath);

      return _mapper.Map<RegionViewModel>(regions.FirstOrDefault(x => x.Id == id));
    }

    public void UpdateRegion(string presetFilePath, RegionViewModel regionViewModel)
    {
      var regions = GetAllRegions(presetFilePath).ToList();
      var region = regions.FirstOrDefault(x => x.Id == regionViewModel.Id);
      region = _mapper.Map<RegionConfig>(regionViewModel);

      File.WriteAllText(presetFilePath, JsonConvert.SerializeObject(regions));
    }

    public void DeleteRegion(string presetFilePath, Guid regionId)
    {
      var regions = GetAllRegions(presetFilePath).ToList();
      var region = regions.RemoveAll(x => x.Id == regionId);

      File.WriteAllText(presetFilePath, JsonConvert.SerializeObject(regions));
    }
  }
}