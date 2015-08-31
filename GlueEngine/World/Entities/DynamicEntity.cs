using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre.PhysX;
using GlueEngine.Core;

namespace GlueEngine.World.Entities
{
    public class DynamicEntity : EntityWorldEntity
    {
        private Vector3 velocity = Vector3.ZERO;
        private float density = 1.0f;
        private bool enableCCD = false;

        public bool EnableCCD
        {
            get
            {
                return this.enableCCD;
            }
            set
            {
                this.enableCCD = value;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
            }
        }

        public float Density 
        {
            get
            {
                return this.density;
            }
            set
            {
                this.density = value;
            }
        }

        public DynamicEntity(string name, string mesh)
            : base(name, mesh)
        {
        }

        public DynamicEntity(string name, string mesh, float unitScale)
            : base(name, mesh, unitScale)
        {
        }

        public DynamicEntity(string name, string mesh, Vector3 scale)
            : base(name, mesh, scale)
        {
        }

        public override void Spawn(Vector3 position, Quaternion orientation)
        {
            // create scene node
            SceneNode sceneNode = this.CreateSceneNode(position, orientation);
            Entity entity = sceneNode.GetAttachedObject(0) as Entity;

            // physics
            BodyDesc bodyDesc = new BodyDesc();
            bodyDesc.LinearVelocity = this.Velocity;

            ActorDesc actorDesc = Engine.Physics.CreateActorDesc(this, entity, position, orientation, this.Scale);
            actorDesc.Density = this.Density;
            actorDesc.Body = bodyDesc;
            actorDesc.GlobalPosition = position;
            actorDesc.GlobalOrientation = orientation.ToRotationMatrix();

            if (this.EnableCCD)
            {
                foreach (ShapeDesc shapeDesc in actorDesc.Shapes)
                    shapeDesc.ShapeFlags = ShapeFlags.DynamicDynamicCCD;
            }

            Actor actor = Engine.Physics.Scene.CreateActor(actorDesc);
            actor.UserData = this;

            ActorNode actorNode = new ActorNode(sceneNode, actor);
            Engine.World.ActorNodes.Add(actorNode);
        }
    }
}
