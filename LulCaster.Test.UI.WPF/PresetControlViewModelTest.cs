using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class PresetControlViewModelTests
  {
    private Fixture _fixture = new Fixture();
    private IPresetListController _presetController;
    private PresetControlViewModel _presetControlViewModel; 

    public PresetControlViewModelTests()
    {
      _presetController = Substitute.For<IPresetListController>();
      _presetControlViewModel = new PresetControlViewModel(_presetController);
    }

    [Fact]
    public void OnViewModelLoad_ControllerLoadsPresets_ListContainsSavedPresets()
    {
      //Arrange
      List<PresetViewModel> mockedPresets = _fixture.Create<List<PresetViewModel>>();
      _presetController.GetAllAsync().Returns(mockedPresets);

      //Act
      _presetControlViewModel = new PresetControlViewModel(_presetController);

      //Assert
      _presetControlViewModel.Presets.Count.ShouldBe(mockedPresets.Count);
    }

    [Fact]
    public void OnViewModelLoad_ControllerLoadsPresets_ItemListShouldBeItenditical()
    {
      //Arrange
      List<PresetViewModel> mockedPresets = _fixture.Create<List<PresetViewModel>>();
      _presetController.GetAllAsync().Returns(mockedPresets);

      //Act
      _presetControlViewModel = new PresetControlViewModel(_presetController);
      var mockedItem = mockedPresets.Last();
      var resultItem = _presetControlViewModel.Presets.Last();

      //Assert
      resultItem.ShouldBe<PresetViewModel>(mockedItem);
    }
  }
}
