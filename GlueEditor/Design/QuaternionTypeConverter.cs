using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.ComponentModel;

namespace GlueEditor.Design
{
    class QuaternionTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Quaternion))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Quaternion)
            {
                Quaternion v = (Quaternion)value;

                return Mogre.StringConverter.ToString(v);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Mogre.StringConverter.ParseQuaternion(value as string);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
