using System;
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

        public void Render(Camera camera, Mesh[] meshes)
        {
            var view = Matrix.LookAtLH(camera.position, camera.target, Vector3.up);
            var projection = 
        }
    }
}
