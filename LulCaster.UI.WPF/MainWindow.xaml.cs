using LulCaster.UI.WPF.Pages;
using System.Windows;

namespace LulCaster.UI.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly WireFramePage _wireFramePage;

    public MainWindow(WireFramePage wireFramePage)
    {
      InitializeComponent();
      _wireFramePage = wireFramePage;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      NavigationFrame.Navigate(_wireFramePage);
    }
  }
}