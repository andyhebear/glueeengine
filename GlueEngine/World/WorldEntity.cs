using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEngine.World
{
    public abstract class WorldEntity
    {
        private string name;

        public event EventHandler NameChanged = null;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;

                if (NameChanged != null)
                    NameChanged(this, EventArgs.Empty);
            }
        }

        public WorldEntity(string name)
        {
            this.name = name;
        }

        public abstract SceneNode CreateSceneNode(Vector3 position, Quaternion orientation);
        public abstract void Spawn(Vector3 position, Quaternion orientation);

        public override string ToString()
        {
            return this.name;
        }
    }
}
