using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for NewPresetDialog.xaml
  /// </summary>
  public partial class NewPresetDialog : Window, ISimpleDialogService<string>
  {
    public NewPresetDialog()
    {
      InitializeComponent();
    }

    public string ReturnValue { get; }

    private void Button_btnOk(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

    private void Button_btnCancel(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
    }
  }
}