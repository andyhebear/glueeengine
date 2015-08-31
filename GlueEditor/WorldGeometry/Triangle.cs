using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEditor.WorldGeometry
{
    public class Triangle
    {
        private Vertex[] vertices = new Vertex[3];

        public Vertex[] Vertices
        {
            get
            {
                return this.vertices;
            }
        }

        public Triangle(Vertex vertex0, Vertex vertex1, Vertex vertex2)
        {
            this.vertices[0] = vertex0;
            this.vertices[1] = vertex1;
            this.vertices[2] = vertex2;
        }
    }
}
