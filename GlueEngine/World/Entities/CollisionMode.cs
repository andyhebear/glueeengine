using System;
using System.Collections.Generic;
using System.Text;

namespace GlueEngine.World.Entities
{
    public enum CollisionMode
    {
        None,
        Shapes,
        TriangleMesh,
        ConvexHull,
        BoundingBox,
        BoundingSphere
    }
}
