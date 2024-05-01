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

        static internal IPolygon Val2Poly(this object[] pvals, bool inv = false)
        {
            var result = new List<IRegion>();
            for (int i = 0; i < pvals.Length; i++)
                if (pvals[i] is IList<double> ld)
                {
                    result.Add(Region.New(ld.Val2Pnts()));
                }
            return Polygon.New(result, inv);
        }

        static internal int FIndex(this ISegment segment) =>
            (segment.MyFill?.Above ?? false ? 8 : 0) +
            (segment.MyFill?.Below ?? false ? 4 : 0) +
            (segment.OtherFill != null && (segment.OtherFill.Above ?? false) ? 2 : 0) +
            (segment.OtherFill != null && (segment.OtherFill.Below ?? false) ? 1 : 0);


        static internal PolySegments Val2PolySeg(this IList<double> pvals)
        {
            var result = new List<ISegment>();
            for (int i = 0; i < pvals.Count - 4; i += 5)
            {
                ISegment _seg =
                Segment.New(
                               Point.New((decimal)pvals[i], (decimal)pvals[i + 1]),
                               Point.New((decimal)pvals[i + 2], (decimal)pvals[i + 3])
                                                                         );
                _seg.MyFill = new Fill();
                _seg.MyFill.Above = ((int)pvals[i + 4] & 8) != 0;
                _seg.MyFill.Below = ((int)pvals[i + 4] & 4) != 0;
                if (((int)pvals[i + 4] & 3) != 0)
                {
                    _seg.OtherFill = new Fill();
                    _seg.OtherFill.Above = ((int)pvals[i + 4] & 2) != 0;
                    _seg.OtherFill.Below = ((int)pvals[i + 4] & 1) != 0;
                }
                result.Add(_seg);
            }
            return new PolySegments(result);
        }
    }
}