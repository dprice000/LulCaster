using System;
using System.Diagnostics;
using System.Linq;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public static class HandleFinder
  {
    public static IntPtr GetWindowsHandle(string processName)
    {
      var titles = Process.GetProcesses().Where(process => process.MainWindowTitle.IndexOf(processName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

      return (titles.Count()) switch
      {
        1 => titles.First().MainWindowHandle,
        0 => throw new ApplicationException("No application was found"),
        _ => throw new ApplicationException("More than one application was found"),
      };
    }
  }
}