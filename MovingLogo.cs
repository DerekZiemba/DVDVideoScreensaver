using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDScreenSaver {

  public struct RectDbl {
    public double X;
    public double Y;
    public double Width;
    public double Height;

    public double Right => X + Width;
    public double Bottom => Y + Height;
    public double Diagonal => Math.Sqrt(Math.Pow(Width, 2) + Math.Pow(Height, 2));

    public RectDbl(double x, double y, double width, double height) {
      this.X = x;
      this.Y = y;
      this.Width = width;
      this.Height = height;
    }
  }

  public enum MoveMode {
    Normal,
    Opposite,
    AllCorners
  }

  public class MovingLogo {

    public event Action<MovingLogo> OnNewPosition;
    public event Action<MovingLogo> OnRedraw;

    private int _colorIdx = -1;
    private Random _rng = new Random();
    private Stopwatch _watch;
    private RectDbl _rect;
    private RectDbl _bounds;
    private double _scale;


    public Bitmap Image;
    public Color[] Colors;
    public MoveMode Mode;
    public bool MoveRight = true;
    public bool MoveDown = true;
    public double Speed = 2;

    public ref RectDbl Rect => ref this._rect;
    public ref RectDbl Bounds => ref this._bounds;


    public MovingLogo(Bitmap image, Color[] colors) {
      this.Image = image;
      this.Colors = colors;
      this.Rect = new RectDbl(0, 0, image.Width, image.Height);
      this._watch = Stopwatch.StartNew();
    }


    public void Animate() {
      double origX = Rect.X, origY = Rect.Y;
      double x = origX, y = origY;
      bool bOutOfBounds = false;
      
      if (Rect.Right >= Bounds.Right) {
        MoveRight = false;
        bOutOfBounds = true;
      } else if (Rect.X <= Bounds.X) {
        MoveRight = true;
        bOutOfBounds = true;
      }

      if (Rect.Bottom >= Bounds.Bottom) {
        MoveDown = false;
        bOutOfBounds = true;
      } else if (Rect.Y <= Bounds.Y) {
        MoveDown = true;
        bOutOfBounds = true;
      }

      if (bOutOfBounds) {
        NextColor();
      }

      switch (Mode) {
        case MoveMode.Normal:
          x += MoveRight ? Speed : -Speed;
          y += MoveDown ? Speed : -Speed;
          break;
        case MoveMode.Opposite:
          double width = Bounds.Width - Rect.Width;
          double height = Bounds.Height - Rect.Height;
          double theta = Math.Atan(height / width);
          double hyppos = Math.Sqrt(Math.Pow(x - Bounds.X, 2) + Math.Pow(y - Bounds.Y, 2));
          x = (hyppos + (MoveRight ? Speed : -Speed) * 2) * Math.Cos(theta);
          y = (hyppos + (MoveDown ? Speed : -Speed) * 2) * Math.Sin(theta);
          break;
        case MoveMode.AllCorners:
          //TODO
          break;
      }

      double step = _watch.ElapsedMilliseconds / 10;
      double moveX = (x - origX) * step;
      double moveY = (y - origY) * step;
      Rect.X += moveX;
      Rect.Y += moveY;

      OnNewPosition(this);
      this._watch.Restart();
    }
    
    public void Rescale(RectDbl bounds, double scale) {
      this._bounds = bounds;
      this._scale = scale;
      double ratio = Image.Width / Image.Height;
      Rect.Height = (Bounds.Diagonal / _scale / ratio);
      Rect.Width = (Rect.Height * ratio);
      Rect.X = Math.Min(Math.Max(Rect.X, 0), Bounds.Width - Rect.Width);
      Rect.Y = Math.Min(Math.Max(Rect.Y, 0), Bounds.Height - Rect.Height);
      Animate();
    }
    
    public void NextColor() {
      RecolorLogo(Image, Colors[_colorIdx = ++_colorIdx % Colors.Length]);
      OnRedraw(this);
    }

    public void PlaceInRandomSpot() {
      Rect.X = Math.Floor((_rng.NextDouble() * (Bounds.Width - Rect.Width)) + 1);
      Rect.Y = Math.Floor((_rng.NextDouble() * (Bounds.Height - Rect.Height)) + 1);
      Animate();
    }


    private struct BGRAPixel {
      public byte B;
      public byte G;
      public byte R;
      public byte A;
      public void SetRGB(Color color) {
        this.B = color.B;
        this.G = color.G;
        this.R = color.R;
      }
    }
    private static unsafe void RecolorLogo(Bitmap img, Color color) {
      BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
      BGRAPixel* ptr = (BGRAPixel*)data.Scan0;
      for (int len = (data.Height * data.Width) - 8, i = 0; i < len; i += 8, ptr += 8) {
        if ((ptr + 0)->A > 0) { (ptr + 0)->SetRGB(color); }
        if ((ptr + 1)->A > 0) { (ptr + 1)->SetRGB(color); }
        if ((ptr + 2)->A > 0) { (ptr + 2)->SetRGB(color); }
        if ((ptr + 3)->A > 0) { (ptr + 3)->SetRGB(color); }
        if ((ptr + 4)->A > 0) { (ptr + 4)->SetRGB(color); }
        if ((ptr + 5)->A > 0) { (ptr + 5)->SetRGB(color); }
        if ((ptr + 6)->A > 0) { (ptr + 6)->SetRGB(color); }
        if ((ptr + 7)->A > 0) { (ptr + 7)->SetRGB(color); }
      }
      img.UnlockBits(data);
    }

  }
}
