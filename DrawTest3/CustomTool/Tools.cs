using DrawTest3.CustomData;
using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using Mathf = UnityEngine.Mathf;

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

    public class MyMathf
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }

            return value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0, 1);
        }

        public static float Lerp(float a, float b, float lerp)
        {
            lerp = Clamp01(lerp);

            return a + (b - a) * lerp;
        }
    }

    public class MeshCreator
    {
        public class MeshInfo
        {
            double[] positions =
            {
                5, -1.110223E-16, 5, 4, -1.110223E-16, 5, 3, -1.110223E-16, 5, 1.99999988, -1.110223E-16, 5, 0.99999994, -1.110223E-16, 5, 0, -1.110223E-16, 5, -1.00000024, -1.110223E-16, 5, -1.99999988, -1.110223E-16, 5, -3, -1.110223E-16, 5, -4.00000048, -1.110223E-16, 5, -5, -1.110223E-16, 5, 5, -8.881784E-17, 4, 4, -8.881784E-17, 4, 3, -8.881784E-17, 4, 1.99999988, -8.881784E-17, 4, 0.99999994, -8.881784E-17, 4, 0, -8.881784E-17, 4, -1.00000024, -8.881784E-17, 4, -1.99999988, -8.881784E-17, 4, -3, -8.881784E-17, 4, -4.00000048, -8.881784E-17, 4, -5, -8.881784E-17, 4, 5, -6.66133841E-17, 3, 4, -6.66133841E-17, 3, 3, -6.66133841E-17, 3, 1.99999988, -6.66133841E-17, 3, 0.99999994, -6.66133841E-17, 3, 0, -6.66133841E-17, 3, -1.00000024, -6.66133841E-17, 3, -1.99999988, -6.66133841E-17, 3, -3, -6.66133841E-17, 3, -4.00000048, -6.66133841E-17, 3, -5, -6.66133841E-17, 3, 5, -4.44089183E-17, 1.99999988, 4, -4.44089183E-17, 1.99999988, 3, -4.44089183E-17, 1.99999988, 1.99999988, -4.44089183E-17, 1.99999988, 0.99999994, -4.44089183E-17, 1.99999988, 0, -4.44089183E-17, 1.99999988, -1.00000024, -4.44089183E-17, 1.99999988, -1.99999988, -4.44089183E-17, 1.99999988, -3, -4.44089183E-17, 1.99999988, -4.00000048, -4.44089183E-17, 1.99999988, -5, -4.44089183E-17, 1.99999988, 5, -2.22044592E-17, 0.99999994, 4, -2.22044592E-17, 0.99999994, 3, -2.22044592E-17, 0.99999994, 1.99999988, -2.22044592E-17, 0.99999994, 0.99999994, -2.22044592E-17, 0.99999994, 0, -2.22044592E-17, 0.99999994, -1.00000024, -2.22044592E-17, 0.99999994, -1.99999988, -2.22044592E-17, 0.99999994, -3, -2.22044592E-17, 0.99999994, -4.00000048, -2.22044592E-17, 0.99999994, -5, -2.22044592E-17, 0.99999994, 5, 0, 0, 4, 0, 0, 3, 0, 0, 1.99999988, 0, 0, 0.99999994, 0, 0, 0, 0, 0, -1.00000024, 0, 0, -1.99999988, 0, 0, -3, 0, 0, -4.00000048, 0, 0, -5, 0, 0, 5, 2.22044658E-17, -1.00000024, 4, 2.22044658E-17, -1.00000024, 3, 2.22044658E-17, -1.00000024, 1.99999988, 2.22044658E-17, -1.00000024, 0.99999994, 2.22044658E-17, -1.00000024, 0, 2.22044658E-17, -1.00000024, -1.00000024, 2.22044658E-17, -1.00000024, -1.99999988, 2.22044658E-17, -1.00000024, -3, 2.22044658E-17, -1.00000024, -4.00000048, 2.22044658E-17, -1.00000024, -5, 2.22044658E-17, -1.00000024, 5, 4.44089183E-17, -1.99999988, 4, 4.44089183E-17, -1.99999988, 3, 4.44089183E-17, -1.99999988, 1.99999988, 4.44089183E-17, -1.99999988, 0.99999994, 4.44089183E-17, -1.99999988, 0, 4.44089183E-17, -1.99999988, -1.00000024, 4.44089183E-17, -1.99999988, -1.99999988, 4.44089183E-17, -1.99999988, -3, 4.44089183E-17, -1.99999988, -4.00000048, 4.44089183E-17, -1.99999988, -5, 4.44089183E-17, -1.99999988, 5, 6.66133841E-17, -3, 4, 6.66133841E-17, -3, 3, 6.66133841E-17, -3, 1.99999988, 6.66133841E-17, -3, 0.99999994, 6.66133841E-17, -3, 0, 6.66133841E-17, -3, -1.00000024, 6.66133841E-17, -3, -1.99999988, 6.66133841E-17, -3, -3, 6.66133841E-17, -3, -4.00000048, 6.66133841E-17, -3, -5, 6.66133841E-17, -3, 5, 8.881785E-17, -4.00000048, 4, 8.881785E-17, -4.00000048, 3, 8.881785E-17, -4.00000048, 1.99999988, 8.881785E-17, -4.00000048, 0.99999994, 8.881785E-17, -4.00000048, 0, 8.881785E-17, -4.00000048, -1.00000024, 8.881785E-17, -4.00000048, -1.99999988, 8.881785E-17, -4.00000048, -3, 8.881785E-17, -4.00000048, -4.00000048, 8.881785E-17, -4.00000048, -5, 8.881785E-17, -4.00000048, 5, 1.110223E-16, -5, 4, 1.110223E-16, -5, 3, 1.110223E-16, -5, 1.99999988, 1.110223E-16, -5, 0.99999994, 1.110223E-16, -5, 0, 1.110223E-16, -5, -1.00000024, 1.110223E-16, -5, -1.99999988, 1.110223E-16, -5, -3, 1.110223E-16, -5, -4.00000048, 1.110223E-16, -5, -5, 1.110223E-16, -5,
            };

            public List<Vector3> GetPositions(double[] pos)
            {
                List<Vector3> newPoss = new List<Vector3>();

                for (int i = 0; i < pos.Length - 3; i += 3)
                {
                    Console.WriteLine(pos[i]);
                    Console.WriteLine(pos[i + 1]);
                    Console.WriteLine(pos[i + 2]);

                    newPoss.Add(new Vector3((float)pos[i], (float)pos[i + 1], (float)pos[i + 2]));
                }

                return newPoss;
            }
        }

        public static Mesh CreateMesh(MeshInfo meshInfo)
        {
            return new Mesh();
        }
    }

    public static class Extensions
    {
        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static float ToRadians(this float angle)
        {
            return angle * (Mathf.PI / 180f);
        }

        public static double ToRadians(this double angle)
        {
            return angle * (Mathf.PI / 180f);
        }

        public static DrawTest3.CustomData.Color ToCustomColor(this System.Drawing.Color color)
        {
            return new DrawTest3.CustomData.Color(color);
        }
    }
}
