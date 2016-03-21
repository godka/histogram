using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace histogram
{
    public partial class Form1 : Form
    {
        [DllImport("AdaptHistEqualize.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern void AdaptHistEqualize(IntPtr Scan0, int Width, int Height, int Stride, int TileX, int TileY, double CutLimit, bool SeparateChannel);
        equalization equ;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = equ.toOriImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            equ = new equalization("lena.jpg");
            this.pictureBox1.Image = equ.toOriImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.pictureBox1.Image = equ.toGrayOriImage();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.pictureBox1.Image = equ.toImage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = equ.toGrayImage();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
