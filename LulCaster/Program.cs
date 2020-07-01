using System;
using System.IO;
using System.Threading;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.Service;

namespace LulCaster
{
  class Program
  {
    private static void Main(string[] args)
    {
      var screenCap = new ScreenCap();
      var ocr = new OcrService();

      while (true)
      {
        
        var bitmap = screenCap.CaptureScreenshot();
        var memoryStream = new MemoryStream();
        var text = ocr.ProcessImage(memoryStream);

        Console.WriteLine(text);
        Thread.Sleep(5000);
      }
    }
  }
}