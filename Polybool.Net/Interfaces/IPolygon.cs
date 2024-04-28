using System.Collections.Generic;

namespace PolyBool.Net.Interfaces;

public interface IPolygon
{
    bool Inverted { get; set; }
    IList<IRegion> Regions { get; set; }
}