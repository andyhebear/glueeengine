using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using GlueEngine.Core;
using System.Windows.Forms;

namespace GlueEditor.Design
{
    class MeshTypeEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is string)
            {
                StringPickerForm form = new StringPickerForm(Engine.Graphics.GetMeshList());

                if (form.ShowDialog() == DialogResult.OK)
                    return form.SelectedItem;
            }

            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;            
        }
    }
}
