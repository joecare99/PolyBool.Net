using System.Collections.Generic;

namespace PolyBool.Net.Interfaces;

public interface IPolygon
{
    bool Inverted { get; set; }
    IList<IRegion> Regions { get; set; }
}

public interface IPolygon<T> where T : struct
{
    bool Inverted { get; set; }
    IList<IRegion<T>> Regions { get; set; }
}