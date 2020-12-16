using System;
using System.Configuration;

namespace LulCaster.UI.WPF.Config
{
  public class ConfigManagerService : IConfigManagerService
  {
    public string this[string name]
    {
      get
      {
        return ConfigurationManager.AppSettings[name];
      }
    }

    public string this[int index]
    {
      get
      {
        return ConfigurationManager.AppSettings[index];
      }
    }

    public int GetAsInteger(string name)
    {
      return Convert.ToInt32(ConfigurationManager.AppSettings[name]);
    }
  }
}