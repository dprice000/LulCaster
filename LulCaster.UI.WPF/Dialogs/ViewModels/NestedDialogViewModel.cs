namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class NestedDialogViewModel<TNestedType> : DialogViewModelBase
  {
    public TNestedType InnerItem { get; set; }

    public NestedDialogViewModel(string title, string message, TNestedType innerItem, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
      InnerItem = innerItem;
    }
  }
}