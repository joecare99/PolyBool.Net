using System;
using PolyBool.Net.Helper;
using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;

namespace PolyBool.Net.Logic;

public static class PointUtils
{
    public static bool PointAboveOrOnLine(IPoint point, IPoint left, IPoint right)
    {
        return (right.X - left.X) * (point.Y - left.Y) - (right.Y - left.Y) * (point.X - left.X) >= -Epsilon.Eps;
    }

    public static bool PointBetween(IPoint point, IPoint left, IPoint right)
    {
        // p must be collinear with left->right
        // returns false if p == left, p == right, or left == right
        decimal dPxLx = point.X - left.X;
        decimal dPyLy = point.Y - left.Y;
        decimal dRxLx = right.X - left.X;
        decimal dRyLy = right.Y - left.Y;

        decimal dot = dPxLx * dRxLx + dPyLy * dRyLy;

        if (dot < Epsilon.Eps)
        {
            return false;
        }

        decimal sqlen = dRxLx * dRxLx + dRyLy * dRyLy;
        if (dot - sqlen > -Epsilon.Eps)
        {
            return false;
        }

        return true;
    }

    private static bool SameX(this IPoint point1, IPoint point2, decimal eps)
    {
        return Math.Abs(point1.X - point2.X) < eps;
    }

    private static bool SameY(this IPoint point1, IPoint point2, decimal eps)
    {
        return Math.Abs(point1.Y - point2.Y) < eps;
    }

    public static bool SameX<T>(this IPoint<T> point1, IPoint<T> point2, T eps) where T : struct, IComparable
    {
       return point1.X.Sub(point2.X).Abs().CompareTo(eps)<0;
    }

    public static bool SameY<T>(this IPoint<T> point1, IPoint<T> point2, T eps) where T : struct, IComparable
    {
        return point1.Y.Sub(point2.Y).Abs().CompareTo(eps) < 0;
    }

    public static bool Same<T>(this IPoint<T> point1, IPoint<T> point2, T eps) where T : struct, IComparable
    {
        return point1.SameX(point2, eps) && point1.SameY(point2, eps);
    }

    public static bool Same(this IPoint point1, IPoint point2, decimal eps)
    {
        return SameX(point1, point2, eps) && SameY(point1, point2, eps);
    }
    public static bool PointsSame(this IPoint point1, IPoint point2)
    {
        return point1.Same(point2, Epsilon.Eps) ;
    }

    public static int Compare(this IPoint point1, IPoint point2, decimal eps)
    {
        if (SameX(point1, point2, eps))
        {
            return SameY(point1, point2, eps) ? 0 : (point1.Y < point2.Y ? -1 : 1);
        }
        return point1.X < point2.X ? -1 : 1;
    }

    public static int PointsCompare(this IPoint point1, IPoint point2)
    {
        return point1.Compare(point2,Epsilon.Eps);
    }

    public static bool PointsCollinear(IPoint pt1, IPoint pt2, IPoint pt3)
    {
        var dx1 = pt1.X - pt2.X;
        var dy1 = pt1.Y - pt2.Y;
        var dx2 = pt2.X - pt3.X;
        var dy2 = pt2.Y - pt3.Y;
        return Math.Abs(dx1 * dy2 - dx2 * dy1) < Epsilon.Eps;
    }

    public static IPoint<T> Add<T>(this IPoint<T> pt1, IPoint<T> pt2) where T : struct, IConvertible
    {
        if (pt1 is IPoint<decimal> p1 && pt2 is IPoint<decimal> p2)
            (p1.X,p1.Y)=(p1.X + p2.X,p1.Y + p2.Y); 
        return pt1;
    } 

    public static IPoint<T> Add<T>(this IPoint<T> pt1, IPoint pt2) where T : struct, IConvertible
    {
        if (pt2 is IPoint<T> p2)
            pt1.Add(p2); 
        return pt1;
    }
      public static IPoint<T> Subract<T>(this IPoint<T> pt1, IPoint<T> pt2) where T : struct, IConvertible
    {
        if (pt1 is IPoint<decimal> p1 && pt2 is IPoint<decimal> p2)
            (p1.X,p1.Y)=(p1.X - p2.X,p1.Y - p2.Y); 
        return pt1;
    } 

    public static IPoint<T> Subtract<T>(this IPoint<T> pt1, IPoint pt2) where T : struct, IConvertible
    {
        if (pt2 is IPoint<T> p2)
            pt1.Subract(p2); 
        return pt1;
    }

    public static IntersectionPoint? LinesIntersect(IPoint a0, IPoint a1, IPoint b0, IPoint b1)
    {
        decimal adx = a1.X - a0.X;
        decimal ady = a1.Y - a0.Y;
        decimal bdx = b1.X - b0.X;
        decimal bdy = b1.Y - b0.Y;

        decimal axb = adx * bdy - ady * bdx;

        if (Math.Abs(axb) < Epsilon.Eps)
        {
            return null;
        }

        decimal dx = a0.X - b0.X;
        decimal dy = a0.Y - b0.Y;

        decimal a = (bdx * dy - bdy * dx) / axb;
        decimal b = (adx * dy - ady * dx) / axb;

        return new IntersectionPoint(CalcAlongUsingValue(a), CalcAlongUsingValue(b), Point.New(a0.X + a * adx, a0.Y + a * ady));
    }

    private static int CalcAlongUsingValue(decimal value)
    {
        if (value <= -Epsilon.Eps)
        {
            return -2;
        }
        else if (value < Epsilon.Eps)
        {
            return -1;
        }
        else if (value - 1 <= -Epsilon.Eps)
        {
            return 0;
        }
        else if (value - 1 < Epsilon.Eps)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}