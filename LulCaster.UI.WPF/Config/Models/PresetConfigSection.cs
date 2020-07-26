using System.Configuration;

namespace LulCaster.UI.WPF.Config.Models
{
  public class PresetConfigSection : ConfigurationSection
  {
    public static PresetConfigSection GetConfig()
    {
      return (PresetConfigSection)ConfigurationManager.GetSection("PresetConfigSection") ?? new PresetConfigSection();
    }

    [ConfigurationProperty("Presets")]
    [ConfigurationCollection(typeof(PresetConfigList))]
    public PresetConfigList Presets
    {
      get { return (PresetConfigList)this["Presets"]; }
    }
  }
}