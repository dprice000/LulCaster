using LulCaster.UI.WPF.Config.UserSettings;
using LulCaster.UI.WPF.ViewModels;
using System;

namespace LulCaster.UI.WPF.Controllers
{
  public class TriggerController : ITriggerController
  {
    private readonly IRegionConfigService _regionConfigService;

    public TriggerController(IRegionConfigService regionConfigService)
    {
      _regionConfigService = regionConfigService;
    }

    public TriggerViewModel CreateTrigger(Guid presetId, Guid regionId, string name)
    {
      var newTrigger = new TriggerViewModel()
      {
        Name = name
      };

      var existingRegion = _regionConfigService.Get(presetId, regionId);
      existingRegion.Triggers.Add(newTrigger);
      _regionConfigService.Update(presetId, existingRegion);

      return newTrigger;
    }

    public void DeleteTrigger(Guid presetId, Guid regionId, TriggerViewModel triggerViewModel)
    {
      var existingRegion = _regionConfigService.Get(presetId, regionId);
      existingRegion.Triggers.Remove(triggerViewModel);
      _regionConfigService.Update(presetId, existingRegion);
    }
  }
}