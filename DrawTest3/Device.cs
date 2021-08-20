using DrawTest3.CustomData;
using DrawTest3.CustomMath;
using DrawTest3.CustomTool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Mathf = DrawTest3.CustomTool.MyMathf;

namespace DrawTest3
{
    public enum DisplayMode
    {
        Point,
        Line,
        Surface,
        Texture,
    }

    public class Device
    {
        private Bitmap texture;

        private Bitmap frameBuffer;
        private Graphics frameGraphics;

        private float[,] zBuffer;

        private Mesh cubeMesh;
        private Mesh planeMesh;

        private Camera camera;

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        private Light light;
        float ambientStrength = 0.1f;
        float diffuseStrength = 0.1f;
        float specularStrength = 0.5f;

        private Size windowSize;
        private Graphics drawGraphic;

        private Matrix model;
        private Matrix view;
        private Matrix projection;

        private DisplayMode displayMode;
        private bool lightingOn = false;

        private bool cutting = false;

        private Thread rendererThread;
        private System.Object lockObj;

        public delegate void OnUpdateHandler(float fps);
        public event OnUpdateHandler OnUpdate;

        public void Init(Size size, Graphics board)
        {
            //Test();

            lockObj = new object();

            windowSize = size;
            drawGraphic = board;

            displayMode = DisplayMode.Surface;

            InitSystem();

            /*System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 6f);
            mainTimer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();*/

            rendererThread = new Thread(new ThreadStart(Renderer));
            rendererThread.Start();
        }

        public void Test()
        {
            DateTime.Now.ToString();
            string str = DateTime.Now.ToString("ddd, dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));
            string str1 = DateTime.Now.ToString("R");

            Console.WriteLine(str);
            Console.WriteLine(str1);
        }

        private void InitSystem()
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile("../../Textures/texture.jpg");
            texture = new Bitmap(image, 256, 256);

            frameBuffer = new Bitmap(windowSize.Width, windowSize.Height);
            frameGraphics = Graphics.FromImage(frameBuffer);
            zBuffer = new float[windowSize.Width, windowSize.Height];

            light = new Light(new Vector3(2, 2, -2), new CustomData.Color(1, 1, 1));

            cubeMesh = Mesh.Cube;
            planeMesh = Mesh.Plane;

            camera = new Camera(new Vector3(0, 0, -8, 1), new Vector3(0, 0, 1, 1), new Vector3(0, 1, 0, 0)
                             , (float)System.Math.PI / 4f, this.windowSize.Width / (float)this.windowSize.Height, 6f, 30f);

            surfacesList = new List<Surface>();
            surfacesQueue = new Queue<Surface>();
        }

        private bool ClipInScreen(Vertex v)
        {
            if (v.position.x >= -v.position.w && v.position.x <= v.position.w &&
                v.position.y >= -v.position.w && v.position.y <= v.position.w &&
                v.position.z >= -v.position.w && v.position.z <= v.position.w)
            {
                return true;
            }

            return false;
        }

        private void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Matrix model, Matrix view, Matrix projection, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            ModelViewProjectionTransform(model, ref v1);
            ModelViewProjectionTransform(model, ref v2);
            ModelViewProjectionTransform(model, ref v3);

            if (lightingOn)
            {
                GouraudLight(model, camera.position, ref v1);
                GouraudLight(model, camera.position, ref v2);
                GouraudLight(model, camera.position, ref v3);
            }

            ModelViewProjectionTransform(model, view, ref v1);
            ModelViewProjectionTransform(model, view, ref v2);
            ModelViewProjectionTransform(model, view, ref v3);

            if (!BackCulling(v1, v2, v3)) return;

            ModelViewProjectionTransform(model, view, projection, ref v1);
            ModelViewProjectionTransform(model, view, projection, ref v2);
            ModelViewProjectionTransform(model, view, projection, ref v3);

            if (!Exclude(v1) && !Exclude(v2) && !Exclude(v3))
            {
                return;
            }

