using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TerraRandom
{
    public class TileGenerator
    {
        private readonly int tileSize;
        private readonly double northWestValue;
        private readonly double northEastValue;
        private readonly double southEastValue;
        private readonly double southWestValue;
        private double[,] tileValues;
        private Random random;

        public TileGenerator(int tileSize, double northWestValue, double northEastValue, double southEastValue, double southWestValue)
        {
            this.tileSize = tileSize;
            this.northWestValue = northWestValue;
            this.northEastValue = northEastValue;
            this.southEastValue = southEastValue;
            this.southWestValue = southWestValue;

            if (tileSize%2 == 0)
                throw new Exception("Tile's must have odd sizes");

            random = new Random();
            
        }

        public void GenerateTile()
        {
            tileValues = new double[tileSize, tileSize];
            tileValues[0, 0] = northWestValue;
            tileValues[0, tileSize - 1] = northEastValue;
            tileValues[tileSize - 1, tileSize - 1] = southEastValue;
            tileValues[0, tileSize - 1] = southWestValue;

            var squares = new List<Square>() {new Square() {NorthWest = new Vertex() {X = 0, Y = 0}, Size = tileSize}};
            List<Diamond> diamonds;
            int iteration = 0;
            while (true)
            {
                diamonds = new List<Diamond>();

                foreach (var square in squares.Distinct().Where(s=>IsInRange(s.CenterVertex)))
                {
                    diamonds.AddRange(RunDiamondStep(square, iteration));
                }

                squares = new List<Square>();

                foreach (var diamond in diamonds.Distinct().Where(s => IsInRange(s.CenterVertex)))
                {
                    squares.AddRange(RunSquaresStep(diamond, iteration));
                }

                if (squares[0].Size == 2)
                    break;

                iteration++;
            }

            using (Bitmap b = new Bitmap(tileSize, tileSize))
            {
                for (int i = 0; i < tileValues.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < tileValues.GetUpperBound(1); j++)
                    {
                        b.SetPixel(i,j, Color.FromArgb((int)(tileValues[i,j]*32), Color.Black));
                    }
                }


                b.Save(@"C:\output.png", ImageFormat.Png);
            }
        }

        private IEnumerable<Square> RunSquaresStep(Diamond diamond, int iteration)
        {
            double average = AverageOfInRangeVertices(diamond, iteration);
            var centerVertex = diamond.CenterVertex;

            if (IsInRange(centerVertex))
                tileValues[centerVertex.X, centerVertex.Y] = average;

            var resultantSquareSize = diamond.Size/2 + 1;

            List<Square> squares = new List<Square>();

            squares.Add(new Square() { NorthWest = new Vertex() {X=diamond.West.X,Y=diamond.North.Y}, Size  = resultantSquareSize});
            squares.Add(new Square() { NorthWest = diamond.West, Size = resultantSquareSize});
            squares.Add(new Square() { NorthWest = diamond.North, Size = resultantSquareSize});
            squares.Add(new Square() { NorthWest = diamond.CenterVertex, Size = resultantSquareSize});

            return squares;
        }

        private IEnumerable<Diamond> RunDiamondStep(Square square, int iteration)
        {
            double average = AverageOfInRangeVertices(square, iteration);
            var centerVertex = square.CenterVertex;

            if(IsInRange(centerVertex))
                tileValues[centerVertex.X, centerVertex.Y] = average;

            List<Diamond> diamonds = new List<Diamond>();

            diamonds.Add(new Diamond() { West = square.NorthWest, Size = square.Size});
            diamonds.Add(new Diamond() { West = centerVertex, Size = square.Size});
            diamonds.Add(new Diamond() { West = square.SouthWest, Size = square.Size });
            diamonds.Add(new Diamond() { West = new Vertex() { X = centerVertex.X - (square.Size-1), Y = centerVertex.Y}, Size = square.Size});

            return diamonds;
        }

        private double AverageOfInRangeVertices(IShape shape, int iteration)
        {
            List<double> values = new List<double>();

            foreach (var vertex in shape.GetAllCorners())
            {
                if (IsInRange(vertex))
                {
                    values.Add(tileValues[vertex.X, vertex.Y]);
                } 
            }

            if (values.Any())
            {
                var retVal = values.Average() + ((random.NextDouble()-0.5)/10*(1 - (iteration/10)));
                if (retVal < 0)
                    return 0;
                if (retVal > 1)
                    return 1;
                return retVal;
            }
            return 0;
        }

        private bool IsInRange(Vertex vertex)
        {
            return vertex.X >= 0 && vertex.Y >= 0 && vertex.X < tileSize && vertex.Y < tileSize;
        }
    }
}
