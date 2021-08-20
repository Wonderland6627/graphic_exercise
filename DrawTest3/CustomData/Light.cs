using DrawTest3.CustomMath;

namespace DrawTest3.CustomData
{
    public struct Light
    {
        public Vector3 worldPosition;

        public Color lightColor;

        public Light(Vector3 worldPosition, Color lightColor)
        {
            this.worldPosition = worldPosition;
            this.lightColor = lightColor;
        }
    }
}
