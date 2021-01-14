using AutoFixture;
using LulCaster.Test.UI.WPF.Utility;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.ObjectModel;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class RegionConfigControlTests
  {
    private Fixture _fixture = new Fixture();
    private RegionConfigViewModel _regionConfigViewModel;
    private IRegionListController _regionListController;
    private ITriggerController _triggerController;

    public RegionConfigControlTests()
    {
      _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _triggerController = Substitute.For<ITriggerController>();
      _regionListController = Substitute.For<IRegionListController>();
      _regionConfigViewModel = new RegionConfigViewModel(_regionListController, _triggerController);
    }

    [Fact]
    public void AddTriggerClicked_OkButtonSelected_TriggerCreated()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      selectedRegion.Triggers.Clear();
      var newTrigger = _fixture.Create<TriggerViewModel>();
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeInputDialog(newTrigger.Name, DialogResults.Ok);
      _triggerController.CreateTrigger(selectedPreset.Id, selectedRegion.Id, newTrigger.Name).Returns(newTrigger);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);

      //Act
      _regionConfigViewModel.AddTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(1);
    }

    [Fact]
    public void AddTriggerClicked_OkButtonSelected_NewTriggerSelected()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      selectedRegion.Triggers.Clear();
      var newTrigger = _fixture.Create<TriggerViewModel>();
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedPreset = selectedPreset;
      DialogMocker.InitializeInputDialog(newTrigger.Name, DialogResults.Ok);
      _triggerController.CreateTrigger(selectedPreset.Id, selectedRegion.Id, newTrigger.Name).Returns(newTrigger);

      _regionConfigViewModel.SelectedTrigger.ShouldBeNull();

      //Act
      _regionConfigViewModel.AddTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedTrigger.ShouldBe(newTrigger);
    }

    [Fact]
    public void AddTriggerClicked_CancelButtonSelected_TriggerIsNotCreated()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      selectedRegion.Triggers.Clear();
      var newTrigger = _fixture.Create<TriggerViewModel>();
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      DialogMocker.InitializeInputDialog(newTrigger.Name, DialogResults.Cancel);
      _triggerController.CreateTrigger(selectedPreset.Id, selectedRegion.Id, newTrigger.Name).Returns(newTrigger);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);

      //Act
      _regionConfigViewModel.AddTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);
    }

    [Fact]
    public void AddTriggerClicked_TriggerNameIsWhitespace_TriggerIsNotCreated()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      selectedRegion.Triggers.Clear();
      var newTrigger = _fixture.Create<TriggerViewModel>();
      newTrigger.Name = "         ";
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      DialogMocker.InitializeInputDialog(newTrigger.Name, DialogResults.Cancel);
      _triggerController.CreateTrigger(selectedPreset.Id, selectedRegion.Id, newTrigger.Name).Returns(newTrigger);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);

      //Act
      _regionConfigViewModel.AddTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);
    }

    [Fact]
    public void AddTriggerClicked_TriggerNameIsNull_TriggerIsNotCreated()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      selectedRegion.Triggers.Clear();
      var newTrigger = _fixture.Create<TriggerViewModel>();
      newTrigger.Name = null;
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      DialogMocker.InitializeInputDialog(newTrigger.Name, DialogResults.Cancel);
      _triggerController.CreateTrigger(selectedPreset.Id, selectedRegion.Id, newTrigger.Name).Returns(newTrigger);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);

      //Act
      _regionConfigViewModel.AddTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(0);
    }

    [Fact]
    public void DeleteTriggerClicked_YesButtonSelected_TriggerDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      var selectedTrigger = _fixture.Create<TriggerViewModel>();

      _regionConfigViewModel.SelectedPreset = selectedPreset;
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedRegion.Triggers = new ObservableCollection<TriggerViewModel>() { selectedTrigger };
      _regionConfigViewModel.SelectedTrigger = selectedTrigger;

      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _regionConfigViewModel.SelectedTrigger.ShouldBe(selectedTrigger);

      //Act
      _regionConfigViewModel.DeleteTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedTrigger.ShouldBeNull();
    }

    [Fact]
    public void DeleteTriggerClicked_NoButtonSelected_TriggerNotDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      var selectedTrigger = _fixture.Create<TriggerViewModel>();

      _regionConfigViewModel.SelectedPreset = selectedPreset;
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedRegion.Triggers = new ObservableCollection<TriggerViewModel>() { selectedTrigger };
      _regionConfigViewModel.SelectedTrigger = selectedTrigger;

      DialogMocker.InitializeYesNoDialog(DialogResults.No);

      _regionConfigViewModel.SelectedTrigger.ShouldBe(selectedTrigger);

      //Act
      _regionConfigViewModel.DeleteTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedTrigger.ShouldBe(selectedTrigger);
    }

    [Fact]
    public void DeleteTriggerClicked_SelectedTriggerIsNull_TriggerNotDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      var selectedTrigger = _fixture.Create<TriggerViewModel>();

      _regionConfigViewModel.SelectedPreset = selectedPreset;
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedRegion.Triggers = new ObservableCollection<TriggerViewModel>() { selectedTrigger };
      _regionConfigViewModel.SelectedTrigger = null;

      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(1);
      _regionConfigViewModel.SelectedTrigger.ShouldBeNull();

      //Act
      _regionConfigViewModel.DeleteTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(1);
    }

    [Fact]
    public void DeleteTriggerClicked_SelectedPresetIsNull_TriggerNotDeleted()
    {
      //Arrange
      var selectedRegion = _fixture.Create<RegionViewModel>();
      var selectedTrigger = _fixture.Create<TriggerViewModel>();

      _regionConfigViewModel.SelectedPreset = null;
      _regionConfigViewModel.SelectedRegion = selectedRegion;
      _regionConfigViewModel.SelectedRegion.Triggers = new ObservableCollection<TriggerViewModel>() { selectedTrigger };
      _regionConfigViewModel.SelectedTrigger = selectedTrigger;

      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(1);
      _regionConfigViewModel.SelectedPreset.ShouldBeNull();

      //Act
      _regionConfigViewModel.DeleteTriggerClicked(null);

      //Assert
      _regionConfigViewModel.SelectedRegion.Triggers.Count.ShouldBe(1);
    }
  }
}
