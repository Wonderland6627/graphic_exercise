using System;
using System.Collections.Generic;
using System.Text;

namespace DrawTest1.CustomMath
{
    public class Matrix
    {
        private float[,] matrix;

        public int rows;

        public int coloumns;

        public Matrix(int rows, int columns)
        {
            matrix = new float[rows, columns];
            this.rows = rows;
            this.coloumns = columns;
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
            matrix = new float[rows, columns];
            this.rows = rows;
            this.coloumns = columns;
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
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            float[,] multi = new float[a.rows, b.coloumns];

            if (a.coloumns == b.rows)
            {
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < b.coloumns; j++)
                    {
                        for (int k = 0; k < a.coloumns; k++)
                        {
                            multi[i, j] += a[i, k] * b[k, j]; //第i行j列的值为a的第i行上的n个数和b的第j列上的n个数对应相乘之和，其中n为a的列数，也是b的行数，a的列数和b的行数相等
                        }
                    }
                }
            }

            return new Matrix(multi, a.rows, b.coloumns);
        }
    }
}
