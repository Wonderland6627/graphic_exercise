using System;
using System.Collections.Generic;
using System.Text;
using DrawTest1.CustomMath;

namespace DrawTest1.Renderer
{
    public class Mesh
    {
        public string name;

        public Vector3[] vertices;

        public Vector3 position;

        public Vector3 rotation;

        public Mesh(string name, int verticesCount)
        {
            this.name = name;
            vertices = new Vector3[verticesCount];
        }
    }
}
