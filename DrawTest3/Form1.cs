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

            RegistMouseEvent();
        }

        public void RegistMouseEvent()
        {
            MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.X);
            Console.WriteLine(e.Y);
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

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            device.ResetCamera();
        }

        private void PointModeBtn_Click(object sender, EventArgs e)
        {
            device.SwitchDisplayMode(DisplayMode.Point);
        }

        private void LineModeBtn_Click(object sender, EventArgs e)
        {
            device.SwitchDisplayMode(DisplayMode.Line);
        }

        private void SurfaceModeBtn_Click(object sender, EventArgs e)
        {
            device.SwitchDisplayMode(DisplayMode.Surface);
        }

        private void TextureModeBtn_Click(object sender, EventArgs e)
        {
            device.SwitchDisplayMode(DisplayMode.Texture);
        }

        private void LightBtn_Click(object sender, EventArgs e)
        {
            bool value;

            device.TurnLighting(out value);

            LightBtn.Text = value ? "Lighting On" :"Lighting Off";
        }

        private void OnAmbientStrengthInputValueChnaged(object sender, EventArgs e)
        {
            float ambient = float.Parse(AmbientStrengthInput.Text);
            ambient = UnityEngine.Mathf.Clamp01(ambient);
            device.SetAmbientStrength(ambient);

            AmbientStrengthInput.Text = ambient.ToString();
        }

        private void AmbientStrength_Click(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.W)
            {
                device.MoveCamera(Camera_Movement_Type.Forward);
            }
            if (keyData == Keys.S)
            {
                device.MoveCamera(Camera_Movement_Type.Backward);
            }
            if (keyData == Keys.A)
            {
                device.MoveCamera(Camera_Movement_Type.Left);
            }
            if (keyData == Keys.D)
            {
                device.MoveCamera(Camera_Movement_Type.Right);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
