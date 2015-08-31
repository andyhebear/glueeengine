using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using GlueEngine.Core;

namespace GlueEngine.Design
{
    class MeshListTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] values = Engine.Graphics.GetMeshList();

            return new StandardValuesCollection(values);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
