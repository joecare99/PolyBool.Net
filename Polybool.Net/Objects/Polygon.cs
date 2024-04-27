using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Objects;
public class Polygon(List<Region> regions, bool isInverted = false) : IPolygon
{

    public Polygon() : this(new List<Region>()) { }

    public List<Region> Regions { get; set; } = regions;
    public bool Inverted { get; set; } = isInverted;

    public static Func<List<Region>, bool, IPolygon> New { get; set; }= (regions, isInverted) => new Polygon(regions, isInverted);
}