namespace LulCaster.UI.WPF.Dialogs
{
  public interface ILulDialog
  {
    object Show(string title, string message, DialogButtons dialogButtons);
  }
}