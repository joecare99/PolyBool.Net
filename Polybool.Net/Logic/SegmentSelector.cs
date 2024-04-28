using PolyBool.Net.Objects;
using PolyBool.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PolyBool.Net.Logic
{
    [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
    public static class SegmentSelector
    {
        public static IList<ISegment> Union(IList<ISegment> segments)
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

            var union = Union(combinedSegments.Combined);

            foreach (var s in union)
            {
                Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            }

            return Polygon.New(PolyBool.SegmentChainer(union), first.Inverted || second.Inverted);
        }

        public static IList<ISegment> Intersect(IList<ISegment> segments)
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

            var intersection = Intersect(combinedSegments.Combined);

            foreach (var s in intersection)
            {
                Console.WriteLine("{0},{1} -> {2},{3}", s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            }
            return Polygon.New(PolyBool.SegmentChainer(intersection), first.Inverted && second.Inverted);
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

            var difference = Difference(combinedSegments);

            return Polygon.New(PolyBool.SegmentChainer(difference.Segments), difference.IsInverted);
        }
        public static IList<ISegment> DifferenceRev(IList<ISegment> segments)
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

            var difference = DifferenceRev(combinedSegments.Combined);

            return Polygon.New(PolyBool.SegmentChainer(difference), !first.Inverted && second.Inverted);
        }
        public static IList<ISegment> Xor(IList<ISegment> segments)
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

            var xor = Xor(combinedSegments.Combined);

            return Polygon.New(PolyBool.SegmentChainer(xor), first.Inverted != second.Inverted);
        }
        private static IList<ISegment> Select(IList<ISegment> segments, int[] selection)
        {
            IList<ISegment> result = new List<ISegment>();

            foreach (ISegment segment in segments)
            {
                int index = (segment.MyFill?.Above ?? false ? 8 : 0) +
                            (segment.MyFill?.Below ?? false ? 4 : 0) +
                            (segment.OtherFill != null && (segment.OtherFill.Above ?? false) ? 2 : 0) +
                            (segment.OtherFill != null && (segment.OtherFill.Below ?? false) ? 1 : 0);

                if (selection[index] != 0)
                {
                    result.Add(Segment.NewF(segment.Start, segment.End, new Fill
                    {
                        Above = selection[index] == 1,
                        Below = selection[index] == 2
                    }));
                }
            }

            return result;
        }
    }
}