using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public interface ISegment
    {
        IPoint Start { get; set; }
        IPoint End { get; set; }
        Fill? MyFill { get; set; }
        Fill? OtherFill { get; set; }
    }
}