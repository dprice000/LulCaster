using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for InputDialog.xaml
  /// </summary>
  public partial class InputDialog : Window, ILulDialog
  {
    public InputDialog()
    {
      InitializeComponent();
    }

    public object Show(string title, string message, DialogButtons dialogButtons)
    {
      DataContext = new InputDialogViewModel()
      {
        Title = title,
        Message = message,
        MessageBoxButtons = dialogButtons
      };

      ShowDialog();

      return (DialogResult ?? false) ? ((InputDialogViewModel)DataContext).Input : null;
    }

    private void Button_btnOk(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
      Close();
    }

    private void Button_btnCancel(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
  }
}