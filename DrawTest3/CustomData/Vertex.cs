using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    public struct Vertex
    {
        public Vector3 position;
        
        public float u;
        public float v;
        
        public Vector3 normal;

        public Color color;

        public Color lightingColor;

        public float onePerZ; //1/z

        public Vertex(Vector3 position, Vector3 normal, float u, float v, float r, float g, float b)
        {
            this.position = position;
            this.position.w = 1;

            color = new Color();
            color.R = r;
            color.G = g;
            color.B = b;

            this.normal = normal;

            this.u = u;
            this.v = v;

            lightingColor = new Color();
            lightingColor.R = 1;
            lightingColor.G = 1;
            lightingColor.B = 1;
            
            onePerZ = 1;
        }
    }
}
