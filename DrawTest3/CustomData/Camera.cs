using DrawTest3.CustomMath;
using DrawTest3.CustomTool;
using System;

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

            yaw = 0.0f;
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
            float speed = 0.05f;

            if (direction == Camera_Movement_Type.Forward)
            {
                position = position + forward * speed;
            }
            if (direction == Camera_Movement_Type.Backward)
            {
                position = position - forward * speed;
            }
            if (direction == Camera_Movement_Type.Left)
            {
                position = position - right * speed;
            }
            if (direction == Camera_Movement_Type.Right)
            {
                position = position + right * speed;
            }
        }

        //yaw y
        //pitch x
        public void Rotate(Matrix rotateMatrix)
        {
            right = rotateMatrix.GetAxis(0).Normalize();
            up = rotateMatrix.GetAxis(1).Normalize();
            forward = rotateMatrix.GetAxis(2).Normalize();

            /*Console.WriteLine("右" + right.toDirection());
            Console.WriteLine("上" + up.toDirection());
            Console.WriteLine("前" + forward.toDirection());*/
        }

        public float yaw;
        public float pitch;

        /// <summary>
        /// x pitch
        /// y yaw
        /// z roll
        /// </summary>
        public void UpdateCameraVectors(float pitch, float yaw)
        {
            this.yaw += yaw * 0.005f;
            this.pitch += pitch * 0.005f;

            if (this.pitch > 89.0f)
            {
                this.pitch = 89.0f;
            }
            if (this.pitch < -89.0f)
            {
                this.pitch = -89.0f;
            }

            Matrix rotateMatrix = Matrix.RotateX(this.pitch) * Matrix.RotateY(this.yaw);
            Rotate(rotateMatrix);
        }

        private void UpdateCameraVectors()
        {
            Vector3 front = new Vector3();

            front.x = (float)(Math.Cos(yaw).ToRadians() * Math.Cos(pitch).ToRadians());
            front.y = (float)(Math.Sin(pitch).ToRadians());
            front.z = (float)(Math.Sin(yaw).ToRadians() * Math.Cos(pitch).ToRadians());

            forward = front.Normalize();

            right = Vector3.Cross(forward, Vector3.up).Normalize();
            up = Vector3.Cross(right, forward).Normalize();
        }

        public void UpdateCameraFOV(float offset)
        {
            fov += offset;
            fov = UnityEngine.Mathf.Clamp(fov, 0, 90f);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.LookAtLH(position, position + forward, up);
        }
    }
}
