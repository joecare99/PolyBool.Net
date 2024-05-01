using System.Collections.Generic;

namespace PolyBool.Net.Objects;

public class CombinedPolySegments : PolySegments
{
    public CombinedPolySegments(IList<ISegment> segments) : base(segments) { }

    public bool IsInverted2 { get; set; }
}