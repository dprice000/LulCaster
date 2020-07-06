namespace LulCaster.UI.WPF.Dialogs
{
  public interface ISimpleDialogService<TReturnType>
  {
    TReturnType ReturnValue { get; }
    bool? ShowDialog();
  }
}