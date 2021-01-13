namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public interface IDialogViewModelBase
  {
    string Message { get; set; }
    DialogButtons MessageBoxButtons { get; set; }
    string Title { get; set; }
  }
}