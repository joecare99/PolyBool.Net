using System.Collections.Generic;

namespace PolyBool.Net.Objects
{
    public class PolySegments(IList<ISegment> segments)
    {
        public bool IsInverted { get; set; }
        public IList<ISegment> Segments { get; set; }= segments;
    }
}