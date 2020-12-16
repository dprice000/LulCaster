using System;
using System.Configuration;

namespace LulCaster.UI.WPF.Config.UserSettings.Models
{
  public class PresetConfig : ConfigurationElement
  {
    [ConfigurationProperty("Id", IsRequired = true, IsKey = true)]
    public Guid Id 
    {
      get { return (Guid)this["Id"]; }
      set { this["Id"] = value; }
    }
    [ConfigurationProperty("Name", IsRequired = true)]
    public string Name 
    {
      get { return (string)this["Name"]; }
      set { this["Name"] = value; }
    }
  }
}