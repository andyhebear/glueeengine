using System;
using System.Collections.Generic;
using System.Text;
using GlueEditor.Tools;
using Mogre;
using GlueEditor.ViewportControllers;
using System.Xml;
using System.Windows.Forms;
using GlueEditor.WorldGeometry;
using GlueEngine.Core;
using GlueEngine.World;
using System.IO;

namespace GlueEditor.Core
{
    public static class Editor
    {
        private static string path = null;
        private static bool playing = false;
        private static bool snapToGrid = true;
        private static ITool tool = null;
        private static Widget widget;
        private static SelectionBox selectionBox = new SelectionBox();
        private static List<IViewportController> viewportControllers = new List<IViewportController>();
        private static List<EditNode> selectedNodes = new List<EditNode>();
        private static List<EditNode> editNodes = new List<EditNode>();
        private static List<Block> blockList = new List<Block>();

        public static event EventHandler OnCreateViewportController;
        public static PropertyGrid PropertyGrid { get; set; }

        public static ISelectableObject[] SelectableObjects
        {
            get
            {
                List<ISelectableObject> selectableObjectList = new List<ISelectableObject>();
                selectableObjectList.AddRange(editNodes.ToArray());
                selectableObjectList.AddRange(blockList.ToArray());
                return selectableObjectList.ToArray();
            }
        }

        public static List<Block> Blocks
        {
            get
            {
                return blockList;
            }
        }

        public static List<EditNode> SelectedNodes
        {
            get
            {
                return selectedNodes;
            }
        }

        public static bool MultiSelect 
        { 
            get;
            set; 
        }

        public static ITool Tool
        {
            get
            {
                return tool;
            }
            set
            {
                tool = value;
            }
        }

        public static SelectionBox SelectionBox
        {
            get
            {
                return selectionBox;
            }
        }

        public static Widget Widget
        {
            get
            {
                return widget;
            }
        }

        public static bool Playing
        {
            get
            {
                return playing;
            }
            set
            {
                playing = value;
            }
        }

        public static bool SnapToGrid
        {
            get
            {
                return snapToGrid;
            }
            set
            {
                snapToGrid = value;
            }
        }

        public static void Initialise()
        {
            GlueEditor.Design.DesignSupportManager.Initialiase();

            if (!Engine.Graphics.Initiliase(false))
                throw new Exception("Unable to initialise graphics");

            //if (!Engine.Physics.Initiliase())
            //    throw new Exception("Unable to initialise physics");

            Editor.OnCreateViewportController += new EventHandler(Editor_OnCreateViewportController);

            Engine.World.LoadWorldEntities();
        }

        private static void Editor_OnCreateViewportController(object sender, EventArgs e)
        {
            widget = new Widget();
            widget.Create();
            widget.Visible = true;
        }

        public static void ShutDown()
        {
            Engine.Graphics.Dispose();
            //Engine.Physics.Dispose();
        }

        public static void Update(float deltaTime)
        {
            //if(Editor.Playing)
            //    Engine.Physics.Update(deltaTime);

            bool valid = true;

            foreach (IViewportController controller in viewportControllers)
            {
                // this test prevents rendering if it finds an invalid viewport
                // however, it may be more correct to test for at least one valid viewport
                // but there is more work to implement it this way
                if (controller.RenderWindow.Width == 0 || controller.RenderWindow.Height == 0)
                    valid = false;

                controller.Update();
            }

            if (valid)
                Engine.Graphics.Render();
        }

        public static Vector3 SnapTo(Vector3 position)
        {
            if (Editor.SnapToGrid)
            {
                Vector3 newPos = new Vector3();
                newPos.x = (float)System.Math.Round(position.x);
                newPos.y = (float)System.Math.Round(position.y);
                newPos.z = (float)System.Math.Round(position.z);
                return newPos;
            }

            return position;
        }

