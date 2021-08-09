using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrawTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "123";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PaintPoint(object sender, PaintEventArgs e)
        {
            Bitmap buffer = new Bitmap(rendererPanel.Width, rendererPanel.Height);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(255, 100, 200, 255);

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    buffer.SetPixel(i, j, c);
                }
            }

            e.Graphics.DrawImage(buffer, 0, 0);
        }
    }
}
