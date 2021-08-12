using DrawTest3.CustomMath;
using DrawTest3.CustomTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTest3.CustomData
{
    public enum Camera_Movement_Type
    {
        Forward,
        Backward,
        Left,
        Right,
    }

    public struct Camera
    {
        public Vector3 position;

        public Vector3 forward;
        public Vector3 right;
        public Vector3 up;

        public float fov;

        public float aspectRatio;

        public float zNear;

        public float zFar;

        public Camera(Vector3 position, Vector3 forward, Vector3 up, float fov, float aspectRatio, float zNear, float zFar)
        {
            this.position = position;
            this.forward = forward;
            this.up = up;
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.zNear = zNear;
            this.zFar = zFar;

            yaw = 0;
            pitch = 0;
            this.right = Vector3.right;
        }

        public void Move(Vector3 dir)
        {
            position = position + dir;
            Console.WriteLine(position.toString());
        }

        public void Move(Camera_Movement_Type direction)
        {
            if (direction == Camera_Movement_Type.Forward)
            {
                position = position + forward;
            }
            if (direction == Camera_Movement_Type.Backward)
            {
                position = position - forward;
            }
            if (direction == Camera_Movement_Type.Left)
            {
                position = position - right;
            }
            if (direction == Camera_Movement_Type.Right)
            {
                position = position + right;
            }
        }

        float yaw;
        float pitch;

        public void UpdateCameraVectors()
        {
            Vector3 front = new Vector3();

            front.x = (float)(Math.Cos(yaw).ToRadians() * Math.Cos(pitch).ToRadians());
            front.y = (float)(Math.Sin(pitch).ToRadians());
            front.z = (float)(Math.Sin(yaw).ToRadians() * Math.Cos(pitch).ToRadians());

            forward = front.Normalize();

            right = Vector3.Cross(forward, Vector3.up).Normalize();
            up = Vector3.Cross(right, forward).Normalize();
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.LookAtLH(position, position + forward, up);
        }
    }
}
