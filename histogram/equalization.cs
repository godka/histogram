using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace histogram
{
    class equalization
    {
        //private Bitmap img;
        private int width, height;
        private string m_filename;
        private byte[] m_data;
        [DllImport("AdaptHistEqualize.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern void AdaptHistEqualize(byte[] Scan0, int Width, int Height, int Stride, int TileX, int TileY, double CutLimit, bool SeparateChannel);

        public equalization(string filename)
        {
            m_filename = filename;
            m_data = readFromFile();
        }
        public Image toCLAHEImage(int tilex, int tiley, double cl, bool sc)
        {

            byte[] mimage = new byte[width * height * 3];
            int j = 0;
            for (int i = 0; i < width * height * 4; i += 4)
            {
                mimage[j++] = m_data[i];
                mimage[j++] = m_data[i + 1];
                mimage[j++] = m_data[i + 2];
            }
            AdaptHistEqualize(mimage, width, height, width * 3,
                tilex, tiley, cl * 1f, sc);
            Bitmap Img = new Bitmap(width, height);
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(mimage, 0, intptr, mimage.Length);
            Img.UnlockBits(data0);

            //data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //Img.UnlockBits(data0);
            return Img;
        }
        private byte[] readFromFile()
        {
            Bitmap image = new Bitmap(m_filename);
            width = image.Width;
            height = image.Height;
            byte[] tmpbyte = new byte[image.Width * image.Height * 4];
            //获取图像的BitmapData对像 
            BitmapData data0 = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(intptr, tmpbyte, 0, tmpbyte.Length);
            image.UnlockBits(data0);
            //img = image;
            image.Dispose();
            return tmpbyte;
        }
        private byte[] toGray()
        {
            byte[] tmpbyte = new byte[width * height];
            int j = 0;
            for (int i = 0; i < width * height * 4; i+=4)
            {
                double tmp = 0.299 * m_data[i] + 0.517 * m_data[i + 1] + 0.114 * m_data[i + 2];
                tmpbyte[j++] = (byte)tmp;

            }
            return tmpbyte;
        }
        private byte[] getPlane(int index)
        {
            byte[] tmpbyte = new byte[width * height];
            int j = 0;
            for (int i = 0; i < width * height * 4; i += 4)
            {
                tmpbyte[j++] = m_data[i + index];

            }
            return tmpbyte;

        }
        private byte[] createnewhis(byte[] a_index,byte[] his_image)
        {
            byte[] his_new = new byte[width * height];
            for (int i = 0; i < width * height; i++)
            {
                his_new[i] = a_index[his_image[i]];
            }
            return his_new;
        }

        private byte[] singleStep(byte[] mbyte)
        {
            int[] his = new int[256];
            byte[] near_p = new byte[256];
            long temp = 0;
            for (int i = 0; i < mbyte.Length; i++)
                his[mbyte[i]]++;
            for (int i = 0; i < 256; i++)
            {
                temp += his[i];
                long mt = temp << 8;
                near_p[i] = (byte)(mt / mbyte.LongLength);
            }
            for (int i = 0; i < mbyte.Length; i++)
                mbyte[i] = near_p[mbyte[i]];
            return mbyte;
        }
        public Image toGrayImage()
        {
            Bitmap Img = new Bitmap(width, height);
            var simpleret = singleStep(toGray());
            byte[] s = new byte[width * height * 4];
            for (int i = 0; i < width * height; i++)
            {
                s[i * 4 + 0] = simpleret[i];
                s[i * 4 + 1] = simpleret[i];
                s[i * 4 + 2] = simpleret[i];
                s[i * 4 + 3] = 255;
            }
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(s, 0, intptr, s.Length);
            Img.UnlockBits(data0);
            return Img;
        }
        public Image toGrayOriImage()
        {
            Bitmap Img = new Bitmap(width, height);
            var simpleret = (toGray());
            byte[] s = new byte[width * height * 4];
            for (int i = 0; i < width * height; i++)
            {
                s[i * 4 + 0] = simpleret[i];
                s[i * 4 + 1] = simpleret[i];
                s[i * 4 + 2] = simpleret[i];
                s[i * 4 + 3] = 255;
            }
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(s, 0, intptr, s.Length);
            Img.UnlockBits(data0);
            return Img;
        }
        public Image toOriImage()
        {
            Bitmap Img = new Bitmap(width, height);
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(m_data, 0, intptr, this.m_data.Length);
            Img.UnlockBits(data0);
            return Img;
        }
        public Image toRGBImage()
        {
            Bitmap Img = new Bitmap(width, height);
            byte[] s = new byte[width * height * 3];
            int j = 0;
            for (int i = 0; i < width * height * 4; i += 4)
            {
                s[j++] = m_data[i];
                s[j++] = m_data[i + 1];
                s[j++] = m_data[i + 2];
            }
            singleStep(s);
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(s, 0, intptr, s.Length);
            Img.UnlockBits(data0);
            return Img;
            //return toImage(new Rectangle(0, 0, width, height));
        }
        public Image toImage()
        {
            return toImage(new Rectangle(0, 0, width, height));
        }
        public Image toImage(Rectangle rect)
        {
            Bitmap Img = new Bitmap(width, height);
            var rr = singleStep(getPlane(0));
            var gg = singleStep(getPlane(1));
            var bb = singleStep(getPlane(2));
            byte[] s = new byte[width * height * 4];
            for (int i = 0; i < width * height; i ++)
            {
                s[i * 4 + 0] = rr[i];
                s[i * 4 + 1] = gg[i];
                s[i * 4 + 2] = bb[i];
                s[i * 4 + 3] = 255;
            }
            BitmapData data0 = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr intptr = data0.Scan0;
            Marshal.Copy(s, 0, intptr, s.Length);
            Img.UnlockBits(data0);
            return Img;
        }
    }
}
