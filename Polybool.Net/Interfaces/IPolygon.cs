using System.Collections.Generic;
using Polybool.Net.Objects;

namespace PolyBool.Net.Interfaces;

public interface IPolygon
{
    bool Inverted { get; set; }
    List<Region> Regions { get; set; }
}