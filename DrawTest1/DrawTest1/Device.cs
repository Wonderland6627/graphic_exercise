﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using DrawTest1.CustomMath;
using DrawTest1.Renderer;

namespace DrawTest1
{
    public class Device
    {
        private Bitmap bitmap;

        public Device(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        public void Present(Graphics graphics)
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

                /*foreach (Triangle triangle in mesh.triangles)
                {
                    var vertexA = mesh.vertices[triangle.a];
                    var vertexB = mesh.vertices[triangle.b];
                    var vertexC = mesh.vertices[triangle.c];
                }*/

                foreach (var vertex in mesh.vertices)
                {
                    var point = Project(vertex, transformMatrix);
                    DrawPoint(point);
                }
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

        public void Clear()
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Black);
            }
        }
    }
}
