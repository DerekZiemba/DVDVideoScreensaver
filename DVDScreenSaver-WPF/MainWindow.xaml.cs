using DVDScreenSaver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DVDScreenSaver_WPF {

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window {
    private const int LogoScale = 6;
    private MovingLogo Logo;
    private DispatcherTimer timer1 = new DispatcherTimer();

    public MainWindow() {
      InitializeComponent();


      WriteableBitmap bitmap = LogoLoader.GetLogo();
      RenderOptions.SetBitmapScalingMode(bitmap, BitmapScalingMode.NearestNeighbor);
      RenderOptions.SetEdgeMode(bitmap, EdgeMode.Aliased);
      
      Logo = new MovingLogo(
        bitmap,
        new Color[] {
          Color.FromRgb(190,0,255),
          Color.FromRgb(255,0,139),
          Color.FromRgb(255,131,0),
          Color.FromRgb(0,38,255),
          Color.FromRgb(255,250,0)
        }
      );

      ImageBrush.ImageSource = Logo.Image;

      Logo.OnNewPosition += (MovingLogo logo) => {
        ref var rect = ref logo.Rect;
        Canvas.SetLeft(LogoBox, rect.X);
        Canvas.SetTop(LogoBox, rect.Y);
        LogoBox.Width = rect.Width;
        LogoBox.Height = rect.Height;
      };

      Logo.OnRedraw += (MovingLogo logo) => {

      };

      SizeChanged += (object obj, SizeChangedEventArgs args) => {
        Logo.Rescale(new RectDbl(0, 0, canvas.ActualWidth, canvas.ActualHeight), LogoScale);
      };

      KeyDown += HandleKeypress;

      timer1.Tick += (object obj, EventArgs args) => {
        Logo.Animate();
      };


      Logo.NextColor();
      Logo.Rescale(new RectDbl(0, 0, canvas.ActualWidth, canvas.ActualHeight), LogoScale);
      Logo.PlaceInRandomSpot();

      timer1.Interval = new TimeSpan(0, 0, 0, 0, 10);
      timer1.Start();

    }

    private void HandleKeypress(object obj, KeyEventArgs args) {
      switch (args.Key) {
        case Key.F11:
        case Key.Enter:
          if (this.WindowStyle != WindowStyle.None) {
            Logo.Speed = 3;
            this.Topmost = true;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
          } else {
            Logo.Speed = 2;
            this.Topmost = false;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
          }
          break;
        case Key.Space:
          Logo.NextMode();
          break;
        case Key.Left:
          Logo.MoveRight = false;
          break;
        case Key.Right:
          Logo.MoveRight = true;
          break;
        case Key.Up:
          Logo.MoveDown = false;
          break;
        case Key.Down:
          Logo.MoveDown = true;
          break;
        case Key.NumPad0:
        case Key.NumPad1:
        case Key.NumPad2:
        case Key.NumPad3:
        case Key.NumPad4:
        case Key.NumPad5:
        case Key.NumPad6:
        case Key.NumPad7:
        case Key.NumPad8:
        case Key.NumPad9:
          Logo.Speed = args.Key - Key.NumPad0;
          break;

      }
    }

  }
}
