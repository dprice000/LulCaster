using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xunit;


namespace LulCaster.Test.UI.WPF
{
  public class RegionControlViewModelTests
  {
    private Fixture _fixture = new Fixture();
    private IRegionListController _regionListController;
    private RegionControlViewModel _regionControlViewModel;

    public RegionControlViewModelTests()
    {
      _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      _regionListController = Substitute.For<IRegionListController>();
      _regionControlViewModel = new RegionControlViewModel(_regionListController);
    }

    [Fact]
    public void PresetSelectionChanged_PresetContainsRegion_RegionsAreIdentical()
    {
      //Arrange
      var mockedPreset = _fixture.Create<PresetViewModel>();
      var mockedRegions = _fixture.Create<List<RegionViewModel>>();
      _regionListController.GetRegions(mockedPreset.Id).Returns(mockedRegions);
      _regionControlViewModel = new RegionControlViewModel(_regionListController);

      //Act
      _regionControlViewModel.SelectedPreset = mockedPreset;

      //Assert
      _regionControlViewModel.Regions.ShouldBe(mockedRegions);
    }

    [Fact]
    public void PresetSelectionChanged_RegionListEmpty_SetToEmptyList()
    {
      //Arrange
      var emptyList = new List<RegionViewModel>();
      _regionListController.GetRegions(Arg.Any<Guid>()).Returns(emptyList);
      _regionControlViewModel = new RegionControlViewModel(_regionListController);

      //Act
      _regionControlViewModel.SelectedPreset = _fixture.Create<PresetViewModel>();

      //Assert
      _regionControlViewModel.Regions.ShouldBe(emptyList);
    }
  }
}
