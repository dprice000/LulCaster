using LulCaster.Utility.Common.Logic;
using System;
using System.Drawing;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionViewModel : ViewModelBase
  {
    private Guid _id;
    private LogicSets _logicSet;
    private string _label;
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

    public string Label
    {
      get
      {
        return _label;
      }
      set
      {
        _label = value;
        OnPropertyChanged(nameof(Label));
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