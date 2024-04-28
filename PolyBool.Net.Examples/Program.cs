using System;
using PolyBool.Net.Logic;
using PolyBool.Net.Objects;

namespace PolyBool.Net.Examples
{
    internal class Program
    {
        private static void Main()
        {

            var p1 = new Polygon
            {
                Regions = [
                    Region.New( [
                            new Point(0, 0),
                            new Point(16, 0),
                            new Point(8, 8) ]
                        )
                    
                ]
            };
            var p2 = new Polygon
            {
                Regions = [
                    Region.New ( [
                            new Point(16, 6),
                            new Point(8, 14),
                            new Point(0, 6) ]
                        )
                    
                ]
            };

            var unified = SegmentSelector.Union(p1, p2);

            Console.WriteLine(unified);
            if (unified.Regions.Count > 0)
            Console.WriteLine(string.Join(", ", unified.Regions[0].Points));

        }
    }
}
