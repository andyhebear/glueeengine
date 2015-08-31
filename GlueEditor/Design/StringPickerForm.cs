using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GlueEditor.Design
{
    public partial class StringPickerForm : Form
    {
        public string SelectedItem
        {
            get
            {
                if(this.listBox.SelectedItem != null)
                    return this.listBox.SelectedItem.ToString();

                return "";
            }
        }

        public StringPickerForm(string[] stringArray)
        {
            InitializeComponent();

            this.listBox.Items.AddRange(stringArray);
        }
    }
}
