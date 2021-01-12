using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LulCaster.Test.UI.WPF
{
  public class PresetControlViewModelTests
  {
    private Fixture _fixture = new Fixture();
    private IPresetListController _presetController;
    private PresetControlViewModel _presetControlViewModel; 

    protected PresetControlViewModelTests()
    {
      _presetController = Substitute.For<IPresetListController>();
      _presetControlViewModel = new PresetControlViewModel(_presetController);
    }

    [Fact]
    public void PresetController_OnViewModelLoad_ListContainsSavedPresets()
    {
      List<PresetViewModel> mockedPresets = _fixture.Create<List<PresetViewModel>>();

      _presetController.GetAllAsync().Returns(mockedPresets);

      _presetControlViewModel.Presets.Count.ShouldBe(mockedPresets.Count);
    }

    [Fact]
    public void PresetController_GetAll_ItemListShouldBeItenditical()
    {
      List<PresetViewModel> mockedPresets = _fixture.Create<List<PresetViewModel>>();

      _presetController.GetAllAsync().Returns(mockedPresets);

      var mockedItem = mockedPresets.Last();
      var resultItem = _presetControlViewModel.Presets.Last();

      resultItem.ShouldBe<PresetViewModel>(mockedItem);
    }
  }
}
