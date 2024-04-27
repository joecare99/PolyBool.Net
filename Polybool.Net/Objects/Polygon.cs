using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Objects;
public class Polygon(IList<IRegion> regions, bool isInverted = false) : IPolygon
{

    public Polygon() : this(new List<IRegion>()) { }

    public IList<IRegion> Regions { get; set; } = regions;
    public bool Inverted { get; set; } = isInverted;

    public static Func<IList<IRegion>, bool, IPolygon> New { get; set; }= (regions, isInverted) => new Polygon(regions, isInverted);
}