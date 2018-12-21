using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DVDScreenSaver {
	public partial class Form1 : Form {

		public Bitmap[] Logos { get; set; }
		private int logoIndex = 0;
		private bool bRight = true;
		private bool bDown = true;

		public Form1() {
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
			ClientSizeChanged += (object obj, EventArgs args) => ScaleLogo();

			//var rng = new Random();
			//var randomX = (int) Math.Floor((rng.NextDouble() * ClientSize.Width) + 1);
			//var randomY = (int) Math.Floor((rng.NextDouble() * ClientSize.Height) + 1);
			//LogoBox.Location = new Point(randomX, randomY);

			timer1.Tick += (object obj, EventArgs args) => Animate();
			timer1.Start();
		}



		private void Animate() {
			const int speed = 3;

			Point location = LogoBox.Location;
			ref Point loc = ref location;
			loc.X = LogoBox.Location.X;
			loc.Y = LogoBox.Location.Y;

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

			//loc.X += bRight ? speed : -speed;
			//loc.Y += bDown ? speed : -speed;

			double hyppos = Math.Sqrt(Math.Pow(loc.X, 2) + Math.Pow(loc.Y, 2));
			double nextpos = hyppos + (bRight && bDown ? speed : -speed) * 2;
			double theta = Math.Atan((double)ClientSize.Height/ClientSize.Width);
			double x =nextpos * Math.Cos(theta);
			double y =nextpos * Math.Sin(theta);
			loc.X = (int)x;
			loc.Y = (int)y;

			LogoBox.Location = loc;
		}

		private void SwitchLogo() {
			logoIndex++;
			logoIndex = logoIndex % Logos.Length;
			LogoBox.Image = Logos[logoIndex];
		}

		private void ScaleLogo() {
			const double aspectratio = 2400f/1051f;
			const int ratio =  6;
			double winsize = Math.Sqrt(Math.Pow(ClientSize.Width, 2) + Math.Pow(ClientSize.Height, 2));
			LogoBox.Height = (int)(winsize / ratio / aspectratio);
			LogoBox.Width = (int)(LogoBox.Height * aspectratio);
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
						(*scanPtr, *(scanPtr + 1), *(scanPtr + 2), *(scanPtr + 3)) = (color.B, color.G, color.R, color.A);
					}
				}
			}
			img.UnlockBits(data);
			return img;
		}
	}
}
