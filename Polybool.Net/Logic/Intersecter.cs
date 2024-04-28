using System;
using System.Collections.Generic;
using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;

namespace PolyBool.Net.Logic;

public class Intersecter
{
    private readonly bool selfIntersection;

    protected Intersecter(bool selfIntersection)
    {
        this.selfIntersection = selfIntersection;
    }

    private readonly LinkedList eventRoot = new LinkedList();

    protected ISegment SegmentNew(IPoint start, IPoint end) 
        => Segment.New(start, end, new Fill());

    protected void EventAddSegment(ISegment segment, bool primary)
    {
        Node evStart = EventAddSegmentStart(segment, primary);
        EventAddSegmentEnd(evStart, segment, primary);
    }

    private void EventAddSegmentEnd(Node evStart, ISegment seg, bool primary)
    {

        Node evEnd = LinkedList.Node(new Node
        {
            IsStart = false,
            Pt = seg.End,
            Seg = seg,
            Primary = primary,
            Other = evStart,
        });
        evStart.Other = evEnd;

        EventAdd(evEnd, evStart.Pt);
    }

    private Node EventAddSegmentStart(ISegment seg, bool primary)
    {
        Node evStart = LinkedList.Node(new Node
        {
            IsStart = true,
            Pt = seg.Start,
            Seg = seg,
            Primary = primary,
        });
        EventAdd(evStart, seg.End);
        return evStart;
    }

    private void EventAdd(Node ev, IPoint otherPt)
    {
        eventRoot.InsertBefore(ev, here =>
        {
            // should ev be inserted before here?
            int comp = EventCompare(
                ev.IsStart, ev.Pt, otherPt,
                here.IsStart, here.Pt, here.Other.Pt
            );
            return comp < 0;
        });
    }

    private int EventCompare(bool p1IsStart, IPoint p11, IPoint p12, bool p2IsStart, IPoint p21, IPoint p22)
    {
        // compare the selected points first
        int comp = p11.PointsCompare(p21);
        if (comp != 0)
        {
            return comp;
        }
        // the selected points are the same

        if (p12.Same(p22,Epsilon.Eps)) // if the non-selected points are the same too...
        {
            return 0; // then the segments are equal
        }

        if (p1IsStart != p2IsStart) // if one is a start and the other isn"t...
        {
            return p1IsStart ? 1 : -1; // favor the one that isn"t the start
        }

        // order matters
        var _seg = p2IsStart ? Segment.New(p21, p22, new Fill()): Segment.New(p22, p21, new Fill());
        // otherwise, we"ll have to calculate which one is below the other manually
        return _seg.PointIsOnOrAbove(p12) ? 1 : -1;
    }

    private int StatusCompare(Node ev1, Node ev2)
    {

        IPoint a1 = ev1.Seg.Start;
        IPoint a2 = ev1.Seg.End;

        if (ev2.Seg.PointIsOn(a1))
        {
            if (ev2.Seg.PointIsOn(a2))
            {
                return 1;
            }
            return ev2.Seg.PointIsOnOrAbove(a2) ? 1 : -1;
        }
        return ev2.Seg.PointIsOnOrAbove(a1) ? 1 : -1;
    }

    private Transition StatusFindSurrounding(LinkedList statusRoot, Node ev)
    {
        return statusRoot.FindTransition((here) =>
        {
            int comp = StatusCompare(ev, here.Ev);
            return comp > 0;
        });
    }

    private void EventUpdateEnd(Node ev, IPoint end)
    {
        // slides an end backwards
        //   (start)------------(end)    to:
        //   (start)---(end)


        ev.Other.Remove();
        ev.Seg.End = end;
        ev.Other.Pt = end;
        EventAdd(ev.Other, ev.Pt);
    }

    private void EventDivide(Node ev, IPoint pt)
    {
        ISegment ns = ev.Seg.Clone();
        ns.Start = pt;
        EventUpdateEnd(ev, pt);
        EventAddSegment(ns, ev.Primary);
    }

