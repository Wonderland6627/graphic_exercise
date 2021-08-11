using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrawTest3.CustomData;
using DrawTest3.CustomMath;
using UnityEngine;

namespace DrawTest3.CustomTool
{
    public class Tools
    {
        public static void LerpProps(ref Vertex vertex, Vertex v1, Vertex v2, float t)
        {
            vertex.onePerZ = Mathf.Lerp(v1.onePerZ, v2.onePerZ, t);

            vertex.u = Mathf.Lerp(v1.u, v2.u, t);
            vertex.v = Mathf.Lerp(v1.v, v2.v, t);

            vertex.color = CustomData.Color.Lerp(v1.color, v2.color, t);
            vertex.lightingColor = CustomData.Color.Lerp(v1.lightingColor, v2.lightingColor, t);
        }
    }
}
