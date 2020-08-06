using LulCaster.UI.WPF.Controls;
using LulCaster.Utility.Common.Logic;
using System;
using System.Drawing;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionViewModel : ViewModelBase, IListItem
  {
    private Guid _id;
    private LogicSets _logicSet;
    private string _name;
    private string _triggerValue;
    private string _soundFilePath;
    private Rectangle _boundingBox;

    public Guid Id
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
        OnPropertyChanged(nameof(Id));
      }
    }

    public LogicSets LogicSet
    {
      get
      {
        return _logicSet;
      }
      set
      {
        _logicSet = value;
        OnPropertyChanged(nameof(LogicSet));
      }
    }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        _name = value;
        OnPropertyChanged(nameof(Name));
      }
    }

    public string TriggerValue
   {
      get
      {
        return _triggerValue;
      }
      set
      {
        _triggerValue = value;
        OnPropertyChanged(nameof(TriggerValue));
      }
    }
    public string SoundFilePath
    {
      get
      {
        return _soundFilePath;
      }
      set
      {
        _soundFilePath = value;
        OnPropertyChanged(nameof(SoundFilePath));
      }
    }

    public Rectangle BoundingBox
    {
      get
      {
        return _boundingBox;
      }
      set
      {
        _boundingBox = value;
        OnPropertyChanged(nameof(BoundingBox));
      }
    }
  }
}