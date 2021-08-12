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

        public Mesh()
        {

        }

        public Mesh(Vector3[] points, int[] indexs, Vector2[] uvs, Color[] colors, Vector3[] normals)
        {
            vertices = new Vertex[indexs.Length];

            for (int i = 0; i < indexs.Length; i++)
            {
                int pointIndex = indexs[i];
                Vector3 point = points[pointIndex];
                vertices[i] = new Vertex(point, normals[i], uvs[i].x, uvs[i].y, colors[i].R, colors[i].G, colors[i].B);
            }
        }

        public static Mesh Cube
        {
            get
            {
                Vector3[] points =
                {
                    new Vector3(-1,  1, -1),
                    new Vector3(-1, -1, -1),
                    new Vector3(1, -1, -1),
                    new Vector3(1, 1, -1),

                    new Vector3( -1,  1, 1),
                    new Vector3(-1, -1, 1),
                    new Vector3(1, -1, 1),
                    new Vector3(1, 1, 1),
                };

                int[] indexs =
                {
                    0,1,2,
                    0,2,3,

                    7,6,5,
                    7,5,4,

                    0,4,5,
                    0,5,1,

                    1,5,6,
                    1,6,2,

                    2,6,7,
                    2,7,3,

                    3,7,4,
                    3,4,0,
                };

                Vector2[] uvs =
                {
                    new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),

                    new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),

                    new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),

                    new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),

                    new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),

                   new Vector2(0, 0),new Vector2( 0, 1),new Vector2(1, 1),
                    new Vector2(0, 0),new Vector2(1, 1),new Vector2(1, 0),
                };

                Color[] colors =
                {
                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),

                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),

                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),

                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),

                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),

                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),
                };

                Vector3[] normals =
                {
                     new Vector3( 0, 0, -1), new Vector3(0, 0, -1), new Vector3( 0, 0, -1),
                     new Vector3(0, 0, -1), new Vector3( 0, 0, -1), new Vector3( 0, 0, -1),

                     new Vector3( 0, 0, 1), new Vector3( 0, 0, 1), new Vector3( 0, 0, 1),
                     new Vector3( 0, 0, 1), new Vector3( 0, 0, 1), new Vector3( 0, 0, 1),

                     new Vector3( -1, 0, 0), new Vector3( -1, 0, 0), new Vector3( -1, 0, 0),
                     new Vector3( -1, 0, 0), new Vector3(-1, 0, 0), new Vector3( -1, 0, 0),

                     new Vector3( 0, -1, 0), new Vector3(  0, -1, 0), new Vector3(  0, -1, 0),
                     new Vector3(  0, -1, 0), new Vector3( 0, -1, 0), new Vector3( 0, -1, 0),

                     new Vector3( 1, 0, 0), new Vector3( 1, 0, 0), new Vector3( 1, 0, 0),
                     new Vector3( 1, 0, 0), new Vector3( 1, 0, 0), new Vector3( 1, 0, 0),

                     new Vector3( 0, 1, 0), new Vector3( 0, 1, 0), new Vector3( 0, 1, 0),
                     new Vector3( 0, 1, 0 ), new Vector3(0, 1, 0), new Vector3( 0, 1, 0),
                };

                Mesh cube = new Mesh(points, indexs, uvs, colors, normals);

                return cube;
            }
        }

        public static Mesh Plane
        {
            get
            {
                Vector3[] points =
                {
                    new Vector3(-1,  1, 0),
                    new Vector3(-1, -1, 0),
                    new Vector3(1, -1, 0),
                    new Vector3(1, 1, 0),
                };

                int[] indexs =
                {
                    0,1,2,
                    0,2,3,
                };

                Vector2[] uvs =
                {
                    new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1),
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0),
                };

                Color[] colors =
                {
                    new Color( 0, 1, 0), new Color( 0, 0, 1), new Color( 1, 0, 0),
                    new Color( 0, 1, 0), new Color( 1, 0, 0), new Color( 0, 0, 1),
                };

                Vector3[] normals =
                {
                     new Vector3( 0, 0, -1), new Vector3( 0, 0, -1), new Vector3( 0, 0, -1),
                     new Vector3( 0, 0, -1), new Vector3( 0, 0, -1), new Vector3( 0, 0, -1),
                };

                Mesh plane = new Mesh(points, indexs, uvs, colors, normals);

                return plane;
            }
        }
    }
}
