using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawTest3.CustomMath;
using DrawTest3.CustomData;
using System.Timers;

namespace DrawTest3
{
    public partial class Form1 : Form
    {
        private Device device;

        public Form1()
        {
            InitializeComponent();

            Graphics drawGraphic = CreateGraphics();

            device = new Device();
            device.Init(MaximumSize, drawGraphic);
        }
    }
}
