using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class CombinedPolySegments
    {
        public bool IsInverted1 { get; set; }

        public bool IsInverted2 { get; set; }

        public IList<ISegment> Combined { get; set; }
    }
}