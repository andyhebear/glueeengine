using System;
using System.Collections.Generic;
using System.Text;
using GlueEditor.Core;
using Mogre;
using GlueEditor.ViewportControllers;
using GlueEngine.Core;
using System.IO;

namespace GlueEditor.WorldGeometry
{
    public class Block : ISelectableObject
    {
        private List<Vector3> vertices;
        private Dictionary<string, List<Surface>> surfacesMap;
        private List<Edge> edgeList = new List<Edge>();
        private Vector3 centre = Vector3.ZERO;
        private ColourValue colour = ColourValue.White;
        private ManualObject solidObject;
        private ManualObject wireObject;

        public AxisAlignedBox BoundingBox
        {
            get
            {
                return this.solidObject.BoundingBox;
            }
            set
            {
                this.Update(value);
            }
        }

        public Vector3[] Vertices
        {
            get
            {
                return this.vertices.ToArray();
            }
        }

        public string MaterialName
        {
            set
            {
                // this isn't right. need to make sure surfaces get moved around in the surface map
                foreach(string key in this.surfacesMap.Keys)
                {
                    foreach (Surface surface in surfacesMap[key])
                        surface.MaterialName = value;
                }

                for(uint i = 0; i < this.solidObject.NumSections; i++)
                    this.solidObject.SetMaterialName(i, value);
            }
        }

        /*
           1-----2  
          /|    /|
         / |   / |
        5-----4  |  
        |  0--|--3  
        | /   | /
        |/    |/
        6-----7     
        */
        public Block(AxisAlignedBox box)
        {
            this.surfacesMap = new Dictionary<string, List<Surface>>();
            this.vertices = new List<Vector3>(box.GetAllCorners());

            this.AddSurface(new Surface("floor", 5, 4, 2, 1)); // top
            this.AddSurface(new Surface("roof", 6, 0, 3, 7));  // bottom
            this.AddSurface(new Surface("wall", 0, 1, 2, 3));  // back
            this.AddSurface(new Surface("wall", 7, 4, 5, 6));  // front
            this.AddSurface(new Surface("wall", 3, 2, 4, 7));  // right
            this.AddSurface(new Surface("wall", 6, 5, 1, 0));  // left

            this.edgeList.Add(new Edge(0, 1));
            this.edgeList.Add(new Edge(1, 2));
            this.edgeList.Add(new Edge(2, 3));
            this.edgeList.Add(new Edge(3, 0));
            this.edgeList.Add(new Edge(5, 1));
            this.edgeList.Add(new Edge(4, 2));
            this.edgeList.Add(new Edge(7, 3));
            this.edgeList.Add(new Edge(6, 0));
            this.edgeList.Add(new Edge(5, 4));
            this.edgeList.Add(new Edge(4, 7));
            this.edgeList.Add(new Edge(7, 6));
            this.edgeList.Add(new Edge(6, 5));

            this.centre = box.Center;
        }

        public static Block Load(StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] bits = line.Split(':',',');
            Vector3 min = StringConverter.ParseVector3(bits[1]);
            Vector3 max = StringConverter.ParseVector3(bits[2]);
            AxisAlignedBox box = new AxisAlignedBox(min, max);
            return new Block(box);
        }

        public void Save(StreamWriter writer)
        {
            writer.Write("Block:");
            writer.Write(StringConverter.ToString(this.BoundingBox.Minimum));
            writer.Write(",");
            writer.Write(StringConverter.ToString(this.BoundingBox.Maximum));
            writer.WriteLine();
        }

        private void AddSurface(Surface surface)
        {
            if (this.surfacesMap.ContainsKey(surface.MaterialName))
            {
                this.surfacesMap[surface.MaterialName].Add(surface);
            }
            else
            {
                List<Surface> surfaces = new List<Surface>();
                surfaces.Add(surface);
                this.surfacesMap.Add(surface.MaterialName, surfaces);
            }
        }

