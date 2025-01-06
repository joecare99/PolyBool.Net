
using PolyBool.Net.Interfaces;
using PolyBool.Net.Logic;
using System;

namespace PolyBool.Net.Objects;

public class Segment(IPoint start,IPoint end) : ISegment
{
    public IPoint Start { get; set; }= start;
    public IPoint End { get; set; }= end;
    public Fill? MyFill { get; set; }
    public Fill? OtherFill { get; set; }

    public static Func<IPoint, IPoint, ISegment> New { get; set; }= (start, end) => new Segment(start, end);
    IPoint<decimal> ISegment<decimal>.Start { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    IPoint<decimal> ISegment<decimal>.End { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public static ISegment NewF(IPoint start, IPoint end, Fill? fill)
    {
        var segment = New(start, end);
        segment.MyFill = fill;
        return segment;
    }

    public bool PointCollinear(IPoint<decimal> pt, decimal eps) 
        => PointUtils.PointsCollinear((IPoint)pt, Start, End);

    public bool PointBetween(IPoint<decimal> pt, decimal eps) 
        => PointUtils.PointBetween((IPoint)pt, Start, End);

    public bool PointOnOrAbove(IPoint<decimal> pt, decimal eps) 
        => PointUtils.PointAboveOrOnLine((IPoint)pt, Start, End);
}

public class Segment<T>(IPoint<T> start, IPoint<T> end) : ISegment<T> where T : struct
{
    public IPoint<T> Start { get; set; } = start;
    public IPoint<T> End { get; set; } = end;
    public Fill? MyFill { get; set; }
    public Fill? OtherFill { get; set; }

    public static Func<IPoint<T>, IPoint<T>, ISegment<T>> New { get; set; } = (start, end) 
        => new Segment<T>(start, end);
    public static ISegment<T> NewF(IPoint<T> start, IPoint<T> end, Fill? fill)
    {
        var segment = New(start, end);
        segment.MyFill = fill;
        return segment;
    }

    public bool PointCollinear(IPoint<T> pt, T eps)
    {
        throw new NotImplementedException();
    }

    public bool PointBetween(IPoint<T> pt, T eps)
    {
        throw new NotImplementedException();
    }

    public bool PointOnOrAbove(IPoint<T> pt, T eps)
    {
        throw new NotImplementedException();
    }
}