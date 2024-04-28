using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class Polygon : IPolygon
    {
        public Polygon(): this(new List<IRegion>())
        {
        }  

        public Polygon(IList<IRegion> regions, bool isInverted = false)
        {
            Regions = regions;
            Inverted = isInverted;
        }

        public IList<IRegion> Regions { get; set; }
        public bool Inverted { get; set; }

        public static Func<IList<IRegion>,bool,IPolygon> New = (regions, isInverted) => new Polygon(regions, isInverted);
    }
}