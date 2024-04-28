using System.Collections.Generic;

namespace PolyBool.Net.Interfaces;

public interface IRegion
{
    IList<IPoint> Points { get; set; }
}