    private Node? CheckIntersection(Node ev1, Node ev2)
    {
        // returns the segment equal to ev1, or false if nothing equal

        ISegment seg1 = ev1.Seg;
        ISegment seg2 = ev2.Seg;

        IntersectionPoint? i = PointUtils.LinesIntersect(seg1.Start, seg1.End, seg2.Start, seg2.End);

        if (i == null)
        {
            // segments are parallel or coincident

            // if points aren't collinear, then the segments are parallel, so no intersections
            if (!seg1.PointIsOn(seg2.Start))
            {
                return null;
            }
            // otherwise, segments are on top of each other somehow (aka coincident)


            if (seg1.Start.Same(seg2.End,Epsilon.Eps) || seg1.End.Same(seg2.Start,Epsilon.Eps))
            {
                return null; // segments touch at endpoints... no intersection
            }

            bool a1EquB1 = seg1.Start.Same(seg2.Start, Epsilon.Eps);
            bool a2EquB2 = seg1.End.Same(seg2.End, Epsilon.Eps);

            if (a1EquB1 && a2EquB2)
            {
                return ev2; // segments are exactly equal
            }

            bool a1Between = !a1EquB1 && seg1.Start.PointBetween(seg2.Start, seg2.End);
            bool a2Between = !a2EquB2 && seg1.End.PointBetween(seg2.Start, seg2.End);

            if (a1EquB1)
            {
                if (a2Between)
                {
                    //  (a1)---(a2)
                    //  (b1)----------(b2)
                    EventDivide(ev2, seg1.End);
                }
                else
                {
                    //  (a1)----------(a2)
                    //  (b1)---(b2)
                    EventDivide(ev1, seg2.End);
                }
                return ev2;
            }
            else if (a1Between)
            {
                if (!a2EquB2)
                {
                    // make a2 equal to b2
                    if (a2Between)
                    {
                        //         (a1)---(a2)
                        //  (b1)-----------------(b2)
                        EventDivide(ev2, seg1.End);
                    }
                    else
                    {
                        //         (a1)----------(a2)
                        //  (b1)----------(b2)
                        EventDivide(ev1, seg2.End);
                    }
                }

                //         (a1)---(a2)
                //  (b1)----------(b2)
                EventDivide(ev2, seg1.Start);
            }
        }
        else
        {
            // otherwise, lines intersect at i.pt, which may or may not be between the endpoints

            // is A divided between its endpoints? (exclusive)
            if (i.AlongA == EIntersVal.Middle)
            {
                switch (i.AlongB) // yes, at exactly b1
                {
                    case EIntersVal.OnStart:
                        EventDivide(ev1, seg2.Start);
                        break;
                    case EIntersVal.Middle:
                        EventDivide(ev1, i.Pt);
                        break;
                    case EIntersVal.OnEnd:
                        EventDivide(ev1, seg2.End);
                        break;
                }
            }

            // is B divided between its endpoints? (exclusive)
            if (i.AlongB == EIntersVal.Middle)
            {
                switch (i.AlongA) // yes, at exactly a1
                {
                    case EIntersVal.OnStart:
                        EventDivide(ev2, seg1.Start);
                        break;
                    case EIntersVal.Middle:
                        EventDivide(ev2, i.Pt);
                        break;
                    case EIntersVal.OnEnd:
                        EventDivide(ev2, seg1.End);
                        break;
                }
            }
        }
        return null;
    }

    private Node? CheckBothIntersections(Node? above, Node ev, Node? below)
    {
        if (above != null)
        {
            Node? eve = CheckIntersection(ev, above);
            if (eve != null)
            {
                return eve;
            }
        }
        if (below != null)
        {
            return CheckIntersection(ev, below);
        }
        return null;
    }

