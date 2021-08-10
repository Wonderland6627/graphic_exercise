using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Timers;
using DrawTest1.CustomMath;
using DrawTest1.Renderer;

namespace DrawTest1
{
    public class Device
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private Camera camera;
        private Mesh[] mesh;

        private System.Timers.Timer timer;

        public Device(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        public void Setup(Graphics graphics, Camera camera, params Mesh[] mesh)
        {
            this.graphics = graphics;
            this.camera = camera;
            this.mesh = mesh;
        }

        public void Start()
        {
            timer = new System.Timers.Timer(1000f / 60f);
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            Clear();
            Render(camera, mesh);
            Present();
        }

        public void Present()
        {
            if (graphics != null)
            {
                graphics.DrawImage(bitmap, 0, 0);
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            bitmap.SetPixel(x, y, color);
        }

        public void Render(Camera camera, params Mesh[] meshes)
        {
            var view = Matrix.LookAtLH(camera.position, camera.target, Vector3.up);
            var projection = Matrix.PerspectiveFovLH(0.78f, (float)bitmap.Width / bitmap.Height, 0.01f, 1.0f);
            foreach (Mesh mesh in meshes)
            {
                var world = Matrix.Translation(mesh.position);
                var transformMatrix = world * view * projection;

                foreach (Triangle triangle in mesh.triangles)
                {
                    var vertexA = mesh.vertices[triangle.a];
                    var vertexB = mesh.vertices[triangle.b];
                    var vertexC = mesh.vertices[triangle.c];

                    var pointA = Project(vertexA.position, transformMatrix);
                    var pointB = Project(vertexB.position, transformMatrix);
                    var pointC = Project(vertexC.position, transformMatrix);

                    DrawLine(pointA, pointB);
                    DrawLine(pointB, pointC);
                    DrawLine(pointC, pointA);

                    RasterizationTriangle(pointA, pointB, pointC);
                }

                /*foreach (var vertex in mesh.vertices)
                {
                    var point = Project(vertex, transformMatrix);
                    DrawPoint(point);
                }*/
            }
        }

        public Vector3 Project(Vector3 coord, Matrix transMatrix)
        {
            var point = Vector3.TransformCoordinate(coord, transMatrix);
            var x = point.x * bitmap.Width + bitmap.Width / 2;
            var y = -point.y * bitmap.Height + bitmap.Height / 2;

            return new Vector3(x, y);
        }

        public void DrawPoint(Vector3 point)
        {
            if (point.x >= 0 && point.y >= 0 && point.x < bitmap.Width && point.y < bitmap.Height)
            {
                SetPixel((int)point.x, (int)point.y, Color.Yellow);
            }
        }

        public void DrawLine(Vector3 point1, Vector3 point2)
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

        public void RasterizationTriangle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            SortVertices(ref point1,ref point2,ref point3);

            if (point1.y == point2.y)//是一个平顶三角形
            {
                FillTopTriangle(point1, point2, point3);
            }
            else if (point2.y == point3.y)//是一个平底三角形
            {
                FillBottomTriangle(point1, point2, point3);
            }
            else//需要分割
            {
                Vector3 midPoint = new Vector3((int)(point1.x + ((point2.y - point1.y) / (point3.y - point1.y)) * (point3.x - point1.x))
                                             , point2.y
                                             , (int)(point1.z + ((point2.y - point1.y) / (point3.y - point1.y)) * (point3.z - point1.z)));

                FillTopTriangle(point2, midPoint, point3);
                FillBottomTriangle(point1, point2, midPoint);
            }
        }

        private void FillTopTriangle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            float slope1 = (point3.x - point1.x) / (point3.y - point1.y);
            float slope2 = (point3.x - point2.x) / (point3.y - point2.y);

            float startX = point3.x;
            float endX = point3.x;

            for (int scanLineY = (int)point3.y; scanLineY <= (int)point1.y; scanLineY++)
            {
                for (int scanLineX = (int)startX; scanLineX <= (int)endX; scanLineX++)
                {
                    SetPixel(scanLineX, scanLineY, Color.Yellow);
                }

                startX += slope1;
                endX += slope2;
            }
        }

        private void FillBottomTriangle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            float slope1 = (point2.x - point1.x) / (point2.y - point1.y);
            float slope2 = (point3.x - point1.x) / (point3.y - point1.y);

            float startX = point1.x;
            float endX = point1.x;

            for (int scanLineY = (int)point1.y; scanLineY >= (int)point2.y; scanLineY--)
            {
                for (int scanLineX = (int)startX; scanLineX <= (int)endX; scanLineX++)
                {
                    SetPixel(scanLineX, scanLineY, Color.Green);
                }

                startX -= slope1;
                endX -= slope2;
            }
        }

        /// <summary>
        /// 根据y给点排序 降序
        /// </summary>
        private void SortVertices( ref Vector3 point1, ref Vector3 point2,ref Vector3 point3)
        {
            Vector3 temp;

            if (point1.y < point2.y)
            {
                temp = point1;
                point1 = point2;
                point2 = temp;
            }

            if (point1.y < point3.y)
            {
                temp = point1;
                point1 = point3;
                point3 = temp;
            }

            if (point2.y < point3.y)
            {
                temp = point2;
                point2 = point3;
                point3 = temp;
            }

            Console.WriteLine("p1y {0} p2y {1} p3y {2}", point1.y, point2.y, point3.y);
        }

        public void Clear()
        {
            //using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);
            }
        }
    }
}
