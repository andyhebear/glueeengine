using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.Core;
using GlueEditor.ViewportControllers;
using System.Windows.Forms;

namespace GlueEditor.Core
{
    public class ResizableBox
    {
        public const int MG_NONE = -1;
        public const int MG_TOP_LEFT = 0;
        public const int MG_TOP_RIGHT = 1;
        public const int MG_BOTTOM_RIGHT = 2;
        public const int MG_BOTTOM_LEFT = 3;
        public const int MG_TOP_MIDDLE = 4;
        public const int MG_RIGHT_MIDDLE = 5;
        public const int MG_BOTTOM_MIDDLE = 6;
        public const int MG_LEFT_MIDDLE = 7;
        public const int MG_MOVE = 255;

        private ManualObject wireBox;
        private Vector3[] corners;
        private Vector3[] mouseGrips;
        private AxisAlignedBox box;
        private int[] indices;
        private uint vfFlag;

        public ResizableBox(int index0, int index1, int index2, int index3, uint vfFlag)
        {
            this.indices = new int[4];
            this.indices[0] = index0;
            this.indices[1] = index1;
            this.indices[2] = index2;
            this.indices[3] = index3;
            this.vfFlag = vfFlag;
            this.mouseGrips = new Vector3[8];
        }

        private int MouseOverGripIndex(IViewportController viewportController, int x, int y)
        {
            int mouseGripIndex = MG_NONE;

            // the the side grips
            if (viewportController.Viewport.VisibilityMask == vfFlag)
            {
                for (int i = 0; i < this.mouseGrips.Length; i++)
                {
                    Vector3 mouseGrip = mouseGrips[i];
                    Vector2 screenPos = viewportController.GetScreenPosition(mouseGrip);
                    int gripSize = 4;

                    if (x >= screenPos.x - gripSize && x <= screenPos.x + gripSize && y >= screenPos.y - gripSize && y <= screenPos.y + gripSize)
                        mouseGripIndex = i;
                }
            }

            // test for special move grip            
            if (mouseGripIndex == MG_NONE)
            {
                Vector2 topLeft = viewportController.GetScreenPosition(this.corners[this.indices[0]]);
                Vector2 bottomRight = viewportController.GetScreenPosition(this.corners[this.indices[2]]);

                if (x > topLeft.x && x < bottomRight.x && y > topLeft.y && y < bottomRight.y)
                    return MG_MOVE;
            }

            return mouseGripIndex;
        }

        private void UpdateMouseGrips()
        {
            this.mouseGrips[MG_TOP_LEFT] = this.corners[indices[0]];
            this.mouseGrips[MG_TOP_RIGHT] = this.corners[indices[1]];
            this.mouseGrips[MG_BOTTOM_RIGHT] = this.corners[indices[2]];
            this.mouseGrips[MG_BOTTOM_LEFT] = this.corners[indices[3]];

            this.mouseGrips[MG_TOP_MIDDLE] = this.corners[indices[0]] + (this.corners[indices[1]] - this.corners[indices[0]]) * 0.5f;
            this.mouseGrips[MG_RIGHT_MIDDLE] = this.corners[indices[1]] + (this.corners[indices[2]] - this.corners[indices[1]]) * 0.5f;
            this.mouseGrips[MG_BOTTOM_MIDDLE] = this.corners[indices[2]] + (this.corners[indices[3]] - this.corners[indices[2]]) * 0.5f;
            this.mouseGrips[MG_LEFT_MIDDLE] = this.corners[indices[3]] + (this.corners[indices[0]] - this.corners[indices[3]]) * 0.5f;
        }

        private void Draw(bool update)
        {            
            this.corners = box.GetAllCorners();
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
            // draw the wire box
            string material = "Editor/SelectionBox";
            ColourValue colour = ColourValue.White;

            // draw the outline
            if (update)
                this.wireBox.BeginUpdate(0);
            else
                this.wireBox.Begin(material, RenderOperation.OperationTypes.OT_LINE_LIST);

            DrawLine(this.wireBox, indices[0], indices[1], colour);
            DrawLine(this.wireBox, indices[1], indices[2], colour);
            DrawLine(this.wireBox, indices[2], indices[3], colour);
            DrawLine(this.wireBox, indices[3], indices[0], colour);
            this.wireBox.End();

            // draw the mouse grips
            UpdateMouseGrips();

            if (update)
                this.wireBox.BeginUpdate(1);
            else
                this.wireBox.Begin(material, RenderOperation.OperationTypes.OT_POINT_LIST);

            foreach (Vector3 grip in mouseGrips)
                this.wireBox.Position(grip);

            this.wireBox.End();
        }

        private void DrawLine(ManualObject manualObject, int index0, int index1, ColourValue colour)
        {
            Vector3 p0 = corners[index0];
            Vector3 p1 = corners[index1];            
            float length = (p1 - p0).Length;

            manualObject.Position(p0);
            manualObject.Colour(colour);
            manualObject.TextureCoord(0);

            manualObject.Position(p1);
            manualObject.Colour(colour);
            manualObject.TextureCoord(length);
        }
        
        private ManualObject CreateViewportManualObject(uint vfFlag)
        {
            ManualObject manualObject = Engine.Graphics.SceneManager.CreateManualObject();
            manualObject.Dynamic = true;
            manualObject.QueryFlags = 0;
            manualObject.VisibilityFlags = vfFlag;
            return manualObject;
        }

        private void SetupBox(AxisAlignedBox box)
        {
            Vector3 min = Editor.MinOf(box.Minimum, box.Maximum);
            Vector3 max = Editor.MaxOf(box.Minimum, box.Maximum);
                 
            this.box = new AxisAlignedBox(min, max);
        }

        public void Update(AxisAlignedBox box)
        {
            SetupBox(box);
            Draw(true);
        }

        public void Create(AxisAlignedBox box)
        {
            this.wireBox = this.CreateViewportManualObject(vfFlag);

            SetupBox(box);
            Draw(false);

            Engine.Graphics.SceneManager.RootSceneNode.AttachObject(this.wireBox);
        }

        public void Destroy()
        {
            if(this.wireBox != null)
                Engine.Graphics.SceneManager.DestroyManualObject(this.wireBox);
        }

        public int MouseOver(IViewportController viewportController, int x, int y)
        {
            if (viewportController.Viewport.VisibilityMask != this.vfFlag)
                return MG_NONE;

            int mouseGripIndex = this.MouseOverGripIndex(viewportController, x, y);

            switch (mouseGripIndex)
            {
                case ResizableBox.MG_TOP_LEFT:
                case ResizableBox.MG_BOTTOM_RIGHT:
                    viewportController.Cursor = Cursors.SizeNWSE;
                    break;
                case ResizableBox.MG_TOP_RIGHT:
                case ResizableBox.MG_BOTTOM_LEFT:
                    viewportController.Cursor = Cursors.SizeNESW;
                    break;
                case ResizableBox.MG_TOP_MIDDLE:
                case ResizableBox.MG_BOTTOM_MIDDLE:
                    viewportController.Cursor = Cursors.SizeNS;
                    break;
                case ResizableBox.MG_LEFT_MIDDLE:
                case ResizableBox.MG_RIGHT_MIDDLE:
                    viewportController.Cursor = Cursors.SizeWE;
                    break;
                case ResizableBox.MG_MOVE:
                    viewportController.Cursor = Cursors.SizeAll;
                    break;
                default:
                    viewportController.Cursor = Cursors.Default;
                    break;
            }

            return mouseGripIndex;
        }
    }
}
