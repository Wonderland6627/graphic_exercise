using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    public struct Camera
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 up;

        public float fov;

        public float aspectRatio;

        public float zNear;

        public float zFar;

        public Camera(Vector3 position, Vector3 direction, Vector3 up, float fov, float aspectRatio, float zNear, float zFar)
        {
            this.position = position;
            this.direction = direction;
            this.up = up;
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.zNear = zNear;
            this.zFar = zFar;
        }
    }
}
