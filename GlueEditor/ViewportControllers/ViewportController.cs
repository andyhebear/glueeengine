using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEditor.Core;
using System.Windows.Forms;
using GlueEngine.Core;

namespace GlueEditor.ViewportControllers
{
    public abstract class ViewportController : IViewportController
    {
        public const uint VF_PERSPECTIVE = 1;
        public const uint VF_TOP = 2;
        public const uint VF_SIDE = 4;
        public const uint VF_FRONT = 8;
        public const uint VF_ORTHOGRAPHIC = VF_TOP + VF_SIDE + VF_FRONT;

        //protected Root root;
        protected SceneManager sceneManager;
        protected Camera camera;
        protected Viewport viewport;
        protected RenderWindow renderWindow;
        protected CameraController cameraController;
        protected Grid grid;

        #region IViewportController Members

        public event CursorEventHandler CursorChanged;

        public Cursor Cursor
        {
            set
            {
                if (CursorChanged != null)
                    CursorChanged(value);
            }
        }

        public CameraController CameraController
        {
            get
            {
                return this.cameraController;
            }
        }

        public Camera Camera
        {
            get
            {
                return this.camera;
            }
        }

        public RenderWindow RenderWindow
        {
            get
            {
                return this.renderWindow;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return this.viewport;
            }
        }

        public Grid Grid
        {
            get
            {
                return this.grid;
            }
        }

        public ViewportController()
        {
            this.sceneManager = Engine.Graphics.SceneManager;
        }
        
        public abstract void Initialise(string name, IntPtr handle);
        public abstract void KeyUp(KeyEventArgs e);
        public abstract void KeyDown(KeyEventArgs e);
        public abstract void MouseWheel(MouseEventArgs e);
        public abstract void Update();

        public virtual void MouseDown(MouseEventArgs e)
        {
            if (Editor.Tool != null)
                Editor.Tool.MouseDown(this, e);
        }

        public virtual void MouseUp(MouseEventArgs e)
        {
            if (Editor.Tool != null)
                Editor.Tool.MouseUp(this, e);
        }

        public virtual void MouseMove(int dx, int dy, MouseEventArgs e)
        {
            if (Editor.Tool != null)
                Editor.Tool.MouseMove(this, dx, dy, e);
        }        

        public void SizeChanged()
        {
            if (renderWindow != null)
            {
                renderWindow.WindowMovedOrResized();
                camera.AspectRatio = (float)viewport.ActualWidth / (float)viewport.ActualHeight;
            }
        }

        private RaycastResult RaycastToPlane(Ray ray, Plane plane)
        {
            Pair<bool, float> pair = ray.Intersects(plane);

            if (pair.first)
            {
                RaycastResult result = new RaycastResult(ray);
                result.Distance = pair.second;
                result.Normal = plane.normal;
                result.Position = ray.GetPoint(pair.second);
                return result;
            }

            return null;
        }

        public Ray CreateViewportRay(float x, float y)
        {
            float tx = x / this.renderWindow.Width;
            float ty = y / this.renderWindow.Height;

            return this.camera.GetCameraToViewportRay(tx, ty);
        }

        public Vector3 MouseToGrid(int x, int y)
        {
            return MouseToGrid(x, y, Editor.SnapToGrid);
        }

        public Vector3 MouseToGrid(int x, int y, bool snapToGrid)
        {
            Plane plane = new Plane(grid.Normal, grid.Position);
            RaycastResult result = MouseToPlane(x, y, plane);

            if (result != null)
            {
                if (snapToGrid)
                    return grid.SnapTo(result.Position);
                else
                    return result.Position;
            }

            return Vector3.ZERO;
        }

        public RaycastResult MouseToPlane(int x, int y, Plane plane)
        {
            Ray ray = this.CreateViewportRay(x, y);

            return RaycastToPlane(ray, plane);
        }

        public Vector2 GetScreenPosition(Vector3 position)
        {
            Vector3 eyeSpacePos = this.camera.GetViewMatrix(true) * position;
            Vector2 screenPos = Vector2.ZERO;

            // z < 0 means in front of cam
            if (eyeSpacePos.z < 0)
            {
                // calculate projected pos
                Vector3 p = this.camera.ProjectionMatrix * eyeSpacePos;
                screenPos.x = p.x;
                screenPos.y = -p.y;
            }
            else
            {
                screenPos.x = (-eyeSpacePos.x > 0) ? -1 : 1;
                screenPos.y = (-eyeSpacePos.y > 0) ? -1 : 1;
            }

            screenPos.x = ((screenPos.x + 1.0f) * 0.5f) * this.viewport.ActualWidth;
            screenPos.y = ((screenPos.y + 1.0f) * 0.5f) * this.viewport.ActualHeight;

            return screenPos;
        }

        public abstract void CreateScene();

        #endregion
    }
}
