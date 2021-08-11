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
using System.Threading;

using Mathf = UnityEngine.Mathf;
using DrawTest3.CustomTool;

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

            texture = new Bitmap(256, 256);
            InitTexture();

            frameBuffer = new Bitmap(windowSize.Width, windowSize.Height);
            frameGraphics = Graphics.FromImage(frameBuffer);
            zBuffer = new float[windowSize.Width, windowSize.Height];

            light = new Light(new Vector3(0, 10, 0), new CustomData.Color(1, 1, 1));
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
            GouraudLight(model, camera.position, ref v1);
            GouraudLight(model, camera.position, ref v2);
            GouraudLight(model, camera.position, ref v3);

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

            RasterizationTriangle(v1, v2, v3);
        }

        private void GouraudLight(Matrix model, Vector3 cameraPos, ref Vertex vertex)
        {
            Vector3 worldPoint = vertex.position * model;//世界空间的顶点位置
            Vector3 normal = vertex.normal * model.Inverse().Transpose();
            normal = normal.Normalize();//世界空间法线

            float ambientStrength = 0.1f;
            DrawTest3.CustomData.Color ambientColor = ambientStrength * this.ambientColor;//环境光

            float diffuseStrength = 0.1f;
            Vector3 lightDir = (light.worldPosition - worldPoint).Normalize();
            float diffuse = Math.Max(Vector3.Dot(normal, lightDir), 0);
            DrawTest3.CustomData.Color diffuseColor = diffuseStrength * diffuse * light.lightColor;//漫反射

            float specularStrength = 0.5f;
            Vector3 viewDir = (cameraPos - worldPoint).Normalize();
            Vector3 reflectDir = (viewDir + lightDir).Normalize();
            float specular = UnityEngine.Mathf.Pow(UnityEngine.Mathf.Clamp01(Vector3.Dot(reflectDir, normal)), 32);
            DrawTest3.CustomData.Color specularColor = specularStrength * specular * light.lightColor;//镜面反射

            vertex.lightingColor = ambientColor + diffuseColor + specularColor;
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

            vertex.color *= vertex.onePerZ;
            vertex.lightingColor *= vertex.onePerZ;
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

        private void RasterizationTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v1.position.y == v2.position.y)
            {
                if (v1.position.y < v3.position.y)
                {
                    FillTopTriangle(v1, v2, v3);
                }
                else
                {
                    FillBottomTriangle(v3, v1, v2);
                }
            }
            else if (v1.position.y == v3.position.y)
            {
                if (v1.position.y < v2.position.y)
                {
                    FillTopTriangle(v1, v3, v2);
                }
                else
                {
                    FillBottomTriangle(v2, v1, v3);
                }
            }
            else if (v2.position.y == v3.position.y)
            {
                if (v2.position.y < v1.position.y)
                {
                    FillTopTriangle(v2, v3, v1);
                }
                else
                {
                    FillBottomTriangle(v1, v2, v3);
                }
            }
            else
            {
                Vertex top;
                Vertex middle;
                Vertex bottom;

                if (v1.position.y > v2.position.y && v2.position.y > v3.position.y)
                {
                    top = v3;
                    middle = v2;
                    bottom = v1;
                }
                else if (v3.position.y > v2.position.y && v2.position.y > v1.position.y)
                {
                    top = v1;
                    middle = v2;
                    bottom = v3;
                }
                else if (v2.position.y > v1.position.y && v1.position.y > v3.position.y)
                {
                    top = v3;
                    middle = v1;
                    bottom = v2;
                }
                else if (v3.position.y > v1.position.y && v1.position.y > v2.position.y)
                {
                    top = v2;
                    middle = v1;
                    bottom = v3;
                }
                else if (v1.position.y > v3.position.y && v3.position.y > v2.position.y)
                {
                    top = v2;
                    middle = v3;
                    bottom = v1;
                }
                else if (v2.position.y > v3.position.y && v3.position.y > v1.position.y)
                {
                    top = v1;
                    middle = v3;
                    bottom = v2;
                }
                else
                {
                    return;
                }

                float middleX = (middle.position.y - top.position.y) * (bottom.position.x - top.position.x) / (bottom.position.y - top.position.y) + top.position.x;
                float dy = middle.position.y - top.position.y;
                float t = dy / (bottom.position.y - top.position.y);

                Vertex midVertex = new Vertex();
                midVertex.position = new Vector3(middleX, middle.position.y, 0);
                Tools.LerpProps(ref midVertex, top, bottom, t);

                FillBottomTriangle(top, midVertex, middle);
                FillTopTriangle(midVertex, middle, bottom);
            }
        }

        private void FillTopTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            for (var y = v1.position.y; y <= v3.position.y; y += 0.5f)
            {
                int yIndex = (int)Math.Round(y, MidpointRounding.AwayFromZero);
                if (yIndex >= 0 && yIndex < windowSize.Height)
                {
                    float xl = (y - v1.position.y) * (v3.position.x - v1.position.x) / (v3.position.y - v1.position.y) + v1.position.x;
                    float xr = (y - v2.position.y) * (v3.position.x - v2.position.x) / (v3.position.y - v2.position.y) + v2.position.x;

                    float dy = y - v1.position.y;
                    float lerpT = dy / (v3.position.y - v1.position.y);

                    Vertex point1 = new Vertex();
                    point1.position = new Vector3(xl, y, 0);
                    Tools.LerpProps(ref point1, v1, v3, lerpT);

                    Vertex point2 = new Vertex();
                    point2.position = new Vector3(xr, y, 0);
                    Tools.LerpProps(ref point2, v2, v3, lerpT);

                    if (point1.position.x < point2.position.x)
                    {
                        Scanline(point1, point2, yIndex);
                    }
                    else
                    {
                        Scanline(point2, point1, yIndex);
                    }
                }
            }
        }

        private void FillBottomTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            for (var y = v1.position.y; y <= v2.position.y; y += 0.5f)
            {
                int yIndex = (int)Math.Round(y, MidpointRounding.AwayFromZero);
                if (yIndex >= 0 && yIndex < windowSize.Height)
                {
                    float xl = (y - v1.position.y) * (v2.position.x - v1.position.x) / (v2.position.y - v1.position.y) + v1.position.x;
                    float xr = (y - v1.position.y) * (v3.position.x - v1.position.x) / (v3.position.y - v1.position.y) + v1.position.x;

                    float dy = y - v1.position.y;
                    float lerpT = dy / (v2.position.y - v1.position.y);

                    Vertex point1 = new Vertex();
                    point1.position = new Vector3(xl, y, 0);
                    Tools.LerpProps(ref point1, v1, v2, lerpT);

                    Vertex point2 = new Vertex();
                    point2.position = new Vector3(xr, y, 0);
                    Tools.LerpProps(ref point2, v1, v3, lerpT);

                    if (point1.position.x < point2.position.x)
                    {
                        Scanline(point1, point2, yIndex);
                    }
                    else
                    {
                        Scanline(point2, point1, yIndex);
                    }
                }
            }
        }

        private void Scanline(Vertex v1, Vertex v2, int yIndex)
        {
            float lineX = v2.position.x - v1.position.x;
            float step = 1 / lineX;

            for (var x = v1.position.x; x <= v2.position.x; x += 0.5f)
            {
                int xIndex = (int)(x + 0.5f);
                if (xIndex > 0 && xIndex < windowSize.Width)
                {
                    float lerpT = (x - v1.position.x) / lineX;
                    float onePerZ = UnityEngine.Mathf.Lerp(v1.onePerZ, v2.onePerZ, lerpT);
                    if (onePerZ >= zBuffer[yIndex, xIndex])
                    {
                        float w = 1 / onePerZ;
                        zBuffer[yIndex, xIndex] = onePerZ;

                        float u = Mathf.Lerp(v1.u, v2.u, lerpT) * w * (texture.Width - 1);
                        float v = Mathf.Lerp(v1.v, v2.v, lerpT) * w * (texture.Height - 1);

                        DrawTest3.CustomData.Color texColor = new CustomData.Color(1, 1, 1);
                        if (true)
                        {
                            int uIndex = (int)Math.Round(u, MidpointRounding.AwayFromZero);
                            int vIndex = (int)Math.Round(v, MidpointRounding.AwayFromZero);

                            uIndex = Mathf.Clamp(uIndex, 0, texture.Width - 1);
                            vIndex = Mathf.Clamp(vIndex, 0, texture.Height - 1);

                            texColor = new CustomData.Color(GetTexturePixel(uIndex, vIndex));
                        }
                        else
                        {

                        }

                        DrawTest3.CustomData.Color vertexColor = DrawTest3.CustomData.Color.Lerp(v1.color, v2.color, lerpT) * w;
                        DrawTest3.CustomData.Color lightColor = DrawTest3.CustomData.Color.Lerp(v1.lightingColor, v2.lightingColor, lerpT) * w;

                        DrawTest3.CustomData.Color finalColor = texColor * lightColor;
                        frameBuffer.SetPixel(xIndex, yIndex, System.Drawing.Color.Red /*finalColor.TransFormToSystemColor()*/);
                    }
                }
            }
        }

        private System.Drawing.Color GetTexturePixel(int x, int y)
        {
            int u = Mathf.Clamp(x, 0, texture.Width - 1);
            int v = Mathf.Clamp(y, 0, texture.Height - 1);

            return texture.GetPixel(u, v);
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
                if (x >= 0 && y >= 0 && x < windowSize.Width && y < windowSize.Height)
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
            Matrix view = Matrix.LookAtLH(camera.position, camera.direction, camera.up);
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
