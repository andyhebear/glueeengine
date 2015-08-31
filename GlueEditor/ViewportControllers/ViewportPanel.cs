using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GlueEditor.ViewportControllers
{
    public partial class ViewportPanel : UserControl
    {
        private Point prevPos = new Point();
        private IViewportController controller;

        public IViewportController ViewportController
        {
            get
            {
                return this.controller;
            }
        }

        public ViewportPanel()
        {
            InitializeComponent();
        }

        public void Initialise(IViewportController controller)
        {
            this.controller = controller;
            this.controller.Initialise(this.Name, this.Handle);
            this.controller.CursorChanged += new CursorEventHandler(controller_CursorChanged);
        }

        void controller_CursorChanged(Cursor cursor)
        {
            if(this.Cursor != cursor)
                this.Cursor = cursor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if(controller != null)
                controller.SizeChanged();

            base.OnSizeChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (controller != null)
                controller.KeyDown(e);

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (controller != null)
                controller.KeyUp(e);

            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (controller != null)
                controller.MouseDown(e);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (controller != null)
                controller.MouseUp(e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.Focused)
                this.Focus();

            Point newPos = new Point(e.X, e.Y);

            if (newPos != prevPos)
            {
                int dx = newPos.X - prevPos.X;
                int dy = newPos.Y - prevPos.Y;

                if (controller != null)
                    controller.MouseMove(dx, dy, e);
            }

            prevPos = newPos;

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (controller != null)
                controller.MouseWheel(e);

            base.OnMouseWheel(e);
        }
    }
}
