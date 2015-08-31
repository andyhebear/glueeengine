using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.Core;

namespace GlueEngine.World.Lights
{
    public class SpotLight : LightWorldEntity
    {
        public override Light.LightTypes Type
        {
            get 
            {
                return Light.LightTypes.LT_SPOTLIGHT;
            }
        }

        public SpotLight(string name) : base(name)
        {
        }

        protected override void OnCreate(Light light)
        {
        }
    }
}
