using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    Task<PresetViewModel> CreateAsync(string name, string processName);
    Task DeleteAsync(PresetViewModel preset);
    Task<IEnumerable<PresetViewModel>> GetAllAsync();
  }
}