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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            equalization equ = new equalization("lena.jpg");
            this.pictureBox1.Image = equ.toGrayImage();
            this.pictureBox2.Image = equ.toOriImage();
            //this.pictureBox3.Image = equ.toGrayImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
