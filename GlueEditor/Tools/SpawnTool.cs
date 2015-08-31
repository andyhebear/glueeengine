using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GlueEditor.ViewportControllers;
using GlueEngine.Core;
using Mogre;
using GlueEngine.World;
using GlueEngine.World.Entities;
using GlueEngine.World.Lights;
using GlueEditor.WorldGeometry;
using GlueEditor.Core;

namespace GlueEditor.Tools
{
    public partial class SpawnTool : UserControl, ITool
    {
        public SpawnTool()
        {
            InitializeComponent();
        }

        public new void MouseDown(IViewportController viewportController, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Vector3 position = viewportController.MouseToGrid(e.X, e.Y);

                if (this.lstEntityTypes.SelectedItem != null)
                {
                    string key = this.lstEntityTypes.SelectedItem.ToString();

                    Editor.CreateEditNode(key, position);
                }
            }
        }

        public new void MouseUp(IViewportController viewportController, MouseEventArgs e)
        {
        }

        public new void MouseMove(IViewportController viewportController, int dx, int dy, MouseEventArgs e)
        {
        }

        private void SpawnTool_Load(object sender, EventArgs e)
        {
            this.lstEntityTypes.Items.AddRange(Engine.World.WorldEntities);
        }

        private void lstEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEntityTypes.SelectedItem != null)
            {
                string key = lstEntityTypes.SelectedItem.ToString();

                if(Editor.PropertyGrid != null)
                    Editor.PropertyGrid.SelectedObject = Engine.World.GetWorldEntity(key);
            }
        }

        private void AddNewWorldEntity(string name, WorldEntity worldEntity)
        {
            worldEntity.NameChanged += new EventHandler(worldEntity_NameChanged);
            
            if (Engine.World.AddWorldEntity(name, worldEntity))
                this.lstEntityTypes.Items.Add(name);
        }

        void worldEntity_NameChanged(object sender, EventArgs e)
        {
            WorldEntity worldEntity = sender as WorldEntity;

            if (worldEntity != null)
            {
                this.lstEntityTypes.Items.Remove(worldEntity);
                this.lstEntityTypes.Items.Add(worldEntity);
            }
        }

        private void staticEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mesh = Engine.Graphics.GetMeshList()[0];
            string name = Engine.UniqueName("StaticEntity");
            AddNewWorldEntity(name, new StaticEntity(name, mesh));
        }

        private void dynamicEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mesh = Engine.Graphics.GetMeshList()[0];
            string name = Engine.UniqueName("DynamicEntity");
            AddNewWorldEntity(name, new DynamicEntity(name, mesh));
        }
    }
}
