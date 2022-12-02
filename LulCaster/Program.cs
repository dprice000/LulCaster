using System;
using System.IO;
using System.Threading;
using LulCaster.Utility.Service;
using LulCaster.Utility.ScreenCapture.Windows;
using System.Drawing;
using System.Drawing.Imaging;

namespace LulCaster
{
  class Program
  {
    private static void Main(string[] args)
    {
      var ocr = new OcrService();

      while (true)
      {
        var image = new Bitmap(@"C:\Users\David\Desktop\POP\TestFeed.tiff");

        using (MemoryStream memoryStream = new MemoryStream())
        {
          image.Save(memoryStream, ImageFormat.Tiff);

          var text = ocr.ProcessImage(memoryStream);
        }
      }
    }
  }
}