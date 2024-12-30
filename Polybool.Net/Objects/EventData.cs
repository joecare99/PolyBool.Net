using PolyBool.Net.Interfaces;
using System;

namespace PolyBool.Net.Objects;

public class EventData(ISegment? seg,bool isStart): IComparable<EventData>
{
    // Point associated with the event
    public IPoint? Pt { get; set; } = isStart?seg?.Start:seg?.End;

    // Event associated with the 'other' endpoint of the segment
    public EventData? Other { get; set; }

    public EventData? Status { get; set; }

    public bool IsStart { get; set; } = isStart;

    public ISegment? Seg { get; set; } = seg;

    public bool Primary { get; set; }

    public int CompareTo(EventData? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        if (Pt == null)
        {
            if (other.Pt == null)
            {
                return 0;
            }
            return -1;
        }
        if (other.Pt == null)
        {
            return 1;
        }
        var x = Pt.X.CompareTo(other.Pt.X);
        if (x != 0)
        {
            return x;
        }
        return Pt.Y.CompareTo(other.Pt.Y);
    }
}