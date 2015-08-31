using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Mogre;

namespace GlueEditor.Design
{
    class AxisAlignedBoxTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(AxisAlignedBox))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is AxisAlignedBox)
            {
                AxisAlignedBox v = (AxisAlignedBox)value;

                string str = v.ToString();

                str = str.Replace("AxisAlignedBox(", "");
                str = str.Replace("))", ")");
                return str;
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
                Vector3 minV;
                Vector3 maxV;
                if (GetMinMaxVector3FromString(value as string, out minV, out maxV))
                {
                    return new AxisAlignedBox(minV, maxV);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        bool GetMinMaxVector3FromString(string aabStr, out Vector3 minV, out Vector3 maxV)
        {
            //AxisAlignedBox(min=Vector3(-1, -1, -1), max=Vector3(1, 1, 1))

            minV = Vector3.ZERO;
            maxV = Vector3.ZERO;

            aabStr = aabStr.Replace(" ", "");
            aabStr = aabStr.Replace("min=Vector3(", "");
            aabStr = aabStr.Replace("max=Vector3(", "");
            aabStr.Substring(0, aabStr.Length - 1);
            aabStr = aabStr.Replace(")", "");
            string[] vectStr = aabStr.Split(new string[] { "," }, StringSplitOptions.None);
            if (vectStr.Length != 6) return false;

            float vectComp;
            for (int i = 0; i < 6; i++)
            {
                if (!float.TryParse(vectStr[i], out vectComp))
                    return false;
            }

            minV = new Vector3(float.Parse(vectStr[0]), float.Parse(vectStr[1]), float.Parse(vectStr[2]));
            maxV = new Vector3(float.Parse(vectStr[3]), float.Parse(vectStr[4]), float.Parse(vectStr[5]));

            return true;
        }
    }
}
