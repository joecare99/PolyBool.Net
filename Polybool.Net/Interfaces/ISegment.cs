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

    public interface ISegment<T> where T : struct
    {
        IPoint<T> Start { get; set; }
        IPoint<T> End { get; set; }
        Fill? MyFill { get; set; }
        Fill? OtherFill { get; set; }
    }
}