            ScreenTransform(ref v1);
            ScreenTransform(ref v2);
            ScreenTransform(ref v3);

            CVVScreen(ref v1);
            CVVScreen(ref v2);
            CVVScreen(ref v3);

            if (Clip(v1) && Clip(v2) && Clip(v3))
            {
                return;
            }

            /*if (cutting)
            {
                MultiSurfaceCutting(new Surface(v1, v2, v3));

                if (surfacesList.Count <= 1)
                {
                    Draw(v1, v2, v3, lightingOn, cuttingOn, displayMode);
                }
                else
                {
                    for (int i = 0; i < surfacesList.Count; i++)
                    {
                        Draw(surfacesList[i], lightingOn, cuttingOn, displayMode);
                    }
                }

                return;
            }

            Draw(v1, v2, v3, lightingOn, cuttingOn, displayMode);*/

            if (cutting)
            {
                MultiSurfaceCutting(new Surface(v1, v2, v3));
            }
            else
            {
                surfacesList.Add(new Surface(v1, v2, v3));
            }

            for (int i = 0; i < surfacesList.Count; i++)
            {
                Draw(surfacesList[i], lightingOn, cuttingOn, displayMode);
            }
        }

        private bool Exclude(Vertex v)
        {
            if (v.position.x >= -v.position.w && v.position.x <= v.position.w &&
                v.position.y >= -v.position.w && v.position.y <= v.position.w)
            {
                return true;
            }

            return false;
        }

        private bool Clip(Vertex v)
        {
            if (v.position.z < -1 || v.position.z > 1)
            {
                return true;
            }

            return false;
        }

        private void Draw(Surface surface, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            Draw(surface.Vertices[0], surface.Vertices[1], surface.Vertices[2], lightingOn, cuttingOn, displayMode);
        }

        private void Draw(Vertex v1, Vertex v2, Vertex v3, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            switch (displayMode)
            {
                case DisplayMode.Point:
                    DrawPoint(v1);
                    DrawPoint(v2);
                    DrawPoint(v3);
                    break;
                case DisplayMode.Line:
                    DrawLineDDA(v1, v2);
                    DrawLineDDA(v2, v3);
                    DrawLineDDA(v3, v1);
                    break;
                case DisplayMode.Surface:
                case DisplayMode.Texture:
                    RasterizationTriangle(v1, v2, v3, lightingOn, cuttingOn, displayMode);
                    break;
            }
        }

        private void RasterizationTriangle(Surface surface, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            RasterizationTriangle(surface.Vertices[0], surface.Vertices[1], surface.Vertices[2], lightingOn, cuttingOn, displayMode);
        }

        private void GouraudLight(Matrix model, Vector3 cameraPos, ref Vertex vertex)
        {
            Vector3 worldPoint = vertex.position * model;//世界空间的顶点位置
            Vector3 normal = vertex.normal * model.Inverse().Transpose();
            normal = normal.Normalize();//世界空间法线

            DrawTest3.CustomData.Color ambientColor = ambientStrength * new CustomData.Color(1, 1, 1);//环境光

            Vector3 lightDir = (light.worldPosition - worldPoint).Normalize();
            float diffuse = Math.Max(Vector3.Dot(normal, lightDir), 0);
            DrawTest3.CustomData.Color diffuseColor = diffuseStrength * diffuse * light.lightColor;//漫反射

            Vector3 viewDir = (cameraPos - worldPoint).Normalize();
            Vector3 reflectDir = (viewDir + lightDir).Normalize();
            float specular = UnityEngine.Mathf.Pow(UnityEngine.Mathf.Clamp01(Vector3.Dot(reflectDir, normal)), 16);
            DrawTest3.CustomData.Color specularColor = specularStrength * specular * light.lightColor;//镜面反射

            vertex.lightingColor = ambientColor + diffuseColor + specularColor;
        }

        private void ModelViewProjectionTransform(Matrix model, ref Vertex vertex)
        {
            vertex.position = vertex.position * model;
        }

