namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class NestedViewModel<TNestedType> : DialogViewModelBase, INestedViewModel<TNestedType>
  {
    public TNestedType InnerItem { get; set; }

    public NestedViewModel(string title, string message, TNestedType innerItem, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
      InnerItem = innerItem;
    }
  }
}