using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Reflection;


namespace DVDScreenSaver {
  public partial class DVDScreenSaver: Form {
    private const int LogoScale = 6;
    private MovingLogo Logo;

    public DVDScreenSaver() {
      InitializeComponent();
      this.DoubleBuffered = true;

      Logo = new MovingLogo(
        LogoLoader.GetLogo(),
        new Color[] {
          Color.FromArgb(190,0,255),
          Color.FromArgb(255,0,139),
          Color.FromArgb(255,131,0),
          Color.FromArgb(0,38,255),
          Color.FromArgb(255,250,0)
        }
      );

      LogoBox.Image = Logo.Image;

      Logo.OnNewPosition += (MovingLogo logo) => {
        ref var rect = ref logo.Rect;
        LogoBox.SetBounds((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
      };

      Logo.OnRedraw += (MovingLogo logo) => {
        LogoBox.Invalidate();
      };
      
      ClientSizeChanged += (object obj, EventArgs args) => {
        Logo.Rescale(new RectDbl(0, 0, ClientSize.Width, ClientSize.Height), LogoScale);
      };

      KeyDown += HandleKeypress;

      timer1.Tick += (object obj, EventArgs args) => {
        Logo.Animate();
      };


      Logo.NextColor();
      Logo.Rescale(new RectDbl(0, 0, ClientSize.Width, ClientSize.Height), LogoScale);
      Logo.PlaceInRandomSpot();

      timer1.Start();
    }


    private void HandleKeypress(object obj, KeyEventArgs args) {
      switch (args.KeyCode) {
        case Keys.F11:
        case Keys.Enter:
          if (this.FormBorderStyle == FormBorderStyle.Sizable) {
            Logo.Speed = 3;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
          } else {
            Logo.Speed = 2;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
          }
          break;
        case Keys.Space:
          Logo.NextMode();
          break;
        case Keys.Left:
          Logo.MoveRight = false;
          break;
        case Keys.Right:
          Logo.MoveRight = true;
          break;
        case Keys.Up:
          Logo.MoveDown = false;
          break;
        case Keys.Down:
          Logo.MoveDown = true;
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
          Logo.Speed = args.KeyCode - Keys.NumPad0;
          break;

      }
    }





  }
}
