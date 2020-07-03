using LulCaster.UI.WPF.Workers;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Pages
{
  /// <summary>
  /// Interaction logic for WireFramePage.xaml
  /// </summary>
  public partial class WireFramePage : Page
  {
    private readonly ScreenCaptureTimer _screenCaptureTimer;
    private const double CAPTURE_TIMER_INTERVAL = 3000; 
    BoundingBoxBrush boundingBoxBrush = new BoundingBoxBrush();

    public WireFramePage()
    {
      InitializeComponent();

      //TODO: Add dependacy injection
      _screenCaptureTimer = new ScreenCaptureTimer(new ScreenCaptureService(), CAPTURE_TIMER_INTERVAL);
      _screenCaptureTimer.ScreenCaptureCompleted += _screenCaptureTimer_ScreenCaptureCompleted;
      _screenCaptureTimer.Start();
    }

    private void _screenCaptureTimer_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      this.Dispatcher?.Invoke(() =>
      {
        CanvasScreenFeed.Children.Clear();
        var imageStream = new MemoryStream(captureArgs.ScreenImageStream);
        var screenCaptureImage = new BitmapImage();
        screenCaptureImage.BeginInit();
        screenCaptureImage.StreamSource = imageStream;
        screenCaptureImage.CacheOption = BitmapCacheOption.OnLoad;
        screenCaptureImage.EndInit();
        screenCaptureImage.Freeze();

        var imageBrush = new ImageBrush(screenCaptureImage);

        CanvasScreenFeed.Background = imageBrush;

        boundingBoxBrush.DrawRectangle();
      });
    }
  }
}