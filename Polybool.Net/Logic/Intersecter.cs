﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PolyBool.Net.Objects;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Logic
{
    public class Intersecter
    {
        private readonly bool selfIntersection;

        protected Intersecter(bool selfIntersection)
        {
            this.selfIntersection = selfIntersection;
        }

        private readonly LinkedList<Node<EventData>, EventData> eventRoot = new();

        protected ISegment SegmentNew(IPoint start, IPoint end)
        {
            var result = Segment.New(start, end);
            result.MyFill = new Fill();
            return result;
        }

        protected void EventAddSegment(ISegment segment, bool primary)
        {
            EventData evStart = EventAddSegmentStart(segment, primary);
            EventAddSegmentEnd(evStart, segment, primary);
        }

        private void EventAddSegmentEnd(EventData evStart, ISegment seg, bool primary)
        {

            EventData evEnd = new EventData(seg, false)
            {
                Primary = primary,
                Other = evStart
            };
            evStart.Other = evEnd;

            EventAdd(evEnd, evEnd.Other.Pt);
        }

        private EventData EventAddSegmentStart(ISegment seg, bool primary)
        {
            EventData evStart = new EventData(seg, true)
            {
                Primary = primary,
            };
            EventAdd(evStart, evStart.Seg.End);
            return evStart;
        }

        private void EventAdd(EventData evd, IPoint pt)
        {
            eventRoot.Insert(evd, here =>
            {
                return eventCompare(evd, pt, here) < 0;
            });
        }

        private int eventCompare(EventData ev, IPoint pt, EventData here)
        {
            IPoint p1_1 = ev.Pt!;
            IPoint p1_2 = pt;
            IPoint p2_1 = here.Pt!;
            IPoint p2_2 = here.Other!.Pt!;
            // returns:
            //   -1 if p1 is smaller
            //    0 if equal
            //    1 if p2 is smaller

            // compare the selected points first
            var comp = p1_1.CompareTo(p2_1, Epsilon.Eps);
            if (comp != 0)
                return comp;
            // the selected points are the same

            // if the non-selected points are the same too...
            if (p1_2.CompareTo(p2_2, Epsilon.Eps) == 0)
                return 0; // then the segments are equal

            // if one is a start event and the other isn't...
            if (ev.IsStart != here.IsStart)
            {
                // favor the one that isn't the start
                return ev.IsStart ? 1 : -1;
            }

            // otherwise, we'll have to calculate which one is below the
            // other manually
            return here!.Seg.PointOnOrAbove(p1_2, Epsilon.Eps) ? 1 : -1;
        }

        private int StatusCompare(EventData ev1, EventData ev2)
        {
            if (ev1.Seg is ISegment a && ev2.Seg is ISegment b)
            {
                IPoint a1 = a.Start;
                IPoint a2 = a.End;

                if (b.PointCollinear(a1, Epsilon.Eps))
                {
                    if (b.PointCollinear(a2, Epsilon.Eps))
                    {
                        return 1;
                    }
                    else return b.PointOnOrAbove(a2,Epsilon.Eps) ? 1 : -1;
                }
                else return b.PointOnOrAbove(a1, Epsilon.Eps) ? 1 : -1;
            }
            return 0;
        }

        private Transition<Node<EventData>> StatusFindSurrounding(LinkedList<Node<EventData>, EventData> statusRoot, Node<EventData> ev)
        {
            return statusRoot.FindTransition((here) =>
            {
                int comp = StatusCompare(ev.Data, here.Data);
                return comp > 0;
            });
        }

        private ISegment SegmentCopy(IPoint start, IPoint end, ISegment seg)
        {
            return Segment.NewF(start, end, seg.MyFill != null ? new Fill()
            {
                Above = seg.MyFill.Above,
                Below = seg.MyFill.Below
            } : null);
        }

        private void EventUpdateEnd(EventData evD, IPoint end)
        {
            // slides an end backwards
            //   (start)------------(end)    to:
            //   (start)---(end)

            eventRoot.Remove(evD.Other);
            evD.Seg.End = end;
            evD.Other.Pt = end;
            EventAdd(evD.Other);
        }

        private void EventDivide(EventData evD, IPoint pt)
        {
            ISegment ns = SegmentCopy(pt, evD.Seg.End, evD.Seg);
            EventUpdateEnd(evD, pt);
            EventAddSegment(ns, evD.Primary);
        }

        private EventData? CheckIntersection(EventData ev1, EventData ev2)
        {
            // returns the segment equal to ev1, or false if nothing equal

            ISegment seg1 = ev1.Seg;
            ISegment seg2 = ev2.Seg;
            IPoint a1 = seg1.Start;
            IPoint a2 = seg1.End;
            IPoint b1 = seg2.Start;
            IPoint b2 = seg2.End;


            IntersectionPoint? i = PointUtils.LinesIntersect(a1, a2, b1, b2);

            if (i == null)
            {
                // segments are parallel or coincident

                // if points aren"t collinear, then the segments are parallel, so no intersections
                if (!PointUtils.PointsCollinear(a1, a2, b1))
                {
                    return null;
                }
                // otherwise, segments are on top of each other somehow (aka coincident)

                if (PointUtils.PointsSame(a1, b2) || PointUtils.PointsSame(a2, b1))
                {
                    return null; // segments touch at endpoints... no intersection
                }

                bool a1EquB1 = PointUtils.PointsSame(a1, b1);
                bool a2EquB2 = PointUtils.PointsSame(a2, b2);

                if (a1EquB1 && a2EquB2)
                {
                    return ev2; // segments are exactly equal
                }

                bool a1Between = !a1EquB1 && PointUtils.PointBetween(a1, b1, b2);
                bool a2Between = !a2EquB2 && PointUtils.PointBetween(a2, b1, b2);

                if (a1EquB1)
                {
                    if (a2Between)
                    {
                        //  (a1)---(a2)
                        //  (b1)----------(b2)
                        EventDivide(ev2, a2);
                    }
                    else
                    {
                        //  (a1)----------(a2)
                        //  (b1)---(b2)
                        EventDivide(ev1, b2);
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
                            EventDivide(ev2, a2);
                        }
                        else
                        {
                            //         (a1)----------(a2)
                            //  (b1)----------(b2)
                            EventDivide(ev1, b2);
                        }
                    }

                    //         (a1)---(a2)
                    //  (b1)----------(b2)
                    EventDivide(ev2, a1);
                }
            }
            else
            {
                // otherwise, lines intersect at i.pt, which may or may not be between the endpoints

                // is A divided between its endpoints? (exclusive)
                if (i.AlongA == 0)
                {
                    if (i.AlongB == -1) // yes, at exactly b1
                    {
                        EventDivide(ev1, b1);
                    }
                    else if (i.AlongB == 0) // yes, somewhere between B"s endpoints
                    {
                        EventDivide(ev1, i.Pt);
                    }
                    else if (i.AlongB == 1) // yes, at exactly b2
                    {
                        EventDivide(ev1, b2);
                    }
                }

                // is B divided between its endpoints? (exclusive)
                if (i.AlongB == 0)
                {
                    if (i.AlongA == -1) // yes, at exactly a1
                    {
                        EventDivide(ev2, a1);
                    }
                    else if (i.AlongA == 0) // yes, somewhere between A"s endpoints (exclusive)
                    {
                        EventDivide(ev2, i.Pt);
                    }
                    else if (i.AlongA == 1) // yes, at exactly a2
                    {
                        EventDivide(ev2, a2);
                    }
                }
            }
            return null;
        }

        private EventData? CheckBothIntersections(EventData? above, EventData ev, EventData? below)
        {
            if (above != null)
            {
                EventData? eve = CheckIntersection(ev, above);
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

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        protected List<ISegment> Calculate(bool primaryPolyInverted, bool secondaryPolyInverted)
        {
            // if selfIntersection is true then there is no secondary polygon, so that isn"t used

            //
            // status logic
            //

            LinkedList<Node<EventData>, EventData> statusRoot = new();

            //
            // main event loop
            //
            List<ISegment> segments = new List<ISegment>();
            while (!eventRoot.IsEmpty())
            {
                EventData ev = eventRoot.First();


                if (ev.IsStart)
                {

                    Transition<Node<EventData>> surrounding = StatusFindSurrounding(statusRoot, ev);
                    EventData? above = surrounding.Before.Data != null ? surrounding.Before.Data : null;
                    EventData? below = surrounding.After.Data != null ? surrounding.After.Data : null;


                    EventData? eve = CheckBothIntersections(above, ev, below);
                    if (eve != null)
                    {
                        // ev and eve are equal
                        // we"ll keep eve and throw away ev

                        // merge ev.seg"s fill information into eve.seg

                        if (selfIntersection)
                        {
                            bool toggle; // are we a toggling edge?
                            if (ev.Seg.MyFill?.Below == null)
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
                            if (toggle && eve.Seg.MyFill != null)
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

                        eventRoot.Remove(ev.Other);
                        eventRoot.Remove(ev);
                    }

                    if (!Equals(eventRoot.First(), ev))
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
                        if (ev.Seg.MyFill?.Below == null) // if we are a new segment...
                        {
                            toggle = true; // then we toggle
                        }
                        else // we are a segment that has previous knowledge from a division
                        {
                            toggle = ev.Seg.MyFill.Above != ev.Seg.MyFill.Below; // calculate toggle
                        }

                        // next, calculate whether we are filled below us
                        if (below == null && ev.Seg.MyFill != null)
                        {
                            // if nothing is below us...
                            // we are filled below us if the polygon is inverted
                            ev.Seg.MyFill.Below = primaryPolyInverted;
                        }
                        else if (below != null && ev.Seg.MyFill != null)
                        {
                            // otherwise, we know the answer -- it"s the same if whatever is below
                            // us is filled above it
                            ev.Seg.MyFill.Below = below.Seg.MyFill?.Above;
                        }

                        // since now we know if we"re filled below us, we can calculate whether
                        // we"re filled above us by applying toggle to whatever is below us
                        if (toggle && ev.Seg.MyFill != null)
                        {
                            ev.Seg.MyFill.Above = !ev.Seg.MyFill.Below;
                        }
                        else if (!toggle && ev.Seg.MyFill != null)
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
                            bool? inside;
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
                                    inside = below.Seg.OtherFill?.Above;
                                }
                                else
                                {
                                    inside = below.Seg.MyFill?.Above;
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
                    ev.Other.Status = surrounding.Insert(new Node<EventData>(new EventData(null, false), n => n.Ev = ev));
                }
                else
                {
                    EventData st = ev.Status;

                    if (st == null)
                    {
                        throw new Exception("PolyBool: Zero-length segment detected; your epsilon is " +
                                            "probably too small or too large");
                    }

                    // removing the status will create two new adjacent edges, so we"ll need to check
                    // for those
                    if (statusRoot.Exists(st.Previous) && statusRoot.Exists(st.Next))
                    {
                        CheckIntersection(st.Previous.Data, st.Next.Data);
                    }


                    // remove the status
                    st.Remove();

                    // if we"ve reached this point, we"ve calculated everything there is to know, so
                    // save the segment for reporting
                    if (!ev.Primary)
                    {
                        // make sure `seg.myFill` actually points to the primary polygon though
                        Fill? s = ev.Seg.MyFill;
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
}