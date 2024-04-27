using System;
using Polybool.Net.Interfaces;
using Polybool.Net.Objects;

namespace Polybool.Net.Logic
{
    public static class PointUtils
    {
        public static bool PointBetween(this IPoint point, IPoint left, IPoint right)
        {
            // p must be collinear with left->right
            // returns false if p == left, p == right, or left == right
            var dPyLy = point.Y - left.Y;
            var dRxLx = right.X - left.X;
            var dPxLx = point.X - left.X;
            var dRyLy = right.Y - left.Y;

            var dot = dPxLx * dRxLx + dPyLy * dRyLy;

            if (dot < Epsilon.Eps)
            {
                return false;
            }

            var sqlen = dRxLx * dRxLx + dRyLy * dRyLy;
            if (dot - sqlen > -Epsilon.Eps)
            {
                return false;
            }

            return true;
        }

        public static bool PointsSame(this IPoint point1, IPoint point2)
        {
            return point1.Same(point2,Epsilon.Eps);
        }

        public static int PointsCompare(this IPoint point1, IPoint point2)
        {
            if (point1.SameX(point2,Epsilon.Eps))
            {
                return point1.SameY(point2, Epsilon.Eps) ? 0 : (point1.Y < point2.Y ? -1 : 1);
            }
            return point1.X < point2.X ? -1 : 1;
        }

        public static bool PointsCollinear(this IPoint pt1, IPoint pt2, IPoint pt3)
        {
            return Math.Abs((pt1.X - pt2.X) * (pt2.Y - pt3.Y) - (pt2.X - pt3.X) * (pt1.Y - pt2.Y)) < Epsilon.Eps;
        }

        public static IntersectionPoint? LinesIntersect(IPoint a0, IPoint a1, IPoint b0, IPoint b1)
        {
            var ad = a0.Clone().Subtract(a1);
            var bd = b0.Clone().Subtract(b1);

            var AxB = ad.Multiply(bd.Clone().Normal());

            if (Math.Abs(AxB) < Epsilon.Eps)
            {
                return null;
            }

            var dr = a0.Clone().Subtract( b0).Normal();

            var a = bd.Multiply(dr) / AxB;
            var b = ad.Multiply(dr) / AxB;

            return new IntersectionPoint(CalcIntersVal(a), CalcIntersVal(b), ad.Clone().Multiply(a).Add(a0) );
        }

        private static EIntersVal CalcIntersVal(double value)
        {
            return value switch
            {
                double d when d <= -Epsilon.Eps => EIntersVal.Before,
                double d when d < Epsilon.Eps => EIntersVal.OnStart,
                double d when d-1d <= -Epsilon.Eps => EIntersVal.Middle,
                double d when d-1d < Epsilon.Eps => EIntersVal.OnEnd,
                _ => EIntersVal.After
            };
        }
    }
}