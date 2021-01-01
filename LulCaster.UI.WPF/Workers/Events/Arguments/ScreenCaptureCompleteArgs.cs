using System.Drawing;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers.Events.Arguments
{
    internal class ScreenCaptureCompletedArgs
    {
        public byte[] ScreenImageStream { get; set; }
        public Rectangle ScreenBounds { get; set; }
    }
}