        private void ModelViewProjectionTransform(Matrix model, Matrix view, ref Vertex vertex)
        {
            vertex.position = vertex.position * view;
        }

        private void ModelViewProjectionTransform(Matrix model, Matrix view, Matrix projection, ref Vertex vertex)
        {
            vertex.position = vertex.position * projection;
        }

        /// <summary>
        /// 投影变换
        /// </summary>
        private void ProjectionTransform(Matrix projection, ref Vertex vertex)
        {
            vertex.position = vertex.position * projection;
        }

        private void ScreenTransform(ref Vertex vertex)
        {
            if (vertex.position.w != 0)
            {
                vertex.onePerZ = 1 / vertex.position.w;

                vertex.position.x *= 1 / vertex.position.w;//透视除法
                vertex.position.y *= 1 / vertex.position.w;
                vertex.position.z *= 1 / vertex.position.w;

                vertex.position.w = 1;

                vertex.u *= vertex.onePerZ;
                vertex.v *= vertex.onePerZ;

                vertex.color *= vertex.onePerZ;
                vertex.lightingColor *= vertex.onePerZ;
            }
        }

        private void CVVScreen(ref Vertex vertex)
        {
            vertex.position.x = (vertex.position.x + 1) * 0.5f * windowSize.Width;
            vertex.position.y = (1 - vertex.position.y) * 0.5f * windowSize.Height;
        }

        private bool BackCulling(Vertex v1, Vertex v2, Vertex v3)
        {
            Vector3 vec1 = v2.position - v1.position;
            Vector3 vec2 = v3.position - v1.position;

            Vector3 normal = Vector3.Cross(vec1, vec2);
            Vector3 viewDir = v2.position - Vector3.zero;

            if (Vector3.Dot(normal, viewDir) > 0)
            {
                return true;
            }

            return false;
        }

