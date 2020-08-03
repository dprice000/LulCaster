using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IRegionListController
  {
    RegionViewModel CreateRegion(Guid presetId, string regionName);
    void DeleteRegion(Guid presetId, Guid regionId);
    string ShowNewRegionDialog();
    public DialogResults ShowMessageBox(string title, string message, DialogButtons dialogButtons);
    IEnumerable<RegionViewModel> GetRegions(Guid presetId);
  }
}