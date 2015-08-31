using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEditor.ViewportControllers
{
    public class RaycastResult
    {
        private Ray ray;

        public Ray Ray
        {
            get
            {
                return ray;
            }
        }

        public float Distance
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 Normal
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        public RaycastResult(Ray ray)
        {
            this.ray = ray;
        }
    }
}
