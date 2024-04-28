using PolyBool.Net.Interfaces;
using System;

namespace PolyBool.Net.Objects
{
    public class Point(double x, double y) : IPoint, IPoint<double>
    {
        public double X { get; private set; } = x;
        public double Y { get; private set; } = y;

        public IPoint Add(IPoint<double> point)
        { 
            (X,Y) = (X + point.X, Y + point.Y);
            return this;
        }

        public override bool Equals(object? other)
        {
            return other is IPoint<double> point ? X == point.X && Y == point.Y : false;
        }

        public IPoint Multiply(double value)
        {
           (X,Y) = (X * value, Y * value);
            return this;
        }

        public double Multiply(IPoint<double> value)
        {
            return X*value.X+Y*value.Y;
        }

        public IPoint CMultiply(IPoint<double> value)
        {
            (X,Y)=(X*value.X-Y*value.Y,X*value.Y+Y*value.X);
            return this;
        }

        public IPoint Subtract(IPoint<double> point)
        {
            (X,Y) = (X - point.X, Y - point.Y);
            return this;
        }

        public IPoint Clone()
        {
            return New(X,Y);
        }

        public IPoint Normal()
        {
            (X, Y) = (-Y, X);
            return this;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode()
        {
            var yh = Y.GetHashCode();
            return X.GetHashCode() ^ (yh >> 16 ^ yh << 16) ;
        }

        public bool SameX(IPoint<double> value, double epsilon) => Math.Abs(X - value.X) < epsilon;

        public bool SameY(IPoint<double> value, double epsilon) => Math.Abs(Y - value.Y) < epsilon;

        public bool Same(IPoint<double> value, double epsilon) 
            => SameX(value,epsilon) && SameY(value,epsilon);

        IPoint<double> IPoint<double>.Add(IPoint<double> point)=> Add(point);

        IPoint<double> IPoint<double>.Subtract(IPoint<double> point)=> Subtract(point);

        IPoint<double> IPoint<double>.Multiply(double value)=> Multiply(value);

        IPoint<double> IPoint<double>.Clone()=> Clone();

        IPoint<double> IPoint<double>.Normal()=> Normal();

        IPoint<double> IPoint<double>.CMultiply(IPoint<double> value) => CMultiply(value);

        public static Func<double,double,IPoint> New { get; set; } = (x,y) => new Point(x,y);

    }
}