        private void RasterizationTriangle(Vertex v1, Vertex v2, Vertex v3, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            if (v1.position.y == v2.position.y)
            {
                if (v1.position.y < v3.position.y)
                {
                    FillTopTriangle(v1, v2, v3, lightingOn, cuttingOn, displayMode);
                }
                else
                {
                    FillBottomTriangle(v3, v1, v2, lightingOn, cuttingOn, displayMode);
                }
            }
            else if (v1.position.y == v3.position.y)
            {
                if (v1.position.y < v2.position.y)
                {
                    FillTopTriangle(v1, v3, v2, lightingOn, cuttingOn, displayMode);
                }
                else
                {
                    FillBottomTriangle(v2, v1, v3, lightingOn, cuttingOn, displayMode);
                }
            }
            else if (v2.position.y == v3.position.y)
            {
                if (v2.position.y < v1.position.y)
                {
                    FillTopTriangle(v2, v3, v1, lightingOn, cuttingOn, displayMode);
                }
                else
                {
                    FillBottomTriangle(v1, v2, v3, lightingOn, cuttingOn, displayMode);
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

                FillBottomTriangle(top, midVertex, middle, lightingOn, cuttingOn, displayMode);
                FillTopTriangle(midVertex, middle, bottom, lightingOn, cuttingOn, displayMode);
            }
        }

        private void FillTopTriangle(Vertex v1, Vertex v2, Vertex v3, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            for (var y = v1.position.y; y < v3.position.y; y++)
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
                        Scanline(point1, point2, yIndex, lightingOn, cuttingOn, displayMode);
                    }
                    else
                    {
                        Scanline(point2, point1, yIndex, lightingOn, cuttingOn, displayMode);
                    }
                }
            }
        }

        private void FillBottomTriangle(Vertex v1, Vertex v2, Vertex v3, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            for (var y = v1.position.y; y < v2.position.y; y++)
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
                        Scanline(point1, point2, yIndex, lightingOn, cuttingOn, displayMode);
                    }
                    else
                    {
                        Scanline(point2, point1, yIndex, lightingOn, cuttingOn, displayMode);
                    }
                }
            }
        }

        private void Scanline(Vertex v1, Vertex v2, int yIndex, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            float lineLength = v2.position.x - v1.position.x;

            for (var x = v1.position.x; (int)(x + 0.5f) <= v2.position.x; x++)
            {
                int xIndex = (int)(x + 0.5f);
                if (xIndex >= 0 && xIndex < windowSize.Width)
                {
                    float lerpT = (x - v1.position.x) / lineLength;
                    float onePerZ = UnityEngine.Mathf.Lerp(v1.onePerZ, v2.onePerZ, lerpT);
                    if (yIndex < zBuffer.GetLength(0) && xIndex < zBuffer.GetLength(1))
                    {
                        if (onePerZ >= zBuffer[yIndex, xIndex])
                        {
                            float w = 1 / onePerZ;
                            zBuffer[yIndex, xIndex] = onePerZ;

                            float u = Mathf.Lerp(v1.u, v2.u, lerpT) * w;
                            float v = Mathf.Lerp(v1.v, v2.v, lerpT) * w;

                            DrawTest3.CustomData.Color texColor = new CustomData.Color(GetTexturePixel(u, v));//纹理点采样
                            DrawTest3.CustomData.Color vertexColor = DrawTest3.CustomData.Color.Lerp(v1.color, v2.color, lerpT) * w;
                            DrawTest3.CustomData.Color lightColor = DrawTest3.CustomData.Color.Lerp(v1.lightingColor, v2.lightingColor, lerpT) * w;
                            DrawTest3.CustomData.Color finalColor = new CustomData.Color(1, 1, 1);

                            if (lightingOn)
                            {
                                if (displayMode == DisplayMode.Surface)
                                {
                                    finalColor = vertexColor * lightColor;
                                }
                                else if (displayMode == DisplayMode.Texture)
                                {
                                    finalColor = texColor * lightColor;
                                }
                            }
                            else
                            {
                                if (displayMode == DisplayMode.Surface)
                                {
                                    finalColor = vertexColor * lightColor;
                                }
                                else if (displayMode == DisplayMode.Texture)
                                {
                                    finalColor = texColor * lightColor;
                                }
                            }

                            frameBuffer.SetPixel(xIndex, yIndex, finalColor.ToColor());
                        }
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

        private System.Drawing.Color GetTexturePixel(float x, float y)
        {
            float u = x * (texture.Width - 1);
            float v = y * (texture.Height - 1);

            return texture.GetPixel((int)u, (int)v);
        }

        private void DrawLineDDA(Vertex vertex1, Vertex vertex2)
        {
            float x = vertex1.position.x;
            float y = vertex1.position.y;

            float lengthX = Math.Abs(vertex1.position.x - vertex2.position.x);
            float lengthY = Math.Abs(vertex1.position.y - vertex2.position.y);
            int maxAbs = (int)Math.Max(lengthX, lengthY);

            float xIncre = (vertex2.position.x - vertex1.position.x) / maxAbs;
            float yIncre = (vertex2.position.y - vertex1.position.y) / maxAbs;

            for (int i = 0; i < maxAbs; i++)
            {
                float lerpT = 0.1f;//(maxAbs - vertex1.position.y) / lengthY;
                float onPreZ = Mathf.Lerp(vertex1.onePerZ, vertex2.onePerZ, lerpT);
                float w = 1 / onPreZ; // 9 11

                if (x >= 0 && y >= 0 && x <= windowSize.Width && y <= windowSize.Height)
                {
                    //DrawTest3.CustomData.Color vertexColor = DrawTest3.CustomData.Color.Lerp(vertex1.color, vertex2.color, lerpT.Abs()) * w;
                    DrawTest3.CustomData.Color vertexColor = DrawTest3.CustomData.Color.Red;
                    frameBuffer.SetPixel((int)x, (int)y, vertexColor.ToColor());
                }

                x += xIncre;
                y += yIncre;
            }
        }

        private void DrawPoint(Vertex vertex)
        {
            float x = vertex.position.x;
            float y = vertex.position.y;

            if (x >= 0 && y >= 0 && x < windowSize.Width && y < windowSize.Height)
            {
                frameBuffer.SetPixel((int)x, (int)y, vertex.color.ToColor());
            }
        }

        private void Draw(bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            model = Matrix.Translation(new Vector3(0, 0, 2));
            view = camera.GetViewMatrix();//Matrix.LookAtLH(camera.position, camera.position + camera.forward, camera.up);
            projection = Matrix.PerspectiveFovLH(camera.fov, camera.aspectRatio, camera.zNear, camera.zFar);

            Draw(model, view, projection, cubeMesh, lightingOn, cuttingOn, displayMode);
            //Draw(model, view, projection, planeMesh);

            if (drawGraphic == null)
            {
                return;
            }
            drawGraphic.DrawImage(frameBuffer, 0, 0);
            //drawGraphic.Clear(System.Drawing.Color.Gray);
        }

        private void Draw(Matrix model, Matrix view, Matrix projection, Mesh mesh, bool lightingOn = false, bool cuttingOn = false, DisplayMode displayMode = DisplayMode.Surface)
        {
            for (int i = 0; i + 2 < mesh.Vertices.Length; i += 3)
            {
                DrawTriangle(mesh.Vertices[i], mesh.Vertices[i + 1], mesh.Vertices[i + 2], model, view, projection, lightingOn, cuttingOn, displayMode);
            }
        }

        private Queue<Surface> surfacesQueue;
        private List<Surface> surfacesList;

        private Vector3[] dotVectors =//顶点和该向量插值，判断顶点到平面的直线距离
        {
                new Vector3(0,0,1),//前
                new Vector3(0,0,-1),//后
                new Vector3(1,0,0),//左
                new Vector3(-1,0,0),//右
                new Vector3(0,1,0),//上
                new Vector3(0,-1,0)//下
        };

        float[] distance = new float[] { -1, -1, 0f, -783, 0f, -560 };//各个平面到原点“距离”

        private void MultiSurfaceCutting(Surface targetSurface)
        {
            bool isClip = false;

            surfacesQueue.Enqueue(targetSurface);

            while (surfacesQueue.Count > 0)
            {
                Surface surface = surfacesQueue.Dequeue();

                for (int i = surface.startIndex; i < 6; i++)
                {
                    if (!isClip)
                    {
                        isClip = MultiSurfaceCutting(surface.Vertices[0], surface.Vertices[1], surface.Vertices[2],
                                                     dotVectors[i], distance[i], i);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!isClip)
                {
                    surfacesList.Add(new Surface(surface.Vertices[0], surface.Vertices[1], surface.Vertices[2]));
                }

                isClip = false;
            }
        }

        private bool MultiSurfaceCutting(Vertex v1, Vertex v2, Vertex v3, Vector3 dotVector, float distance, int startIndex)
        {
            //插值因子
            float lerpT = 0;
            //点在法线上的投影
            float projectV1 = Vector3.Dot(dotVector, v1.position);
            float projectV2 = Vector3.Dot(dotVector, v2.position);
            float projectV3 = Vector3.Dot(dotVector, v3.position);

            // v1,v2,v3都在立方体内
            if (projectV1 > distance && projectV2 > distance && projectV3 > distance)
            {
                //不做任何处理
                return false;
            }

            //点与点之间的距离
            float dv1v2 = Math.Abs(projectV1 - projectV2);
            float dv1v3 = Math.Abs(projectV1 - projectV3);
            float dv2v3 = Math.Abs(projectV2 - projectV3);
            //点倒平面的距离
            float pv1 = Math.Abs(projectV1 - distance);
            float pv2 = Math.Abs(projectV2 - distance);
            float pv3 = Math.Abs(projectV3 - distance);

            if (projectV1 < distance && projectV2 > distance && projectV3 > distance)//只有v1在外
            {
                Vertex temp2 = new Vertex();
                lerpT = pv2 / dv1v2;
                temp2.position.x = Mathf.Lerp(v2.position.x, v1.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v2.position.y, v1.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v2.position.z, v1.position.z, lerpT);
                Vertex.Lerp(ref temp2, v2, v1, lerpT);

                Vertex temp1 = new Vertex();
                lerpT = pv3 / dv1v3;
                temp1.position.x = Mathf.Lerp(v3.position.x, v1.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v3.position.y, v1.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v3.position.z, v1.position.z, lerpT);
                Vertex.Lerp(ref temp1, v3, v1, lerpT);

                //画线或光栅化
                Vertex temp3 = new Vertex();
                Vertex temp4 = new Vertex();
                Vertex.Clone(v2, ref temp3);
                Vertex.Clone(temp1, ref temp4);
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v2, startIndex + 1));
                surfacesQueue.Enqueue(new Surface(temp4, temp3, v3, startIndex + 1));

                return true;
            }
            else if (projectV1 > distance && projectV2 < distance && projectV3 > distance)//只有v2在外
            {
                Vertex temp1 = new Vertex();
                lerpT = pv1 / dv1v2;
                temp1.position.x = Mathf.Lerp(v1.position.x, v2.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v1.position.y, v2.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v1.position.z, v2.position.z, lerpT);
                Vertex.Lerp(ref temp1, v1, v2, lerpT);

                Vertex temp2 = new Vertex();
                lerpT = pv3 / dv2v3;
                temp2.position.x = Mathf.Lerp(v3.position.x, v2.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v3.position.y, v2.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v3.position.z, v2.position.z, lerpT);
                Vertex.Lerp(ref temp2, v3, v2, lerpT);

                //画线或光栅化
                Vertex temp3 = new Vertex();
                Vertex temp4 = new Vertex();
                Vertex.Clone(v3, ref temp3);
                Vertex.Clone(temp1, ref temp4);
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v3, startIndex + 1));
                surfacesQueue.Enqueue(new Surface(temp4, temp3, v1, startIndex + 1));

                return true;
            }
            else if (projectV1 > distance && projectV2 > distance && projectV3 < distance)//只有v3在外
            {
                Vertex temp1 = new Vertex();
                lerpT = pv2 / dv2v3;
                temp1.position.x = Mathf.Lerp(v2.position.x, v3.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v2.position.y, v3.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v2.position.z, v3.position.z, lerpT);
                Vertex.Lerp(ref temp1, v2, v3, lerpT);

                Vertex temp2 = new Vertex();
                lerpT = pv1 / dv1v3;
                temp2.position.x = Mathf.Lerp(v1.position.x, v3.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v1.position.y, v3.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v1.position.z, v3.position.z, lerpT);
                Vertex.Lerp(ref temp2, v1, v3, lerpT);

                //画线或光栅化
                Vertex temp3 = new Vertex();
                Vertex temp4 = new Vertex();
                Vertex.Clone(v1, ref temp3);
                Vertex.Clone(temp1, ref temp4);
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v1, startIndex + 1));
                surfacesQueue.Enqueue(new Surface(temp4, temp3, v2, startIndex + 1));

                return true;
            }
            else if (projectV1 > distance && projectV2 < distance && projectV3 < distance)//只有v1在内
            {
                Vertex temp1 = new Vertex();
                lerpT = pv1 / dv1v2;
                temp1.position.x = Mathf.Lerp(v1.position.x, v2.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v1.position.y, v2.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v1.position.z, v2.position.z, lerpT);
                Vertex.Lerp(ref temp1, v1, v2, lerpT);

                Vertex temp2 = new Vertex();
                lerpT = pv1 / dv1v3;
                temp2.position.x = Mathf.Lerp(v1.position.x, v3.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v1.position.y, v3.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v1.position.z, v3.position.z, lerpT);
                Vertex.Lerp(ref temp2, v1, v3, lerpT);

                //画线或光栅化
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v1, startIndex + 1));

                return true;
            }
            else if (projectV1 < distance && projectV2 > distance && projectV3 < distance)//只有v2在内
            {
                Vertex temp1 = new Vertex();
                lerpT = pv2 / dv2v3;
                temp1.position.x = Mathf.Lerp(v2.position.x, v3.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v2.position.y, v3.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v2.position.z, v3.position.z, lerpT);
                Vertex.Lerp(ref temp1, v2, v3, lerpT);

                Vertex temp2 = new Vertex();
                lerpT = pv2 / dv1v2;
                temp2.position.x = Mathf.Lerp(v2.position.x, v1.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v2.position.y, v1.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v2.position.z, v1.position.z, lerpT);
                Vertex.Lerp(ref temp2, v2, v1, lerpT);

                //画线或光栅化
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v2, startIndex + 1));

                return true;
            }
            else if (projectV1 < distance && projectV2 < distance && projectV3 > distance)//只有v3在内
            {
                Vertex temp1 = new Vertex();
                lerpT = pv3 / dv1v3;
                temp1.position.x = Mathf.Lerp(v3.position.x, v1.position.x, lerpT);
                temp1.position.y = Mathf.Lerp(v3.position.y, v1.position.y, lerpT);
                temp1.position.z = Mathf.Lerp(v3.position.z, v1.position.z, lerpT);
                Vertex.Lerp(ref temp1, v3, v1, lerpT);

                Vertex temp2 = new Vertex();
                lerpT = pv3 / dv2v3;
                temp2.position.x = Mathf.Lerp(v3.position.x, v2.position.x, lerpT);
                temp2.position.y = Mathf.Lerp(v3.position.y, v2.position.y, lerpT);
                temp2.position.z = Mathf.Lerp(v3.position.z, v2.position.z, lerpT);
                Vertex.Lerp(ref temp2, v3, v2, lerpT);

                //画线或光栅化
                surfacesQueue.Enqueue(new Surface(temp1, temp2, v3, startIndex + 1));

                return true;
            }

            return false;
        }

        private void OnElapsedEvent(object sender, EventArgs e)
        {
            lock (frameBuffer)
            {
                Clear();

                Draw(lightingOn, cutting, displayMode);
            }
        }

        private DateTime lastTime;

        private void Renderer()
        {
            while (true)
            {
                lock (frameBuffer)
                {
                    lastTime = DateTime.Now;

                    Clear();

                    UpdateSettings();

                    Draw(lightingOn, cutting, displayMode);

                    float fps = (int)Math.Ceiling(1000 / (DateTime.Now - lastTime).TotalMilliseconds);
                    if (OnUpdate != null)
                    {
                        OnUpdate.Invoke(fps);
                    }
                }
            }
        }

        private void UpdateSettings()
        {

        }

        private void Clear()
        {
            frameGraphics.Clear(System.Drawing.Color.Gray);
            Array.Clear(zBuffer, 0, zBuffer.Length);
            //surfacesQueue.Clear();
            surfacesList.Clear();
        }

        public void SwitchDisplayMode(DisplayMode mode)
        {
            displayMode = mode;
        }

        public void TurnLighting(out bool value)
        {
            lightingOn = !lightingOn;
            value = lightingOn;
        }

        public void TurnCutting(out bool value)
        {
            cutting = !cutting;
            value = cutting;
        }

        /// <summary>
        /// 设置环境光
        /// </summary>
        public void SetAmbientStrength(float value)
        {
            ambientStrength = value;
        }

        public void MoveCamera(Vector3 dir)
        {
            camera.Move(dir);
        }

        public void MoveCamera(Camera_Movement_Type direction)
        {
            camera.Move(direction);
        }

        public void ResetCamera()
        {
            camera.position = new Vector3(0, 0, -8);
            camera.Rotate(Matrix.RotateX(0) * Matrix.RotateY(0));
            camera.fov = (float)System.Math.PI / 4f;
            camera.yaw = 0;
            camera.pitch = 0;
        }

        public void UpdateCameraFOV(float offset)
        {
            camera.UpdateCameraFOV(offset);
        }

        public void RotateCamera(float yaw, float pitch)
        {
            camera.UpdateCameraVectors(yaw, pitch);
        }
    }
}
