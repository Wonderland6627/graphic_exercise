using System;

namespace DrawTest3.CustomMath
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector3(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        public float Length
        {
            get
            {
                float sq = x * x + y * y + z * z;

                return (float)System.Math.Sqrt(sq);
            }
        }

        public Vector3 Normalize()
        {
            float length = Length;
            if (length != 0)
            {
                float s = 1 / length;
                x *= s;
                y *= s;
                z *= s;
            }

            return this;
        }

        public string toString()
        {
            return string.Format("x:{0} y:{1} z:{2} w:{3}", x, y, z, w);
        }

        public string toDirection()
        {
            return string.Format("x:{0} y:{1} z:{2} w:{3}", Log01(x), Log01(y), Log01(z), Log01(w));
        }

        private ValueType Log01(float value)
        {
            if (value > -0.1f && value < 0.1f)
            {
                return 0;
            }
            else if (value > 0.9f)
            {
                return 1;
            }
            else if (value < -0.9f)
            {
                return -1;
            }

            return 0;
        }

        public static Vector3 zero => new Vector3(0, 0, 0);

        public static Vector3 one => new Vector3(1, 1, 1);

        public static Vector3 right => new Vector3(1, 0, 0);

        public static Vector3 up => new Vector3(0, 1, 0);

        public static Vector3 forward => new Vector3(0, 0, 1);

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            Vector3 v = new Vector3();

            v.x = lhs.x + rhs.x;
            v.y = lhs.y + rhs.y;
            v.z = lhs.z + rhs.z;
            v.w = 0;

            return v;
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            Vector3 v = new Vector3();

            v.x = lhs.x - rhs.x;
            v.y = lhs.y - rhs.y;
            v.z = lhs.z - rhs.z;
            v.w = 0;

            return v;
        }

        public static Vector3 operator *(Vector3 lhs, Vector3 rhs)
        {
            Vector3 v = new Vector3();

            v.x = rhs.z * lhs.y - lhs.z * rhs.y;
            v.y = lhs.z * rhs.x - lhs.x * rhs.y;
            v.z = lhs.x * rhs.y - rhs.x * lhs.y;
            v.w = 1;

            return v;
        }

        public static Vector3 operator *(Vector3 lhs, Matrix rhs)
        {
            Vector3 v = new Vector3();

            v.x = lhs.x * rhs[0, 0] + lhs.y * rhs[1, 0] + lhs.z * rhs[2, 0] + lhs.w * rhs[3, 0];
            v.y = lhs.x * rhs[0, 1] + lhs.y * rhs[1, 1] + lhs.z * rhs[2, 1] + lhs.w * rhs[3, 1];
            v.z = lhs.x * rhs[0, 2] + lhs.y * rhs[1, 2] + lhs.z * rhs[2, 2] + lhs.w * rhs[3, 2];
            v.w = lhs.x * rhs[0, 3] + lhs.y * rhs[1, 3] + lhs.z * rhs[2, 3] + lhs.w * rhs[3, 3];

            return v;
        }

        public static Vector3 operator *(Matrix lhs, Vector3 rhs)
        {
            Vector3 v = new Vector3();

            v.x = rhs.x * lhs[0, 0] + rhs.y * lhs[0, 1] + rhs.z * lhs[0, 2] + rhs.w * lhs[0, 3];
            v.y = rhs.x * lhs[1, 0] + rhs.y * lhs[1, 1] + rhs.z * lhs[1, 2] + rhs.w * lhs[1, 3];
            v.z = rhs.x * lhs[2, 0] + rhs.y * lhs[2, 1] + rhs.z * lhs[2, 2] + rhs.w * lhs[2, 3];
            v.w = rhs.x * lhs[3, 0] + rhs.y * lhs[3, 1] + rhs.z * lhs[3, 2] + rhs.w * lhs[3, 3];

            return v;
        }

        public static Vector3 operator *(Vector3 vector, float multi)
        {
            return new Vector3(vector.x * multi, vector.y * multi, vector.z * multi);
        }

        /// <summary>
        /// 点乘
        /// </summary>
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        /// <summary>
        /// 叉乘
        /// </summary>
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            float x = lhs.y * rhs.z - lhs.z * rhs.y;
            float y = lhs.z * rhs.x - lhs.x * rhs.z;
            float z = lhs.x * rhs.y - lhs.y * rhs.x;

            return new Vector3(x, y, z, 0);
        }

        public static Vector3 CalculateNormal(Vector3 lhs, Vector3 rhs)
        {
            return (lhs * rhs).Normalize();
        }
    }
}