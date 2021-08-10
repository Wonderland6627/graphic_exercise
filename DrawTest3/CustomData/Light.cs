using DrawTest3.CustomMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    /// <summary>
    /// 灯光信息
    /// </summary>
    public struct Light
    {
        /// <summary>
        /// 灯光时间坐标
        /// </summary>
        public Vector3 worldPosition;
        /// <summary>
        /// 灯光颜色
        /// </summary>
        public Color lightColor;

        public Light(Vector3 worldPosition, Color lightColor)
        {
            this.worldPosition = worldPosition;
            this.lightColor = lightColor;
        }
    }
}
