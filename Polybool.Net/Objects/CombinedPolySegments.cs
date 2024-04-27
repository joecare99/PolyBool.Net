using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Objects
{
    public class CombinedPolySegments
    {
        public bool IsInverted1 { get; set; }

        public bool IsInverted2 { get; set; }

        public List<ISegment> Combined { get; set; }
    }
}