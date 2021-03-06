using System;
using System.Collections.Generic;
using System.Text;

namespace DrawTest1.CustomMath
{
    public class Vector3
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector3()
        {

        }

        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 (float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector3 zero => new Vector3(0, 0, 0);

        public static Vector3 one => new Vector3(1, 1, 1);

        public static Vector3 right => new Vector3(1, 0, 0);

        public static Vector3 up => new Vector3(0, 1, 0);

        public static Vector3 forward => new Vector3(0, 0, 1);

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 Normalize()
        {
            return this * (1 / Length());
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            return new Vector3(left.y * right.z - left.z * right.y,
                               left.z * right.x - left.x * right.z,
                               left.x * right.y - left.y * right.x);
        }

        public static Vector3 TransformCoordinate(Vector3 coord, Matrix transMatrix)
        {
            var x = coord.x * transMatrix[0, 0] + coord.y * transMatrix[1, 0] + coord.z * transMatrix[2, 0] + transMatrix[3, 0];
            var y = coord.x * transMatrix[0, 1] + coord.y * transMatrix[1, 1] + coord.z * transMatrix[2, 1] + transMatrix[3, 1];
            var z = coord.x * transMatrix[0, 2] + coord.y * transMatrix[1, 2] + coord.z * transMatrix[2, 2] + transMatrix[3, 2];
            var w = coord.x * transMatrix[0, 3] + coord.y * transMatrix[1, 3] + coord.z * transMatrix[2, 3] + transMatrix[3, 3];

            return new Vector3(x / w, y / w, z / w);
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        public static Vector3 operator *(Vector3 vector, float multi)
        {
            return new Vector3(vector.x * multi, vector.y * multi, vector.z * multi);
        }

        public static Vector3 operator *(Vector3 vector, Matrix matrix)
        {
            Vector3 v = new Vector3();

            v.x = vector.x * matrix[0, 0] + vector.y * matrix[1, 0] + vector.z * matrix[2, 0] + vector.w * matrix[3, 0];
            v.y = vector.x * matrix[0, 1] + vector.y * matrix[1, 1] + vector.z * matrix[2, 1] + vector.w * matrix[3, 1];
            v.z = vector.x * matrix[0, 2] + vector.y * matrix[1, 2] + vector.z * matrix[2, 2] + vector.w * matrix[3, 2];
            v.w = vector.x * matrix[0, 3] + vector.y * matrix[1, 3] + vector.z * matrix[2, 3] + vector.w * matrix[3, 3];

            return v;
        }

        public static Vector3 operator *(Matrix matrix, Vector3 vector)
        {
            Vector3 v = new Vector3();

            v.x = vector.x * matrix[0, 0] + vector.y * matrix[0, 1] + vector.z * matrix[0, 2] + vector.w * matrix[0, 3];
            v.y = vector.x * matrix[1, 0] + vector.y * matrix[1, 1] + vector.z * matrix[1, 2] + vector.w * matrix[1, 3];
            v.z = vector.x * matrix[2, 0] + vector.y * matrix[2, 1] + vector.z * matrix[2, 2] + vector.w * matrix[2, 3];
            v.w = vector.x * matrix[3, 0] + vector.y * matrix[3, 1] + vector.z * matrix[3, 2] + vector.w * matrix[3, 3];

            return v;
        }
    }
}
