using System.Collections.Generic;

namespace PolyBool.Net.Objects
{
    public class PolySegments
    {
        public bool IsInverted { get; set; }
        public List<ISegment> Segments { get; set; }
    }
}