using AutoMapper;
using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Config.UserSettings;
using LulCaster.UI.WPF.Config.UserSettings.Models;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.Services;
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
      throw new NotImplementedException();
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<MainWindow>();
      services.AddSingleton<IConfigManagerService, ConfigManagerService>();
      services.AddSingleton<IPresetConfigService, PresetConfigService>();
      services.AddSingleton<IRegionConfigService, RegionConfigService>();
      services.AddScoped<IScreenCaptureService, GameCaptureService>();

      RegisterViewModels(services);
      RegisterControllers(services);
      RegisterPages(services);
      RegisterDialogServices(services);
      RegisterDialogProviders(services);
      RegisterAutoMapper(services);
    }

    private void RegisterAutoMapper(IServiceCollection services)
    {
      services.AddScoped<IMapper>(provider =>
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<PresetViewModel, PresetConfig>();
          cfg.CreateMap<TriggerViewModel, TriggerConfig>();
          cfg.CreateMap<PresetConfig, PresetViewModel>();
          cfg.CreateMap<TriggerConfig, TriggerViewModel>();

          cfg.CreateMap<RegionConfig, RegionViewModel>()
              .ForMember(dest => dest.BoundingBox, opt => opt.MapFrom(s => s.BoundingBox));

          cfg.CreateMap<RegionViewModel, RegionConfig>()
            .ForMember(dest => dest.BoundingBox, opt => opt.Ignore())
            .ForMember(dest => dest.X, opt => opt.MapFrom(s => s.BoundingBox.X))
            .ForMember(dest => dest.Y, opt => opt.MapFrom(s => s.BoundingBox.Y))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(s => s.BoundingBox.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(s => s.BoundingBox.Height));
        });

        return new Mapper(config);
      });
    }

    private void RegisterViewModels(IServiceCollection services)
    {
      services.AddScoped<WireFrameViewModel>();
      services.AddScoped<PresetControlViewModel>();
      services.AddScoped<RegionControlViewModel>();
      services.AddScoped<RegionConfigViewModel>();
    }

    private void RegisterPages(IServiceCollection services)
    {
      services.AddScoped<WireFramePage>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
      InitializeDialogProviders();

      var mainWindow = _serviceProvider.GetService<MainWindow>();
      mainWindow.Show();
    }

    private void InitializeDialogProviders()
    {
      CrudDialogProvider.AddDialog<PresetViewModel>(_serviceProvider.GetService<INestedViewDialog<PresetViewModel>>());
      CrudDialogProvider.AddDialog<RegionViewModel>(_serviceProvider.GetService<INestedViewDialog<RegionViewModel>>());
      var messageBox = new MessageBoxProvider(_serviceProvider.GetService<IDialogService<MessageBoxDialog, LulDialogResult>>());
      var inputProvider = new InputDialogProvider(_serviceProvider.GetService<IDialogService<InputDialog, InputDialogResult>>());
    }

    private void RegisterControllers(IServiceCollection services)
    {
      services.AddScoped<IPresetListController, PresetListController>();
      services.AddScoped<IRegionListController, RegionListController>();
      services.AddScoped<ITriggerController, TriggerController>();
    }

    private void RegisterDialogServices(IServiceCollection services)
    {
      services.AddScoped<InputDialog>();
      services.AddScoped<PresetInputDialog>();
      services.AddScoped<RegionDialog>();
      services.AddScoped<MessageBoxDialog>();
      services.AddTransient(typeof(IDialogService<InputDialog, InputDialogResult>), typeof(DialogService<InputDialog, InputDialogResult>));
      services.AddTransient(typeof(IDialogService<PresetInputDialog, NestedDialogResults<PresetViewModel>>), typeof(DialogService<PresetInputDialog, NestedDialogResults<PresetViewModel>>));
      services.AddTransient(typeof(IDialogService<MessageBoxDialog, LulDialogResult>), typeof(DialogService<MessageBoxDialog, LulDialogResult>));
      services.AddTransient(typeof(INestedViewDialog<PresetViewModel>), typeof(PresetInputDialog));
      services.AddTransient(typeof(INestedViewDialog<RegionViewModel>), typeof(RegionDialog));
    }

    private void RegisterDialogProviders(IServiceCollection services)
    {
      services.AddSingleton<InputDialogProvider>();
      services.AddSingleton<MessageBoxProvider>();
    }
  }
}