using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using GlueEditor.ViewportControllers;

namespace GlueEditor.Core
{
    public interface ISelectableObject
    {
        AxisAlignedBox BoundingBox
        {
            get;
            set;
        }

        Pair<bool, float> RayIntersects(Ray ray, IViewportController viewportController);
    }
}
