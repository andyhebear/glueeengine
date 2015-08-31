using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Runtime.InteropServices;

namespace GlueEngine.Core
{
    public class GraphicsManager
    {
        private RaySceneQuery raySceneQuery = null;
        private Root root;
        private SceneManager sceneManager;
        private RenderWindow renderWindow;
        private Viewport viewport;
        private Camera camera;

        public RenderTarget.FrameStats FrameStats
        {
            get
            {
                return this.renderWindow.GetStatistics();
            }
        }

        public Camera Camera
        {
            get
            {
                return this.camera;
            }
            set
            {
                this.camera = value;
            }
        }

        public SceneManager SceneManager
        {
            get
            {
                return this.sceneManager;
            }
        }

        public IntPtr WindowHandle
        {
            get
            {
                IntPtr handle;
                this.renderWindow.GetCustomAttribute("WINDOW", out handle);
                return handle;
            }
        }

        public GraphicsManager()
        {
        }

        public bool Initiliase(bool autoCreateWindow)
        {
            root = new Root();

            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);

            // Go through all sections & settings in the file
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();

            String secName, typeName, archName;

            // Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }

            //----------------------------------------------------- 
            // 3 Configures the application and creates the window
            //----------------------------------------------------- 
            string renderSystem = "Direct3D9 Rendering Subsystem";

            if (!FindRenderSystem(renderSystem))
                throw new Exception("Unable to find render system: " + renderSystem);

            //we found it, we might as well use it!
            root.RenderSystem.SetConfigOption("VSync", "Yes");      // graphics card doesn't work so hard, physics engine should be more stable
            root.RenderSystem.SetConfigOption("Full Screen", "No");
            root.RenderSystem.SetConfigOption("Video Mode", "1024 x 768 @ 32-bit colour");

            // scene manager
            sceneManager = root.CreateSceneManager(SceneType.ST_GENERIC);

            // render window
            this.renderWindow = root.Initialise(autoCreateWindow, "GlueEngine");
            return true;
        }

        public bool Initiliase(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
            {
                // auto create the window
                if (!Initiliase(true))
                    return false;
            }
            else
            {
                // we'll create the window
                if(!Initiliase(false))
                    return false;

                // manual creation of render window
                this.renderWindow = this.CreateRenderWindow(windowHandle, "DefaultRenderWindow");
            }   
            
            // camera
            this.camera = sceneManager.CreateCamera("DefaultCamera");
            this.camera.NearClipDistance = 0.1f;

            // viewport
            this.viewport = renderWindow.AddViewport(camera);
            this.viewport.BackgroundColour = ColourValue.Black;

            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
            return true;
        }

        public void StartRendering()
        {
            Overlay debugOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
            debugOverlay.Show();
            this.root.FrameStarted += new FrameListener.FrameStartedHandler(root_FrameStarted);
            this.root.StartRendering();
        }

        private bool root_FrameStarted(FrameEvent evt)
        {
            UpdateStats();
            return Engine.Update(evt.timeSinceLastFrame);            
        }

        private void UpdateStats()
        {
            string currFps = "Current FPS: ";
            string avgFps = "Average FPS: ";
            string bestFps = "Best FPS: ";
            string worstFps = "Worst FPS: ";
            string tris = "Triangle Count: ";

            // update stats when necessary
            try
            {
                OverlayElement guiAvg = OverlayManager.Singleton.GetOverlayElement("Core/AverageFps");
                OverlayElement guiCurr = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps");
                OverlayElement guiBest = OverlayManager.Singleton.GetOverlayElement("Core/BestFps");
                OverlayElement guiWorst = OverlayManager.Singleton.GetOverlayElement("Core/WorstFps");

                RenderTarget.FrameStats stats = this.renderWindow.GetStatistics();

                guiAvg.Caption = avgFps + stats.AvgFPS;
                guiCurr.Caption = currFps + stats.LastFPS;
                guiBest.Caption = bestFps + stats.BestFPS + " " + stats.BestFrameTime + " ms";
                guiWorst.Caption = worstFps + stats.WorstFPS + " " + stats.WorstFrameTime + " ms";

                OverlayElement guiTris = OverlayManager.Singleton.GetOverlayElement("Core/NumTris");
                guiTris.Caption = tris + stats.TriangleCount;
            }
            catch
            {
                // ignore
            }
        }

        private bool FindRenderSystem(string name)
        {
            foreach (RenderSystem rs in root.GetAvailableRenderers())
            {
                if (rs.Name == name)
                {
                    root.RenderSystem = rs;
                    return true;
                }
            }
            
            return false;
        }

        public void Render()
        {
            try
            {
                if (root != null)
                    root.RenderOneFrame();
            }
            catch (SEHException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        public RenderWindow CreateRenderWindow(IntPtr handle, string name)
        {
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = handle.ToString();
            return root.CreateRenderWindow(name, 0, 0, false, misc);
        }

        public void Dispose()
        {
            if (root != null)
            {
                root.Dispose();
                root = null;
            }
        }

        protected RaySceneQueryResult RaycastBoundingBox(Ray ray)
        {
            raySceneQuery = this.sceneManager.CreateRayQuery(ray);
            raySceneQuery.SetSortByDistance(true);

            // check we are initialised
            if (raySceneQuery != null)
            {
                // create a query object
                raySceneQuery.Ray = ray;
                raySceneQuery.SetSortByDistance(true);
                raySceneQuery.QueryMask = 1;

                // execute the query, returns a vector of hits
                return raySceneQuery.Execute();
            }
            
            return null;            
        }

        public string[] GetMeshList()
        {
            List<string> fileList = new List<string>();
            FileInfoListPtr fileInfoList;

            foreach (string group in ResourceGroupManager.Singleton.GetResourceGroups())
            {
                fileInfoList = ResourceGroupManager.Singleton.FindResourceFileInfo(group, "*.mesh");

                for (int i = 0; i < fileInfoList.Count; i++)
                    fileList.Add(fileInfoList[i].filename);
            }

            return fileList.ToArray();
        }

        public string[] GetMaterialGroups()
        {
            List<string> groupList = new List<string>();

            foreach (ResourcePtr map in MaterialManager.Singleton.GetResourceIterator())
            {
                if (!groupList.Contains(map.Group))
                    groupList.Add(map.Group);
            }

            return groupList.ToArray();
        }

        public string[] GetMaterialList(string group)
        {
            List<string> materialList = new List<string>();

            foreach (ResourcePtr map in MaterialManager.Singleton.GetResourceIterator())
            {
                if (map.Group == group)
                {
                    materialList.Add(map.Name);
                }
            }

            return materialList.ToArray();
        }


    }
}
