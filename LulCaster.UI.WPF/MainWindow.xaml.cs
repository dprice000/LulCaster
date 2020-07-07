using LulCaster.UI.WPF.Pages;
using System.Windows;
using System.Windows.Navigation;

namespace LulCaster.UI.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private NavigationService _navigationService;

    public MainWindow(WireFramePage wireFramePage)
    {
      InitializeComponent();
      _navigationService = NavigationService.GetNavigationService(this);
      _navigationService.Navigate(wireFramePage);
    }
  }
}