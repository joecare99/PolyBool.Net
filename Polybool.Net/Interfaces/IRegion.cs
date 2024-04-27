using System.Collections.Generic;

namespace Polybool.Net.Interfaces;

public interface IRegion
{
    IList<IPoint> Points { get; set; }
}