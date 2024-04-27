using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Polybool.Net.Interfaces;
using Polybool.Net.Objects;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Logic
{
    public static class PolyBool
    {
        internal static IList<IRegion> SegmentChainer(IList<ISegment> segments)
        {
            IList<IRegion> regions = new List<IRegion>();
            IList<IList<IPoint>> chains = new List<IList<IPoint>>();

            foreach (ISegment seg in segments)
            {
                IPoint pt1 = seg.Start;
                IPoint pt2 = seg.End;
                if (pt1.Same( pt2,Epsilon.Eps))
                {
                    Debug.WriteLine("PolyBool: Warning: Zero-length segment detected; your epsilon is " +
                        "probably too small or too large");
                    continue;
                }

                // search for two chains that this segment matches
                Matcher firstMatch = new Matcher()
                {
                    Index = 0,
                    MatchesHead = false,
                    MatchesPt1 = false

                };
                Matcher secondMatch = new Matcher()
                {
                    Index = 0,
                    MatchesHead = false,
                    MatchesPt1 = false

                };
                Matcher? nextMatch = firstMatch;

                bool setMatch(int index, bool matchesHead, bool matchesPt1)
                {
                    // return true if we've matched twice
                    if (nextMatch != null)
                    {
                        nextMatch.Value.Set(index, matchesHead, matchesPt1);
                        if (Equals(nextMatch, firstMatch))
                        {
                            nextMatch = secondMatch;
                            return false;
                        }
                        nextMatch = null;
                    }
                    return true; // we've matched twice, we're done here
                }


                for (int i = 0; i < chains.Count; i++)
                {
                    IList<IPoint> chain = chains[i];
                    IPoint head = chain[0];
                    IPoint tail = chain[chain.Count - 1];
                    if (head.Same(pt1,Epsilon.Eps))
                    {
                        if (setMatch(i, true, true))
                        {
                            break;
                        }
                    }
                    else if (head.Same( pt2, Epsilon.Eps))
                    {
                        if (setMatch(i, true, false))
                        {
                            break;
                        }
                    }
                    else if (tail.Same( pt1, Epsilon.Eps))
                    {
                        if (setMatch(i, false, true))
                        {
                            break;
                        }
                    }
                    else if (tail.Same( pt2, Epsilon.Eps))
                    {
                        if (setMatch(i, false, false))
                        {
                            break;
                        }
                    }
                }

                if (Equals(nextMatch, firstMatch))
                {
                    // we didn't match anything, so create a new chain
                    chains.Add(new List<IPoint> { pt1, pt2 });
                    continue;
                }

                if (Equals(nextMatch, secondMatch))
                {
                    // we matched a single chain

                    // add the other point to the apporpriate end, and check to see if we've closed the
                    // chain into a loop

                    int index = firstMatch.Index;
                    IPoint pt = firstMatch.MatchesPt1 ? pt2 : pt1; // if we matched pt1, then we add pt2, etc
                    bool addToHead = firstMatch.MatchesHead; // if we matched at head, then add to the head

                    IList<IPoint> chain = chains[index];
                    IPoint grow = addToHead ? chain[0] : chain[chain.Count - 1];
                    IPoint grow2 = addToHead ? chain[1] : chain[chain.Count - 2];
                    IPoint oppo = addToHead ? chain[chain.Count - 1] : chain[0];
                    IPoint oppo2 = addToHead ? chain[chain.Count - 2] : chain[1];

                    if (grow2.PointsCollinear( grow, pt))
                    {
                        // grow isn't needed because it's directly between grow2 and pt:
                        // grow2 ---grow---> pt
                        if (addToHead)
                        {
                            chain.Shift();
                        }
                        else
                        {
                            chain.Pop();
                        }
                        grow = grow2; // old grow is gone... new grow is what grow2 was
                    }

                    if (oppo.PointsSame( pt))
                    {
                        // we're closing the loop, so remove chain from chains
                        chains.Splice(index, 1);

                        if (oppo2.PointsCollinear( oppo, grow))
                        {
                            // oppo isn't needed because it's directly between oppo2 and grow:
                            // oppo2 ---oppo--->grow
                            if (addToHead)
                            {
                                chain.Pop();
                            }
                            else
                            {
                                chain.Shift();
                            }
                        }

                        // we have a closed chain!
                        regions.Add(Region.New( chain.ToList() ));
                        continue;
                    }

                    // not closing a loop, so just add it to the apporpriate side
                    if (addToHead)
                    {
                        chain.Unshift(pt);
                    }
                    else
                    {
                        chain.Push(pt);
                    }
                    continue;
                }

                // otherwise, we matched two chains, so we need to combine those chains together

                Action<int> reverseChain = (index) =>
                {
                    chains[index].Reverse(); // gee, that's easy
                };

                Action<int, int> appendChain = (index1, index2) =>
                {
                    // index1 gets index2 appended to it, and index2 is removed
                    IList<IPoint> chain1 = chains[index1];
                    IList<IPoint> chain2 = chains[index2];

                    var _seg = Segment.New(chain2.First(), chain1.Last(), null!);

                    IPoint tail2 = chain1[chain1.Count - 2];
                    IPoint head2 = chain2[1];

                    if (_seg.PointIsOn(tail2)) //??
                    {
                        // tail isn't needed because it's directly between tail2 and head
                        // tail2 ---tail---> head
                        chain1.Pop();
                        _seg.End = tail2; // old tail is gone... new tail is what tail2 was
                    }

                    if ( _seg.PointIsOn( head2)) //??
                    {
                        // head isn't needed because it's directly between tail and head2
                        // tail ---head---> head2
                        chain2.Shift();
                    }
                    chains[index1] = chain1.Concat(chain2).ToList();
                    chains.Splice(index2, 1);
                };

                int f = firstMatch.Index;
                int s = secondMatch.Index;


                bool reverseF = chains[f].Count < chains[s].Count; // reverse the shorter chain, if needed
                if (firstMatch.MatchesHead)
                {
                    if (secondMatch.MatchesHead)
                    {
                        if (reverseF)
                        {
                            // <<<< F <<<< --- >>>> S >>>>
                            reverseChain(f);
                            // >>>> F >>>> --- >>>> S >>>>
                            appendChain(f, s);
                        }
                        else
                        {
                            // <<<< F <<<< --- >>>> S >>>>
                            reverseChain(s);
                            // <<<< F <<<< --- <<<< S <<<<   logically same as:
                            // >>>> S >>>> --- >>>> F >>>>
                            appendChain(s, f);
                        }
                    }
                    else
                    {
                        // <<<< F <<<< --- <<<< S <<<<   logically same as:
                        // >>>> S >>>> --- >>>> F >>>>
                        appendChain(s, f);
                    }
                }
                else
                {
                    if (secondMatch.MatchesHead)
                    {
                        // >>>> F >>>> --- >>>> S >>>>
                        appendChain(f, s);
                    }
                    else
                    {
                        if (reverseF)
                        {
                            // >>>> F >>>> --- <<<< S <<<<
                            reverseChain(f);
                            // <<<< F <<<< --- <<<< S <<<<   logically same as:
                            // >>>> S >>>> --- >>>> F >>>>
                            appendChain(s, f);
                        }
                        else
                        {
                            // >>>> F >>>> --- <<<< S <<<<
                            reverseChain(s);
                            // >>>> F >>>> --- >>>> S >>>>
                            appendChain(f, s);
                        }
                    }
                }
            }

            return regions;
        }

        public static PolySegments Segments(IPolygon poly)
        {
            RegionIntersecter i = new RegionIntersecter();
            foreach (IRegion region in poly.Regions)
            {
                i.AddRegion(region);
            }

            return new PolySegments
            {
                Segments = i.Calculate(poly.Inverted),
                IsInverted = poly.Inverted
            };
        }

        public static CombinedPolySegments Combine(PolySegments segments1, PolySegments segments2)
        {
            SegmentIntersecter i = new SegmentIntersecter();
            return new CombinedPolySegments
            {
                Combined = i.Calculate(segments1.Segments, segments1.IsInverted, segments2.Segments, segments2.IsInverted),
                IsInverted1 = segments1.IsInverted,
                IsInverted2 = segments2.IsInverted

            };
        }

        public static IPolygon Polygon(PolySegments polySegments)
        {
            return newPolygon(

                SegmentChainer(polySegments.Segments),
                polySegments.IsInverted
            );
        }

        private static IPolygon newPolygon(IList<IRegion> regions, bool isInverted = false)
        {
            return Objects.Polygon.New(regions, isInverted);
        }
    }
}