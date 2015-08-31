using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Windows.Forms;
using GlueEditor.Core;
using GlueEngine.Core;

namespace GlueEditor.ViewportControllers
{
    public class PerspectiveViewport : ViewportController
    {
        private Vector3 direction = Vector3.ZERO;

        public PerspectiveViewport()
        {
        }
        
        public override void Initialise(string name, IntPtr handle)
        {
            this.renderWindow = Engine.Graphics.CreateRenderWindow(handle, Engine.UniqueName("PerspectiveViewportRenderWindow"));

            this.camera = sceneManager.CreateCamera(Engine.UniqueName("PerspectiveViewportCamera"));
            this.camera.Position = new Vector3(0, 20, 20);
            this.camera.LookAt(Vector3.ZERO);
            this.camera.NearClipDistance = 0.1f;
            Engine.Graphics.Camera = this.camera;

            this.viewport = renderWindow.AddViewport(camera);
            this.viewport.BackgroundColour = ColourValue.Black;
            this.viewport.SetVisibilityMask(VF_PERSPECTIVE);

            this.cameraController = new CameraController(this.camera);
            
            this.renderWindow.PreViewportUpdate += new RenderTargetListener.PreViewportUpdateHandler(renderWindow_PreViewportUpdate);
            this.renderWindow.PostViewportUpdate += new RenderTargetListener.PostViewportUpdateHandler(renderWindow_PostViewportUpdate);
        }
        
        public override void CreateScene()
        {
            this.grid = new Grid(256, 256, 1f);
            this.grid.Create(Engine.UniqueName("PerspectiveGrid"), sceneManager, Vector3.UNIT_Y);
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
            direction = Vector3.ZERO;
        }

        public override void KeyDown(KeyEventArgs e)
        {
            float speed = 0.5f;

            if(e.KeyCode == Keys.W)
                direction += Vector3.NEGATIVE_UNIT_Z;

            if(e.KeyCode == Keys.A)
                direction += Vector3.NEGATIVE_UNIT_X;
            
            if(e.KeyCode == Keys.S)
                direction += Vector3.UNIT_Z;
            
            if(e.KeyCode == Keys.D)
                direction += Vector3.UNIT_X;

            direction = direction.NormalisedCopy * speed;
        }

        public override void MouseMove(int dx, int dy, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                cameraController.Rotate(dx, dy);

            base.MouseMove(dx, dy, e);
        }

        public override void MouseWheel(MouseEventArgs e)
        {
            cameraController.Move(0, 0, -e.Delta * 0.005f);
        }

        public override void Update()
        {
            cameraController.Move(direction.x, direction.y, direction.z);
        }
    }
}
