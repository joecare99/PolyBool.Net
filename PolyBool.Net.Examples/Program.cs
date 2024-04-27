using System;
using System.Collections.Generic;
using Polybool.Net.Logic;
using Polybool.Net.Objects;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Examples;

internal class Program
{
    private static void Main()
    {

        var p1 = Polygon.New([
            new Region {
                Points = [
                    new Point(0, 0),
                    new Point(16, 0),
                    new Point(8, 8)
                ]
            }], false);
        var p2 = Polygon.New([
            new Region {
                Points = [
                    new Point(16, 6),
                    new Point(8, 14),
                    new Point(0, 6),
                ]
            }], false);

        var unified = SegmentSelector.Union(p1, p2);

        Console.WriteLine(unified);
        Console.WriteLine(unified.Regions.Count);
        foreach (var region in unified.Regions)
        {
            Console.WriteLine(region);
            Console.WriteLine(region.Points.Count);
            foreach (var point in region.Points)
            {
                Console.WriteLine(point);
            }
        }

    }
}
