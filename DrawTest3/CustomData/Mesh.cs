using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    /// <summary>
    /// 网格类
    /// </summary>
    public class Mesh
    {
        private Vertex[] vertices;
        /// <summary>
        /// 顶点数组
        /// </summary>
        public Vertex[] Vertices
        {
            get { return vertices; }
        }

        private Material material;
        /// <summary>
        /// 材质
        /// </summary>
        public Material Material
        {
            get { return material; }
        }

        public Mesh(Vector3[] pointList, int[] indexs, Vector2[] uvs, Vector3[] vertColors, Vector3[] normals, Material mat)
        {
            vertices = new Vertex[indexs.Length];

            for (int i = 0; i < indexs.Length; i++)
            {
                int pointIndex = indexs[i];
                Vector3 point = pointList[pointIndex];
                vertices[i] = new Vertex(point, normals[i], uvs[i].x, uvs[i].y, vertColors[i].x, vertColors[i].y, vertColors[i].z);
            }

            material = mat;
        }
    }
}
