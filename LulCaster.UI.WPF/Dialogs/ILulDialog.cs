using LulCaster.UI.WPF.Dialogs.Models;

namespace LulCaster.UI.WPF.Dialogs
{
  public interface ILulDialog
  {
    DialogResults DialogResult { get; set; }
    LulDialogResult Show(string title, string message, DialogButtons dialogButtons);
  }
}