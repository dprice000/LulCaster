using System.IO;

namespace LulCaster.Utility.Service
{
  public interface IOcrService
  {
    string ProcessImage(MemoryStream imageStream);
  }
}