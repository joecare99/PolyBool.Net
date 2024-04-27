
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects;

public class Segment
{
    public IPoint End { get; set; }
    public IPoint Start { get; set; }
    public Fill MyFill { get; set; }
    public Fill OtherFill { get; set; }
}