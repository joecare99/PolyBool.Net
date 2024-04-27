using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects;

public class IntersectionPoint
{
    public IPoint Pt { get; set; }
    public int AlongA { get; set; }
    public int AlongB { get; set; }

    public IntersectionPoint(int alongA, int alongB, IPoint pt)
    {
        AlongA = alongA;
        AlongB = alongB;
        Pt = pt;
    }
}