using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEngine.World.Lights
{
    public class PointLight : LightWorldEntity
    {
        public override Light.LightTypes Type
        {
            get
            {
                return Light.LightTypes.LT_POINT;
            }
        }

        public PointLight(string name) : base(name)
        {
        }

        protected override void OnCreate(Light light)
        {
        }
    }
}
