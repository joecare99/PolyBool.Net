using System.Collections.Generic;

namespace PolyBool.Net.Objects
{
    public class CombinedPolySegments
    {
        public bool IsInverted1 { get; set; }

        public bool IsInverted2 { get; set; }

        public List<ISegment> Combined { get; set; }
    }
}