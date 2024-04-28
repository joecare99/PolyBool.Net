namespace PolyBool.Net.Interfaces;

public interface IPoint : IPoint<decimal>
{
}

public interface IPoint<T> where T : struct
{
    T X { get; set; }
    T Y { get; set; }

    string ToString();
}