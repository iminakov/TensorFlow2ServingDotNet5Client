using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace TensorFlowServingClient.Utils
{
	public class ImageUtils
	{
		public static int[][] ConvertImageStreamToDimArraysGrayScale(Bitmap bitmap)
		{
			var bitmapArray = BitmapToByteArrayGrayScale(bitmap);
			using (var memoryStream = new MemoryStream(bitmapArray))
			{
				memoryStream.Position = 0;
				return ConvertImageDataToDimArraysGreyScaled(bitmap.Height, bitmap.Width, memoryStream);
			}
		}

		public static (int, int, int)[][] ConvertImageStreamToDimArraysColor(Bitmap bitmap)
		{
			var bitmapArray = BitmapToByteArrayColor(bitmap);
			using (var memoryStream = new MemoryStream(bitmapArray))
			{
				memoryStream.Position = 0;
				return ConvertImageDataToDimArraysColor(bitmap.Height, bitmap.Width, memoryStream);
			}
		}

		public static int[][] ConvertImageStreamToDimArraysGrayScale(Stream stream)
		{
			using (var bitmap = new Bitmap(stream))
			{
				var bitmapArray = BitmapToByteArrayGrayScale(bitmap);
				using (var memoryStream = new MemoryStream(bitmapArray))
				{
					memoryStream.Position = 0;
					return ConvertImageDataToDimArraysGreyScaled(bitmap.Height, bitmap.Width, memoryStream);
				}
			}
		}

		private static int[][] ConvertImageDataToDimArraysGreyScaled(int numRows, int numCols, MemoryStream stream)
		{
			var imageMatrix = new int[numRows][];
			for (int row = 0; row < numRows; row++)
			{
				imageMatrix[row] = new int[numCols];
				for (int col = 0; col < numCols; ++col)
				{
					imageMatrix[row][col] = stream.ReadByte();
				}
			}
			return imageMatrix;
		}

		private static (int, int, int)[][] ConvertImageDataToDimArraysColor(int numRows, int numCols, MemoryStream stream)
		{
			var imageMatrix = new (int, int, int)[numRows][];
			for (int row = 0; row < numRows; row++)
			{
				imageMatrix[row] = new (int, int, int)[numCols];
				for (int col = 0; col < numCols; ++col)
				{
					imageMatrix[row][col] = (stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
				}
			}
			return imageMatrix;
		}

		private static byte[] BitmapToByteArrayGrayScale(Bitmap bitmap)
		{
			BitmapData bmpdata = null;

			try
			{
				bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				int numbytes = bmpdata.Stride * bitmap.Height;
				var bytedata = new byte[numbytes];
				var ptr = bmpdata.Scan0;
				Marshal.Copy(ptr, bytedata, 0, numbytes);

				if(bitmap.PixelFormat == PixelFormat.Format24bppRgb)
				{
					var byteData8BFormat = new byte[bytedata.Length / 3];

					for(int i = 0; i < byteData8BFormat.Length; i++)
					{
						byteData8BFormat[i] = (byte)((bytedata[i * 3] + bytedata[i * 3 + 1] + bytedata[i * 3 + 2]) / 3);
					}

					return byteData8BFormat;
				}
				else if(bitmap.PixelFormat == PixelFormat.Format32bppArgb)
				{
					var byteData8BFormat = new byte[bytedata.Length / 4];

					for (int i = 0; i < byteData8BFormat.Length; i++)
					{
						byteData8BFormat[i] = (byte)((bytedata[i * 4 + 1] + bytedata[i * 4 + 2] + bytedata[i * 4 + 3]) / 3);
					}

					return byteData8BFormat;
				}


				return bytedata;
			}
			finally
			{
				if (bmpdata != null)
					bitmap.UnlockBits(bmpdata);
			}
		}

		private static byte[] BitmapToByteArrayColor(Bitmap bitmap)
		{
			BitmapData bmpdata = null;

			try
			{
				bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				int numbytes = bmpdata.Stride * bitmap.Height;
				var bytedata = new byte[numbytes];
				var ptr = bmpdata.Scan0;
				Marshal.Copy(ptr, bytedata, 0, numbytes);
				return bytedata;
			}
			finally
			{
				if (bmpdata != null)
					bitmap.UnlockBits(bmpdata);
			}
		}

		public static Bitmap ResizeImage(Image image, int width, int height, int src_width, int src_height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.FillRectangle(new SolidBrush(Color.Black), destRect);
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, src_width, src_height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
