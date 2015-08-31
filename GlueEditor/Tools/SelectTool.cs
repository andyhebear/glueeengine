using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GlueEditor.ViewportControllers;
using Mogre;
using GlueEngine.Core;
using GlueEditor.Core;
using GlueEditor.WorldGeometry;
using GlueEngine.World;

namespace GlueEditor.Tools
{
    public partial class SelectTool : UserControl, ITool
    {
        public SelectTool()
        {
            InitializeComponent();
        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (Editor.SelectionBox.SelectedObject != null)
                Editor.SelectionBox.MouseDown(viewportController, e);
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
            if (Editor.SelectionBox.SelectedObject != null)
                Editor.SelectionBox.MouseUp(viewportController, e);
            else
            {
                Ray ray = viewportController.CreateViewportRay(e.X, e.Y);
                ISelectableObject selectedObject = null;
                float d = float.MaxValue;

                foreach (ISelectableObject selectable in Editor.SelectableObjects)
                {
                    Pair<bool, float> pair = selectable.RayIntersects(ray, viewportController);

                    if (pair.first && pair.second < d)
                    {
                        d = pair.second;
                        selectedObject = selectable;
                    }
                }

                Editor.SelectionBox.SelectedObject = selectedObject;
            }
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
            Editor.SelectionBox.MouseMove(viewportController, e);
        }
    }
}
