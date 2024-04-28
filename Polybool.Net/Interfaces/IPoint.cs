namespace PolyBool.Net.Interfaces
{
    public interface IPoint: IPoint<double>
    {
        new IPoint Normal();
        new IPoint Clone();
        new IPoint Subtract(IPoint<double> point);
        new IPoint Multiply(double value);
        new IPoint Add(IPoint<double> point);
        new IPoint CMultiply(IPoint<double> value);
    }
    public interface IPoint<T> where T : struct
    {
        T X { get; }
        T Y { get; }

        bool Equals(object other);

        IPoint<T> Normal();
        IPoint<T> Add(IPoint<T> point);
        IPoint<T> Subtract(IPoint<T> point);
        IPoint<T> Multiply(T value);
        IPoint<T> CMultiply(IPoint<double> value);
        T Multiply(IPoint<T> value);
        IPoint<T> Clone();
        bool SameX(IPoint<T> value,T Epsilon);
        bool SameY(IPoint<T> value,T Epsilon);
        bool Same(IPoint<T> value,T Epsilon);
    }
}