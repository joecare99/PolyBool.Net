using System.Collections.Generic;

namespace Polybool.Net.Interfaces;

public interface IPolygon
{
    bool Inverted { get; set; }
    IList<IRegion> Regions { get; set; }
}