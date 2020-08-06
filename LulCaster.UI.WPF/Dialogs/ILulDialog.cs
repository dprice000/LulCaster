using LulCaster.UI.WPF.Dialogs.Models;

namespace LulCaster.UI.WPF.Dialogs
{
  public interface ILulDialog
  {
    DialogResults DialogResult { get; set; }
    DialogResult Show(string title, string message, DialogButtons dialogButtons);
  }
}