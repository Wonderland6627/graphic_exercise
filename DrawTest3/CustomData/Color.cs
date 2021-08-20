using UnityEngine;

namespace DrawTest3.CustomData
{
    public struct Color
    {
        private float r;
        private float g;
        private float b;

        public float R
        {
            get
            {
                return Mathf.Clamp01(r);
            }
            set
            {
                r = Mathf.Clamp01(value);
            }
        }

        public float G
        {
            get
            {
                return Mathf.Clamp01(g);
            }
            set
            {
                g = Mathf.Clamp01(value);
            }
        }

        public float B
        {
            get
            {
                return Mathf.Clamp01(b);
            }
            set
            {
                b = Mathf.Clamp01(value);
            }
        }

        public Color(float r, float g, float b)
        {
            this.r = Mathf.Clamp01(r);
            this.g = Mathf.Clamp01(g);
            this.b = Mathf.Clamp01(b);
        }

        public Color(System.Drawing.Color c)
        {
            this.r = Mathf.Clamp01((float)c.R / 255);
            this.g = Mathf.Clamp01((float)c.G / 255);
            this.b = Mathf.Clamp01((float)c.B / 255);
        }

        public static Color Red
        {
            get
            {
                return new Color(1, 0, 0);
            }
        }

        public static Color Default
        {
            get
            {
                return new Color(1, 1, 1);
            }
        }

        public System.Drawing.Color ToColor()
        {
            float r = this.R * 255f;
            float g = this.G * 255f;
            float b = this.B * 255f;

            return System.Drawing.Color.FromArgb(Mathf.CeilToInt(r), Mathf.CeilToInt(g), Mathf.CeilToInt(b));
        }

        public string toString()
        {
            return string.Format("r:{0} g:{1} b:{2}", r, g, b);
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            return new Color(Mathf.Lerp(a.r, b.r, t), Mathf.Lerp(a.g, b.g, t), Mathf.Lerp(a.b, b.b, t));
        }

        /// <summary>
        /// 颜色乘法 颜色混合
        /// </summary>
        public static Color operator *(Color a, Color b)
        {
            Color c = new Color();

            c.R = a.R * b.R;
            c.G = a.G * b.G;
            c.B = a.B * b.B;

            return c;
        }

        public static Color operator *(float a, Color b)
        {
            Color c = new Color();

            c.R = a * b.R;
            c.G = a * b.G;
            c.B = a * b.B;

            return c;
        }

        public static Color operator *(Color a, float b)
        {
            Color c = new Color();

            c.R = a.R * b;
            c.G = a.G * b;
            c.B = a.B * b;

            return c;
        }

        public static Color operator +(Color a, Color b)
        {
            Color c = new Color();

            c.R = a.R + b.R;
            c.G = a.G + b.G;
            c.B = a.B + b.B;

            return c;
        }

        public static Color operator -(Color a, Color b)
        {
            Color c = new Color();

            c.R = a.R - b.R;
            c.G = a.G - b.G;
            c.B = a.B - b.B;

            return c;
        }
    }
}