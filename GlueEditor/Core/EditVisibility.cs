using System;

namespace GlueEditor.Geometry
{
    [Flags]
    public enum EditVisibility : uint
    {
        Perspective = 1,
        Top = 2,
        Side = 4,
        Front = 8,
        Orthographic = Top | Side | Front
    }
}