using System;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects;

public class Point : Point<decimal>, IPoint
{
    public Point(decimal x, decimal y) : base(x, y)
    {
    }

    public static new Func<decimal, decimal, IPoint> New { get; set; } = (x, y) => new Point(x, y);
}

public class Point<T>(T x, T y) : IPoint<T> where T : struct
{
    public T X { get; set; } = x;
    public T Y { get; set; } = y;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is IPoint<T> p)
        {
            return X.Equals(p.X) && Y.Equals(p.Y);
        }
        return base.Equals(obj);
    }

    public static Func<T, T, IPoint<T>> New { get; set; } = (x, y) => new Point<T>(x, y);
}