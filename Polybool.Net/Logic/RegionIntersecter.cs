using System.Collections.Generic;
using Polybool.Net.Interfaces;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Logic
{
    public class RegionIntersecter : Intersecter
    {
        public void AddRegion(IRegion region)
        {
            // regions are a list of points:
            //  [ [0, 0], [100, 0], [50, 100] ]
            // you can add multiple regions before running calculate
            IPoint pt1;
            IPoint pt2 = region.Points[region.Points.Count - 1];
            for (int i = 0; i < region.Points.Count; i++)
            {
                pt1 = pt2;
                pt2 = region.Points[i];

                int forward = pt1.PointsCompare(pt2);
                if (forward == 0) // points are equal, so we have a zero-length segment
                {
                    continue; // just skip it
                }

                EventAddSegment(
                    SegmentNew(
                        forward < 0 ? pt1 : pt2,
                        forward < 0 ? pt2 : pt1
                    ),
                    true
                );
            }
        }

        internal List<ISegment> Calculate(bool inverted)
        {
            return Calculate(inverted, false);
        }

        public RegionIntersecter() : base(true)
        {
        }
    }

}