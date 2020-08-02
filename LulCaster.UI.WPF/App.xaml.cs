using AutoMapper;
using AutoMapper.Configuration;
using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Config.Models;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Pages;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using LulCaster.Utility.ScreenCapture.Windows;
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
      services.AddSingleton<IPresetConfigService, PresetConfigService>();
      services.AddSingleton<IRegionConfigService, RegionConfigService>();
      services.AddScoped<IScreenCaptureService, ScreenCaptureService>();

      RegisterControllers(services);
      RegisterPages(services);
      RegisterDialogServices(services);
      RegisterAutoMapper(services);
    }

    private void RegisterAutoMapper(IServiceCollection services)
    {
      services.AddScoped<IMapper>(provider =>
      {
        var config = new MapperConfiguration(cfg => 
        {
          cfg.CreateMap<PresetConfig, PresetViewModel>();

          cfg.CreateMap<RegionConfig, RegionViewModel>()
              .ForMember(dest => dest.BoundingBox, opt => opt.MapFrom(s => s.BoundingBox));

          cfg.CreateMap<RegionViewModel, RegionConfig>()
            .ForMember(dest => dest.BoundingBox, opt => opt.Ignore())
            .ForMember(dest => dest.X, opt => opt.MapFrom(s => s.BoundingBox.X))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(s => s.BoundingBox.Y))
            .ForMember(dest => dest.Widht, opt => opt.MapFrom(s => s.BoundingBox.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(s => s.BoundingBox.Height));
         });

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
      services.AddScoped<IRegionListController, RegionListController>();
    }

    private void RegisterDialogServices(IServiceCollection services)
    {
      services.AddScoped<ISimpleDialogService<string>, NewPresetDialog>();
      services.AddScoped<IMessageBoxDialogService, MessageBoxDialogService>();
    }
  }
}