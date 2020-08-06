using LulCaster.UI.WPF.Controls;
using System;

namespace LulCaster.UI.WPF.ViewModels
{
  public class PresetViewModel : IListItem
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FilePath { get; set; }
  }
}