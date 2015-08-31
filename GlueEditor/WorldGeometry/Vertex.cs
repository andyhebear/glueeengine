using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

namespace GlueEditor.WorldGeometry
{
    public class Vertex
    {
        private int index;
        private Vector2 textureCoord;

        public int Index
        {
            get
            {
                return index;
            }
        }

        public Vector2 TextureCoord
        {
            get
            {
                return textureCoord;
            }
        }

        public Vertex(int index, Vector2 textureCoord)
        {
            this.index = index;
            this.textureCoord = textureCoord;
        }

        public Vertex(int index, float u, float v)
        {
            this.index = index;
            this.textureCoord = new Vector2(u, v);
        }
    }
}
