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
    public class Device
    {
        private Bitmap texture;
        private Bitmap frameBuffer;
        private Graphics frameGraphics;

        private float[,] zBuffer;

        private Mesh mesh;
        private Camera camera;

        private Light light;
        private DrawTest3.CustomData.Color ambientColor;//环境光颜色

        private Size windowSize;
        private Graphics drawGraphic;

        public void Init(Size size, Graphics board)
        {
            windowSize = size;
            drawGraphic = board;

            InitSystem();

            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 60f);
            mainTimer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();
        }

        private void InitSystem()
        {
            /*System.Drawing.Image image = System.Drawing.Image.FromFile("");
            texture = new Bitmap(image, 256, 256);*/

            /*try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile("../../Texture/texture.jpg");
                texture = new Bitmap(img, 256, 256);
            }
            catch (Exception)
            {
                texture = new Bitmap(256, 256);
                InitTexture();
            }*/

            frameBuffer = new Bitmap(windowSize.Width, windowSize.Height);
            frameGraphics = Graphics.FromImage(frameBuffer);
            zBuffer = new float[windowSize.Width, windowSize.Height];
            ambientColor = new CustomData.Color(1, 1, 1);

            mesh = Mesh.Cube;

            camera = new Camera(new Vector3(5, 5, -10, 1), new Vector3(0, 0, 0, 1), new Vector3(0, 1, 0, 0)
                             , (float)System.Math.PI / 4, this.windowSize.Width / (float)this.windowSize.Height, 1f, 500f);
        }

        public void InitTexture()
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    texture.SetPixel(i, j, ((i + j) % 32 == 0) ? System.Drawing.Color.White : System.Drawing.Color.Green);
                }
            }
        }

        private void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Matrix model, Matrix view, Matrix projection)
        {
            ModelViewTransform(model, view, ref v1);
            ModelViewTransform(model, view, ref v2);
            ModelViewTransform(model, view, ref v3);

            ProjectionTransform(projection, ref v1);
            ProjectionTransform(projection, ref v2);
            ProjectionTransform(projection, ref v3);

            ScreenTransform(ref v1);
            ScreenTransform(ref v2);
            ScreenTransform(ref v3);

            Console.WriteLine(v1.position.toString());
            Console.WriteLine(v2.position.toString());
            Console.WriteLine(v3.position.toString());

            DrawLineDDA(v1.position, v2.position);
            DrawLineDDA(v2.position, v3.position);
            DrawLineDDA(v3.position, v1.position);
        }

        private void ModelViewTransform(Matrix model, Matrix view, ref Vertex vertex)
        {
            vertex.position = vertex.position * model * view;
        }

        /// <summary>
        /// 投影变换
        /// </summary>
        private void ProjectionTransform(Matrix projection, ref Vertex vertex)
        {
            vertex.position = vertex.position * projection;

            vertex.onePerZ = 1 / vertex.position.w;

            vertex.u *= vertex.onePerZ;
            vertex.v *= vertex.onePerZ;
        }

        private void ScreenTransform(ref Vertex vertex)
        {
            if (vertex.position.w != 0)
            {
                vertex.position.x *= 1 / vertex.position.w;//透视除法
                vertex.position.y *= 1 / vertex.position.w;
                vertex.position.z *= 1 / vertex.position.w;

                vertex.position.w = 1;
                vertex.position.x = (vertex.position.x + 1) * 0.5f * windowSize.Width;
                vertex.position.y = (1 - vertex.position.y) * 0.5f * windowSize.Height;
            }
        }

        private void DrawLineDDA(Vector3 point1, Vector3 point2)
        {
            float x = point1.x;
            float y = point1.y;
            int maxAbs = (int)Math.Max(Math.Abs(point1.x - point2.x), Math.Abs(point1.y - point2.y));
            float xIncre = (point2.x - point1.x) / maxAbs;
            float yIncre = (point2.y - point1.y) / maxAbs;

            for (int i = 0; i < maxAbs; i++)
            {
                if (x >= 0 && y >=0 && x < windowSize.Width && y<windowSize.Height)
                {
                    frameBuffer.SetPixel((int)x, (int)y, System.Drawing.Color.Orange);
                }

                x += xIncre;
                y += yIncre;
            }
        }

        private void Draw()
        {
            Clear();

            frameBuffer.SetPixel(100, 50, System.Drawing.Color.Red);

            var vs = mesh.Vertices;
            DrawLineDDA(new Vector3(0, 0, 0), new Vector3(50, 50, 0));

            Matrix model = Matrix.Translation(Vector3.one);
            Matrix view = Matrix.LookAtLH(camera.pos, camera.lookAt, camera.up);
            Matrix projection = Matrix.PerspectiveFovLH(camera.fov, camera.aspectRatio, camera.zNear, camera.zFar);

            Draw(model, view, projection);

            if (drawGraphic == null)
            {
                return;
            }
            drawGraphic.Clear(System.Drawing.Color.Black);
            drawGraphic.DrawImage(frameBuffer, 0, 0);
        }

        private void Draw(Matrix model, Matrix view, Matrix projection)
        {
            for (int i = 0; i + 2 < mesh.Vertices.Length; i += 3)
            {
                DrawTriangle(mesh.Vertices[i], mesh.Vertices[i + 1], mesh.Vertices[i + 2], model, view, projection);
            }
        }

        private void OnElapsedEvent(object sender, EventArgs e)
        {
            lock (frameBuffer)
            {
                Draw();
            }
        }

        private void Clear()
        {
            frameGraphics.Clear(System.Drawing.Color.Black);
            Array.Clear(zBuffer, 0, zBuffer.Length);
        }
    }
}
