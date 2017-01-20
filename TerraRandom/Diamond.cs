using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraRandom
{
    [DebuggerDisplay("Diamond Western ({West.X},{West.Y})")]
    class Diamond : IShape
    {
        public Vertex West { get; set; }

        public Vertex North => new Vertex() {X = West.X + Size/2,Y = West.Y - Size/2};
        public int Size { get; set; }

        public Vertex CenterVertex => new Vertex() {X = West.X + Size/2, Y = West.Y};
        public Vertex[] GetAllCorners()
        {
            var halfsize = Size / 2;
            var vertices = new Vertex[4];

            vertices[0] = West;
            vertices[1] = new Vertex() {X = West.X + Size - 1, Y = West.Y};
            vertices[2] = North;
            vertices[3] = new Vertex() { X = North.X, Y = West.Y + halfsize };

            return vertices;
        }
    }
}
