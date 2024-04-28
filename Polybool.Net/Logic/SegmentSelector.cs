using PolyBool.Net.Objects;
using PolyBool.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Logic
{
    [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
    public static class SegmentSelector
    {
        public static List<ISegment> Union(List<ISegment> segments)
        {
            return Select(segments, new[] {
                0, 2, 1, 0,
                2, 2, 0, 0,
                1, 0, 1, 0,
                0, 0, 0, 0
            });
        }

        public static IPolygon Union(IPolygon first, IPolygon second)
        {
            var firstPolygonRegions = PolyBool.Segments(first);
            var secondPolygonRegions = PolyBool.Segments(second);
            var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

            var union = Select(combinedSegments.Combined, new[] {
                0, 2, 1, 0,
                2, 2, 0, 0,
                1, 0, 1, 0,
                0, 0, 0, 0
            });

            foreach (var s in union)
            {
                Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            }

            return newPolygon(PolyBool.SegmentChainer(union), first.Inverted || second.Inverted);
        }

        private static IPolygon newPolygon(IList<IRegion> regions, bool v)
        {
            return Polygon.New(regions, v);
        }

        public static List<ISegment> Intersect(List<ISegment> segments)
        {
            return Select(segments, new[] {   0, 0, 0, 0,
                0, 2, 0, 2,
                0, 0, 1, 1,
                0, 2, 1, 0
            });
        }

        public static IPolygon Intersect(IPolygon first, IPolygon second)
        {
            var firstPolygonRegions = PolyBool.Segments(first);
            var secondPolygonRegions = PolyBool.Segments(second);
            var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

            var intersection = Select(combinedSegments.Combined, new[] { 0, 0, 0, 0,
                0, 2, 0, 2,
                0, 0, 1, 1,
                0, 2, 1, 0
            });


            foreach (var s in intersection)
            {
                Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            }
            return newPolygon(PolyBool.SegmentChainer(intersection), first.Inverted && second.Inverted);
        }

        public static PolySegments Difference(CombinedPolySegments combined)
        {
            return new PolySegments
            {
                Segments = Select(combined.Combined, new[]
                {
                    0, 0, 0, 0,
                    2, 0, 2, 0,
                    1, 1, 0, 0,
                    0, 1, 2, 0
                }),
                IsInverted = !combined.IsInverted1 && combined.IsInverted2
            };
        }
        public static IPolygon Difference(IPolygon first, IPolygon second)
        {
            var firstPolygonRegions = PolyBool.Segments(first);
            var secondPolygonRegions = PolyBool.Segments(second);
            var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

            var difference = Select(combinedSegments.Combined, new[]
                {
                    0, 0, 0, 0,
                    2, 0, 2, 0,
                    1, 1, 0, 0,
                    0, 1, 2, 0
                });

            return newPolygon(PolyBool.SegmentChainer(difference), first.Inverted && !second.Inverted);
        }
        public static List<ISegment> DifferenceRev(List<ISegment> segments)
        {
            return Select(segments, new[] {   0, 2, 1, 0,
                0, 0, 1, 1,
                0, 2, 0, 2,
                0, 0, 0, 0
            });
        }

        public static IPolygon DifferenceRev(IPolygon first, IPolygon second)
        {
            var firstPolygonRegions = PolyBool.Segments(first);
            var secondPolygonRegions = PolyBool.Segments(second);
            var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

            var difference = Select(combinedSegments.Combined, new[] {   0, 2, 1, 0,
                0, 0, 1, 1,
                0, 2, 0, 2,
                0, 0, 0, 0
            });

            return newPolygon(PolyBool.SegmentChainer(difference), !first.Inverted && second.Inverted);
        }
        public static List<ISegment> Xor(List<ISegment> segments)
        {
            return Select(segments, new[] {   0, 2, 1, 0,
                2, 0, 0, 1,
                1, 0, 0, 2,
                0, 1, 2, 0
            });
        }
        public static IPolygon Xor(IPolygon first, IPolygon second)
        {
            var firstPolygonRegions = PolyBool.Segments(first);
            var secondPolygonRegions = PolyBool.Segments(second);
            var combinedSegments = PolyBool.Combine(firstPolygonRegions, secondPolygonRegions);

            var xor = Select(combinedSegments.Combined, new[] {   0, 2, 1, 0,
                2, 0, 0, 1,
                1, 0, 0, 2,
                0, 1, 2, 0
            });

            return newPolygon(PolyBool.SegmentChainer(xor), first.Inverted != second.Inverted);
        }
        private static List<ISegment> Select(List<ISegment> segments, int[] selection)
        {
            List<ISegment> result = new List<ISegment>();

            foreach (ISegment segment in segments)
            {
                int index = (segment.MyFill.Above ?? false ? 8 : 0) +
                            (segment.MyFill.Below ?? true ? 4 : 0) +
                            (segment.OtherFill != null && (segment.OtherFill.Above ?? false) ? 2 : 0) +
                            (segment.OtherFill != null && (segment.OtherFill.Below ?? true) ? 1 : 0);

                if (selection[index] != 0)
                {
                    var _seg=segment.Clone();
                    _seg.MyFill = new Fill
                    {
                        Above = selection[index] == 1,
                        Below = selection[index] == 2
                    };
                    result.Add(_seg);
                }
            }

            return result;
        }
    }
}