using System;
using System.IO;
using System.Threading;
using LulCaster.Utility.Service;
using LulCaster.Utility.ScreenCapture.Windows;

namespace LulCaster
{
  class Program
  {
    private static void Main(string[] args)
    {
      var screenCap = new ScreenCaptureService();
      var ocr = new OcrService();

      while (true)
      {
        var bitmap = screenCap.CaptureScreenshot();
        var text = ocr.ProcessImage(new MemoryStream(bitmap));

        Console.WriteLine(text);
        Thread.Sleep(5000);
      }
    }
  }
}