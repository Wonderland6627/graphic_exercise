using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrawTest1.CustomMath;
using DrawTest1.Renderer;

namespace DrawTest1
{
    public partial class Form1 : Form
    {
        private Device device;
        private Mesh mesh;
        private Camera camera;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            device = new Device(rendererPanel.Width, rendererPanel.Height);

            mesh = new Mesh("MeshTest", 8, 12);
            mesh.vertices[0] = new Vector3(-1, 1, 1);
            mesh.vertices[1] = new Vector3(1, 1, 1);
            mesh.vertices[2] = new Vector3(-1, -1, 1);
            mesh.vertices[3] = new Vector3(1, -1, 1);
            mesh.vertices[4] = new Vector3(-1, 1, -1);
            mesh.vertices[5] = new Vector3(1, 1, -1);
            mesh.vertices[6] = new Vector3(1, -1, -1);
            mesh.vertices[7] = new Vector3(-1, -1, -1);

            camera = new Camera();
            camera.position = new Vector3(0, 0, 10);
            camera.target = Vector3.zero;
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
            /*Bitmap buffer = new Bitmap(rendererPanel.Width, rendererPanel.Height);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(255, 100, 200, 255);

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    buffer.SetPixel(i, j, c);
                }
            }
            
            e.Graphics.DrawImage(buffer, 0, 0);*/

            device.Clear();
            device.Render(camera, mesh);
            device.Present(e.Graphics);
        }
    }
}
