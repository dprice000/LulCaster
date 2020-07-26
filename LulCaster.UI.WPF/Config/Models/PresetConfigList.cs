using System;
using System.Configuration;

namespace LulCaster.UI.WPF.Config.Models
{
  public class PresetConfigList : ConfigurationElementCollection
  {
    public PresetConfig this[object key]
    {
      get
      {
        return base.BaseGet(key) as PresetConfig;
      }
      set
      {
        if (base.BaseGet(key) != null)
        {
          base.BaseRemove(key);
        }
        this.BaseAdd(value);
      }
    }

    public void Add(PresetConfig preset)
    {
      BaseAdd(preset);
    }

    public void Remove(Guid presetId)
    {
      BaseRemove(presetId);
    }

    public new PresetConfig this[string responseString]
    {
      get { return (PresetConfig)BaseGet(responseString); }
      set
      {
        if (BaseGet(responseString) != null)
        {
          BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
        }
        BaseAdd(value);
      }
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return new PresetConfig();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((PresetConfig)element).Id;
    }
  }
}