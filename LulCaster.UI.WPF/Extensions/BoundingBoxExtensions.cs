using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Drawing;

namespace LulCaster.UI.WPF.Extensions
{
  public static class BoundingBoxExtensions
  {

    public static Rectangle BoundingBoxToRectangle(this RegionConfig regionConfig)
    {
      var dimensions = regionConfig.BoundingBoxDimensions.Split(",");
      return new Rectangle() { X = Convert.ToInt32(dimensions[0]), Y = Convert.ToInt32(dimensions[1]), Width = Convert.ToInt32(dimensions[2]), Height = Convert.ToInt32(dimensions[3]) };
    }

    public static string BoundingBoxToString(this RegionViewModel regionViewModel)
    {
      var boundingBox = regionViewModel.BoundingBox;
      return $"{boundingBox.X}, {boundingBox.Y}, {boundingBox.Width}, {boundingBox.Height}";
    }
  }
}