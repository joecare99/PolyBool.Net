using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class PolySegments
    {
        public bool IsInverted { get; set; }
        public IList<ISegment> Segments { get; set; }
    }
}