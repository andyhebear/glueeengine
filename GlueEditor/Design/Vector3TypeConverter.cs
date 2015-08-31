using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Mogre;

namespace GlueEditor.Design
{
    class Vector3TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                float unit;
                
                if (float.TryParse(value.ToString(), out unit))
                    return Vector3.UNIT_SCALE * unit;
                else                
                    return Mogre.StringConverter.ParseVector3(value.ToString());
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Vector3 vector3 = (Vector3)value;
                return Mogre.StringConverter.ToString(vector3);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
