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
        equalization equ;
        [DllImport("AdaptHistEqualize.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern void AdaptHistEqualize(IntPtr Scan0, int Width, int Height, int Stride, int TileX, int TileY, double CutLimit, bool SeparateChannel);
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
            int tilex = 10;
            int tiley = 15;
            double cl = 0.01f;
            bool sc = this.checkBox1.Checked;
            int.TryParse(this.textBox1.Text, out tilex);
            int.TryParse(this.textBox2.Text, out tiley);
            double.TryParse(this.textBox3.Text, out cl);
            this.pictureBox1.Image = equ.toCLAHEImage(tilex,tiley,cl,sc);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    equ = new equalization(of.FileName);
                    this.pictureBox1.Image = equ.toOriImage();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }
    }
}
