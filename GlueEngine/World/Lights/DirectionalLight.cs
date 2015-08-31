using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEngine.World.Lights
{
    public class DirectionalLight : LightWorldEntity
    {
        private Vector3 direction = Vector3.NEGATIVE_UNIT_Y;

        public Vector3 Direction
        {
            get
            {
                return this.direction;
            }
        }

        public override Light.LightTypes Type
        {
            get 
            {
                return Light.LightTypes.LT_DIRECTIONAL;
            }
        }

        public DirectionalLight(string name) : base(name)
        {
        }

        public DirectionalLight(string name, Vector3 direction) : base(name)
        {
            this.direction = direction;
        }

        protected override void OnCreate(Light light)
        {
            light.Direction = this.Direction;
        }
    }
}
