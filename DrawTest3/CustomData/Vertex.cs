using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    /// <summary>
    /// 顶点信息
    /// </summary>
    public struct Vertex
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// 纹理坐标
        /// </summary>
        public float u;
        public float v;
        /// <summary>
        /// 顶点色
        /// </summary>
        public Color color;
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 normal;

        /// <summary>
        /// 光照颜色
        /// </summary>
        public Color lightingColor;

        /// <summary>
        /// 1/z，用于顶点信息的透视校正
        /// </summary>
        public float onePerZ;

        public Vertex(Vector3 point, Vector3 normal, float u, float v, float r, float g, float b)
        {
            this.position = point;
            this.normal = normal;
            this.position.w = 1;
            color = new Color();
            color.R = r;
            color.G = g;
            color.B = b;
            onePerZ = 1;
            this.u = u;
            this.v = v;
            lightingColor = new Color();
            lightingColor.R = 1;
            lightingColor.G = 1;
            lightingColor.B = 1;
        }

        public Vertex(Vertex vertex)
        {
            position = vertex.position;
            normal = vertex.normal;
            this.color = vertex.color;
            onePerZ = 1;
            this.u = vertex.u;
            this.v = vertex.v;
            this.lightingColor = vertex.lightingColor;
        }
    }
}
