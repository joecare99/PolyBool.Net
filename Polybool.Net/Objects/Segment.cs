
using PolyBool.Net.Interfaces;
using System;

namespace PolyBool.Net.Objects;

public class Segment(IPoint start,IPoint end) : ISegment
{
    public IPoint Start { get; set; }= start;
    public IPoint End { get; set; }= end;
    public Fill? MyFill { get; set; }
    public Fill? OtherFill { get; set; }

    public static Func<IPoint, IPoint, ISegment> New { get; set; }= (start, end) => new Segment(start, end);
    public static ISegment NewF(IPoint start, IPoint end, Fill? fill)
    {
        var segment = New(start, end);
        segment.MyFill = fill;
        return segment;
    }
}