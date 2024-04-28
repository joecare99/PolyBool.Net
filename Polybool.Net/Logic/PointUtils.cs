using System;
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
        decimal dPyLy = point.Y - left.Y;
        decimal dRxLx = right.X - left.X;
        decimal dPxLx = point.X - left.X;
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

    private static bool PointsSameX(IPoint point1, IPoint point2)
    {
        return Math.Abs(point1.X - point2.X) < Epsilon.Eps;
    }

    private static bool PointsSameY(IPoint point1, IPoint point2)
    {
        return Math.Abs(point1.Y - point2.Y) < Epsilon.Eps;
    }

    public static bool PointsSame(IPoint point1, IPoint point2)
    {
        return PointsSameX(point1, point2) && PointsSameY(point1, point2);
    }

    public static int PointsCompare(IPoint point1, IPoint point2)
    {
        if (PointsSameX(point1, point2))
        {
            return PointsSameY(point1, point2) ? 0 : (point1.Y < point2.Y ? -1 : 1);
        }
        return point1.X < point2.X ? -1 : 1;
    }

    public static bool PointsCollinear(IPoint pt1, IPoint pt2, IPoint pt3)
    {
        var dx1 = pt1.X - pt2.X;
        var dy1 = pt1.Y - pt2.Y;
        var dx2 = pt2.X - pt3.X;
        var dy2 = pt2.Y - pt3.Y;
        return Math.Abs(dx1 * dy2 - dx2 * dy1) < Epsilon.Eps;
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