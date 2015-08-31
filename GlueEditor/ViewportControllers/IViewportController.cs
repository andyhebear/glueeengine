using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mogre;
using GlueEditor.Core;

namespace GlueEditor.ViewportControllers
{
    public delegate void CursorEventHandler(Cursor cursor);

    public interface IViewportController
    {
        RenderWindow RenderWindow
        {
            get;
        }

        CameraController CameraController
        {
            get;
        }

        Cursor Cursor
        {
            set;
        }

        Viewport Viewport
        {
            get;
        }

        Grid Grid
        {
            get;
        }

        event CursorEventHandler CursorChanged;

        void Initialise(string name, IntPtr handle);
        void SizeChanged();
        void KeyUp(KeyEventArgs e);
        void KeyDown(KeyEventArgs e);
        void MouseDown(MouseEventArgs e);
        void MouseUp(MouseEventArgs e);
        void MouseMove(int dx, int dy, MouseEventArgs e);
        void MouseWheel(MouseEventArgs e);
        void Update();
        Ray CreateViewportRay(float x, float y);
        Vector3 MouseToGrid(int x, int y);
        Vector3 MouseToGrid(int x, int y, bool snapToGrid);
        Vector2 GetScreenPosition(Vector3 position);
        RaycastResult MouseToPlane(int x, int y, Plane plane);

        void CreateScene();
    }
}
