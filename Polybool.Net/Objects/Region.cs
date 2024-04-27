using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Objects;

public class Region(IList<Point> points) : IRegion
{
    public IList<Point> Points { get; set; }= points;

     public static Func<IList<Point>, IRegion> New { get; set; }= points => new Region(points);
}