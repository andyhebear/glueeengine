using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Mogre;

namespace GlueEditor.Design
{
    public static class DesignSupportManager
    {
        public static bool Initialiase()
        {
            TypeDescriptor.AddProvider(new GlueTypeProvider(new Vector2TypeConverter()), typeof(Vector2));
            TypeDescriptor.AddProvider(new GlueTypeProvider(new Vector3TypeConverter()), typeof(Vector3));
            TypeDescriptor.AddProvider(new GlueTypeProvider(new QuaternionTypeConverter()), typeof(Quaternion));
            TypeDescriptor.AddProvider(new GlueTypeProvider(new ColourValueTypeConverter()), typeof(ColourValue));
            TypeDescriptor.AddProvider(new GlueTypeProvider(new AxisAlignedBoxTypeConverter()), typeof(AxisAlignedBox));
            return true;
        }
    }
}
