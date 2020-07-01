using Tesseract;
using System.IO;

namespace LulCaster.Utility.Service
{
  public class OcrService
  {
    public string ProcessImage(MemoryStream imageStream)
    {
      using (var ocr = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
      using (var pix = Pix.LoadTiffFromMemory(imageStream.ToArray()))
      using (var page = ocr.Process(pix))
      {
        return page.GetText();
      }
    }
  }
}

