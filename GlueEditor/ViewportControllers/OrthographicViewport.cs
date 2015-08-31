using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Windows.Forms;
using GlueEditor.Core;
using GlueEngine.Core;

namespace GlueEditor.ViewportControllers
{
    public class OrthographicViewport : ViewportController
    {
        private Vector3 axis;
        private float zoom = 20;
        private Vector3 mouseDown;

        public OrthographicViewport(Vector3 axis)
        {
            this.axis = axis;
        }

        #region IViewportController Members

        public override void Initialise(string name, IntPtr handle)
        {
            this.renderWindow = Engine.Graphics.CreateRenderWindow(handle, Engine.UniqueName("OrthographicViewportRenderWindow"));

            this.camera = Engine.Graphics.SceneManager.CreateCamera(Engine.UniqueName("OrthographicCamera"));
            this.camera.ProjectionType = ProjectionType.PT_ORTHOGRAPHIC;
            this.camera.NearClipDistance = 0.1f;
            this.camera.FarClipDistance = 500.0f;
            this.camera.Position = axis * 100;
            this.camera.Orientation = Vector3.UNIT_Z.GetRotationTo(axis.NormalisedCopy);
            this.camera.SetOrthoWindow(zoom, zoom);
            this.camera.PolygonMode = PolygonMode.PM_WIREFRAME;

            this.viewport = renderWindow.AddViewport(camera);
            this.viewport.BackgroundColour = ColourValue.Black;

            if (axis == Vector3.UNIT_X || axis == Vector3.NEGATIVE_UNIT_X)
                this.viewport.SetVisibilityMask(VF_SIDE);

            if (axis == Vector3.UNIT_Y || axis == Vector3.NEGATIVE_UNIT_Y)
                this.viewport.SetVisibilityMask(VF_TOP);

            if (axis == Vector3.UNIT_Z || axis == Vector3.NEGATIVE_UNIT_Z)
                this.viewport.SetVisibilityMask(VF_FRONT);

            this.viewport.MaterialScheme = "WireframeScheme";

            this.renderWindow.PreViewportUpdate += new RenderTargetListener.PreViewportUpdateHandler(renderWindow_PreViewportUpdate);
            this.renderWindow.PostViewportUpdate += new RenderTargetListener.PostViewportUpdateHandler(renderWindow_PostViewportUpdate);
            this.cameraController = new CameraController(this.camera);            

        }

        public override void CreateScene()
        {
            this.grid = new Grid(1024, 1024, 1f);
            this.grid.Create(Engine.UniqueName("OrthographicGrid"), sceneManager, axis);
        }

        void renderWindow_PreViewportUpdate(RenderTargetViewportEvent_NativePtr evt)
        {            
            grid.Visible = true;
        }

        void renderWindow_PostViewportUpdate(RenderTargetViewportEvent_NativePtr evt)
        {
            grid.Visible = false;
        }

        public override void KeyUp(KeyEventArgs e)
        {
        }

        private Vector3 Forward
        {
            get
            {
                if (axis == Vector3.UNIT_Y || axis == Vector3.NEGATIVE_UNIT_Y)
                    return Vector3.UNIT_Z;

                if (axis == Vector3.UNIT_X || axis == Vector3.NEGATIVE_UNIT_X)
                    return Vector3.UNIT_Y;

                if (axis == Vector3.UNIT_Z || axis == Vector3.NEGATIVE_UNIT_Z)
                    return Vector3.UNIT_Y;

                return Vector3.ZERO;
            }
        }

        private Vector3 Sideways
        {
            get
            {
                if (axis == Vector3.UNIT_Y || axis == Vector3.NEGATIVE_UNIT_Y)
                    return Vector3.NEGATIVE_UNIT_X;

                if (axis == Vector3.UNIT_X || axis == Vector3.NEGATIVE_UNIT_X)
                    return Vector3.UNIT_Z;

                if (axis == Vector3.UNIT_Z || axis == Vector3.NEGATIVE_UNIT_Z)
                    return Vector3.NEGATIVE_UNIT_X;

                return Vector3.ZERO;
            }
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                this.camera.Move(this.Forward);

            if (e.KeyCode == Keys.A)
                this.camera.Move(this.Sideways);

            if (e.KeyCode == Keys.S)
                this.camera.Move(-this.Forward);

            if (e.KeyCode == Keys.D)
                this.camera.Move(-this.Sideways);
        }

        public override void Update()
        {
        }
        
        public override void MouseDown(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
                this.mouseDown = this.MouseToGrid(e.X, e.Y, false);

            base.MouseDown(e);
        }

        public override void MouseUp(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
                this.mouseDown = Vector3.ZERO;

            base.MouseUp(e);
        }

        public override void MouseMove(int dx, int dy, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Vector3 mousePos = this.MouseToGrid(e.X, e.Y, false);
                Vector3 mouseDelta = mousePos - this.mouseDown;
                this.camera.Move(-mouseDelta);
                this.mouseDown = mousePos - mouseDelta;
            }

            base.MouseMove(dx, dy, e);
        }

        public override void MouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                zoom -= e.Delta * 0.01f;
                camera.SetOrthoWindow(zoom * this.camera.AspectRatio, zoom);
            }                
        }

        #endregion
    }
}