    protected List<ISegment> Calculate(bool primaryPolyInverted, bool secondaryPolyInverted)
    {
        // if selfIntersection is true then there is no secondary polygon, so that isn"t used

        //
        // status logic
        //

        LinkedList statusRoot = new LinkedList();

        //
        // main event loop
        //
        List<ISegment> segments = new List<ISegment>();
        while (!eventRoot.IsEmpty())
        {
            Node ev = eventRoot.GetHead();


            if (ev.IsStart)
            {

                Transition surrounding = StatusFindSurrounding(statusRoot, ev);
                Node? above = surrounding.Before != null ? surrounding.Before.Ev : null;
                Node? below = surrounding.After != null ? surrounding.After.Ev : null;

                Node eve = CheckBothIntersections(above, ev, below);
                if (eve != null)
                {
                    // ev and eve are equal
                    // we"ll keep eve and throw away ev

                    // merge ev.seg"s fill information into eve.seg

                    if (selfIntersection)
                    {
                        bool toggle; // are we a toggling edge?
                        if (ev.Seg.MyFill.Below == null)
                        {
                            toggle = true;
                        }
                        else
                        {
                            toggle = ev.Seg.MyFill.Above != ev.Seg.MyFill.Below;
                        }

                        // merge two segments that belong to the same polygon
                        // think of this as sandwiching two segments together, where `eve.seg` is
                        // the bottom -- this will cause the above fill flag to toggle
                        if (toggle)
                        {
                            eve.Seg.MyFill.Above = !eve.Seg.MyFill.Above;
                        }
                    }
                    else
                    {
                        // merge two segments that belong to different polygons
                        // each segment has distinct knowledge, so no special logic is needed
                        // note that this can only happen once per segment in this phase, because we
                        // are guaranteed that all self-intersections are gone
                        eve.Seg.OtherFill = ev.Seg.MyFill;
                    }

                    ev.Other.Remove();
                    ev.Remove();
                }

                if (!Equals(eventRoot.GetHead(), ev))
                {
                    // something was inserted before us in the event queue, so loop back around and
                    // process it before continuing
                    continue;
                }

                //
                // calculate fill flags
                //
                if (selfIntersection)
                {
                    bool toggle; // are we a toggling edge?
                    if (ev.Seg.MyFill.Below == null) // if we are a new segment...
                    {
                        toggle = true; // then we toggle
                    }
                    else // we are a segment that has previous knowledge from a division
                    {
                        toggle = ev.Seg.MyFill.Above != ev.Seg.MyFill.Below; // calculate toggle
                    }

                    // next, calculate whether we are filled below us
                    if (below == null)
                    {
                        // if nothing is below us...
                        // we are filled below us if the polygon is inverted
                        ev.Seg.MyFill.Below = primaryPolyInverted;
                    }
                    else
                    {
                        // otherwise, we know the answer -- it"s the same if whatever is below
                        // us is filled above it
                        ev.Seg.MyFill.Below = below.Seg.MyFill.Above;
                    }

                    // since now we know if we"re filled below us, we can calculate whether
                    // we"re filled above us by applying toggle to whatever is below us
                    if (toggle)
                    {
                        ev.Seg.MyFill.Above= !ev.Seg.MyFill.Below;
                    }
                    else
                    {
                        ev.Seg.MyFill.Above = ev.Seg.MyFill.Below;
                    }
                }
                else
                {
                    // now we fill in any missing transition information, since we are all-knowing
                    // at this point

                    if (ev.Seg.OtherFill == null)
                    {
                        // if we don"t have other information, then we need to figure out if we"re
                        // inside the other polygon
                        bool inside;
                        if (below == null)
                        {
                            // if nothing is below us, then we"re inside if the other polygon is
                            // inverted
                            inside =
                                ev.Primary ? secondaryPolyInverted : primaryPolyInverted;
                        }
                        else
                        {
                            // otherwise, something is below us
                            // so copy the below segment"s other polygon"s above
                            if (ev.Primary == below.Primary)
                            {
                                inside = below.Seg.OtherFill?.Above ?? false;
                            }
                            else
                            {
                                inside = below.Seg.MyFill.Above ?? false;
                            }
                        }
                        ev.Seg.OtherFill = new Fill()
                        {
                            Above = inside,
                            Below = inside
                        };
                    }
                }


                // insert the status and remember it for later removal
                ev.Other.Status = surrounding.Insert(LinkedList.Node(new Node() { Ev = ev }));
            }
            else
            {
                Node st = ev.Status;

                if (st == null)
                {
                    throw new Exception("PolyBool: Zero-length segment detected; your epsilon is " +
                                        "probably too small or too large");
                }

                // removing the status will create two new adjacent edges, so we"ll need to check
                // for those
                if (statusRoot.Exists(st.Previous) && statusRoot.Exists(st.Next))
                {
                    CheckIntersection(st.Previous.Ev, st.Next.Ev);
                }

                // remove the status
                st.Remove();

                // if we"ve reached this point, we"ve calculated everything there is to know, so
                // save the segment for reporting
                if (!ev.Primary)
                {
                    // make sure `seg.myFill` actually points to the primary polygon though
                    Fill s = ev.Seg.MyFill;
                    ev.Seg.MyFill = ev.Seg.OtherFill;
                    ev.Seg.OtherFill = s;
                }
                segments.Add(ev.Seg);
            }

            // remove the event and continue
            eventRoot.GetHead().Remove();
        }

        return segments;
    }
}