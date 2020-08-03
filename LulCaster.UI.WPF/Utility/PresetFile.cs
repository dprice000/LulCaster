using System;

namespace LulCaster.UI.WPF.Utility
{
  public class PresetFile
  {
    public static string ResolvePresetFilePath(Guid presetId)
    {
      return $@"Presets\{presetId}.json";
    }
  }
}