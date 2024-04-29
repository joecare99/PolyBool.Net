using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;
using System.Collections.Generic;
using System.Linq;

namespace PolyBool.Net.Logic.Tests
{
    internal static class PolyBoolTestsHelpers
    {
        static internal IList<IPoint> Val2Pnts(this IList<double> pvals)
        {
            var result = new List<IPoint>();
            for (int i = 0; i < pvals.Count - 1; i += 2)
            {
                result.Add(Point.New((decimal)pvals[i], (decimal)pvals[i + 1]));
            }
            return result;
        }

        static internal IPolygon Val2Poly(this object[] pvals,bool inv=false)
        {
            var result = new List<IRegion>();
            for (int i = 0; i < pvals.Length; i ++)
            if (pvals[i] is IList<double> ld ){
                result.Add(Region.New(ld.Val2Pnts()));
            }
            return Polygon.New( result,inv);
        }

        static internal int FIndex(this ISegment segment) =>
            (segment.MyFill?.Above ?? false ? 8 : 0) +
            (segment.MyFill?.Below ?? false ? 4 : 0) +
            (segment.OtherFill != null && (segment.OtherFill.Above ?? false) ? 2 : 0) +
            (segment.OtherFill != null && (segment.OtherFill.Below ?? false) ? 1 : 0);  

    }
}