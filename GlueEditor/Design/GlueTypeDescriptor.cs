using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GlueEditor.Design
{
    class GlueTypeDescriptor : CustomTypeDescriptor
    {
        TypeConverter typeConverter = null;

        public override TypeConverter GetConverter()
        {
            if (typeConverter != null)
                return typeConverter;

            return base.GetConverter();
        }

        public override object GetEditor(Type editorBaseType)
        {
            return new MeshTypeEditor();
            //return base.GetEditor(editorBaseType);
        }

        public GlueTypeDescriptor(TypeConverter typeConverter)
        {
            this.typeConverter = typeConverter;
        }
    }
}
