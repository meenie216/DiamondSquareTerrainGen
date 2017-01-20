using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraRandom
{
    [DebuggerDisplay("Vertex ({X},{Y})")]
    public class Vertex: IEqualityComparer<Vertex>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Equals(Vertex x, Vertex y)
        {
            return (x.X == y.X) && (x.Y == y.Y);
        }

        public int GetHashCode(Vertex obj)
        {
            return 0;
        }
    }
}
