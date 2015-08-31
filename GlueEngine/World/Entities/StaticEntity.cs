using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre.PhysX;
using GlueEngine.Core;

namespace GlueEngine.World.Entities
{
    public class StaticEntity : EntityWorldEntity
    {
        private bool castShadows = false;

        public bool CastShadows
        {
            get
            {
                return this.castShadows;
            }
            set
            {
                this.castShadows = value;
            }
        }

        public StaticEntity(string name, string mesh)
            : base(name, mesh)
        {
            this.collisionMode = CollisionMode.TriangleMesh;
        }

        public StaticEntity(string name, string mesh, float unitScale)
            : base(name, mesh, unitScale)
        {
            this.collisionMode = CollisionMode.TriangleMesh;
        }

        public StaticEntity(string name, string mesh, Vector3 scale)
            : base(name, mesh, scale)
        {
            this.collisionMode = CollisionMode.TriangleMesh;
        }

        public override void Spawn(Vector3 position, Quaternion orientation)
        {
            // create entity
            Entity entity = this.CreateEntity(); 

            // add it to the static geometry
            if (this.CastShadows)
                Engine.World.StaticGeometryCaster.AddEntity(entity, position, orientation, scale);
            else
                Engine.World.StaticGeometryReceiver.AddEntity(entity, position, orientation, scale);

            // create the physics mesh
            ActorDesc actorDesc = Engine.Physics.CreateActorDesc(this, entity, position, orientation, scale);
            Actor actor = Engine.Physics.Scene.CreateActor(actorDesc);
            actor.UserData = this;
        }
    }
}
