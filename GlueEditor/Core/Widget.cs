using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEditor.ViewportControllers;
using GlueEngine.Core;

namespace GlueEditor.Core
{
    public class Widget
    {
        private Entity entity;
        private SceneNode sceneNode;
        private Vector3 grabPosition = Vector3.ZERO;
        private Vector3 moveAxis = Vector3.ZERO;
        private IWidgetNode node = null;
        private int mx, my;

        public bool IsGrabbed
        {
            get
            {
                return moveAxis != Vector3.ZERO;
            }
        }

        public IWidgetNode Node
        {
            get
            {
                return this.node;
            }
            set
            {
                this.node = value;

                if (node != null)
                    this.Position = node.Position;
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.sceneNode.Position;
            }
            set
            {
                this.sceneNode.Position = value;
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return this.sceneNode.Orientation;
            }
            set
            {
                this.sceneNode.Orientation = value;
            }
        }

        public Vector3 MoveAxis
        {
            get
            {
                return this.moveAxis;
            }
        }

        public bool Visible
        {
            get
            {
                return this.entity.Visible;
            }
            set
            {
                this.entity.Visible = value;
            }
        }

        public Widget()
        {
        }

        public void Create()
        {
            this.entity = Engine.Graphics.SceneManager.CreateEntity("Widget_entAxes", "axes.mesh");
            this.entity.QueryFlags = 0; // exclude from scene queries
            this.sceneNode = Engine.Graphics.SceneManager.RootSceneNode.CreateChildSceneNode();
            this.sceneNode.AttachObject(this.entity);
            this.sceneNode.SetScale(0.1f, 0.1f, 0.1f);
            this.sceneNode.Position = new Vector3(0, 0, 0);

            this.entity.Visible = false;
        }

        private bool CloseTo(float a, float b, float range)
        {
            if (a > b - range && a < b + range)
                return true;

            return false;
        }

        private Vector3 MouseTo(IViewportController viewportController, int x, int y)
        {
            // determine the active plane            
            Plane activePlane = viewportController.CameraController.GetActivePlane(moveAxis, this.Position);

            // raycast onto the active plane
            RaycastResult result = viewportController.MouseToPlane(x, y, activePlane);

            if (result != null)
                return result.Position;

            return Vector3.ZERO;
        }

        public Vector3 MouseOverAxis(IViewportController viewportController, int x, int y)
        {
            Ray ray = viewportController.CreateViewportRay(x, y);

            float th = 0.1f;        // thickness
            float len = 1.1f;       // length
            AxisAlignedBox xBox = CreateOffsetBox(new Vector3(0.0f, -th, -th), new Vector3(len, th, th));
            AxisAlignedBox yBox = CreateOffsetBox(new Vector3(-th, 0.0f, -th), new Vector3(th, len, th));
            AxisAlignedBox zBox = CreateOffsetBox(new Vector3(-th, -th, 0.0f), new Vector3(th, th, len));

            if (ray.Intersects(xBox).first && !viewportController.CameraController.IsLookingDownAxis(Vector3.UNIT_X))
                return Vector3.UNIT_X;

            if (ray.Intersects(yBox).first && !viewportController.CameraController.IsLookingDownAxis(Vector3.UNIT_Y))
                return Vector3.UNIT_Y;

            if (ray.Intersects(zBox).first && !viewportController.CameraController.IsLookingDownAxis(Vector3.UNIT_Z))
                return Vector3.UNIT_Z;

            return Vector3.ZERO;
        }

        private AxisAlignedBox CreateOffsetBox(Vector3 min, Vector3 max)
        {
            min += this.Position;
            max += this.Position;
            return new AxisAlignedBox(min, max);
        }

        public bool Grab(IViewportController viewportController, int x, int y)
        {
            this.mx = x;
            this.my = y;

            this.moveAxis = MouseOverAxis(viewportController, x, y);

            if (moveAxis != Vector3.ZERO)
            {
                grabPosition = MouseTo(viewportController, x, y) - this.Position;
                return true;                
            }

            return false;
        }

        public void MoveTo(IViewportController viewportController, int x, int y)
        {
            if (grabPosition != Vector3.ZERO)
            {
                Vector3 p = MouseTo(viewportController, x, y);
                Vector3 newPos = p - grabPosition;
                
                this.Position = Editor.SnapTo((newPos * moveAxis) + (this.Position * (1 - moveAxis)));

                if (this.node != null)
                    this.node.Position = this.Position;
            }
        }

        internal void Rotate(int x, int y)
        {
            // work in progress
            if (grabPosition != Vector3.ZERO)
            {
                float delta;

                if (moveAxis == Vector3.UNIT_Y)
                    delta = (mx - x) * -0.01f;
                else
                    delta = (my - y) * 0.01f;

                if (this.node != null)
                {
                    this.node.Rotate(moveAxis, new Radian(delta), Mogre.Node.TransformSpace.TS_WORLD);
                }

                this.mx = x;
                this.my = y;
            }
        }

        internal void ResetOrientation()
        {
            if (this.node != null)
                this.node.Orientation = Quaternion.IDENTITY;
        }
    }
}
