﻿using System.Collections.Generic;
using PolyBool.Net.Objects;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Logic;

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

            int forward = PointUtils.PointsCompare(pt1, pt2);
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

    public List<ISegment> Calculate(bool inverted)
    {
        return Calculate(inverted, false);
    }

    public RegionIntersecter() : base(true)
    {
    }
}