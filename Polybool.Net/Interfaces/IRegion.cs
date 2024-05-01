using System.Collections.Generic;

namespace PolyBool.Net.Interfaces;

public interface IRegion
{
    IList<IPoint> Points { get; set; }
}

public interface IRegion<T> where T : struct
{
    IList<IPoint<T>> Points { get; set; }
}