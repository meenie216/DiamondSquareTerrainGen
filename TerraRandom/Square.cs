using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraRandom
{
    [DebuggerDisplay("Square North Western ({NorthWest.X}, {NorthWest.Y})")]

    public class Square : IShape, IEqualityComparer<Square>
    {
        public Vertex NorthWest { get; set; }
        public int Size { get; set; }

        public Vertex SouthWest => new Vertex() {X = NorthWest.X, Y = NorthWest.Y + Size - 1};

        public Vertex[] GetAllCorners()
        {
            var vertices = new Vertex[4];

            vertices[0] = NorthWest;
            vertices[1] = new Vertex() {X = NorthWest.X + Size - 1, Y = NorthWest.Y };
            vertices[2] = new Vertex() { X = NorthWest.X + Size - 1, Y = NorthWest.Y + Size - 1 };
            vertices[3] = new Vertex() { X = NorthWest.X, Y = NorthWest.Y + Size - 1 };

            return vertices;
        }

        public Vertex CenterVertex
        {
            get
            {
                var halfsize = Size/2;

                return new Vertex() {X = NorthWest.X + halfsize, Y = NorthWest.Y + halfsize};
            }
        }

        public bool Equals(Square x, Square y)
        {
            return (x.Size == y.Size) && (x.NorthWest == y.NorthWest);
        }

        public int GetHashCode(Square obj)
        {
            return 0;
        }
    }
}
