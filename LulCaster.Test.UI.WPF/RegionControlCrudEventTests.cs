using AutoFixture;
using LulCaster.Test.UI.WPF.Utility;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.ObjectModel;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class RegionControlCrudEventTests
  {
    private Fixture _fixture = new Fixture();
    private IRegionListController _regionListController;
    private RegionControlViewModel _regionControlViewModel;

    public RegionControlCrudEventTests()
    {
      _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _regionListController = Substitute.For<IRegionListController>();
      _regionControlViewModel = new RegionControlViewModel(_regionListController);
    }

    [Fact]
    public void NewItemClicked_OkButtonSelected_RegionAddedToList()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionListController.Create(selectedPreset.Id, region.Name).Returns(region);
      DialogMocker.InitializeOkCancelDialog(region, DialogResults.Ok);

      _regionControlViewModel.Regions.Count.ShouldBe(0);

      //Act
      _regionControlViewModel.NewItemClicked(new ButtonClickArgs("Create", "Create"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(1);
    }

    [Fact]
    public void NewItemClicked_OkButtonSelected_NewRegionSelected()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.SelectedRegion = null;
      _regionListController.Create(selectedPreset.Id, region.Name).Returns(region);
      DialogMocker.InitializeOkCancelDialog(region, DialogResults.Ok);

      _regionControlViewModel.SelectedRegion.ShouldBeNull();

      //Act
      _regionControlViewModel.NewItemClicked(new ButtonClickArgs("Create", "Create"));

      //Assert
      _regionControlViewModel.SelectedRegion.ShouldBe(region);
    }

    [Fact]
    public void NewItemClicked_CancelButtonSelected_RegionNotAdded()
    {
      //Arrange
      var region = _fixture.Create<RegionViewModel>();
      DialogMocker.InitializeOkCancelDialog(region, DialogResults.Cancel);

      _regionControlViewModel.Regions.Count.ShouldBe(0);

      //Act
      _regionControlViewModel.NewItemClicked(new ButtonClickArgs("Create", "Create"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(0);
    }

    [Fact]
    public void DeleteItemClicked_YesButtonSelected_RegionDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.SelectedRegion = region;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { region };
      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _regionControlViewModel.Regions.Count.ShouldBe(1);

      //Act
      _regionControlViewModel.DeleteItemClicked(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(0);
    }

    [Fact]
    public void DeleteItemClicked_YesButtonSelected_SelectedRegionIsNull()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.SelectedRegion = region;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { region };
      DialogMocker.InitializeYesNoDialog(DialogResults.Yes);

      _regionControlViewModel.SelectedRegion.ShouldBe(region);

      //Act
      _regionControlViewModel.DeleteItemClicked(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _regionControlViewModel.SelectedRegion.ShouldBeNull();
    }

    [Fact]
    public void DeleteItemClicked_NoButtonSelected_RegionNotDeleted()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.SelectedRegion = region;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { region };
      DialogMocker.InitializeYesNoDialog(DialogResults.No);

      _regionControlViewModel.Regions.Count.ShouldBe(1);

      //Act
      _regionControlViewModel.DeleteItemClicked(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(1);
    }

    [Fact]
    public void DeleteItemClicked_NoButtonSelected_RegionStillSelected()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.SelectedRegion = region;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { region };
      DialogMocker.InitializeYesNoDialog(DialogResults.No);

      _regionControlViewModel.SelectedRegion.ShouldBe(region);

      //Act
      _regionControlViewModel.DeleteItemClicked(this, new ButtonClickArgs("Delete", "Delete"));

      //Assert
      _regionControlViewModel.SelectedRegion.ShouldBe(region);
    }

    [Fact]
    public void EditItemClicked_OkButtonSelected_RegionCountHasNotChanged()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var region = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { region };
      _regionControlViewModel.SelectedRegion = region;
      var updatedRegion = _fixture.Create<RegionViewModel>();
      DialogMocker.InitializeOkCancelDialog(region, DialogResults.Ok);

      _regionListController.Create(selectedPreset.Id, region.Name).Returns(updatedRegion);

      _regionControlViewModel.Regions.Count.ShouldBe(1);

      //Act
      _regionControlViewModel.EditItemClicked(this, new ButtonClickArgs("Edit", "Edit"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(1);
    }

    [Fact]
    public void EditItemClicked_OkButtonSelected_SelectedRegionUpdated()
    {
      //Arrange
      var selectedPreset = _fixture.Create<PresetViewModel>();
      var selectedRegion = _fixture.Create<RegionViewModel>();
      _regionControlViewModel.SelectedPreset = selectedPreset;
      _regionControlViewModel.Regions = new ObservableCollection<RegionViewModel>() { selectedRegion };
      _regionControlViewModel.SelectedRegion = selectedRegion;
      var updatedRegion = _fixture.Create<RegionViewModel>();
      DialogMocker.InitializeOkCancelDialog(selectedRegion, DialogResults.Ok);

      _regionListController.Create(selectedPreset.Id, selectedRegion.Name).Returns(updatedRegion);

      _regionControlViewModel.SelectedRegion.ShouldBe(selectedRegion);

      //Act
      _regionControlViewModel.EditItemClicked(this, new ButtonClickArgs("Edit", "Edit"));

      //Assert
      _regionControlViewModel.SelectedRegion.ShouldBe(updatedRegion);
    }
  }
}
