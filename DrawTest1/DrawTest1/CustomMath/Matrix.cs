using System;
using System.Collections.Generic;
using System.Text;

namespace DrawTest1.CustomMath
{
    public class Matrix
    {
        private float[,] matrix;

        public int rows;

        public int columns;

        public Matrix()
        {
            this.rows = 4;
            this.columns = 4;
            matrix = new float[rows, columns];
        }

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new float[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = 1;
                }
            }
        }

        public Matrix(float[,] ms, int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new float[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = ms[i, j];
                }
            }
        }

        public float this[int row, int coloumns]
        {
            get
            {
                return matrix[row, coloumns];
            }
            set
            {
                matrix[row, coloumns] = value;
            }
        }

        public static Matrix Identity
        {
            get
            {
                Matrix matrix = new Matrix();

                matrix[0, 0] = 1;
                matrix[1, 1] = 1;
                matrix[2, 2] = 1;
                matrix[3, 3] = 1;

                return matrix;
            }
        }

        /// <summary>
        /// 视锥矩阵
        /// </summary>
        public static Matrix LookAtLH(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            var zaxis = (cameraTarget - cameraPosition).Normalize();
            var xaxis = Vector3.Cross(cameraUpVector, zaxis).Normalize();
            var yaxis = Vector3.Cross(zaxis, xaxis);

            var matrix = Identity;

            matrix[0, 0] = xaxis.x;
            matrix[1, 0] = xaxis.y;
            matrix[2, 0] = xaxis.z;
            matrix[3, 0] = -Vector3.Dot(xaxis, cameraPosition);

            matrix[0, 1] = yaxis.x;
            matrix[1, 1] = yaxis.y;
            matrix[2, 1] = yaxis.z;
            matrix[3, 1] = -Vector3.Dot(yaxis, cameraPosition);

            matrix[0, 2] = zaxis.x;
            matrix[1, 2] = zaxis.y;
            matrix[2, 2] = zaxis.z;
            matrix[3, 2] = -Vector3.Dot(zaxis, cameraPosition);

            return matrix;
        }

        /// <summary>
        /// 投影矩阵
        /// fieldOfViewY 表示视场在 Y 方向的弧度
        /// aspectRatio 表示平面纵横比
        /// </summary>
        public static Matrix PerspectiveFovLH(float fovY, float aspectRatio, float zNear, float zFar)
        {
            float yScale = (float)(1f / (Math.Tan(fovY / 2)));
            float xScale = yScale / aspectRatio;

            var matrix = new Matrix();

            matrix[0, 0] = xScale;
            matrix[1, 1] = yScale;
            matrix[2, 2] = zFar / (zFar - zNear);
            matrix[2, 3] = 1;
            matrix[3, 2] = -zNear * zFar / (zFar - zNear);

            return matrix;
        }

        public static Matrix Translation(Vector3 vector)
        {
            var matrix = Identity;

            matrix[3, 0] = vector.x;
            matrix[3, 1] = vector.y;
            matrix[3, 2] = vector.z;

            return matrix;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            float[,] multi = new float[a.rows, b.columns];

            if (a.columns == b.rows)
            {
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < b.columns; j++)
                    {
                        for (int k = 0; k < a.columns; k++)
                        {
                            multi[i, j] += a[i, k] * b[k, j]; //第i行j列的值为a的第i行上的n个数和b的第j列上的n个数对应相乘之和，其中n为a的列数，也是b的行数，a的列数和b的行数相等
                        }
                    }
                }
            }

            return new Matrix(multi, a.rows, b.columns);
        }
    }
}
