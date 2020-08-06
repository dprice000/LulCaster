using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for InputDialog.xaml
  /// </summary>
  public partial class InputDialog : Window, ILulDialog
  {
    public new DialogResults DialogResult { get; set; }

    public InputDialog()
    {
      InitializeComponent();
    }

    public DialogResult Show(string title, string message, DialogButtons dialogButtons)
    {
      DataContext = new InputDialogViewModel()
      {
        Title = title,
        Message = message,
        MessageBoxButtons = dialogButtons
      };

      ShowDialog();

      return new InputDialogResult
      {
        DialogResult = this.DialogResult,
        Input = (DialogResult == DialogResults.Ok) ? ((InputDialogViewModel)DataContext).Input : null
      };
    }

    private void Button_btnOk(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Ok;
      Close();
    }

    private void Button_btnCancel(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Cancel;
      Close();
    }
  }
}