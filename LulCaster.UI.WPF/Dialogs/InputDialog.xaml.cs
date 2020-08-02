using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for InputDialog.xaml
  /// </summary>
  public partial class InputDialog : Window
  {
    public InputDialog()
    {
      InitializeComponent();

      DataContext = new InputDialogViewModel();
    }

    public static string Show(string title, string message)
    {
      var dialog = new InputDialog();
      dialog.ShowDialog();

      return (dialog.DialogResult ?? false) ? ((InputDialogViewModel)dialog.DataContext).Input : null;
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