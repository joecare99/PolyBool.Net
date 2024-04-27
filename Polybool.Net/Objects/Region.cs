using Polybool.Net.Interfaces;
using System;
using System.Collections.Generic;

namespace Polybool.Net.Objects
{
    public class Region(IList<IPoint> pnts) : IRegion
    {
        public IList<IPoint> Points { get; set; }=pnts;

        public static Func<IList<IPoint>, IRegion> New { get; set; } = (points) => new Region(points);
    }
}