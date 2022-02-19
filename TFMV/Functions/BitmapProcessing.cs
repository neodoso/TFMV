using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TFMV.Functions
{
    public class BitmapProcessing
    {

        // returns bitmap with alpha transparency based on the differences between two bitmaps
        public Bitmap get_transp_bitmap(Bitmap bitm_dst, Bitmap bitm_src)
        {
            Rectangle rect = new Rectangle(0, 0, bitm_src.Width, bitm_src.Height);
            PixelFormat PixFormat = PixelFormat.Format32bppPArgb;

            // clone images to change the pixel format
            bitm_src = bitm_src.Clone(rect, PixFormat);
            bitm_dst = bitm_dst.Clone(rect, PixFormat);

            unsafe
            {
                BitmapData DataB = bitm_src.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.ReadWrite, PixFormat);
                BitmapData DataW = bitm_dst.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.ReadOnly, PixFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitm_src.PixelFormat) / 8;

                int heightInPixels = DataB.Height;
                int widthInBytes = DataB.Width * bytesPerPixel;

                byte* ptrFirstPixelB = (byte*)DataB.Scan0;
                byte* ptrFirstPixelW = (byte*)DataW.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* Line_src = ptrFirstPixelB + (y * DataB.Stride);
                    byte* Line_dst = ptrFirstPixelW + (y * DataW.Stride);

                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        // calculate pixel differences between bitmB and bitmW
                        byte alpha = (byte)Math.Abs(((Line_src[x] + Line_src[x + 1] + Line_src[x + 2]) - (Line_dst[x] + Line_dst[x + 1] + Line_dst[x + 2])) / 3);

                        // multiply pixel exposure to get a sharper alpha mask
                        int mult_alpha = Convert.ToInt16(alpha); // * 1.15

                        // todo replace this by a Math. method() to cap the value to 255, if it's faster
                        if (mult_alpha > 255) { mult_alpha = 255; }
                        alpha = (byte)mult_alpha;

                        // invert alpha
                        alpha = (byte)Math.Abs(alpha - 255);

                        // set alpha pixel
                        Line_src[x + 3] = alpha;
                    }
                }
                bitm_src.UnlockBits(DataB);
                bitm_dst.UnlockBits(DataW);
            }
            // return bitmap with alpha mask
            return bitm_src;
        }


        // copies RGB from source to target bitmap
        public Bitmap transfer_rgb(Bitmap bmp_src, Bitmap bmp_dst)
        {
            Rectangle rect = new Rectangle(0, 0, bmp_src.Width, bmp_src.Height);

            PixelFormat PixFormat = PixelFormat.Format32bppPArgb;

            // clone images to change the pixel format
            bmp_src = bmp_src.Clone(rect, PixFormat);
            bmp_dst = bmp_dst.Clone(rect, PixFormat);

            unsafe
            {
                BitmapData Data_src = bmp_src.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.ReadWrite, PixFormat);
                BitmapData Data_dst = bmp_dst.LockBits(new Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.ReadOnly, PixFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp_src.PixelFormat) / 8;

                int heightInPixels = Data_src.Height;
                int widthInBytes = Data_src.Width * bytesPerPixel;

                byte* ptrFirstPixelB = (byte*)Data_src.Scan0;
                byte* ptrFirstPixelW = (byte*)Data_dst.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* Line_src = ptrFirstPixelB + (y * Data_src.Stride);
                    byte* Line_dst = ptrFirstPixelW + (y * Data_dst.Stride);

                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        Line_dst[x] = Line_src[x];
                        Line_dst[x + 1] = Line_src[x + 1];
                        Line_dst[x + 2] = Line_src[x + 2];
                    }
                }
                bmp_src.UnlockBits(Data_src);
                bmp_dst.UnlockBits(Data_dst);
            }
            // return bitmap with alpha mask
            return bmp_dst;
        }

    }
}
