namespace LulCaster.UI.WPF.Controls.EventArgs
{
  public class ButtonClickArgs
  {
    public string Action { get; set; }
    public string ItemDescriptor { get; set; }

    public ButtonClickArgs(string action, string itemDescriptor)
    {
      Action = action;
      ItemDescriptor = itemDescriptor;
    }
  }
}