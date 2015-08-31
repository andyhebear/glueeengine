using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GlueEditor.Design
{
    class GlueTypeProvider : TypeDescriptionProvider
    {
        private TypeConverter typeConverter = null;
        private ICustomTypeDescriptor typeDescriptor = null;

        public GlueTypeProvider(TypeConverter typeConverter)
        {
            this.typeConverter = typeConverter;
            this.typeDescriptor = new GlueTypeDescriptor(typeConverter);
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if(typeDescriptor != null)
                return this.typeDescriptor;
            
            return base.GetTypeDescriptor(objectType, instance);            
        }
    }
}
