using AutoFixture;
using LulCaster.Test.UI.WPF.Utility;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class PresetControlCrudEventTests
  {
    private Fixture _fixture = new Fixture();
    private IPresetListController _presetController;
    private PresetControlViewModel _presetControlViewModel;

    public PresetControlCrudEventTests()
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
      DialogMocker.InitializeOkCancelDialog(preset, DialogResults.Ok);

      _presetControlViewModel.Presets.Count.ShouldBe(0);

      //Act
      await _presetControlViewModel.NewItemClickedAsync(new ButtonClickArgs("Create", "Preset"));
      var newPreset = _presetControlViewModel.Presets.First();

      //Assert
      newPreset.ShouldBe(preset);
    }

    [Fact]
    public async Task NewItemClicked_CancelButtonSelected_NoPresetAdded()
    {
      //Arrange
      var preset = _fixture.Create<PresetViewModel>();
      DialogMocker.InitializeOkCancelDialog(preset, DialogResults.Cancel);

      _presetControlViewModel.Presets.Count.ShouldBe(0);

      //Act
      await _presetControlViewModel.NewItemClickedAsync(new ButtonClickArgs("Create", "Preset"));

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(0);
    }

    [Fact]
    public async Task NewItemClicked_CancelButtonSelected_SelectedPresetIsNull()
    {
      //Arrange
      var preset = _fixture.Create<PresetViewModel>();
      DialogMocker.InitializeOkCancelDialog(preset, DialogResults.Cancel);

      _presetControlViewModel.Presets.Count.ShouldBe(0);

      //Act
      await _presetControlViewModel.NewItemClickedAsync(new ButtonClickArgs("Create", "Preset"));

      //Assert
      _presetControlViewModel.SelectedPreset.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteItemClicked_YesButtonSelected_PresetDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _presetControlViewModel.Presets.Count.ShouldBe(1);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(0);
    }

    [Fact]
    public async Task DeleteItemClicked_NoButtonSelected_PresetStillSelected()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeYesNoDialog(DialogResults.No);

      _presetControlViewModel.SelectedPreset.ShouldBe(selectedPreset);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.SelectedPreset.ShouldBe(selectedPreset);
    }

    [Fact]
    public async Task DeleteItemClicked_NoButtonSelected_PresetStillExists()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeYesNoDialog(DialogResults.No);

      _presetControlViewModel.Presets.Count.ShouldBe(1);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(1);
    }

    [Fact]
    public async Task DeleteItemClicked_YesButtonSelected_SelectedPresetIsNull()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      _presetControlViewModel.Presets = new ObservableCollection<PresetViewModel>() { selectedPreset };
      _presetControlViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _presetControlViewModel.Presets.Count.ShouldBe(1);

      //Act
      await _presetControlViewModel.DeleteItemClickedAsync(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _presetControlViewModel.SelectedPreset.ShouldBeNull();
    }
  }
}