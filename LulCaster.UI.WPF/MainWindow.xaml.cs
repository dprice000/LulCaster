using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Pages;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace LulCaster.UI.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const string configFileExtension = "JSON|*.json";
    private readonly WireFramePage _wireFramePage;
    private readonly IPresetListController _presetListController;
    private readonly IRegionListController _regionListController;
    private readonly InputDialog _inputDialog;

    public MainWindow(WireFramePage wireFramePage, IPresetListController presetListController, IRegionListController regionListController, InputDialog inputDialog)
    {
      InitializeComponent();
      _wireFramePage = wireFramePage;
      _presetListController = presetListController;
      _regionListController = regionListController;
      _inputDialog = inputDialog;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      NavigationFrame.Navigate(_wireFramePage);
    }

    private void MenuItemImport_Click(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog
      {
        Filter = configFileExtension
      };

      if (openFileDialog.ShowDialog() != true)
        return;

      if (_inputDialog.Show("Import Preset", "Preset Name:", DialogButtons.OkCancel) is InputDialogResult dialogResult
        && dialogResult.DialogResult == DialogResults.Ok)
      {
        var presetViewModel = _presetListController.CreatePreset(dialogResult.Input);
        var regionList = _regionListController.GetAllRegions(openFileDialog.FileName);
        _regionListController.WriteAllRegions(PresetFile.ResolvePresetFilePath(presetViewModel.Id), regionList);

        var wirePageViewModel = (WireFrameViewModel)_wireFramePage.DataContext;
        wirePageViewModel.Presets.Add(presetViewModel);
        wirePageViewModel.SelectedPreset = presetViewModel;
      }
    }

    private void MenuItemExport_Click(object sender, RoutedEventArgs e)
    {
      var saveFileDialog = new SaveFileDialog()
      {
        Filter = configFileExtension
      };

      if (saveFileDialog.ShowDialog() != true)
        return;

      _regionListController.WriteAllRegions(saveFileDialog.FileName, ((WireFrameViewModel)_wireFramePage.DataContext).Regions);
    }
  }
}