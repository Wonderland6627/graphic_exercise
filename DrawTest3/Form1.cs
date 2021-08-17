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

            CheckForIllegalCrossThreadCalls = false;

            Graphics drawGraphic = CreateGraphics();

            device = new Device();
            device.Init(MaximumSize, drawGraphic);

            RegistMouseEvent();
        }

        public void RegistMouseEvent()
        {
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseWheel += OnMouseWheel;

            device.OnUpdate += 
                (fps) => 
                { 
                    FPSLabel.Text = string.Format("FPS: {0}",fps.ToString());
                };
        }

        private bool move = false;

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            lastX = e.X;
            lastY = e.Y;
            move = true;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float offset = e.Delta / (1200 * 3f);
            device.UpdateCameraFOV(offset);
        }

        private int lastX = 400;
        private int lastY = 300;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!move)
            {
                return;
            }

            int X = e.X;
            int Y = e.Y;

            int deltaX = X - lastX;
            int deltaY = Y - lastY;

            //Console.WriteLine(deltaX);
            //Console.WriteLine(deltaY);

            device.RotateCamera(deltaY * 0.1f, deltaX * 0.1f);

            lastX = X;
            lastY = Y;
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

            UpdateCameraPosLabel();
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

            LightBtn.Text = value ? "Lighting Off" :"Lighting On";
        }

        private void CuttingBtn_Click(object sender, EventArgs e)
        {
            bool value;

            device.TurnCutting(out value);

            CuttingBtn.Text = value ? "Cutting Off" : "Cutting On";
        }

        private void OnAmbientStrengthInputValueChnaged(object sender, EventArgs e)
        {
            /*float result = 0;
            if (float.TryParse(AmbientStrengthInput.Text, out result))
            {
                float ambient = result;
                ambient = UnityEngine.Mathf.Clamp01(ambient);
                device.SetAmbientStrength(ambient);

                AmbientStrengthInput.Text = ambient.ToString();
            }*/
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

            UpdateCameraPosLabel();

            if (keyData == Keys.J)
            {
                device.RotateCamera(0, 0.8f);
            }
            if (keyData == Keys.L)
            {
                device.RotateCamera(0, -0.8f);
            }
            if (keyData == Keys.K)
            {
                device.RotateCamera(-0.8f, 0);
            }
            if (keyData == Keys.I)
            {
                device.RotateCamera(0.8f, 0);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateCameraPosLabel()
        {
            CameraPosLabel.Text = device.Camera.position.toString();
        }
    }
}
