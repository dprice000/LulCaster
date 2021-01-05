using System;
using System.IO;

namespace LulCaster.UI.WPF.Utility
{
  public class PresetFile
  {
    private const string _directoryBase = "Presets";

    public static string ListingFilePath => Path.Combine(_directoryBase, "PresetListing.json");

    public static string ResolvePresetFilePath(Guid presetId)
    {
      return Path.Combine(_directoryBase ,$"{presetId}.json");
    }
  }
}