using System;
using System.Collections.Generic;
using System.Text;
using Mogre.PhysX;
using Mogre;
using System.IO;
using GlueEngine.World.Entities;

namespace GlueEngine.Core
{
    public class PhysicsManager : IUserContactReport
    {
        private const uint NX_Y = 1;

        private Physics physics;
        private Scene scene;

        public Scene Scene
        {
            get
            {
                return this.scene;
            }
        }

        public PhysicsManager()
        {
        }

        public bool Initiliase()
        {            
            this.physics = Physics.Create();
            this.physics.Parameters.SkinWidth = 0.0025f;
            this.physics.RemoteDebugger.Connect("localhost");
            this.physics.Parameters.ContinuousCd = 1;
            this.physics.Parameters.CcdEpsilon = 0.01f;

            SceneDesc sceneDesc = new SceneDesc();
            sceneDesc.SetToDefault();
            sceneDesc.Gravity = new Vector3(0, -9.8f, 0);
            sceneDesc.UpAxis = NX_Y;
            sceneDesc.TimeStepMethod = TimeStepMethods.Fixed;

            this.scene = physics.CreateScene(sceneDesc);
            this.scene.ActorGroupPairFlags[0, 0] = ContactPairFlags.NotifyOnStartTouch | ContactPairFlags.NotifyForces;
            this.scene.UserContactReport = this;
            
            // default material
            scene.Materials[0].Restitution = 0.2f;
            scene.Materials[0].StaticFriction = 0.8f;
            scene.Materials[0].DynamicFriction = 0.8f;

            // begin simulation
            this.scene.Simulate(0);
            return true;
        }
        
        public void Dispose()
        {
            this.physics.Dispose();
        }

        public void Update(double deltaTime)
        {
            this.physics.ControllerManager.UpdateControllers();

            this.scene.FlushStream();
            this.scene.FetchResults(SimulationStatuses.AllFinished, true);
            this.scene.Simulate(deltaTime);
        }

        public TriangleMeshShapeDesc CreateTriangleMesh(StaticMeshData meshData)
        {
            // create descriptor for triangle mesh
            TriangleMeshShapeDesc triangleMeshShapeDesc = null;
		    TriangleMeshDesc triangleMeshDesc = new TriangleMeshDesc();
            triangleMeshDesc.PinPoints<float>(meshData.Points, 0, sizeof(float) * 3);
            triangleMeshDesc.PinTriangles<uint>(meshData.Indices, 0, sizeof(uint) * 3);
            triangleMeshDesc.VertexCount = (uint)meshData.Vertices.Length;
            triangleMeshDesc.TriangleCount = (uint)meshData.TriangleCount;

            MemoryStream stream = new MemoryStream(1024);
            CookingInterface.InitCooking();

            if (CookingInterface.CookTriangleMesh(triangleMeshDesc, stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                TriangleMesh triangleMesh = physics.CreateTriangleMesh(stream);
                triangleMeshShapeDesc = new TriangleMeshShapeDesc(triangleMesh);
                CookingInterface.CloseCooking();
            }

            triangleMeshDesc.UnpinAll();
            return triangleMeshShapeDesc;
        }

        public ConvexShapeDesc CreateConvexHull(StaticMeshData meshData)
        {
            // create descriptor for convex hull
            ConvexShapeDesc convexMeshShapeDesc = null;
            ConvexMeshDesc convexMeshDesc = new ConvexMeshDesc();
            convexMeshDesc.PinPoints<float>(meshData.Points, 0, sizeof(float) * 3);
            convexMeshDesc.PinTriangles<uint>(meshData.Indices, 0, sizeof(uint) * 3);
            convexMeshDesc.VertexCount = (uint)meshData.Vertices.Length;
            convexMeshDesc.TriangleCount = (uint)meshData.TriangleCount;
            convexMeshDesc.Flags = ConvexFlags.ComputeConvex;

            MemoryStream stream = new MemoryStream(1024);
            CookingInterface.InitCooking();

            if (CookingInterface.CookConvexMesh(convexMeshDesc, stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                ConvexMesh convexMesh = physics.CreateConvexMesh(stream);
                convexMeshShapeDesc = new ConvexShapeDesc(convexMesh);
                CookingInterface.CloseCooking();
            }

            convexMeshDesc.UnpinAll();
            return convexMeshShapeDesc;
        }

        public void SetActorPairFlags(Actor actor1, Actor actor2, ContactPairFlags flags)
        {
            this.scene.ActorPairFlags[actor1, actor2] = flags;
        }

        public Controller CreateCharacterController(CapsuleControllerDesc desc)
        {
            return physics.ControllerManager.CreateController(this.scene, desc);
        }

        public Controller CreateCharacterController(BoxControllerDesc desc)
        {
            return physics.ControllerManager.CreateController(this.scene, desc);
        }

        public ActorDesc CreateActorDesc(EntityWorldEntity entityNode, Entity entity, Vector3 position, Quaternion orientation, Vector3 scale)
        {
            ActorDesc actorDesc = new ActorDesc();
            actorDesc.GlobalPosition = position;
            actorDesc.GlobalOrientation = orientation.ToRotationMatrix();

            if (entityNode.CollisionMode == CollisionMode.ConvexHull || entityNode.CollisionMode == CollisionMode.TriangleMesh)
            {
                StaticMeshData meshData = new StaticMeshData(entity.GetMesh(), scale);

                if (entityNode.CollisionMode == CollisionMode.TriangleMesh)
                    actorDesc.Shapes.Add(Engine.Physics.CreateTriangleMesh(meshData));
                else
                    actorDesc.Shapes.Add(Engine.Physics.CreateConvexHull(meshData));
            }
            else
            {
                switch (entityNode.CollisionMode)
                {
                    case CollisionMode.BoundingBox:
                        actorDesc.Shapes.Add(new BoxShapeDesc(entity.BoundingBox.HalfSize * scale, entity.BoundingBox.Center * scale));
                        break;
                    case CollisionMode.BoundingSphere:
                        actorDesc.Shapes.Add(new SphereShapeDesc(Engine.MaxAxis(entity.BoundingBox.HalfSize), entity.BoundingBox.Center * scale));
                        break;
                    case CollisionMode.Shapes:
                        foreach (ShapeDesc shapeDesc in entityNode.Shapes)
                            actorDesc.Shapes.Add(shapeDesc);
                        break;
                    default:
                        throw new Exception(entityNode.CollisionMode.ToString() + " not implemented");
                }
            }

            return actorDesc;
        }

        public void OnContactNotify(ContactPair pair, ContactPairFlags events)
        {
            Vector3 position = Vector3.ZERO;

            if (pair.ActorFirst.IsDynamic)
                position = pair.ActorFirst.GlobalPosition;
            else if (pair.ActorSecond.IsDynamic)
                position = pair.ActorSecond.GlobalPosition;

            float volume = pair.SumNormalForce.Length / 1000.0f;

            if (volume > 1.0f)
                volume = 1.0f;

            if (volume > 0.0f)
            {
                EntityWorldEntity node1 = pair.ActorFirst.UserData as EntityWorldEntity;
                EntityWorldEntity node2 = pair.ActorSecond.UserData as EntityWorldEntity;

                if (node1 != null)
                    Engine.Sound.Play3D(node1.CollisionSound, position, volume);

                if (node2 != null)
                    Engine.Sound.Play3D(node2.CollisionSound, position, volume);
            }
        }
    }
}
