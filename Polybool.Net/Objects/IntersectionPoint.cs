using PolyBool.Net.Interfaces;
using PolyBool.Net.Logic;

namespace PolyBool.Net.Objects
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