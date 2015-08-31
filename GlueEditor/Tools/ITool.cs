using System;
using System.Collections.Generic;
using System.Text;
using GlueEditor.ViewportControllers;
using System.Windows.Forms;

namespace GlueEditor.Tools
{
    public interface ITool
    {
        void MouseDown(IViewportController viewportController, MouseEventArgs e);
        void MouseUp(IViewportController viewportController, MouseEventArgs e);
        void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e);
    }
}
