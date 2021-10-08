using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using DrawTest1.CustomMath;
using DrawTest1.Renderer;

using Timer = System.Timers.Timer;

namespace DrawTest1
{
    public partial class Form1 : Form
    {
        private Device device;
        private Mesh mesh;
        private Camera camera;

        private Bitmap bitmap;
        private Bitmap frameBuffer;
        private Graphics graphicsDevice;

        private Timer timer;

        public Form1()
        {
            InitializeComponent();

            InitScene();
            InitDrawing();
            InitSystem();
        }

        private void InitSystem()
        {
            timer = new Timer(100f);

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Start();
        }

        private void InitDrawing()
        {
            bitmap = new Bitmap(rendererPanel.Width, rendererPanel.Height);
            frameBuffer = new Bitmap(rendererPanel.Width, rendererPanel.Height);
            graphicsDevice = Graphics.FromImage(bitmap);
        }

        private void InitScene()
        {
            mesh = new Mesh("MeshTest", 8, 12);

            //mesh.vertices[0] = new Vector3(-1, 1, 1);
            //mesh.vertices[1] = new Vector3(1, 1, 1);
            //mesh.vertices[2] = new Vector3(-1, -1, 1);
            //mesh.vertices[3] = new Vector3(1, -1, 1);
            //mesh.vertices[4] = new Vector3(-1, 1, -1);
            //mesh.vertices[5] = new Vector3(1, 1, -1);
            //mesh.vertices[6] = new Vector3(1, -1, -1);
            //mesh.vertices[7] = new Vector3(-1, -1, -1);

            //mesh.triangles[0] = new Triangle { a = 0, b = 1, c = 2 };
            //mesh.triangles[1] = new Triangle { a = 1, b = 2, c = 3 };
            //mesh.triangles[2] = new Triangle { a = 1, b = 3, c = 6 };
            //mesh.triangles[3] = new Triangle { a = 1, b = 5, c = 6 };
            //mesh.triangles[4] = new Triangle { a = 0, b = 1, c = 4 };
            //mesh.triangles[5] = new Triangle { a = 1, b = 4, c = 5 };

            //mesh.triangles[6] = new Triangle { a = 2, b = 3, c = 7 };
            //mesh.triangles[7] = new Triangle { a = 3, b = 6, c = 7 };
            //mesh.triangles[8] = new Triangle { a = 0, b = 2, c = 7 };
            //mesh.triangles[9] = new Triangle { a = 0, b = 4, c = 7 };
            //mesh.triangles[10] = new Triangle { a = 4, b = 5, c = 6 };
            //mesh.triangles[11] = new Triangle { a = 4, b = 6, c = 7 };

            mesh.vertices[0] = new Vertex(-1, 1, -1);
            mesh.vertices[1] = new Vertex(-1, -1, -1);
            mesh.vertices[2] = new Vertex(1, -1, -1);
            mesh.vertices[3] = new Vertex(1, 1, -1);
            mesh.vertices[4] = new Vertex(-1, 1, 1);
            mesh.vertices[5] = new Vertex(-1, -1, 1);
            mesh.vertices[6] = new Vertex(1, -1, 1);
            mesh.vertices[7] = new Vertex(1, 1, 1);

            mesh.triangles[0] = new Triangle { a = 0, b = 1, c = 2 };
            mesh.triangles[1] = new Triangle { a = 0, b = 2, c = 3 };
            mesh.triangles[2] = new Triangle { a = 7, b = 6, c = 5 };
            mesh.triangles[3] = new Triangle { a = 7, b = 5, c = 4 };
            mesh.triangles[4] = new Triangle { a = 0, b = 4, c = 5 };
            mesh.triangles[5] = new Triangle { a = 0, b = 5, c = 1 };

            mesh.triangles[6] = new Triangle { a = 1, b = 5, c = 6 };
            mesh.triangles[7] = new Triangle { a = 1, b = 6, c = 2 };
            mesh.triangles[8] = new Triangle { a = 2, b = 6, c = 7 };
            mesh.triangles[9] = new Triangle { a = 2, b = 7, c = 3 };
            mesh.triangles[10] = new Triangle { a = 3, b = 7, c = 4 };
            mesh.triangles[11] = new Triangle { a = 3, b = 4, c = 0 };

            camera = new Camera
            {
                position = new Vector3(0, 2.5f, -10),
                target = Vector3.zero
            };
        }

