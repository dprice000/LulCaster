using LulCaster.UI.WPF.Controls;
using System;

namespace LulCaster.UI.WPF.ViewModels
{
  public class PresetViewModel : ViewModelBase, IListItem
  {
    private Guid _id;
    private string _name;
    private string _filePath;

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
    public string FilePath
    {
      get
      {
        return _filePath;
      }
      set
      {
        _filePath = value;
        OnPropertyChanged(nameof(FilePath));
      }
    }
  }
}