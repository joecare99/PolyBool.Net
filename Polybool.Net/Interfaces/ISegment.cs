using PolyBool.Net.Interfaces;
using System.Data.SqlClient;

namespace PolyBool.Net.Objects
{
    public interface ISegment : ISegment<decimal>
    {
        new IPoint Start { get; set; }
        new IPoint End { get; set; }
    }

    public interface ISegment<T> where T : struct
    {
        IPoint<T> Start { get; set; }
        IPoint<T> End { get; set; }
        Fill? MyFill { get; set; }
        Fill? OtherFill { get; set; }

        bool PointCollinear(IPoint<T> pt, T eps);

        bool PointBetween(IPoint<T> pt, T eps);

        bool PointOnOrAbove(IPoint<T> pt, T eps);
    }
}