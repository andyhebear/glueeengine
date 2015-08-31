using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEditor.ViewportControllers
{
    public class CameraController
    {
        private Camera camera;

        public CameraController(Camera camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// Creates a movement plane based on the camera direction. 
        /// It will never be the same plane as the movement axis.
        /// </summary>
        /// <param name="moveAxis">Axis that movement will occur on</param>
        /// <returns>A movement plane</returns>
        public Plane GetActivePlane(Vector3 moveAxis, Vector3 position)
        {
            Vector3 activeAxis;
            float absX = Mogre.Math.Abs(camera.Direction.x);
            float absY = Mogre.Math.Abs(camera.Direction.y);
            float absZ = Mogre.Math.Abs(camera.Direction.z);

            if (absX > absY && absX > absZ && moveAxis != Vector3.UNIT_X)    // X is greatest
                activeAxis = Vector3.UNIT_X;
            else if (absY >= absZ && moveAxis != Vector3.UNIT_Y)             // Y is greatest
                activeAxis = Vector3.UNIT_Y;
            else if (moveAxis != Vector3.UNIT_Z)                             // Z is greatest
                activeAxis = Vector3.UNIT_Z;
            else
                throw new Exception("Invalid active plane");    // can occur (need to investigate, maybe upright cam can help)

            return new Plane(activeAxis, position);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 direction = new Vector3(x, y, z);
            camera.Move(camera.Orientation * direction);
        }

        public void Rotate(int x, int y)
        {
            float fx = x / 1000.0f;
            float fy = y / 1000.0f;
            camera.Yaw(new Radian(fx));
            camera.Pitch(new Radian(fy));
        }

        public bool IsLookingDownAxis(Vector3 axis)
        {
            if (this.camera.Direction == axis)
                return true;

            if (this.camera.Direction == -axis)
                return true;

            return false;
        }
    }
}
