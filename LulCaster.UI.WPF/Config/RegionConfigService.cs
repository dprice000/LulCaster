using AutoMapper;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
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

    public RegionConfigService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public RegionViewModel CreateRegion(Guid presetId, string regionName)
    {
      var newRegion = new RegionConfig
      {
        Id = Guid.NewGuid(),
        Label = regionName
      };

      var regions = GetAllRegions(presetId).ToList();
      regions.Add(newRegion);
      File.WriteAllText(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));

      return _mapper.Map<RegionViewModel>(newRegion);
    }

    public IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(Guid presetId)
    {
      return _mapper.Map<IEnumerable<RegionViewModel>>(GetAllRegions(presetId));
    }

    public IEnumerable<RegionConfig> GetAllRegions(Guid presetId)
    {
      return JsonConvert.DeserializeObject<IEnumerable<RegionConfig>>(File.ReadAllText(PresetFile.ResolvePresetFilePath(presetId)));
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

    public void DeleteRegion(Guid presetId, Guid regionId)
    {
      var regions = GetAllRegions(presetId).ToList();
      var region = regions.RemoveAll(x => x.Id == regionId);

      File.WriteAllText(PresetFile.ResolvePresetFilePath(presetId), JsonConvert.SerializeObject(regions));
    }
  }
}