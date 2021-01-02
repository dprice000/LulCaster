using LulCaster.Utility.Common.Enums;

namespace LulCaster.UI.WPF.Dialogs.Models
{
  public class RegionDialogResults : LulDialogResult
  {
    public string Name { get; set; }
    public RegionTypes Type { get; set; }
  }
}