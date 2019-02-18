using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DVDScreenSaver {
	public partial class DVDScreenSaver : Form {

		private enum Mode {
			Normal,
			Opposite,
			AllCorners
		}

    private Bitmap[] Logos { get; set; }
		private Random RNG = new Random();
		private int logoIndex = 0;
		private bool bRight = true;
		private bool bDown = true;
		private Mode eMode = Mode.Normal;


		public DVDScreenSaver() {
			InitializeComponent();
			Logos = new Bitmap[]{
				ColorLogo(Color.FromArgb(190,0,255)),
				ColorLogo(Color.FromArgb(255,0,139)),
				ColorLogo(Color.FromArgb(255,131,0)),
				ColorLogo(Color.FromArgb(0,38,255)),
				ColorLogo(Color.FromArgb(255,250,0))
			};

			LogoBox.Image = Logos[logoIndex];
			ScaleLogo();
			PlaceInRandomSpot();

      ClientSizeChanged += HandleWindowResize;
      Click += (object obj, EventArgs args) => NextMode();
			timer1.Tick += (object obj, EventArgs args) => Animate();

			timer1.Start();
		}

		private void Animate() {
			int speed = 2;

			Point location = LogoBox.Location;
			ref Point loc = ref location;

			if(loc.Y + LogoBox.Height >= ClientSize.Height) {
				bDown = false;
				SwitchLogo();
			}
			if(loc.Y <= 0) {
				bDown = true;
				SwitchLogo();
			}
			if(loc.X + LogoBox.Width >= ClientSize.Width) {
				bRight = false;
				SwitchLogo();
			}
			if(loc.X <= 0) {
				bRight = true;
				SwitchLogo();
			}

			switch(eMode) {
				case Mode.Normal:
					loc.X += bRight ? speed : -speed;
					loc.Y += bDown ? speed : -speed;
					break;
				case Mode.Opposite:
					double hyppos = Math.Sqrt(Math.Pow(loc.X, 2) + Math.Pow(loc.Y, 2));
					double nextpos = hyppos + (bRight && bDown ? speed : -speed) * 2;
					double theta = Math.Atan((double)ClientSize.Height/ClientSize.Width);
					double x =nextpos * Math.Cos(theta);
					double y =nextpos * Math.Sin(theta);
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
			switch(eMode) {
				case Mode.Normal:
					eMode = Mode.Opposite;
					break;
				case Mode.Opposite:
					eMode = Mode.AllCorners;
					PlaceInRandomSpot();
					break;
				case Mode.AllCorners:
					eMode = Mode.Normal;
					break;
			}
		}

		private void PlaceInRandomSpot() {
			var randomX = (int) Math.Floor((RNG.NextDouble() * (ClientSize.Width - LogoBox.Width)) + 1);
			var randomY = (int) Math.Floor((RNG.NextDouble() * (ClientSize.Height - LogoBox.Height)) + 1);
			LogoBox.Location = new Point(randomX, randomY);
		}

		private void SwitchLogo() {
			logoIndex++;
			logoIndex = logoIndex % Logos.Length;
			LogoBox.Image = Logos[logoIndex];
		}

		private void ScaleLogo() {
      const int ratio = 6;
      double aspectratio = Properties.Resources.DVDVideo.Width/ Properties.Resources.DVDVideo.Height;		
			double winsize = Math.Sqrt(Math.Pow(ClientSize.Width, 2) + Math.Pow(ClientSize.Height, 2));
			LogoBox.Height = (int)(winsize / ratio / aspectratio);
			LogoBox.Width = (int)(LogoBox.Height * aspectratio);
		}

    private void HandleWindowResize(object obj, EventArgs args) {
      ScaleLogo();
      Point location = LogoBox.Location;
      ref Point loc = ref location;
      if (loc.X < 0 || loc.X + LogoBox.Width > ClientSize.Width || loc.Y < 0 || loc.Y + LogoBox.Height > ClientSize.Height) {
        PlaceInRandomSpot();
      }
    }


    private static unsafe Bitmap ColorLogo(Color color) {
			Bitmap img = new Bitmap(Properties.Resources.DVDVideo);
			BitmapData data = img.LockBits(new Rectangle(0,0,img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
			byte* ptr = (byte*) data.Scan0;
			for(int j = 0; j < data.Height; j++) {
				byte* scanPtr = ptr + (j * data.Stride);
				for(int i = 0; i < data.Stride; i += 4, scanPtr += 4) {
					Color pixel = Color.FromArgb(*(scanPtr+3), *(scanPtr+2), *(scanPtr+1), *scanPtr);
					if(pixel.R == 0 && pixel.G == 0 && pixel.B == 0 && pixel.A > 0) {
						(*scanPtr, *(scanPtr + 1), *(scanPtr + 2), *(scanPtr + 3)) = (color.B, color.G, color.R, pixel.A);
					}
				}
			}
			img.UnlockBits(data);
			return img;
		}
	}
}
