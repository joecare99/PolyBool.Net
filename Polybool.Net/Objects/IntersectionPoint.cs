using Polybool.Net.Interfaces;
using Polybool.Net.Logic;

namespace Polybool.Net.Objects
{
    public class IntersectionPoint
    {
        public IPoint Pt { get; set; }
        public EIntersVal AlongA { get; set; }
        public EIntersVal AlongB { get; set; }

        public IntersectionPoint(EIntersVal alongA, EIntersVal alongB, IPoint pt)
        {
            AlongA = alongA;
            AlongB = alongB;
            Pt = pt;
        }
    }
}