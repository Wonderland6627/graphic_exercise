using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DrawTest1.Renderer
{
    /// <summary>
    /// 限制颜色 0-1
    /// </summary>
    public class LimColor
    {
        public float r;

        public float g;

        public float b;

        public float a;

        public LimColor()
        {

        }

        public LimColor(float r, float g, float b)
        {
            this.r = Mathf.Clamp01(r);
            this.g = Mathf.Clamp01(g);
            this.b = Mathf.Clamp01(b);
        }

        public LimColor(System.Drawing.Color source)
        {
            float r = (float)(source.R / 255f);
            float g = (float)(source.G / 255f);
            float b = (float)(source.B / 255f);

            this.r = Mathf.Clamp01(r);
            this.g = Mathf.Clamp01(g);
            this.b = Mathf.Clamp01(b);
        }

        public System.Drawing.Color ToColor()
        {
            float r = this.r * 255f;
            float g = this.g * 255f;
            float b = this.b * 255f;

            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }

        public static LimColor operator *(LimColor a, LimColor b)
        {
            LimColor color = new LimColor();

            color.r = a.r * b.r;
            color.g = a.g * b.g;
            color.b = a.b * b.b;

            return color;
        }

        public static LimColor operator *(LimColor a, float b)
        {
            LimColor color = new LimColor();

            color.r = a.r * b;
            color.g = a.g * b;
            color.b = a.b * b;

            return color;
        }

        public static LimColor operator +(LimColor a, LimColor b)
        {
            LimColor color = new LimColor();

            color.r = a.r + b.r;
            color.g = a.g + b.g;
            color.b = a.b + b.b;

            return color;
        }
    }
}
