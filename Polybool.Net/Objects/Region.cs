using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects;

public class Region(IList<IPoint> points) : IRegion
{
    public IList<IPoint> Points { get; set; }= points;

     public static Func<IList<IPoint>, IRegion> New { get; set; }= points => new Region(points);
}