        float translateZ = 0;
        float rotateX = 0;
        float rotateY = 0;
        float rotateZ = 0;

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Clear();

            Matrix model = Matrix.Translate(0, 3, translateZ) * Matrix.RotateY(rotateY) * Matrix.RotateX(rotateX) * Matrix.RotateZ(rotateZ);
            Matrix view = Matrix.LookAtLH(camera.position, camera.target, Vector3.up);
            Matrix projection = Matrix.PerspectiveFovLH(camera.fov, camera.aspectRatio, camera.zNear, camera.zFar);

            Draw(model, view, projection);

            if (graphicsDevice == null)
            {
                graphicsDevice = CreateGraphics();
            }
            graphicsDevice.Clear(Color.Black);
            graphicsDevice.DrawImage(frameBuffer, 0, 0);

            Console.WriteLine("时间 " + e.SignalTime);
        }

        private void Draw(Matrix model, Matrix view, Matrix projection)
        {
            for (int i = 0; i < mesh.vertices.Length - 2; i += 3)
            {
                /*DrawTriangle(mesh.vertices[i]
                            , mesh.vertices[i + 1]
                            , mesh.vertices[i + 2]
                            , model, view, projection);*/

                DrawTriangle(mesh.vertices[i], mesh.vertices[i + 1], mesh.vertices[i + 2]);
            }
        }

        private void DrawTriangle(Vertex point1, Vertex point2, Vertex point3)
        {
            SetPixel((int)point1.position.x, (int)point1.position.y, Color.Red);
            SetPixel((int)point2.position.x, (int)point2.position.y, Color.Red);
            SetPixel((int)point3.position.x, (int)point3.position.y, Color.Red);
            /*DrawLine(point1.position, point2.position);
            DrawLine(point2.position, point3.position);
            DrawLine(point3.position, point1.position);*/
        }

        private void DrawLine(Vector3 point1, Vector3 point2)
        {
            float x = point1.x;
            float y = point1.y;
            int maxAbs = (int)Math.Max(Math.Abs(point1.x - point2.x), Math.Abs(point1.y - point2.y));
            float xIncre = (point2.x - point1.x) / maxAbs;
            float yIncre = (point2.y - point1.y) / maxAbs;
            for (int i = 0; i < maxAbs; i++)
            {
                SetPixel((int)x, (int)y, Color.Orange);
                x += xIncre;
                y += yIncre;
            }
        }

        private void SetPixel(int x, int y, Color color)
        {
            if (x >= 0 && y >= 0 && x < frameBuffer.Width && y < frameBuffer.Height)
            {
                frameBuffer.SetPixel(x, y, color);
            }

            Console.WriteLine("x {0} y {0} ", x, y);
        }

        private void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Matrix model, Matrix view, Matrix projection)
        {
            Model2World(model, v1);
            Model2World(model, v2);
            Model2World(model, v3);

            World2Camera(view, v1);
            World2Camera(view, v2);
            World2Camera(view, v3);

            Camera2Projection(projection, v1);
            Camera2Projection(projection, v2);
            Camera2Projection(projection, v3);
        }

        /// <summary>
        /// To 世界坐标
        /// </summary>
        private void Model2World(Matrix matrix, Vertex vertex)
        {
            vertex.position = matrix * vertex.position;
        }

        /// <summary>
        /// 世界坐标转相机坐标
        /// </summary>
        private void World2Camera(Matrix matrix, Vertex vertex)
        {
            vertex.position = matrix * vertex.position;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Camera2Projection(Matrix matrix, Vertex vertex)
        {
            vertex.position = matrix * vertex.position;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "123";
        }

        private void PaintPoint(object sender, PaintEventArgs e)
        {
            /*Clear();

            Draw(null, null, null);
            if (graphicsDevice == null)
            {
                graphicsDevice = e.Graphics;
            }
            graphicsDevice.Clear(Color.Black);
            graphicsDevice.DrawImage(frameBuffer, 0, 0);*/
        }

        private void OnLeftMoveBtnClick(object sender, EventArgs e)
        {
            camera.position.x--;
            Console.WriteLine(camera.position.x);
        }

        private void Clear()
        {
            graphicsDevice.Clear(Color.Black);
        }
    }
}