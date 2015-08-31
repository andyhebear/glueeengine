using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.Core;
using GlueEngine.World.Entities;
using GlueEngine.World.Lights;
using Mogre.PhysX;
using System.IO;

namespace GlueEngine.World
{
    public class WorldManager
    {
        private ColourValue ambientLight = new ColourValue(0.5f, 0.5f, 0.5f);
        private Dictionary<string, WorldEntity> worldEntityMap = new Dictionary<string, WorldEntity>();
        private StaticGeometry staticGeometryCaster;
        private StaticGeometry staticGeometryReceiver;
        private List<ActorNode> actorNodeList = new List<ActorNode>();

        public WorldEntity[] WorldEntities
        {
            get
            {
                WorldEntity[] array = new WorldEntity[this.worldEntityMap.Values.Count];
                this.worldEntityMap.Values.CopyTo(array, 0);
                return array;
            }
        }

        public List<ActorNode> ActorNodes
        {
            get
            {
                return this.actorNodeList;
            }
        }

        public StaticGeometry StaticGeometryCaster
        {
            get
            {
                return this.staticGeometryCaster;
            }
        }

        public StaticGeometry StaticGeometryReceiver
        {
            get
            {
                return this.staticGeometryReceiver;
            }
        }

        public ColourValue AmbientLight
        {
            get
            {
                return this.ambientLight;
            }
        }

        public WorldManager()
        {
        }

        public bool LoadWorldEntities()
        {
            // load and map the world entities
            worldEntityMap.Add("DirectionalLight0", new DirectionalLight("DirectionalLight0", new Vector3(-0.5f, -1.0f, -0.3f)));
            worldEntityMap.Add("TestLevel", new StaticEntity("TestLevel", "test_level.mesh"));

            StaticEntity cage = new StaticEntity("PipeCage", "pipe_cage_001.mesh", 2);
            cage.CastShadows = true;
            cage.CollisionMode = CollisionMode.BoundingBox;
            worldEntityMap.Add("PipeCage", cage);

            DynamicEntity cone = new DynamicEntity("TrafficCone", "traffic_cone_001.mesh");
            cone.CollisionMode = CollisionMode.ConvexHull;
            worldEntityMap.Add("TrafficCone", cone);

            DynamicEntity pot = new DynamicEntity("FlowerPot", "pot_001.mesh");
            pot.CollisionMode = CollisionMode.ConvexHull;
            pot.CollisionSound = @"Media\sounds\brickhit.wav";
            worldEntityMap.Add("FlowerPot", pot);
            return true;
        }
       
        public bool Load(string filename)
        {
            // shadows
            Engine.Graphics.SceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
            Engine.Graphics.SceneManager.AmbientLight = this.ambientLight;
            Engine.Graphics.SceneManager.ShadowColour = this.ambientLight;

            if (!LoadWorldEntities())
                return false;

            // static mesh
            // TODO: implement depth shadow mapping
            // http://www.ogre3d.org/tikiwiki/Depth+Shadow+Mapping&bl=y&fullscreen=y
            staticGeometryCaster = Engine.Graphics.SceneManager.CreateStaticGeometry("StaticGeometryCaster");
            staticGeometryReceiver = Engine.Graphics.SceneManager.CreateStaticGeometry("StaticGeometryReceiver");
            staticGeometryCaster.CastShadows = true;

            StreamReader reader = new StreamReader(filename);
            string line = reader.ReadLine();

            // skip header
            if (line != null)
                line = reader.ReadLine();

            while (line != null)
            {
                string[] bits = line.Split('|');
                string key = bits[0];
                Vector3 position = StringConverter.ParseVector3(bits[1]);
                Quaternion orientation = StringConverter.ParseQuaternion(bits[2]);

                worldEntityMap[key].Spawn(position, orientation);

                line = reader.ReadLine();
            }

            reader.Close();

            staticGeometryCaster.Build();
            staticGeometryReceiver.Build();

            Engine.Graphics.Camera.Position = new Vector3(0, 0, 0);
            Engine.Graphics.Camera.Yaw(new Radian(new Degree(-15)));
            Engine.Sound.SetListenerPosition(Engine.Graphics.Camera.Position, Engine.Graphics.Camera.Direction);
            return true;
        }

        public void Update(float deltaTime)
        {
            foreach (ActorNode actorNode in this.actorNodeList)
                actorNode.Update(deltaTime);
        }

        public WorldEntity GetWorldEntity(string key)
        {
            if(this.worldEntityMap.ContainsKey(key))
                return this.worldEntityMap[key];

            return null;
        }

        public bool AddWorldEntity(string name, WorldEntity worldEntity)
        {
            if (!this.worldEntityMap.ContainsKey(name))
            {
                this.worldEntityMap.Add(name, worldEntity);
                return true;
            }

            return false;
        }
    }
}
