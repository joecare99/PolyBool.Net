using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace Polybool.Net.Objects
{
    public class PolySegments
    {
        public bool IsInverted { get; set; }
        public List<ISegment> Segments { get; set; }
    }
}