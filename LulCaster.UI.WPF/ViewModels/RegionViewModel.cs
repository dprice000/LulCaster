using LulCaster.UI.WPF.Controls;
using LulCaster.Utility.Common.Logic;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionViewModel : ViewModelBase, IListItem
  {
    private Guid _id;
    private TriggerTypes _logicSet;
    private string _name;
    private Rectangle _boundingBox;
    private ObservableCollection<TriggerViewModel> _triggers;

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

    public TriggerTypes LogicSet
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

    public ObservableCollection<TriggerViewModel> Triggers
    {
      get
      {
        return _triggers;
      }
      set
      {
        _triggers = value;
        OnPropertyChanged(nameof(Triggers));
      }
    }
  }
}