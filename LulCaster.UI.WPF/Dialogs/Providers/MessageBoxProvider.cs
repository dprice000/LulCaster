using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;

namespace LulCaster.UI.WPF.Dialogs.Providers
{
  public class MessageBoxProvider
  {
    private static IDialogService<MessageBoxDialog, LulDialogResult> _messageBoxService;

    public MessageBoxProvider(IDialogService<MessageBoxDialog, LulDialogResult> messageBoxService)
    {
      _messageBoxService = messageBoxService;
    }

    public static LulDialogResult Show(string title, string message, DialogButtons dialogButtons)
    {
      return _messageBoxService.Show(title, message, dialogButtons);
    }

    public static LulDialogResult ShowDeleteDialog(string itemDescriptor)
    {
      return MessageBoxProvider.Show($"Delete {itemDescriptor}", $"Do you want to delete selected {itemDescriptor}(s)?", DialogButtons.YesNo);
    }
  }
}
