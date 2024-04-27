using Polybool.Net.Interfaces;
using PolyBool.Net.Interfaces;
using System;

namespace Polybool.Net.Objects
{
    public class Segment(IPoint start, IPoint end, Fill? fill) : ISegment
    {
        public IPoint Start { get; set; } = start;
        public IPoint End { get; set; } = end;
        public Fill? MyFill { get; set; } = fill;
        public Fill? OtherFill { get; set; }

        public ISegment Clone()
        {
            return New(Start, End, MyFill);
        }

        public double PntHeight(IPoint pt)
        {
            return End.Clone().Subtract(Start).Normal().Multiply(pt.Clone().Subtract(Start));
        }

        public bool PointIsOn(IPoint pt)
        {
          return Math.Abs(PntHeight(pt))<=Epsilon.Eps;
        }

        public bool PointIsOnOrAbove(IPoint pt)
        {
            return Math.Abs(PntHeight(pt)) >= -Epsilon.Eps;
        }

        public static Func<IPoint, IPoint, Fill?, ISegment> New { get; set; } = (start, end, fill) => new Segment(start, end, fill);

        public static ISegment GetNew(IPoint start,IPoint end) => New(start, end, null);
    }
}