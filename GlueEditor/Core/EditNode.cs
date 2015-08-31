using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEngine.Core;
using GlueEngine.World;

namespace GlueEditor.Core
{
    public class EditNode : ISelectableObject
    {
        private string key;
        private WorldEntity worldEntity;
        private SceneNode sceneNode = null;

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public WorldEntity WorldEntity
        {
            get
            {
                return this.worldEntity;
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

        public EditNode(string key, WorldEntity worldEntity, SceneNode sceneNode)
        {
            this.key = key;
            this.worldEntity = worldEntity;
            this.sceneNode = sceneNode;
        }

        public void Rotate(Vector3 axis, Radian radian)
        {
            this.sceneNode.Rotate(axis, radian);
        }

        public AxisAlignedBox BoundingBox
        {
            get
            {
                MovableObject m = this.sceneNode.GetAttachedObject(0);
                return new AxisAlignedBox(m.BoundingBox.Minimum * this.sceneNode.GetScale() + this.sceneNode.Position, m.BoundingBox.Maximum * this.sceneNode.GetScale() + this.sceneNode.Position);
            }
            set
            {
                this.sceneNode.Position = value.Center;
                this.sceneNode.SetScale(value.Size);
            }
        }

        public Pair<bool, float> RayIntersects(Ray ray, ViewportControllers.IViewportController viewportController)
        {
            return ray.Intersects(this.BoundingBox);
        }
    }
}
