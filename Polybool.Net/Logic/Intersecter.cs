using System;
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

        private readonly LinkedList<Node<NodeData>,NodeData> eventRoot = new();

        protected ISegment SegmentNew(IPoint start, IPoint end)
        {
            var result = Segment.New(start, end);
            result.MyFill = new Fill();
            return result;
        }

        protected void EventAddSegment(ISegment segment, bool primary)
        {
            Node<NodeData> evStart = EventAddSegmentStart(segment, primary);
            EventAddSegmentEnd(evStart, segment, primary);
        }

        private void EventAddSegmentEnd(Node<NodeData> evStart, ISegment seg, bool primary)
        {

            Node<NodeData> evEnd = new Node<NodeData>(new NodeData(seg,false)
            {
                Primary = primary,
            },n=>n.Other= evStart);
            evStart.Other = evEnd;

            EventAdd(evEnd, evStart.Data.Pt);
        }

        private Node<NodeData> EventAddSegmentStart(ISegment seg, bool primary)
        {
            Node<NodeData> evStart = new Node<NodeData>(new NodeData(seg,true)
            {
                Primary = primary,
            });
            EventAdd(evStart, seg.End);
            return evStart;
        }

        private void EventAdd(Node<NodeData> ev, IPoint otherPt)
        {
            eventRoot.InsertBefore(ev, here =>
            {
                // should ev be inserted before here?
                int comp = EventCompare(
                    ev.Data.IsStart, ev.Data.Pt, otherPt,
                    here.Data.IsStart, here.Data.Pt, here.Other.Data.Pt
                );
                return comp < 0;
            });
        }

        private int EventCompare(bool p1IsStart, IPoint p11, IPoint p12, bool p2IsStart, IPoint p21, IPoint p22)
        {
            // compare the selected points first
            int comp = PointUtils.PointsCompare(p11, p21);
            if (comp != 0)
            {
                return comp;
            }
            // the selected points are the same

            if (PointUtils.PointsSame(p12, p22)) // if the non-selected points are the same too...
            {
                return 0; // then the segments are equal
            }

            if (p1IsStart != p2IsStart) // if one is a start and the other isn"t...
            {
                return p1IsStart ? 1 : -1; // favor the one that isn"t the start
            }

            // otherwise, we"ll have to calculate which one is below the other manually
            return PointUtils.PointAboveOrOnLine(p12,
                p2IsStart ? p21 : p22, // order matters
                p2IsStart ? p22 : p21
            )
                ? 1
                : -1;
        }

        private int StatusCompare(NodeData ev1, NodeData ev2)
        {
            IPoint a1 = ev1.Seg.Start;
            IPoint a2 = ev1.Seg.End;
            IPoint b1 = ev2.Seg.Start;
            IPoint b2 = ev2.Seg.End;

            if (PointUtils.PointsCollinear(a1, b1, b2))
            {
                if (PointUtils.PointsCollinear(a2, b1, b2))
                {
                    return 1;
                }
                return PointUtils.PointAboveOrOnLine(a2, b1, b2) ? 1 : -1;
            }
            return PointUtils.PointAboveOrOnLine(a1, b1, b2) ? 1 : -1;
        }

        private Transition<Node<NodeData>> StatusFindSurrounding(LinkedList<Node<NodeData>,NodeData> statusRoot, Node<NodeData> ev)
        {
            return statusRoot.FindTransition((here) =>
            {
                int comp = StatusCompare(ev.Data, here.Ev.Data);
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

        private void EventUpdateEnd(Node<NodeData> ev, IPoint end)
        {
            // slides an end backwards
            //   (start)------------(end)    to:
            //   (start)---(end)


            ev.Other.Remove();
            ev.Data.Seg.End = end;
            ev.Other.Data.Pt = end;
            EventAdd(ev.Other, ev.Data.Pt);
        }

        private void EventDivide(Node<NodeData> ev, IPoint pt)
        {
            ISegment ns = SegmentCopy(pt, ev.Data.Seg.End, ev.Data.Seg);
            EventUpdateEnd(ev, pt);
            EventAddSegment(ns, ev.Data.Primary);
        }

        private Node<NodeData>? CheckIntersection(Node<NodeData> ev1, Node<NodeData> ev2)
        {
            // returns the segment equal to ev1, or false if nothing equal

            ISegment seg1 = ev1.Data.Seg;
            ISegment seg2 = ev2.Data.Seg;
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

        private Node<NodeData>? CheckBothIntersections(Node<NodeData>? above, Node<NodeData> ev, Node<NodeData>? below)
        {
            if (above != null)
            {
                Node<NodeData>? eve = CheckIntersection(ev, above);
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

            LinkedList<Node<NodeData>,NodeData> statusRoot = new ();



            //
            // main event loop
            //
            List<ISegment> segments = new List<ISegment>();
            while (!eventRoot.IsEmpty())
            {
                Node<NodeData> ev = eventRoot.GetHead();


                if (ev.Data.IsStart)
                {


                    Transition<Node<NodeData>> surrounding = StatusFindSurrounding(statusRoot, ev);
                    Node<NodeData>? above = surrounding.Before != null ? surrounding.Before.Ev : null;
                    Node<NodeData>? below = surrounding.After != null ? surrounding.After.Ev : null;


                    Node<NodeData>? eve = CheckBothIntersections(above, ev, below);
                    if (eve != null)
                    {
                        // ev and eve are equal
                        // we"ll keep eve and throw away ev

                        // merge ev.seg"s fill information into eve.seg

                        if (selfIntersection)
                        {
                            bool toggle; // are we a toggling edge?
                            if (ev.Data.Seg.MyFill?.Below == null)
                            {
                                toggle = true;
                            }
                            else
                            {
                                toggle = ev.Data.Seg.MyFill.Above != ev.Data.Seg.MyFill.Below;
                            }

                            // merge two segments that belong to the same polygon
                            // think of this as sandwiching two segments together, where `eve.seg` is
                            // the bottom -- this will cause the above fill flag to toggle
                            if (toggle && eve.Data.Seg.MyFill!=null)
                            {
                                eve.Data.Seg.MyFill.Above = !eve.Data.Seg.MyFill.Above;
                            }
                        }
                        else
                        {
                            // merge two segments that belong to different polygons
                            // each segment has distinct knowledge, so no special logic is needed
                            // note that this can only happen once per segment in this phase, because we
                            // are guaranteed that all self-intersections are gone
                            eve.Data.Seg.OtherFill = ev.Data.Seg.MyFill;
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
                        if (ev.Data.Seg.MyFill?.Below == null) // if we are a new segment...
                        {
                            toggle = true; // then we toggle
                        }
                        else // we are a segment that has previous knowledge from a division
                        {
                            toggle = ev.Data.Seg.MyFill.Above != ev.Data.Seg.MyFill.Below; // calculate toggle
                        }

                        // next, calculate whether we are filled below us
                        if (below == null && ev.Data.Seg.MyFill!= null)
                        {
                            // if nothing is below us...
                            // we are filled below us if the polygon is inverted
                            ev.Data.Seg.MyFill.Below = primaryPolyInverted;
                        }
                        else if (below != null && ev.Data.Seg.MyFill != null)
                        {
                            // otherwise, we know the answer -- it"s the same if whatever is below
                            // us is filled above it
                            ev.Data.Seg.MyFill.Below = below.Data.Seg.MyFill?.Above;
                        }

                        // since now we know if we"re filled below us, we can calculate whether
                        // we"re filled above us by applying toggle to whatever is below us
                        if (toggle && ev.Data.Seg.MyFill != null)
                        {
                            ev.Data.Seg.MyFill.Above = !ev.Data.Seg.MyFill.Below;
                        }
                        else if (!toggle && ev.Data.Seg.MyFill != null)
                        {
                            ev.Data.Seg.MyFill.Above = ev.Data.Seg.MyFill.Below;
                        }
                    }
                    else
                    {
                        // now we fill in any missing transition information, since we are all-knowing
                        // at this point

                        if (ev.Data.Seg.OtherFill == null)
                        {
                            // if we don"t have other information, then we need to figure out if we"re
                            // inside the other polygon
                            bool? inside;
                            if (below == null)
                            {
                                // if nothing is below us, then we"re inside if the other polygon is
                                // inverted
                                inside =
                                    ev.Data.Primary ? secondaryPolyInverted : primaryPolyInverted;
                            }
                            else
                            {
                                // otherwise, something is below us
                                // so copy the below segment"s other polygon"s above
                                if (ev.Data.Primary == below.Data.Primary)
                                {
                                    inside = below.Data.Seg.OtherFill?.Above;
                                }
                                else
                                {
                                    inside = below.Data.Seg.MyFill?.Above;
                                }
                            }
                            ev.Data.Seg.OtherFill = new Fill()
                            {
                                Above = inside,
                                Below = inside
                            };
                        }
                    }


                    // insert the status and remember it for later removal
                    ev.Other.Status = surrounding.Insert(new Node<NodeData>(new NodeData(null,false),n=>  n.Ev = ev ));
                }
                else
                {
                    Node<NodeData> st = ev.Status;

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
                    if (!ev.Data.Primary)
                    {
                        // make sure `seg.myFill` actually points to the primary polygon though
                        Fill? s = ev.Data.Seg.MyFill;
                        ev.Data.Seg.MyFill = ev.Data.Seg.OtherFill;
                        ev.Data.Seg.OtherFill = s;
                    }
                    segments.Add(ev.Data.Seg);
                }

                // remove the event and continue
                eventRoot.GetHead().Remove();
            }



            return segments;
        }
    }
}