using System;
using System.Collections.Generic;
using System.Text;
using DrawTest1.CustomMath;

namespace DrawTest1.Renderer
{
    public struct Triangle
    {
        public int a;
        public int b;
        public int c;
    }

    public class Mesh
    {
        public string name;

        public Vector3[] vertices;

        public Triangle[] triangles;

        public Vector3 position;

        public Vector3 rotation;

        public Mesh(string name, int verticesCount, int trianglesCount)
        {
            this.name = name;
            vertices = new Vector3[verticesCount];
            triangles = new Triangle[trianglesCount];

            position = new Vector3();
            rotation = new Vector3();
        }
    }
}