        private static IViewportController RegisterViewportController(IViewportController controller, ViewportPanel control)
        {
            control.Initialise(controller);

            // first viewport
            if (viewportControllers.Count == 0)
            {
                ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

                // lighting
                Engine.Graphics.SceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
                Engine.Graphics.SceneManager.AmbientLight = new ColourValue(0.85f, 0.85f, 0.85f);
                Engine.Graphics.SceneManager.ShadowColour = new ColourValue(0.75f, 0.75f, 0.75f);

                // default light
                Light light = Engine.Graphics.SceneManager.CreateLight("defaultLight");
                light.DiffuseColour = ColourValue.White;
                light.SpecularColour = ColourValue.White;
                light.Direction = new Vector3(0.2f, -1.0f, 0.2f);
                light.Type = Light.LightTypes.LT_DIRECTIONAL;

                if (OnCreateViewportController != null)
                    OnCreateViewportController(controller, EventArgs.Empty);
            }

            controller.SizeChanged();
            controller.CreateScene();

            viewportControllers.Add(controller);
            return controller;
        }

        public static PerspectiveViewport CreatePerspectiveViewport(ViewportPanel viewportControl)
        {
            PerspectiveViewport controller = new PerspectiveViewport();
            RegisterViewportController(controller, viewportControl);
            return controller;
        }

        public static OrthographicViewport CreateOrthographicViewport(ViewportPanel viewportControl, Vector3 axis)
        {
            OrthographicViewport controller = new OrthographicViewport(axis);
            RegisterViewportController(controller, viewportControl);
            return controller;
        }

        public static float GridPlus()
        {
            foreach (IViewportController viewportController in viewportControllers)
                viewportController.Grid.UnitSize *= 2.0f;

            return viewportControllers[0].Grid.UnitSize;
        }

        public static float GridMinus()
        {
            foreach (IViewportController viewportController in viewportControllers)
                viewportController.Grid.UnitSize *= 0.5f;

            return viewportControllers[0].Grid.UnitSize;
        }

        public static Vector2 MinOf(Vector2 v1, Vector2 v2)
        {
            Vector2 min = new Vector2();

            // x
            if (v1.x < v2.x)
                min.x = v1.x;
            else
                min.x = v2.x;

            // y
            if (v1.y < v2.y)
                min.y = v1.y;
            else
                min.y = v2.y;

            return min;
        }

        public static Vector2 MaxOf(Vector2 v1, Vector2 v2)
        {
            Vector2 max = new Vector2();

            // x
            if (v1.x > v2.x)
                max.x = v1.x;
            else
                max.x = v2.x;

            // y
            if (v1.y > v2.y)
                max.y = v1.y;
            else
                max.y = v2.y;

            return max;
        }

        public static Vector3 MaxOf(Vector3 v1, Vector3 v2)
        {
            Vector3 max = new Vector3();

            // x
            if (v1.x > v2.x)
                max.x = v1.x;
            else
                max.x = v2.x;

            // y
            if (v1.y > v2.y)
                max.y = v1.y;
            else
                max.y = v2.y;

            // z
            if (v1.z > v2.z)
                max.z = v1.z;
            else
                max.z = v2.z;

            return max;
        }

        public static Vector3 MinOf(Vector3 v1, Vector3 v2)
        {
            Vector3 min = new Vector3();

            // x
            if (v1.x < v2.x)
                min.x = v1.x;
            else
                min.x = v2.x;

            // y
            if (v1.y < v2.y)
                min.y = v1.y;
            else
                min.y = v2.y;

            // z
            if (v1.z < v2.z)
                min.z = v1.z;
            else
                min.z = v2.z;

            return min;
        }

        public static void CreateEditNode(string key, Vector3 position)
        {
            CreateEditNode(key, position, Quaternion.IDENTITY);
        }

