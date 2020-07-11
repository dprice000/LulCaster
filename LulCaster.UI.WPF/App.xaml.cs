using AutoMapper;
using AutoMapper.Configuration;
using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Pages;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using LulCaster.Utility.ScreenCapture.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      var p = (Exception)e.ExceptionObject;
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<MainWindow>();
      services.AddSingleton<IConfigService, ConfigService>();
      services.AddScoped<IScreenCaptureService, ScreenCaptureService>();
      services.AddScoped<Microsoft.Extensions.Configuration.IConfiguration>(provider => {
        return new ConfigurationBuilder().AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                  .Build();
      });

      RegisterControllers(services);
      RegisterPages(services);
      RegisterDialogServices(services);
      RegisterAutoMapper(services);
    }

    private void RegisterAutoMapper(IServiceCollection services)
    {
      services.AddScoped<IMapper>(provider =>
      {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<RegionConfig, RegionViewModel>());
        return new Mapper(config);
      });
    }

    private void RegisterPages(IServiceCollection services)
    {
      services.AddScoped<WireFramePage>();
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
      services.AddScoped<ISimpleDialogService<string>, NewPresetDialog>();
    }
  }
}