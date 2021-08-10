using System;
using System.Collections.Generic;
using System.Text;
using DrawTest1.CustomMath;

namespace DrawTest1.Renderer
{
    public class Vertex
    {
        public Vector3 position;

        public LimColor color;

        public Vertex(Vector3 pos, LimColor col)
        {
            position = pos;
            color = col;
        }

        public Vertex(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }
    }
}