        public static void CreateEditNode(string key, Vector3 position, Quaternion orientation)
        {
            WorldEntity worldEntity = Engine.World.GetWorldEntity(key);
            SceneNode sceneNode = worldEntity.CreateSceneNode(position, orientation);

            if (sceneNode != null)
            {
                EditNode editNode = new EditNode(key, worldEntity, sceneNode);
                ushort numObjects = sceneNode.NumAttachedObjects();
                ushort numEntities = 0;

                for (ushort i = 0; i < numObjects; i++)
                {
                    Entity entity = sceneNode.GetAttachedObject(i) as Entity;

                    // if we haven't found an entity by now we need to make one ourselves
                    if (entity == null && i == numObjects - 1 && numEntities == 0)
                    {
                        entity = Engine.Graphics.SceneManager.CreateEntity("ball.mesh");
                        sceneNode.AttachObject(entity);
                    }

                    // setup special material scheme used in the editor only
                    if (entity != null)
                    {
                        for (uint j = 0; j < entity.NumSubEntities; j++)
                        {
                            Technique technique = entity.GetSubEntity(j).GetMaterial().CreateTechnique();
                            technique.SchemeName = "WireframeScheme";
                            Pass pass = technique.CreatePass();
                            pass.LightingEnabled = false;
                            TextureUnitState textureUnit = pass.CreateTextureUnitState();
                            textureUnit.SetColourOperationEx(LayerBlendOperationEx.LBX_SOURCE1, LayerBlendSource.LBS_MANUAL, LayerBlendSource.LBS_CURRENT,
                                new ColourValue(
                                    0.5f + Mogre.Math.RangeRandom(0, 0.5f),
                                    0.5f + Mogre.Math.RangeRandom(0, 0.5f),
                                    0.5f + Mogre.Math.RangeRandom(0, 0.5f)));
                        }

                        entity.UserObject = editNode;
                        numEntities++;
                    }
                }

                Editor.editNodes.Add(editNode);
            }
        }

        public static bool Save(string path)
        {
            Editor.path = path;

            StreamWriter writer = new StreamWriter(path);

            writer.Write("BlockCount:");
            writer.Write(blockList.Count);
            writer.WriteLine();
            
            foreach (Block block in Editor.blockList)
                block.Save(writer);

            writer.WriteLine("WorldEntity|Position|Orientation");

            foreach (EditNode editNode in Editor.editNodes)
            {                
                string key = editNode.Key;
                string position = StringConverter.ToString(editNode.Position);
                string orientation = StringConverter.ToString(editNode.Orientation);
                writer.WriteLine(key + "|" + position + "|" + orientation);
            }

            writer.Close();
            return true;
        }

        public static bool Save()
        {
            if (Editor.path == null)
                return false;

            return Save(Editor.path);
        }

        public static void Load(string path)
        {
            Editor.path = path;

            StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();
            string[] bits = line.Split(':');
            int blockCount = int.Parse(bits[1]);

            for (int i = 0; i < blockCount; i++)
            {
                Block block = Block.Load(reader);
                block.Create();
                blockList.Add(block);
            }
            
            
            line = reader.ReadLine();

            // skip header
            if (line != null)
                line = reader.ReadLine();

            while (line != null)
            {
                bits = line.Split('|');
                string key = bits[0];
                Vector3 position = StringConverter.ParseVector3(bits[1]);
                Quaternion orientation = StringConverter.ParseQuaternion(bits[2]);

                CreateEditNode(key, position, orientation);
                line = reader.ReadLine();
            }

            reader.Close();
        }


        public static void Close()
        {
            throw new NotImplementedException();
        }

        internal static void New()
        {
            throw new NotImplementedException();
        }

        public static Block SelectBlock(IViewportController viewportController, int x, int y)
        {
            Ray ray = viewportController.CreateViewportRay(x, y);
            Block hitBlock = null;
            float d = float.MaxValue;

            foreach (Block block in Editor.Blocks)
            {
                Pair<bool, float> pair = ray.Intersects(block.BoundingBox);

                if (pair.first && pair.second < d)
                {
                    d = pair.second;
                    hitBlock = block;
                }
            }

            return hitBlock;
        }
    }
}
