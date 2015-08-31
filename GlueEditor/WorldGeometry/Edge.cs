using System;
using System.Collections.Generic;
using System.Text;

namespace GlueEditor.WorldGeometry
{
    public class Edge
    {
        private int index0;
        private int index1;

        public int Index0        
        {
            get
            {
                return this.index0;
            }
        }

        public int Index1
        {
            get
            {
                return this.index1;
            }
        }

        public Edge(int index0, int index1)
        {
            this.index0 = index0;
            this.index1 = index1;
        }
    }
}
