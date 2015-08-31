using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEditor.WorldGeometry
{
    public class Surface
    {
        private string materialName;
        private int[] indices;
        private List<Vertex> vertices = new List<Vertex>();
        private List<Triangle> triangles = new List<Triangle>();

        public string MaterialName
        {
            get
            {
                return this.materialName;
            }
            set
            {
                this.materialName = value;
            }
        }

        public int[] Indices
        {
            get
            {
                return this.indices;
            }
        }

        public Triangle[] Triangles
        { 
            get
            {
                return this.triangles.ToArray();
            }            
        }

        //public Surface(string materialName, int[] indices)
        //{
        //    this.materialName = materialName;
        //    this.triangles = new List<Triangle>();
        //}

        public Surface(string materialName, int index0, int index1, int index2, int index3)
        {
            this.materialName = materialName;
            this.vertices = new List<Vertex>();
            this.indices = new int[4];

            this.indices[0] = index0;
            this.indices[1] = index1;
            this.indices[2] = index2;
            this.indices[3] = index3;

            this.vertices.Add(new Vertex(index0, 0.25f, 0.25f));
            this.vertices.Add(new Vertex(index1, 0.25f, 0));
            this.vertices.Add(new Vertex(index2, 0, 0));
            this.vertices.Add(new Vertex(index3, 0, 0.25f));

            this.triangles.Add(new Triangle(this.vertices[0], this.vertices[1], this.vertices[2]));
            this.triangles.Add(new Triangle(this.vertices[0], this.vertices[2], this.vertices[3]));
        }
    }
}
