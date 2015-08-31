using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEditor.ViewportControllers;
using System.Windows.Forms;
using GlueEngine.Core;
using GlueEditor.WorldGeometry;

namespace GlueEditor.Core
{
    /*
       1-----2  
      /|    /|
     / |   / |
    5-----4  |  
    |  0--|--3  
    | /   | /
    |/    |/
    6-----7     
    */
    public class SelectionBox
    {
        private const int RB_TOP = 0;
        private const int RB_SIDE = 1;
        private const int RB_FRONT = 2;

        private int selectedGrip = ResizableBox.MG_NONE;
        private Vector3 mouseDownPos = Vector3.ZERO;
        private Vector3 min;
        private Vector3 max;
        private AxisAlignedBox box;
        private ResizableBox[] resizableBoxes = new ResizableBox[3];
        private ManualObject wireBox = null;
        private ISelectableObject selectedObject = null;
        private bool isCreated = false;

        public ISelectableObject SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                if (value != null && value != selectedObject)
                {
                    this.selectedObject = null;
                    this.min = value.BoundingBox.Minimum;
                    this.max = value.BoundingBox.Maximum;

                    if (this.isCreated == false)
                        this.Create();

                    this.Update();
                }

                this.selectedObject = value;

                if (this.selectedObject == null)
                    this.Destroy();
            }
        }

        public AxisAlignedBox Box
        {
            get
            {
                return this.box;
            }
        }
        
        public SelectionBox()
        {
            resizableBoxes[RB_TOP] = new ResizableBox(1, 2, 4, 5, ViewportController.VF_TOP);
            resizableBoxes[RB_SIDE] = new ResizableBox(4, 2, 3, 7, ViewportController.VF_SIDE);
            resizableBoxes[RB_FRONT] = new ResizableBox(5, 4, 7, 6, ViewportController.VF_FRONT);
        }

        private void Create()
        {
            if (!isCreated)
            {
                this.box = new AxisAlignedBox(Vector3.ZERO, Vector3.ZERO);

                foreach (ResizableBox resizableBox in this.resizableBoxes)
                    resizableBox.Create(box);

                UpdateWireBox();

                this.isCreated = true;
            }
        }

        private void Destroy()
        {
            if (isCreated)
            {
                this.box = null;

                foreach (ResizableBox resizableBox in this.resizableBoxes)
                    resizableBox.Destroy();

                if (this.wireBox != null)
                {
                    Engine.Graphics.SceneManager.DestroyManualObject(this.wireBox);
                    this.wireBox = null;
                }

                this.isCreated = false;
            }
        }

        private void UpdateWireBox()
        {
            if (this.wireBox == null)
            {
                this.wireBox = Engine.Graphics.SceneManager.CreateManualObject();
                this.wireBox.Dynamic = true;
                this.wireBox.QueryFlags = 0;
                this.wireBox.VisibilityFlags = ViewportController.VF_PERSPECTIVE;

                Engine.Graphics.SceneManager.RootSceneNode.AttachObject(wireBox);
                this.wireBox.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_LIST);
            }
            else
            {
                this.wireBox.BeginUpdate(0);
            }

            Vector3[] corners = box.GetAllCorners();
            this.wireBox.Position(corners[0]);  // back quad
            this.wireBox.Position(corners[1]);
            this.wireBox.Position(corners[1]);
            this.wireBox.Position(corners[2]);
            this.wireBox.Position(corners[2]);
            this.wireBox.Position(corners[3]);
            this.wireBox.Position(corners[3]);
            this.wireBox.Position(corners[0]);

            this.wireBox.Position(corners[0]);  // back to front lines
            this.wireBox.Position(corners[6]);
            this.wireBox.Position(corners[1]);
            this.wireBox.Position(corners[5]);
            this.wireBox.Position(corners[2]);
            this.wireBox.Position(corners[4]);
            this.wireBox.Position(corners[3]);
            this.wireBox.Position(corners[7]);

            this.wireBox.Position(corners[7]);  // front quad
            this.wireBox.Position(corners[6]);
            this.wireBox.Position(corners[6]);
            this.wireBox.Position(corners[5]);
            this.wireBox.Position(corners[5]);
            this.wireBox.Position(corners[4]);
            this.wireBox.Position(corners[4]);
            this.wireBox.Position(corners[7]); 
            this.wireBox.End();
        }

        private int GetSelectedGrip(IViewportController viewportController, int x, int y)
        {
            if (this.isCreated)
            {
                foreach (ResizableBox resizableBox in this.resizableBoxes)
                {
                    int grip = resizableBox.MouseOver(viewportController, x, y);

                    if (grip != ResizableBox.MG_NONE)
                        return grip;
                }
            }

            return ResizableBox.MG_NONE;
        }

        public bool MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mouseDownPos = viewportController.MouseToGrid(e.X, e.Y);
                this.selectedGrip = this.GetSelectedGrip(viewportController, e.X, e.Y);

                // create a new box
                if (this.selectedGrip == ResizableBox.MG_NONE)
                {
                    this.selectedObject = null;
                    this.min = mouseDownPos;
                    this.max = mouseDownPos;

                    if (this.isCreated)
                        this.Destroy();
                }
            }

            return this.selectedGrip != ResizableBox.MG_NONE;
        }

        public bool MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
            bool returnValue = this.selectedGrip != ResizableBox.MG_NONE;

            if (e.Button == MouseButtons.Left)
            {
                if(this.selectedGrip == ResizableBox.MG_NONE)
                {
                    // finish creating a new box
                    this.max = viewportController.MouseToGrid(e.X, e.Y);
                }

                if (min == max)
                    this.Destroy();

                if (min.x == max.x)
                    min.x = max.x + 1;

                if (min.y == max.y)
                    min.y = max.y + 1;
                
                if (min.z == max.z)
                    min.z = max.z + 1;

                // make sure min an max are around the right way
                Vector3 temp = Editor.MinOf(min, max);
                this.max = Editor.MaxOf(min, max);
                this.min = temp;

                this.Update();
                this.selectedGrip = ResizableBox.MG_NONE;
            }

            return returnValue;
        }

        public bool MouseMove(IViewportController viewportController, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.isCreated)
            {
                // update the visual feedback if we are not doing anything else
                if (this.selectedGrip == ResizableBox.MG_NONE)
                    GetSelectedGrip(viewportController, e.X, e.Y);

                if (e.Button == MouseButtons.Left)
                {
                    Vector3 mousePos = viewportController.MouseToGrid(e.X, e.Y);

                    switch (this.selectedGrip)
                    {
                        case ResizableBox.MG_NONE:      // creating a new box
                            this.max = mousePos;
                            break;
                        case ResizableBox.MG_MOVE:      // moving a box
                            Vector3 movement = mousePos - this.mouseDownPos;
                            this.min += movement;
                            this.max += movement;
                            this.mouseDownPos = mousePos;
                            break;
                        default:
                            this.Resize(mousePos, viewportController.Viewport.VisibilityMask);
                            break;
                    }

                    this.Update();
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left && this.selectedGrip == ResizableBox.MG_NONE)
                    this.Create();
            }

            return this.selectedGrip != ResizableBox.MG_NONE;
        }

        private void Resize(Vector3 mousePos, uint vfFlag)
        {
            switch (vfFlag)
            {
                case ViewportController.VF_TOP:
                    ResizeTop(mousePos);
                    break;
                case ViewportController.VF_SIDE:
                    ResizeSide(mousePos);
                    break;
                case ViewportController.VF_FRONT:
                    ResizeFront(mousePos);
                    break;
            }
        }

        private void ResizeFront(Vector3 mousePos)
        {
            switch (this.selectedGrip)
            {
                case ResizableBox.MG_RIGHT_MIDDLE:
                    this.max.x = mousePos.x;
                    break;
                case ResizableBox.MG_LEFT_MIDDLE:
                    this.min.x = mousePos.x;
                    break;
                case ResizableBox.MG_TOP_MIDDLE:
                    this.max.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_MIDDLE:
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_TOP_RIGHT:
                    this.max.x = mousePos.x;
                    this.max.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_RIGHT:
                    this.max.x = mousePos.x;
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_LEFT:
                    this.min.x = mousePos.x;
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_TOP_LEFT:
                    this.min.x = mousePos.x;
                    this.max.y = mousePos.y;
                    break;
            }
        }

        private void ResizeSide(Vector3 mousePos)
        {
            switch (this.selectedGrip)
            {
                case ResizableBox.MG_RIGHT_MIDDLE:
                    this.min.z = mousePos.z;
                    break;
                case ResizableBox.MG_LEFT_MIDDLE:
                    this.max.z = mousePos.z;
                    break;
                case ResizableBox.MG_TOP_MIDDLE:
                    this.max.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_MIDDLE:
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_TOP_RIGHT:
                    this.min.z = mousePos.z;
                    this.max.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_RIGHT:
                    this.min.z = mousePos.z;
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_BOTTOM_LEFT:
                    this.max.z = mousePos.z;
                    this.min.y = mousePos.y;
                    break;
                case ResizableBox.MG_TOP_LEFT:
                    this.max.z = mousePos.z;
                    this.max.y = mousePos.y;
                    break;
            }
        }

        private void ResizeTop(Vector3 mousePos)
        {
            switch (this.selectedGrip)
            {
                case ResizableBox.MG_RIGHT_MIDDLE:
                    this.max.x = mousePos.x;
                    break;
                case ResizableBox.MG_LEFT_MIDDLE:
                    this.min.x = mousePos.x;
                    break;
                case ResizableBox.MG_TOP_MIDDLE:
                    this.min.z = mousePos.z;
                    break;
                case ResizableBox.MG_BOTTOM_MIDDLE:
                    this.max.z = mousePos.z;
                    break;
                case ResizableBox.MG_TOP_RIGHT:
                    this.max.x = mousePos.x;
                    this.min.z = mousePos.z;
                    break;
                case ResizableBox.MG_BOTTOM_RIGHT:
                    this.max.x = mousePos.x;
                    this.max.z = mousePos.z;
                    break;
                case ResizableBox.MG_BOTTOM_LEFT:
                    this.min.x = mousePos.x;
                    this.max.z = mousePos.z;
                    break;
                case ResizableBox.MG_TOP_LEFT:
                    this.min.x = mousePos.x;
                    this.min.z = mousePos.z;
                    break;
            }
        }

        private void Update()
        {
            if (this.isCreated)
            {
                // recreate the box
                this.box = new AxisAlignedBox(this.min, this.max);

                // update the resizable boxes
                foreach (ResizableBox resizableBox in this.resizableBoxes)
                    resizableBox.Update(box);

                // redraw the perspective view box
                this.UpdateWireBox();

                // update the selected block
                if (this.selectedObject != null)
                    this.selectedObject.BoundingBox = this.box;
            }
        }
    }
}
