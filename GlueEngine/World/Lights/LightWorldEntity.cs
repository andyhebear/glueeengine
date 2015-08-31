using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.Core;

namespace GlueEngine.World.Lights
{
    public abstract class LightWorldEntity : WorldEntity
    {
        private ColourValue colour = ColourValue.White;

        public string Key
        {
            get { throw new NotImplementedException(); }
        }

        public ColourValue Colour
        {
            get
            {
                return this.colour;
            }
        }

        public abstract  Light.LightTypes Type
        {
            get;
        }

        public LightWorldEntity(string name) 
            : base(name) 
        {
        }

        protected abstract void OnCreate(Light light);

        private Light CreateLight()
        {
            Light light = Engine.Graphics.SceneManager.CreateLight(Engine.UniqueName("Light"));
            light.Type = this.Type;
            light.DiffuseColour = this.Colour;
            
            OnCreate(light);
            return light;
        }

        public override void Spawn(Vector3 position, Quaternion orientation)
        {
            // spawn a static light
            Light light = CreateLight();
            light.Position = position;
        }

        public override SceneNode CreateSceneNode(Vector3 position, Quaternion orientation)
        {
            Light light = CreateLight();
            SceneNode sceneNode = Engine.Graphics.SceneManager.RootSceneNode.CreateChildSceneNode();
            sceneNode.AttachObject(light);
            sceneNode.Position = position;
            sceneNode.Orientation = orientation;
            return sceneNode;
        }
    }
}
