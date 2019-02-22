using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Linq;

#if WINFORMS
using BitMap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using System.Drawing.Imaging;
#elif WPF
using BitMap = System.Windows.Media.Imaging.WriteableBitmap;
using Color = System.Windows.Media.Color;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif



namespace DVDScreenSaver {

  public static class LogoLoader {


    public static BitMap GetLogo() {
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = "DVDVideo360.png";
      var allResources = assembly.GetManifestResourceNames();
      string fullName = allResources.Single(str => str.EndsWith(resourceName));
#if WINFORMS
      using (Stream stream = assembly.GetManifestResourceStream(fullName)) { 
        return new BitMap(stream);
      }
#elif WPF
      using (Stream stream = assembly.GetManifestResourceStream(fullName)) {
        var source = new BitmapImage();
        source.BeginInit();
        source.StreamSource = stream;
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.EndInit();
        return new BitMap(source);
      }
#endif
    }


    [StructLayout(LayoutKind.Sequential)]
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

    public static unsafe void RecolorLogo(BitMap img, Color color) {
#if WINFORMS
      BitmapData data = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
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
#elif WPF
      img.Lock();
      BGRAPixel* ptr = (BGRAPixel*)img.BackBuffer;
      for (int len = (img.PixelWidth * img.PixelHeight) - 8, i = 0; i < len; i += 8, ptr += 8) {
        if ((ptr + 0)->A > 0) { (ptr + 0)->SetRGB(color); }
        if ((ptr + 1)->A > 0) { (ptr + 1)->SetRGB(color); }
        if ((ptr + 2)->A > 0) { (ptr + 2)->SetRGB(color); }
        if ((ptr + 3)->A > 0) { (ptr + 3)->SetRGB(color); }
        if ((ptr + 4)->A > 0) { (ptr + 4)->SetRGB(color); }
        if ((ptr + 5)->A > 0) { (ptr + 5)->SetRGB(color); }
        if ((ptr + 6)->A > 0) { (ptr + 6)->SetRGB(color); }
        if ((ptr + 7)->A > 0) { (ptr + 7)->SetRGB(color); }
      }
      img.AddDirtyRect(new System.Windows.Int32Rect(0, 0, img.PixelWidth, img.PixelHeight));
      img.Unlock();
#endif
    }




  }

}
