using LulCaster.UI.WPF.Config.UserSettings;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IPresetConfigService _presetConfigService;

    public PresetListController(IPresetConfigService configService)
    {
      _presetConfigService = configService;
    }

    public async Task<PresetViewModel> CreateAsync(string name, string processName)
    {
      return await _presetConfigService.CreateAsync(name, processName);
    }

    public async Task<IEnumerable<PresetViewModel>> GetAllAsync()
    {
      return await _presetConfigService.GetAllAsync();
    }

    public async Task UpdateAsync(PresetViewModel preset)
    {
      await _presetConfigService.UpdateAsync(preset);
    }

    public async Task DeleteAsync(PresetViewModel preset)
    {
      await _presetConfigService.DeleteAsync(preset);
    }
  }
}