namespace LulCaster.UI.WPF.Dialogs.Models
{
  public class NestedDialogResults<TNestedType> : LulDialogResult
  {
    public TNestedType InnerResults;

    public NestedDialogResults()
    {
    }

    public NestedDialogResults(TNestedType innerItem, DialogResults dialogResults)
    {
      DialogResult = dialogResults;
      InnerResults = innerItem;
    }
  }
}