using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Xml;
using GlueEditor.Core;

namespace GlueEditor.Core
{
    public interface IWidgetNode
    {
        Vector3 Position
        {
            get;
            set;
        }

        Quaternion Orientation
        {
            get;
            set;
        }

        bool Selected
        {
            get;
            set;
        }

        void Rotate(Vector3 moveAxis, Radian radian, Node.TransformSpace transformSpace);
    }
}
