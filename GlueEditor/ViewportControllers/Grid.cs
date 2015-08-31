using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEditor.Core;
using GlueEngine.Core;

namespace GlueEditor.ViewportControllers
{
    public class Grid
    {
        private ManualObject manualObject;
        private SceneNode sceneNode;
        private int columnCount;
        private int rowCount;
        private float unitSize;
        private Vector3 normal;

        public float UnitSize
        {
            get
            {
                return this.unitSize;
            }
            set
            {
                this.unitSize = value;
                this.Draw(true);
            }
        }

        public Vector3 Position
        {
            get
            {
                return sceneNode.Position;
            }
            set
            {
                sceneNode.Position = value;
            }
        }

        public bool Visible
        {
            get
            {
                return sceneNode.GetAttachedObject(0).Visible;
            }
            set
            {
                sceneNode.GetAttachedObject(0).Visible = value;
            }
        }

        public Vector3 Normal
        {
            get
            {
                return normal;
            }
            set
            {
                this.normal = value;
                sceneNode.Orientation = Quaternion.IDENTITY;
                sceneNode.Orientation = Vector3.UNIT_Z.GetRotationTo(normal);
            }
        }

        public Grid(int columnCount, int rowCount, float unitSize)
        {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            this.unitSize = unitSize;
        }

        private void Draw(bool update)
        {
            int largeGrid = 8;
            float width = (float)columnCount * unitSize;
            float depth = (float)rowCount * unitSize;
            Vector3 center = new Vector3(-width / 2.0f, -depth / 2.0f, 0);
            ColourValue colour;
            ColourValue colourLight = new ColourValue(0.5f, 0.5f, 0.5f, 0.2f);
            ColourValue colourDark = new ColourValue(1.0f, 1.0f, 1.0f, 0.3f);
            Vector3 start, end;

            if (update)
                manualObject.BeginUpdate(0);
            else
                manualObject.Begin("Editor/Grid", RenderOperation.OperationTypes.OT_LINE_LIST);

            for (int i = 0; i <= rowCount; i++)
            {
                start.x = 0.0f;
                start.y = i * unitSize;
                start.z = 0.0f;

                end.x = width;
                end.y = i * unitSize;
                end.z = 0.0f;

                if (i * unitSize % largeGrid != 0)
                    colour = colourLight;
                else
                    colour = colourDark;

                manualObject.Position(start + center);
                manualObject.Colour(colour);
                manualObject.Position(end + center);
                manualObject.Colour(colour);
            }

            for (int i = 0; i <= columnCount; i++)
            {
                start.x = i * unitSize;
                start.y = depth;
                start.z = 0.0f;

                end.x = i * unitSize;
                end.y = 0.0f;
                end.z = 0.0f;

                if (i * unitSize % largeGrid != 0)
                    colour = colourLight;
                else
                    colour = colourDark;

                manualObject.Position(start + center);
                manualObject.Colour(colour);
                manualObject.Position(end + center);
                manualObject.Colour(colour);
            }

            manualObject.End();
        }

        public void Create(string name, SceneManager sceneManager, Vector3 axis)
        {
            this.manualObject = sceneManager.CreateManualObject(Engine.UniqueName(name + "Grid"));
            this.manualObject.QueryFlags = 0; // exlcude from scene queries
            this.manualObject.RenderQueueGroup = (byte)Mogre.RenderQueueGroupID.RENDER_QUEUE_OVERLAY - 1;

            Draw(false);

            this.normal = axis;
            this.sceneNode = sceneManager.RootSceneNode.CreateChildSceneNode();
            this.sceneNode.AttachObject(this.manualObject);
            this.sceneNode.Orientation = Vector3.UNIT_Z.GetRotationTo(axis);
        }

        public void Move(float x, float y, float z)
        {
            sceneNode.Translate(x, y, z);
        }

        public Vector3 SnapTo(Vector3 vector3)
        {
            Vector3 v = new Vector3();

            if (normal == Vector3.UNIT_X)
                v.x = vector3.x;
            else
                v.x = (float)System.Math.Round(vector3.x / unitSize) * unitSize;

            if (normal == Vector3.UNIT_Y)
                v.y = vector3.y;
            else
                v.y = (float)System.Math.Round(vector3.y / unitSize) * unitSize;

            if (normal == Vector3.UNIT_Z)
                v.z = vector3.z;
            else
                v.z = (float)System.Math.Round(vector3.z / unitSize) * unitSize;

            return v;
        }
    }
}
