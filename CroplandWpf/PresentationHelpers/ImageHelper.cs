using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CroplandWpf.PresentationHelpers
{
	public class ImageHelper
	{
		public static double CalculateAverageLightness(BitmapSource source)
		{
			Bitmap bm = ConvertBitmapSourceToBitmap(source);
			double lum = 0;
			Bitmap tmpBmp = new Bitmap(bm);
			int width = bm.Width;
			int height = bm.Height;
			int bppModifier = bm.PixelFormat == PixelFormat.Format24bppRgb ? 3 : 4;

			BitmapData srcData = tmpBmp.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
			int stride = srcData.Stride;
			IntPtr scan0 = srcData.Scan0;

			//Luminance (standard, objective): (0.2126*R) + (0.7152*G) + (0.0722*B)
			//Luminance (perceived option 1): (0.299*R + 0.587*G + 0.114*B)
			//Luminance (perceived option 2, slower to calculate): sqrt( 0.241*R^2 + 0.691*G^2 + 0.068*B^2 )

			unsafe
			{
				byte* p = (byte*)(void*)scan0;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int idx = (y * stride) + x * bppModifier;
						lum += (0.299 * p[idx + 2] + 0.587 * p[idx + 1] + 0.114 * p[idx]);
					}
				}
			}
			tmpBmp.UnlockBits(srcData);
			tmpBmp.Dispose();

			double avgLum = lum / (width * height);
			return avgLum / 255.0;
		}

		public static Bitmap ConvertBitmapSourceToBitmap(BitmapSource source)
		{
			//convert image format
			FormatConvertedBitmap src = new FormatConvertedBitmap();
			src.BeginInit();
			src.Source = source;
			src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
			src.EndInit();

			//copy to bitmap
			Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, PixelFormat.Format32bppArgb);
			BitmapData data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
			bitmap.UnlockBits(data);

			return bitmap;
		}
	}
}
