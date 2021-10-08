using System;
using System.Collections.Generic;
using System.Text;
using DrawTest1.CustomMath;

namespace DrawTest1
{
    public class Camera
    {
        public Vector3 position;

        public Vector3 target;

        public float fov;

        public float aspectRatio;

        public float zNear;

        public float zFar;

        public Camera()
        {

        }

        public Camera(Vector3 position, Vector3 target, float fov, float aspectRatio, float zNear, float zFar)
        {
            this.position = position;
            this.target = target;
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.zNear = zNear;
            this.zFar = zFar;
        }
    }
}
