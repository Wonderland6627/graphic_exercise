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

            mesh.triangles[0] = new Triangle { a = 0, b = 1, c = 2 };
            mesh.triangles[1] = new Triangle { a = 1, b = 2, c = 3 };
            mesh.triangles[2] = new Triangle { a = 1, b = 3, c = 6 };
            mesh.triangles[3] = new Triangle { a = 1, b = 5, c = 6 };
            mesh.triangles[4] = new Triangle { a = 0, b = 1, c = 4 };
            mesh.triangles[5] = new Triangle { a = 1, b = 4, c = 5 };

            mesh.triangles[6] = new Triangle { a = 2, b = 3, c = 7 };
            mesh.triangles[7] = new Triangle { a = 3, b = 6, c = 7 };
            mesh.triangles[8] = new Triangle { a = 0, b = 2, c = 7 };
            mesh.triangles[9] = new Triangle { a = 0, b = 4, c = 7 };
            mesh.triangles[10] = new Triangle { a = 4, b = 5, c = 6 };
            mesh.triangles[11] = new Triangle { a = 4, b = 6, c = 7 };

            camera = new Camera();
            camera.position = new Vector3(1.5f, 2.5f, 10);
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
