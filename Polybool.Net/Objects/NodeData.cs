using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects;

public class NodeData(ISegment? seg,bool isStart)
{
    public bool IsStart { get; set; } = isStart;

    public IPoint? Pt { get; set; } = isStart?seg?.Start:seg?.End;

    public ISegment? Seg { get; set; } = seg;

    public bool Primary { get; set; }

}