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
    private readonly MessageBoxDialog _messageBoxDialog;
    private readonly PresetInputDialog _presetInputDialog;

    private WireFrameViewModel WireFrameViewModel { get => (WireFrameViewModel)_wireFramePage.DataContext; }

    public MainWindow(WireFramePage wireFramePage, IPresetListController presetListController, IRegionListController regionListController, InputDialog inputDialog, PresetInputDialog presetInputDialog, MessageBoxDialog messageBoxDialog)
    {
      InitializeComponent();
      _wireFramePage = wireFramePage;
      _presetListController = presetListController;
      _regionListController = regionListController;
      _inputDialog = inputDialog;
      _presetInputDialog = presetInputDialog;
      _messageBoxDialog = messageBoxDialog;
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

      if (_presetInputDialog.Show("Import Preset", "Preset Name:", DialogButtons.OkCancel) is PresetInputDialogResult dialogResult
        && dialogResult.DialogResult == DialogResults.Ok)
      {
        var presetViewModel = _presetListController.CreatePreset(dialogResult.PresetName, dialogResult.ProcessName);
        var regionList = _regionListController.GetAllRegions(openFileDialog.FileName);
        _regionListController.WriteAllRegions(PresetFile.ResolvePresetFilePath(presetViewModel.Id), regionList);

        WireFrameViewModel.Presets.Add(presetViewModel);
        WireFrameViewModel.SelectedPreset = presetViewModel;
      }
    }

    private void MenuItemExport_Click(object sender, RoutedEventArgs e)
    {
      if (WireFrameViewModel.SelectedPreset == null)
      {
        _messageBoxDialog.Show("Export Fail!", "Unable to export as no preset is selected!", DialogButtons.Ok);
        return;
      }

      var saveFileDialog = new SaveFileDialog()
      {
        Filter = configFileExtension
      };

      if (saveFileDialog.ShowDialog() != true)
        return;

      _regionListController.WriteAllRegions(saveFileDialog.FileName, WireFrameViewModel.Regions);
    }
  }
}