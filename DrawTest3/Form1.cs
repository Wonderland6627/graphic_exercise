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
        
        private void OnUpBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(0, 1, 0));
        }

        private void OnDownBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(0, -1, 0));
        }

        private void OnLeftBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(-1, 0, 0));
        }

        private void OnRightBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(1, 0, 0));
        }

        private void OnForwardBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(0, 0, 1));
        }

        private void OnBackBtnClicked(object sender, EventArgs e)
        {
            device.MoveCamera(new Vector3(0, 0, -1));
        }
    }
}
