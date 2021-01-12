using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
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
    public void OnViewModelLoad_PresetsExist_PresetsAreLoaded()
    {
      //Arrange
      List<PresetViewModel> mockedPresets = _fixture.Create<List<PresetViewModel>>();
      _presetController.GetAllAsync().Returns(mockedPresets);

      //Act
      _presetControlViewModel = new PresetControlViewModel(_presetController);

      //Assert
      _presetControlViewModel.Presets.ShouldBe(mockedPresets);
    }

    [Fact]
    public void OnViewModelLoad_EmptyListReturned_PresetSetToEmptyList()
    {
      //Arrange
      var emptyList = new List<PresetViewModel>();
      _presetController.GetAllAsync().Returns(emptyList);

      //Act
      _presetControlViewModel = new PresetControlViewModel(_presetController);

      //Assert
      _presetControlViewModel.Presets.ShouldBe(emptyList);
    }
  }
}
