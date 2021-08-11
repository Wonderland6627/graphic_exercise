using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                r = value;
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
                g = value;
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
                b = value;
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
        /// <summary>
        /// 转换为系统的color
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color TransFormToSystemColor()
        {
            float r = this.R * 255;
            float g = this.G * 255;
            float b = this.B * 255;

            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            else if (t >= 1)
            {
                return b;
            }
            else
            {
                return t * b + (1 - t) * a;
            }
        }

        /// <summary>
        /// 颜色乘法，用于颜色混合，实际叫做Modulate（调制）
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
    }
}