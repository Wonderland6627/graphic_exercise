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

        public static Vertex Lerp(Vertex v1, Vertex v2, float t)
        {
            Vertex v = new Vertex();

            v.color = Color.Lerp(v1.color, v2.color, t);
            v.lightingColor = Color.Lerp(v1.lightingColor, v2.lightingColor, t);

            v.u = UnityEngine.Mathf.Lerp(v1.u, v2.v, t);
            v.v = UnityEngine.Mathf.Lerp(v1.v, v2.v, t);
            v.onePerZ = UnityEngine.Mathf.Lerp(v1.onePerZ, v2.onePerZ, t);

            return v;
        }

        public static Vertex Lerp(ref Vertex v, Vertex v1, Vertex v2, float t)
        {
            v.color = Color.Lerp(v1.color, v2.color, t);
            v.lightingColor = Color.Lerp(v1.lightingColor, v2.lightingColor, t);

            v.u = UnityEngine.Mathf.Lerp(v1.u, v2.v, t);
            v.v = UnityEngine.Mathf.Lerp(v1.v, v2.v, t);
            v.onePerZ = UnityEngine.Mathf.Lerp(v1.onePerZ, v2.onePerZ, t);

            return v;
        }

        public static void Clone(Vertex source, ref Vertex target)
        {
            target.position.x = source.position.x;
            target.position.y = source.position.y;
            target.position.z = source.position.z;
            target.position.w = source.position.w;

            target.u = source.u;
            target.v = source.v;

            target.color.R = source.color.R;
            target.color.G = source.color.G;
            target.color.B = source.color.B;

            target.lightingColor.R = source.lightingColor.R;
            target.lightingColor.G = source.lightingColor.G;
            target.lightingColor.B = source.lightingColor.B;

            target.onePerZ = source.onePerZ;
        }
    }
}