        private void DrawSolid(bool update)
        {
            uint sectionIndex = 0;

            foreach (string materialName in this.surfacesMap.Keys)
            {
                if (update)
                    this.solidObject.BeginUpdate(sectionIndex);
                else
                    this.solidObject.Begin(materialName);

                foreach (Surface surface in this.surfacesMap[materialName])
                {
                    float width = (this.vertices[surface.Indices[2]] - this.vertices[surface.Indices[1]]).Length;
                    float height = (this.vertices[surface.Indices[1]] - this.vertices[surface.Indices[0]]).Length;
                    Vector3 normal = Mogre.Math.CalculateBasicFaceNormal(this.vertices[surface.Indices[0]], 
                        this.vertices[surface.Indices[1]], this.vertices[surface.Indices[3]]);
                    
                    foreach (Triangle triangle in surface.Triangles)
                    {
                        foreach (Vertex vertex in triangle.Vertices)
                        {
                            this.solidObject.Position(this.vertices[vertex.Index]);
                            this.solidObject.TextureCoord(vertex.TextureCoord.x * width, vertex.TextureCoord.y * height);
                            this.solidObject.Normal(normal);
                        }
                    }
                }

                this.solidObject.End();
                sectionIndex++;
            }
        }

        private void DrawWire(bool update)
        {
            if (update)
                this.wireObject.BeginUpdate(0);
            else
                this.wireObject.Begin("Editor/Block", RenderOperation.OperationTypes.OT_LINE_LIST);

            foreach (Edge edge in this.edgeList)
            {
                this.wireObject.Position(this.vertices[edge.Index0]);
                this.wireObject.Colour(colour);
                this.wireObject.Position(this.vertices[edge.Index1]);
                this.wireObject.Colour(colour);
            }

            // draw the little cross in the middle
            float size = 0.1f;
            this.wireObject.Position(centre - Vector3.UNIT_X * size);
            this.wireObject.Colour(colour);
            this.wireObject.Position(centre + Vector3.UNIT_X * size);
            this.wireObject.Colour(colour);

            this.wireObject.Position(centre - Vector3.UNIT_Y * size);
            this.wireObject.Colour(colour);
            this.wireObject.Position(centre + Vector3.UNIT_Y * size);
            this.wireObject.Colour(colour);

            this.wireObject.Position(centre - Vector3.UNIT_Z * size);
            this.wireObject.Colour(colour);
            this.wireObject.Position(centre + Vector3.UNIT_Z * size);
            this.wireObject.Colour(colour);

            this.wireObject.End();
        }

        public void Create()
        {
            // create the solid object
            this.solidObject = Engine.Graphics.SceneManager.CreateManualObject();
            this.solidObject.VisibilityFlags = ViewportController.VF_PERSPECTIVE;
            this.solidObject.UserObject = this;

            DrawSolid(false);

            // create the wireframe version
            float r = Mogre.Math.RangeRandom(0.5f, 1.0f);
            float g = Mogre.Math.RangeRandom(0.5f, 1.0f);
            float b = Mogre.Math.RangeRandom(0.5f, 1.0f);
            this.colour = new ColourValue(r, g, b);

            this.wireObject = Engine.Graphics.SceneManager.CreateManualObject();
            this.wireObject.VisibilityFlags = ViewportController.VF_ORTHOGRAPHIC;
            this.wireObject.UserObject = this;

            DrawWire(false);

            SceneNode sceneNode = Engine.Graphics.SceneManager.RootSceneNode.CreateChildSceneNode();
            sceneNode.AttachObject(solidObject);
            sceneNode.AttachObject(wireObject);
        }

        private void Update(AxisAlignedBox box)
        {
            this.vertices = new List<Vector3>(box.GetAllCorners());
            this.centre = box.Center;
            this.solidObject.BoundingBox = box;
            this.wireObject.BoundingBox = box;

            DrawSolid(true);
            DrawWire(true);
        }
        
        public Pair<bool, float> RayIntersects(Ray ray, IViewportController viewportController)
        {
            if (viewportController.Viewport.VisibilityMask == ViewportController.VF_PERSPECTIVE)
                return ray.Intersects(this.BoundingBox);
            else
                return ray.Intersects(new Sphere(this.BoundingBox.Center, 0.2f));
        }
    }
}
