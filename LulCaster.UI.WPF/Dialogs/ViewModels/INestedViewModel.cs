namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public interface INestedViewModel<TNestedType> : IDialogViewModelBase
  {
    TNestedType InnerItem { get; set; }
  }
}