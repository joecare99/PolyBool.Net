using System.Collections.Generic;
using Polybool.Net.Objects;

namespace PolyBool.Net.Interfaces
{
    public interface IRegion
    {
        IList<Point> Points { get; set; }
    }
}