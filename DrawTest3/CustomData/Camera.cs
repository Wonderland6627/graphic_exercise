using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    public struct Camera
    {
        public Vector3 pos;
        public Vector3 lookAt;
        public Vector3 up;

        /// <summary>
        /// 观察角，弧度
        /// </summary>
        public float fov;
        /// <summary>
        /// 长宽比
        /// </summary>
        public float aspectRatio;
        /// <summary>
        /// 近裁平面
        /// </summary>
        public float zNear;
        /// <summary>
        /// 远裁平面
        /// </summary>
        public float zFar;

        public Camera(Vector3 pos, Vector3 lookAt, Vector3 up, float fov, float aspectRatio, float zn, float zf)
        {
            this.pos = pos;
            this.lookAt = lookAt;
            this.up = up;
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.zNear = zn;
            this.zFar = zf;
        }
    }
}
