using LulCaster.UI.WPF.ViewModels;
using System;

namespace LulCaster.UI.WPF.Controllers
{
  public interface ITriggerController
  {
    TriggerViewModel CreateTrigger(Guid presetId, Guid regionId, string name);
    void DeleteTrigger(Guid presetId, Guid regionId, TriggerViewModel triggerViewModel);
  }
}