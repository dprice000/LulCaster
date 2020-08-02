using LulCaster.UI.WPF.ViewModels;
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
      DataContext = new PresetViewModel();
    }

    public string ReturnValue {
      get
      {
        return ((PresetViewModel)DataContext).Name;
      }
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