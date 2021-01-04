using System.Drawing;
using System.IO;

namespace LulCaster.UI.WPF.Workers.Events.Arguments
{
    internal class ScreenCaptureCompletedArgs
    {
        public byte[] ScreenImageStream { get; set; }
        public Rectangle ScreenBounds { get; set; }
    }
}