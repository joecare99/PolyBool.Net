using System.Collections.Generic;

namespace PolyBool.Net.Objects;

public class CombinedPolySegments(IList<ISegment> combined)
{
    public bool IsInverted1 { get; set; }

    public bool IsInverted2 { get; set; }

    public IList<ISegment> Combined { get; set; }= combined;
}