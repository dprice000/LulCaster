using AutoFixture;
using LulCaster.UI.WPF.Controllers;
using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;
using Shouldly;
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

      _regionControlViewModel.Regions.Count.ShouldBe(0);

      //Act
      _regionControlViewModel.NewItemClicked(new ButtonClickArgs("Create", "Create"));

      //Assert
      _regionControlViewModel.Regions.Count.ShouldBe(1);
    }
  }
}
