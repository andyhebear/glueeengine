using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre.PhysX;
using GlueEngine.Core;
using System.ComponentModel;
using GlueEngine.Design;

namespace GlueEngine.World.Entities
{
    public abstract class EntityWorldEntity : WorldEntity
    {
        protected string mesh;
        protected Vector3 scale = Vector3.UNIT_SCALE;
        protected List<ShapeDesc> shapeList = new List<ShapeDesc>();
        protected CollisionMode collisionMode = CollisionMode.BoundingBox;
        private string collisionSound = "";
        private List<SceneNode> sceneNodeList = new List<SceneNode>();

        public string CollisionSound
        {
            get
            {
                return this.collisionSound;
            }
            set
            {
                this.collisionSound = value;
            }
        }

        public CollisionMode CollisionMode
        {
            get
            {
                return this.collisionMode;
            }
            set
            {
                this.collisionMode = value;
            }
        }

        public List<ShapeDesc> Shapes
        {
            get
            {
                return this.shapeList;
            }
        }
        
        [TypeConverter(typeof(MeshListTypeConverter))]
        public string Mesh
        {
            get
            {
                return this.mesh;
            }
            set
            {
                this.mesh = value;

                foreach (SceneNode sceneNode in sceneNodeList)
                {
                    Entity entity = sceneNode.GetAttachedObject(0) as Entity;
                    Engine.Graphics.SceneManager.DestroyEntity(entity);

                    entity = CreateEntity();
                    sceneNode.AttachObject(entity);
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                if (scale != Vector3.ZERO)
                {
                    this.scale = value;

                    foreach (SceneNode sceneNode in sceneNodeList)
                        sceneNode.SetScale(value);
                }
            }
        }

        public float UnitScale
        {
            set
            {
                this.scale = Vector3.UNIT_SCALE * value;
            }
        }

        public EntityWorldEntity(string name, string mesh) 
            : base(name)
        {
            this.mesh = mesh;
        }

        public EntityWorldEntity(string name, string mesh, float unitScale)
            : base(name)
        {
            this.mesh = mesh;
            this.UnitScale = unitScale;
        }

        public EntityWorldEntity(string name, string mesh, Vector3 scale)
            : base(name)
        {
            this.mesh = mesh;
            this.scale = scale;
        }
        
        protected Entity CreateEntity()
        {
            return Engine.Graphics.SceneManager.CreateEntity(Engine.UniqueName("EntityNode"), this.mesh);
        }

        public override SceneNode CreateSceneNode(Vector3 position, Quaternion orientation)
        {
            Entity entity = CreateEntity();
            SceneNode sceneNode = Engine.Graphics.SceneManager.RootSceneNode.CreateChildSceneNode();
            
            sceneNode.AttachObject(entity);
            sceneNode.SetScale(this.scale);
            sceneNode.Position = position;
            sceneNode.Orientation = orientation;

            sceneNodeList.Add(sceneNode);

            return sceneNode;
        }
    }
}
