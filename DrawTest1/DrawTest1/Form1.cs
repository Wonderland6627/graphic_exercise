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
            label1.Text = checkBox1.Checked.ToString();
        }
    }
}
