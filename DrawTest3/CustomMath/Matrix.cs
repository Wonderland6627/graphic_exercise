using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomMath
{
    public class Matrix
    {
        private float[,] matrix = new float[Size, Size];
        private const int Size = 4;

        public Matrix()
        {

        }

        public Matrix(float[,] ms)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrix[i, j] = ms[i, j];
                }
            }
        }

        public Matrix(float a1, float a2, float a3, float a4,
                      float b1, float b2, float b3, float b4,
                      float c1, float c2, float c3, float c4,
                      float d1, float d2, float d3, float d4)
        {
            matrix[0, 0] = a1; matrix[0, 1] = a2; matrix[0, 2] = a3; matrix[0, 3] = a4;
            matrix[1, 0] = b1; matrix[1, 1] = b2; matrix[1, 2] = b3; matrix[1, 3] = b4;
            matrix[2, 0] = c1; matrix[2, 1] = c2; matrix[2, 2] = c3; matrix[2, 3] = c4;
            matrix[3, 0] = d1; matrix[3, 1] = d2; matrix[3, 2] = d3; matrix[3, 3] = d4;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            Matrix m = new Matrix();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        m[i, j] += lhs[i, k] * rhs[k, j];
                    }
                }
            }

            return m;
        }

        public float this[int row, int coloum]
        {
            get
            {
                return matrix[row, coloum];
            }
            set
            {
                matrix[row, coloum] = value;
            }
        }

        /// <summary>
        /// 单位矩阵
        /// </summary>
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

        public static Matrix Zero
        {
            get
            {
                Matrix matrix = new Matrix();

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        matrix[i, j] = 0;
                    }
                }

                return matrix;
            }
        }

        /// <summary>
        /// 单位化矩阵
        /// </summary>
        public void GetIdentity()
        {
            matrix[0, 0] = 1; matrix[0, 1] = 0; matrix[0, 2] = 0; matrix[0, 3] = 0;

            matrix[1, 0] = 0; matrix[1, 1] = 1; matrix[1, 2] = 0; matrix[1, 3] = 0;

            matrix[2, 0] = 0; matrix[2, 1] = 0; matrix[2, 2] = 1; matrix[2, 3] = 0;

            matrix[3, 0] = 0; matrix[3, 1] = 0; matrix[3, 2] = 0; matrix[3, 3] = 1;
        }

        public void SetZero()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// 求转置
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = i; j < 4; j++)
                {

                    float temp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = temp;
                }
            }

            return this;
        }

        /// <summary>
        /// 求矩阵行列式
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public float Determinate()
        {
            return Determinate(matrix, 4);
        }

        private float Determinate(float[,] m, int n)
        {
            if (n == 1)
            {
                //递归出口
                return m[0, 0];
            }
            else
            {
                float result = 0;
                float[,] tempM = new float[n - 1, n - 1];
                for (int i = 0; i < n; i++)
                {
                    //求代数余子式
                    for (int j = 0; j < n - 1; j++)//行
                    {
                        for (int k = 0; k < n - 1; k++)//列
                        {
                            int x = j + 1;//原矩阵行
                            int y = k >= i ? k + 1 : k;//原矩阵列
                            tempM[j, k] = m[x, y];
                        }
                    }

                    result += (float)System.Math.Pow(-1, 1 + (1 + i)) * m[0, i] * Determinate(tempM, n - 1);
                }

                return result;
            }
        }

        /// <summary>
        /// 获取当前矩阵的伴随矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix GetAdjoint()
        {
            int x, y;
            float[,] tempM = new float[3, 3];
            Matrix result = new Matrix();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int t = 0; t < 3; ++t)
                        {
                            x = k >= i ? k + 1 : k;
                            y = t >= j ? t + 1 : t;

                            tempM[k, t] = matrix[x, y];
                        }
                    }
                    result.matrix[i, j] = (float)System.Math.Pow(-1, (1 + j) + (1 + i)) * Determinate(tempM, 3);
                }
            }

            return result.Transpose();
        }

        /// <summary>
        /// 求当前矩阵的逆矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            float a = Determinate();
            if (a == 0)
            {
                Console.WriteLine("矩阵不可逆");

                return null;
            }

            Matrix adj = GetAdjoint();//伴随矩阵
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    adj.matrix[i, j] = adj.matrix[i, j] / a;
                }
            }

            return adj;
        }

        public static Matrix Translation(Vector3 vector)
        {
            var matrix = Identity;

            matrix[3, 0] = vector.x;
            matrix[3, 1] = vector.y;
            matrix[3, 2] = vector.z;

            return matrix;
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
    }
}
