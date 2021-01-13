using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class PresetControlEventTests
  {
    private Fixture _fixture = new Fixture();
    private IPresetListController _presetController;
    private PresetControlViewModel _presetControlViewModel;

    public PresetControlEventTests()
    {
      _presetController = Substitute.For<IPresetListController>();
      _presetControlViewModel = new PresetControlViewModel(_presetController);
    }

    [Fact]
    public async Task NewItemClicked_CompletedDialog_PresetAddedToList()
    {
      //Arrange
      var preset = _fixture.Create<PresetViewModel>();
      _presetController.CreateAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(preset));
      InitializeOkedDialog(preset);

      _presetControlViewModel.Presets.Count.ShouldBe(0);

      //Act
      await _presetControlViewModel.NewItemClickedAsync(new ButtonClickArgs("Create", "Preset"));
      var newPreset = _presetControlViewModel.Presets.First();

      //Assert
      newPreset.ShouldBe(preset);
    }

    [Fact]
    public async Task DeleteItemClicked_SelectedYesConfirmation_PresetDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      InitializeYesNoDialog(DialogResults.Yes);

      _presetControlViewModel.Presets.Count.ShouldBe(1);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(0);
    }

    [Fact]
    public async Task DeleteItemClicked_SelectedNoConfirmation_PresetDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      InitializeYesNoDialog(DialogResults.No);

      _presetControlViewModel.Presets.Count.ShouldBe(1);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(1);
    }

    private void InitializeOkedDialog(PresetViewModel dialogViewModel)
    {
      var dialogResult = new NestedDialogResults<PresetViewModel>(dialogViewModel, DialogResults.Ok);
      var dialogService = Substitute.For<INestedViewDialog<PresetViewModel>>();
      dialogService.Show(Arg.Any<NestedViewModel<PresetViewModel>>()).Returns(dialogResult);

      CrudDialogProvider.AddDialog<PresetViewModel>(dialogService);
    }

    private void InitializeYesNoDialog(DialogResults dialogResult)
    {
      var result = new LulDialogResult() { DialogResult = dialogResult };
      var dialogService = Substitute.For<IDialogService<MessageBoxDialog, LulDialogResult>>();
      dialogService.Show(Arg.Any<string>(), Arg.Any<string>(), DialogButtons.YesNo).Returns(result);

      var messanger = new MessageBoxProvider(dialogService);
    }
  }
}