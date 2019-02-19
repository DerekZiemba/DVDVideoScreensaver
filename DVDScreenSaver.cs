using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DVDScreenSaver {
  public partial class DVDScreenSaver: Form {

    private enum Mode {
      Normal,
      Opposite,
      AllCorners
    }

    //private Bitmap[] Logos { get; set; }
    private static Color[] Colors = new Color[] {
      Color.FromArgb(190,0,255),
      Color.FromArgb(255,0,139),
      Color.FromArgb(255,131,0),
      Color.FromArgb(0,38,255),
      Color.FromArgb(255,250,0)
    };

    private Random RNG = new Random();
    private int logoIndex = -1;
    private bool bRight = true;
    private bool bDown = true;
    private Mode eMode = Mode.Normal;
    private int nSpeed = 2;

    public DVDScreenSaver() {
      InitializeComponent();

      SwitchLogo();
      ScaleLogo();
      PlaceInRandomSpot();

      KeyDown += HandleKeypress;
      ClientSizeChanged += HandleWindowResize;
      timer1.Tick += (object obj, EventArgs args) => Animate();

      timer1.Start();
    }

    private void Animate() {
      Point location = LogoBox.Location;
      ref Point loc = ref location;

      if (loc.Y + LogoBox.Height > ClientSize.Height) {
        bDown = false;
        SwitchLogo();
      } else if (loc.Y < 0) {
        bDown = true;
        SwitchLogo();
      }
      if (loc.X + LogoBox.Width > ClientSize.Width) {
        bRight = false;
        SwitchLogo();
      } else if (loc.X < 0) {
        bRight = true;
        SwitchLogo(); ;
      }

      int speed = nSpeed;
      switch (eMode) {
        case Mode.Normal:
          loc.X += bRight ? speed : -speed;
          loc.Y += bDown ? speed : -speed;
          break;
        case Mode.Opposite:
          double height = ClientSize.Height - LogoBox.Height;
          double width = ClientSize.Width - LogoBox.Width;
          double theta = Math.Atan(height / width);
          double hyppos = Math.Sqrt(Math.Pow(loc.X, 2) + Math.Pow(loc.Y, 2));
          double nextx = hyppos + (bRight ? speed : -speed) * 2;
          double nexty = hyppos + (bDown ? speed : -speed) * 2;
          double x = nextx * Math.Cos(theta);
          double y = nexty * Math.Sin(theta);
          loc.X = (int)x;
          loc.Y = (int)y;
          break;
        case Mode.AllCorners:
          //TODO
          break;

      }

      LogoBox.Location = loc;
    }

    private void NextMode() {
      switch (eMode) {
        case Mode.Normal:
          eMode = Mode.Opposite;
          break;
        case Mode.Opposite:
          eMode = Mode.Normal;
          PlaceInRandomSpot();
          break;
        case Mode.AllCorners:
          eMode = Mode.Normal;
          break;
      }
    }

    private void PlaceInRandomSpot() {
      var randomX = (int)Math.Floor((RNG.NextDouble() * (ClientSize.Width - LogoBox.Width)) + 1);
      var randomY = (int)Math.Floor((RNG.NextDouble() * (ClientSize.Height - LogoBox.Height)) + 1);
      LogoBox.Location = new Point(randomX, randomY);
    }

    private void SwitchLogo() {
      logoIndex++;
      logoIndex = logoIndex % Colors.Length;
      RecolorLogo((Bitmap)LogoBox.Image, Colors[logoIndex]);
      LogoBox.Invalidate();
    }

    private void ScaleLogo() {
      const int scale = 6;
      double aspectratio = Properties.Resources.DVDVideo.Width / Properties.Resources.DVDVideo.Height;
      double winsize = Math.Sqrt(Math.Pow(ClientSize.Width, 2) + Math.Pow(ClientSize.Height, 2));
      LogoBox.Height = (int)(winsize / scale / aspectratio);
      LogoBox.Width = (int)(LogoBox.Height * aspectratio);
    }

    private void HandleWindowResize(object obj, EventArgs args) {
      ScaleLogo();
      Point location = LogoBox.Location;
      ref Point loc = ref location;
      loc.X = Math.Min(Math.Max(loc.X, 0), ClientSize.Width - LogoBox.Width);
      loc.Y = Math.Min(Math.Max(loc.Y, 0), ClientSize.Height - LogoBox.Height);
      LogoBox.Location = loc;
    }

    private void HandleKeypress(object obj, KeyEventArgs args) {
      switch (args.KeyCode) {
        case Keys.F11:
        case Keys.Enter:
          if (this.FormBorderStyle == FormBorderStyle.Sizable) {
            this.nSpeed = 3;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
          } else {
            this.nSpeed = 2;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
          }
          break;
        case Keys.Space:
          NextMode();
          break;
        case Keys.Left:
          bRight = false;
          break;
        case Keys.Right:
          bRight = true;
          break;
        case Keys.Up:
          bDown = false;
          break;
        case Keys.Down:
          bDown = true;
          break;
        case Keys.NumPad0:
        case Keys.NumPad1:
        case Keys.NumPad2:
        case Keys.NumPad3:
        case Keys.NumPad4:
        case Keys.NumPad5:
        case Keys.NumPad6:
        case Keys.NumPad7:
        case Keys.NumPad8:
        case Keys.NumPad9:
          nSpeed = args.KeyCode - Keys.NumPad0;
          break;

      }
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
      for (int len = (data.Height * data.Width) - 4, i = 0; i < len; i += 4, ptr += 4) {
        if ((ptr + 0)->A > 0) { (ptr + 0)->SetRGB(color); }
        if ((ptr + 1)->A > 0) { (ptr + 1)->SetRGB(color); }
        if ((ptr + 2)->A > 0) { (ptr + 2)->SetRGB(color); }
        if ((ptr + 3)->A > 0) { (ptr + 3)->SetRGB(color); }
      }
      img.UnlockBits(data);
    }

  }
}
