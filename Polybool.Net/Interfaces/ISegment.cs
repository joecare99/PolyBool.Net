using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;

namespace PolyBool.Net.Interfaces
{
    public interface ISegment
    {
        IPoint Start { get; set; }
        IPoint End { get; set; }
        Fill MyFill { get; set; }
        Fill? OtherFill { get; set; }

        /// <summary>
        /// the (virtual) 'height' of the point over the segment.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <returns>System.Double.</returns>
        double PntHeight(IPoint pt);
        bool PointIsOn(IPoint pt);
        ISegment Clone();
        bool PointIsOnOrAbove(IPoint pt);
    }
}