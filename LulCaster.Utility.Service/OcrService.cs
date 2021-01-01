using Tesseract;
using System.IO;

namespace LulCaster.Utility.Service
{
  public class OcrService : IOcrService
  {
    public string ProcessImage(MemoryStream imageStream)
    {
      using (var ocr = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
      {
        ocr.SetVariable("segment_penalty_garbage", "0");
        ocr.SetVariable("segment_penalty_dict_nonword", "0");
        ocr.SetVariable("segment_penalty_dict_frequent_word", "0");
        ocr.SetVariable("segment_penalty_dict_case_ok", "0");
        ocr.SetVariable("segment_penalty_dict_case_bad", "0");

        using (var pix = Pix.LoadTiffFromMemory(imageStream.ToArray()))
        using (var page = ocr.Process(pix))
        {
          return page.GetText();
        }
      }
    }
  }
}

