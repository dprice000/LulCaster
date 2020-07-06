using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Controllers;
using LulCaster.Utility.ScreenCapture.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LulCaster.UI.WPF
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<MainWindow>();
      services.AddSingleton<IConfigService, ConfigService>();
      services.AddScoped<IScreenCaptureService, ScreenCaptureService>();

      RegisterControllers(services);
      RegisterDialogServices(services);
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
      var mainWindow = _serviceProvider.GetService<MainWindow>();
      mainWindow.Show();
    }

    private void RegisterControllers(IServiceCollection services)
    {
      services.AddScoped<IPresetListController, PresetListController>();
    }

    private void RegisterDialogServices(IServiceCollection services)
    {
    }
  }
}